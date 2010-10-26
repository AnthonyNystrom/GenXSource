namespace Boxes
{
    using Facade;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_Text : BaseBox
    {
        public Box_Text()
        {
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
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if ((printMode == PaintMode.BACKGROUND))
            {
                if (node.isVisible)
                {
                    base.painter_.FillRectangle(node);
                }
            }
            else if ((printMode == PaintMode.FOREGROUND))
            {
                if ((node.isVisible && (node.literalText != null)) && (node.literalText.Length > 0))
                {
                    if (node.NotBlack())
                    {
                        color = node.style_.color;
                    }
                    base.painter_.DrawString(node, node.style_, color);
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

    }
}

