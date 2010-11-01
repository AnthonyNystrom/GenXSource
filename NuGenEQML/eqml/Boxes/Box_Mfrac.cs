namespace Boxes
{
    using Attrs;
    using Facade;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_Mfrac : BaseBox
    {
        public Box_Mfrac()
        {
            this.totalH = 0;
            this.baseline = 0;
            this.height = 0;
            this.width = 0;
            this.lineThick = 0;
            this.thick = 1;
            this.isBevelled = false;
            this.beveledFirstWidth = 0;
            this.beveledWidth = 0;
            this.firstChildHeight = 0;
            this.childHeight = 0;
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (!this.isBevelled)
            {
                if (childNode.prevSibling == null)
                {
                    childNode.box.Y = base.rect.y;
                    if (this.attrs != null)
                    {
                        if (this.attrs.numAlign == FractionAlign.LEFT)
                        {
                            childNode.box.X = base.rect.x + (this.width / 2);
                        }
                        else if (this.attrs.numAlign == FractionAlign.RIGHT)
                        {
                            childNode.box.X = ((base.rect.x + base.rect.width) - childNode.box.Width) - (this.width / 2);
                        }
                        else
                        {
                            childNode.box.X = base.rect.x + ((base.rect.width - childNode.box.Width) / 2);
                        }
                    }
                    else
                    {
                        childNode.box.X = base.rect.x + ((base.rect.width - childNode.box.Width) / 2);
                    }
                }
                else
                {
                    childNode.box.Y = (base.rect.y + base.rect.height) - childNode.box.Height;
                    if (this.attrs != null)
                    {
                        if (this.attrs.denomAlign == FractionAlign.LEFT)
                        {
                            childNode.box.X = base.rect.x + (this.width / 2);
                        }
                        else if (this.attrs.denomAlign == FractionAlign.RIGHT)
                        {
                            childNode.box.X = ((base.rect.x + base.rect.width) - childNode.box.Width) - (this.width / 2);
                        }
                        else
                        {
                            childNode.box.X = base.rect.x + ((base.rect.width - childNode.box.Width) / 2);
                        }
                    }
                    else
                    {
                        childNode.box.X = base.rect.x + ((base.rect.width - childNode.box.Width) / 2);
                    }
                }
            }
            else if (childNode.childIndex == 0)
            {
                childNode.box.X = base.rect.x + (this.width / 4);
                childNode.box.Y = (base.rect.y + base.rect.baseline) - childNode.box.Baseline;
            }
            else
            {
                childNode.box.X = ((base.rect.x + base.rect.width) - childNode.box.Width) - (this.width / 4);
                childNode.box.Y = (base.rect.y + base.rect.baseline) - childNode.box.Baseline;
            }
        }

        public override void getSize(Node containerNode)
        {
            if ((this.thick > 1) && this.isBevelled)
            {
                base.rect.width += this.thick;
            }
            if ((this.attrs != null) && this.attrs.isBevelled)
            {
                base.rect.width += this.width / 2;
            }
            else
            {
                base.rect.width += this.width;
            }
            if (!this.isBevelled)
            {
                base.rect.height = ((this.firstChildHeight + this.childHeight) + (this.lineThick * 4)) + this.thick;
                this.totalH = this.firstChildHeight + ((int) Math.Round(((this.lineThick * 4) + this.thick) * 0.5));
                base.rect.baseline = this.totalH + base.painter_.CenterHeight(containerNode);
            }
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if ((printMode == PaintMode.BACKGROUND))
            {
                base.painter_.FillRectangle(node);
            }
            else if ((printMode == PaintMode.FOREGROUND))
            {
                try
                {
                    if (node.NotBlack())
                    {
                        color = node.style_.color;
                    }
                }
                catch
                {
                }
                if ((this.attrs != null) && this.isBevelled)
                {
                    for (int i = 0; i < this.thick; i++)
                    {
                        base.painter_.DrawLine((((base.rect.x + this.beveledFirstWidth) + this.lineThick) + i) + (this.width / 4), (base.rect.y + base.rect.height) - (3 * this.lineThick), (((((base.rect.x + base.rect.width) - this.beveledWidth) - this.lineThick) - this.thick) + i) - (this.width / 4), base.rect.y + (3 * this.lineThick), 1, color);
                    }
                }
                else
                {
                    base.painter_.DrawLine(base.rect.x + (this.width / 4), base.rect.y + this.totalH, (base.rect.x + base.rect.width) - (this.width / 4), base.rect.y + this.totalH, this.thick, color);
                }
            }
        }

        public override void setChildSize(Node childNode)
        {
            if (childNode.childIndex == 0)
            {
                float emHeight = 0f;
                float dpi = base.painter_.DpiX();
                if (childNode.parent_ != null)
                {
                    emHeight = base.painter_.FontSize(childNode.parent_, childNode.parent_.style_);
                }
                else
                {
                    emHeight = base.painter_.FontSize(childNode, childNode.style_);
                }
                this.attrs = AttributeBuilder.FractionAttrsFromNode(emHeight, dpi, childNode.parent_);
                this.thick = base.painter_.LineThickness(childNode.parent_);
                if ((this.attrs != null) && (this.attrs.lineThickness > 1))
                {
                    this.thick = this.attrs.lineThickness * this.thick;
                }
                try
                {
                    this.baseline = base.painter_.MeasureBaseline(childNode.parent_, childNode.parent_.style_, "X");
                    this.height = base.painter_.MeasureHeight(childNode.parent_, childNode.parent_.style_, "X");
                    this.width = base.painter_.MeasureWidth(childNode.parent_, childNode.parent_.style_, "X");
                }
                catch
                {
                }
                if (this.height <= 0)
                {
                    this.height = 20;
                }
                if (this.baseline <= 0)
                {
                    this.baseline = 0x10;
                }
                if (this.width <= 0)
                {
                    this.width = 11;
                }
                this.lineThick = Convert.ToInt32(Math.Round((double) (this.height * 0.05)));
                this.lineThick = Math.Max(this.lineThick, 1);
            }
            if ((this.attrs != null) && this.attrs.isBevelled)
            {
                this.isBevelled = true;
            }
            if (!this.isBevelled)
            {
                base.rect.height += childNode.box.Height;
                if (childNode.box.Width > base.rect.width)
                {
                    base.rect.width = childNode.box.Width;
                }
                if (childNode.childIndex == 0)
                {
                    this.firstChildHeight = childNode.box.Height;
                }
                else
                {
                    this.childHeight = childNode.box.Height;
                }
                if ((childNode.childIndex == 0) && (childNode.nextSibling != null))
                {
                    childNode.lowerNode = childNode.nextSibling;
                    childNode.nextSibling.upperNode = childNode;
                }
            }
            else
            {
                base.rect.width += childNode.box.Width;
                if (childNode.childIndex == 0)
                {
                    this.beveledFirstWidth = childNode.box.Width;
                }
                else
                {
                    this.beveledWidth = childNode.box.Width;
                }
                if (childNode.box.Baseline > base.rect.baseline)
                {
                    base.rect.height += childNode.box.Baseline - base.rect.baseline;
                    base.rect.baseline = childNode.box.Baseline;
                }
                if ((childNode.box.Height - childNode.box.Baseline) > (base.rect.height - base.rect.baseline))
                {
                    base.rect.height += (childNode.box.Height - childNode.box.Baseline) - (base.rect.height - base.rect.baseline);
                }
                if (childNode.childIndex == 1)
                {
                    base.rect.width += (int) (base.rect.height * 0.33333333);
                }
            }
            base.Skip(childNode, true);
        }


        private int totalH;
        private int baseline;
        private int firstChildHeight;
        private int childHeight;
        private int height;
        private int width;
        private int lineThick;
        private int thick;
        private FractionAttributes attrs;
        private bool isBevelled;
        private int beveledFirstWidth;
        private int beveledWidth;
    }
}

