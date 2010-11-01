using Facade;

namespace Fonts
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Xml;

    public class FontTable
    {
        public FontTable()
        {
            this.name_ = "";
            this.fontFamily_ = "";
            this.filename_ = "";
            this.corX_ = 0;
            this.corY_ = 0;
            this.corH_ = 1;
            this.corW_ = 0;
            this.corB_ = 0;
            this.list_ = new ArrayList();
            this.hash_ = new Hashtable();
        }

        public FontTable(string path, string fileName)
        {
            this.name_ = "";
            this.fontFamily_ = "";
            this.filename_ = "";
            this.corX_ = 0;
            this.corY_ = 0;
            this.corH_ = 1;
            this.corW_ = 0;
            this.corB_ = 0;
            this.list_ = new ArrayList();
            this.hash_ = new Hashtable();
            this.Parse(path, fileName);
        }

        public Symbol ByCode(object key)
        {
            Symbol symbol = null;
            try
            {
                object o = this.hash_[key];
                if (o != null)
                {
                    symbol = (Symbol) o;
                }
            }
            catch (Exception)
            {
            }
            return symbol;
        }

        public void Clear()
        {
            try
            {
                this.list_.Clear();
                this.list_ = null;
                this.hash_.Clear();
                this.hash_.Clear();
            }
            catch
            {
            }
        }

        public void Parse(string path, string fileName)
        {
            this.list_ = new ArrayList(100);
            this.hash_ = new Hashtable(100, 0.5f);
            try
            {
                XmlDocument doc = new XmlDocument();
                this.Filename = fileName;

                Stream stream = null;
                    stream = ResourceLoader.GetStream(path, fileName);
                    if (stream == null)
                    {
                        return;
                    }
                    doc.Load(stream);
                    stream.Close();
                
                
                XmlNode root = doc.DocumentElement;
                XmlNode nameNode = root.Attributes.GetNamedItem("name");
                if (nameNode != null)
                {
                    this.Name = nameNode.Value;
                }
                XmlNode ffNode = root.Attributes.GetNamedItem("font-family");
                if (ffNode != null)
                {
                    this.FontFamily = ffNode.Value;
                }
                XmlNode n = null;
                n = root.Attributes.GetNamedItem("correctX");
                if (n != null)
                {
                    try
                    {
                        this.CorrectX = Convert.ToInt32(n.Value);
                    }
                    catch
                    {
                    }
                }
                n = null;
                n = root.Attributes.GetNamedItem("correctW");
                if (n != null)
                {
                    try
                    {
                        this.CorrectW = Convert.ToInt32(n.Value);
                    }
                    catch
                    {
                    }
                }
                n = null;
                n = root.Attributes.GetNamedItem("correctY");
                if (n != null)
                {
                    try
                    {
                        this.CorrectY = Convert.ToInt32(n.Value);
                    }
                    catch
                    {
                    }
                }
                n = null;
                n = root.Attributes.GetNamedItem("correctH");
                if (n != null)
                {
                    try
                    {
                        this.CorrectH = Convert.ToInt32(n.Value);
                    }
                    catch
                    {
                    }
                }
                n = null;
                n = root.Attributes.GetNamedItem("correctB");
                if (n != null)
                {
                    try
                    {
                        this.CorrectB = Convert.ToInt32(n.Value);
                    }
                    catch
                    {
                    }
                }
                for (int i = 0; i < root.ChildNodes.Count; i++)
                {
                    try
                    {
                        XmlNode child = root.ChildNodes[i];
                        if (child != null)
                        {
                            Symbol symbol = new Symbol(i, child);
                            try
                            {
                                this.hash_.Add(symbol.Unicode, symbol);
                            }
                            catch
                            {
                            }
                            this.list_.Add(symbol);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch
            {
            }
        }


        public string Filename
        {
            set
            {
                this.filename_ = value.Trim();
            }
        }

        public string FontFamily
        {
            get
            {
                return this.fontFamily_;
            }
            set
            {
                this.fontFamily_ = value.Trim();
            }
        }

        public string Name
        {
            get
            {
                return this.name_;
            }
            set
            {
                this.name_ = value.Trim();
            }
        }

        public int CorrectX
        {
            get
            {
                return this.corX_;
            }
            set
            {
                this.corX_ = value;
            }
        }

        public int CorrectY
        {
            get
            {
                return this.corY_;
            }
            set
            {
                this.corY_ = value;
            }
        }

        public int CorrectH
        {
            get
            {
                return this.corH_;
            }
            set
            {
                this.corH_ = value;
            }
        }

        public int CorrectW
        {
            get
            {
                return this.corW_;
            }
            set
            {
                this.corW_ = value;
            }
        }

        public int CorrectB
        {
            get
            {
                return this.corB_;
            }
            set
            {
                this.corB_ = value;
            }
        }


        private Hashtable hash_;
        private ArrayList list_;
        private string name_;
        private string fontFamily_;
        private string filename_;
        private int corX_;
        private int corY_;
        private int corH_;
        private int corW_;
        private int corB_;
    }
}

