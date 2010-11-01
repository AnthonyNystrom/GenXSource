namespace Attrs
{
    using Nodes;
    using Facade;
    using System;
    using System.Drawing;
    using System.Globalization;

    public class StyleAttributes
    {
        public StyleAttributes()
        {
            this.canOverride = false;
            this.scriptLevel = ScriptLevel.NONE;
            this.displayStyle = DisplayStyle.AUTOMATIC;
            this.isTop = false;
            this.color = Color.Black;
            this.background = Color.White;
            this.styleClass = "";
            this.size = "";
            this.scale = 1;
            this.hasSize = false;
            this.hasColor = false;
            this.hasBackground = false;
            this.isBold = false;
            this.isItalic = false;
            this.isNormal = false;
            this.isScript = false;
            this.isFractur = false;
            this.isMonospace = false;
            this.isDoubleStruck = false;
            this.isSans = false;
            this.isUnderline = false;
            this.fontFamily = "";
        }

        public void CopyTo(StyleAttributes styleAttributes)
        {
            styleAttributes.displayStyle = this.displayStyle;
            styleAttributes.scriptLevel = this.scriptLevel;
            styleAttributes.canOverride = this.canOverride;
            styleAttributes.color = this.color;
            styleAttributes.background = this.background;
            styleAttributes.size = this.size;
            styleAttributes.scale = this.scale;
            styleAttributes.isBold = this.isBold;
            styleAttributes.isItalic = this.isItalic;
            styleAttributes.isNormal = this.isNormal;
            styleAttributes.isUnderline = this.isUnderline;
            styleAttributes.isTop = this.isTop;
            styleAttributes.isDoubleStruck = this.isDoubleStruck;
            styleAttributes.isFractur = this.isFractur;
            styleAttributes.isScript = this.isScript;
            styleAttributes.isSans = this.isSans;
            styleAttributes.isMonospace = this.isMonospace;
            styleAttributes.styleClass = this.styleClass;
            styleAttributes.hasColor = this.hasColor;
            styleAttributes.hasBackground = this.hasBackground;
            styleAttributes.hasSize = this.hasSize;
            styleAttributes.fontFamily = this.fontFamily;
            
        }

        public bool HasSameStyle(StyleAttributes styleAttributes)
        {
            if (((((styleAttributes.displayStyle == this.displayStyle) &&
                   (styleAttributes.scriptLevel == this.scriptLevel)) &&
                  ((styleAttributes.color == this.color) && (styleAttributes.background == this.background))) &&
                 (((styleAttributes.size == this.size) && (styleAttributes.scale == this.scale)) &&
                  ((styleAttributes.isBold == this.isBold) && (styleAttributes.isNormal == this.isNormal)))) &&
                ((((styleAttributes.isItalic == this.isItalic) &&
                   (styleAttributes.isDoubleStruck == this.isDoubleStruck)) &&
                  ((styleAttributes.isFractur == this.isFractur) && (styleAttributes.isScript == this.isScript))) &&
                 ((styleAttributes.isSans == this.isSans) && (styleAttributes.isMonospace == this.isMonospace))))
            {
                return true;
            }
            return false;
        }

        public string FontToString()
        {
            if (((this.isBold && this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "sans-serif-bold-italic";
            }
            if (((this.isBold && !this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "bold-sans-serif";
            }
            if (((!this.isBold && this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "sans-serif-italic";
            }
            if (((this.isBold && !this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((!this.isSans && this.isScript) && !this.isMonospace))
            {
                return "bold-script";
            }
            if (((this.isBold && !this.isItalic) && (this.isFractur && !this.isDoubleStruck)) && ((!this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "bold-fraktur";
            }
            if (((this.isBold && this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((!this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "bold-italic";
            }
            if (((this.isBold && !this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((!this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "bold";
            }
            if (((!this.isBold && this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((!this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "italic";
            }
            if (((!this.isBold && !this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((!this.isSans && this.isScript) && !this.isMonospace))
            {
                return "script";
            }
            if (((!this.isBold && !this.isItalic) && (this.isFractur && !this.isDoubleStruck)) && ((!this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "fraktur";
            }
            if (((!this.isBold && !this.isItalic) && (!this.isFractur && this.isDoubleStruck)) && ((!this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "double-struck";
            }
            if (((!this.isBold && !this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((this.isSans && !this.isScript) && !this.isMonospace))
            {
                return "sans-serif";
            }
            if (((!this.isBold && !this.isItalic) && (!this.isFractur && !this.isDoubleStruck)) && ((!this.isSans && !this.isScript) && this.isMonospace))
            {
                return "monospace";
            }
            if (this.isNormal)
            {
                return "normal";
            }
            return "";
        }

        public string FontSize()
        {
            
            try
            {
                return (((this.scale * 100)).ToString() + "%").Replace(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator, ".");
            }
            catch
            {
                return "100.0%";
            }
        }

        public static double FontScale(string sFontSize, double BasePTSize)
        {
            try
            {
                double numericSize = 0;
                string numeric = "";
                string unit = "";

                for (int i = 0; i < sFontSize.Length; i++)
                {
                    char c = sFontSize[i];
                    if ((char.IsDigit(c) || char.IsPunctuation(c)) && ((c != '%') && (c != '-')))
                    {
                        numeric = numeric + c;
                    }
                    else if ((char.IsLetter(c) || (c == '%')) || (c == '-'))
                    {
                        unit = unit + c;
                    }
                }

                numeric = numeric.Trim().ToUpper();
                try
                {
                    numeric = numeric.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    if (numeric.Length > 0)
                    {
                        numericSize = Convert.ToDouble(numeric);
                    }
                }
                catch
                {
                }
                
                unit = unit.Trim().ToUpper();
                switch (unit)
                {
                    case "XX-SMALL":
                    {
                        return 0.51;
                    }
                    case "X-SMALL":
                    {
                        return 0.64;
                    }
                    case "SMALL":
                    {
                        return 0.8;
                    }
                    case "MEDIUM":
                    {
                        return 1;
                    }
                    case "LARGE":
                    {
                        return 1.25;
                    }
                    case "X-LARGE":
                    {
                        return 1.56;
                    }
                    case "XX-LARGE":
                    {
                        return 1.95;
                    }
                    case "BIG":
                    {
                        return 1.25;
                    }
                    case "NORMAL":
                    {
                        return 1;
                    }
                    case "EX":
                    {
                        return (numericSize*BasePTSize*0.5)/BasePTSize;
                    }
                    case "PX":
                    {
                        return (numericSize/4)/BasePTSize;
                    }
                    case "EM":
                    {
                        return (numericSize*BasePTSize)/BasePTSize;
                    }
                    case "MM":
                    {
                        return ((numericSize * 72) / 25.4)/BasePTSize;
                    }
                    case "CM":
                    {
                        return ((numericSize * 10 * 72) / 25.4)/BasePTSize;
                    }
                    case "IN":
                    {
                        return (numericSize * 72)/BasePTSize;
                    }
                    case "%":
                    {
                        return numericSize/100;
                    }
                    case "PT":
                    {
                        return numericSize/BasePTSize;
                    }
                    case "PC":
                    {
                        return (numericSize*12)/BasePTSize;
                    }
                    default:
                        return 1;
                }
            }
            catch
            {
                return 1;
            }
        }

        public bool IsStyled
        {
            get
            {
                if (((!this.isBold && !this.isItalic) && (!this.isMonospace && !this.isNormal)) &&
                    ((!this.isSans && !this.isScript) && (!this.isDoubleStruck && !this.isFractur)))
                {
                    return false;
                }
                return true;
            }
        }


        public bool canOverride;
        public ScriptLevel scriptLevel;
        public bool hasColor;
        public bool hasBackground;
        public bool isBold;
        public bool isItalic;
        public bool isNormal;
        public bool isScript;
        public bool isFractur;
        public bool isMonospace;
        public bool isDoubleStruck;
        public DisplayStyle displayStyle;
        public bool isSans;
        public bool isUnderline;
        public string fontFamily;
        public bool isTop;
        public Color color;
        public Color background;
        public string styleClass;
        public string size;
        public double scale;
        public bool hasSize;
    }
}

