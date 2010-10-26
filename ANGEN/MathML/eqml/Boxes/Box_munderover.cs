namespace Boxes
{
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_munderover : BaseBox
    {
        public Box_munderover()
        {
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode.childIndex == 1)
            {
                childNode.box.Y = (base.rect.y + base.rect.height) - childNode.box.Height;
            }
            else if (childNode.childIndex == 2)
            {
                childNode.box.Y = base.rect.y;
            }
            else if (childNode.childIndex == 0)
            {
                if ((childNode.nextSibling != null) && (childNode.nextSibling.nextSibling != null))
                {
                    childNode.box.Y = base.rect.y + childNode.nextSibling.nextSibling.box.Height;
                }
                else
                {
                    childNode.box.Y = base.rect.y;
                }
            }
            childNode.box.X = base.rect.x + ((base.rect.width - childNode.box.Width) / 2);
        }

        public override void getSize(Node containerNode)
        {
            if (containerNode.numChildren == 3)
            {
                Node first = containerNode.firstChild;
                Node afterFirst = containerNode.firstChild.nextSibling;
                Node afterAfterFirst = containerNode.firstChild.nextSibling.nextSibling;
                if (this.IsHarpoon(first))
                {
                    first.box.Width = containerNode.box.Width;
                    if (first.firstChild != null)
                    {
                        Node entity = null;
                        Node cell = null;
                        Node row = null;
                        if (first.type_.type == ElementType.Mrow)
                        {
                            row = first;
                            if ((first.numChildren == 1) && (first.firstChild.type_.type == ElementType.Mo))
                            {
                                cell = first.firstChild;
                            }
                        }
                        else if (first.type_.type == ElementType.Mo)
                        {
                            cell = first;
                        }
                        if (((cell != null) && (cell.numChildren == 1)) && (cell.firstChild.type_.type == ElementType.Entity))
                        {
                            entity = cell.firstChild;
                        }
                        if ((entity == null) || (cell == null))
                        {
                            return;
                        }
                        int totalSpacing = 0;
                        int lspace = 0;
                        int width = Math.Max(afterAfterFirst.box.Width, afterFirst.box.Width) + entity.box.Width;
                        if (row != null)
                        {
                            row.box.Width = width;
                        }
                        if (cell != null)
                        {
                            try
                            {
                                cell.box.Width = width;
                                Box_Mo box = (Box_Mo) cell.box;
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
                            entity.box.X = cell.box.X + lspace;
                        }
                        containerNode.box.Width = width;
                        afterFirst.box.X = (width - afterFirst.box.Width) / 2;
                        afterAfterFirst.box.X = (width - afterFirst.box.Width) / 2;
                    }
                }
                else
                {
                    int maxW = Math.Max(first.box.Width, afterAfterFirst.box.Width);
                    if (afterFirst.type_.type == ElementType.Mrow)
                    {
                        if (((afterFirst.numChildren == 1) && this.IsStretchy(afterFirst.firstChild)) && (afterFirst.box.Width < maxW))
                        {
                            afterFirst.box.Width = maxW;
                            afterFirst.firstChild.box.Width = maxW;
                            if (afterFirst.firstChild.numChildren > 0)
                            {
                                afterFirst.firstChild.firstChild.box.Width = maxW;
                            }
                        }
                    }
                    else if (this.IsStretchy(afterFirst) && (afterFirst.box.Width < maxW))
                    {
                        afterFirst.box.Width = maxW;
                        if (afterFirst.numChildren > 0)
                        {
                            afterFirst.firstChild.box.Width = maxW;
                        }
                    }
                    maxW = Math.Max(first.box.Width, afterFirst.box.Width);
                    if (afterAfterFirst.type_.type == ElementType.Mrow)
                    {
                        if (((afterAfterFirst.numChildren != 1) || !this.IsStretch(afterAfterFirst.firstChild)) || (afterAfterFirst.box.Width >= maxW))
                        {
                            return;
                        }
                        afterAfterFirst.box.Width = maxW;
                        afterAfterFirst.firstChild.box.Width = maxW;
                        if (afterAfterFirst.firstChild.numChildren <= 0)
                        {
                            return;
                        }
                        afterAfterFirst.firstChild.firstChild.box.Width = maxW;
                    }
                    else if (this.IsStretch(afterAfterFirst) && (afterAfterFirst.box.Width < maxW))
                    {
                        afterAfterFirst.box.Width = maxW;
                        if (afterAfterFirst.numChildren > 0)
                        {
                            afterAfterFirst.firstChild.box.Width = maxW;
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
            else if (childNode.childIndex == 2)
            {
                base.rect.baseline += childNode.box.Height;
            }
            if ((childNode.childIndex == 0) && (childNode.nextSibling != null))
            {
                childNode.upperNode = childNode.nextSibling;
                childNode.lowerNode = childNode.nextSibling;
                childNode.nextSibling.upperNode = childNode;
                if (childNode.nextSibling.nextSibling != null)
                {
                    childNode.nextSibling.nextSibling.lowerNode = childNode;
                    childNode.upperNode = childNode.nextSibling.nextSibling;
                }
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
                  ((node.firstChild.glyph.Code != "0005E") && (node.firstChild.glyph.Code != "0FE37")))))
            {
                return false;
            }
            return true;
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
            Node entityNode = null;
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
                    entityNode = firstChild.firstChild;
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
            if ((entityNode == null) || (entityNode.glyph == null))
            {
                return false;
            }
            string g = entityNode.glyph.Code;
            if ((((g != "0F578") && (g != "0F577")) && ((g != "0F576") && (g != "021C4"))) &&
                (((g != "021C6") && (g != "021CC")) && (g != "021CB")))
            {
                return false;
            }
            return true;
        }

    }
}

