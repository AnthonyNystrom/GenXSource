namespace Boxes
{
    using Rendering;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_Msqrt : BaseBox
    {
        public Box_Msqrt()
        {
            this.spaceHeight_ = 0;
            this.tlinethick_ = 0;
            this.ftlineThick_ = 0;
            this.vthinSpace_ = 0;
            this.lineThick_ = 0;
            this.dlineThick_ = 0;
            this.flineThick_ = 0;
            this.elineThick_ = 0;
            this.twlineThick_ = 0;
            this.lthick2_ = 0;
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
                childNode.box.X = (base.rect.x + this.ftlineThick_);
            }
            childNode.box.Y = (base.rect.y + base.rect.baseline) - childNode.box.Baseline;
        }

        public override void getSize(Node containerNode)
        {
            this.spaceHeight_ = Convert.ToInt32(Math.Round((double) base.painter_.CalcSpaceHeight(Space.VeryThin, containerNode, containerNode.style_)));
            this.lineThick_ = base.painter_.LineThickness(containerNode);
            this.dlineThick_ = base.painter_.DoubleLineThickness(containerNode);
            this.tlinethick_ = 3 * this.lineThick_;
            this.lthick2_ = this.lineThick_;
            this.vthinSpace_ = Convert.ToInt32(Math.Round((double) base.painter_.CalcSpaceHeight(Space.VeryThin, containerNode, containerNode.style_)));
            this.flineThick_ = this.dlineThick_ * 2;
            this.flineThick_ = Math.Max(this.flineThick_, 3);
            this.elineThick_ = 2 * this.flineThick_;
            this.twlineThick_ = 3 * this.flineThick_;
            this.ftlineThick_ = this.twlineThick_ + this.dlineThick_;
            base.rect.width += ((this.ftlineThick_ + this.vthinSpace_)) + this.spaceHeight_;
            base.rect.height += this.tlinethick_;
            base.rect.baseline += this.tlinethick_;
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if ((printMode == PaintMode.BACKGROUND))
            {
                base.painter_.FillRectangle(node);
            }
            else if ((printMode == PaintMode.FOREGROUND))
            {
                int x1 = 0;
                int y1 = 0;
                int x2 = 0;
                int y2 = 0;
                int xx2 = 0;
                int yy2 = 0;
                if ((printMode == PaintMode.BACKGROUND))
                {
                    base.painter_.FillRectangle(node);
                }
                else if ((printMode == PaintMode.FOREGROUND))
                {
                    Point[] points;
                    if (node.NotBlack())
                    {
                        color = node.style_.color;
                    }
                    if (this.lineThick_ > 1)
                    {
                        x1 = (((base.rect.x) + this.flineThick_) - this.dlineThick_) + 1;
                        y1 = base.rect.y + (base.rect.height / 2);
                        x2 = (base.rect.x) + this.elineThick_;
                        y2 = base.rect.y + base.rect.height;
                        xx2 = (((base.rect.x ) + this.twlineThick_) + this.lineThick_) - 1;
                        yy2 = ((base.rect.y + this.lthick2_) + this.lineThick_) - 1;
                        double diff1 = this.diff(x1, y1, this.aspect(x1, y1, x2, y2));
                        double d = this.aspect(x2, y2, xx2, yy2);
                        double diff2 = this.diff(x2, y2, d);
                        int num11 = 0;
                        int num12 = 0;
                        double num13 = 0;
                        double num14 = 0;
                        num13 = (diff2 - diff1) / (this.aspect(x1, y1, x2, y2) - d);
                        num14 = (num13 * this.aspect(x1, y1, x2, y2)) + diff1;
                        num11 = Convert.ToInt32(Math.Round(num13));
                        num12 = Convert.ToInt32(Math.Round(num14));
                        Point point9 = new Point(x1, (y1 + this.lineThick_) - 1);
                        Point point8 = new Point(num11, num12);
                        diff1 = this.diff((x1 + this.dlineThick_) - 1, y1, this.aspect(x1, y1, x2, y2));
                        diff2 = this.diff((base.rect.x ) + this.twlineThick_, base.rect.y + this.lthick2_, d);
                        num13 = (diff2 - diff1) / (this.aspect(x1, y1, x2, y2) - d);
                        num14 = (num13 * this.aspect(x1, y1, x2, y2)) + diff1;
                        num11 = Convert.ToInt32(Math.Round(num13));
                        num12 = Convert.ToInt32(Math.Round(num14));
                        Point point3 = new Point(num11, num12);
                        Point point2 = new Point((x1 + this.dlineThick_) - 1, y1);
                        Point point10 = new Point(base.rect.x, ((base.rect.y + (base.rect.height / 2)) + this.lineThick_) - 1);
                        Point point11 = new Point(base.rect.x, base.rect.y + (base.rect.height / 2));
                        Point point1 = new Point(base.rect.x, base.rect.y + (base.rect.height / 2));
                        Point point4 = new Point((base.rect.x) + this.twlineThick_, base.rect.y + this.lthick2_);
                        Point point7 = new Point(((base.rect.x) + this.twlineThick_) + this.lineThick_, ((base.rect.y + this.lthick2_) + this.lineThick_) - 1);
                        Point point5 = new Point((base.rect.x + base.rect.width) - this.spaceHeight_, base.rect.y + this.lthick2_);
                        Point point6 = new Point((base.rect.x + base.rect.width) - this.spaceHeight_, ((base.rect.y + this.lthick2_) + this.lineThick_) - 1);
                        points = new Point[] { point1, point2, point3, point4, point5, point6, point7, point8, point9, point10, point11 };
                        Point[] pointArray1 = points;
                        base.painter_.Polyline(pointArray1, color);
                    }
                    else
                    {
                        base.painter_.DrawLine(base.rect.x, base.rect.y + (base.rect.height / 2), ((base.rect.x ) + this.flineThick_) - 1, base.rect.y + (base.rect.height / 2), color);
                        x1 = (((base.rect.x ) + this.flineThick_) - this.dlineThick_) + 1;
                        y1 = base.rect.y + (base.rect.height / 2);
                        x2 = (base.rect.x ) + this.elineThick_;
                        y2 = base.rect.y + base.rect.height;
                        xx2 = (base.rect.x ) + this.twlineThick_;
                        yy2 = base.rect.y + this.lthick2_;
                        double num15 = this.aspect(x1, y1, x2, y2);
                        double num16 = this.diff(x1, y1, num15);
                        double num17 = this.aspect(x2, y2, xx2, yy2);
                        double num18 = this.diff(x2, y2, num17);
                        int num19 = 0;
                        int num20 = 0;
                        double num21 = 0;
                        double num22 = 0;
                        num21 = (num18 - num16) / (num15 - num17);
                        num22 = (num21 * num15) + num16;
                        num19 = Convert.ToInt32(Math.Round(num21));
                        num20 = Convert.ToInt32(Math.Round(num22));
                        Point point12 = new Point(x1, y1);
                        Point point16 = new Point(x1, y1);
                        Point point15 = new Point(num19, num20);
                        num16 = this.diff((x1 + this.dlineThick_) - 1, y1, num15);
                        num21 = (num18 - num16) / (num15 - num17);
                        num22 = (num21 * num15) + num16;
                        num19 = Convert.ToInt32(Math.Round(num21));
                        num20 = Convert.ToInt32(Math.Round(num22));
                        Point point14 = new Point(num19, num20);
                        Point point13 = new Point((base.rect.x ) + this.flineThick_, y1);
                        points = new Point[] { point12, point13, point14, point15, point16 };
                        base.painter_.Polyline(points, color);
                        base.painter_.DrawLine((base.rect.x ) + this.elineThick_, base.rect.y + base.rect.height, (base.rect.x) + this.twlineThick_, base.rect.y + this.lthick2_, color);
                        base.painter_.DrawLine((base.rect.x ) + this.twlineThick_, base.rect.y + this.lthick2_, (base.rect.x + base.rect.width) - this.spaceHeight_, base.rect.y + this.lthick2_, color);
                    }
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
            base.Skip(childNode, true);
        }

        private double aspect(int x1, int y1, int x2, int y2)
        {
            double deltaY = 0;
            double deltaX = 0;
            
            deltaY = y2 - y1;
            deltaX = x2 - x1;
            return (deltaY / deltaX);
        }

        private double diff(int x1, int y1, double S)
        {
            return (y1 - (S * x1));
        }

        private int spaceHeight_;
        private int lthick2_;
        private int tlinethick_;
        private int ftlineThick_;
        private int vthinSpace_;
        private int lineThick_;
        private int dlineThick_;
        private int flineThick_;
        private int elineThick_;
        private int twlineThick_;
    }
}

