namespace Boxes
{
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_mlabeledtr : BaseBox
    {
        public Box_mlabeledtr()
        {
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
        }

        public override void getSize(Node containerNode)
        {
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if (((printMode == PaintMode.FOREGROUND)) && (node.firstChild == null))
            {
                base.painter_.OutlineRect(node);
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
            base.Skip(childNode, true);
        }

    }
}

