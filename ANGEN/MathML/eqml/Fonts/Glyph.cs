namespace Fonts
{
    using System;
    using System.Globalization;

    public class Glyph
    {
        public Glyph(EntityInfo item)
        {
            this.name = "";
            this.fontFamily = "";
            this.code = "";
            this.codeAlias = "";
            this.charPos = "";
            this.description = "";
            this.Name = item.Name;
            this.Code = item.Code;
            this.CodeAlias = item.CodeAlias;
            if (item.DescriptionFont.Length > 0)
            {
                this.Description = item.DescriptionFont;
            }
            else
            {
                this.Description = item.Description;
            }
            this.Category = item.Category;
            this.FontFamily = item.FontFamily;
            this.CharPos = item.CharPos;
            this.IsVisible = item.IsVisible;
        }

        private char FromHex(string hex)
        {
            char ch1 = ' ';
            try
            {
                string text1 = "";
                text1 = hex;
                while ((text1.Length > 1) && (text1.Substring(0, 1) == "0"))
                {
                    text1 = text1.Substring(1, text1.Length - 1);
                }
                if (text1.Length <= 4)
                {
                    text1 = text1.PadLeft(4, '0');
                    int num1 = int.Parse(text1, NumberStyles.HexNumber);
                    ch1 = Convert.ToChar(num1);
                }
            }
            catch
            {
            }
            return ch1;
        }


        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value.Trim();
            }
        }

        public UnicodeCategory Category
        {
            get
            {
                return this.cat;
            }
            set
            {
                this.cat = value;
            }
        }

        public string FontFamily
        {
            get
            {
                return this.fontFamily;
            }
            set
            {
                this.fontFamily = value;
            }
        }

        public bool IsVisible
        {
            get
            {
                return this.isVisible;
            }
            set
            {
                this.isVisible = value;
            }
        }

        public string CharPos
        {
            get
            {
                return this.charPos;
            }
            set
            {
                this.charPos = value;
            }
        }

        public char CharValue
        {
            get
            {
                char charValue = ' ';
                try
                {
                    if (this.CharPos.Length > 0)
                    {
                        charValue = this.FromHex(this.CharPos);
                    }
                }
                catch
                {
                }
                return charValue;
            }
        }

        public string Code
        {
            get
            {
                return this.code;
            }
            set
            {
                this.code = value.Trim().PadLeft(5, '0');
            }
        }

        public string CodeAlias
        {
            set
            {
                if (value.Length > 0)
                {
                    this.codeAlias = value.Trim().PadLeft(5, '0');
                }
                else
                {
                    this.codeAlias = "";
                }
            }
        }

        public string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value.Trim();
            }
        }

        private string name;
        private string fontFamily;
        private string code;
        private string codeAlias;
        private string charPos;
        private string description;
        private UnicodeCategory cat;
        private bool isVisible;
    }
}

