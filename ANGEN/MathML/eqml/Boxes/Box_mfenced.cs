namespace Boxes
{
    using Attrs;
    using Facade;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_mfenced : BaseBox
    {
        public Box_mfenced()
        {
            this.attrs = null;
            this.openWidth = 0;
            this.closeWidth = 0;
            this.leading = 0;
            this.baseline = 0;
            this.isFenced = false;
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode.prevSibling != null)
            {
                childNode.box.X = (childNode.prevSibling.box.X + childNode.prevSibling.box.Width) + this.SeparatorWidth(childNode);
            }
            else
            {
                childNode.box.X = base.rect.x + this.openWidth;
            }
            childNode.box.Y = (base.rect.y + base.rect.baseline) - childNode.box.Baseline;
        }

        public override void getSize(Node containerNode)
        {
            base.painter_.roundOpFontSize(containerNode, containerNode.style_);
            if (this.attrs == null)
            {
                this.attrs = new FencedAttributes();
            }
            if (containerNode.numChildren == 0)
            {
                base.painter_.MeasureBox(containerNode, containerNode.style_, "X");
            }
            else if (this.isFenced)
            {
                if (this.baseline == 0)
                {
                    this.baseline = base.painter_.MeasureBaseline(containerNode, containerNode.style_, "X");
                }
                base.rect.height = 2 * Math.Max((int) (base.rect.accent + base.painter_.CenterHeight(containerNode)), (int) (base.rect.baseline - base.painter_.CenterHeight(containerNode)));
                base.rect.baseline = (2 * Math.Max((int) (base.rect.accent + base.painter_.CenterHeight(containerNode)), (int) (base.rect.baseline - base.painter_.CenterHeight(containerNode))) / 2) + base.painter_.CenterHeight(containerNode);
            }
            if (this.attrs != null)
            {
                if (this.attrs.open.Length > 0)
                {
                    if (this.attrs.open != "NONE")
                    {
                        this.openWidth = base.painter_.OpWidth(true, containerNode, this.attrs.open);
                    }
                }
                else
                {
                    this.openWidth = 0;
                }
                if (this.attrs.close.Length > 0)
                {
                    if (this.attrs.close == "NONE")
                    {
                        this.closeWidth = 0;
                    }
                    else
                    {
                        this.closeWidth = base.painter_.OpWidth(true, containerNode, this.attrs.close);
                    }
                }
                else
                {
                    this.closeWidth = 0;
                }
            }
            base.rect.width += this.openWidth + this.closeWidth;
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if ((printMode == PaintMode.BACKGROUND))
            {
                base.painter_.FillRectangle(node);
            }
            else if ((printMode == PaintMode.FOREGROUND))
            {
                if (node.NotBlack())
                {
                    color = node.style_.color;
                }
                
                if (this.attrs != null)
                {
                    if (this.attrs.open.Length > 0)
                    {
                        base.painter_.DrawFence(node, this.attrs.open, color);
                    }
                    if (this.attrs.close.Length > 0)
                    {
                        int x = node.box.X;
                        int w = node.box.Width;
                        try
                        {
                            node.box.X = (x + w) - this.closeWidth;
                            node.box.Width = this.closeWidth;
                            base.painter_.DrawFence(node, this.attrs.close, color);
                        }
                        catch
                        {
                        }
                        node.box.X = x;
                        node.box.Width = w;
                    }
                }
                int b = base.painter_.MeasureBaseline(node, node.style_, ",");
                NodesList nodesList = node.GetChildrenNodes();
                if (nodesList != null)
                {
                    Node next = nodesList.Next();
                    if (next != null)
                    {
                        next = nodesList.Next();
                    }
                    while (next != null)
                    {
                        int xoff = 2;
                        try
                        {
                            xoff = this.leading / 3;
                        }
                        catch
                        {
                        }
                        if (this.Separator(next) != "")
                        {
                            base.painter_.DrawString(next, next.style_, (next.box.X - this.SeparatorWidth(next)) + xoff, (base.rect.y + base.rect.baseline) - b, 0, 0, this.Separator(next), color);
                        }
                        next = nodesList.Next();
                    }
                }
            }
        }

        public override void setChildSize(Node childNode)
        {
            if (childNode.childIndex == 0)
            {
                this.isFenced = false;
                this.attrs = AttributeBuilder.FencedAttrsFromNode(childNode.parent_);
                if (this.attrs != null)
                {
                    if (this.IsFence(this.attrs.open) || this.IsFence(this.attrs.close))
                    {
                        this.isFenced = true;
                    }
                }
                else
                {
                    this.isFenced = true;
                }
                this.baseline = base.painter_.MeasureBaseline(childNode.parent_, childNode.parent_.style_, "X");
                this.leading = (int) Math.Round((double) (this.baseline * 0.4));
            }
            base.rect.width += childNode.box.Width;
            if (childNode.prevSibling != null)
            {
                base.rect.width += this.SeparatorWidth(childNode);
            }
            if (childNode.box.Baseline > base.rect.baseline)
            {
                base.rect.baseline = childNode.box.Baseline;
            }
            if ((childNode.box.Height - childNode.box.Baseline) > base.rect.accent)
            {
                base.rect.accent = childNode.box.Height - childNode.box.Baseline;
            }
            base.rect.height = base.rect.baseline + base.rect.accent;
            base.Skip(childNode, true);
        }

        private int SeparatorWidth(Node node)
        {
            if ((this.attrs != null) && (this.attrs.separators != null))
            {
                try
                {
                    if (this.attrs.separators == "NONE")
                    {
                        return 0;
                    }
                    if (((node.childIndex - 1) >= 0) && ((node.childIndex - 1) < this.attrs.separators.Length))
                    {
                        return (this.leading + base.painter_.MeasureWidth(node, node.style_, "" + this.attrs.separators[node.childIndex - 1]));
                    }
                    if (this.attrs.separators.Length > 0)
                    {
                        return (this.leading + base.painter_.MeasureWidth(node, node.style_, "" + this.attrs.separators[this.attrs.separators.Length - 1]));
                    }
                    return (this.leading + base.painter_.MeasureWidth(node, node.style_, ","));
                }
                catch
                {
                    return (this.leading + base.painter_.MeasureWidth(node, node.style_, ","));
                }
            }
            return (this.leading + base.painter_.MeasureWidth(node, node.style_, ","));
        }

        private string Separator(Node node)
        {
            if ((this.attrs != null) && (this.attrs.separators != null))
            {
                try
                {
                    if (this.attrs.separators == "NONE")
                    {
                        return "";
                    }
                    if (((node.childIndex - 1) >= 0) && ((node.childIndex - 1) < this.attrs.separators.Length))
                    {
                        return ("" + this.attrs.separators[node.childIndex - 1]);
                    }
                    if (this.attrs.separators.Length > 0)
                    {
                        return ("" + this.attrs.separators[this.attrs.separators.Length - 1]);
                    }
                    return ",";
                }
                catch
                {
                    return ",";
                }
            }
            return ",";
        }

        private bool IsFence(string op)
        {
            op = op.Trim();
            if ((op.Length <= 0) ||
                (((((op != "{") && (op != "[")) && ((op != "(") && (op != "|"))) &&
                  (((op != "}") && (op != "]")) && ((op != ")") && (op[0] != '\u2329')))) &&
                 (((op[0] != '<') && (op[0] != '<')) && (((op[0] != '\u232a') && (op[0] != '>')) && (op[0] != '>')))))
            {
                return false;
            }
            return true;
        }


        private FencedAttributes attrs;
        private int openWidth;
        private int closeWidth;
        private int leading;
        private int baseline;
        private bool isFenced;
    }
}

