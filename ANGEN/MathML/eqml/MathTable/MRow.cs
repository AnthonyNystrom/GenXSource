namespace MathTable
{
    using Attrs;
    using Nodes;
    using System;
    using System.Collections;

    public class MRow
    {
        public MRow(MTable matrix, Node node, int index)
        {
            this.isLabeled = false;
            this.index = 0;
            this.align = RowAlign.UNKNOWN;
            this.spacing = "0.5ex";
            this.lines = TableLineStyle.NONE;
            this.index = index;
            this.attrs = AttributeBuilder.MRowAttributes(node);
            this.colAligns = new HAlign[] { HAlign.UNKNOWN };
            this.matrix = matrix;
            this.node = node;
            this.cells = new ArrayList();
        }

        public MCell Get(int index)
        {
            if (index < this.Count)
            {
                return (MCell) this.cells[index];
            }
            return null;
        }

        public MCell AddCell(Node node, int index)
        {
            MCell cell = new MCell(this, node, index);
            this.cells.Add(cell);
            return cell;
        }

        public MCell AddLabel(Node node, int index)
        {
            this.cell = new MCell(this, node, index);
            return this.cell;
        }


        public int Count
        {
            get
            {
                return this.cells.Count;
            }
        }


        public bool isLabeled;
        public MCell cell;
        public string spacing;
        public TableLineStyle lines;
        public int index;
        public Node node;
        public MTable matrix;
        public RowAlign align;
        public HAlign[] colAligns;
        public ArrayList cells;
        public TableRowAttributes attrs;
    }
}

