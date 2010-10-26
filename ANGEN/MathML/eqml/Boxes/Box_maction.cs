namespace Boxes
{
    using Attrs;
    using Facade;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_maction : BaseBox
    {
        public Box_maction()
        {
            this.target = null;
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode == this.target)
            {
                childNode.box.X = base.rect.x;
                childNode.box.Y = (base.rect.y + base.rect.baseline) - childNode.box.Baseline;
                childNode.isVisible = true;
            }
            else
            {
                childNode.isVisible = false;
            }
        }

        public override void getSize(Node containerNode)
        {
            if (this.target == null)
            {
                base.painter_.MeasureBox(containerNode, containerNode.style_, "X");
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
            bool foundTarget = false;
            if (childNode.childIndex == 0)
            {
                this.actionAttrs = AttributeBuilder.ActionAttributes(childNode.parent_);
            }
            if (this.actionAttrs != null)
            {
                switch (this.actionAttrs.actionType)
                {
                    case ActionType.StatusLine:
                    case ActionType.ToolTip:
                    {
                        if (childNode.childIndex == 0)
                        {
                            this.target = childNode;
                            foundTarget = true;
                        }
                        break;
                    }
                    case ActionType.Toggle:
                    {
                        if ((childNode.childIndex + 1) == this.actionAttrs.selection)
                        {
                            this.target = childNode;
                            foundTarget = true;
                        }
                        break;
                    }
                    case ActionType.Highlight:
                    {
                        if (childNode.childIndex == 0)
                        {
                            this.target = childNode;
                            foundTarget = true;
                        }
                        break;
                    }
                    case ActionType.Unknown:
                    {
                        if (childNode.childIndex == 0)
                        {
                            this.target = childNode;
                            foundTarget = true;
                        }
                        break;
                    }
                }
            }
            if ((foundTarget && (this.target != null)) && (childNode == this.target))
            {
                base.rect.width = this.target.box.Width;
                base.rect.baseline = this.target.box.Baseline;
                base.rect.accent = this.target.box.Height - this.target.box.Baseline;
                base.Skip(childNode, false);
                base.rect.height = base.rect.baseline + base.rect.accent;
                this.target.skip = false;
                this.target.isVisible = true;
                this.target.isGlyph = false;
            }
            else
            {
                childNode.box.Width = 0;
                childNode.box.Height = 0;
                childNode.box.X = 0;
                childNode.box.Y = 0;
                childNode.skip = true;
                childNode.isVisible = false;
                childNode.isGlyph = true;
            }
            if (childNode.type_.type == ElementType.Mrow)
            {
                base.Skip(childNode, true);
            }
        }

        private ActionAttributes actionAttrs;
        public Node target;
    }
}

