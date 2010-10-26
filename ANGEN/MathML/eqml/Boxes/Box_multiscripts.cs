namespace Boxes
{
    using Facade;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_multiscripts : BaseBox
    {
        public Box_multiscripts()
        {
            this.b_ = 0;
            this.h_ = 0;
            this.width_ = 0;
            this.center_ = 0;
            this.widthCenter_ = 0;
            this.tripWidth_ = 0;
            this.childWidth_ = 0;
            this.left_ = 0;
            this.right_ = 0;
            this.w2_ = 0;
            this.w_ = 0;
            this.childBlineLeft_ = 0;
            this.childBlineRight = 0;
            this.childBaseline_ = 0;
            this.w3Bline_ = 0;
            this.baseline_ = 0;
            this.top_ = 0;
            this.above_ = 0;
            this.bottom_ = 0;
            this.beyond_ = 0;
            this.childHeight_ = 0;
            this.chtWidth_ = 0;
            this.chtWidth2_ = 0;
            this.chtWidth3_ = 0;
            this.chtWidth4_ = 0;
            this.prescriptsIndex = 0;
            this.hasPrescripts = false;
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            int chIndex = childNode.childIndex;
            if (chIndex == 0)
            {
                this.hasPrescripts = false;
                this.prescriptsIndex = 0;
                int w = 0;
                if (base.max(this.left_, this.right_) > 0)
                {
                    w = this.tripWidth_;
                }
                childNode.box.X = (base.rect.x + w) + base.max(this.left_, this.right_);
                childNode.box.Y = (base.rect.y + base.rect.baseline) - this.childBaseline_;
            }
            else if (childNode.type_.type == ElementType.Mprescripts)
            {
                this.hasPrescripts = true;
                this.prescriptsIndex = chIndex;
            }
            else if (this.hasPrescripts)
            {
                if ((((chIndex - this.prescriptsIndex) == 1) || ((chIndex - this.prescriptsIndex) == 3)) || ((((chIndex - this.prescriptsIndex) == 5) || ((chIndex - this.prescriptsIndex) == 7)) || ((chIndex - this.prescriptsIndex) == 9)))
                {
                    if ((chIndex - this.prescriptsIndex) == 1)
                    {
                        this.chtWidth_ = 0;
                        this.chtWidth2_ = 0;
                    }
                    childNode.box.X = ((base.rect.x + base.max(this.left_, this.right_)) - this.left_) + this.chtWidth_;
                    if ((this.childHeight_ - (this.childBaseline_ - this.center_)) > this.top_)
                    {
                        childNode.box.Y = (((base.rect.y + base.rect.height) - this.top_) + this.childBlineLeft_) - childNode.box.Baseline;
                    }
                    else
                    {
                        childNode.box.Y = (((base.rect.y + base.rect.baseline) + -this.center_) + this.childBlineLeft_) - childNode.box.Baseline;
                    }
                    this.chtWidth_ += childNode.box.Width + this.widthCenter_;
                }
                else
                {
                    if (childNode.prevSibling != null)
                    {
                        childNode.prevSibling.upperNode = childNode;
                        childNode.lowerNode = childNode.prevSibling;
                    }
                    childNode.box.X = ((base.rect.x + base.max(this.left_, this.right_)) - this.right_) + this.chtWidth2_;
                    if ((this.childBaseline_ - this.center_) > this.above_)
                    {
                        childNode.box.Y = (base.rect.y + this.childBlineRight) - childNode.box.Baseline;
                    }
                    else
                    {
                        childNode.box.Y = ((((base.rect.y + base.rect.baseline) - this.center_) - this.above_) + this.childBlineRight) - childNode.box.Baseline;
                    }
                    this.chtWidth2_ += childNode.box.Width + this.widthCenter_;
                }
            }
            else if (((chIndex == 1) || (chIndex == 3)) || (((chIndex == 5) || (chIndex == 7)) || (chIndex == 9)))
            {
                if (chIndex == 1)
                {
                    this.chtWidth3_ = 0;
                    this.chtWidth4_ = 0;
                }
                childNode.box.X = ((base.rect.x + base.max(this.left_, this.right_)) + this.childWidth_) + this.chtWidth3_;
                if ((this.childHeight_ - (this.childBaseline_ - this.center_)) > this.bottom_)
                {
                    childNode.box.Y = (((base.rect.y + base.rect.height) - this.bottom_) + this.w3Bline_) - childNode.box.Baseline;
                }
                else
                {
                    childNode.box.Y = (((base.rect.y + base.rect.baseline) + -this.center_) + this.w3Bline_) - childNode.box.Baseline;
                }
                this.chtWidth3_ += childNode.box.Width + this.widthCenter_;
            }
            else
            {
                if (childNode.prevSibling != null)
                {
                    childNode.prevSibling.upperNode = childNode;
                    childNode.lowerNode = childNode.prevSibling;
                }
                childNode.box.X = ((base.rect.x + base.max(this.left_, this.right_)) + this.childWidth_) + this.chtWidth4_;
                if (this.childBaseline_ > this.beyond_)
                {
                    childNode.box.Y = (base.rect.y + this.baseline_) - childNode.box.Baseline;
                }
                else
                {
                    childNode.box.Y = ((((base.rect.y + base.rect.baseline) - this.center_) - this.beyond_) + this.baseline_) - childNode.box.Baseline;
                }
                this.chtWidth4_ += childNode.box.Width + this.widthCenter_;
            }
        }

        public override void getSize(Node node)
        {
            if (base.max(this.left_, this.right_) > 0)
            {
                this.childWidth_ += this.tripWidth_;
            }
            if (base.max(this.w2_, this.w_) > 0)
            {
                this.childWidth_ += this.tripWidth_;
            }
            base.rect.width = (base.max(this.left_, this.right_) + this.childWidth_) + base.max(this.w2_, this.w_);
            base.rect.height = base.max(this.above_, this.beyond_, this.childBaseline_ - this.center_) + base.max(this.top_, this.bottom_, this.childHeight_ - (this.childBaseline_ - this.center_));
            base.rect.baseline = base.max(this.above_ + this.center_, this.beyond_ + this.center_, this.childBaseline_);
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
            int childIndex = childNode.childIndex;
            if (childIndex == 0)
            {
                try
                {
                    this.b_ = base.painter_.MeasureBaseline(childNode.parent_, childNode.parent_.style_, "X");
                    this.h_ = base.painter_.MeasureHeight(childNode.parent_, childNode.parent_.style_, "X");
                    this.width_ = base.painter_.MeasureWidth(childNode.parent_, childNode.parent_.style_, "X");
                }
                catch
                {
                }
                if (this.h_ <= 0)
                {
                    this.h_ = 20;
                }
                if (this.b_ <= 0)
                {
                    this.b_ = 0x10;
                }
                if (this.width_ <= 0)
                {
                    this.width_ = 11;
                }
                try
                {
                    this.center_ = (this.h_ - (2 * (this.h_ - this.b_))) / 2;
                }
                catch
                {
                }
                if (this.center_ <= 0)
                {
                    this.center_ = 4;
                }
                try
                {
                    this.widthCenter_ = this.width_ / 3;
                }
                catch
                {
                }
                if (this.widthCenter_ <= 1)
                {
                    this.widthCenter_ = 2;
                }
                try
                {
                    this.tripWidth_ = this.width_ / 3;
                }
                catch
                {
                }
                if (this.tripWidth_ <= 1)
                {
                    this.tripWidth_ = 2;
                }
            }
            base.Skip(childNode, true);
            if (childIndex == 0)
            {
                this.hasPrescripts = false;
                this.prescriptsIndex = 0;
                this.childWidth_ = childNode.box.Width;
                this.childHeight_ = childNode.box.Height;
                this.childBaseline_ = childNode.box.Baseline;
            }
            else if (childNode.type_.type == ElementType.Mprescripts)
            {
                this.hasPrescripts = true;
                this.prescriptsIndex = childIndex;
            }
            else if (this.hasPrescripts)
            {
                if ((((childIndex - this.prescriptsIndex) == 1) || ((childIndex - this.prescriptsIndex) == 3)) || ((((childIndex - this.prescriptsIndex) == 5) || ((childIndex - this.prescriptsIndex) == 7)) || ((childIndex - this.prescriptsIndex) == 9)))
                {
                    this.left_ += childNode.box.Width;
                    if ((childIndex - this.prescriptsIndex) > 1)
                    {
                        this.left_ += this.widthCenter_;
                    }
                    if (this.childBlineLeft_ < childNode.box.Baseline)
                    {
                        this.childBlineLeft_ = childNode.box.Baseline;
                    }
                    if ((this.top_ - this.childBlineLeft_) < (childNode.box.Height - childNode.box.Baseline))
                    {
                        this.top_ = this.childBlineLeft_ + (childNode.box.Height - childNode.box.Baseline);
                    }
                }
                else
                {
                    this.right_ += childNode.box.Width;
                    if ((childIndex - this.prescriptsIndex) > 2)
                    {
                        this.right_ += this.widthCenter_;
                    }
                    if (this.childBlineRight < childNode.box.Baseline)
                    {
                        this.childBlineRight = childNode.box.Baseline;
                    }
                    if ((this.above_ - this.childBlineRight) < (childNode.box.Height - childNode.box.Baseline))
                    {
                        this.above_ = this.childBlineRight + (childNode.box.Height - childNode.box.Baseline);
                    }
                }
            }
            else if (((childIndex == 1) || (childIndex == 3)) || (((childIndex == 5) || (childIndex == 7)) || (childIndex == 9)))
            {
                this.w2_ += childNode.box.Width;
                if (childIndex > 1)
                {
                    this.w2_ += this.widthCenter_;
                }
                if (this.w3Bline_ < childNode.box.Baseline)
                {
                    this.w3Bline_ = childNode.box.Baseline;
                }
                if ((this.bottom_ - this.w3Bline_) < (childNode.box.Height - childNode.box.Baseline))
                {
                    this.bottom_ = this.w3Bline_ + (childNode.box.Height - childNode.box.Baseline);
                }
            }
            else
            {
                this.w_ += childNode.box.Width;
                if (childIndex > 2)
                {
                    this.w_ += this.widthCenter_;
                }
                if (this.baseline_ < childNode.box.Baseline)
                {
                    this.baseline_ = childNode.box.Baseline;
                }
                if ((this.beyond_ - this.baseline_) < (childNode.box.Height - childNode.box.Baseline))
                {
                    this.beyond_ = this.baseline_ + (childNode.box.Height - childNode.box.Baseline);
                }
            }
        }


        private int b_;
        private int h_;
        private int right_;
        private int w2_;
        private int w_;
        private int childBlineLeft_;
        private int childBlineRight;
        private int childBaseline_;
        private int w3Bline_;
        private int baseline_;
        private int top_;
        private int above_;
        private int width_;
        private int bottom_;
        private int beyond_;
        private int childHeight_;
        private int chtWidth_;
        private int chtWidth2_;
        private int chtWidth3_;
        private int chtWidth4_;
        private int prescriptsIndex;
        private bool hasPrescripts;
        private int center_;
        private int widthCenter_;
        private int tripWidth_;
        private int childWidth_;
        private int left_;
    }
}

