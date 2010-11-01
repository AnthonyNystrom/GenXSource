namespace Boxes
{
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_munder : BaseBox
    {
        public Box_munder()
        {
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode.childIndex == 0)
            {
                childNode.box.Y = base.rect.y;
            }
            else
            {
                childNode.box.Y = (base.rect.y + base.rect.height) - childNode.box.Height;
            }
            childNode.box.X = base.rect.x + ((base.rect.width - childNode.box.Width) / 2);
        }

        public override void getSize(Node containerNode)
        {
            if ((containerNode.firstChild != null) && (containerNode.firstChild.nextSibling != null))
            {
                Node afterFirst = containerNode.firstChild.nextSibling;
                Node first = containerNode.firstChild;
                if (this.IsHarpoon(first))
                {
                    first.box.Width = containerNode.box.Width;
                    if (first.firstChild != null)
                    {
                        Node entity = null;
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
                            entity = op.firstChild;
                        }
                        if ((entity == null) || (op == null))
                        {
                            return;
                        }
                        int totalSpacing = 0;
                        int lspace = 0;
                        int width = afterFirst.box.Width + entity.box.Width;
                        if (row != null)
                        {
                            row.box.Width = width;
                        }
                        if (op != null)
                        {
                            try
                            {
                                op.box.Width = width;
                                Box_Mo box = (Box_Mo) op.box;
                                totalSpacing = box.totalSpacing;
                                lspace = box.lspace;
                            }
                            catch
                            {
                            }
                        }
                        if (entity != null)
                        {
                            entity.box.Width = width - totalSpacing;
                            entity.box.X = op.box.X + lspace;
                        }
                        containerNode.box.Width = width;
                        afterFirst.box.X = (width - afterFirst.box.Width) / 2;
                    }
                }
                else if (afterFirst.type_.type == ElementType.Mrow)
                {
                    if (((afterFirst.numChildren != 1) || !this.IsStretch(afterFirst.firstChild)) || (afterFirst.box.Width >= containerNode.firstChild.box.Width))
                    {
                        return;
                    }
                    afterFirst.box.Width = containerNode.firstChild.box.Width;
                    afterFirst.firstChild.box.Width = containerNode.firstChild.box.Width;
                    if (afterFirst.firstChild.numChildren <= 0)
                    {
                        return;
                    }
                    afterFirst.firstChild.firstChild.box.Width = containerNode.firstChild.box.Width;
                }
                else if (this.IsStretch(afterFirst) && (afterFirst.box.Width < containerNode.firstChild.box.Width))
                {
                    afterFirst.box.Width = containerNode.firstChild.box.Width;
                    if (afterFirst.numChildren > 0)
                    {
                        afterFirst.firstChild.box.Width = containerNode.firstChild.box.Width;
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
            if ((childNode.childIndex == 0) && (childNode.nextSibling != null))
            {
                childNode.lowerNode = childNode.nextSibling;
                childNode.nextSibling.upperNode = childNode;
            }
            base.Skip(childNode, true);
        }

        private bool IsStretch(Node node)
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
            if ((((node.firstChild == null) || (node.firstChild.type_.type != ElementType.Entity)) ||
                 (node.firstChild.glyph == null)) ||
                ((((node.firstChild.glyph.Code != "021C0") && (node.firstChild.glyph.Code != "021C1")) &&
                  ((node.firstChild.glyph.Code != "021BC") && (node.firstChild.glyph.Code != "021BD"))) &&
                 ((((node.firstChild.glyph.Code != "02192") && (node.firstChild.glyph.Code != "02190")) &&
                   ((node.firstChild.glyph.Code != "02194") && (node.firstChild.glyph.Code != "000AF"))) &&
                  ((node.firstChild.glyph.Code != "0FE38") && (node.firstChild.glyph.Code != "00332")))))
            {
                return false;
            }
            return true;
        }

        private bool IsHarpoon(Node node)
        {
            Node entity = null;
            Node op = null;
            if (node.type_.type == ElementType.Mrow)
            {
                if ((node.numChildren == 1) && (node.firstChild.type_.type == ElementType.Mo))
                {
                    op = node.firstChild;
                }
            }
            else if (node.type_.type == ElementType.Mo)
            {
                op = node;
            }
            if (op != null)
            {
                if ((op.numChildren == 1) && (op.firstChild.type_.type == ElementType.Entity))
                {
                    entity = op.firstChild;
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
            if ((entity == null) || (entity.glyph == null))
            {
                return false;
            }
            string g = entity.glyph.Code;
            if ((((g != "0F578") && (g != "0F577")) && ((g != "0F576") && (g != "021C4"))) &&
                (((g != "021C6") && (g != "021CC")) && (g != "021CB")))
            {
                return false;
            }
            return true;
        }

    }
}

