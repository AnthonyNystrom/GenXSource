namespace Attrs
{
    public class ActionAttributes
    {
        public ActionAttributes()
        {
            this.actionType = ActionType.StatusLine;
            this.actionString = "statusline";
            this.selection = 1;
        }


        public ActionType actionType;
        public string actionString;
        public int selection;
    }

    public enum ActionType
    {
        StatusLine,
        ToolTip,
        Toggle,
        Highlight,
        Unknown
    }

    public enum DisplayStyle
    {
        AUTOMATIC,
        TRUE,
        FALSE
    }

    public class FencedAttributes
    {
        public FencedAttributes()
        {
            this.open = "(";
            this.close = ")";
            this.separators = ",";
        }


        public string open;
        public string close;
        public string separators;
    }

    public enum FractionAlign
    {
        LEFT = 1,
        CENTER = 2,
        RIGHT = 3
    }

    public class FractionAttributes
    {
        public FractionAttributes()
        {
            this.lineThickness = 1;
            this.numAlign = FractionAlign.CENTER;
            this.denomAlign = FractionAlign.CENTER;
            this.isBevelled = false;
        }


        public int lineThickness;
        public FractionAlign numAlign;
        public FractionAlign denomAlign;
        public bool isBevelled;
    }

    public enum HAlign
    {
        LEFT,
        CENTER,
        RIGHT,
        UNKNOWN
    }

    public class QuoteAttributes
    {
        public QuoteAttributes()
        {
            this.lquote = "&quot;";
            this.rquote = "&quot;";
        }


        public string lquote;
        public string rquote;
    }

    public enum RowAlign
    {
        TOP,
        BOTTOM,
        CENTER,
        BASELINE,
        AXIS,
        UNKNOWN
    }

    public enum ScriptLevel
    {
        NONE,
        ZERO,
        ONE,
        TWO,
        PLUS_ONE,
        PLUS_TWO,
        MINUS_ONE,
        MINUS_TWO
    }

     public enum Side
    {
        LEFT,
        RIGHT,
        LEFTOVERLAP,
        RIGHTOVERLAP
    }

    public enum TableAlign
    {
        TOP,
        BOTTOM,
        CENTER,
        BASELINE,
        AXIS
    }

     public class TableAttributes
    {
        public TableAttributes()
        {
            this.align = TableAlign.AXIS;
            this.frame = TableLineStyle.NONE;
            this.framespacing = "0.4em 0.5ex";
            this.equalRows = false;
            this.equalColumns = false;
            this.displaystyle = false;
            this.side = Side.RIGHT;
            this.minlabelspacing = "0.8em";
            this.rowLines = new TableLineStyle[] { TableLineStyle.NONE };
            this.colLines = new TableLineStyle[] { TableLineStyle.NONE };
            this.rowSpacing = new string[] { "0.5ex" };
            this.colSpacing = new string[] { "0.8em" };
            this.rowAligns = new RowAlign[] { RowAlign.BASELINE };
            this.colAligns = new HAlign[] { HAlign.CENTER };
        }


        public TableAlign align;
        public TableLineStyle frame;
        public string[] colSpacing;
        public TableLineStyle[] colLines;
        public Side side;
        public string minlabelspacing;
        public string framespacing;
        public bool equalRows;
        public bool equalColumns;
        public bool displaystyle;
        public RowAlign[] rowAligns;
        public string[] rowSpacing;
        public TableLineStyle[] rowLines;
        public HAlign[] colAligns;
    }

    public class TableCellAttributes
    {
        public TableCellAttributes()
        {
            this.rowAlign = RowAlign.UNKNOWN;
            this.columnAlign = HAlign.UNKNOWN;
            this.rowSpan = 1;
            this.columnSpan = 1;
        }


        public RowAlign rowAlign;
        public HAlign columnAlign;
        public int rowSpan;
        public int columnSpan;
    }

    public enum TableLineStyle
    {
        NONE,
        SOLID,
        DASHED
    }

    public class TableRowAttributes
    {
        public TableRowAttributes()
        {
            this.align = RowAlign.UNKNOWN;
            this.colAligns = new HAlign[] { HAlign.UNKNOWN };
        }

        public RowAlign align;
        public HAlign[] colAligns;
    }
}