namespace Boxes
{
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_mover : BaseBox
    {
        public Box_mover()
        {
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode.childIndex == 0)
            {
                childNode.box.Y = (base.rect.y + base.rect.height) - childNode.box.Height;
            }
            else
            {
                childNode.box.Y = base.rect.y;
            }
            childNode.box.X = base.rect.x + ((base.rect.width - childNode.box.Width) / 2);
        }

        public override void getSize(Node containerNode)
        {
            if ((containerNode.firstChild != null) && (containerNode.firstChild.nextSibling != null))
            {
                Node first = containerNode.firstChild;
                Node next = containerNode.firstChild.nextSibling;
                if (this.IsStretchyRec(first))
                {
                    first.box.Width = containerNode.box.Width;
                    if (first.firstChild != null)
                    {
                        Node target = null;
                        Node op = null;
                        Node row = null;
                        if (first.type_.type == ElementType.Mrow)
                        {
                            row = first;
                            if ((first.numChildren == 1) && (first.firstChild.type_.type == ElementType.Mo))
                            {
                                op = first.firstChild;
                            }
                        }
                        else if (first.type_.type == ElementType.Mo)
                        {
                            op = first;
                        }
                        if (((op != null) && (op.numChildren == 1)) && (op.firstChild.type_.type == ElementType.Entity))
                        {
                            target = op.firstChild;
                        }
                        if ((target == null) || (op == null))
                        {
                            return;
                        }
                        int totalSpacing = 0;
                        int lspace = 0;
                        int nw = next.box.Width;
                        int tw = target.box.Width;
                        int width = nw + tw;
                        if (row != null)
                        {
                            row.box.Width = width;
                        }
                        if (op != null)
                        {
                            try
                            {
                                op.box.Width = width;
                                Box_Mo mo = (Box_Mo) op.box;
                                totalSpacing = mo.totalSpacing;
                                lspace = mo.lspace;
                            }
                            catch
                            {
                            }
                        }
                        if (target != null)
                        {
                            target.box.Width = width - totalSpacing;
                            target.box.X = op.box.X + lspace;
                        }
                        containerNode.box.Width = width;
                        next.box.X = (width - next.box.Width) / 2;
                    }
                }
                else if (next.type_.type == ElementType.Mrow)
                {
                    if (((next.numChildren != 1) || !this.IsStretchy(next.firstChild)) || (next.box.Width >= next.prevSibling.box.Width))
                    {
                        return;
                    }
                    next.box.Width = next.prevSibling.box.Width;
                    next.firstChild.box.Width = next.prevSibling.box.Width;
                    if (next.firstChild.numChildren <= 0)
                    {
                        return;
                    }
                    next.firstChild.firstChild.box.Width = next.prevSibling.box.Width;
                }
                else if (this.IsStretchy(next) && (next.box.Width < next.prevSibling.box.Width))
                {
                    next.box.Width = next.prevSibling.box.Width;
                    if (next.numChildren > 0)
                    {
                        next.firstChild.box.Width = next.prevSibling.box.Width;
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
        }

        public override void setChildSize(Node childNode)
        {
            if (childNode.box.Width > base.rect.width)
            {
                base.rect.width = childNode.box.Width;
            }
            base.rect.height += childNode.box.Height;
            if (childNode.childIndex == 0)
            {
                base.rect.baseline = childNode.box.Baseline;
            }
            else
            {
                base.rect.baseline += childNode.box.Height;
            }
            if ((childNode.childIndex == 0) && (childNode.nextSibling != null))
            {
                childNode.upperNode = childNode.nextSibling;
                childNode.nextSibling.lowerNode = childNode;
            }
            base.Skip(childNode, true);
        }

        private bool IsStretchy(Node node)
        {
            try
            {
                if (node.attrs != null)
                {
                    Nodes.Attribute attribute = node.attrs.Get("stretchy");
                    if ((attribute != null) && !Convert.ToBoolean(attribute.val))
                    {
                        return false;
                    }
                }
            }
            catch
            {
            }
            if ((((node.firstChild == null) || (node.firstChild.type_.type != ElementType.Entity)) || (node.firstChild.glyph == null)) || ((((node.firstChild.glyph.Code != "021C0") && (node.firstChild.glyph.Code != "021C1")) && ((node.firstChild.glyph.Code != "021BC") && (node.firstChild.glyph.Code != "021BD"))) && ((((node.firstChild.glyph.Code != "02192") && (node.firstChild.glyph.Code != "02190")) && ((node.firstChild.glyph.Code != "02194") && (node.firstChild.glyph.Code != "000AF"))) && ((node.firstChild.glyph.Code != "0005E") && (node.firstChild.glyph.Code != "0FE37")))))
            {
                return false;
            }
            return true;
        }

        private bool IsStretchyRec(Node node)
        {
            bool r = false;
            Node target = null;
            Node firstChild = null;
            if (node.type_.type == ElementType.Mrow)
            {
                if ((node.numChildren == 1) && (node.firstChild.type_.type == ElementType.Mo))
                {
                    firstChild = node.firstChild;
                }
            }
            else if (node.type_.type == ElementType.Mo)
            {
                firstChild = node;
            }
            if (firstChild != null)
            {
                if ((firstChild.numChildren == 1) && (firstChild.firstChild.type_.type == ElementType.Entity))
                {
                    target = firstChild.firstChild;
                }
                try
                {
                    if (node.attrs != null)
                    {
                        Nodes.Attribute attribute = node.attrs.Get("stretchy");
                        if ((attribute != null) && !Convert.ToBoolean(attribute.val))
                        {
                            return false;
                        }
                    }
                }
                catch
                {
                }
            }
            if ((target == null) || (target.glyph == null))
            {
                return r;
            }
            string code = target.glyph.Code;
            if ((((code != "0F578") && (code != "0F577")) && ((code != "0F576") && (code != "021C4"))) && (((code != "021C6") && (code != "021CC")) && (code != "021CB")))
            {
                return r;
            }
            return true;
        }

    }
}

