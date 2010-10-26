namespace Boxes
{
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_mphantom : BaseBox
    {
        public Box_mphantom()
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
            if (containerNode.firstChild == null)
            {
                containerNode.box.Width = 15;
                containerNode.box.Height = 15;
                containerNode.box.Baseline = 10;
            }
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            base.painter_.FillRectangle(node, Color.Gray);
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

