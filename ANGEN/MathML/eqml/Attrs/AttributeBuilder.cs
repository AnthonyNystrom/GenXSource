namespace Attrs
{
    using Nodes;
    
    using System;
    using System.Drawing;
    using System.Globalization;

    public class AttributeBuilder
    {
        public static TableCellAttributes FromNode(Node node)
        {
            Nodes.Attribute n = null;
            TableCellAttributes tableCellAttributes = null;
            try
            {
                if (node.attrs == null)
                {
                    return tableCellAttributes;
                }
                node.attrs.Reset();
                for (n = node.attrs.Next(); n != null; n = node.attrs.Next())
                {
                    string s = n.val.Trim();
                    if (n.name == "rowalign")
                    {
                        if (s.Length > 0)
                        {
                            if (tableCellAttributes == null)
                            {
                                tableCellAttributes = new TableCellAttributes();
                            }
                            if (s.ToUpper() == "TOP")
                            {
                                tableCellAttributes.rowAlign = RowAlign.TOP;
                            }
                            else if (s.ToUpper() == "BOTTOM")
                            {
                                tableCellAttributes.rowAlign = RowAlign.BOTTOM;
                            }
                            else if (s.ToUpper() == "CENTER")
                            {
                                tableCellAttributes.rowAlign = RowAlign.CENTER;
                            }
                            else if (s.ToUpper() == "BASELINE")
                            {
                                tableCellAttributes.rowAlign = RowAlign.BASELINE;
                            }
                            else if (s.ToUpper() == "AXIS")
                            {
                                tableCellAttributes.rowAlign = RowAlign.AXIS;
                            }
                            else
                            {
                                tableCellAttributes.rowAlign = RowAlign.CENTER;
                            }
                        }
                    }
                    else if (n.name == "columnalign")
                    {
                        if (s.Length > 0)
                        {
                            if (tableCellAttributes == null)
                            {
                                tableCellAttributes = new TableCellAttributes();
                            }
                            if (s.ToUpper() == "LEFT")
                            {
                                tableCellAttributes.columnAlign = HAlign.LEFT;
                            }
                            else if (s.ToUpper() == "CENTER")
                            {
                                tableCellAttributes.columnAlign = HAlign.CENTER;
                            }
                            else if (s.ToUpper() == "RIGHT")
                            {
                                tableCellAttributes.columnAlign = HAlign.RIGHT;
                            }
                            else
                            {
                                tableCellAttributes.columnAlign = HAlign.LEFT;
                            }
                        }
                    }
                    else if (n.name == "rowspan")
                    {
                        if (s.Length > 0)
                        {
                            if (tableCellAttributes == null)
                            {
                                tableCellAttributes = new TableCellAttributes();
                            }
                            tableCellAttributes.rowSpan = Convert.ToInt32(s.Trim());
                        }
                    }
                    else if ((n.name == "columnspan") && (s.Length > 0))
                    {
                        if (tableCellAttributes == null)
                        {
                            tableCellAttributes = new TableCellAttributes();
                        }
                        tableCellAttributes.columnSpan = Convert.ToInt32(s.Trim());
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return tableCellAttributes;
        }

        public static void ApplyAttrs(Node node, TableCellAttributes tableCellAttributes)
        {
            if (((node != null) && (node.type_ != null)) && ((node.type_.type == ElementType.Mtd) && (tableCellAttributes != null)))
            {
                if (tableCellAttributes.rowAlign == RowAlign.TOP)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "top");
                }
                else if (tableCellAttributes.rowAlign == RowAlign.BOTTOM)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "bottom");
                }
                else if (tableCellAttributes.rowAlign == RowAlign.CENTER)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "center");
                }
                else if (tableCellAttributes.rowAlign == RowAlign.BASELINE)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "baseline");
                }
                else if (tableCellAttributes.rowAlign == RowAlign.AXIS)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "axis");
                }
                else if ((tableCellAttributes.rowAlign == RowAlign.UNKNOWN) && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("rowalign");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (tableCellAttributes.columnAlign == HAlign.LEFT)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("columnalign", "left");
                }
                else if (tableCellAttributes.columnAlign == HAlign.CENTER)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("columnalign", "center");
                }
                else if (tableCellAttributes.columnAlign == HAlign.RIGHT)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("columnalign", "right");
                }
                else if ((tableCellAttributes.columnAlign == HAlign.UNKNOWN) && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("columnalign");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (tableCellAttributes.rowSpan != 1)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    string s = tableCellAttributes.rowSpan.ToString();
                    if (s.Length > 0)
                    {
                        node.attrs.Add("rowspan", s);
                    }
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("rowspan");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (tableCellAttributes.columnSpan != 1)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    string s = tableCellAttributes.columnSpan.ToString();
                    if (s.Length > 0)
                    {
                        node.attrs.Add("columnspan", s);
                    }
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("columnspan");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
            }
        }

        public static FencedAttributes FencedAttrsFromNode(Node node)
        {
            Nodes.Attribute attribute = null;
            FencedAttributes attributes = null;
            try
            {
                if (node.attrs == null)
                {
                    return attributes;
                }
                node.attrs.Reset();
                for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                {
                    string s = attribute.val.Trim();
                    if (attributes == null)
                    {
                        attributes = new FencedAttributes();
                    }
                    if (attribute.name == "open")
                    {
                        if (s.Length > 0)
                        {
                            attributes.open = s;
                        }
                        else
                        {
                            attributes.open = "NONE";
                        }
                    }
                    else if (attribute.name == "close")
                    {
                        if (s.Length > 0)
                        {
                            attributes.close = s;
                        }
                        else
                        {
                            attributes.close = "NONE";
                        }
                    }
                    else if (attribute.name == "separators")
                    {
                        if (s.Length > 0)
                        {
                            attributes.separators = s;
                        }
                        else
                        {
                            attributes.separators = "NONE";
                        }
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return attributes;
        }

        public static void ApplyAttrs(Node node, FencedAttributes fencedAttributes)
        {
            if (((node != null) && (node.type_ != null)) && ((node.type_.type == ElementType.Mfenced) && (fencedAttributes != null)))
            {
                if (fencedAttributes.separators.Length > 0)
                {
                    if (fencedAttributes.separators == ",")
                    {
                        if (node.attrs != null)
                        {
                            Nodes.Attribute attribute = node.attrs.Get("separators");
                            if (attribute != null)
                            {
                                node.attrs.Remove(attribute);
                            }
                        }
                    }
                    else
                    {
                        if (node.attrs == null)
                        {
                            node.attrs = new AttributeList();
                        }
                        if (fencedAttributes.separators == "NONE")
                        {
                            node.attrs.Add("separators", "");
                        }
                        else
                        {
                            node.attrs.Add("separators", fencedAttributes.separators);
                        }
                    }
                }
                if (fencedAttributes.open.Length > 0)
                {
                    if ((((fencedAttributes.open == "{") || (fencedAttributes.open == "[")) || ((fencedAttributes.open == "|") || (fencedAttributes.open == "<"))) || ((fencedAttributes.open[0] == '\u2329') || (fencedAttributes.open[0] == '<')))
                    {
                        if (node.attrs == null)
                        {
                            node.attrs = new AttributeList();
                        }
                        node.attrs.Add("open", fencedAttributes.open);
                    }
                    else if (fencedAttributes.open == "NONE")
                    {
                        if (node.attrs == null)
                        {
                            node.attrs = new AttributeList();
                        }
                        node.attrs.Add("open", "");
                    }
                    else if ((fencedAttributes.open == "(") && (node.attrs != null))
                    {
                        Nodes.Attribute attribute = node.attrs.Get("open");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
                if (fencedAttributes.close.Length > 0)
                {
                    if ((((fencedAttributes.close == "}") || (fencedAttributes.close == "]")) || ((fencedAttributes.close == "|") || (fencedAttributes.close == ">"))) || ((fencedAttributes.close[0] == '\u232a') || (fencedAttributes.close[0] == '>')))
                    {
                        if (node.attrs == null)
                        {
                            node.attrs = new AttributeList();
                        }
                        node.attrs.Add("close", fencedAttributes.close);
                    }
                    else if (fencedAttributes.close == "NONE")
                    {
                        if (node.attrs == null)
                        {
                            node.attrs = new AttributeList();
                        }
                        node.attrs.Add("close", "");
                    }
                    else if ((fencedAttributes.close == ")") && (node.attrs != null))
                    {
                        Nodes.Attribute attribute = node.attrs.Get("close");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
            }
        }

        public static FractionAttributes FractionAttrsFromNode(Node node)
        {
            Nodes.Attribute attribute = null;
            FractionAttributes fractionAttributes = null;
            try
            {
                if (node.attrs == null)
                {
                    return fractionAttributes;
                }
                node.attrs.Reset();
                for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                {
                    string s = attribute.val.Trim();
                    if (attribute.name == "linethickness")
                    {
                        if (s.Length > 0)
                        {
                            int lineThickness = 1;
                            if (s.ToUpper() == "THIN")
                            {
                                lineThickness = 1;
                            }
                            else if (s.ToUpper() == "MEDIUM")
                            {
                                lineThickness = 2;
                            }
                            else if (s.ToUpper() == "THICK")
                            {
                                lineThickness = 3;
                            }
                            else
                            {
                                bool isInteger = true;
                                for (int i = 0; i < s.Length; i++)
                                {
                                    if (s[i] == '.')
                                    {
                                        isInteger = false;
                                    }
                                }
                                if (isInteger)
                                {
                                    try
                                    {
                                        lineThickness = Convert.ToInt32(s);
                                    }
                                    catch
                                    {
                                        lineThickness = 1;
                                    }
                                }
                                else
                                {
                                    lineThickness = 1;
                                }
                            }
                            if (fractionAttributes == null)
                            {
                                fractionAttributes = new FractionAttributes();
                            }
                            fractionAttributes.lineThickness = lineThickness;
                        }
                    }
                    else if (attribute.name == "numalign")
                    {
                        if (s.Length > 0)
                        {
                            if (fractionAttributes == null)
                            {
                                fractionAttributes = new FractionAttributes();
                            }
                            if (s.ToUpper() == "LEFT")
                            {
                                fractionAttributes.numAlign = FractionAlign.LEFT;
                            }
                            else if (s.ToUpper() == "CENTER")
                            {
                                fractionAttributes.numAlign = FractionAlign.CENTER;
                            }
                            else if (s.ToUpper() == "RIGHT")
                            {
                                fractionAttributes.numAlign = FractionAlign.RIGHT;
                            }
                        }
                    }
                    else if (attribute.name == "denomalign")
                    {
                        if (s.Length > 0)
                        {
                            if (fractionAttributes == null)
                            {
                                fractionAttributes = new FractionAttributes();
                            }
                            if (s.ToUpper() == "LEFT")
                            {
                                fractionAttributes.denomAlign = FractionAlign.LEFT;
                            }
                            else if (s.ToUpper() == "CENTER")
                            {
                                fractionAttributes.denomAlign = FractionAlign.CENTER;
                            }
                            else if (s.ToUpper() == "RIGHT")
                            {
                                fractionAttributes.denomAlign = FractionAlign.RIGHT;
                            }
                        }
                    }
                    else if ((attribute.name == "bevelled") && (s.Length > 0))
                    {
                        if (fractionAttributes == null)
                        {
                            fractionAttributes = new FractionAttributes();
                        }
                        if (s.ToUpper() == "TRUE")
                        {
                            fractionAttributes.isBevelled = true;
                        }
                        else
                        {
                            fractionAttributes.isBevelled = false;
                        }
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return fractionAttributes;
        }

        public static FractionAttributes FractionAttrsFromNode(float emHeight, float DPI, Node node)
        {
            Nodes.Attribute attribute = null;
            FractionAttributes attributes = null;
            try
            {
                if (node.attrs == null)
                {
                    return attributes;
                }
                node.attrs.Reset();
                for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                {
                    string s = attribute.val.Trim();
                    if (attribute.name == "linethickness")
                    {
                        if (s.Length > 0)
                        {
                            int lineThickness = 1;
                            if (s.ToUpper() == "THIN")
                            {
                                lineThickness = 1;
                            }
                            else if (s.ToUpper() == "MEDIUM")
                            {
                                lineThickness = 2;
                            }
                            else if (s.ToUpper() == "THICK")
                            {
                                lineThickness = 3;
                            }
                            else
                            {
                                bool isInteger = true;
                                for (int i = 0; i < s.Length; i++)
                                {
                                    if (s[i] == '.')
                                    {
                                        isInteger = false;
                                    }
                                }
                                if (isInteger)
                                {
                                    try
                                    {
                                        lineThickness = Convert.ToInt32(s);
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            if (!char.IsLetter(s[s.Length - 1]))
                                            {
                                                s = s + "ex";
                                            }
                                            lineThickness = AttributeBuilder.FontWidth(emHeight, DPI, s, 1);
                                        }
                                        catch
                                        {
                                            lineThickness = 1;
                                        }
                                    }
                                }
                                else
                                {
                                    try
                                    {
                                        if (!char.IsLetter(s[s.Length - 1]))
                                        {
                                            s = s + "ex";
                                        }
                                        lineThickness = AttributeBuilder.FontWidth(emHeight, DPI, s, 1);
                                    }
                                    catch
                                    {
                                        lineThickness = 1;
                                    }
                                }
                            }
                            if (attributes == null)
                            {
                                attributes = new FractionAttributes();
                            }
                            attributes.lineThickness = lineThickness;
                        }
                    }
                    else if (attribute.name == "numalign")
                    {
                        if (s.Length > 0)
                        {
                            if (attributes == null)
                            {
                                attributes = new FractionAttributes();
                            }
                            if (s.ToUpper() == "LEFT")
                            {
                                attributes.numAlign = FractionAlign.LEFT;
                            }
                            else if (s.ToUpper() == "CENTER")
                            {
                                attributes.numAlign = FractionAlign.CENTER;
                            }
                            else if (s.ToUpper() == "RIGHT")
                            {
                                attributes.numAlign = FractionAlign.RIGHT;
                            }
                        }
                    }
                    else if (attribute.name == "denomalign")
                    {
                        if (s.Length > 0)
                        {
                            if (attributes == null)
                            {
                                attributes = new FractionAttributes();
                            }
                            if (s.ToUpper() == "LEFT")
                            {
                                attributes.denomAlign = FractionAlign.LEFT;
                            }
                            else if (s.ToUpper() == "CENTER")
                            {
                                attributes.denomAlign = FractionAlign.CENTER;
                            }
                            else if (s.ToUpper() == "RIGHT")
                            {
                                attributes.denomAlign = FractionAlign.RIGHT;
                            }
                        }
                    }
                    else if ((attribute.name == "bevelled") && (s.Length > 0))
                    {
                        if (attributes == null)
                        {
                            attributes = new FractionAttributes();
                        }
                        if (s.ToUpper() == "TRUE")
                        {
                            attributes.isBevelled = true;
                        }
                        else
                        {
                            attributes.isBevelled = false;
                        }
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return attributes;
        }

        public static void ApplyAttrs(Node node, FractionAttributes fractionAttributes)
        {
            if (((node != null) && (node.type_ != null)) && ((node.type_.type == ElementType.Mfrac) && (fractionAttributes != null)))
            {
                if (fractionAttributes.isBevelled)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("bevelled", "true");
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("bevelled");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (fractionAttributes.lineThickness != 1)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("linethickness", fractionAttributes.lineThickness.ToString());
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("linethickness");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (fractionAttributes.denomAlign != FractionAlign.CENTER)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (fractionAttributes.denomAlign == FractionAlign.LEFT)
                    {
                        node.attrs.Add("denomalign", "left");
                    }
                    else if (fractionAttributes.denomAlign == FractionAlign.RIGHT)
                    {
                        node.attrs.Add("denomalign", "right");
                    }
                    else
                    {
                        node.attrs.Add("denomalign", "center");
                    }
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("denomalign");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (fractionAttributes.numAlign != FractionAlign.CENTER)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (fractionAttributes.numAlign == FractionAlign.LEFT)
                    {
                        node.attrs.Add("numalign", "left");
                    }
                    else if (fractionAttributes.numAlign == FractionAlign.RIGHT)
                    {
                        node.attrs.Add("numalign", "right");
                    }
                    else
                    {
                        node.attrs.Add("numalign", "center");
                    }
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("numalign");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
            }
        }

        public static ActionAttributes ActionAttributes(Node node)
        {
            Nodes.Attribute attribute = null;
            ActionAttributes attributes = null;
            try
            {
                if (node.attrs == null)
                {
                    return attributes;
                }
                node.attrs.Reset();
                for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                {
                    string s = attribute.val.Trim();
                    if (attribute.name == "actiontype")
                    {
                        if (s.Length > 0)
                        {
                            ActionType actionType = ActionType.StatusLine;
                            if (s.ToUpper() == "STATUSLINE")
                            {
                                actionType = ActionType.StatusLine;
                            }
                            else if (s.ToUpper() == "TOOLTIP")
                            {
                                actionType = ActionType.ToolTip;
                            }
                            else if (s.ToUpper() == "HIGHLIGHT")
                            {
                                actionType = ActionType.Highlight;
                            }
                            else if (s.ToUpper() == "TOGGLE")
                            {
                                actionType = ActionType.Toggle;
                            }
                            else
                            {
                                actionType = ActionType.Unknown;
                            }
                            if (attributes == null)
                            {
                                attributes = new ActionAttributes();
                            }
                            attributes.actionType = actionType;
                            attributes.actionString = s;
                        }
                    }
                    else if ((attribute.name == "selection") && (s.Length > 0))
                    {
                        if (attributes == null)
                        {
                            attributes = new ActionAttributes();
                        }
                        attributes.selection = Convert.ToInt32(s.Trim());
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return attributes;
        }

        public static StyleAttributes StyleAttrsFromNode(Node node)
        {
            return AttributeBuilder.StyleAttrsFromNode(node, false);
        }

        public static StyleAttributes StyleAttrsFromNode(Node node, bool bMStyle)
        {
            Nodes.Attribute attribute = null;
            StyleAttributes attributes = null;
            try
            {
                if (node.attrs == null)
                {
                    return attributes;
                }
                node.attrs.Reset();
                for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                {
                    string s = attribute.val.Trim();
                    if (bMStyle && (attribute.name == "displaystyle"))
                    {
                        if (s.ToUpper() == "TRUE")
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.displayStyle = DisplayStyle.TRUE;
                        }
                        else if (s.ToUpper() == "FALSE")
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.displayStyle = DisplayStyle.FALSE;
                        }
                    }
                    else if (bMStyle && (attribute.name == "scriptlevel"))
                    {
                        s = s.Trim();
                        string levelStr = "";
                        int level = 0;
                        bool plus = false;
                        bool minus = false;
                        if ((s.Length > 0) && (s[0] == '+'))
                        {
                            plus = true;
                            levelStr = s.Substring(1, s.Length - 1);
                        }
                        else if ((s.Length > 0) && (s[0] == '-'))
                        {
                            minus = true;
                            levelStr = s.Substring(1, s.Length - 1);
                        }
                        else
                        {
                            levelStr = s;
                        }
                        try
                        {
                            level = Convert.ToInt32(levelStr);
                            if (level > 2)
                            {
                                level = 2;
                            }
                            if (level < 0)
                            {
                                level = 0;
                            }
                        }
                        catch
                        {
                            level = 0;
                        }
                        if (plus && (level == 1))
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.scriptLevel = ScriptLevel.PLUS_ONE;
                        }
                        else if (plus && (level == 2))
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.scriptLevel = ScriptLevel.PLUS_TWO;
                        }
                        else if (minus && (level == 1))
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.scriptLevel = ScriptLevel.MINUS_TWO;
                        }
                        else if (minus && (level == 2))
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.scriptLevel = ScriptLevel.MINUS_TWO;
                        }
                        else if ((!plus && !minus) && (level == 0))
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.scriptLevel = ScriptLevel.ZERO;
                        }
                        else if ((!plus && !minus) && (level == 1))
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.scriptLevel = ScriptLevel.ONE;
                        }
                        else if ((!plus && !minus) && (level == 2))
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.scriptLevel = ScriptLevel.TWO;
                        }
                    }
                    else if (attribute.name == "mathvariant")
                    {
                        if (s.Length > 0)
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            if (s == "fraktur")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = false;
                                attributes.isItalic = false;
                                attributes.isFractur = true;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "bold")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = true;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "bold-italic")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = true;
                                attributes.isItalic = true;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "bold-fraktur")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = true;
                                attributes.isItalic = false;
                                attributes.isFractur = true;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "bold-script")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = true;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = true;
                                attributes.isMonospace = false;
                            }
                            else if (s == "bold-sans-serif")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = true;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = true;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "italic")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = false;
                                attributes.isItalic = true;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "sans-serif-italic")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = false;
                                attributes.isItalic = true;
                                attributes.isFractur = false;
                                attributes.isSans = true;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "sans-serif-bold-italic")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = true;
                                attributes.isItalic = true;
                                attributes.isFractur = false;
                                attributes.isSans = true;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "double-struck")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = false;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = true;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "monospace")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = false;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = true;
                            }
                            else if (s == "script")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = false;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = true;
                                attributes.isMonospace = false;
                            }
                            else if (s == "sans-serif")
                            {
                                attributes.isNormal = false;
                                attributes.isBold = false;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = true;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else if (s == "normal")
                            {
                                attributes.isNormal = true;
                                attributes.isBold = false;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                            else
                            {
                                attributes.isNormal = false;
                                attributes.isBold = false;
                                attributes.isItalic = false;
                                attributes.isFractur = false;
                                attributes.isSans = false;
                                attributes.isDoubleStruck = false;
                                attributes.isScript = false;
                                attributes.isMonospace = false;
                            }
                        }
                    }
                    else if (attribute.name == "mathcolor")
                    {
                        if (s.Length > 0)
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.color = ColorTranslator.FromHtml(s);
                        }
                        attributes.hasColor = true;
                    }
                    else if (attribute.name == "mathbackground")
                    {
                        if (s.Length > 0)
                        {
                            if (attributes == null)
                            {
                                attributes = new StyleAttributes();
                            }
                            attributes.background = ColorTranslator.FromHtml(s);
                        }
                        attributes.hasBackground = true;
                    }
                    if ((attribute.name == "mathsize") && (s.Length > 0))
                    {
                        if (attributes == null)
                        {
                            attributes = new StyleAttributes();
                        }
                        attributes.size = s;
                        attributes.hasSize = true;
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return attributes;
        }

        public static void CascadeStyles(Node parentNode, Node node, StyleAttributes styleAttributes)
        {
            string fontSize;
            string mathSize;
            if ((node == null) || (styleAttributes == null))
            {
                return;
            }
            
            if (node.style_ == null)
            {
                string font = styleAttributes.FontToString();
                if (font == "")
                {
                    if (node.attrs != null)
                    {
                        Nodes.Attribute attribute = node.attrs.Get("mathvariant");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
                else
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (font.Length > 0)
                    {
                        node.attrs.Add("mathvariant", font);
                    }
                }
                
                if (styleAttributes.displayStyle == DisplayStyle.AUTOMATIC)
                {
                    if (node.attrs != null)
                    {
                        Nodes.Attribute attribute = node.attrs.Get("displaystyle");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
                else
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }

                    switch (styleAttributes.displayStyle)
                    {
                        case DisplayStyle.TRUE:
                        {
                            node.attrs.Add("displaystyle", "true");
                            break;
                        }
                        case DisplayStyle.FALSE:
                        {
                            node.attrs.Add("displaystyle", "false");
                            break;
                        }
                    }
                }
            }
            else
            {
                string fontToString = "";
                string styleFont = "";
                fontToString = styleAttributes.FontToString();
                styleFont = node.style_.FontToString();
                if ((fontToString.Length > 0) && (fontToString != styleFont))
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("mathvariant", fontToString);
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("mathvariant");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                
                bool topLevel = false;
                bool hasDisplayStyle = false;
                if (((parentNode != null) && (parentNode.type_ != null)) && (parentNode.type_.type == ElementType.Math))
                {
                    topLevel = true;
                }
                DisplayStyle displayStyle = styleAttributes.displayStyle;
                DisplayStyle styleDisplayStyle = node.style_.displayStyle;
                if ((topLevel && (parentNode != null)) &&
                    (((displayStyle == DisplayStyle.TRUE) && parentNode.displayStyle) ||
                     ((displayStyle == DisplayStyle.FALSE) && !parentNode.displayStyle)))
                {
                    hasDisplayStyle = true;
                }
                if (((displayStyle != styleDisplayStyle) && (displayStyle != DisplayStyle.AUTOMATIC)) && !hasDisplayStyle)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    switch (displayStyle)
                    {
                        case DisplayStyle.TRUE:
                        {
                            node.attrs.Add("displaystyle", "true");
                            break;
                        }
                        case DisplayStyle.FALSE:
                        {
                            node.attrs.Add("displaystyle", "false");
                            break;
                        }
                    }
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("displaystyle");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                
                hasDisplayStyle = false;
                ScriptLevel scriptLevel = styleAttributes.scriptLevel;
                ScriptLevel styleScriptLevel = node.style_.scriptLevel;
                if ((topLevel && (parentNode != null)) &&
                    ((((scriptLevel == ScriptLevel.ZERO) && (parentNode.scriptLevel_ == 0)) ||
                      ((scriptLevel == ScriptLevel.ONE) && (parentNode.scriptLevel_ == 1))) ||
                     ((scriptLevel == ScriptLevel.TWO) && (parentNode.scriptLevel_ == 2))))
                {
                    hasDisplayStyle = true;
                }
                if (((scriptLevel != styleScriptLevel) && (scriptLevel != ScriptLevel.NONE)) && !hasDisplayStyle)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    switch (scriptLevel)
                    {
                        case ScriptLevel.ZERO:
                        {
                            node.attrs.Add("scriptlevel", "0");
                            break;
                        }
                        case ScriptLevel.ONE:
                        {
                            node.attrs.Add("scriptlevel", "1");
                            break;
                        }
                        case ScriptLevel.TWO:
                        {
                            node.attrs.Add("scriptlevel", "2");
                            break;
                        }
                        case ScriptLevel.PLUS_ONE:
                        {
                            node.attrs.Add("scriptlevel", "+1");
                            break;
                        }
                        case ScriptLevel.PLUS_TWO:
                        {
                            node.attrs.Add("scriptlevel", "+2");
                            break;
                        }
                        case ScriptLevel.MINUS_ONE:
                        {
                            node.attrs.Add("scriptlevel", "-1");
                            break;
                        }
                        case ScriptLevel.MINUS_TWO:
                        {
                            node.attrs.Add("scriptlevel", "-2");
                            break;
                        }
                    }
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("scriptlevel");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                
                fontSize = "";
                string ownFontSize = "";
                fontSize = styleAttributes.FontSize();
                ownFontSize = node.style_.FontSize();
                if (fontSize != ownFontSize)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (fontSize.Length > 0)
                    {
                        node.attrs.Add("mathsize", fontSize);
                    }
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("mathsize");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (styleAttributes.color != node.style_.color)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("mathcolor", ColorTranslator.ToHtml(styleAttributes.color));
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("mathcolor");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (styleAttributes.background != node.style_.background)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("mathbackground", ColorTranslator.ToHtml(styleAttributes.background));
                    return;
                }
                if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("mathbackground");
                    if (attribute == null)
                    {
                        return;
                    }
                    node.attrs.Remove(attribute);
                }
                return;
            }
        
            if (styleAttributes.scriptLevel == ScriptLevel.NONE)
            {
                if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("scriptlevel");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
            }
            else
            {
                if (node.attrs == null)
                {
                    node.attrs = new AttributeList();
                }
                switch (styleAttributes.scriptLevel)
                {
                    case ScriptLevel.ZERO:
                    {
                        node.attrs.Add("scriptlevel", "0");
                        break;
                    }
                    case ScriptLevel.ONE:
                    {
                        node.attrs.Add("scriptlevel", "1");
                        break;
                    }
                    case ScriptLevel.TWO:
                    {
                        node.attrs.Add("scriptlevel", "2");
                        break;
                    }
                    case ScriptLevel.PLUS_ONE:
                    {
                        node.attrs.Add("scriptlevel", "+1");
                        break;
                    }
                    case ScriptLevel.PLUS_TWO:
                    {
                        node.attrs.Add("scriptlevel", "+2");
                        break;
                    }
                    case ScriptLevel.MINUS_ONE:
                    {
                        node.attrs.Add("scriptlevel", "-1");
                        break;
                    }
                    case ScriptLevel.MINUS_TWO:
                    {
                        node.attrs.Add("scriptlevel", "-2");
                        break;
                    }
                }
            }
        
            mathSize = "";
            mathSize = styleAttributes.FontSize();
            if (mathSize == "normal")
            {
                if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("mathsize");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
            }
            else
            {
                if (node.attrs == null)
                {
                    node.attrs = new AttributeList();
                }
                if (mathSize.Length > 0)
                {
                    node.attrs.Add("mathsize", mathSize);
                }
            }
            if (styleAttributes.color == Color.Black)
            {
                if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("mathcolor");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
            }
            else
            {
                if (node.attrs == null)
                {
                    node.attrs = new AttributeList();
                }
                node.attrs.Add("mathcolor", ColorTranslator.ToHtml(styleAttributes.color));
            }
            if (styleAttributes.background == Color.White)
            {
                if (node.attrs == null)
                {
                    return;
                }
                Nodes.Attribute attribute = node.attrs.Get("mathbackground");
                if (attribute == null)
                {
                    return;
                }
                node.attrs.Remove(attribute);
            }
            else
            {
                if (node.attrs == null)
                {
                    node.attrs = new AttributeList();
                }
                node.attrs.Add("mathbackground", ColorTranslator.ToHtml(styleAttributes.background));
            }
        }

        public static void ApplyMglyphAttrs(Node node)
        {
            Nodes.Attribute attribute = null;
            try
            {
                if (node.attrs == null)
                {
                    return;
                }

                node.attrs.Reset();
                for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                {
                    string attrval = attribute.val.Trim();
                    if (attribute.name == "fontfamily")
                    {
                        if (attrval.Length > 0)
                        {
                            node.fontFamily = attrval;
                        }
                    }
                    else if ((attribute.name == "index") && (attrval.Length > 0))
                    {
                        node.literalText = "" + ((char) ((ushort) Convert.ToInt32(attrval)));
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
        }

        public static QuoteAttributes QuoteAttributes(Node node)
        {
            Nodes.Attribute attribute = null;
            QuoteAttributes quoteAttributes = null;
            try
            {
                if (node.attrs == null)
                {
                    return quoteAttributes;
                }
                node.attrs.Reset();
                for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                {
                    string s = attribute.val.Trim();
                    if (quoteAttributes == null)
                    {
                        quoteAttributes = new QuoteAttributes();
                    }
                    if (attribute.name == "lquote")
                    {
                        if (s.Length > 0)
                        {
                            quoteAttributes.lquote = s;
                        }
                        else
                        {
                            quoteAttributes.lquote = "NONE";
                        }
                    }
                    else if (attribute.name == "rquote")
                    {
                        if (s.Length > 0)
                        {
                            quoteAttributes.rquote = s;
                        }
                        else
                        {
                            quoteAttributes.rquote = "NONE";
                        }
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return quoteAttributes;
        }

        public static int FontWidth(float emHeight, float DPI, string s, double dDefault)
        {
            int length = 0;
            bool hasThickness = false;
            bool hasEms = false;
            bool hasEx = false;
            bool hasPx = false;
            bool hasUnits = false;
            bool hasPercents = false;
            bool isInfinity = false;
            double scale = 0;
            double thickScale = 1;
            string suffix = "";
            string sAmount = "";
            double amount = 0;
            double val = 2;

            try
            {
                string thickStr;
                s = s.Trim();
                length = s.Length;
                if (length < 2)
                {
                    return (int) Math.Round(val, 0);
                }
                
                suffix = "";
                suffix = suffix + s[length - 2];
                suffix = suffix + s[length - 1];

                bool gotThickness = false;
                if ((thickStr = s) != null)
                {
                    if (thickStr == "infinity")
                    {
                        isInfinity = true;
                        gotThickness = true;
                    }
                    else if (thickStr == "veryverythinmathspace")
                    {
                        thickScale = 0.055555600672960281;
                        hasEms = true;
                        gotThickness = true;
                    }
                    else if (thickStr == "verythinmathspace")
                    {
                        thickScale = 0.11111100018024445;
                        hasThickness = true;
                        gotThickness = true;
                    }
                    else if (thickStr == "thinmathspace")
                    {
                        thickScale = 0.16666699945926666;
                        hasThickness = true;
                        gotThickness = true;
                    }
                    else if (thickStr == "mediummathspace")
                    {
                        thickScale = 0.22222200036048889;
                        hasThickness = true;
                        gotThickness = true;
                    }
                    else if (thickStr == "thickmathspace")
                    {
                        thickScale = 0.27777799963951111;
                        hasThickness = true;
                        gotThickness = true;
                    }
                    else if (thickStr == "verythickmathspace")
                    {
                        thickScale = 0.33333298563957214;
                        hasThickness = true;
                        gotThickness = true;
                    }
                    else if (thickStr == "veryverythickmathspace")
                    {
                        thickScale = 0.38888901472091675;
                        hasThickness = true;
                        gotThickness = true;
                    }
                }
            
                if (! gotThickness)
                {
                    if ((thickStr = suffix) != null)
                    {
                        if (thickStr == "em")
                        {
                            sAmount = s.Substring(0, length - 2);
                            hasEms = true;
                        }
                        else if (thickStr == "ex")
                        {
                            sAmount = s.Substring(0, length - 2);
                            hasEx = true;
                        }
                        else if (thickStr == "px")
                        {
                            sAmount = s.Substring(0, length - 2);
                            hasPx = true;
                        }
                        else if (thickStr == "in")
                        {
                            sAmount = s.Substring(0, length - 2);
                            scale = 25.4;
                            hasUnits = true;
                        }
                        else if (thickStr == "cm")
                        {
                            sAmount = s.Substring(0, length - 2);
                            scale = 10;
                            hasUnits = true;
                        }
                        else if (thickStr == "mm")
                        {
                            sAmount = s.Substring(0, length - 2);
                            scale = 1;
                            hasUnits = true;
                        }
                        else if (thickStr == "pt")
                        {
                            sAmount = s.Substring(0, length - 2);
                            scale = 0.352777777777552;
                            hasUnits = true;
                        }
                        else if (thickStr == "pc")
                        {
                            sAmount = s.Substring(0, length - 2);
                            scale = 4.2333333333306236;
                            hasUnits = true;
                        }
                        else
                        {
                            if (s[length] == '%')
                            {
                                sAmount = s.Substring(0, length - 1);
                                hasPercents = true;
                            }
                            else
                            {
                                sAmount = s;
                                hasPx = true;
                            }
                        }
                        
                    }
                    else
                    {
                        if (s[length] == '%')
                        {
                            sAmount = s.Substring(0, length - 1);
                            hasPercents = true;
                        }
                        else
                        {
                            sAmount = s;
                            hasPx = true;
                        }
                    }
                }
            
                sAmount = sAmount.Trim();
                if (sAmount.Length > 0)
                {
                    if (sAmount[0] == '.')
                    {
                        sAmount = "0" + sAmount;
                    }
                    else if (((sAmount.Length > 1) && (sAmount[0] == '-')) && (sAmount[1] == '.'))
                    {
                        sAmount = "-0" + sAmount.Substring(1, sAmount.Length - 2);
                    }
                    sAmount = sAmount.Replace(".", NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
                    try
                    {
                        amount = Convert.ToDouble(sAmount);
                    }
                    catch
                    {
                    }
                }
                else
                {
                    amount = 0;
                }

                if (hasThickness)
                {
                    val = thickScale * emHeight;
                }
                else if (hasPercents)
                {
                    val = (dDefault * amount) * 0.01;
                }
                else if (hasPx)
                {
                    val = amount;
                }
                else if (hasUnits)
                {
                    val = (amount * scale) * (((double) DPI) / 25.4);
                }
                else if (hasEms)
                {
                    val = amount * emHeight;
                }
                else if (hasEx)
                {
                    val = (amount * emHeight) * 0.5;
                }
                else if (isInfinity)
                {
                    val = 100000;
                }

                return (int) Math.Round(val, 0);
            }
            catch
            {
                return (int) Math.Round(dDefault, 0);
            }
        }

        public static int SizeByAttr(float emHeight, float DPI, Node node, string attrName, double dDefault)
        {
            try
            {
                if (node.attrs != null)
                {
                    string s = node.attrs.GetValue(attrName);
                    if ((s != null) && (s.Length > 0))
                    {
                        return AttributeBuilder.FontWidth(emHeight, DPI, s, dDefault);
                    }
                    return (int) dDefault;
                }
                return (int) dDefault;
            }
            catch
            {
                return (int) dDefault;
            }
        }

        public static void ApplyAttrs(Node node, ActionAttributes actionAttributes)
        {
            if (((node != null) && (node.type_ != null)) && ((node.type_.type == ElementType.Maction) && (actionAttributes != null)))
            {
                if (node.attrs == null)
                {
                    node.attrs = new AttributeList();
                }
                if (actionAttributes.actionType == ActionType.StatusLine)
                {
                    node.attrs.Add("actiontype", "statusline");
                }
                else if (actionAttributes.actionType == ActionType.Toggle)
                {
                    node.attrs.Add("actiontype", "toggle");
                }
                else if (actionAttributes.actionType == ActionType.Highlight)
                {
                    node.attrs.Add("actiontype", "highlight");
                }
                else if (actionAttributes.actionType == ActionType.ToolTip)
                {
                    node.attrs.Add("actiontype", "tooltip");
                }
                else if ((actionAttributes.actionType == ActionType.Unknown) && (actionAttributes.actionString.Length > 0))
                {
                    node.attrs.Add("actiontype", actionAttributes.actionString);
                }
                string s = actionAttributes.selection.ToString();
                if (s.Length > 0)
                {
                    node.attrs.Add("selection", s);
                }
            }
        }

        public static TableAttributes mtableAttributes(Node node)
        {
            Nodes.Attribute attribute = null;
            TableAttributes tableAttributes = null;
            try
            {
                if (node.attrs == null)
                {
                    return tableAttributes;
                }
                node.attrs.Reset();
                for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                {
                    string s = attribute.val.Trim();
                    if (attribute.name == "align")
                    {
                        if (s.Length > 0)
                        {
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            if (s.ToUpper() == "TOP")
                            {
                                tableAttributes.align = TableAlign.TOP;
                            }
                            else if (s.ToUpper() == "BOTTOM")
                            {
                                tableAttributes.align = TableAlign.BOTTOM;
                            }
                            else if (s.ToUpper() == "CENTER")
                            {
                                tableAttributes.align = TableAlign.CENTER;
                            }
                            else if (s.ToUpper() == "BASELINE")
                            {
                                tableAttributes.align = TableAlign.BASELINE;
                            }
                            else if (s.ToUpper() == "AXIS")
                            {
                                tableAttributes.align = TableAlign.AXIS;
                            }
                            else
                            {
                                tableAttributes.align = TableAlign.AXIS;
                            }
                        }
                    }
                    else if (attribute.name == "side")
                    {
                        if (s.Length > 0)
                        {
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            if (s.ToUpper() == "LEFT")
                            {
                                tableAttributes.side = Side.LEFT;
                            }
                            else if (s.ToUpper() == "RIGHT")
                            {
                                tableAttributes.side = Side.RIGHT;
                            }
                            else if (s.ToUpper() == "LEFTOVERLAP")
                            {
                                tableAttributes.side = Side.LEFTOVERLAP;
                            }
                            else if (s.ToUpper() == "RIGHTOVERLAP")
                            {
                                tableAttributes.side = Side.RIGHTOVERLAP;
                            }
                        }
                    }
                    else if (attribute.name == "minlabelspacing")
                    {
                        if (s.Length > 0)
                        {
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            tableAttributes.minlabelspacing = s.Trim();
                        }
                    }
                    else if (attribute.name == "rowalign")
                    {
                        if (s.Length > 0)
                        {
                            s = s.Trim();
                            string[] strings = s.Split(new char[] { ' ' }, 100);
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            int numAligns = 0;
                            for (int i = 0; i < strings.Length; i++)
                            {
                                if (((strings[i].ToUpper() == "TOP") || (strings[i].ToUpper() == "BOTTOM")) || (((strings[i].ToUpper() == "CENTER") || (strings[i].ToUpper() == "BASELINE")) || (strings[i].ToUpper() == "AXIS")))
                                {
                                    numAligns++;
                                }
                            }
                            tableAttributes.rowAligns = new RowAlign[numAligns];
                            if (numAligns > 0)
                            {
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    if (strings[i].ToUpper() == "TOP")
                                    {
                                        tableAttributes.rowAligns[i] = RowAlign.TOP;
                                    }
                                    else if (strings[i].ToUpper() == "BOTTOM")
                                    {
                                        tableAttributes.rowAligns[i] = RowAlign.BOTTOM;
                                    }
                                    else if (strings[i].ToUpper() == "CENTER")
                                    {
                                        tableAttributes.rowAligns[i] = RowAlign.CENTER;
                                    }
                                    else if (strings[i].ToUpper() == "BASELINE")
                                    {
                                        tableAttributes.rowAligns[i] = RowAlign.BASELINE;
                                    }
                                    else if (strings[i].ToUpper() == "AXIS")
                                    {
                                        tableAttributes.rowAligns[i] = RowAlign.AXIS;
                                    }
                                }
                            }
                            else
                            {
                                tableAttributes.rowAligns = new RowAlign[] { RowAlign.BASELINE };
                            }
                        }
                    }
                    else if (attribute.name == "columnalign")
                    {
                        if (s.Length > 0)
                        {
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            s = s.Trim();
                            string[] strings = s.Split(new char[] { ' ' }, 100);
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            int numAligns = 0;
                            for (int i = 0; i < strings.Length; i++)
                            {
                                if (((strings[i].ToUpper() == "LEFT") || (strings[i].ToUpper() == "CENTER")) || (strings[i].ToUpper() == "RIGHT"))
                                {
                                    numAligns++;
                                }
                            }
                            tableAttributes.colAligns = new HAlign[numAligns];
                            if (numAligns > 0)
                            {
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    if (strings[i].ToUpper() == "LEFT")
                                    {
                                        tableAttributes.colAligns[i] = HAlign.LEFT;
                                    }
                                    else if (strings[i].ToUpper() == "CENTER")
                                    {
                                        tableAttributes.colAligns[i] = HAlign.CENTER;
                                    }
                                    else if (strings[i].ToUpper() == "RIGHT")
                                    {
                                        tableAttributes.colAligns[i] = HAlign.RIGHT;
                                    }
                                }
                            }
                            else
                            {
                                tableAttributes.colAligns = new HAlign[] { HAlign.CENTER };
                            }
                        }
                    }
                    else if (attribute.name == "frame")
                    {
                        if (s.Length > 0)
                        {
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            if (s.ToUpper() == "SOLID")
                            {
                                tableAttributes.frame = TableLineStyle.SOLID;
                            }
                            else if (s.ToUpper() == "DASHED")
                            {
                                tableAttributes.frame = TableLineStyle.DASHED;
                            }
                            else
                            {
                                tableAttributes.frame = TableLineStyle.NONE;
                            }
                        }
                    }
                    else if (attribute.name == "framespacing")
                    {
                        if (s.Length > 0)
                        {
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            tableAttributes.framespacing = s;
                        }
                    }
                    else if (attribute.name == "rowspacing")
                    {
                        if (s.Length > 0)
                        {
                            s = s.Trim();
                            string[] strings = s.Split(new char[] { ' ' }, 100);
                            if (strings.Length > 0)
                            {
                                if (tableAttributes == null)
                                {
                                    tableAttributes = new TableAttributes();
                                }
                                tableAttributes.rowSpacing = new string[strings.Length];
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    tableAttributes.rowSpacing[i] = strings[i];
                                }
                            }
                        }
                    }
                    else if (attribute.name == "columnspacing")
                    {
                        if (s.Length > 0)
                        {
                            s = s.Trim();
                            string[] strings = s.Split(new char[] { ' ' }, 100);
                            if (strings.Length > 0)
                            {
                                if (tableAttributes == null)
                                {
                                    tableAttributes = new TableAttributes();
                                }
                                tableAttributes.colSpacing = new string[strings.Length];
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    tableAttributes.colSpacing[i] = strings[i];
                                }
                            }
                        }
                    }
                    else if (attribute.name == "rowlines")
                    {
                        if (s.Length > 0)
                        {
                            s = s.Trim();
                            string[] strings = s.Split(new char[] { ' ' }, 100);
                            int numLines = 0;
                            if (strings.Length > 0)
                            {
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    if (((strings[i].ToUpper() == "NONE") || (strings[i].ToUpper() == "SOLID")) || (strings[i].ToUpper() == "DASHED"))
                                    {
                                        numLines++;
                                    }
                                }
                            }
                            if (numLines > 0)
                            {
                                if (tableAttributes == null)
                                {
                                    tableAttributes = new TableAttributes();
                                }
                                tableAttributes.rowLines = new TableLineStyle[numLines];
                                int lineIndex = 0;
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    if (((strings[i].ToUpper() == "NONE") || (strings[i].ToUpper() == "SOLID")) || (strings[i].ToUpper() == "DASHED"))
                                    {
                                        if (strings[i].ToUpper() == "SOLID")
                                        {
                                            tableAttributes.rowLines[lineIndex] = TableLineStyle.SOLID;
                                            lineIndex++;
                                        }
                                        else if (strings[i].ToUpper() == "DASHED")
                                        {
                                            tableAttributes.rowLines[lineIndex] = TableLineStyle.DASHED;
                                            lineIndex++;
                                        }
                                        else if (strings[i].ToUpper() == "NONE")
                                        {
                                            tableAttributes.rowLines[lineIndex] = TableLineStyle.NONE;
                                            lineIndex++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (attribute.name == "columnlines")
                    {
                        if (s.Length > 0)
                        {
                            s = s.Trim();
                            string[] strings = s.Split(new char[] { ' ' }, 100);
                            int numLines = 0;
                            if (strings.Length > 0)
                            {
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    if (((strings[i].ToUpper() == "NONE") || (strings[i].ToUpper() == "SOLID")) || (strings[i].ToUpper() == "DASHED"))
                                    {
                                        numLines++;
                                    }
                                }
                            }
                            if (numLines > 0)
                            {
                                if (tableAttributes == null)
                                {
                                    tableAttributes = new TableAttributes();
                                }
                                tableAttributes.colLines = new TableLineStyle[numLines];
                                int lineIndex = 0;
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    if (((strings[i].ToUpper() == "NONE") || (strings[i].ToUpper() == "SOLID")) || (strings[i].ToUpper() == "DASHED"))
                                    {
                                        if (strings[i].ToUpper() == "SOLID")
                                        {
                                            tableAttributes.colLines[lineIndex] = TableLineStyle.SOLID;
                                            lineIndex++;
                                        }
                                        else if (strings[i].ToUpper() == "DASHED")
                                        {
                                            tableAttributes.colLines[lineIndex] = TableLineStyle.DASHED;
                                            lineIndex++;
                                        }
                                        else if (strings[i].ToUpper() == "NONE")
                                        {
                                            tableAttributes.colLines[lineIndex] = TableLineStyle.NONE;
                                            lineIndex++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (attribute.name == "equalrows")
                    {
                        if (s.Length > 0)
                        {
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            tableAttributes.equalRows = Convert.ToBoolean(s);
                        }
                    }
                    else if (attribute.name == "equalcolumns")
                    {
                        if (s.Length > 0)
                        {
                            if (tableAttributes == null)
                            {
                                tableAttributes = new TableAttributes();
                            }
                            tableAttributes.equalColumns = Convert.ToBoolean(s);
                        }
                    }
                    else if ((attribute.name == "displaystyle") && (s.Length > 0))
                    {
                        if (tableAttributes == null)
                        {
                            tableAttributes = new TableAttributes();
                        }
                        tableAttributes.displaystyle = Convert.ToBoolean(s);
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return tableAttributes;
        }

        public static void ApplyAttrs(Node node, TableAttributes tableAttributes)
        {
            if (((node != null) && (node.type_ != null)) && ((node.type_.type == ElementType.Mtable) && (tableAttributes != null)))
            {
                if (tableAttributes.align == TableAlign.TOP)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("align", "top");
                }
                else if (tableAttributes.align == TableAlign.BOTTOM)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("align", "bottom");
                }
                else if (tableAttributes.align == TableAlign.CENTER)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("align", "center");
                }
                else if (tableAttributes.align == TableAlign.BASELINE)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("align", "baseline");
                }
                else if ((tableAttributes.align == TableAlign.AXIS) && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("align");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (tableAttributes.side == Side.LEFT)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("side", "left");
                }
                else if (tableAttributes.side == Side.LEFTOVERLAP)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("side", "leftoverlap");
                }
                else if (tableAttributes.side == Side.RIGHTOVERLAP)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("side", "rightoverlap");
                }
                else if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("side");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                string s = "";
                for (int i = 0; i < tableAttributes.rowAligns.Length; i++)
                {
                    if (tableAttributes.rowAligns[i] == RowAlign.TOP)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "top";
                    }
                    else if (tableAttributes.rowAligns[i] == RowAlign.BOTTOM)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "bottom";
                    }
                    else if (tableAttributes.rowAligns[i] == RowAlign.CENTER)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "center";
                    }
                    else if (tableAttributes.rowAligns[i] == RowAlign.AXIS)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "axis";
                    }
                    else if (tableAttributes.rowAligns[i] == RowAlign.BASELINE)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "baseline";
                    }
                }
                if ((tableAttributes.rowAligns.Length == 1) && (s.Trim() == "baseline"))
                {
                    if (node.attrs != null)
                    {
                        Nodes.Attribute attribute = node.attrs.Get("rowalign");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
                else
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (s.Length > 0)
                    {
                        node.attrs.Add("rowalign", s);
                    }
                }
                string sAlign = "";
                for (int i = 0; i < tableAttributes.colAligns.Length; i++)
                {
                    if (tableAttributes.colAligns[i] == HAlign.LEFT)
                    {
                        if (i > 0)
                        {
                            sAlign = sAlign + " ";
                        }
                        sAlign = sAlign + "left";
                    }
                    else if (tableAttributes.colAligns[i] == HAlign.CENTER)
                    {
                        if (i > 0)
                        {
                            sAlign = sAlign + " ";
                        }
                        sAlign = sAlign + "center";
                    }
                    else if (tableAttributes.colAligns[i] == HAlign.RIGHT)
                    {
                        if (i > 0)
                        {
                            sAlign = sAlign + " ";
                        }
                        sAlign = sAlign + "right";
                    }
                }
                if ((tableAttributes.colAligns.Length == 1) && (sAlign.Trim() == "center"))
                {
                    if (node.attrs != null)
                    {
                        Nodes.Attribute attribute = node.attrs.Get("columnalign");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
                else
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (sAlign.Length > 0)
                    {
                        node.attrs.Add("columnalign", sAlign);
                    }
                }
                if (tableAttributes.equalRows)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("equalrows", "True");
                }
                else if (!tableAttributes.equalRows && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("equalrows");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (tableAttributes.equalColumns)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("equalcolumns", "True");
                }
                else if (!tableAttributes.equalColumns && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("equalcolumns");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (tableAttributes.displaystyle)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("displaystyle", "True");
                }
                else if (!tableAttributes.displaystyle && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("displaystyle");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if (tableAttributes.framespacing != "0.4em 0.5ex")
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("framespacing", tableAttributes.framespacing);
                }
                else if ((tableAttributes.framespacing == "0.4em 0.5ex") && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("framespacing");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if ((tableAttributes.rowSpacing.Length == 1) && (tableAttributes.rowSpacing[0] == "0.5ex"))
                {
                    if (node.attrs != null)
                    {
                        Nodes.Attribute attribute = node.attrs.Get("rowspacing");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
                else
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    string ss = "";
                    for (int i = 0; i < tableAttributes.rowSpacing.Length; i++)
                    {
                        if (i > 0)
                        {
                            ss = ss + " ";
                        }
                        ss = ss + tableAttributes.rowSpacing[i];
                    }
                    if (ss.Length > 0)
                    {
                        node.attrs.Add("rowspacing", ss);
                    }
                }
                if ((tableAttributes.colSpacing.Length == 1) && (tableAttributes.colSpacing[0] == "0.8em"))
                {
                    if (node.attrs != null)
                    {
                        Nodes.Attribute attribute = node.attrs.Get("columnspacing");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
                else
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    string ss = "";
                    for (int i = 0; i < tableAttributes.colSpacing.Length; i++)
                    {
                        if (i > 0)
                        {
                            ss = ss + " ";
                        }
                        ss = ss + tableAttributes.colSpacing[i];
                    }
                    if (ss.Length > 0)
                    {
                        node.attrs.Add("columnspacing", ss);
                    }
                }
                if (tableAttributes.frame == TableLineStyle.DASHED)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("frame", "dashed");
                }
                if (tableAttributes.frame == TableLineStyle.SOLID)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("frame", "solid");
                }
                if ((tableAttributes.frame == TableLineStyle.NONE) && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("frame");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                if ((tableAttributes.rowLines.Length == 1) && (tableAttributes.rowLines[0] == TableLineStyle.NONE))
                {
                    if (node.attrs != null)
                    {
                        Nodes.Attribute attribute = node.attrs.Get("rowlines");
                        if (attribute != null)
                        {
                            node.attrs.Remove(attribute);
                        }
                    }
                }
                else
                {
                    string ss = "";
                    for (int i = 0; i < tableAttributes.rowLines.Length; i++)
                    {
                        if (tableAttributes.rowLines[i] == TableLineStyle.DASHED)
                        {
                            if (i > 0)
                            {
                                ss = ss + " ";
                            }
                            ss = ss + "dashed";
                        }
                        else if (tableAttributes.rowLines[i] == TableLineStyle.SOLID)
                        {
                            if (i > 0)
                            {
                                ss = ss + " ";
                            }
                            ss = ss + "solid";
                        }
                        else if (tableAttributes.rowLines[i] == TableLineStyle.NONE)
                        {
                            if (i > 0)
                            {
                                ss = ss + " ";
                            }
                            ss = ss + "none";
                        }
                    }
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (ss.Length > 0)
                    {
                        node.attrs.Add("rowlines", ss);
                    }
                }
                if ((tableAttributes.colLines.Length == 1) && (tableAttributes.colLines[0] == TableLineStyle.NONE))
                {
                    if (node.attrs == null)
                    {
                        return;
                    }
                    Nodes.Attribute attribute = node.attrs.Get("columnlines");
                    if (attribute == null)
                    {
                        return;
                    }
                    node.attrs.Remove(attribute);
                }
                else
                {
                    string ss = "";
                    for (int i = 0; i < tableAttributes.colLines.Length; i++)
                    {
                        if (tableAttributes.colLines[i] == TableLineStyle.DASHED)
                        {
                            if (i > 0)
                            {
                                ss = ss + " ";
                            }
                            ss = ss + "dashed";
                        }
                        else if (tableAttributes.colLines[i] == TableLineStyle.SOLID)
                        {
                            if (i > 0)
                            {
                                ss = ss + " ";
                            }
                            ss = ss + "solid";
                        }
                        else if (tableAttributes.colLines[i] == TableLineStyle.NONE)
                        {
                            if (i > 0)
                            {
                                ss = ss + " ";
                            }
                            ss = ss + "none";
                        }
                    }
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (ss.Length > 0)
                    {
                        node.attrs.Add("columnlines", ss);
                    }
                }
            }
        }

        public static TableRowAttributes MRowAttributes(Node node)
        {
            Nodes.Attribute attribute = null;
            TableRowAttributes tableRowAttributes = null;
            try
            {
                if (node.attrs != null)
                {
                    node.attrs.Reset();
                    for (attribute = node.attrs.Next(); attribute != null; attribute = node.attrs.Next())
                    {
                        string s = attribute.val.Trim();
                        if (attribute.name == "rowalign")
                        {
                            if (s.Length > 0)
                            {
                                if (tableRowAttributes == null)
                                {
                                    tableRowAttributes = new TableRowAttributes();
                                }
                                if (s.ToUpper() == "TOP")
                                {
                                    tableRowAttributes.align = RowAlign.TOP;
                                }
                                else if (s.ToUpper() == "BOTTOM")
                                {
                                    tableRowAttributes.align = RowAlign.BOTTOM;
                                }
                                else if (s.ToUpper() == "CENTER")
                                {
                                    tableRowAttributes.align = RowAlign.CENTER;
                                }
                                else if (s.ToUpper() == "BASELINE")
                                {
                                    tableRowAttributes.align = RowAlign.BASELINE;
                                }
                                else if (s.ToUpper() == "AXIS")
                                {
                                    tableRowAttributes.align = RowAlign.AXIS;
                                }
                                else
                                {
                                    tableRowAttributes.align = RowAlign.UNKNOWN;
                                }
                            }
                        }
                        else if ((attribute.name == "columnalign") && (s.Length > 0))
                        {
                            if (tableRowAttributes == null)
                            {
                                tableRowAttributes = new TableRowAttributes();
                            }
                            s = s.Trim();
                            string[] strings = s.Split(new char[] { ' ' }, 100);
                            int numAligns = 0;
                            for (int i = 0; i < strings.Length; i++)
                            {
                                if (((strings[i].ToUpper() == "LEFT") || (strings[i].ToUpper() == "CENTER")) || (strings[i].ToUpper() == "RIGHT"))
                                {
                                    numAligns++;
                                }
                            }
                            tableRowAttributes.colAligns = new HAlign[numAligns];
                            if (numAligns > 0)
                            {
                                for (int i = 0; i < strings.Length; i++)
                                {
                                    if (strings[i].ToUpper() == "LEFT")
                                    {
                                        tableRowAttributes.colAligns[i] = HAlign.LEFT;
                                    }
                                    else if (strings[i].ToUpper() == "CENTER")
                                    {
                                        tableRowAttributes.colAligns[i] = HAlign.CENTER;
                                    }
                                    else if (strings[i].ToUpper() == "RIGHT")
                                    {
                                        tableRowAttributes.colAligns[i] = HAlign.RIGHT;
                                    }
                                }
                            }
                            else
                            {
                                tableRowAttributes.colAligns = new HAlign[] { HAlign.CENTER };
                            }
                        }
                    }
                }
                node.attrs.Reset();
            }
            catch
            {
            }
            return tableRowAttributes;
        }

        public static void ApplyAttributes(Node node, TableRowAttributes tableRowAttributes)
        {
            if ((((node != null) && (node.type_ != null)) && 
                ((node.type_.type == ElementType.Mtr) || (node.type_.type == ElementType.Mlabeledtr))) && (tableRowAttributes != null))
            {
                if (tableRowAttributes.align == RowAlign.TOP)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "top");
                }
                else if (tableRowAttributes.align == RowAlign.BOTTOM)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "bottom");
                }
                else if (tableRowAttributes.align == RowAlign.CENTER)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "center");
                }
                else if (tableRowAttributes.align == RowAlign.BASELINE)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "baseline");
                }
                else if (tableRowAttributes.align == RowAlign.AXIS)
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    node.attrs.Add("rowalign", "axis");
                }
                else if ((tableRowAttributes.align == RowAlign.UNKNOWN) && (node.attrs != null))
                {
                    Nodes.Attribute attribute = node.attrs.Get("rowalign");
                    if (attribute != null)
                    {
                        node.attrs.Remove(attribute);
                    }
                }
                string s = "";
                for (int i = 0; i < tableRowAttributes.colAligns.Length; i++)
                {
                    if (tableRowAttributes.colAligns[i] == HAlign.LEFT)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "left";
                    }
                    else if (tableRowAttributes.colAligns[i] == HAlign.CENTER)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "center";
                    }
                    else if (tableRowAttributes.colAligns[i] == HAlign.RIGHT)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "right";
                    }
                    else if (tableRowAttributes.colAligns[i] == HAlign.UNKNOWN)
                    {
                        if (i > 0)
                        {
                            s = s + " ";
                        }
                        s = s + "center";
                    }
                }
                if ((tableRowAttributes.colAligns.Length == 1) && (s.Trim() == "center"))
                {
                    if (node.attrs == null)
                    {
                        return;
                    }
                    Nodes.Attribute attribute = node.attrs.Get("columnalign");
                    if (attribute == null)
                    {
                        return;
                    }
                    node.attrs.Remove(attribute);
                }
                else
                {
                    if (node.attrs == null)
                    {
                        node.attrs = new AttributeList();
                    }
                    if (s.Length > 0)
                    {
                        node.attrs.Add("columnalign", s);
                    }
                }
            }
        }

    }
}

