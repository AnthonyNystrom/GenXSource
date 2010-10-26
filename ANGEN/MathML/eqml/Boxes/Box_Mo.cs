namespace Boxes
{
    using Attrs;
    using Nodes;
    using Operators;
    using System;
    using System.Drawing;

    public class Box_Mo : BaseBox
    {
        public Box_Mo()
        {
            this.totalSpacing = 0;
            this.lspace = 0;
            this.rspace = 0;
            this.isBracketed = false;
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode.prevSibling != null)
            {
                childNode.box.X = (childNode.prevSibling.box.X + childNode.prevSibling.box.Width) + this.lspace;
            }
            else
            {
                childNode.box.X = base.rect.x + this.lspace;
            }
            childNode.box.Y = (base.rect.y + base.rect.baseline) - childNode.box.Baseline;
        }

        public override void getSize(Node containerNode)
        {
            this.totalSpacing = 0;
            this.lspace = 0;
            this.rspace = 0;
            Form form = Form.UNKNOWN;
            int lspace = 0;
            int rspace = 0;
            bool hasLspace = false;
            bool hasRspace = false;
            bool hasForm = false;
            if (containerNode.attrs != null)
            {
                if ((containerNode.attrs.Get("lspace") != null) || (containerNode.attrs.Get("rspace") != null))
                {
                    float emHeight = base.painter_.FontSize(containerNode, containerNode.style_);
                    if (containerNode.attrs.Get("lspace") != null)
                    {
                        lspace = AttributeBuilder.SizeByAttr(emHeight, base.painter_.DpiX(), containerNode, "lspace", 0);
                        hasLspace = true;
                    }
                    if (containerNode.attrs.Get("rspace") != null)
                    {
                        rspace = AttributeBuilder.SizeByAttr(emHeight, base.painter_.DpiX(), containerNode, "rspace", 0);
                        hasRspace = true;
                    }
                }
                switch (containerNode.attrs.GetValue("form"))
                {
                    case "prefix":
                    {
                        form = Form.PREFIX;
                        hasForm = true;
                        break;
                    }
                    case "infix":
                    {
                        form = Form.INFIX;
                        hasForm = true;
                        break;
                    }
                    case "postfix":
                    {
                        form = Form.POSTFIX;
                        hasForm = true;
                        break;
                    }
                }
            }
            if (containerNode.numChildren > 0)
            {
                Node firstChild = null;
                firstChild = containerNode.firstChild;
                if ((firstChild.type_.type == ElementType.Entity) && (firstChild.glyph != null))
                {
                    if (((firstChild.glyph.Code == "02062") || (firstChild.glyph.Name == "0200B")) || (firstChild.glyph.Name == "02061"))
                    {
                        int w = 0;
                        int h = 0;
                        int b = 0;
                        try
                        {
                            double d = base.painter_.FontSize(containerNode, containerNode.style_);
                            w = Convert.ToInt32(Math.Round((double) (d * 0.0555556)));
                            h = Convert.ToInt32(Math.Round((double) (d * 0.8)));
                            b = Convert.ToInt32(Math.Round((double) (d * 0.6)));
                        }
                        catch
                        {
                        }
                        base.rect.width = Math.Max(w, 1);
                        base.rect.height = Math.Max(h, 5);
                        base.rect.baseline = Math.Max(b, 3);
                    }
                    else if (base.painter_.IsStretchy(containerNode))
                    {
                        base.rect.width = base.painter_.FencedWidth(false, containerNode, containerNode.firstChild.glyph.Code);
                    }
                    else if (!this.IsChildOfUnderOver(containerNode) && (!hasLspace || !hasRspace))
                    {
                        try
                        {
                            Operator op = null;
                            if ((containerNode.prevSibling != null) || (containerNode.nextSibling != null))
                            {
                                try
                                {
                                    if (!hasForm)
                                    {
                                        if ((containerNode.prevSibling != null) && (containerNode.nextSibling != null))
                                        {
                                            form = Form.INFIX;
                                        }
                                        else if ((containerNode.prevSibling == null) && (containerNode.nextSibling != null))
                                        {
                                            form = Form.PREFIX;
                                        }
                                        else if ((containerNode.prevSibling != null) && (containerNode.nextSibling == null))
                                        {
                                            form = Form.POSTFIX;
                                        }
                                    }
                                    op = base.painter_.operators.Indexer(firstChild.glyph.Code, "", form);
                                }
                                catch
                                {
                                }
                                if (op == null)
                                {
                                    op = new Operator();
                                }
                                if (op != null)
                                {
                                    double d = base.painter_.FontSize(containerNode, containerNode.style_);
                                    if (!hasLspace)
                                    {
                                        this.lspace = Convert.ToInt32(Math.Round(d * op.LSpace));
                                    }
                                    if (!hasRspace)
                                    {
                                        this.rspace = Convert.ToInt32(Math.Round(d * op.RSpace));
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    if (hasLspace)
                    {
                        this.lspace = lspace;
                    }
                    if (hasRspace)
                    {
                        this.rspace = rspace;
                    }
                    this.totalSpacing = this.lspace + this.rspace;
                    base.rect.width += this.totalSpacing;
                }
            }
            else if ((containerNode.literalText != null) && (containerNode.literalText.Length > 0))
            {
                base.painter_.MeasureBox(containerNode, containerNode.style_);
                if (!hasLspace || !hasRspace)
                {
                    try
                    {
                        Operator op = null;
                        string ltext = containerNode.literalText.Trim();
                        if (!hasForm)
                        {
                            if ((containerNode.prevSibling != null) && (containerNode.nextSibling != null))
                            {
                                form = Form.INFIX;
                            }
                            else if ((containerNode.prevSibling == null) && (containerNode.nextSibling != null))
                            {
                                form = Form.PREFIX;
                            }
                            else if ((containerNode.prevSibling != null) && (containerNode.nextSibling == null))
                            {
                                form = Form.POSTFIX;
                            }
                        }
                        op = base.painter_.operators.Indexer("", ltext, form);
                        if (op == null)
                        {
                            op = new Operator();
                        }
                        if (op != null)
                        {
                            double d = base.painter_.FontSize(containerNode, containerNode.style_);
                            if (!hasLspace)
                            {
                                this.lspace = Convert.ToInt32(Math.Round(d * op.LSpace));
                            }
                            if (!hasRspace)
                            {
                                this.rspace = Convert.ToInt32(Math.Round(d * op.RSpace));
                            }
                        }
                    }
                    catch
                    {
                    }
                }
                if (hasLspace)
                {
                    this.lspace = lspace;
                }
                if (hasRspace)
                {
                    this.rspace = rspace;
                }
                this.totalSpacing = this.lspace + this.rspace;
                base.rect.width += this.totalSpacing;
            }
            else
            {
                base.painter_.MeasureBox(containerNode, containerNode.style_, "X");
                if (hasLspace)
                {
                    this.lspace = lspace;
                }
                if (hasRspace)
                {
                    this.rspace = rspace;
                }
                this.totalSpacing = this.lspace + this.rspace;
                base.rect.width += this.totalSpacing;
            }
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            this.isBracketed = false;
            if ((printMode == PaintMode.BACKGROUND))
            {
                base.painter_.FillRectangle(node);
            }
            else if ((printMode == PaintMode.FOREGROUND))
            {
                int x = node.box.X;
                int width = node.box.Width;
                node.box.X = x + this.lspace;
                node.box.Width = (width - this.lspace) - this.rspace;
                try
                {
                    if (node.NotBlack())
                    {
                        color = node.style_.color;
                    }
                    if (base.painter_.IsStretchy(node))
                    {
                        if (base.painter_.DrawBracketed(node, color))
                        {
                            this.isBracketed = true;
                        }
                    }
                    else if ((node.literalText != null) && (node.literalText.Length > 0))
                    {
                        base.painter_.DrawString(node, node.style_, color);
                    }
                }
                catch
                {
                }
                node.box.X = x;
                node.box.Width = width;
            }
        }

        public override void setChildSize(Node childNode)
        {
            base.rect.width += childNode.box.Width;
            if (childNode.box.Baseline > base.rect.baseline)
            {
                base.rect.baseline = childNode.box.Baseline;
            }
            if ((childNode.box.Height - childNode.box.Baseline) > (base.rect.height - base.rect.baseline))
            {
                base.rect.height = base.rect.baseline + (childNode.box.Height - childNode.box.Baseline);
            }
        }

        private bool IsChildOfUnderOver(Node node)
        {
            bool r = false;
            try
            {
                if ((node.parent_ != null) &&
                    (((node.parent_.type_.type == ElementType.Munder) ||
                      (node.parent_.type_.type == ElementType.Munderover)) ||
                     (node.parent_.type_.type == ElementType.Mover)))
                {
                    if (node.prevSibling == null)
                    {
                        return r;
                    }
                    return true;
                }
                
                if ((((node.parent_ == null) || (node.parent_.type_.type != ElementType.Mrow)) ||
                     (node.parent_.parent_ == null)) ||
                    (((node.parent_.parent_.type_.type != ElementType.Munder) &&
                      (node.parent_.parent_.type_.type != ElementType.Munderover)) &&
                     (node.parent_.parent_.type_.type != ElementType.Mover)))
                {
                    return r;
                }
                
                if (((node.prevSibling != null) || (node.nextSibling != null)) || (node.parent_.prevSibling == null))
                {
                    return r;
                }
                return true;
            }
            catch
            {
                return r;
            }
        }

        public void updateRowSize(Node containerNode, int nRowH, int nRowB, int nBaseLine_Shift)
        {
            if ((((containerNode.type_.type == ElementType.Mo) && (containerNode.firstChild != null)) &&
                 ((containerNode.firstChild.type_.type == ElementType.Entity) &&
                  (containerNode.firstChild.glyph != null))) && base.painter_.IsStretchy(containerNode))
            {
                try
                {
                    bool isSymmetric = true;
                    bool hasMaxSize = false;
                    bool hasMinSize = false;
                    int maxSize = 0;
                    int minSize = 0;
                    try
                    {
                        if (containerNode.attrs != null)
                        {
                            if (containerNode.attrs.GetValue("symmetric").Trim().ToUpper() == "FALSE")
                            {
                                isSymmetric = false;
                            }
                            if (containerNode.attrs.Get("maxsize") != null)
                            {
                                hasMaxSize = true;
                                maxSize =
                                    AttributeBuilder.SizeByAttr(
                                        base.painter_.FontSize(containerNode, containerNode.style_),
                                        base.painter_.DpiX(), containerNode, "maxsize",
                                        (double) base.painter_.FontSize(containerNode, containerNode.style_));
                            }
                            if (containerNode.attrs.Get("minsize") != null)
                            {
                                hasMinSize = true;
                                minSize =
                                    AttributeBuilder.SizeByAttr(
                                        base.painter_.FontSize(containerNode, containerNode.style_),
                                        base.painter_.DpiX(), containerNode, "minsize",
                                        (double) base.painter_.FontSize(containerNode, containerNode.style_));
                            }
                        }
                    }
                    catch
                    {
                    }
                    if (isSymmetric)
                    {
                        int max = Math.Max((int) (nRowB - nBaseLine_Shift), (int) ((nRowH - nRowB) + nBaseLine_Shift));
                        if (hasMaxSize && ((2 * max) > maxSize))
                        {
                            base.rect.height = maxSize;
                            base.rect.baseline = (maxSize / 2) + nBaseLine_Shift;
                        }
                        else if (hasMinSize && ((2 * max) < minSize))
                        {
                            base.rect.height = minSize;
                            base.rect.baseline = (minSize / 2) + nBaseLine_Shift;
                        }
                        else
                        {
                            base.rect.height = 2 * max;
                            base.rect.baseline = max + nBaseLine_Shift;
                        }
                    }
                    else if (hasMaxSize && (nRowH > maxSize))
                    {
                        base.rect.height = maxSize;
                        base.rect.baseline = nRowB - ((nRowH - maxSize) / 2);
                    }
                    else if (hasMinSize && (nRowH < minSize))
                    {
                        base.rect.height = minSize;
                        base.rect.baseline = nRowB - ((nRowH - minSize) / 2);
                    }
                    else
                    {
                        base.rect.height = nRowH;
                        base.rect.baseline = nRowB;
                    }
                    this.getSize(containerNode);
                }
                catch
                {
                }
            }
        }


        public int totalSpacing;
        public int lspace;
        public int rspace;
        public bool isBracketed;
    }
}

