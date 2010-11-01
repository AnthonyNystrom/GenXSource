namespace Fonts
{
    using System;
    using System.Collections;

    //
    public class EntityInfo
    {
        public EntityInfo(int index, string name, string code, string codeAlias, string description, string description_font, bool bVisible)
        {
            this.index = 0;
            this.name = "";
            this.fontFamily = "";
            this.code = "";
            this.codeAlias = "";
            this.charPos = "";
            this.description = "";
            this.descrFont = "";
            this.fontTables = new ArrayList(0);
            this.Index = index;
            this.Name = name;
            this.Code = code;
            this.CodeAlias = codeAlias;
            this.Description = description;
            this.DescriptionFont = description_font;
            this.IsVisible = bVisible;
        }

        public void Add(FontTable fontTable)
        {
            try
            {
                if ((this.fontTables == null) || (this.fontTables.Count == 0))
                {
                    this.fontTables = new ArrayList(1);
                    this.fontTables.Add(fontTable);
                }
                else
                {
                    this.fontTables.Add(fontTable);
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
                this.index = value;
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

        public string DescriptionFont
        {
            get
            {
                return this.descrFont;
            }
            set
            {
                this.descrFont = value.Trim();
            }
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

        public FontTable[] FontTables
        {
            set
            {
                FontTable[] tables = value;
                if ((tables != null) && (tables.Length > 0))
                {
                    this.fontTables = new ArrayList(tables.Length);
                    for (int i = 0; i < tables.Length; i++)
                    {
                        this.fontTables.Add(tables[i]);
                    }
                }
                else
                {
                    this.fontTables = null;
                }
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
            get
            {
                return this.codeAlias;
            }
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


        private int index;
        private string name;
        private bool isVisible;
        public ArrayList fontTables;
        private string fontFamily;
        private string code;
        private string codeAlias;
        private string charPos;
        private string description;
        private string descrFont;
        private UnicodeCategory cat;
    }
}

