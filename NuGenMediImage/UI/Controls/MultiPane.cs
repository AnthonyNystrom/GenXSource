using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    /// <summary>
    /// Summary description for Multiplexer.
    /// </summary>
    public class MultiPane : System.Windows.Forms.Panel
    {
        public event EventHandler SelectedIndexChanged;


        private NuGenMediImageCtrl ngMediImage;

        public NuGenMediImageCtrl NgMediImage
        {
            get { return ngMediImage; }
            set { ngMediImage = value; }
        }

        private Control[,] ViewerAnnotPanes;

        private bool fitToWindow = false;
        private bool singleWindowMode = true;
        private bool windowsVisible = false;

        private int rows = 1;
        private int cols = 1;
        private int cellWidth = 320;
        private int cellHeight = 240;
        private bool _autoScrollVar = true;

        private ViewerAnnot lastClicked;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public bool MultiPaneAutoScroll
        {
            get
            {
                return _autoScrollVar;
            }
            set
            {
                _autoScrollVar = value;
                // show/hide all windows
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        try
                        {
                            ((ViewerAnnot)ViewerAnnotPanes[i, j].Controls[0]).AutoScroll = value;
                        }
                        catch { }
                    }
                }
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                base.BackColor = value;

                // Add ViewerAnnots
                for (int i = 0; i < this.Rows; i++)
                {
                    for (int j = 0; j < this.Cols; j++)
                    {
                        ViewerAnnot v = this.GetViewer(i, j);
                        if (v != null && v.Parent!=null)
                        {                           
                            ((SelectablePanel)v.Parent).BackColor = value;
                        }
                    }
                }
            }
        }

        // FitToWindow property
        [DefaultValue(false)]
        public bool FitToWindow
        {
            get { return fitToWindow; }
            set
            {
                fitToWindow = value;

                if ((ViewerAnnotPanes[0, 0].AutoSize = (!fitToWindow && singleWindowMode)) == true)
                {
                    //camWindows[0, 0].UpdatePosition();
                }
                else
                {
                    UpdateSize();
                }
            }
        }
        // SingleCameraMode property
        [DefaultValue(true)]
        public bool SingleWindowMode
        {
            get { return singleWindowMode; }
            set
            {
                singleWindowMode = value;
                if (!fitToWindow)
                    ViewerAnnotPanes[0, 0].AutoSize = value;
            }
        }
        // CamerasVisible property
        [DefaultValue(false)]
        public bool PanesVisible
        {
            get { return windowsVisible; }
            set
            {
                windowsVisible = value;

                // show/hide all windows
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        ViewerAnnotPanes[i, j].Visible = value;
                    }
                }
            }
        }
        // Rows property
        [DefaultValue(1)]
        public int Rows
        {
            get { return rows; }
            set
            {
                rows = value;
                InitPaneWindows();
                UpdateVisiblity();
                UpdateSize();
            }
        }
        // Cols property
        [DefaultValue(1)]
        public int Cols
        {
            get { return cols; }
            set
            {
                cols = value;
                InitPaneWindows();
                UpdateVisiblity();
                UpdateSize();
            }
        }
        // CellWidth
        [DefaultValue(320)]
        public int CellWidth
        {
            get { return cellWidth; }
            set
            {
                cellWidth = Math.Max(50, Math.Min(800, value));
                UpdateSize();
            }
        }
        // CellHeight
        [DefaultValue(240)]
        public int CellHeight
        {
            get { return cellHeight; }
            set
            {
                cellHeight = Math.Max(50, Math.Min(800, value));
                UpdateSize();
            }
        }

        [Browsable(false)]
        public ViewerAnnot Selected
        {
            get { return (lastClicked == null) ? null : lastClicked; }
        }

        // Constructor
        public MultiPane()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();

            base.AutoScroll = false;

            // TODO: Add any initialization after the InitForm call
            //camWindows = new Control[rows, MaxCols];
            InitPaneWindows();
        }

        private void InitPaneWindows()
        {
            this.Controls.Clear();
            ViewerAnnotPanes = null;
            ViewerAnnotPanes = new Control[rows, cols];
            
            // Add Panels
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    SelectablePanel p = new SelectablePanel();                    
                    p.Padding = new Padding(5);
                    //p.BackColor = ngMediImage.GetColorConfig().MultiPaneBackColor;

                    this.Controls.Add(p);
                    p.SelectedChanged += new EventHandler(p_SelectedChanged);

                    ViewerAnnotPanes[i, j] = p;
                }
            }

            // Add ViewerAnnots
            for (int i = 0; i < this.Rows; i++)
            {
                for (int j = 0; j < this.Cols; j++)
                {
                    ViewerAnnot v = new ViewerAnnot();
                    //v.Size = new Size(50, 50);
                    v.Dock = DockStyle.Fill;
                    //v.Viewer.ZoomFitForMultiPane();

                    this.SetViewerAnnot(i, j, v);
                }
            }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Multiplexer
            // 

            this.Size = new System.Drawing.Size(424, 376);
            this.Resize += new System.EventHandler(this.Multiplexer_Resize);
            this.ResumeLayout(false);

        }
        #endregion


        // Close all panes
        public void CloseAll()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    ViewerAnnotPanes[i, j] = null;
                }
            }
        }

        // Set ViewerAnnot to the specified position of the multiplexer
        public void SetViewerAnnot(int row, int col, ViewerAnnot v)
        {
            if ((row >= 0) && (col >= 0) && (row < rows) && (col < cols))
            {
                v.AutoScroll = this.AutoScroll;
                v.MouseClick += new MouseEventHandler(v_MouseClick);
                ViewerAnnotPanes[row, col].Controls.Add(v);
            }
        }

        // Set ViewerAnnot to the specified position of the multiplexer
        public ViewerAnnot GetViewer(int row, int col)
        {
            if ((row >= 0) && (col >= 0) && (row < rows) && (col < cols))
            {
                return (ViewerAnnot)ViewerAnnotPanes[row, col].Controls[0];
            }

            return null;
        }

        // Set multiplexer size
        public void SetSize(int rows, int cols, int cellWidth, int cellHeight)
        {
            this.rows = rows;
            this.cols = cols;
            this.cellWidth = cellWidth;
            this.cellHeight = cellHeight;
            UpdateSize();
        }

        // Update panes visibility
        private void UpdateVisiblity()
        {
            if (windowsVisible)
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        ViewerAnnotPanes[i, j].Visible = ((i < rows) && (j < cols));
                    }
                }
            }
        }

        // Update panes size and position
        private void UpdateSize()
        {
            int width, height;

            if (!fitToWindow)
            {
                // original width & height
                width = cellWidth;
                height = cellHeight;
            }
            else
            {
                // calculate width & height of panes to fit the view to the window
                width = (ClientRectangle.Width / cols) - 4;
                height = (ClientRectangle.Height / rows) - 4;
            }

            // starting position of the view
            int startX = (ClientRectangle.Width - cols * (width + 4)) / 2;
            int startY = (ClientRectangle.Height - rows * (height + 4)) / 2;

            this.SuspendLayout();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    ViewerAnnotPanes[i, j].Location = new Point(startX + (width + 4) * j + 1, startY + (height + 4) * i + 1);
                    ViewerAnnotPanes[i, j].Size = new Size(width + 4, height + 4);
                }
            }

            this.ResumeLayout(false);
        }

        // On size changed
        private void Multiplexer_Resize(object sender, System.EventArgs e)
        {
            UpdateSize();
        }

        // On mouse down in camera window
        private void Control_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            lastClicked = (ViewerAnnot)sender;
        }


        void p_SelectedChanged(object sender, EventArgs e)
        {
            v_MouseClick(((SelectablePanel)sender).Controls[0], null);
        }

        void p_MouseLeave(object sender, EventArgs e)
        {
            ViewerAnnot v = (ViewerAnnot)sender;
            SelectablePanel p = (SelectablePanel)v.Parent;

            p.Hover = false;
        }

        void p_MouseEnter(object sender, EventArgs e)
        {

            ViewerAnnot v = (ViewerAnnot)sender;
            SelectablePanel p = (SelectablePanel)v.Parent;

            p.Hover = true;
        }


        void v_MouseClick(object sender, MouseEventArgs e)
        {
            ViewerAnnot v = (ViewerAnnot)sender;

            if (lastClicked == v)
                return;

            SelectablePanel p = (SelectablePanel)v.Parent;
            
            p.SuspendLayout();
            v.SuspendLayout();

            UnselectAll();
            
            //p.Padding = new Padding(5);
            p._selected = true;

            v.ResumeLayout();
            p.ResumeLayout();            

            lastClicked = v;

            if (SelectedIndexChanged != null)
            {
                SelectedIndexChanged(this, EventArgs.Empty);
            }

            this.Invalidate(true);
        }

        private void UnselectAll()
        {
            foreach (SelectablePanel x in this.Controls)
            {
                //x.Padding = new Padding(1);
                x._selected = false;                
            }
        }
    }

    public class SelectablePanel : System.Windows.Forms.Panel
    {
        internal bool _selected = false;
        private bool _hover = false;

        public SelectablePanel()
        {
            this.BackColor = Color.FromArgb(230, 233, 240);
        }

        public event EventHandler SelectedChanged;

        internal bool Hover
        {
            get { return _hover; }
            set
            {
                _hover = value;
                Invalidate();
            }
        }

        public bool Selected
        {
            get
            {
                return _selected;
            }
            set
            {   
                _selected = value;
                Invalidate();

                if( SelectedChanged!= null )
                    SelectedChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            if (_selected)
            {
                RibbonControl.RenderSelection(e.Graphics, rect, 2f, true);
            }
            //else if (_hover)
            //{
            //    RibbonControl.RenderSelection(e.Graphics, rect, 2f, this._selected);
            //}
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.Selected = true;
            this.Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            this._hover = true;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            this._hover = false;
            this.Invalidate();
        }
    }
}
