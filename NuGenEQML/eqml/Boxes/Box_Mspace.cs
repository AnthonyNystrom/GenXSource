namespace Boxes
{
    using Attrs;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_Mspace : BaseBox
    {
        public Box_Mspace()
        {
            this.width = 2;
            this.height = 2;
            this.depth = 0;
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
        }

        public override void getSize(Node containerNode)
        {
            int d = 0;
            float dpi = base.painter_.DpiX();
            float f = base.painter_.FontSize(containerNode, containerNode.style_);
            base.rect.width = AttributeBuilder.SizeByAttr(f, dpi, containerNode, "width", this.width);
            base.rect.height = AttributeBuilder.SizeByAttr(f, dpi, containerNode, "height", this.height);
            d = AttributeBuilder.SizeByAttr(f, dpi, containerNode, "depth", this.depth);
            if (base.rect.height == 0)
            {
                base.rect.height = 2;
            }
            if (base.rect.width == 0)
            {
                base.rect.width = 2;
            }
            if (d == 0)
            {
                base.rect.baseline = base.rect.height / 2;
            }
            else if (d > base.rect.height)
            {
                base.rect.height += d;
                base.rect.baseline = 0;
            }
            else
            {
                base.rect.baseline = base.rect.height - d;
            }
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if ((printMode == PaintMode.BACKGROUND))
            {
                base.painter_.FillRectangle(node);
            }
        }

        public override void setChildSize(Node childNode)
        {
        }


        private double width;
        private double height;
        private double depth;
    }
}

