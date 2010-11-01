namespace Nodes
{
    using Attrs;
    using Rendering;
    using Boxes;
    using Nodes;
    
    using Fonts;
    using Facade;
    using System;
    using System.Collections;
    using System.Drawing;
    using System.Globalization;
    using System.Xml;

    public partial class Node
    {
                public void SetStyle(StyleAttributes styleAttributes)
        {
            if (style_ == null)
            {
                style_ = new StyleAttributes();
            }
            styleAttributes.CopyTo(style_);
            style_.canOverride = true;
            if (HasChildren())
            {
                NodesList list = GetChildrenNodes();
                for (Node child = list.Next(); child != null; child = list.Next())
                {
                    if (child.type_.type != ElementType.Entity)
                    {
                        child.SetStyle(styleAttributes);
                    }
                }
            }
        }

        public void SetColor(Color _color)
        {
            if (style_ == null)
            {
                style_ = new StyleAttributes();
            }
            style_.canOverride = true;
            style_.color = _color;
            style_.hasColor = true;
            if (HasChildren())
            {
                NodesList list = GetChildrenNodes();
                for (Node node = list.Next(); node != null; node = list.Next())
                {
                    node.SetColor(_color);
                }
            }
        }

        public void SetBackground(Color _color)
        {
            if (style_ == null)
            {
                style_ = new StyleAttributes();
            }
            style_.canOverride = true;
            style_.background = _color;
            style_.hasBackground = true;
            if (HasChildren())
            {
                NodesList list = GetChildrenNodes();
                for (Node node = list.Next(); node != null; node = list.Next())
                {
                    if (node.type_.type != ElementType.Entity)
                    {
                        node.SetBackground(_color);
                    }
                }
            }
        }

        public void ClearStyle()
        {
            style_ = null;
            try
            {
                if (!HasChildren())
                {
                    return;
                }
                NodesList list = GetChildrenNodes();
                for (Node node = list.Next(); node != null; node = list.Next())
                {
                    try
                    {
                        if (node.type_.type != ElementType.Entity)
                        {
                            node.ClearStyle();
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

        public void SetScale(double scaling)
        {
            if (style_ == null)
            {
                style_ = new StyleAttributes();
            }
            if (scaling == 1)
            {
                style_.hasSize = false;
                style_.size = "";
                style_.scale = 1;
                style_.canOverride = true;
            }
            else
            {
                style_.hasSize = true;
                style_.scale = scaling;
                string size = (scaling*100).ToString();
                size = size.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator, ".");
                size = size + "%";
                style_.size = size;
                style_.canOverride = true;
            }
            if (HasChildren())
            {
                NodesList list = GetChildrenNodes();
                for (Node node = list.Next(); node != null; node = list.Next())
                {
                    if (node.type_.type != ElementType.Entity)
                    {
                        node.SetScale(scaling);
                    }
                }
            }
        }

               public int FontStyle
        {
            get
            {
                if (style_ == null)
                {
                    return 0;
                }
                int style = Nodes.FontStyle.NONE;
                if (style_.isBold)
                {
                    style |= Nodes.FontStyle.BOLD;
                }
                if (style_.isItalic)
                {
                    style |= Nodes.FontStyle.ITALIC;
                }
                if (style_.isUnderline)
                {
                    style |= Nodes.FontStyle.UNDERLINE;
                }
                return style;
            }
            set
            {
                int r = value;
                if (r == Nodes.FontStyle.NONE)
                {
                    if (style_ != null)
                    {
                        style_.isBold = false;
                        style_.isItalic = false;
                        style_.isUnderline = false;
                    }
                }
                else
                {
                    if (style_ == null)
                    {
                        style_ = new StyleAttributes();
                    }
                    if ((r & Nodes.FontStyle.BOLD) == Nodes.FontStyle.BOLD)
                    {
                        style_.isBold = true;
                    }
                    else
                    {
                        style_.isBold = false;
                    }
                    if ((r & Nodes.FontStyle.ITALIC) == Nodes.FontStyle.ITALIC)
                    {
                        style_.isItalic = true;
                    }
                    else
                    {
                        style_.isItalic = false;
                    }
                    if ((r & Nodes.FontStyle.UNDERLINE) == Nodes.FontStyle.UNDERLINE)
                    {
                        style_.isUnderline = true;
                    }
                    else
                    {
                        style_.isUnderline = false;
                    }
                }
            }
        }

        public int LiteralLength
        {
            get
            {
                if (literalText != null)
                {
                    int len = literalText.Length;
                    if (len > 1)
                    {
                        return len;
                    }
                }
                return 1;
            }
        }

        public int LiteralStart
        {
            get
            {
                int start = 0;
                if (literalStart == 0)
                {
                    start = GuessLiteralStart();
                }
                else
                {
                    start = literalStart;
                }
                try
                {
                    if ((type_ != null) && (type_.type == ElementType.Ms))
                    {
                        start += ((Box_Ms) box).leftQuoteWidth;
                    }
                }
                catch
                {
                }
                return start;
            }
            set { literalStart = value; }
        }

        public string StyleClass
        {
            get
            {
                if (style_ != null)
                {
                    return style_.styleClass;
                }
                return "";
            }
            set
            {
                if (value.Length > 0)
                {
                    if (style_ == null)
                    {
                        style_ = new StyleAttributes();
                    }
                    style_.styleClass = value;
                }
                else if (style_ != null)
                {
                    style_.styleClass = value;
                }
            }
        }

        public string Class
        {
            get
            {
                try
                {
                    if (attrs != null)
                    {
                        Attribute attribute = null;
                        attribute = attrs.Get("class");
                        if (attribute == null)
                        {
                            return "";
                        }
                        return attribute.val;
                    }
                }
                catch
                {
                }
                return "";
            }
            set
            {
                string val = "";
                val = value;
                try
                {
                    if (val.Length > 0)
                    {
                        if (attrs == null)
                        {
                            attrs = new AttributeList();
                        }
                        attrs.Add("class", val);
                    }
                    else if (attrs != null)
                    {
                        Attribute attribute = null;
                        attribute = attrs.Get("class");
                        if (attribute == null)
                        {
                            return;
                        }
                        attrs.Remove(attribute);
                    }
                }
                catch
                {
                }
            }
        }

        public Color StyleColor
        {
            get
            {
                if (style_ != null)
                {
                    return style_.color;
                }
                return Color.Black;
            }
            set
            {
                if (style_ == null)
                {
                    if (value == Color.Black)
                    {
                        return;
                    }
                    style_ = new StyleAttributes();
                    style_.color = value;
                }
                else
                {
                    style_.color = value;
                }
            }
        }

        public Color Background
        {
            get
            {
                if (style_ != null)
                {
                    return style_.background;
                }
                return Color.White;
            }
            set
            {
                if (style_ == null)
                {
                    if (value == Color.White)
                    {
                        return;
                    }
                    style_ = new StyleAttributes();
                    style_.background = value;
                }
                else
                {
                    style_.background = value;
                }
            }
        }

        public string StyleFontFamily
        {
            get
            {
                if (style_ != null)
                {
                    return style_.fontFamily;
                }
                return "";
            }
            set
            {
                if (style_ == null)
                {
                    if (value.Length <= 0)
                    {
                        return;
                    }
                    style_ = new StyleAttributes();
                    style_.fontFamily = value;
                }
                else
                {
                    style_.fontFamily = value;
                }
            }
        }

        public double StyleScale
        {
            get
            {
                if (style_ != null)
                {
                    return style_.scale;
                }
                return 1;
            }
            set
            {
                if (style_ == null)
                {
                    if (value != 1)
                    {
                        style_ = new StyleAttributes();
                        style_.scale = value;
                        style_.hasSize = true;
                    }
                }
                else
                {
                    style_.scale = value;
                    if (value != 1)
                    {
                        style_.hasSize = true;
                    }
                    else
                    {
                        style_.hasSize = false;
                    }
                }
            }
        }

        public int InternalMark
        {
            get
            {
                try
                {
                    return Math.Min(literalCaret, LiteralLength);
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                int length = 0;
                length = value;
                if (length > LiteralLength)
                {
                    length = LiteralLength;
                }
                if (length < 0)
                {
                    length = 0;
                }
                literalCaret = length;
            }
        }


    }
}