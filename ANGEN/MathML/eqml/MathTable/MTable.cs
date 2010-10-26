namespace MathTable
{
    using Attrs;
    using Nodes;
    using System;
    using System.Collections;

    public enum TableCellKind
    {
        SelAll,
        RowSelected,
        ColSelected,
        RowColSelected,
        BottomSelected
    }

    public class MTable
    {
        public MTable(Node node)
        {
            this.colColSpan = 0;
            this.totalHorzFrameSpacing = 0;
            this.totalWidth = 0;
            this.minWidth = 30;
            this.totalVertFrameSpacing = 0;
            this.tableAlign = 0;
            this.rowFrameSpacing = 0;
            this.colFrameSpacing = 0;
            this.maxWidth = 0;
            this.curRow = 0;
            this.curCol = 0;
            this.selKind_ = TableCellKind.SelAll;
            this.displayStyle = false;
            this.equalRows = false;
            this.equalColumns = false;
            this.align = TableAlign.AXIS;
            this.frame = TableLineStyle.NONE;
            this.framespacing = "0.4em 0.5ex";
            this.side = Side.RIGHT;
            this.minlabelSpacing = "0.8em";
            this.colLines = new TableLineStyle[] { TableLineStyle.NONE };
            this.colSpacing = new string[] { "0.8em" };
            this.colAligns = new HAlign[] { HAlign.CENTER };
            this.rowAligns = new RowAlign[] { RowAlign.BASELINE };
            this.node_ = node;
            this.attrs = AttributeBuilder.mtableAttributes(node);
            if (this.attrs != null)
            {
                this.rowAligns = this.attrs.rowAligns;
                this.colAligns = this.attrs.colAligns;
                this.colLines = this.attrs.colLines;
                this.colSpacing = this.attrs.colSpacing;
                this.displayStyle = this.attrs.displaystyle;
                this.equalRows = this.attrs.equalRows;
                this.equalColumns = this.attrs.equalColumns;
                this.align = this.attrs.align;
                this.frame = this.attrs.frame;
                this.framespacing = this.attrs.framespacing;
                this.side = this.attrs.side;
                this.minlabelSpacing = this.attrs.minlabelspacing;
            }
            this.rows = new ArrayList();
            if (node.HasChildren())
            {
                NodesList nodesList = node.GetChildrenNodes();
                Node n = nodesList.Next();
                for (int i = 0; n != null; i++)
                {
                    MRow row = this.AddRow(n, i);
                    if (this.attrs != null)
                    {
                        if (i < this.attrs.rowSpacing.Length)
                        {
                            row.spacing = this.attrs.rowSpacing[i];
                        }
                        else if (this.attrs.rowSpacing.Length > 0)
                        {
                            row.spacing = this.attrs.rowSpacing[this.attrs.rowSpacing.Length - 1];
                        }
                        if (i < this.attrs.rowLines.Length)
                        {
                            row.lines = this.attrs.rowLines[i];
                        }
                        else if (this.attrs.rowLines.Length > 0)
                        {
                            row.lines = this.attrs.rowLines[this.attrs.rowLines.Length - 1];
                        }
                    }
                    if (row.attrs != null)
                    {
                        row.colAligns = row.attrs.colAligns;
                        row.align = row.attrs.align;
                    }
                    if (n.HasChildren())
                    {
                        NodesList list = n.GetChildrenNodes();
                        Node child = list.Next();
                        int colSpan = 0;
                        if (n.type_.type == ElementType.Mlabeledtr)
                        {
                            row.isLabeled = true;
                            MCell cell = row.AddLabel(child, n.numChildren - 1);
                            if (cell.tableAttrs != null)
                            {
                                cell.rowAlign = cell.tableAttrs.rowAlign;
                                cell.columnAlign = cell.tableAttrs.columnAlign;
                                cell.columnSpan = cell.tableAttrs.columnSpan;
                                cell.rowSpan = cell.tableAttrs.rowSpan;
                            }
                            child = list.Next();
                        }
                        while (child != null)
                        {
                            MCell cell = row.AddCell(child, colSpan);
                            if (cell.tableAttrs != null)
                            {
                                cell.rowAlign = cell.tableAttrs.rowAlign;
                                cell.columnAlign = cell.tableAttrs.columnAlign;
                                cell.columnSpan = cell.tableAttrs.columnSpan;
                                cell.rowSpan = cell.tableAttrs.rowSpan;
                            }
                            child = list.Next();
                            if ((cell.tableAttrs != null) && (cell.tableAttrs.columnSpan > 1))
                            {
                                colSpan += cell.tableAttrs.columnSpan;
                                continue;
                            }
                            colSpan++;
                        }
                        if (colSpan > this.colColSpan)
                        {
                            this.colColSpan = colSpan;
                        }
                    }
                    n = nodesList.Next();
                }
            }
            if (this.colLines.Length < this.colColSpan)
            {
                TableLineStyle[] lines = new TableLineStyle[this.colColSpan];
                for (int colIndex = 0; colIndex < this.colColSpan; colIndex++)
                {
                    if (colIndex < this.colLines.Length)
                    {
                        lines[colIndex] = this.colLines[colIndex];
                    }
                    else if (this.colLines.Length > 0)
                    {
                        lines[colIndex] = this.colLines[this.colLines.Length - 1];
                    }
                    else
                    {
                        lines[colIndex] = TableLineStyle.NONE;
                    }
                }
                this.colLines = lines;
            }
            if (this.colSpacing.Length < this.colColSpan)
            {
                string[] strings = new string[this.colColSpan];
                for (int colIndex = 0; colIndex < this.colColSpan; colIndex++)
                {
                    if (colIndex < this.colSpacing.Length)
                    {
                        strings[colIndex] = this.colSpacing[colIndex];
                    }
                    else if (this.colSpacing.Length > 0)
                    {
                        strings[colIndex] = this.colSpacing[this.colSpacing.Length - 1];
                    }
                    else
                    {
                        strings[colIndex] = "0.8em";
                    }
                }
                this.colSpacing = strings;
            }
            for (int rowIndex = 0; rowIndex < this.RowCount; rowIndex++)
            {
                MRow row = this.GetRow(rowIndex);
                if (row.isLabeled && (row.cell != null))
                {
                    row.cell.colSpan = this.ColCount;
                }
            }
            this.UpdateRowspan();
            this.FixVAligns();
            this.FixHAligns();
            this.UpdateUpDownTies();
        }

        public void CalcSize(float DPI, float emHeight)
        {
            this.heights = new int[this.RowCount];
            this.baselines = new int[this.RowCount];
            this.spacingWidth = new int[this.RowCount];
            this.spanHeight = new int[this.ColCount];
            this.spanBases = new int[this.ColCount];
            this.spanWidth = new int[this.ColCount];
            for (int i = 0; i < this.RowCount; i++)
            {
                this.heights[i] = 0;
                this.baselines[i] = 0;
                this.spacingWidth[i] = 0;
            }
            for (int i = 0; i < this.ColCount; i++)
            {
                this.spanHeight[i] = 0;
                this.spanWidth[i] = 0;
            }
            for (int i = 0; i < this.RowCount; i++)
            {
                this.spacingWidth[i] = AttributeBuilder.FontWidth(emHeight, DPI, ((MRow) this.rows[i]).spacing, 0.5);
            }
            for (int i = 0; i < this.ColCount; i++)
            {
                if (i < this.colSpacing.Length)
                {
                    this.spanWidth[i] = AttributeBuilder.FontWidth(emHeight, DPI, this.colSpacing[i], 0.5);
                }
                else
                {
                    this.spanWidth[i] = AttributeBuilder.FontWidth(emHeight, DPI, this.colSpacing[this.spanWidth.Length - 1], 0.8);
                }
            }
            if ((this.frame == TableLineStyle.SOLID) || (this.frame == TableLineStyle.DASHED))
            {
                string[] strings = this.framespacing.Split(new char[] { ' ' }, 100);
                if (strings.Length > 0)
                {
                    this.rowFrameSpacing = AttributeBuilder.FontWidth(emHeight, DPI, strings[0], 0.5);
                    this.colFrameSpacing = AttributeBuilder.FontWidth(emHeight, DPI, strings[1], 0.4);
                }
            }
            else
            {
                this.rowFrameSpacing = 0;
                this.colFrameSpacing = 0;
            }
            for (int rowIndex = 0; rowIndex < this.RowCount; rowIndex++)
            {
                MRow row = this.GetRow(rowIndex);
                for (int colIndex = 0; colIndex < row.Count; colIndex++)
                {
                    MCell cell = (MCell) row.cells[colIndex];
                    Node node = cell.node;
                    if ((cell.columnSpan == 1) && (node.box.Width > this.spanHeight[cell.colSpan]))
                    {
                        this.spanHeight[cell.colSpan] = node.box.Width;
                    }
                    if (cell.rowSpan == 1)
                    {
                        if ((cell.rowAlign == RowAlign.BASELINE) || (cell.rowAlign == RowAlign.AXIS))
                        {
                            if (node.box.Baseline > this.baselines[rowIndex])
                            {
                                this.heights[rowIndex] += node.box.Baseline - this.baselines[rowIndex];
                                this.baselines[rowIndex] = node.box.Baseline;
                            }
                            if ((node.box.Height - node.box.Baseline) > (this.heights[rowIndex] - this.baselines[rowIndex]))
                            {
                                this.heights[rowIndex] = this.baselines[rowIndex] + (node.box.Height - node.box.Baseline);
                            }
                        }
                        else if ((((cell.rowAlign == RowAlign.CENTER) || (cell.rowAlign == RowAlign.TOP)) || (cell.rowAlign == RowAlign.BOTTOM)) && (node.box.Height > this.heights[rowIndex]))
                        {
                            this.heights[rowIndex] = node.box.Height;
                        }
                    }
                    if ((cell.rowSpan > 1) && ((cell.rowAlign == RowAlign.BASELINE) || (cell.rowAlign == RowAlign.AXIS)))
                    {
                        if (node.box.Baseline > this.baselines[rowIndex])
                        {
                            this.heights[rowIndex] += node.box.Baseline - this.baselines[rowIndex];
                            this.baselines[rowIndex] = node.box.Baseline;
                        }
                    }
                }
            }
            for (int rowIndex = 0; rowIndex < this.RowCount; rowIndex++)
            {
                MRow row = this.GetRow(rowIndex);
                for (int colIndex = 0; colIndex < row.Count; colIndex++)
                {
                    MCell cell = (MCell) row.cells[colIndex];
                    Node node = cell.node;
                    if (cell.rowSpan > 1)
                    {
                        int rowspanWidth = this.rowspanWidth(rowIndex, cell.rowSpan);
                        if (node.box.Height > rowspanWidth)
                        {
                            for (int k = 0; k < cell.rowSpan; k++)
                            {
                                this.heights[rowIndex + k] += (node.box.Height - rowspanWidth) / cell.rowSpan;
                            }
                            while (node.box.Height > rowspanWidth)
                            {
                                for (int m = 0; m < cell.rowSpan; m++)
                                {
                                    this.heights[rowIndex + m] += 1;
                                }
                                rowspanWidth = this.rowspanWidth(rowIndex, cell.rowSpan);
                            }
                        }
                    }
                    if (cell.columnSpan > 1)
                    {
                        int totalColSpan = this.TotalColSpan(cell.colSpan, cell.columnSpan);
                        if (node.box.Width > totalColSpan)
                        {
                            for (int i = 0; i < cell.columnSpan; i++)
                            {
                                this.spanHeight[cell.colSpan + i] += ((node.box.Width - totalColSpan) / cell.columnSpan);
                            }
                            while (node.box.Width > totalColSpan)
                            {
                                for (int j = 0; j < cell.columnSpan; j++)
                                {
                                    this.spanHeight[rowIndex + j] += 1;
                                }
                                totalColSpan = this.TotalColSpan(cell.colSpan, cell.columnSpan);
                            }
                        }
                    }
                }
            }
            
            // make all rows equal height
            if (this.equalRows)
            {
                int max = 0;
                for (int i = 0; i < this.RowCount; i++)
                {
                    if (this.heights[i] > max)
                    {
                        max = this.heights[i];
                    }
                }
                for (int i = 0; i < this.RowCount; i++)
                {
                    this.heights[i] = max;
                }
            }

            if (this.equalColumns)
            {
                int max = 0;
                for (int i = 0; i < this.ColCount; i++)
                {
                    if (this.spanHeight[i] > max)
                    {
                        max = this.spanHeight[i];
                    }
                }
                for (int i = 0; i < this.ColCount; i++)
                {
                    this.spanHeight[i] = max;
                }
            }

            for (int i = 0; i < this.RowCount; i++)
            {
                MRow row = this.GetRow(i);
                if (row.isLabeled)
                {
                    if (row.cell.node.box.Height > this.heights[i])
                    {
                        this.heights[i] = row.cell.node.box.Height;
                    }
                    if (row.cell.node.box.Width > this.maxWidth)
                    {
                        this.maxWidth = row.cell.node.box.Width;
                    }
                }
            }
            this.totalHorzFrameSpacing = 2 * this.rowFrameSpacing;
            for (int i = 0; i < this.ColCount; i++)
            {
                this.totalHorzFrameSpacing += this.spanHeight[i];
                if (i < (this.ColCount - 1))
                {
                    this.totalHorzFrameSpacing += this.spanWidth[i];
                }
            }
            this.totalWidth = this.totalHorzFrameSpacing;
            if (this.maxWidth > 0)
            {
                this.totalWidth += this.minWidth + this.maxWidth;
            }
            this.totalVertFrameSpacing = 2 * this.colFrameSpacing;
            for (int i = 0; i < this.RowCount; i++)
            {
                if (i < (this.RowCount - 1))
                {
                    this.totalVertFrameSpacing += this.spacingWidth[i];
                }
                this.totalVertFrameSpacing += this.heights[i];
            }
            this.tableAlign = this.totalVertFrameSpacing / 2;
            for (int i = 0; i < this.RowCount; i++)
            {
                MRow row = this.GetRow(i);
                row.node.box.Width = this.totalHorzFrameSpacing - (2 * this.rowFrameSpacing);
                row.node.box.Height = this.heights[i];
                row.node.box.Baseline = this.baselines[i];
            }
            if (this.align == TableAlign.TOP)
            {
                this.tableAlign = 0;
            }
            else if (this.align == TableAlign.BOTTOM)
            {
                this.tableAlign = this.totalVertFrameSpacing;
            }
            else if (this.align == TableAlign.CENTER)
            {
                this.tableAlign = this.totalVertFrameSpacing / 2;
            }
            else if (this.align == TableAlign.AXIS)
            {
                this.tableAlign = this.totalVertFrameSpacing / 2;
            }
            else if (this.align == TableAlign.BASELINE)
            {
                this.tableAlign = this.totalVertFrameSpacing / 2;
            }
        }

        public void update(int fontWidth, int fontHeight)
        {
            int x = this.node_.box.X;
            int y = this.node_.box.Y;
            
            for (int i = 0; i < this.ColCount; i++)
            {
                this.spanBases[i] = this.rowFrameSpacing;
                for (int j = 0; j < i; j++)
                {
                    this.spanBases[i] += this.spanWidth[j];
                    this.spanBases[i] += this.spanHeight[j];
                }
            }
            
            for (int i = 0; i < this.RowCount; i++)
            {
                MRow row = this.GetRow(i);
                row.node.box.X = x + this.rowFrameSpacing;
                int max_y = y + this.colFrameSpacing;
                for (int j = 0; j < i; j++)
                {
                    max_y += this.spacingWidth[j];
                    max_y += this.heights[j];
                }
                row.node.box.Y = max_y;
            }

            for (int i = 0; i < this.RowCount; i++)
            {
                int rindex;
                MRow row = this.GetRow(i);
                if (row.isLabeled)
                {
                    MCell cell = row.cell;
                    switch (cell.rowAlign)
                    {
                        case RowAlign.TOP:
                        {
                            cell.node.box.Y = row.node.box.Y;
                            break;
                        }
                        case RowAlign.BOTTOM:
                        {
                            cell.node.box.Y = (row.node.box.Y + this.heights[i]) - cell.node.box.Height;
                            break;
                        }
                        case RowAlign.CENTER:
                        {
                            cell.node.box.Y = row.node.box.Y + ((this.heights[i] - cell.node.box.Height)/2);
                            break;
                        }
                        case RowAlign.BASELINE:
                        case RowAlign.AXIS:
                            cell.node.box.Y = (row.node.box.Y + this.baselines[i]) - cell.node.box.Baseline;
                            break;

                        default:
                            break;
                    }

                    switch (cell.columnAlign)
                    {
                        case HAlign.LEFT:
                        {
                            cell.node.box.X = (x + this.totalHorzFrameSpacing) + this.minWidth;
                            break;
                        }
                        case HAlign.CENTER:
                        {
                            cell.node.box.X = ((x + this.totalHorzFrameSpacing) + this.minWidth) + ((this.maxWidth - cell.node.box.Width)/2);
                            break;
                        }
                        case HAlign.RIGHT:
                            cell.node.box.X = (((x + this.totalHorzFrameSpacing) + this.minWidth) + this.maxWidth) - 
                                                     cell.node.box.Width;
                            break;

                        default:
                            break;
                    }
                }
            
                rindex = 0;
                while (rindex < row.Count)
                {
                    MCell mCell = (MCell) row.cells[rindex];
                    int colspan = mCell.colSpan;
                    int tcspan = this.TotalColSpan(colspan, mCell.columnSpan);
                    int rspan = this.rowspanWidth(i, mCell.rowSpan);
                    switch (mCell.rowAlign)
                    {
                        case RowAlign.TOP:
                        {
                            mCell.node.box.Y = row.node.box.Y;
                            break;
                        }
                        case RowAlign.BOTTOM:
                        {
                            mCell.node.box.Y = (row.node.box.Y + rspan) - mCell.node.box.Height;
                            break;
                        }
                        case RowAlign.CENTER:
                        {
                            mCell.node.box.Y = row.node.box.Y + ((rspan - mCell.node.box.Height) / 2);
                            break;
                        }
                        case RowAlign.BASELINE:
                        case RowAlign.AXIS:
                            mCell.node.box.Y = (row.node.box.Y + this.baselines[i]) - mCell.node.box.Baseline;
                            break;
                    }
                    
                
                    switch (mCell.columnAlign)
                    {
                        case HAlign.LEFT:
                        {
                            mCell.node.box.X = x + this.spanBases[colspan];
                            break;
                        }
                        case HAlign.CENTER:
                        {
                            mCell.node.box.X = (x + this.spanBases[colspan]) + ((tcspan - mCell.node.box.Width) / 2);
                            break;
                        }
                        case HAlign.RIGHT:
                            mCell.node.box.X = ((x + this.spanBases[colspan]) + tcspan) - mCell.node.box.Width;
                            break;
                    }
                
                    rindex++;
                }
            }
        }

        public void RowAligns()
        {
            RowAlign rowAlign = RowAlign.BASELINE;
            bool same = true;
            for (int i = 0; i < this.RowCount; i++)
            {
                MRow row = (MRow) this.rows[i];
                same = true;
                for (int j = 0; j < row.Count; j++)
                {
                    MCell cell = (MCell) row.cells[j];
                    if ((j > 0) && (cell.rowAlign != rowAlign))
                    {
                        same = false;
                    }
                    rowAlign = cell.rowAlign;
                }
                if (same && (rowAlign != RowAlign.UNKNOWN))
                {
                    row.align = rowAlign;
                    for (int j = 0; j < row.Count; j++)
                    {
                        MCell cell = (MCell) row.cells[j];
                        cell.rowAlign = RowAlign.UNKNOWN;
                    }
                }
                for (int ii = 0; ii < row.Count; ii++)
                {
                    MCell cell = (MCell) row.cells[ii];
                    if (row.align != RowAlign.UNKNOWN)
                    {
                        if (cell.rowAlign == row.align)
                        {
                            cell.rowAlign = RowAlign.UNKNOWN;
                        }
                    }
                    else if (ii < this.rowAligns.Length)
                    {
                        if (cell.rowAlign == this.rowAligns[ii])
                        {
                            cell.rowAlign = RowAlign.UNKNOWN;
                        }
                    }
                    else if ((this.rowAligns.Length > 0) && (cell.rowAlign == this.rowAligns[this.rowAligns.Length - 1]))
                    {
                        cell.rowAlign = RowAlign.UNKNOWN;
                    }
                }
            }
            same = true;
            for (int i = 0; (i < this.RowCount) && same; i++)
            {
                MRow row = (MRow) this.rows[i];
                if ((i > 0) && (row.align != rowAlign))
                {
                    same = false;
                }
                rowAlign = row.align;
            }
            if (same)
            {
                this.rowAligns = new RowAlign[] { rowAlign };
                for (int i = 0; i < this.RowCount; i++)
                {
                    MRow row = (MRow) this.rows[i];
                    row.align = RowAlign.UNKNOWN;
                }
            }
            for (int i = 0; i < this.RowCount; i++)
            {
                MRow row = (MRow) this.rows[i];
                if (i < this.rowAligns.Length)
                {
                    if (row.align == this.rowAligns[i])
                    {
                        row.align = RowAlign.UNKNOWN;
                    }
                }
                else if ((this.rowAligns.Length > 0) && (row.align == this.rowAligns[this.rowAligns.Length - 1]))
                {
                    row.align = RowAlign.UNKNOWN;
                }
            }
        }

        public MRow GetRow(int index)
        {
            if (index < this.RowCount)
            {
                return (MRow) this.rows[index];
            }
            return null;
        }

        private MRow AddRow(Node node, int index)
        {
            MRow row = new MRow(this, node, index);
            this.rows.Add(row);
            return row;
        }

        public void SelectAll()
        {
            try
            {
                this.selKind_ = TableCellKind.SelAll;
                
            }
            catch
            {
            }
        }

        public void SetCurRow(int nRow)
        {
            try
            {
                this.selKind_ = TableCellKind.RowSelected;
                this.curRow = nRow;
                
            }
            catch
            {
            }
        }

        public void SetCurCol(int nCol)
        {
            try
            {
                this.selKind_ = TableCellKind.ColSelected;
                this.curCol = nCol;
                
            }
            catch
            {
            }
        }

        public void SetCurRowCol(int nRow, int nCol)
        {
            this.selKind_ = TableCellKind.RowColSelected;
            this.curRow = nRow;
            this.curCol = nCol;

            if (nCol == this.ColCount)
            {
                this.selKind_ = TableCellKind.BottomSelected;
            }
        }

        public MCell Get(int nRow, int nCol)
        {
            int count = ((MRow) this.rows[nRow]).cells.Count;
            for (int i = 0; i < count; i++)
            {
                if (((MCell) ((MRow) this.rows[nRow]).cells[i]).colSpan == nCol)
                {
                    return (MCell) ((MRow) this.rows[nRow]).cells[i];
                }
            }
            return null;
        }

        public void SetColAlign(HAlign colalign)
        {
            switch (this.selKind_)
            {
                case TableCellKind.ColSelected:
                {
                    int i = 0;

                    while (i < this.RowCount)
                    {
                        MCell cell = this.Get(i, this.curCol);
                        if (cell != null)
                        {
                            cell.columnAlign = colalign;
                        }
                        i++;
                    }
                    break;
                }
                case TableCellKind.RowColSelected:
                {
                    this.Get(this.curRow, this.curCol).columnAlign = colalign;
                    return;
                }
                case TableCellKind.BottomSelected:
                {
                    MRow row = this.GetRow(this.curRow);
                    if (row.isLabeled && (row.cell != null))
                    {
                        row.cell.columnAlign = colalign;
                    }
                    return;
                }
            }
        
        }

        public int TotalColSpan(int nCol, int nColSpan)
        {
            int r = 0;
            if (nCol < this.ColCount)
            {
                for (int i = 0; i < nColSpan; i++)
                {
                    r += this.spanHeight[nCol + i];
                    if (i < (nColSpan - 1))
                    {
                        r += this.spanWidth[nCol + i];
                    }
                }
            }
            return r;
        }

        public void SetRowAlign(RowAlign rowalign)
        {
            switch (this.selKind_)
            {
                case TableCellKind.RowSelected:
                {
                    MRow row = this.GetRow(this.curRow);
                    int i = 0;
                    while (i < row.cells.Count)
                    {
                        MCell cell = (MCell) row.cells[i];
                        if (cell != null)
                        {
                            cell.rowAlign = rowalign;
                        }
                        i++;
                    }
                    return;
                }
                case TableCellKind.ColSelected:
                    return;

                case TableCellKind.RowColSelected:
                {
                    this.Get(this.curRow, this.curCol).rowAlign = rowalign;
                    return;
                }
                case TableCellKind.BottomSelected:
                {
                    if (this.GetRow(this.curRow).isLabeled && (this.GetRow(this.curRow).cell != null))
                    {
                        this.GetRow(this.curRow).cell.rowAlign = rowalign;
                    }
                    return;
                }
                default:
                    return;
            }
       
        }

        public void SetRowSpacing(string sRowSpacing)
        {
            if (this.selKind_ == TableCellKind.RowSelected)
            {
                this.GetRow(this.curRow).spacing = sRowSpacing;
            }
        }

        public void SetLineStyle(TableLineStyle rowlines)
        {
            if (this.selKind_ == TableCellKind.RowSelected)
            {
                MRow row = this.GetRow(this.curRow);
                row.lines = rowlines;
            }
        }

        public TableLineStyle GetTableLineStyle(int nCol)
        {
            if (nCol < this.colLines.Length)
            {
                return this.colLines[nCol];
            }
            if (this.colLines.Length > 0)
            {
                return this.colLines[this.colLines.Length - 1];
            }
            return TableLineStyle.NONE;
        }

        public string GetColSpacing(int nCol)
        {
            if (nCol < this.colSpacing.Length)
            {
                return this.colSpacing[nCol];
            }
            if (this.colSpacing.Length > 0)
            {
                return this.colSpacing[this.colSpacing.Length - 1];
            }
            return "0.8em";
        }

        public void SetColSpacing(string sColSpacing)
        {
            if (this.selKind_ == TableCellKind.ColSelected)
            {
                this.colSpacing[this.curCol] = sColSpacing;
            }
        }

        public void SetTableLineStyle(TableLineStyle colLines)
        {
            if (this.selKind_ == TableCellKind.ColSelected)
            {
                this.colLines[this.curCol] = colLines;
            }
        }

        public int rowspanWidth(int nRow, int nRowSpan)
        {
            int r = 0;
            if (nRow < this.RowCount)
            {
                for (int i = 0; i < nRowSpan; i++)
                {
                    r += this.heights[nRow + i];
                    if (i < (nRowSpan - 1))
                    {
                        r += this.spacingWidth[nRow + i];
                    }
                }
            }
            return r;
        }

        private void UpdateRowspan()
        {
            for (int i = 0; i < this.rows.Count; i++)
            {
                for (int j = 0; j < ((MRow) this.rows[i]).cells.Count; j++)
                {
                    if ((((MCell) ((MRow) this.rows[i]).cells[j]).tableAttrs != null) && (((MCell) ((MRow) this.rows[i]).cells[j]).tableAttrs.rowSpan > 1))
                    {
                        MCell cell = (MCell) ((MRow) this.rows[i]).cells[j];
                        if (cell.rowSpan > 1)
                        {
                            int cspan = cell.colSpan;
                            for (int k = 1; k < cell.rowSpan; k++)
                            {
                                int ci = 0;
                                bool err = false;
                                int numCells = 0;
                                while (((cspan + ci) < this.ColCount) && !err)
                                {
                                    try
                                    {
                                        if (this.Get(i + k, cspan + ci) != null)
                                        {
                                            numCells++;
                                        }
                                        ci++;
                                        continue;
                                    }
                                    catch
                                    {
                                        err = true;
                                        continue;
                                    }
                                }
                                MCell[] cells = new MCell[numCells];
                                ci = 0;
                                while (((cspan + ci) < this.ColCount) && !err)
                                {
                                    try
                                    {
                                        MCell cll = this.Get(i + k, cspan + ci);
                                        if (cll != null)
                                        {
                                            cells[ci] = cll;
                                        }
                                        ci++;
                                        continue;
                                    }
                                    catch
                                    {
                                        err = true;
                                        continue;
                                    }
                                }
                                for (int c = 0; c < numCells; c++)
                                {
                                    cells[c].colSpan += cell.columnSpan;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void FixHAligns()
        {
            for (int i = 0; i < this.RowCount; i++)
            {
                MRow row = (MRow) this.rows[i];
                if (row.isLabeled && (row.cell != null))
                {
                    row.cell.rowAlign = row.cell.GetRowAlign();
                }
                for (int j = 0; j < row.Count; j++)
                {
                    ((MCell) row.cells[j]).rowAlign = ((MCell) row.cells[j]).GetRowAlign();
                }
            }
        }

        public void FixVAligns()
        {
            for (int i = 0; i < this.RowCount; i++)
            {
                MRow row = (MRow) this.rows[i];
                if (row.isLabeled && (row.cell != null))
                {
                    row.cell.columnAlign = row.cell.GetColAlign();
                }
                for (int j = 0; j < row.Count; j++)
                {
                    ((MCell) row.cells[j]).columnAlign = ((MCell) row.cells[j]).GetColAlign();
                }
            }
        }

        public void UpdateUpDownTies()
        {
            for (int i = 0; i < (this.rows.Count - 1); i++)
            {
                MRow row = this.GetRow(i);
                for (int j = 0; j < row.Count; j++)
                {
                    try
                    {
                        MCell cell = (MCell) row.cells[j];
                        int nCol = cell.colSpan;
                        int nRow = i + cell.rowSpan;
                        if (nRow < this.rows.Count)
                        {
                            MCell c = this.Get(nRow, nCol);
                            if (c != null)
                            {
                                c.node.upperNode = cell.node;
                                cell.node.lowerNode = c.node;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public void ApplyAttrs()
        {
            try
            {
                TableAttributes attrs;
                this.ColAligns();
                this.RowAligns();
                if (this.attrs != null)
                {
                    attrs = this.attrs;
                }
                else
                {
                    attrs = new TableAttributes();
                }
                attrs.colAligns = this.colAligns;
                attrs.rowAligns = this.rowAligns;
                attrs.rowSpacing = new string[this.rows.Count];
                attrs.rowLines = new TableLineStyle[this.rows.Count];
                attrs.colSpacing = this.colSpacing;
                attrs.colLines = this.colLines;
                attrs.displaystyle = this.displayStyle;
                attrs.equalColumns = this.equalColumns;
                attrs.equalRows = this.equalRows;
                attrs.align = this.align;
                attrs.frame = this.frame;
                attrs.framespacing = this.framespacing;
                attrs.side = this.side;
                attrs.minlabelspacing = this.minlabelSpacing;
                bool sameStyle = true;
                TableLineStyle lineStyle = TableLineStyle.NONE;
                for (int i = 0; i < attrs.colLines.Length; i++)
                {
                    if ((i > 0) && (attrs.colLines[i] != lineStyle))
                    {
                        sameStyle = false;
                    }
                    lineStyle = attrs.colLines[i];
                }
                if (sameStyle)
                {
                    attrs.colLines = new TableLineStyle[] { lineStyle };
                }
                bool sameColSpacing = true;
                string colSpacing = "0.8em";
                for (int i = 0; i < attrs.colSpacing.Length; i++)
                {
                    if ((i > 0) && (attrs.colSpacing[i] != colSpacing))
                    {
                        sameColSpacing = false;
                    }
                    colSpacing = attrs.colSpacing[i];
                }
                if (sameColSpacing)
                {
                    attrs.colSpacing = new string[] { colSpacing };
                }
                for (int i = 0; i < this.rows.Count; i++)
                {
                    MRow row = this.GetRow(i);
                    attrs.rowSpacing[i] = row.spacing;
                    attrs.rowLines[i] = row.lines;
                    Node node = row.node;
                    TableRowAttributes rowAttributes = row.attrs;
                    if (rowAttributes != null)
                    {
                        rowAttributes.colAligns = row.colAligns;
                        rowAttributes.align = row.align;
                        AttributeBuilder.ApplyAttributes(node, rowAttributes);
                    }
                    else
                    {
                        rowAttributes = new TableRowAttributes();
                        rowAttributes.colAligns = row.colAligns;
                        rowAttributes.align = row.align;
                        AttributeBuilder.ApplyAttributes(node, rowAttributes);
                    }
                    if ((row.isLabeled && (row.cell != null)) && (row.cell.node != null))
                    {
                        TableCellAttributes cellAttributes = row.cell.tableAttrs;
                        Node n = row.cell.node;
                        if (cellAttributes != null)
                        {
                            cellAttributes.columnAlign = row.cell.columnAlign;
                            cellAttributes.rowAlign = row.cell.rowAlign;
                            AttributeBuilder.ApplyAttrs(n, cellAttributes);
                        }
                        else
                        {
                            cellAttributes = new TableCellAttributes();
                            cellAttributes.columnAlign = row.cell.columnAlign;
                            cellAttributes.rowAlign = row.cell.rowAlign;
                            AttributeBuilder.ApplyAttrs(n, cellAttributes);
                        }
                    }
                    for (int j = 0; j < ((MRow) this.rows[i]).cells.Count; j++)
                    {
                        Node n = ((MCell) ((MRow) this.rows[i]).cells[j]).node;
                        TableCellAttributes cellAttributes = ((MCell) ((MRow) this.rows[i]).cells[j]).tableAttrs;
                        RowAlign rowAlign = ((MCell) ((MRow) this.rows[i]).cells[j]).rowAlign;
                        if (cellAttributes != null)
                        {
                            cellAttributes.columnAlign = ((MCell) ((MRow) this.rows[i]).cells[j]).columnAlign;
                            cellAttributes.rowAlign = ((MCell) ((MRow) this.rows[i]).cells[j]).rowAlign;
                            AttributeBuilder.ApplyAttrs(n, cellAttributes);
                        }
                        else
                        {
                            cellAttributes = new TableCellAttributes();
                            cellAttributes.columnAlign = ((MCell) ((MRow) this.rows[i]).cells[j]).columnAlign;
                            cellAttributes.rowAlign = ((MCell) ((MRow) this.rows[i]).cells[j]).rowAlign;
                            AttributeBuilder.ApplyAttrs(n, cellAttributes);
                        }
                    }
                }
                bool sameRowL = true;
                TableLineStyle tableLineStyle = TableLineStyle.NONE;
                for (int i = 0; i < attrs.rowLines.Length; i++)
                {
                    if ((i > 0) && (attrs.rowLines[i] != tableLineStyle))
                    {
                        sameRowL = false;
                    }
                    tableLineStyle = attrs.rowLines[i];
                }
                if (sameRowL)
                {
                    attrs.rowLines = new TableLineStyle[] { tableLineStyle };
                }
                bool sameRowS = true;
                string rowSpacing = "0.5ex";
                for (int i = 0; i < attrs.rowSpacing.Length; i++)
                {
                    if ((i > 0) && (attrs.rowSpacing[i] != rowSpacing))
                    {
                        sameRowS = false;
                    }
                    rowSpacing = attrs.rowSpacing[i];
                }
                if (sameRowS)
                {
                    attrs.rowSpacing = new string[] { rowSpacing };
                }
                AttributeBuilder.ApplyAttrs(this.node_, attrs);
            }
            catch
            {
            }
        }

        public void ColAligns()
        {
            HAlign hAlign = HAlign.CENTER;
            bool same = true;
            for (int row_index = 0; row_index < this.RowCount; row_index++)
            {
                MRow row = (MRow) this.rows[row_index];
                same = true;
                for (int col_index = 0; col_index < row.Count; col_index++)
                {
                    MCell cell = (MCell) row.cells[col_index];
                    if ((col_index > 0) && (cell.columnAlign != hAlign))
                    {
                        same = false;
                    }
                    hAlign = cell.columnAlign;
                }
                if (same && (hAlign != HAlign.UNKNOWN))
                {
                    row.colAligns = new HAlign[] { hAlign };
                    for (int i = 0; i < row.Count; i++)
                    {
                        MCell cell = (MCell) row.cells[i];
                        cell.columnAlign = HAlign.UNKNOWN;
                    }
                }
                for (int rowndex  = 0; rowndex < row.Count; rowndex++)
                {
                    MCell cell = (MCell) row.cells[rowndex];
                    if (cell.colSpan < row.colAligns.Length)
                    {
                        if (row.colAligns[cell.colSpan] != HAlign.UNKNOWN)
                        {
                            if (cell.columnAlign == row.colAligns[cell.colSpan])
                            {
                                cell.columnAlign = HAlign.UNKNOWN;
                            }
                        }
                        else if (cell.colSpan < this.colAligns.Length)
                        {
                            if (cell.columnAlign == this.colAligns[cell.colSpan])
                            {
                                cell.columnAlign = HAlign.UNKNOWN;
                            }
                        }
                        else if ((this.colAligns.Length > 0) && (cell.columnAlign == this.colAligns[this.colAligns.Length - 1]))
                        {
                            cell.columnAlign = HAlign.UNKNOWN;
                        }
                    }
                    else if (row.colAligns.Length > 0)
                    {
                        if (row.colAligns[row.colAligns.Length - 1] != HAlign.UNKNOWN)
                        {
                            if (cell.columnAlign == row.colAligns[row.colAligns.Length - 1])
                            {
                                cell.columnAlign = HAlign.UNKNOWN;
                            }
                        }
                        else if (cell.colSpan < this.colAligns.Length)
                        {
                            if (cell.columnAlign == this.colAligns[cell.colSpan])
                            {
                                cell.columnAlign = HAlign.UNKNOWN;
                            }
                        }
                        else if ((this.colAligns.Length > 0) && (cell.columnAlign == this.colAligns[this.colAligns.Length - 1]))
                        {
                            cell.columnAlign = HAlign.UNKNOWN;
                        }
                    }
                }
            }
            same = true;
            for (int row_index = 0; (row_index < this.RowCount) && same; row_index++)
            {
                MRow row = (MRow) this.rows[row_index];
                if (row.colAligns.Length != 1)
                {
                    same = false;
                }
                if (((row_index > 0) && same) && (row.colAligns[0] != hAlign))
                {
                    same = false;
                }
            }
            if (same)
            {
                this.colAligns = new HAlign[] { hAlign };
                for (int row_index = 0; row_index < this.RowCount; row_index++)
                {
                    MRow row = (MRow) this.rows[row_index];
                    row.colAligns = new HAlign[] { HAlign.UNKNOWN };
                }
            }
            for (int i = 0; i < this.RowCount; i++)
            {
                MRow row = (MRow) this.rows[i];
                if (row.colAligns.Length == this.colAligns.Length)
                {
                    bool b = true;
                    for (int j = 0; j < row.colAligns.Length; j++)
                    {
                        if (row.colAligns[j] != this.colAligns[j])
                        {
                            b = false;
                        }
                    }
                    if (b)
                    {
                        row.colAligns = new HAlign[] { HAlign.UNKNOWN };
                    }
                }
            }
        }

        public int RowCount
        {
            get
            {
                return this.rows.Count;
            }
        }

        public int ColCount
        {
            get
            {
                return this.colColSpan;
            }
        }

        private int colColSpan;
        public int totalHorzFrameSpacing;
        public int[] spanWidth;
        public int[] heights;
        public int[] baselines;
        public int[] spacingWidth;
        public int maxWidth;
        public int curRow;
        public int curCol;
        public TableCellKind selKind_;
        public Node node_;
        public int totalWidth;
        public RowAlign[] rowAligns;
        public HAlign[] colAligns;
        public ArrayList rows;
        public TableAttributes attrs;
        public TableLineStyle[] colLines;
        public string[] colSpacing;
        public bool displayStyle;
        public bool equalRows;
        public bool equalColumns;
        public TableAlign align;
        public int minWidth;
        public TableLineStyle frame;
        public string framespacing;
        public Side side;
        public string minlabelSpacing;
        public int totalVertFrameSpacing;
        public int tableAlign;
        public int rowFrameSpacing;
        public int colFrameSpacing;
        public int[] spanBases;
        public int[] spanHeight;
    }
}

