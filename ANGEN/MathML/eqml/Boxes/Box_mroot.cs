namespace Boxes
{
    using Rendering;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_mroot : BaseBox
    {
        public Box_mroot()
        {
            this.w_ = 0;
            this.h_ = 0;
            this.totalThick_ = 0;
            this.lspace_ = 0;
            this.rspace_ = 0;
            this.tripleThick = 0;
            this.thick_ = 0;
            this.vthinSpace_ = 0;
            this.lineThickness = 0;
            this.doubleLineThickness = 0;
            this.ddThick_ = 0;
            this.ddThick2_ = 0;
            this.ddThick3_ = 0;
            this.lthick_ = 0;
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode.childIndex == 0)
            {
                childNode.box.X = ((base.rect.x + this.lspace_) + this.w_) + this.thick_;
                childNode.box.Y = (base.rect.y + base.rect.baseline) - childNode.box.Baseline;
            }
            else if (childNode.childIndex == 1)
            {
                childNode.box.X = base.rect.x + this.lspace_;
                childNode.box.Y = (((base.rect.y + base.rect.height) - (this.totalThick_ / 2)) - childNode.box.Height) - (2 * this.lineThickness);
            }
        }

        public override void getSize(Node containerNode)
        {
            this.lspace_ = Convert.ToInt32(Math.Round((double) base.painter_.CalcSpaceHeight(Space.Medium, containerNode, containerNode.style_)));
            this.rspace_ = Convert.ToInt32(Math.Round((double) base.painter_.CalcSpaceHeight(Space.Medium, containerNode, containerNode.style_)));
            this.lineThickness = base.painter_.LineThickness(containerNode);
            this.doubleLineThickness = base.painter_.DoubleLineThickness(containerNode);
            this.tripleThick = 3 * this.lineThickness;
            this.vthinSpace_ = Convert.ToInt32(Math.Round((double) base.painter_.CalcSpaceHeight(Space.VeryThin, containerNode, containerNode.style_)));
            this.lthick_ = this.lineThickness;
            this.ddThick_ = this.doubleLineThickness * 2;
            this.ddThick_ = Math.Max(this.ddThick_, 3);
            this.ddThick2_ = 2 * this.ddThick_;
            this.ddThick3_ = 3 * this.ddThick_;
            this.thick_ = this.ddThick3_ + this.doubleLineThickness;
            base.rect.width += ((this.thick_ + this.vthinSpace_) + this.lspace_) + this.rspace_;
            base.rect.height += this.tripleThick;
            this.totalThick_ += this.tripleThick;
            base.rect.baseline += this.h_ + this.tripleThick;
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if (printMode == PaintMode.BACKGROUND)
            {
                DrawBackground(node);
            }
            else if (printMode == PaintMode.FOREGROUND)
            {
                DrawForeground(color, node);
            }
        }

        private void DrawBackground(Node node)
        {
            base.painter_.FillRectangle(node);
        }

        private void DrawForeground(Color color, Node node)
        {
            int x1 = 0;
            int y1 = 0;
            int x2 = 0;
            int y2 = 0;
            int num5 = 0;
            int num6 = 0;


            Point[] pointArray3;
            if (node.NotBlack())
            {
                color = node.style_.color;
            }
            
            if (this.lineThickness > 1)
            {
                x1 = ((((base.rect.x + this.lspace_) + this.w_) + this.ddThick_) -
                        this.doubleLineThickness) + 1;
                y1 = base.rect.y + (this.h_ + (this.totalThick_/2));
                x2 = ((base.rect.x + this.lspace_) + this.w_) + this.ddThick2_;
                y2 = base.rect.y + base.rect.height;
                num5 = ((((base.rect.x + this.lspace_) + this.w_) + this.ddThick3_) +
                        this.lineThickness) - 1;
                num6 = (((base.rect.y + this.h_) + this.lthick_) + this.lineThickness) - 1;
                double num7 = this.aspect(x1, y1, x2, y2);
                double num8 = this.diff(x1, y1, num7);
                double num9 = this.aspect(x2, y2, num5, num6);
                double num10 = this.diff(x2, y2, num9);
                int num11 = 0;
                int num12 = 0;
                double num13 = 0;
                double num14 = 0;
                num13 = (num10 - num8)/(num7 - num9);
                num14 = (num13*num7) + num8;
                num11 = Convert.ToInt32(Math.Round(num13));
                num12 = Convert.ToInt32(Math.Round(num14));
                Point point9 = new Point(x1, (y1 + this.lineThickness) - 1);
                Point point8 = new Point(num11, num12);
                num8 = this.diff((x1 + this.doubleLineThickness) - 1, y1, num7);
                num10 =
                    this.diff(((base.rect.x + this.lspace_) + this.w_) + this.ddThick3_,
                            (base.rect.y + this.h_) + this.lthick_, num9);
                num13 = (num10 - num8)/(num7 - num9);
                num14 = (num13*num7) + num8;
                num11 = Convert.ToInt32(Math.Round(num13));
                num12 = Convert.ToInt32(Math.Round(num14));
                Point point3 = new Point(num11, num12);
                Point point2 = new Point((x1 + this.doubleLineThickness) - 1, y1);
                Point point10 =
                    new Point((base.rect.x + this.lspace_) + this.w_,
                              (((base.rect.y + this.h_) + (this.totalThick_/2)) + this.lineThickness) - 1);
                Point point11 =
                    new Point((base.rect.x + this.lspace_) + this.w_,
                              (base.rect.y + this.h_) + (this.totalThick_/2));
                Point point1 =
                    new Point((base.rect.x + this.lspace_) + this.w_,
                              (base.rect.y + this.h_) + (this.totalThick_/2));
                Point point4 =
                    new Point(((base.rect.x + this.lspace_) + this.w_) + this.ddThick3_,
                              (base.rect.y + this.h_) + this.lthick_);
                Point point7 =
                    new Point(
                        (((base.rect.x + this.lspace_) + this.w_) + this.ddThick3_) +
                        this.lineThickness,
                        (((base.rect.y + this.h_) + this.lthick_) + this.lineThickness) - 1);
                Point point5 =
                    new Point((base.rect.x + base.rect.width) - this.rspace_,
                              (base.rect.y + this.h_) + this.lthick_);
                Point point6 =
                    new Point((base.rect.x + base.rect.width) - this.rspace_,
                              (((base.rect.y + this.h_) + this.lthick_) + this.lineThickness) - 1);
                pointArray3 =
                    new Point[]
                        {point1, point2, point3, point4, point5, point6, point7, point8, point9, point10, point11};
                Point[] pointArray1 = pointArray3;
                base.painter_.Polyline(pointArray1, color);
            }
            else
            {
                base.painter_.DrawLine((base.rect.x + this.lspace_) + this.w_,
                                       (base.rect.y + this.h_) + (this.totalThick_/2),
                                       (((base.rect.x + this.lspace_) + this.w_) + this.ddThick_) -
                                       1, (base.rect.y + this.h_) + (this.totalThick_/2), color);
                x1 = ((((base.rect.x + this.lspace_) + this.w_) + this.ddThick_) -
                        this.doubleLineThickness) + 1;
                y1 = (base.rect.y + this.h_) + (this.totalThick_/2);
                x2 = ((base.rect.x + this.lspace_) + this.w_) + this.ddThick2_;
                y2 = (base.rect.y + this.h_) + this.totalThick_;
                num5 = ((base.rect.x + this.lspace_) + this.w_) + this.ddThick3_;
                num6 = (base.rect.y + this.h_) + this.lthick_;
                double num15 = this.aspect(x1, y1, x2, y2);
                double num16 = this.diff(x1, y1, num15);
                double num17 = this.aspect(x2, y2, num5, num6);
                double num18 = this.diff(x2, y2, num17);
                int num19 = 0;
                int num20 = 0;
                double num21 = 0;
                double num22 = 0;
                num21 = (num18 - num16)/(num15 - num17);
                num22 = (num21*num15) + num16;
                num19 = Convert.ToInt32(Math.Round(num21));
                num20 = Convert.ToInt32(Math.Round(num22));
                Point point12 = new Point(x1, y1);
                Point point16 = new Point(x1, y1);
                Point point15 = new Point(num19, num20);
                num16 = this.diff((x1 + this.doubleLineThickness) - 1, y1, num15);
                num21 = (num18 - num16)/(num15 - num17);
                num22 = (num21*num15) + num16;
                num19 = Convert.ToInt32(Math.Round(num21));
                num20 = Convert.ToInt32(Math.Round(num22));
                Point point14 = new Point(num19, num20);
                Point point13 =
                    new Point(((base.rect.x + this.lspace_) + this.w_) + this.ddThick_, y1);
                pointArray3 = new Point[] {point12, point13, point14, point15, point16};
                Point[] pointArray2 = pointArray3;
                base.painter_.Polyline(pointArray2, color);
                base.painter_.DrawLine(((base.rect.x + this.lspace_) + this.w_) + this.ddThick2_,
                                       (base.rect.y + this.h_) + this.totalThick_,
                                       ((base.rect.x + this.w_) + this.lspace_) + this.ddThick3_,
                                       (base.rect.y + this.h_) + this.lthick_, color);
                base.painter_.DrawLine(((base.rect.x + this.lspace_) + this.w_) + this.ddThick3_,
                                       (base.rect.y + this.h_) + this.lthick_,
                                       (base.rect.x + base.rect.width) - this.rspace_,
                                       (base.rect.y + this.h_) + this.lthick_, color);
            }
        }

        public override void setChildSize(Node childNode)
        {
            if (childNode.childIndex == 0)
            {
                this.w_ = 0;
                this.h_ = 0;
                this.totalThick_ = 0;
                base.rect.width = childNode.box.Width;
                if (childNode.box.Baseline > base.rect.baseline)
                {
                    base.rect.baseline = childNode.box.Baseline;
                }
                if ((childNode.box.Height - childNode.box.Baseline) > (base.rect.height - base.rect.baseline))
                {
                    base.rect.height = base.rect.baseline + (childNode.box.Height - childNode.box.Baseline);
                }
            }
            else if (childNode.childIndex == 1)
            {
                if (childNode.prevSibling != null)
                {
                    childNode.lowerNode = childNode.prevSibling;
                    childNode.prevSibling.upperNode = childNode;
                }
                this.totalThick_ = base.rect.height;
                if (childNode.box.Width > this.ddThick_)
                {
                    base.rect.width += childNode.box.Width - this.ddThick_;
                    this.w_ = childNode.box.Width - this.ddThick_;
                }
                if ((childNode.box.Height + (2 * this.lineThickness)) > (base.rect.height / 2))
                {
                    this.h_ = (childNode.box.Height + (2 * this.lineThickness)) - (base.rect.height / 2);
                    base.rect.height += this.h_;
                }
            }
            base.Skip(childNode, true);
        }

        private double aspect(int x1, int y1, int x2, int y2)
        {
            double deltay = 0;
            double deltax = 0;
            deltay = y2 - y1;
            deltax = x2 - x1;
            return (deltay / deltax);
        }

        private double diff(int x1, int y1, double S)
        {
            return (y1 - (S * x1));
        }

        private int w_;
        private int h_;
        private int ddThick_;
        private int ddThick2_;
        private int ddThick3_;
        private int lthick_;
        private int totalThick_;
        private int lspace_;
        private int rspace_;
        private int tripleThick;
        private int thick_;
        private int vthinSpace_;
        private int lineThickness;
        private int doubleLineThickness;
    }
}

