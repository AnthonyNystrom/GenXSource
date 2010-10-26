namespace Boxes
{
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_none : BaseBox
    {
        public Box_none()
        {
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void getSize(Node containerNode)
        {
            containerNode.box.Width = 0;
            containerNode.box.Height = 0;
            containerNode.box.Baseline = 0;
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
        }

        public override void setChildSize(Node childNode)
        {
            childNode.isVisible = false;
        }

    }
}

