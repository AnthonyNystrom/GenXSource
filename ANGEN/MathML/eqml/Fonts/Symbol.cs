namespace Fonts
{
    using System;
    using System.Xml;

    public class Symbol
    {
        public Symbol(int index, XmlNode node)
        {
            this.index_ = 0;
            this.unicode_ = "";
            this.charpos_ = "";
            this.desc_ = "";

            try
            {
                node.Attributes.GetNamedItem("name");
                XmlNode uniNode = node.Attributes.GetNamedItem("unicode");
                XmlNode charPos = node.Attributes.GetNamedItem("charPos");
                XmlNode desc = node.Attributes.GetNamedItem("desc");
                this.Index = index;
                if (uniNode != null)
                {
                    this.Unicode = uniNode.Value;
                }
                if (charPos != null)
                {
                    this.CharPos = charPos.Value;
                }
                if (desc != null)
                {
                    this.Desc = desc.Value;
                }
            }
            catch
            {
            }
        }


        public int Index
        {
            set
            {
                this.index_ = value;
            }
        }

        public string Unicode
        {
            get
            {
                return this.unicode_;
            }
            set
            {
                this.unicode_ = value.Trim().PadLeft(5, '0');
            }
        }

        public string CharPos
        {
            get
            {
                return this.charpos_;
            }
            set
            {
                this.charpos_ = value.Trim();
            }
        }

        public string Desc
        {
            set
            {
                this.desc_ = value.Trim();
            }
        }

        private int index_;
        private string unicode_;
        private string charpos_;
        private string desc_;
    }
}

