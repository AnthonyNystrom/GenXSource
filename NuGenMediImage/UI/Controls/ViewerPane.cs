using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Genetibase.NuGenMediImage.UI.Controls
{
    public partial class ViewerPane : UserControl
    {
        int paneCount = 0;
        ImageArray _images = null;

        Viewer _selected = null;

        

        public Viewer Selected
        {
            get { return _selected; }            
        }

        [Browsable(true)]
        public int PaneCount
        {
            get { return paneCount; }
            set { paneCount = value; Setup(); }
        }

        public ImageArray Images
        {
            get
            {
                return _images;
            }
            set
            {
                _images = value;
            }
        }

        private void Setup()
        {
            int columns = paneCount / 2;
            int rows = 2;

            this.tableLayoutPanel.RowCount = rows;
            this.tableLayoutPanel.ColumnCount = columns;
            this.tableLayoutPanel.RowStyles.Clear();
            this.tableLayoutPanel.ColumnStyles.Clear();


            for (int j = 0; j < columns; j++)
            {
                this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            }

            for (int i = 0; i < rows; i++)
            {
                this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent,50));
            }

            for (int j = 0; j < columns; j++)
            {
                for (int i = 0; i < rows; i++)
                {
                    TempPanel p = new TempPanel();
                    Viewer v = new Viewer();

                    v.Dock = DockStyle.Fill;
                    p.Controls.Add(v);
                    p.Padding = new Padding(5);

                    v.MouseClick += new MouseEventHandler(v_MouseClick);
                    v.MouseEnter +=new EventHandler(v_MouseEnter);
                    v.MouseLeave += new EventHandler(v_MouseLeave);

                    p.Dock = DockStyle.Fill;
                    this.tableLayoutPanel.Controls.Add(p,j, i);
                }
            }
        }

        void v_MouseLeave(object sender, EventArgs e)
        {
            Viewer v = (Viewer)sender;
            TempPanel p = (TempPanel)v.Parent;

            p.Hover = false;
        }

        void v_MouseEnter(object sender, EventArgs e)
        {
            
            Viewer v = (Viewer)sender;
            TempPanel p = (TempPanel)v.Parent;

            p.Hover = true;
        }

        void v_MouseClick(object sender, MouseEventArgs e)
        {
            UnselectAll();
            Viewer v = (Viewer)sender;
            TempPanel p = (TempPanel)v.Parent;
            p.Selected = true;

            _selected = v;

            
        }

        private void UnselectAll()
        {
            foreach (TempPanel x in tableLayoutPanel.Controls)
            {
                x.Selected = false;
            }
        }

        public ViewerPane()
        {
            InitializeComponent();        
        }
    }
    


    class TempPanel : System.Windows.Forms.Panel
    {
        private bool _selected = false;
        private bool _hover = false;

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
                Invalidate();
                _selected = value; 
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

            this._selected = true;
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
