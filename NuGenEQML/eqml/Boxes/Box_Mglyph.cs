namespace Boxes
{
    using Attrs;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_Mglyph : BaseBox
    {
        public Box_Mglyph()
        {
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
        }

        public override void getSize(Node node)
        {
            AttributeBuilder.ApplyMglyphAttrs(node);
            base.painter_.MeasureBox(node, node.style_);
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if ((printMode == PaintMode.BACKGROUND))
            {
                base.painter_.FillRectangle(node);
            }
            else if (((printMode == PaintMode.FOREGROUND)) && ((node.literalText != null) && (node.literalText.Length > 0)))
            {
                base.painter_.DrawString(node, node.style_, color);
            }
        }

        public override void setChildSize(Node childNode)
        {
        }

    }
}

