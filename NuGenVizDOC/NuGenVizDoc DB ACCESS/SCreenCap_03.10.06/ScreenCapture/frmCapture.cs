using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScreenCapture
{
	/// <summary>
	/// Summary description for frmCapture.
	/// </summary>
	public class frmCapture : System.Windows.Forms.Form
	{
        private System.Windows.Forms.PictureBox picBoxMain;
        private IContainer components;

		// Moving ant border
		private bool drawMovingAntBorder = true;

		// Show coordinates with mouse pointer
		private bool showCoords = true;

		// The current location of mouse pointer
		private int mouseX = 0;
		private int mouseY = 0;

		private bool drawingRect = false;
		private bool drawingAnts = false;		
		private Rectangle rect = new Rectangle(0,0,0,0);
		private int initX = 0;
		private int initY = 0;		
		private Pen p = null;
		private Pen p2 = null;
		private Brush b = null;
		private int offSet = 0;

		private Image selectedImage = null;

		private Keys closeKey = Keys.Enter;
        private System.Timers.Timer redrawTimer;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem bevelToolStripMenuItem;
        private ToolStripMenuItem roundEdgesToolStripMenuItem;
		private Keys refreshKey = Keys.F5;

		public Image SelectedImage
		{
			get
			{				
				return selectedImage;
			}
		}

		public Image Image
		{
			get
			{
				return this.picBoxMain.Image;
			}
			set
			{
				this.picBoxMain.Image = value;
			}
		}

		public frmCapture()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.redrawTimer.Stop();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.picBoxMain = new System.Windows.Forms.PictureBox();
            this.redrawTimer = new System.Timers.Timer();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundEdgesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redrawTimer)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBoxMain
            // 
            this.picBoxMain.ContextMenuStrip = this.contextMenuStrip1;
            this.picBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picBoxMain.Location = new System.Drawing.Point(0, 0);
            this.picBoxMain.Name = "picBoxMain";
            this.picBoxMain.Size = new System.Drawing.Size(292, 266);
            this.picBoxMain.TabIndex = 0;
            this.picBoxMain.TabStop = false;
            this.picBoxMain.DoubleClick += new System.EventHandler(this.picBoxMain_DoubleClick);
            this.picBoxMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseDown);
            this.picBoxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseMove);
            this.picBoxMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picBoxMain_Paint);
            this.picBoxMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseUp);
            // 
            // redrawTimer
            // 
            this.redrawTimer.Enabled = true;
            this.redrawTimer.SynchronizingObject = this;
            this.redrawTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.redrawTimer_Elapsed);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bevelToolStripMenuItem,
            this.roundEdgesToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 48);
            // 
            // bevelToolStripMenuItem
            // 
            this.bevelToolStripMenuItem.Name = "bevelToolStripMenuItem";
            this.bevelToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.bevelToolStripMenuItem.Text = "Bevel";
            // 
            // roundEdgesToolStripMenuItem
            // 
            this.roundEdgesToolStripMenuItem.Name = "roundEdgesToolStripMenuItem";
            this.roundEdgesToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.roundEdgesToolStripMenuItem.Text = "Round Edges";
            // 
            // frmCapture
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.ControlBox = false;
            this.Controls.Add(this.picBoxMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCapture";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.VisibleChanged += new System.EventHandler(this.frmCapture_VisibleChanged);
            this.Load += new System.EventHandler(this.frmCapture_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redrawTimer)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void frmCapture_Load(object sender, System.EventArgs e)
		{			
		}

		private void picBoxMain_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			mouseX = e.X;
			mouseY = e.Y;

			//if( showCoords )
			//	this.picBoxMain.Invalidate();

			if(!drawingRect)
				return;

			SetRectangle(e.X, e.Y);			
			this.picBoxMain.Invalidate();			
						
		}

		private void SetRectangle( int x, int y)
		{
			if (x >= rect.X)    // moving right
				rect.Width = x - initX;
			else                          // moving left
			{
				rect.X = x;
				rect.Width = initX - x;
			}

			if (y >= rect.Y)    // moving down
				rect.Height = y - initY;
			else                          // moving up
			{
				rect.Y = y;
				rect.Height = initY - y;
			}
		}

		private void picBoxMain_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
            if (e.Button == MouseButtons.Left)
            {

                drawingRect = true;
                drawingAnts = true;
                initX = e.X;
                initY = e.Y;

                rect.X = e.X;
                rect.Y = e.Y;

                rect.Width = 0;
                rect.Height = 0;

                p = new Pen(Color.Black);
                p2 = new Pen(Color.White);

                p.DashCap = p2.DashCap = DashCap.Flat;
                p.DashStyle = p2.DashStyle = DashStyle.Custom;
                p.DashPattern = p2.DashPattern = new float[] { 4, 4 };

                Color c = Color.Blue;
                c = Color.FromArgb(20, c.R, c.G, c.B);
                b = new SolidBrush(c);

                if (drawMovingAntBorder)
                    this.redrawTimer.Start();
            }
		}

		private void picBoxMain_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{	
			//this.redrawTimer.Stop();
			drawingRect = false;
			
			if( rect.Width > 0 && rect.Height > 0 )
			{
				if( selectedImage != null )
					selectedImage.Dispose();

				selectedImage = new Bitmap( rect.Width, rect.Height );
				Graphics g = Graphics.FromImage( selectedImage );

				Rectangle destRect = new Rectangle( 0,0, rect.Width, rect.Height );
				g.DrawImage( Image, destRect,rect,GraphicsUnit.Pixel);
				g.Dispose();
			}			
		}

		private void picBoxMain_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if( drawingAnts || (drawingRect && rect.Width > 0 && rect.Height > 0) )
			{
				offSet = (offSet - 1) % 8;

				p.DashOffset = offSet;
				p2.DashOffset = offSet + 4;

				e.Graphics.FillRectangle( b, rect );
				
				e.Graphics.DrawRectangle( p, rect );
				e.Graphics.DrawRectangle( p2, rect );
			}

			if( showCoords )
			{				
				string str = "("+mouseX.ToString()+","+mouseY.ToString()+")";
               
                if( drawingRect )
                    str = "(" + rect.Width.ToString() + "," + rect.Height.ToString() + ")";

				Size coordinatesSize = e.Graphics.MeasureString(str,Font).ToSize();
				e.Graphics.DrawString(str,this.Font,Brushes.White,mouseX+5-coordinatesSize.Width/2,mouseY+25);
			}
		}

		private void picBoxMain_DoubleClick(object sender, System.EventArgs e)
		{                        
			Clipboard.SetDataObject(this.SelectedImage,true);			
			this.Close();
		}

		// Timer repaints the selection
		// Helpfull in drawing the dotted selection boundry
		private void redrawTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			this.picBoxMain.Invalidate();			
		}

		private void frmCapture_VisibleChanged(object sender, System.EventArgs e)
		{			
		}
	}
}
