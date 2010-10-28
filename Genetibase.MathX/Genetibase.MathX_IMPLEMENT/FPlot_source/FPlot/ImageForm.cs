using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for ImageForm.
	/// </summary>
	public class ImageForm : System.Windows.Forms.Form {
		private System.Windows.Forms.TextBox width;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
		private ImageControl picture;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private GraphControl graph, copy;
		private System.Windows.Forms.Button applyButton;
	
		private Bitmap bitmap;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;

		private Rectangle r;
		private Graphics g;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox height;

		public ImageForm(GraphControl graph) {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.graph = graph;
			this.picture = new ImageControl();
			this.picture.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.picture.Location = new System.Drawing.Point(0, 35);
			this.picture.Name = "picture";
			this.picture.Size = new System.Drawing.Size(440, 210);
			this.picture.TabIndex = 3;
			this.picture.TabStop = false;
			this.Controls.Add(this.picture);
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (copy != null) copy.Dispose();
				if(components != null) {
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public void DoDispose() {
			Dispose();
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ImageForm));
			this.width = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.saveButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.label2 = new System.Windows.Forms.Label();
			this.height = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// width
			// 
			this.width.Location = new System.Drawing.Point(80, 8);
			this.width.Name = "width";
			this.width.Size = new System.Drawing.Size(48, 20);
			this.width.TabIndex = 1;
			this.width.Text = "";
			this.width.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.intKeyPress);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Pixel width:";
			// 
			// saveButton
			// 
			this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.saveButton.Location = new System.Drawing.Point(8, 261);
			this.saveButton.Name = "saveButton";
			this.saveButton.TabIndex = 4;
			this.saveButton.Text = "Save...";
			this.saveButton.Click += new System.EventHandler(this.saveClick);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancelButton.Location = new System.Drawing.Point(96, 261);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelClick);
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(256, 8);
			this.applyButton.Name = "applyButton";
			this.applyButton.TabIndex = 6;
			this.applyButton.Text = "Apply";
			this.applyButton.Click += new System.EventHandler(this.applyClick);
			// 
			// saveFileDialog
			// 
			this.saveFileDialog.DefaultExt = "gif";
			this.saveFileDialog.Filter = "Image Files (GIF JPEG TIFF BMP PNG EMF)|*.gif;*.jpg;*.jpeg;*.bmp;*.png;*.tif;*.ti" +
				"ff;*.emf";
			this.saveFileDialog.Title = "Save as GIF File";
			// 
			// progressBar
			// 
			this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar.Location = new System.Drawing.Point(264, 261);
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(160, 23);
			this.progressBar.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(136, 8);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 8;
			this.label2.Text = "Pixel height:";
			// 
			// height
			// 
			this.height.Location = new System.Drawing.Point(200, 8);
			this.height.Name = "height";
			this.height.Size = new System.Drawing.Size(48, 20);
			this.height.TabIndex = 9;
			this.height.Text = "";
			this.height.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.intKeyPress);
			// 
			// ImageForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(432, 288);
			this.Controls.Add(this.height);
			this.Controls.Add(this.width);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(360, 184);
			this.Name = "ImageForm";
			this.ShowInTaskbar = false;
			this.Text = "Save as Image";
			this.ResumeLayout(false);

		}
		#endregion

		private void ResetImage() {
			int w = int.Parse(width.Text);
			int h = int.Parse(height.Text);
			if (w <= 0) {w = 1; width.Text = "1"; }
			if (h <= 0) {h = 1; height.Text = "1"; }
			r = new Rectangle(0, 0, w, h); 
			bitmap = new Bitmap(w, h, PixelFormat.Format24bppRgb);
			g = Graphics.FromImage(bitmap);
			g.Clear(Color.White);
			copy = (GraphControl)graph.Clone();
			copy.Parent = picture;
			copy.Visible = false;
			copy.Bounds = r;
			lock(copy) {
				copy.Bar = progressBar;
				copy.xLabel = null; copy.yLabel = null;
				copy.Model.Invalidated += new GraphModel.InvalidateEventHandler(CopyNotify);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				copy.PaintGraph(g);
				picture.Image = bitmap;
			}
		}

		public void CopyNotify(GraphModel model) {
			lock(copy) {
				g = Graphics.FromImage(bitmap);
				g.Clear(Color.White);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				copy.PaintGraph(g);
				picture.Image = bitmap;
			}
		}

		void SaveAsMetafile(string filename) {
			if (copy != null) {
				copy.AsyncDraw = false;
				Graphics g2 = this.CreateGraphics();
				IntPtr hdc = g2.GetHdc();
				Metafile m = new Metafile(filename, hdc);
				g = Graphics.FromImage(m);
				g.SmoothingMode = SmoothingMode.HighQuality;
				copy.PaintGraph(g);
				g.Dispose();
				m.Dispose();
				g2.ReleaseHdc(hdc);
				copy.AsyncDraw = true;
			}
		}

		public void Reset() {
			Width = graph.Width + 10;
			Height = graph.Height + 105;
			width.Text = graph.Width.ToString();
			height.Text = graph.Height.ToString();
			ResetImage();
		}

		private void saveClick(object sender, System.EventArgs e) {
			DialogResult res = saveFileDialog.ShowDialog();
			if (res == DialogResult.OK) {
				string ext = Path.GetExtension(saveFileDialog.FileName);
				ext.ToLower();
				Console.WriteLine(ext);
				if (ext.Equals(".gif"))
					bitmap.Save(saveFileDialog.FileName, ImageFormat.Gif);
				else if (ext.Equals(".bmp"))
					bitmap.Save(saveFileDialog.FileName, ImageFormat.Bmp);
				else if ((ext.Equals(".jpg")) || (ext.Equals(".jpeg")))
					bitmap.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
				else if (ext.Equals(".png"))
					bitmap.Save(saveFileDialog.FileName, ImageFormat.Png);
				else if ((ext.Equals(".tif")) ||(ext.Equals(".tiff")))
					bitmap.Save(saveFileDialog.FileName, ImageFormat.Tiff);
				else if (ext.Equals(".emf"))
					SaveAsMetafile(saveFileDialog.FileName);
			}
		}

		private void cancelClick(object sender, System.EventArgs e) {
			this.Hide();
		}

		private void intKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			e.Handled = !char.IsDigit(e.KeyChar) && ((int)e.KeyChar >= (int)' ');
		}

		private void applyClick(object sender, System.EventArgs e) {
			ResetImage();
		}

	}
}
