namespace Rendering
{
    using Attrs;
    using Boxes;
    using Nodes;
    using Operators;
    
    using Fonts;
    using Facade;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Text;
    using System.IO;

    public partial class Painter
    {
        private System.Drawing.Font MakeFont(Node node, int mode, Category category, int level, StyleAttributes styleAttributes)
        {
            string fontName;
            double level0Scale = 1;
            double level1Scale = 0.71;
            double level2Scale = 0.5;
            System.Drawing.FontStyle fontStyle = System.Drawing.FontStyle.Regular;
            if (styleAttributes != null)
            {
                level0Scale = styleAttributes.scale;
                if (styleAttributes.isBold)
                {
                    fontStyle |= System.Drawing.FontStyle.Bold;
                }
                if (styleAttributes.isItalic)
                {
                    fontStyle |= System.Drawing.FontStyle.Italic;
                }
                bool flag1 = styleAttributes.isUnderline;
            }
            switch (category)
            {
                case Category.BOLD:
                {
                    fontStyle |= System.Drawing.FontStyle.Bold;
                    break;
                }
                case Category.UNKNOWN:
                {
                    if (((node != null) && (node.type_ != null)) && (node.type_.type == ElementType.Mi))
                    {
                        if (styleAttributes == null)
                        {
                            fontStyle |= System.Drawing.FontStyle.Italic;
                            break;
                        }
                        if (styleAttributes.isTop)
                        {
                            fontStyle |= System.Drawing.FontStyle.Italic;
                        }
                        else if (((!styleAttributes.isNormal && !styleAttributes.isBold) && (!styleAttributes.isDoubleStruck && !styleAttributes.isFractur)) && ((!styleAttributes.isMonospace && !styleAttributes.isSans) && !styleAttributes.isScript))
                        {
                            fontStyle |= System.Drawing.FontStyle.Italic;
                        }
                    }
                    break;
                }
            }
            
            switch (category)
            {
                case Category.OPERATOR:
                case Category.NUMBER:
                case Category.UNKNOWN:
                    if (styleAttributes != null)
                    {
                        if (styleAttributes.isSans)
                        {
                            category = Category.SANSSERIF;
                        }
                        else if (styleAttributes.isMonospace)
                        {
                            category = Category.MONOSPACE;
                        }
                        else if (((styleAttributes.isFractur && (node != null)) &&
                                  ((node.literalText != null) && (node.literalText.Length > 0))) &&
                                 this.IsAlphaCaseless(node.literalText))
                        {
                            category = Category.FRACTUR;
                        }
                        else if (((styleAttributes.isDoubleStruck && (node != null)) &&
                                  ((node.literalText != null) && (node.literalText.Length > 0))) &&
                                 this.IsAlpha(node.literalText))
                        {
                            category = Category.DOUBLESTRUCK;
                        }
                        else if (((styleAttributes.isScript && (node != null)) &&
                                  ((node.literalText != null) && (node.literalText.Length > 0))) &&
                                 this.IsAlphaCaseless(node.literalText))
                        {
                            category = Category.SCRIPT;
                        }
                    }
                    break;
            }    
        
            switch (category)
            {
                case Category.SYMBOL:
                {
                    switch (level)
                    {
                        case 0:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Symbol", this.textSize);
                            }
                            if (styleAttributes == null)
                            {
                                return this.symbol0;
                            }
                            return new System.Drawing.Font("Symbol", (float) (this.textSize * level0Scale), fontStyle);
                        }
                        case 1:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Symbol", (float) (this.textSize * level1Scale));
                            }
                            if (styleAttributes == null)
                            {
                                return this.symbol1;
                            }
                            return new System.Drawing.Font("Symbol", (float) ((this.textSize * level1Scale) * level0Scale), fontStyle);
                        }
                        case 2:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Symbol", (float) (this.textSize * level2Scale));
                            }
                            if (styleAttributes == null)
                            {
                                return this.symbol2;
                            }
                            return new System.Drawing.Font("Symbol", (float) ((this.textSize * level2Scale) * level0Scale), fontStyle);
                        }
                    }
                    break;
                }
                case Category.BOLD:
                {
                    switch (level)
                    {
                        case 0:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Symbol", (float) (this.textSize * 1.5), System.Drawing.FontStyle.Bold);
                            }
                            if (styleAttributes == null)
                            {
                                return this.bold0;
                            }
                            return new System.Drawing.Font("Symbol", (float) ((this.textSize * 1.5) * level0Scale), fontStyle);
                        }
                        case 1:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Symbol", (float) ((this.textSize * 1.5) * level1Scale), System.Drawing.FontStyle.Bold);
                            }
                            if (styleAttributes == null)
                            {
                                return this.bold1;
                            }
                            return new System.Drawing.Font("Symbol", (float) (((this.textSize * 1.5) * level1Scale) * level0Scale), fontStyle);
                        }
                        case 2:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Symbol", (float) ((this.textSize * 1.5) * level2Scale), System.Drawing.FontStyle.Bold);
                            }
                            if (styleAttributes == null)
                            {
                                return this.bold2;
                            }
                            return new System.Drawing.Font("Symbol", (float) (((this.textSize * 1.5) * level2Scale) * level0Scale), fontStyle);
                        }
                    }
                    break;
                }
                
                case Category.OPERATOR:
                {
                    switch (level)
                    {
                        case 0:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", this.textSize);
                            }
                            if (styleAttributes == null)
                            {
                                return this.operator0;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level0Scale), fontStyle);
                        }
                        case 1:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level1Scale));
                            }
                            if (styleAttributes == null)
                            {
                                return this.operator1;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) ((this.textSize * level1Scale) * level0Scale), fontStyle);
                        }
                        case 2:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level2Scale));
                            }
                            if (styleAttributes == null)
                            {
                                return this.operator2;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) ((this.textSize * level2Scale) * level0Scale), fontStyle);
                        }
                    }
                    break;
                }
                case Category.NUMBER:
                {
                    switch (level)
                    {
                        case 0:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", this.textSize);
                            }
                            if (styleAttributes == null)
                            {
                                return this.number0;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level0Scale), fontStyle);
                        }
                        case 1:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level1Scale));
                            }
                            if (styleAttributes == null)
                            {
                                return this.number1;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) ((this.textSize * level1Scale) * level0Scale), fontStyle);
                        }
                        case 2:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level2Scale));
                            }
                            if (styleAttributes == null)
                            {
                                return this.number2;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) ((this.textSize * level2Scale) * level0Scale), fontStyle);
                        }
                    }
                    break;
                }
               
                case Category.UNKNOWN:
                {
                    switch (level)
                    {
                        case 0:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", this.textSize, System.Drawing.FontStyle.Italic);
                            }
                            if (styleAttributes == null)
                            {
                                return this.unknown0;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level0Scale), fontStyle);
                        }
                        case 1:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level1Scale), System.Drawing.FontStyle.Italic);
                            }
                            if (styleAttributes == null)
                            {
                                return this.unknown1;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) ((this.textSize * level1Scale) * level0Scale), fontStyle);
                        }
                        case 2:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Times New Roman", (float) (this.textSize * level2Scale), System.Drawing.FontStyle.Italic);
                            }
                            if (styleAttributes == null)
                            {
                                return this.unknown2;
                            }
                            return new System.Drawing.Font("Times New Roman", (float) ((this.textSize * level2Scale) * level0Scale), fontStyle);
                        }
                    }
                    break;
                }
                
                case Category.SANSSERIF:
                {
                    switch (level)
                    {
                        case 0:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Arial", this.textSize);
                            }
                            if (styleAttributes == null)
                            {
                                return this.operator0;
                            }
                            return new System.Drawing.Font("Arial", (float) (this.textSize * level0Scale), fontStyle);
                        }
                        case 1:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Arial", (float) (this.textSize * level1Scale));
                            }
                            if (styleAttributes == null)
                            {
                                return this.operator1;
                            }
                            return new System.Drawing.Font("Arial", (float) ((this.textSize * level1Scale) * level0Scale), fontStyle);
                        }
                        case 2:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Arial", (float) (this.textSize * level2Scale));
                            }
                            if (styleAttributes == null)
                            {
                                return this.operator2;
                            }
                            return new System.Drawing.Font("Arial", (float) ((this.textSize * level2Scale) * level0Scale), fontStyle);
                        }
                    }
                    break;
                }
                case Category.FRACTUR:
                case Category.DOUBLESTRUCK:
                case Category.SCRIPT:
                {
                    fontName = "";
                    switch (category)
                    {
                        case Category.FRACTUR:
                        {
                            fontName = "ESSTIXFifteen";
                            break;
                        }
                        case Category.DOUBLESTRUCK:
                        {
                            fontName = "ESSTIXFourteen";
                            break;
                        }
                        case Category.SCRIPT:
                        {
                            fontName = "ESSTIXThirteen";
                            break;
                        }
                    }
                    float sc = 1f;
                    switch (level)
                    {
                        case 0:
                        {
                            sc = (float) level0Scale;
                            break;
                        }
                        case 1:
                        {
                            sc = (float) (level1Scale*level0Scale);
                            break;
                        }
                        case 2:
                        {
                            sc = (float) (level2Scale*level0Scale);
                            break;
                        }
                    }
                    FontFamily family = null;
                    if (this.fonts_ != null)
                    {
                        family = this.fonts_.Get(fontName);
                    }
                    if (family != null)
                    {
                        return new System.Drawing.Font(family, this.textSize*sc, fontStyle);
                    }
                    return new System.Drawing.Font(fontName, this.textSize*sc, fontStyle);
                }
                case Category.MONOSPACE:
                {
                    switch (level)
                    {
                        case 0:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Courier New", this.textSize, System.Drawing.FontStyle.Italic);
                            }
                            if (styleAttributes == null)
                            {
                                return this.mono0;
                            }
                            return new System.Drawing.Font("Courier New", (float) (this.textSize * level0Scale), fontStyle);
                        }
                        case 1:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Courier New", (float) (this.textSize * level1Scale), System.Drawing.FontStyle.Italic);
                            }
                            if (styleAttributes == null)
                            {
                                return this.mono1;
                            }
                            return new System.Drawing.Font("Courier New", (float) ((this.textSize * level1Scale) * level0Scale), fontStyle);
                        }
                        case 2:
                        {
                            if (mode == 1)
                            {
                                return new System.Drawing.Font("Courier New", (float) (this.textSize * level2Scale), System.Drawing.FontStyle.Italic);
                            }
                            if (styleAttributes == null)
                            {
                                return this.mono2;
                            }
                            return new System.Drawing.Font("Courier New", (float) ((this.textSize * level2Scale) * level0Scale), fontStyle);
                        }
                    }
                    break;
                }
                
                case Category.GLYPH:
                {
                    switch (level)
                    {
                        case 0:
                        {
                            if (mode != 1)
                            {
                                return new System.Drawing.Font(node.fontFamily, (float) (this.textSize * level0Scale), fontStyle);
                            }
                            break;
                        }
                        case 1:
                        {
                            if (mode != 1)
                            {
                                return new System.Drawing.Font(node.fontFamily, (float) ((this.textSize * level1Scale) * level0Scale), fontStyle);
                            }
                            break;
                        }
                        case 2:
                        {
                            if (mode != 1)
                            {
                                return new System.Drawing.Font(node.fontFamily, (float) ((this.textSize * level2Scale) * level0Scale), fontStyle);
                            }
                            break;
                        }
                    }
                    break;
                }
            }
            
            return new System.Drawing.Font("Symbol", this.textSize);
        }

        public float MeasureBaseline(Graphics g, string sMaster_Font, Font font)
        {
            float b = 0f;
            string dpi = "";
            try
            {
                dpi = g.DpiX.ToString();
            }
            catch
            {
            }
            try
            {
                if (((font.FontFamily.Name.IndexOf("ESSTIX") == -1) || (font.FontFamily.Name == "ESSTIXEight")) || (font.FontFamily.Name == "ESSTIXSeventeen"))
                {
                    return b;
                }
                string fontID = sMaster_Font + "::" + dpi + "::" + font.Size.ToString();
                if (hash_.ContainsKey(fontID))
                {
                    return hash_[fontID];
                }

                Font f = new Font(sMaster_Font, font.Size, System.Drawing.FontStyle.Regular);
                SizeF ef1 = g.MeasureString("X", f, new PointF(0f, 0f), strFormat);
                b = 0.021f * ef1.Height;
                try
                {
                    this.hash_.Add(fontID, b);
                }
                catch
                {
                }
            }
            catch
            {
            }
            return b;
        }

        private static StringFormat strFormat = new StringFormat(StringFormat.GenericTypographic);
        private IDictionary<string,float> hash_ = new Dictionary<string, float>();
    }
}