namespace MathTable
{
    using Attrs;
    using Nodes;
    using System;

    public class MCell
    {
        public MCell(MRow row, Node node, int index)
        {
            this.rowAlign = RowAlign.UNKNOWN;
            this.columnAlign = HAlign.UNKNOWN;
            this.columnSpan = 1;
            this.rowSpan = 1;
            this.colSpan = 0;
            this.tableAttrs = AttributeBuilder.FromNode(node);
            this.row_ = row;
            this.node = node;
            this.colSpan = index;
        }

        public HAlign GetColAlign()
        {
            if (this.columnAlign != HAlign.UNKNOWN)
            {
                return this.columnAlign;
            }
            if ((this.colSpan < this.row_.colAligns.Length) && (this.row_.colAligns[this.colSpan] != HAlign.UNKNOWN))
            {
                return this.row_.colAligns[this.colSpan];
            }
            if ((this.row_.colAligns.Length > 0) && (this.row_.colAligns[this.row_.colAligns.Length - 1] != HAlign.UNKNOWN))
            {
                return this.row_.colAligns[this.row_.colAligns.Length - 1];
            }
            if (this.colSpan < this.row_.matrix.colAligns.Length)
            {
                return this.row_.matrix.colAligns[this.colSpan];
            }
            if (this.row_.matrix.colAligns.Length > 0)
            {
                return this.row_.matrix.colAligns[this.row_.matrix.colAligns.Length - 1];
            }
            return HAlign.CENTER;
        }

        public RowAlign GetRowAlign()
        {
            if (this.rowAlign != RowAlign.UNKNOWN)
            {
                return this.rowAlign;
            }
            if (this.row_.align != RowAlign.UNKNOWN)
            {
                return this.row_.align;
            }
            if (this.colSpan < this.row_.matrix.rowAligns.Length)
            {
                return this.row_.matrix.rowAligns[this.colSpan];
            }
            if (this.row_.matrix.rowAligns.Length > 0)
            {
                return this.row_.matrix.rowAligns[this.row_.matrix.rowAligns.Length - 1];
            }
            return RowAlign.BASELINE;
        }


        public Node node;
        public MRow row_;
        public RowAlign rowAlign;
        public HAlign columnAlign;
        public TableCellAttributes tableAttrs;
        public int columnSpan;
        public int rowSpan;
        public int colSpan;
    }
}

