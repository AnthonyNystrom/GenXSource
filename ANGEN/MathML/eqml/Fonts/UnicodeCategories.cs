using Facade;

namespace Fonts
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public delegate void UnknownUnicodeCategory(object sender, UnknownUnicodeCategoryArgs e);

    public class UnknownUnicodeCategoryArgs : EventArgs
    {
        public UnknownUnicodeCategoryArgs(string msg)
        {
            this.message = msg;
        }


        public string message;
    }

    public class UnicodeCategories
    {
        public event UnknownUnicodeCategory BadUniCat;

        public UnicodeCategories(string path)
        {
            this.source_ = "UnicodeCategories.xml";
            this.list_ = null;

            try
            {
                this.list_ = new ArrayList(40);
                this.Parse(path, Source);
            }
            catch
            {
            }
        }

        public UnicodeCategory Indexer(string sUnicode)
        {
            UnicodeCategory category = null;
            try
            {
                bool found = false;
                for (int i = 0; (i < this.Count) && !found; i++)
                {
                    UnicodeCategory c = (UnicodeCategory) this.list_[i];
                    int cv = this.FromHex(sUnicode);
                    if (((c != null) && (cv >= c.MinValue)) && (cv <= c.MaxValue))
                    {
                        category = c;
                        found = true;
                    }
                }
            }
            catch (Exception e)
            {
                this.BadUniCat(this, new UnknownUnicodeCategoryArgs(e.Message));
            }
            return category;
        }

        public UnicodeCategory Indexer(int index)
        {
            UnicodeCategory category = null;
            try
            {
                if (index < this.Count)
                {
                    category = (UnicodeCategory) this.list_[index];
                }
            }
            catch (Exception e)
            {
                this.BadUniCat(this, new UnknownUnicodeCategoryArgs(e.Message));
            }
            return category;
        }

        private void Parse(string file)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(file);
                this.Parse(doc);
            }
            catch
            {
            }
        }

        private void Parse(XmlDocument xmlDoc)
        {
            try
            {
                XmlNode root = xmlDoc.DocumentElement;
                if (!root.HasChildNodes)
                {
                    return;
                }
                int count = 0;
                count = root.ChildNodes.Count;
                for (int i = 0; i < count; i++)
                {
                    XmlNode child = root.ChildNodes[i];
                    if (child != null)
                    {
                        string id = "";
                        string name = "";
                        string description = "";
                        string min = "";
                        string max = "";
                        string sType = "";
                        XmlNode node = child.Attributes.GetNamedItem("name", "");
                        if (node != null)
                        {
                            name = node.Value;
                        }
                        node = child.Attributes.GetNamedItem("index", "");
                        if (node != null)
                        {
                            id = node.Value;
                        }
                        node = child.Attributes.GetNamedItem("dscription", "");
                        if (node != null)
                        {
                            description = node.Value;
                        }
                        node = child.Attributes.GetNamedItem("min", "");
                        if (node != null)
                        {
                            min = node.Value;
                        }
                        node = child.Attributes.GetNamedItem("max", "");
                        if (node != null)
                        {
                            max = node.Value;
                        }
                        node = child.Attributes.GetNamedItem("type", "");
                        if (node != null)
                        {
                            sType = node.Value;
                        }
                        UnicodeCategory category = new UnicodeCategory(id, name, sType, description, min, max);
                        this.list_.Add(category);
                    }
                }
            }
            catch
            {
            }
        }

        private void Parse(string appNamespace, string resourceName)
        {
            try
            {
                Stream stream = ResourceLoader.GetStream(appNamespace, resourceName);
                if (stream != null)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(stream);
                    stream.Close();
                    this.Parse(doc);
                }
            }
            catch
            {
            }
        }

        private int FromHex(string hex)
        {
            int r = 0;
            try
            {
                string s = hex;
                while ((s.Length > 1) && (s.Substring(0, 1) == "0"))
                {
                    s = s.Substring(1, s.Length - 1);
                }
                if (s.Length <= 4)
                {
                    s = s.PadLeft(4, '0');
                    return int.Parse(s, NumberStyles.HexNumber);
                }
                if ((s.Length == 5) && (s.Substring(0, 1) == "1"))
                {
                    s = s.Substring(s.Length - 4, 4);
                    r = 0x10000 + int.Parse(s, NumberStyles.HexNumber);
                }
            }
            catch
            {
            }
            return r;
        }


        private string Source
        {
            get
            {
                return this.source_;
            }
        }

        public int Count
        {
            get
            {
                int r = 0;
                try
                {
                    if (this.list_ == null)
                    {
                        return r;
                    }
                    return this.list_.Count;
                }
                catch
                {
                }
                return r;
            }
        }

        private string source_;
        private ArrayList list_;
    }
}

