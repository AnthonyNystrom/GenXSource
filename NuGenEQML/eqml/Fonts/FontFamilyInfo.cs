namespace Fonts
{
    using System;

    public class FontFamilyInfo
    {
        public FontFamilyInfo()
        {
            this.name_ = "";
            this.ffamily_ = "";
            this.corX_ = 0;
            this.corY_ = 0;
            this.corH_ = 1;
            this.corW_ = 0;
            this.corB_ = 0;
        }


        public string FontFamily
        {
            get
            {
                return this.ffamily_;
            }
            set
            {
                this.ffamily_ = value.Trim();
            }
        }

        public string Name
        {
            set
            {
                this.name_ = value.Trim();
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

        public bool NeedCorrectY
        {
            get
            {
                if (this.CorrectY != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool NeedCorrectX
        {
            get
            {
                if (this.CorrectX != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool NeedCorrectW
        {
            get
            {
                if (this.CorrectW != 0)
                {
                    return true;
                }
                return false;
            }
        }

        public bool NeedCorrectH
        {
            get
            {
                if (this.CorrectH != 1)
                {
                    return true;
                }
                return false;
            }
        }

        public bool NeedCorrectB
        {
            get
            {
                if (this.CorrectB != 0)
                {
                    return true;
                }
                return false;
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


        private string name_;
        private string ffamily_;
        private int corX_;
        private int corY_;
        private int corH_;
        private int corW_;
        private int corB_;
    }
}

