using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Drawing.Imaging;

namespace test
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem bevelToolStripMenuItem;
        private ToolStripMenuItem roundEdgesToolStripMenuItem;
        private IContainer components;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

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
				if (components != null) 
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.bevelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roundEdgesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 245);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Show";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(0, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Capture";
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.bevelToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.bevelToolStripMenuItem.Text = "Bevel";
            this.bevelToolStripMenuItem.Click += new System.EventHandler(this.bevelToolStripMenuItem_Click);
            // 
            // roundEdgesToolStripMenuItem
            // 
            this.roundEdgesToolStripMenuItem.Name = "roundEdgesToolStripMenuItem";
            this.roundEdgesToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.roundEdgesToolStripMenuItem.Text = "Round Edges";
            this.roundEdgesToolStripMenuItem.Click += new System.EventHandler(this.roundEdgesToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(320, 245);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		

		frmCapture f;

		private void Form1_Load(object sender, System.EventArgs e)
		{	
			
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			this.pictureBox1.Image = f.SelectedImage;
		}

		private void button2_Click(object sender, System.EventArgs e)
		{	
			this.Hide();
			f = new frmCapture();
			ScreenCapture sc = new ScreenCapture();
			Image img = sc.CaptureScreen();						
			this.Show();
			f.Image = img;
			f.Show();
			f.Focus();
		}

        private void bevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image newBitmap = Effects.Bevel(this.pictureBox1.Image);
            Image    old = this.pictureBox1.Image;

            this.pictureBox1.Image = newBitmap;
            old.Dispose();
        }

        private void roundEdgesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image newBitmap = Effects.RoundImage(this.pictureBox1.Image);
            Image old = this.pictureBox1.Image;

            this.pictureBox1.Image = newBitmap;
            old.Dispose();
        }
	}
}
