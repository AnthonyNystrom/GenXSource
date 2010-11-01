namespace Operators
{
    using System;

    public class Operator
    {
        public Operator()
        {
            this.vvThin = 0.0555556;
            this.vThin = 0.111111;
            this.thin = 0.166667;
            this.medium = 0.222222;
            this.thick = 0.277778;
            this.vThick = 0.333333;
            this.vvThick = 0.388889;
            this.form = Form.UNKNOWN;
            this.fence = false;
            this.separator = false;
            this.lspace = "";
            this.rspace = "";
            this.stretchy = false;
            this.symmetric = true;
            this.maxsize = "";
            this.minsize = "";
            this.largeop = false;
            this.movablelimits = false;
            this.accent = false;
            this.text = "";
            this.unicode = "";
            this.entity = "";
            this.active = true;
        }


        public double LSpace
        {
            get
            {
                try
                {
                    switch (this.lspace.Trim().ToUpper())
                    {
                        case "0":
                            return 0;

                        case "VERYVERYTHINMATHSPACE":
                            return this.vvThin;

                        case "VERYTHINMATHSPACE":
                            return this.vThin;

                        case "THINMATHSPACE":
                            return this.thin;

                        case "MEDIUMMATHSPACE":
                            return this.medium;

                        case "THICKMATHSPACE":
                            return this.thick;

                        case "VERYTHICKMATHSPACE":
                            return this.vThick;

                        case "VERYVERYTHICKMATHSPACE":
                            return this.vvThick;
                    }
                    return this.thick;
                }
                catch
                {
					return 0;
                }
            }
        }

        public double RSpace
        {
            get
            {
                try
                {
                    switch (this.rspace.Trim().ToUpper())
                    {
                        case "0":
                            return 0;

                        case "VERYVERYTHINMATHSPACE":
                            return this.vvThin;

                        case "VERYTHINMATHSPACE":
                            return this.vThin;

                        case "THINMATHSPACE":
                            return this.thin;

                        case "MEDIUMMATHSPACE":
                            return this.medium;

                        case "THICKMATHSPACE":
                            return this.thick;

                        case "VERYTHICKMATHSPACE":
                            return this.vThick;

                        case "VERYVERYTHICKMATHSPACE":
                            return this.vvThick;
                    }
                    return this.thick;
                }
                catch
                {
					return 0;
                }
            }
        }


        private double vvThin;
        private double vThin;
        public string lspace;
        public string rspace;
        public bool stretchy;
        public bool symmetric;
        public string maxsize;
        public string minsize;
        public bool largeop;
        public bool movablelimits;
        public bool accent;
        public string text;
        private double thin;
        public string unicode;
        public string entity;
        public bool active;
        private double medium;
        private double thick;
        private double vThick;
        private double vvThick;
        public Form form;
        public bool fence;
        public bool separator;
    }
}

