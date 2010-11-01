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
        public Node()
        {
            firstChild = null;
            lastChild = null;
            nextSibling = null;
            prevSibling = null;
            parent_ = null;
            upperNode = null;
            lowerNode = null;
            glyph = null;
            namespaceURI = "";
            isVisible = true;
            isGlyph = false;
            skip = false;
            literalCaret = 0;
            literalStart = 0;
            isAppend = false;
            fontFamily = "";
            tokenType = Tokens.ID;
            yOffset = 0;
            displayStyle = false;
            scriptLevel_ = 0;

            style_ = null;
            literalText = "";
            type_ = null;
            attrs = null;
            tagDeleted = false;
        }

        public Node(StyleAttributes styleAttributes)
        {
            firstChild = null;
            lastChild = null;
            nextSibling = null;
            prevSibling = null;
            parent_ = null;
            upperNode = null;
            lowerNode = null;
            glyph = null;
            namespaceURI = "";
            isVisible = true;
            isGlyph = false;
            skip = false;
            literalCaret = 0;
            literalStart = 0;
            isAppend = false;
            fontFamily = "";
            tokenType = Tokens.ID;
            yOffset = 0;
            displayStyle = false;
            scriptLevel_ = 0;
            style_ = null;
            literalText = "";
            type_ = null;
            attrs = null;
            tagDeleted = false;
            if (styleAttributes != null)
            {
                style_ = new StyleAttributes();
                styleAttributes.CopyTo(style_);
            }
        }

        public Node(string name)
        {
            firstChild = null;
            lastChild = null;
            nextSibling = null;
            prevSibling = null;
            parent_ = null;
            upperNode = null;
            lowerNode = null;
            glyph = null;
            namespaceURI = "";
            isVisible = true;
            isGlyph = false;
            skip = false;
            literalCaret = 0;
            literalStart = 0;
            isAppend = false;
            fontFamily = "";
            tokenType = Tokens.ID;
            yOffset = 0;
            displayStyle = false;
            scriptLevel_ = 0;
            style_ = null;
            literalText = "";
            type_ = null;
            attrs = null;
            tagDeleted = false;
            xmlTagName = name;
        }

        public Node(string name, StyleAttributes styleAttributes)
        {
            firstChild = null;
            lastChild = null;
            nextSibling = null;
            prevSibling = null;
            parent_ = null;
            upperNode = null;
            lowerNode = null;
            glyph = null;
            namespaceURI = "";
            isVisible = true;
            isGlyph = false;
            skip = false;
            literalCaret = 0;
            literalStart = 0;
            isAppend = false;
            fontFamily = "";
            tokenType = Tokens.ID;
            yOffset = 0;
            displayStyle = false;
            scriptLevel_ = 0;
            style_ = null;
            literalText = "";
            type_ = null;
            attrs = null;
            tagDeleted = false;
            xmlTagName = name;
            if (styleAttributes != null)
            {
                style_ = new StyleAttributes();
                styleAttributes.CopyTo(style_);
            }
        }

        public bool IsAtom()
        {
            if (this.tokenType == Tokens.TEXT || this.tokenType == Tokens.GLYPH || this.tokenType == Tokens.NUMBER)
            {
                return true;
            }
            return false;
        }

        public void CopyTo(Node node)
        {
            if (node != null)
            {
                node.firstChild = firstChild;
                node.lastChild = lastChild;
                node.xmlTagName = xmlTagName;
                node.namespaceURI = namespaceURI;
                node.childIndex = childIndex;
                node.level = level;
                node.numChildren = numChildren;
                node.isVisible = isVisible;
                node.isGlyph = isGlyph;
                node.skip = skip;

                node.InternalMark = InternalMark;
                node.literalText = literalText;
                node.LiteralStart = LiteralStart;
                node.IsAppend = IsAppend;
                
                node.tokenType = tokenType;
                node.yOffset = yOffset;
                node.displayStyle = displayStyle;

                node.glyph = glyph;

                node.scriptLevel_ = scriptLevel_;

                node.literalText = literalText;
                node.box = box;
                node.type_ = type_;
                if (attrs != null)
                {
                    node.attrs = new AttributeList();
                    attrs.CopyTo(node.attrs);
                }
                node.FontStyle = FontStyle;
                if (style_ != null)
                {
                    node.style_ = new StyleAttributes();
                    style_.CopyTo(node.style_);
                }
            }
        }

        public void CopyProperties(Node node)
        {
            if (node != null)
            {
                node.type_ = type_;
                if (attrs != null)
                {
                    node.attrs = new AttributeList();
                    attrs.CopyTo(node.attrs);
                }
                node.displayStyle = displayStyle;

                node.glyph = glyph;

                node.scriptLevel_ = scriptLevel_;

                node.xmlTagName = xmlTagName;
                node.namespaceURI = namespaceURI;
                node.skip = skip;
                node.isGlyph = isGlyph;
                node.isVisible = isVisible;
                node.FontStyle = FontStyle;
                if (style_ != null)
                {
                    node.style_ = new StyleAttributes();
                    style_.CopyTo(node.style_);
                }
            }
        }

        private int GuessLiteralStart()
        {
            int r = 0;
            try
            {
                if (InternalMark == 0)
                {
                    return 0;
                }
                if (InternalMark == LiteralLength)
                {
                    return box.Width;
                }
                double charW = 0;
                double w = 0;
                double literalLength = 0;
                w = box.Width;
                literalLength = LiteralLength;
                if ((w > 0) && (literalLength > 0))
                {
                    charW = w/literalLength;
                    r = Math.Min(Convert.ToInt32(Math.Round((double) (InternalMark*charW))), box.Width);
                }
            }
            catch
            {
            }
            return r;
        }

        public bool AdoptChild(Node ChildNode)
        {
            if (! ((type_ != null) && (ChildNode.type_ != null)))
            {
                return false;
            }
            Node node = lastChild;
            ChildNode.level = level + 1;
            if (node != null)
            {
                ChildNode.childIndex = node.childIndex + 1;
                ChildNode.prevSibling = node;
                ChildNode.nextSibling = null;
                ChildNode.parent_ = this;
                lastChild = ChildNode;
                node.nextSibling = ChildNode;
            }
            else
            {
                ChildNode.childIndex = 0;
                ChildNode.prevSibling = null;
                ChildNode.nextSibling = null;
                ChildNode.parent_ = this;
                firstChild = ChildNode;
                lastChild = ChildNode;
            }
            numChildren++;
            ChildNode.EmbraceParent();
            return true;
        }

        public bool PrependNode(Node node)
        {
            if (!(((parent_ != null) && (parent_.type_ != null)) && (node.type_ != null)))
            {
                return false;
            }
            Node nextSibling = this.nextSibling;
            Node prevSibling = this.prevSibling;
            
            Node parent = this.parent_;
            node.level = parent.level + 1;
            
            if (prevSibling == null)
            {
                node.childIndex = 0;
                node.prevSibling = null;
                node.nextSibling = this;
                node.parent_ = parent;
                parent.firstChild = node;
                this.prevSibling = node;
                childIndex++;
                while (nextSibling != null)
                {
                    nextSibling.childIndex++;
                    nextSibling = nextSibling.nextSibling;
                }
            }
            else
            {
                node.childIndex = prevSibling.childIndex + 1;
                node.prevSibling = prevSibling;
                node.nextSibling = this;
                node.parent_ = parent;
                prevSibling.nextSibling = node;
                this.prevSibling = node;
                childIndex++;
                while (nextSibling != null)
                {
                    nextSibling.childIndex++;
                    nextSibling = nextSibling.nextSibling;
                }
            }
            if (((prevSibling != null) && prevSibling.HasStyleClass()) && (prevSibling.StyleClass.Length > 0))
            {
                node.StyleClass = prevSibling.StyleClass;
            }
            node.EmbraceParent();
            parent.numChildren++;
            return true;
        }

        public bool AppendNode(Node node)
        {
            if (! (((parent_ != null) && (parent_.type_ != null)) && (node.type_ != null)))
            {
                return false;
            }
            Node nextSibling = this.nextSibling;
            Node parent = this.parent_;
            node.level = parent.level + 1;
            if (nextSibling == null)
            {
                node.childIndex = childIndex + 1;
                node.prevSibling = this;
                node.nextSibling = null;
                node.parent_ = parent;
                parent.lastChild = node;
                this.nextSibling = node;
            }
            else
            {
                node.childIndex = childIndex + 1;
                node.prevSibling = this;
                nextSibling.prevSibling = node;
                node.nextSibling = nextSibling;
                node.parent_ = parent;
                this.nextSibling = node;
                while (nextSibling != null)
                {
                    nextSibling.childIndex++;
                    nextSibling = nextSibling.nextSibling;
                }
            }
            if (HasStyleClass() && (StyleClass.Length > 0))
            {
                node.StyleClass = StyleClass;
            }
            node.EmbraceParent();
            parent.numChildren++;
            return true;
        }

        public void ReplaceChild(Node oldChild, Node newChild)
        {
            newChild.prevSibling = oldChild.prevSibling;
            newChild.nextSibling = oldChild.nextSibling;
            newChild.lowerNode = oldChild.lowerNode;
            newChild.upperNode = oldChild.upperNode;
            if (oldChild.prevSibling != null)
            {
                oldChild.prevSibling.nextSibling = newChild;
            }
            if (oldChild.nextSibling != null)
            {
                oldChild.nextSibling.prevSibling = newChild;
            }
            if (oldChild.upperNode != null)
            {
                oldChild.upperNode.lowerNode = newChild;
            }
            if (oldChild.lowerNode != null)
            {
                oldChild.lowerNode.upperNode = newChild;
            }
            if (firstChild == oldChild)
            {
                firstChild = newChild;
            }
            if (lastChild == oldChild)
            {
                lastChild = newChild;
            }
            newChild.level = oldChild.level;
            newChild.childIndex = oldChild.childIndex;
            newChild.displayStyle = displayStyle;

            newChild.glyph = glyph;

            newChild.scriptLevel_ = scriptLevel_;

            newChild.parent_ = this;
            newChild.EmbraceParent();
        }

        public bool HasChildren()
        {
            if (firstChild != null)
            {
                return true;
            }
            return false;
        }

        public NodesList GetChildrenNodes()
        {
            NodesList list = new NodesList();
            if (HasChildren())
            {
                Node node = firstChild;
                Node sibling = null;
                if (node != null)
                {
                    sibling = node.nextSibling;
                    list.Add(node);
                }
                while (sibling != null)
                {
                    list.Add(sibling);
                    sibling = sibling.nextSibling;
                }
            }
            return list;
        }

        private void PushdownStyleScript()
        {
            if ((type_ != null))
            {
                bool overrodeParentDispStyle = false;
                bool overrodeScriptLevel = false;
                bool overrodePlus = false;
                bool overrodeMinus = false;
                int plusValue = 0;
                int minusValue = 0;

                if (type_.type == ElementType.Math)
                {
                    scriptLevel_ = 0;
                }
                else if (style_ != null)
                {
                    DisplayStyle ownDispStyle = style_.displayStyle;
                    DisplayStyle parentDisplayStyle = DisplayStyle.AUTOMATIC;
                
                    if ((parent_ != null) && (parent_.style_ != null))
                    {
                        parentDisplayStyle = parent_.style_.displayStyle;
                    }
                    
                    if ((ownDispStyle != DisplayStyle.AUTOMATIC) && (ownDispStyle != parentDisplayStyle))
                    {
                        if (style_.displayStyle == DisplayStyle.TRUE)
                        {
                            displayStyle = true;
                            overrodeParentDispStyle = true;
                        }
                        else if (style_.displayStyle == DisplayStyle.FALSE)
                        {
                            displayStyle = false;
                            overrodeParentDispStyle = true;
                        }
                    }
                    else
                    {
                        displayStyle = parent_.displayStyle;
                    }
                    
                    ScriptLevel ownscriptLevel = style_.scriptLevel;
                    ScriptLevel parentScriptLevel = ScriptLevel.NONE;
                    
                    if ((parent_ != null) && (parent_.style_ != null))
                    {
                        parentScriptLevel = parent_.style_.scriptLevel;
                    }
                    if ((ownscriptLevel != ScriptLevel.NONE) && (ownscriptLevel != parentScriptLevel))
                    {
                        if (style_.scriptLevel == ScriptLevel.ZERO)
                        {
                            scriptLevel_ = 0;
                            overrodeScriptLevel = true;
                        }
                        else if (style_.scriptLevel == ScriptLevel.ONE)
                        {
                            scriptLevel_ = 1;
                            overrodeScriptLevel = true;
                        }
                        else if (style_.scriptLevel == ScriptLevel.TWO)
                        {
                            scriptLevel_ = 2;
                            overrodeScriptLevel = true;
                        }
                        else if (style_.scriptLevel == ScriptLevel.PLUS_ONE)
                        {
                            overrodePlus = true;
                            minusValue = 1;
                        }
                        else if (style_.scriptLevel == ScriptLevel.PLUS_TWO)
                        {
                            overrodePlus = true;
                            minusValue = 2;
                        }
                        else if (style_.scriptLevel == ScriptLevel.MINUS_ONE)
                        {
                            overrodeMinus = true;
                            plusValue = 1;
                        }
                        else if (style_.scriptLevel == ScriptLevel.MINUS_TWO)
                        {
                            overrodeMinus = true;
                            plusValue = 2;
                        }
                    }
                    else
                    {
                        scriptLevel_ = parent_.scriptLevel_;
                    }
                }

                if (((type_.type != ElementType.Math) && (parent_ != null)) && (parent_.type_ != null))
                {
                    if (!overrodeParentDispStyle)
                    {
                        displayStyle = parent_.displayStyle;
                    }
                    if (!overrodeScriptLevel)
                    {
                        scriptLevel_ = parent_.scriptLevel_;
                    }

                    if ((((parent_.type_.type == ElementType.Mover) ||
                          (parent_.type_.type == ElementType.Munder)) ||
                         ((parent_.type_.type == ElementType.Munderover) ||
                          (parent_.type_.type == ElementType.Msub))) ||
                        (((parent_.type_.type == ElementType.Msup) ||
                          (parent_.type_.type == ElementType.Mmultiscripts)) ||
                         (parent_.type_.type == ElementType.Msubsup)))
                    {
                        if (childIndex > 0)
                        {
                            if (!overrodeScriptLevel)
                            {
                                scriptLevel_ = parent_.scriptLevel_ + 1;
                            }
                            if (!overrodeParentDispStyle)
                            {
                                displayStyle = false;
                            }
                        }
                    }
                    else if (parent_.type_.type == ElementType.Mroot)
                    {
                        if ((childIndex > 0) && !overrodeScriptLevel)
                        {
                            scriptLevel_ = parent_.scriptLevel_ + 2;
                        }
                    }
                    else if (parent_.type_.type == ElementType.Mfrac)
                    {
                        if (parent_.displayStyle)
                        {
                            if (!overrodeScriptLevel)
                            {
                                scriptLevel_ = parent_.scriptLevel_;
                            }
                        }
                        else if (!overrodeScriptLevel)
                        {
                            scriptLevel_ = parent_.scriptLevel_ + 1;
                        }
                        if (!overrodeParentDispStyle)
                        {
                            displayStyle = false;
                        }
                    }
                }
                if (overrodePlus)
                {
                    scriptLevel_ += plusValue;
                    scriptLevel_ = Math.Min(scriptLevel_, 2);
                }
                if (overrodeMinus)
                {
                    scriptLevel_ -= minusValue;
                    scriptLevel_ = Math.Max(scriptLevel_, 0);
                }
            }

            NodesList list = GetChildrenNodes();
            for (Node child = list.Next(); child != null; child = list.Next())
            {
                child.PushdownStyleScript();
            }

            scriptLevel_ = Math.Min(scriptLevel_, 2);
            scriptLevel_ = Math.Max(scriptLevel_, 0);
        }

        public void MeasurePass(Painter g)
        {
            if (parent_ != null)
            {
                level = parent_.level + 1;
            }
            int x = 0;
            int y = 0;
            bool hasBox = false;
            if ((box != null))
            {
                hasBox = true;
                x = box.X;
                y = box.Y;
            }
            
            if ((((box == null) || ((level < 2))) || ((level == 2))) || ((level > 2)))
            {
                BoxBuilder.MakeBox(this, g);
            }
            if ((type_ != null) && (type_.type == ElementType.Math))
            {
                PushdownStyleScript();
            }

            if (hasBox)
            {
                box.X = x;
                box.Y = y;
            }
            
            
                NodesList list = GetChildrenNodes();
                Node n = list.Next();
                if ((style_ != null) && (style_.size.Length > 0))
                {
                    try
                    {
                        style_.scale = StyleAttributes.FontScale(style_.size, (double) g.GetSuitableFont(this, style_).SizeInPoints); 
                        style_.size = "";
                    }
                    catch
                    {
                    }
                }
                if ((style_ != null))
                {
                    if (type_.type == ElementType.Math)
                    {
                        style_.isTop = true;
                    }
                    else if (style_.canOverride)
                    {
                        style_.isTop = false;
                    }
                }
                while (n != null)
                {
                    if (style_ != null)
                    {
                        if (((n.style_ != null) && n.style_.canOverride) && n.IsSameStyleParent())
                        {
                            n.CombineStyles(n.style_, style_);
                        }
                        else if (n.type_.type == ElementType.Entity)
                        {
                            n.style_ = null;
                        }
                        else
                        {
                            try
                            {
                                StyleAttributes style = n.CascadeOverride(style_);
                                if (style != null)
                                {
                                    if (type_.type == ElementType.Math)
                                    {
                                        style.fontFamily = "";
                                        style.isUnderline = false;
                                    }
                                    n.style_ = new StyleAttributes();
                                    style.CopyTo(n.style_);
                                    if ((style_ != null) && style_.isTop)
                                    {
                                        n.style_.canOverride = false;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    n.MeasurePass(g);
                    box.setChildSize(n);
                    n = list.Next();
                }
            
            if (((((level < 2)) || ((level == 2))) || ((level > 2))))
            {
                box.getSize(this);
            }
        }

        public void CombineStyles(StyleAttributes first, StyleAttributes second)
        {
            try
            {
                if (style_ == null)
                {
                    return;
                }
                if (first.hasColor)
                {
                    style_.color = first.color;
                    style_.hasColor = true;
                }
                else
                {
                    style_.color = second.color;
                    style_.hasColor = false;
                }
                if (first.hasBackground)
                {
                    style_.background = first.background;
                    style_.hasBackground = true;
                }
                else
                {
                    style_.background = second.background;
                    style_.hasBackground = false;
                }
                if (first.hasSize)
                {
                    style_.scale = first.scale;
                    style_.size = first.size;
                    style_.hasSize = true;
                }
                else
                {
                    style_.scale = second.scale;
                    style_.size = second.size;
                    style_.hasSize = false;
                }
                if (first.IsStyled)
                {
                    style_.isBold = first.isBold;
                    style_.isItalic = first.isItalic;
                    style_.isUnderline = first.isUnderline;
                    style_.isNormal = first.isNormal;
                    style_.isMonospace = first.isMonospace;
                    style_.isScript = first.isScript;
                    style_.isSans = first.isSans;
                    style_.isFractur = first.isFractur;
                    style_.isDoubleStruck = first.isDoubleStruck;
                }
                else
                {
                    style_.isBold = second.isBold;
                    style_.isItalic = second.isItalic;
                    style_.isUnderline = second.isUnderline;
                    style_.isNormal = second.isNormal;
                    style_.isMonospace = second.isMonospace;
                    style_.isScript = second.isScript;
                    style_.isSans = second.isSans;
                    style_.isFractur = second.isFractur;
                    style_.isDoubleStruck = second.isDoubleStruck;
                }
                if (type_.type == ElementType.Math)
                {
                    style_.fontFamily = "";
                    style_.isUnderline = false;
                }
            }
            catch
            {
            }
        }

        public void PropogateYOffset()
        {
            if (yOffset != 0)
            {
                box.Y = box.Y + yOffset;
            }
            if ((yOffset != 0))
            {
                NodesList list = GetChildrenNodes();
                for (Node node = list.Next(); node != null; node = list.Next())
                {
                    if (yOffset != 0)
                    {
                        node.yOffset = yOffset;
                    }
                    node.PropogateYOffset();
                }
            }
            yOffset = 0;
        }

        public void PositionPass()
        {
            if (((box != null)))
            {
                box.SetPosition(this);

                NodesList list = GetChildrenNodes();
                for (Node node = list.Next(); node != null; node = list.Next())
                {
                    box.UpdateChildPosition(node);


                    node.PositionPass();
                }
            }
        }

        public void print(Rectangle rect, PaintMode printMode, Color MathColor)
        {
            try
            {
                if (box == null)
                {
                    return;
                }

                box.Draw(this, printMode, MathColor);

                NodesList nodesList = GetChildrenNodes();
                Node node = nodesList.Next();
                bool notOnWhite = false;

                while (node != null)
                {
                    if (!notOnWhite &&
                        (((type_.type == ElementType.Math) ||
                          (type_.type == ElementType.Mtd)) ||
                         (type_.type == ElementType.Mrow)))
                    {
                        notOnWhite = node.NotOnWhite();
                    }
                    if (node.isVisible && (node.type_.type != ElementType.Mphantom))
                    {
                        node.print(rect, printMode, MathColor);
                    }
                    node = nodesList.Next();
                }

                if ((printMode == PaintMode.BACKGROUND))
                {
                    if (!notOnWhite ||
                        (((type_.type != ElementType.Math) &&
                          (type_.type != ElementType.Mtd)) &&
                         (type_.type != ElementType.Mrow)))
                    {
                        return;
                    }
                    ((BaseBox) box).painter_.FillBackground(this);
                }
            }
            catch
            {
            }
        }
                
        public bool IsSameStyleParent()
        {
            if (parent_ != null)
            {
                if ((style_ != null) && (parent_.style_ == null))
                {
                    StyleAttributes styleAttributes = new StyleAttributes();
                    if (!styleAttributes.HasSameStyle(style_))
                    {
                        return true;
                    }
                }
                else if ((style_ == null) && (parent_.style_ != null))
                {
                    StyleAttributes styleAttributes = new StyleAttributes();
                    if (!styleAttributes.HasSameStyle(parent_.style_))
                    {
                        return true;
                    }
                }
                else if (((style_ != null) && (parent_.style_ != null)) &&
                         !parent_.style_.HasSameStyle(style_))
                {
                    if (parent_.type_.type == ElementType.Math)
                    {
                        StyleAttributes styleAttributes = new StyleAttributes();
                        if (styleAttributes.HasSameStyle(style_))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public bool IsSameStyle(Node node)
        {
            if ((style_ == null) && (node.style_ == null))
            {
                return true;
            }
            if ((style_ != null) && (node.style_ == null))
            {
                StyleAttributes styleAttributes = new StyleAttributes();
                if (styleAttributes.HasSameStyle(style_))
                {
                    return true;
                }
            }
            else if ((style_ == null) && (node.style_ != null))
            {
                StyleAttributes styleAttributes = new StyleAttributes();
                if (styleAttributes.HasSameStyle(node.style_))
                {
                    return true;
                }
            }
            else if (((style_ != null) && (node.style_ != null)) &&
                     style_.HasSameStyle(node.style_))
            {
                return true;
            }
            return false;
        }

        public bool HasStyleClass()
        {
            if (StyleClass.Length > 0)
            {
                return true;
            }
            return false;
        }

        public bool IsUnderlined()
        {
            if ((FontStyle & Nodes.FontStyle.UNDERLINE) == Nodes.FontStyle.UNDERLINE)
            {
                return true;
            }
            return false;
        }

        public bool NotBlack()
        {
            if (StyleColor != Color.Black)
            {
                return true;
            }
            return false;
        }

        public bool NotOnWhite()
        {
            if (Background != Color.White)
            {
                return true;
            }
            return false;
        }

        public Node NodeAtPoint(Point p, CaretLocation result)
        {
            NodesList list = GetChildrenNodes();
            for (Node n = list.Next(); n != null; n = list.Next())
            {
                if (n.GetCaretPos(p) != CaretPosition.None)
                {
                    return n.NodeAtPoint(p, result);
                }
            }

            result.pos = GetCaretPos(p);
            if (result.pos == CaretPosition.None)
            {
                return null;
            }
            return this;
        }

        private CaretPosition GetCaretPos(Point p)
        {
            try
            {
                if (box != null)
                {
                    if (((p.X >= box.X) &&
                         (p.X <= (box.X + (box.Width/2)))) &&
                        ((p.Y >= box.Y) && (p.Y <= (box.Y + box.Height))))
                    {
                        return CaretPosition.Left;
                    }
                    if (((p.X > (box.X + (box.Width/2))) &&
                         (p.X <= (box.X + box.Width))) &&
                        ((p.Y >= box.Y) && (p.Y <= (box.Y + box.Height))))
                    {
                        return CaretPosition.Right;
                    }
                    return CaretPosition.None;
                }
                return CaretPosition.None;
            }
            catch
            {
                return CaretPosition.None;
            }
        }
        //
        public void MarkFromPoint(Point p)
        {
            LiteralStart = -1*(p.X - box.X);
        }


        public bool IsAppend
        {
            get
            {
                if (InternalMark == LiteralLength)
                {
                    isAppend = true;
                }
                else
                {
                    isAppend = false;
                }
                return isAppend;
            }
            set
            {
                isAppend = value;
                if (isAppend)
                {
                    InternalMark = LiteralLength;
                }
                else
                {
                    InternalMark = 0;
                }
            }
        }


        public Node firstChild;
        public Node lastChild;
        public int childIndex;
        public int level;
        public int numChildren;
        public bool isVisible;
        public bool isGlyph;
        public bool skip;
        public int literalCaret;
        public int literalStart;
        private bool isAppend;
        public Node nextSibling;
        public string fontFamily;
        public Tokens tokenType;
        public int yOffset;
        public bool displayStyle;
        public int scriptLevel_;
        public Node prevSibling;
        public StyleAttributes style_;
        public string literalText;
        public IBox box;
        public global::Nodes.Type type_;
        public AttributeList attrs;
        public bool tagDeleted;
        public Node parent_;
        public Node upperNode;
        public Node lowerNode;
        public Glyph glyph;
        public string xmlTagName;
        public string namespaceURI;
    }
}