namespace Boxes
{
    using Attrs;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_Ms : BaseBox
    {
        public Box_Ms()
        {
            this.leftQuoteWidth = 0;
            this.rightQuoteWidth = 0;
            this.leftQuote = "\"";
            this.rightQuote = "\"";
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode.prevSibling != null)
            {
                childNode.box.X = childNode.prevSibling.box.X + childNode.prevSibling.box.Width;
            }
            else
            {
                childNode.box.X = base.rect.x;
            }
            childNode.box.Y = (base.rect.y + base.rect.baseline) - childNode.box.Baseline;
        }

        public override void getSize(Node containerNode)
        {
            BoxRect boxRect;
            this.attrs = AttributeBuilder.QuoteAttributes(containerNode);
            if (containerNode.numChildren <= 0)
            {
                if ((containerNode.literalText != null) && (containerNode.literalText.Length > 0))
                {
                    base.painter_.MeasureBox(containerNode, containerNode.style_);
                }
                else
                {
                    base.painter_.MeasureBox(containerNode, containerNode.style_, "X");
                }
            }
            if (this.attrs != null)
            {
                if (this.attrs.lquote != "NONE")
                {
                    this.leftQuote = this.attrs.lquote;
                }
                else
                {
                    this.leftQuote = "";
                }
                if (this.attrs.rquote != "NONE")
                {
                    this.rightQuote = this.attrs.rquote;
                }
                else
                {
                    this.rightQuote = "";
                }
            }
            else
            {
                this.leftQuote = "\"";
                this.rightQuote = "\"";
            }
            if (this.leftQuote.Length > 0)
            {
                boxRect = base.painter_.MeasureTextRect(containerNode, this.leftQuote, containerNode.scriptLevel_, containerNode.style_);
                this.leftQuoteWidth = boxRect.width;
            }
            else
            {
                this.leftQuoteWidth = 0;
            }
            if (this.leftQuote.Length > 0)
            {
                boxRect = base.painter_.MeasureTextRect(containerNode, this.rightQuote, containerNode.scriptLevel_, containerNode.style_);
                this.rightQuoteWidth = boxRect.width;
            }
            else
            {
                this.rightQuoteWidth = 0;
            }
            base.rect.width += this.leftQuoteWidth + this.rightQuoteWidth;
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if ((printMode == PaintMode.BACKGROUND))
            {
                base.painter_.FillRectangle(node);
            }
            else if ((printMode == PaintMode.FOREGROUND))
            {
                if (this.leftQuote.Length > 0)
                {
                    base.painter_.DrawQuote(node, node.box.X, node.box.Y, this.leftQuote, node.scriptLevel_, node.style_, color);
                }
                if ((node.isVisible && (node.literalText != null)) && (node.literalText.Length > 0))
                {
                    if (node.NotBlack())
                    {
                        color = node.style_.color;
                    }
                    int px = node.box.X;
                    node.box.X = node.box.X + this.leftQuoteWidth;
                    try
                    {
                        base.painter_.DrawString(node, node.style_, color);
                    }
                    catch
                    {
                    }
                    node.box.X = px;
                }
                if (this.rightQuote.Length > 0)
                {
                    base.painter_.DrawQuote(node, (node.box.X + node.box.Width) - this.rightQuoteWidth, node.box.Y, this.rightQuote, node.scriptLevel_, node.style_, color);
                }
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


        public QuoteAttributes attrs;
        public int leftQuoteWidth;
        private int rightQuoteWidth;
        public string leftQuote;
        private string rightQuote;
    }
}

