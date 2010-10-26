namespace Boxes
{
    using Attrs;
    using MathTable;
    using Nodes;
    using System;
    using System.Drawing;

    public class Box_mtable : BaseBox
    {
        public Box_mtable()
        {
            this.fontHeight = 0;
            this.fontWidth = 0;
            this.baseline = 0;
            this.height = 0;
            this.width = 0;
            base.rect = new BoxRect(0, 0, 0);
        }

        public override void UpdateChildPosition(Node childNode)
        {
            if (childNode.childIndex == 0)
            {
                this.table.update(this.fontWidth, this.fontHeight);
            }
        }

        public override void getSize(Node tableNode)
        {
            this.table = new MTable(tableNode);
            float height = base.painter_.FontSize(tableNode, tableNode.style_);
            float dpi = base.painter_.DpiX();
            this.table.CalcSize(dpi, height);
            base.rect.width = this.table.totalWidth;
            base.rect.height = this.table.totalVertFrameSpacing;
            base.rect.baseline = this.table.tableAlign + ((this.height - (2 * (this.height - this.baseline))) / 2);
        }

        public override void Draw(Node tableNode, PaintMode printMode, Color color)
        {
            if ((printMode == PaintMode.BACKGROUND))
            {
                base.painter_.FillRectangle(tableNode);
            }
            else
            {
                if (tableNode.NotBlack())
                {
                    color = tableNode.style_.color;
                }
                if (this.table.frame == TableLineStyle.SOLID)
                {
                    tableNode.box.Width = this.table.totalHorzFrameSpacing;
                    base.painter_.Rectangle(tableNode, color);
                    tableNode.box.Width = this.table.totalWidth;
                }
                else if (this.table.frame == TableLineStyle.DASHED)
                {
                    tableNode.box.Width = this.table.totalHorzFrameSpacing;
                    base.painter_.DrawNodeRect(tableNode, color);
                    tableNode.box.Width = this.table.totalWidth;
                }
                for (int i = 0; i < this.table.RowCount; i++)
                {
                    MRow row = this.table.GetRow(i);
                    for (int j = 0; j < row.Count; j++)
                    {
                        MCell cell = (MCell) row.cells[j];
                        int rspan = 0;
                        int colSpan = 0;
                        rspan = (i + cell.rowSpan) - 1;
                        colSpan = cell.colSpan;
                        if (rspan < (this.table.RowCount - 1))
                        {
                            int x1 = 0;
                            int x2 = 0;
                            int y1 = 0;
                            if (colSpan == 0)
                            {
                                x1 = base.rect.x;
                            }
                            else
                            {
                                x1 = (base.rect.x + this.table.spanBases[colSpan]) - (this.table.spanWidth[colSpan - 1] / 2);
                            }
                            if (colSpan == (this.table.ColCount - 1))
                            {
                                x2 = base.rect.x + this.table.totalHorzFrameSpacing;
                            }
                            else
                            {
                                x2 = ((base.rect.x + this.table.spanBases[(colSpan + cell.columnSpan) - 1]) + this.table.spanHeight[(colSpan + cell.columnSpan) - 1]) + (this.table.spanWidth[(colSpan + cell.columnSpan) - 1] / 2);
                            }
                            y1 = (row.node.box.Y + this.table.rowspanWidth(i, cell.rowSpan)) + (this.table.spacingWidth[rspan] / 2);
                            if (row.lines == TableLineStyle.SOLID)
                            {
                                base.painter_.DrawLine(x1, y1, x2, y1, color);
                            }
                            else if (row.lines == TableLineStyle.DASHED)
                            {
                                base.painter_.DrawDashLine(x1, y1, x2, y1, color);
                            }
                        }
                    }
                }
                for (int i = 0; i < this.table.RowCount; i++)
                {
                    MRow row = this.table.GetRow(i);
                    for (int j = 0; j < row.Count; j++)
                    {
                        MCell cell = (MCell) row.cells[j];
                        int cspn = (cell.colSpan + cell.columnSpan) - 1;
                        if (cspn < (this.table.ColCount - 1))
                        {
                            int x1 = 0;
                            int y1 = 0;
                            int y2 = 0;
                            int hh = 0;
                            for (int k = 0; k < cell.rowSpan; k++)
                            {
                                MRow rrow = this.table.GetRow(i + k);
                                hh += rrow.node.box.Height;
                                if (k < (cell.rowSpan - 1))
                                {
                                    hh += this.table.spacingWidth[i + k];
                                }
                            }
                            x1 = ((base.rect.x + this.table.spanBases[cspn]) + this.table.spanHeight[cspn]) + (this.table.spanWidth[cspn] / 2);
                            if (i == 0)
                            {
                                y1 = base.rect.y;
                            }
                            else
                            {
                                y1 = row.node.box.Y - (this.table.spacingWidth[i - 1] / 2);
                            }
                            if ((i + (cell.rowSpan - 1)) == (this.table.RowCount - 1))
                            {
                                y2 = base.rect.y + base.rect.height;
                            }
                            else
                            {
                                y2 = (row.node.box.Y + hh) + (this.table.spacingWidth[i] / 2);
                            }
                            if (this.table.colLines[cspn] == TableLineStyle.SOLID)
                            {
                                base.painter_.DrawLine(x1, y1, x1, y2, color);
                            }
                            else if (this.table.colLines[cspn] == TableLineStyle.DASHED)
                            {
                                base.painter_.DrawDashLine(x1, y1, x1, y2, color);
                            }
                        }
                    }
                }
                for (int i = 0; i < this.table.RowCount; i++)
                {
                    MRow row = this.table.GetRow(i);
                    if (row.isLabeled)
                    {
                        row.node.box.Width = this.table.totalWidth;
                    }
                }
            }
        }

        public override void setChildSize(Node childNode)
        {
            if (childNode.isVisible)
            {
                if (childNode.childIndex == 0)
                {
                    try
                    {
                        this.baseline = base.painter_.MeasureBaseline(childNode.parent_, childNode.parent_.style_, "X");
                        this.height = base.painter_.MeasureHeight(childNode.parent_, childNode.parent_.style_, "X");
                        this.width = base.painter_.MeasureWidth(childNode.parent_, childNode.parent_.style_, "X");
                    }
                    catch
                    {
                    }
                    if (this.height <= 0)
                    {
                        this.height = 20;
                    }
                    if (this.baseline <= 0)
                    {
                        this.baseline = 0x10;
                    }
                    if (this.width <= 0)
                    {
                        this.width = 11;
                    }
                }
                base.Skip(childNode, true);
            }
        }


        private MTable table;
        private int fontHeight;
        private int fontWidth;
        private int baseline;
        private int height;
        private int width;
    }
}

