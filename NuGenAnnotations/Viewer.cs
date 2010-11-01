using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Genetibase2.NuGenMediImage2.UI2.Controls2
{
	/// <summary>
	/// Summary description for Viewer.
	/// </summary>
	public class Viewer : System.Windows.Forms.UserControl
	{
		private System.ComponentModel.IContainer components;	

		//private bool ZoomDrag = false;

		//private bool MouseIsDown = false;
		
		//private bool _lutApplied = false;

		//private bool _overLay = false;
		//private bool _overLayPaint = false;
		//private float currentBrightness = 0;
		//private float currentContrast = 1;
		private string currentfileName = string.Empty;
		private Color currentOverlayColor = Color.White;

		private const int DragBoxWidth = 100;
		private PictureBox picBoxZoom;
		private const int DragBoxHeight = 100;

		//private Bitmap originalBitmap = null;
		//private Bitmap tempOriginalBitmap = null;
		//private Bitmap zoomBoxBitmap = null;	

        //private Bitmap currentBitmap = null;

		

        //private bool headerVisible = false;

		//private bool MLeft = false;
		//private bool MRight = false;
		public bool MShift = false;

		//private int mouseDownStartX = 0;
		//private int mouseDownStartY = 0;
		private Panel OuterPanel;
		private PictureBox  picBoxMain;
		private System.Windows.Forms.RichTextBox txtBoxMain;

		//private double zoomBoxlastZoom = 1.0;
		//private int zoomLevel = 2;


		//private bool _zoomFit = false;

        
		private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private Panel textBoxMainPanel;
        

		public Viewer()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

           
		}

        

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.picBoxZoom = new System.Windows.Forms.PictureBox();
            this.OuterPanel = new System.Windows.Forms.Panel();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.picBoxMain = new System.Windows.Forms.PictureBox();
            this.textBoxMainPanel = new System.Windows.Forms.Panel();
            this.txtBoxMain = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxZoom)).BeginInit();
            this.OuterPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).BeginInit();
            this.textBoxMainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // picBoxZoom
            // 
            this.picBoxZoom.Location = new System.Drawing.Point(72, 136);
            this.picBoxZoom.Margin = new System.Windows.Forms.Padding(0);
            this.picBoxZoom.Name = "picBoxZoom";
            this.picBoxZoom.Size = new System.Drawing.Size(100, 100);
            this.picBoxZoom.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picBoxZoom.TabIndex = 1;
            this.picBoxZoom.TabStop = false;
            this.picBoxZoom.Visible = false;
            // 
            // OuterPanel
            // 
            this.OuterPanel.Controls.Add(this.hScrollBar1);
            this.OuterPanel.Controls.Add(this.vScrollBar1);
            this.OuterPanel.Controls.Add(this.picBoxMain);
            this.OuterPanel.Controls.Add(this.textBoxMainPanel);
            this.OuterPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OuterPanel.Location = new System.Drawing.Point(0, 0);
            this.OuterPanel.Name = "OuterPanel";
            this.OuterPanel.Size = new System.Drawing.Size(416, 328);
            this.OuterPanel.TabIndex = 5;
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Location = new System.Drawing.Point(280, 168);
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(80, 17);
            this.hScrollBar1.TabIndex = 6;
            this.hScrollBar1.Visible = false;
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Location = new System.Drawing.Point(264, 240);
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 80);
            this.vScrollBar1.TabIndex = 5;
            this.vScrollBar1.Visible = false;
            // 
            // picBoxMain
            //             
            this.picBoxMain.Location = new System.Drawing.Point(0, 0);
            this.picBoxMain.Name = "picBoxMain";
            this.picBoxMain.Size = new System.Drawing.Size(150, 140);
            this.picBoxMain.TabIndex = 3;
            this.picBoxMain.TabStop = false;
            this.picBoxMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseDown);
            this.picBoxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseMove);
            this.picBoxMain.Paint += new System.Windows.Forms.PaintEventHandler(this.picBoxMain_Paint);
            this.picBoxMain.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picBoxMain_MouseUp);
            // 
            // textBoxMainPanel
            // 
            this.textBoxMainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(233)))), ((int)(((byte)(240)))));
            this.textBoxMainPanel.Controls.Add(this.txtBoxMain);
            this.textBoxMainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMainPanel.Location = new System.Drawing.Point(0, 0);
            this.textBoxMainPanel.Name = "textBoxMainPanel";
            this.textBoxMainPanel.Padding = new System.Windows.Forms.Padding(5);
            this.textBoxMainPanel.Size = new System.Drawing.Size(416, 328);
            this.textBoxMainPanel.TabIndex = 7;
            this.textBoxMainPanel.Visible = false;
            // 
            // txtBoxMain
            // 
            this.txtBoxMain.BackColor = System.Drawing.SystemColors.Window;
            this.txtBoxMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtBoxMain.Location = new System.Drawing.Point(5, 5);
            this.txtBoxMain.Name = "txtBoxMain";
            this.txtBoxMain.ReadOnly = true;
            this.txtBoxMain.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtBoxMain.Size = new System.Drawing.Size(406, 318);
            this.txtBoxMain.TabIndex = 4;
            this.txtBoxMain.Text = "";
            // 
            // Viewer
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.picBoxZoom);
            this.Controls.Add(this.OuterPanel);
            this.Name = "Viewer";
            this.Size = new System.Drawing.Size(416, 328);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Viewer_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxZoom)).EndInit();
            this.OuterPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBoxMain)).EndInit();
            this.textBoxMainPanel.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        private void picBoxMain_MouseDown(object sender, MouseEventArgs e)
        {
            this.OnMouseDown(e);
        }

        private void picBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        private void picBoxMain_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        private void Viewer_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.DrawLine(Pens.Red, 0, 0, this.Width, this.Height);
        }

        private void picBoxMain_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(Pens.Red, 0, 0, this.Width, this.Height);
            this.OnPaint(e);
        }

		
	}
}
