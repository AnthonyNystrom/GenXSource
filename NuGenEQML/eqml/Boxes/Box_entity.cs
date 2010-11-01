namespace Boxes
{
    using Facade;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_entity : BaseBox
    {
        public Box_entity()
        {
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
            try
            {
                if (containerNode.numChildren > 0)
                {
                    return;
                }
                if ((containerNode.literalText != null) && (containerNode.literalText.Length > 0))
                {
                    base.painter_.MeasureBox(containerNode, containerNode.parent_.style_);
                }
                else
                {
                    base.painter_.MeasureBox(containerNode, containerNode.style_, "X");
                }
            }
            catch (Exception)
            {
            }
        }

        public override void Draw(Node node, PaintMode printMode, Color color)
        {
            if (((printMode != PaintMode.BACKGROUND)) && ((printMode == PaintMode.FOREGROUND)))
            {
                if ((node.isVisible && (node.literalText != null)) && (node.literalText.Length > 0))
                {
                    if (node.parent_ != null)
                    {
                        bool notInBrackets = true;
                        try
                        {
                            if ((node.parent_.type_.type == ElementType.Mo) && ((Box_Mo) node.parent_.box).isBracketed)
                            {
                                notInBrackets = false;
                            }
                        }
                        catch
                        {
                        }
                        if (notInBrackets)
                        {
                            base.painter_.DrawString(node, node.parent_.style_, color);
                        }
                    }
                    else
                    {
                        base.painter_.DrawString(node, null, color);
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
        }

    }
}

