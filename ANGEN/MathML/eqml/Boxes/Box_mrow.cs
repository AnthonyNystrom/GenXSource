namespace Boxes
{
    using Facade;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_mrow : BaseBox
    {
        public Box_mrow()
        {
            this.isStretchy = false;
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
                base.painter_.MeasureBox(containerNode, containerNode.style_, "X");
            }
            else if (this.isStretchy)
            {
                int rowB = containerNode.box.Baseline;
                int rowH = containerNode.box.Height;
                int normBase = base.painter_.MeasureBaseline(containerNode, containerNode.style_, "X");
                int baseLine_Shift = base.painter_.CenterHeight(containerNode);
                if (!base.painter_.IsAboveBaseline(base.rect.height, base.rect.baseline, normBase))
                {
                    NodesList list = containerNode.GetChildrenNodes();
                    for (int i = 0; i < list.Count; i++)
                    {
                        Node node = list.Get(i);
                        if (base.painter_.IsStretchy(node))
                        {
                            int w = node.box.Width;
                            
                            try
                            {
                                ((Box_Mo) node.box).updateRowSize(node, rowH, rowB, baseLine_Shift);
                            }
                            catch
                            {
                            }

                            base.rect.width -= w - node.box.Width;
                            if (node.box.Height > base.rect.height)
                            {
                                base.rect.height = node.box.Height;
                            }
                            if (node.box.Baseline > base.rect.baseline)
                            {
                                base.rect.baseline = node.box.Baseline;
                            }
                        }
                    }
                }
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
                if ((node.firstChild == null))
                {
                    base.painter_.OutlineRect(node);
                }
            }
        }

        public override void setChildSize(Node childNode)
        {
            if (childNode.childIndex == 0)
            {
                this.isStretchy = false;
            }
            if (base.painter_.IsStretchy(childNode))
            {
                this.isStretchy = true;
            }
            base.rect.width += childNode.box.Width;
            if (childNode.box.Baseline > base.rect.baseline)
            {
                base.rect.baseline = childNode.box.Baseline;
            }
            if ((childNode.box.Height - childNode.box.Baseline) > base.rect.accent)
            {
                base.rect.accent = childNode.box.Height - childNode.box.Baseline;
            }
            base.Skip(childNode, false);
            base.rect.height = base.rect.baseline + base.rect.accent;
            childNode.skip = false;
        }


        private bool isStretchy;
    }
}

