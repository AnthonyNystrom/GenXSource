namespace Fonts
{
    using System;
    using System.Globalization;

    public enum CategoryType
    {
        OP,
        ID,
        UNKNOWN
    }

    public class UnicodeCategory
    {
        public UnicodeCategory(string sID, string name, string sType, string description, string min, string max)
        {
            this.ID = System.Convert.ToInt32(sID);
            this.Name = name;
            this.Description = description;
            this.Min = min;
            this.Max = max;
            if (sType == "mo")
            {
                this.Type = CategoryType.OP;
            }
            else if (sType == "mi")
            {
                this.Type = CategoryType.ID;
            }
            else
            {
                this.Type = CategoryType.UNKNOWN;
            }
        }

        private int Convert(string hex)
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

        public int ID
        {
            get
            {
                return this.id_;
            }
            set
            {
                this.id_ = value;
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
                this.name_ = value;
            }
        }

        public CategoryType Type
        {
            set
            {
                this.type_ = value;
            }
        }

        public string Description
        {
            set
            {
                this.desc_ = value;
            }
        }

        public int MinValue
        {
            get
            {
                try
                {
                    return this.Convert(this.min_);
                }
                catch
                {
					return 0;
                }
            }
        }

        public int MaxValue
        {
            get
            {
                try
                {
                    return this.Convert(this.max_);
                }
                catch
                {
					return 0;
                }
            }
        }

        public string Min
        {
            set
            {
                this.min_ = value;
            }
        }

        public string Max
        {
            set
            {
                this.max_ = value;
            }
        }

        private int id_;
        private string name_;
        private string desc_;
        private string min_;
        private string max_;
        private CategoryType type_;
    }
}

