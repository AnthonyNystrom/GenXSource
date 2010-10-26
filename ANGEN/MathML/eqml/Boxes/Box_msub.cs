namespace Boxes
{
    using Facade;
    using Nodes;
    using System;
    using System.Drawing;

    using Rendering;
    using Attrs;

    public class Box_msub : BaseBox
    {
            public class SubscriptCalc
            {
                public enum Heights
                {
                    Base,
                    Sup,
                    Sub,
                    Subsup
                }

                public enum SubType
                {
                    sub,
                    sup,
                    subsup
                }

                public class Info
                {
                    public class Size
                    {
                        public Size(int _w, int _h)
                        {
                            this.w = _w;
                            this.h = _h;
                        }

                        public int w;
                        public int h;
                    }

                    public Info(Node _node, Painter Graphics)
                    {
                        this.x = 0;
                        this.y = 0;
                        this.w = 0;
                        this.h = 0;
                        this.b = 0;
                        this.painter = Graphics;
                        this.node = _node;
                        this.w = this.node.box.Width;
                        this.h = this.node.box.Height;
                        this.b = this.node.box.Baseline;
                        Rectangle rectangle = this.painter.CalcRect(this.node, this.node.style_);
                        this.size_ = new Size(rectangle.Width, rectangle.Height);
                    }

                    public int x;
                    public int y;
                    public int w;
                    public int h;
                    public int b;
                    public Node node;
                    private Painter painter;
                    public Size size_;
                }

                public SubscriptCalc(Node _node, Painter Graphics)
                {
                    this.x = 0;
                    this.y = 0;
                    this.w = 0;
                    this.h = 0;
                    this.b = 0;
                    this.xInc = 0;
                    this.yInc = 0;
                    this.xplus = 0;
                    this.wplus = 0;
                    this.bplus = 0;
                    this.hplus = 0;
                    this.kind_ = SubType.subsup;
                    this.base_ = null;
                    this.subInfo = null;
                    this.supInfo = null;
                    this.painter = Graphics;
                    this.node = _node;
                    switch (this.node.type_.type)
                    {
                        case ElementType.Msup:
                        {
                            this.kind_ = SubType.sup;
                            break;
                        }
                        case ElementType.Msub:
                        {
                            this.kind_ = SubType.sub;
                            break;
                        }
                        case ElementType.Msubsup:
                        {
                            this.kind_ = SubType.subsup;
                            break;
                        }
                    }
                    if (this.node.HasChildren())
                    {
                        NodesList list = this.node.GetChildrenNodes();
                        for (Node n = list.Next(); n != null; n = list.Next())
                        {
                            if (n.childIndex == 0)
                            {
                                this.BaseInfo(n, this.painter);
                            }
                            else if (((this.kind_ == SubType.sup) && (n.childIndex == 1)) ||
                                     ((this.kind_ == SubType.subsup) && (n.childIndex == 2)))
                            {
                                if (n.prevSibling != null)
                                {
                                    if (n.childIndex == 1)
                                    {
                                        n.lowerNode = n.prevSibling;
                                        n.prevSibling.upperNode = n;
                                    }
                                    else if ((n.childIndex == 2) && (n.prevSibling.prevSibling != null))
                                    {
                                        n.prevSibling.prevSibling.upperNode = n;
                                        n.lowerNode = n.prevSibling.prevSibling;
                                    }
                                }
                                this.SupInfo(n, this.painter);
                            }
                            else if (((this.kind_ == SubType.sub) && (n.childIndex == 1)) ||
                                     ((this.kind_ == SubType.subsup) && (n.childIndex == 1)))
                            {
                                if (n.prevSibling != null)
                                {
                                    n.upperNode = n.prevSibling;
                                    n.prevSibling.lowerNode = n;
                                }
                                this.SubInfo(n, this.painter);
                            }
                        }
                    }
                    this.calc();
                    this.updateNode();
                }

                public void BaseInfo(Node node, Painter Graphics)
                {
                    this.base_ = new Info(node, Graphics);
                }

                public void SubInfo(Node node, Painter Graphics)
                {
                    this.subInfo = new Info(node, Graphics);
                }

                public void SupInfo(Node node, Painter Graphics)
                {
                    this.supInfo = new Info(node, Graphics);
                }

                public void calc()
                {
                    double t = 0;
                    t = 0.2*this.base_.size_.w;
                    t = Math.Round(t);
                    this.xInc = (int) t;


                    this.yInc = 0;

                    t = 0.2*this.base_.size_.w;
                    t = Math.Round(t);
                    this.xplus = (int) t;

                    t = 0.2*this.base_.size_.w;
                    t = Math.Round(t);
                    this.wplus = (int) t;

                    t = 0.45*this.base_.size_.h;
                    t = Math.Round(t);
                    this.bplus = (int) t;

                    t = 0.2*this.base_.size_.h;
                    t = Math.Round(t);
                    this.hplus = (int) t;

                    switch (this.kind_)
                    {
                        case SubType.sub:
                        {
                            if ((this.choose(this.base_.h, this.base_.size_.h) >= this.height(Heights.Base)) ||
                                (this.choose(this.subInfo.h, this.subInfo.size_.h) >= this.height(Heights.Sub)))
                            {
                                if (this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base))
                                {
                                    if (this.subInfo.h > this.subInfo.size_.h)
                                    {
                                        this.w = (((this.xInc + this.base_.w) + this.wplus) + this.subInfo.w) +
                                                 this.xInc;
                                        this.b = this.yInc + this.base_.b;
                                        this.h = (((this.b - this.base_.b) + (this.base_.h/2)) + this.subInfo.h) +
                                                 this.yInc;
                                        return;
                                    }
                                    this.w = (((this.xInc + this.base_.w) + this.wplus) + this.subInfo.w) + this.xInc;
                                    this.b = this.yInc + this.base_.b;
                                    this.h = ((this.b + (this.base_.h - this.base_.b)) + (this.subInfo.h/2)) + this.yInc;
                                    return;
                                }
                                this.w = (((this.xInc + this.base_.w) + this.wplus) + this.subInfo.w) + this.xInc;
                                this.b = this.yInc + this.base_.b;
                                this.h = (((this.b + (this.base_.h - this.base_.b)) + this.subInfo.h) - this.hplus) +
                                         this.yInc;
                                return;
                            }
                            this.w = (((this.xInc + this.base_.w) + this.wplus) + this.subInfo.w) + this.xInc;
                            this.b = this.yInc + this.base_.b;
                            this.h = (this.b +
                                      Math.Max((int) (this.base_.h - this.base_.b),
                                               (int) ((this.subInfo.h - this.subInfo.b) + this.hplus))) + this.yInc;
                            return;
                        }
                        case SubType.sup:
                        {
                            if ((this.choose(this.base_.h, this.base_.size_.h) >= this.height(Heights.Base)) ||
                                (this.choose(this.supInfo.h, this.supInfo.size_.h) >= this.height(Heights.Sup)))
                            {
                                if (this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base))
                                {
                                    if (this.supInfo.h > this.supInfo.size_.h)
                                    {
                                        this.w = (((this.xInc + this.base_.w) + this.xplus) + this.supInfo.w) +
                                                 this.xInc;
                                        this.b = ((this.yInc + this.supInfo.h) + this.base_.b) - (this.base_.h/2);
                                        this.h = (this.b + (this.base_.h - this.base_.b)) + this.yInc;
                                        return;
                                    }
                                    this.w = (((this.xInc + this.base_.w) + this.xplus) + this.supInfo.w) + this.xInc;
                                    this.b = (this.yInc + (this.supInfo.h/2)) + this.base_.b;
                                    this.h = (this.b + (this.base_.h - this.base_.b)) + this.yInc;
                                    return;
                                }
                                this.w = (((this.xInc + this.base_.w) + this.xplus) + this.supInfo.w) + this.xInc;
                                this.b = ((this.yInc + this.supInfo.h) - this.bplus) + this.base_.b;
                                this.h = (this.b + (this.base_.h - this.base_.b)) + this.yInc;
                                return;
                            }
                            this.w = (((this.xInc + this.base_.w) + this.xplus) + this.supInfo.w) + this.xInc;
                            this.b = this.yInc + Math.Max(this.supInfo.b + this.bplus, this.base_.b);
                            this.h = (this.b + (this.base_.h - this.base_.b)) + this.yInc;
                            return;
                        }
                        case SubType.subsup:
                        {
                            if (((this.choose(this.base_.h, this.base_.size_.h) >= this.height(Heights.Base)) ||
                                 (this.choose(this.supInfo.h, this.supInfo.size_.h) >= this.height(Heights.Sup))) ||
                                (this.choose(this.subInfo.h, this.subInfo.size_.h) >= this.height(Heights.Subsup)))
                            {
                                if ((this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base)) &&
                                    (this.choose(this.supInfo.h, this.supInfo.size_.h) < this.height(Heights.Sup)))
                                {
                                    if (this.subInfo.h > this.subInfo.size_.h)
                                    {
                                        this.w = ((this.xInc + this.base_.w) +
                                                  Math.Max((int) (this.xplus + this.supInfo.w),
                                                           (int) (this.wplus + this.subInfo.w))) + this.xInc;
                                        this.b = this.yInc + Math.Max(this.supInfo.b + this.bplus, this.base_.b);
                                        this.h = (((this.b - this.base_.b) + (this.base_.h/2)) + this.subInfo.h) +
                                                 this.yInc;
                                        int yy = ((this.y + this.b) - this.bplus) - this.supInfo.b;
                                        int yyy = ((this.y + this.h) - this.subInfo.h) - this.yInc;
                                        if ((yy + this.supInfo.h) > yyy)
                                        {
                                            this.h += (yy + this.supInfo.h) - yyy;
                                        }
                                        return;
                                    }
                                    this.w = ((this.xInc + this.base_.w) +
                                              Math.Max((int) (this.xplus + this.supInfo.w),
                                                       (int) (this.wplus + this.subInfo.w))) + this.xInc;
                                    this.b = this.yInc + Math.Max(this.supInfo.b + this.bplus, this.base_.b);
                                    this.h = (((this.b + this.base_.h) - this.base_.b) + (this.subInfo.h/2)) + this.yInc;
                                    int yxy = ((this.y + this.b) - this.bplus) - this.supInfo.b;
                                    int yxyy = ((this.y + this.h) - this.subInfo.h) - this.yInc;
                                    if ((yxy + this.supInfo.h) > yxyy)
                                    {
                                        this.h += (yxy + this.supInfo.h) - yxyy;
                                    }
                                }
                                else if ((this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base)) &&
                                         (this.choose(this.subInfo.h, this.subInfo.size_.h) <
                                          this.height(Heights.Subsup)))
                                {
                                    if (this.supInfo.h > this.supInfo.size_.h)
                                    {
                                        this.w = ((this.xInc + this.base_.w) +
                                                  Math.Max((int) (this.xplus + this.supInfo.w),
                                                           (int) (this.wplus + this.subInfo.w))) + this.xInc;
                                        this.b = ((this.yInc + this.supInfo.h) + this.base_.b) - (this.base_.h/2);
                                        this.h = (this.b +
                                                  Math.Max((int) (this.base_.h - this.base_.b),
                                                           (int) ((this.subInfo.h - this.subInfo.b) + this.hplus))) +
                                                 this.yInc;
                                        int yy = this.y + this.yInc;
                                        int yyy = ((this.y + this.h) - this.subInfo.h) - this.yInc;
                                        if ((yy + this.supInfo.h) > yyy)
                                        {
                                            this.h += (yy + this.supInfo.h) - yyy;
                                        }
                                    }
                                    else
                                    {
                                        this.w = ((this.xInc + this.base_.w) +
                                                  Math.Max((int) (this.xplus + this.supInfo.w),
                                                           (int) (this.wplus + this.subInfo.w))) + this.xInc;
                                        this.b = (this.yInc + (this.supInfo.h/2)) + this.base_.b;
                                        this.h = (this.b +
                                                  Math.Max((int) (this.base_.h - this.base_.b),
                                                           (int) ((this.subInfo.h - this.subInfo.b) + this.hplus))) +
                                                 this.yInc;
                                        int yy = this.y + this.yInc;
                                        int yyy = ((this.y + this.h) - this.subInfo.h) - this.yInc;
                                        if ((yy + this.supInfo.h) > yyy)
                                        {
                                            this.h += (yy + this.supInfo.h) - yyy;
                                        }
                                    }
                                }
                                else if (this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base))
                                {
                                    this.w = ((this.xInc + this.base_.w) +
                                              Math.Max((int) (this.xplus + this.supInfo.w),
                                                       (int) (this.wplus + this.subInfo.w))) + this.xInc;
                                    this.b = (this.yInc + this.supInfo.h) + (this.base_.b - (this.base_.h/2));
                                    this.h = ((this.yInc + this.supInfo.h) + this.subInfo.h) + this.yInc;
                                }
                                else
                                {
                                    this.w = ((this.xInc + this.base_.w) +
                                              Math.Max((int) (this.xplus + this.supInfo.w),
                                                       (int) (this.wplus + this.subInfo.w))) + this.xInc;
                                    this.b = ((this.yInc + this.supInfo.h) + this.base_.b) - this.bplus;
                                    this.h = (((this.b + (this.base_.h - this.base_.b)) + this.subInfo.h) + this.yInc) -
                                             this.hplus;
                                    int yy = this.y + this.yInc;
                                    int yyy = ((this.y + this.h) - this.subInfo.h) - this.yInc;
                                    if ((yy + this.supInfo.h) > yyy)
                                    {
                                        this.h += (yy + this.supInfo.h) - yyy;
                                    }
                                }
                                return;
                            }
                            this.w = ((this.xInc + this.base_.w) +
                                      Math.Max((int) (this.xplus + this.supInfo.w), (int) (this.wplus + this.subInfo.w))) +
                                     this.xInc;
                            this.b = this.yInc + Math.Max(this.supInfo.b + this.bplus, this.base_.b);
                            this.h = (this.b +
                                      Math.Max((int) (this.base_.h - this.base_.b),
                                               (int) ((this.subInfo.h - this.subInfo.b) + this.hplus))) + this.yInc;
                            int bb = ((this.y + this.b) - this.bplus) - this.supInfo.b;
                            int bbb = ((this.y + this.b) + this.hplus) - this.subInfo.b;
                            if ((bb + this.supInfo.h) > bbb)
                            {
                                this.hplus += (bb + this.supInfo.h) - bbb;
                                this.h = (this.b +
                                          Math.Max((int) (this.base_.h - this.base_.b),
                                                   (int) ((this.subInfo.h - this.subInfo.b) + this.hplus))) + this.yInc;
                            }
                            return;
                        }
                    }
                }

                public void getSize(int x, int y)
                {
                    this.x = x;
                    this.y = y;
                    switch (this.kind_)
                    {
                        case SubType.sub:
                        {
                            if ((this.choose(this.base_.h, this.base_.size_.h) >= this.height(Heights.Base)) ||
                                (this.choose(this.subInfo.h, this.subInfo.size_.h) >= this.height(Heights.Sub)))
                            {
                                if (this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base))
                                {
                                    this.base_.x = this.x + this.xInc;
                                    this.base_.y = (this.y + this.b) - this.base_.b;
                                    this.subInfo.x = ((this.x + this.xInc) + this.base_.w) + this.wplus;
                                    this.subInfo.y = ((this.y + this.h) - this.yInc) - this.subInfo.h;
                                    return;
                                }
                                this.base_.x = this.x + this.xInc;
                                this.base_.y = (this.y + this.b) - this.base_.b;
                                this.subInfo.x = ((this.x + this.xInc) + this.base_.w) + this.wplus;
                                this.subInfo.y = ((this.y + this.h) - this.yInc) - this.subInfo.h;
                                return;
                            }
                            this.base_.x = this.x + this.xInc;
                            this.base_.y = (this.y + this.b) - this.base_.b;
                            this.subInfo.x = ((this.x + this.xInc) + this.base_.w) + this.wplus;
                            this.subInfo.y = ((this.y + this.b) + this.hplus) - this.subInfo.b;
                            return;
                        }
                        case SubType.sup:
                        {
                            if ((this.choose(this.base_.h, this.base_.size_.h) >= this.height(Heights.Base)) ||
                                (this.choose(this.supInfo.h, this.supInfo.size_.h) >= this.height(Heights.Sup)))
                            {
                                if (this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base))
                                {
                                    this.base_.x = this.x + this.xInc;
                                    this.base_.y = (this.y + this.b) - this.base_.b;
                                    this.supInfo.x = ((this.x + this.xInc) + this.base_.w) + this.xplus;
                                    this.supInfo.y = this.y + this.yInc;
                                    return;
                                }
                                this.base_.x = this.x + this.xInc;
                                this.base_.y = (this.y + this.b) - this.base_.b;
                                this.supInfo.x = ((this.x + this.xInc) + this.base_.w) + this.xplus;
                                this.supInfo.y = this.y + this.yInc;
                                return;
                            }
                            this.base_.x = this.x + this.xInc;
                            this.base_.y = (this.y + this.b) - this.base_.b;
                            this.supInfo.x = ((this.x + this.xInc) + this.base_.w) + this.xplus;
                            this.supInfo.y = ((this.y + this.b) - this.bplus) - this.supInfo.b;
                            return;
                        }
                        case SubType.subsup:
                        {
                            if (((this.choose(this.base_.h, this.base_.size_.h) >= this.height(Heights.Base)) ||
                                 (this.choose(this.supInfo.h, this.supInfo.size_.h) >= this.height(Heights.Sup))) ||
                                (this.choose(this.subInfo.h, this.subInfo.size_.h) >= this.height(Heights.Subsup)))
                            {
                                if ((this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base)) &&
                                    (this.choose(this.supInfo.h, this.supInfo.size_.h) < this.height(Heights.Sup)))
                                {
                                    this.base_.x = this.x + this.xInc;
                                    this.base_.y = (this.y + this.b) - this.base_.b;
                                    this.supInfo.x = ((this.x + this.xInc) + this.base_.w) + this.xplus;
                                    this.supInfo.y = ((this.y + this.b) - this.bplus) - this.supInfo.b;
                                    this.subInfo.x = ((this.x + this.xInc) + this.base_.w) + this.wplus;
                                    this.subInfo.y = ((this.y + this.h) - this.yInc) - this.subInfo.h;
                                }
                                else if ((this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base)) &&
                                         (this.choose(this.subInfo.h, this.subInfo.size_.h) < this.height(Heights.Subsup)))
                                {
                                    this.base_.x = this.x + this.xInc;
                                    this.base_.y = (this.y + this.b) - this.base_.b;
                                    this.supInfo.x = ((this.x + this.xInc) + this.base_.w) + this.xplus;
                                    this.supInfo.y = this.y + this.yInc;
                                    this.subInfo.x = ((this.x + this.xInc) + this.base_.w) + this.wplus;
                                    this.subInfo.y = ((this.y + this.h) - this.yInc) - this.subInfo.h;
                                }
                                else if (this.choose(this.base_.h, this.base_.size_.h) < this.height(Heights.Base))
                                {
                                    this.base_.x = this.x + this.xInc;
                                    this.base_.y = (this.y + this.b) - this.base_.b;
                                    this.supInfo.x = ((this.x + this.xInc) + this.base_.w) + this.xplus;
                                    this.supInfo.y = this.y + this.yInc;
                                    this.subInfo.x = ((this.x + this.xInc) + this.base_.w) + this.wplus;
                                    this.subInfo.y = ((this.y + this.h) - this.yInc) - this.subInfo.h;
                                }
                                else
                                {
                                    this.base_.x = this.x + this.xInc;
                                    this.base_.y = (this.y + this.b) - this.base_.b;
                                    this.supInfo.x = ((this.x + this.xInc) + this.base_.w) + this.xplus;
                                    this.supInfo.y = this.y + this.yInc;
                                    this.subInfo.x = ((this.x + this.xInc) + this.base_.w) + this.wplus;
                                    this.subInfo.y = ((this.y + this.h) - this.yInc) - this.subInfo.h;
                                }
                                return;
                            }
                            this.base_.x = this.x + this.xInc;
                            this.base_.y = (this.y + this.b) - this.base_.b;
                            this.supInfo.x = ((this.x + this.xInc) + this.base_.w) + this.xplus;
                            this.supInfo.y = ((this.y + this.b) - this.bplus) - this.supInfo.b;
                            this.subInfo.x = ((this.x + this.xInc) + this.base_.w) + this.wplus;
                            this.subInfo.y = ((this.y + this.b) + this.hplus) - this.subInfo.b;
                            return;
                        }
                    }
                }

                public void updateNode()
                {
                    this.node.box.X = this.x;
                    this.node.box.Y = this.y;
                    this.node.box.Width = this.w;
                    this.node.box.Height = this.h;
                    this.node.box.Baseline = this.b;
                    this.base_.node.box.X = this.base_.x;
                    this.base_.node.box.Y = this.base_.y;
                    this.base_.node.box.Width = this.base_.w;
                    this.base_.node.box.Height = this.base_.h;
                    this.base_.node.box.Baseline = this.base_.b;
                    switch (this.kind_)
                    {
                        case SubType.sub:
                        {
                            this.subInfo.node.box.X = this.subInfo.x;
                            this.subInfo.node.box.Y = this.subInfo.y;
                            this.subInfo.node.box.Width = this.subInfo.w;
                            this.subInfo.node.box.Height = this.subInfo.h;
                            this.subInfo.node.box.Baseline = this.subInfo.b;
                            return;
                        }
                        case SubType.sup:
                        {
                            this.supInfo.node.box.X = this.supInfo.x;
                            this.supInfo.node.box.Y = this.supInfo.y;
                            this.supInfo.node.box.Width = this.supInfo.w;
                            this.supInfo.node.box.Height = this.supInfo.h;
                            this.supInfo.node.box.Baseline = this.supInfo.b;
                            return;
                        }
                        case SubType.subsup:
                        {
                            this.supInfo.node.box.X = this.supInfo.x;
                            this.supInfo.node.box.Y = this.supInfo.y;
                            this.supInfo.node.box.Width = this.supInfo.w;
                            this.supInfo.node.box.Height = this.supInfo.h;
                            this.supInfo.node.box.Baseline = this.supInfo.b;
                            this.subInfo.node.box.X = this.subInfo.x;
                            this.subInfo.node.box.Y = this.subInfo.y;
                            this.subInfo.node.box.Width = this.subInfo.w;
                            this.subInfo.node.box.Height = this.subInfo.h;
                            this.subInfo.node.box.Baseline = this.subInfo.b;
                            return;
                        }
                    }
                }

                public int choose(int a, int b)
                {
                    int r = a - b;
                    if (r < 0)
                    {
                        r = -r;
                    }
                    return r;
                }

                public int height(Heights type)
                {
                    double r;
                    switch (type)
                    {
                        case Heights.Base:
                        {
                            r = 0.5*this.base_.size_.h;
                            r = Math.Round(r);
                            return (int) r;
                        }
                        case Heights.Sup:
                        {
                            r = 0.8*this.supInfo.size_.h;
                            r = Math.Round(r);
                            return (int) r;
                        }
                        case Heights.Sub:
                        {
                            r = 0.8*this.subInfo.size_.h;
                            r = Math.Round(r);
                            return (int) r;
                        }
                        case Heights.Subsup:
                        {
                            r = 0.3*this.subInfo.size_.h;
                            r = Math.Round(r);
                            return (int) r;
                        }
                    }
                    return 0;
                }


                public int x;
                private int xInc;
                private int yInc;
                private int xplus;
                private int wplus;
                public int y;
                private int bplus;
                private int hplus;
                private SubType kind_;
                private Info base_;
                private Info subInfo;
                private Info supInfo;
                private Painter painter;
                private Node node;
                public int w;
                public int h;
                public int b;
            }
        public Box_msub()
        {
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            childNode.box.X = base.rect.x + childNode.box.X;
            childNode.box.Y = base.rect.y + childNode.box.Y;
        }

        public override void getSize(Node node)
        {
            this.sizer = new SubscriptCalc(node, base.painter_);
            this.sizer.getSize(0, 0);
            this.sizer.updateNode();
            this.sizer = null;
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
            if (childNode.type_ != null)
            {
                base.Skip(childNode, true);
            }
        }


        private SubscriptCalc sizer;

        
    }


}

