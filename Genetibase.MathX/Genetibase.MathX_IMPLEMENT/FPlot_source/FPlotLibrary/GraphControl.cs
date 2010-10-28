using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Data;
using System.Windows.Forms;
using System.Threading;

namespace FPlotLibrary {
	
	/// <summary>
	/// GraphControl is a Windows.Forms Control that Displays Mathematical Funcions and Data. Here is some example code, that displays a
	/// x^2 function.
	/// <code>
	/// using FPlotLibrary;
	/// using System.Windows.Forms;
	/// 
	/// namespace FPlotTest {
	/// 
	///		public class MainForm: Form;
	///		
	///		MainForm() {
	///		  GraphControl graph = new GraphControl;
	///		  Function1D f = new Function1D(graph.Model);
	///		  f.source = "return x*x";
	///		  f.Compile(true);
	///		  graph.Model.Items.Add(f);
	///		  graph.Model.Invalidate();
	///		}
	///}
	/// </code>
	/// </summary>
	public class GraphControl : System.Windows.Forms.Control
	{
		private int sx, sy, sw, sh, fx, fy, fw, fh, fd;
		private bool zoomIn;
		/// <summary>
		/// The data model of the Control.
		/// </summary>
		public GraphModel Model;
		/// <summary>
		/// The thread that calculates the function values.
		/// </summary>
		public CalcThread Calc;
		private SolidBrush brush = new SolidBrush(Color.Black);
		private Pen pen = new Pen(Color.Black, 1);
		/// <summary>
		/// A System.Windows.Forms TextBoxe, that will show the current x-coordinate of the mouse inside the control.
		/// </summary>
		public TextBox xLabel;
		/// <summary>
		/// A System.Windows.Forms TextBoxe, that will show the current y-coordinate of the mouse inside the control.
		/// </summary>
		public TextBox yLabel;
		/// <summary>
		/// <c>Bar</c> denotes a System.Windows.Form ProgressBar, that shows the progress of the calculating thread <see cref="Calc">Calc</see>.
		/// </summary>
		public ProgressBar Bar;
		/// <summary>
		/// A constant that defines the white-space border of the control.
		/// </summary>
		public const int D = 12;
		/// <summary>
		/// if AsyncDraw is set to true, <see cref="PaintGraph">PaintGraph</see> retruns before the complete graph is drawn.
		/// </summary>
		public bool AsyncDraw = true;

		private struct zScaleData {
			public Bitmap img;
			public Color color;
			public int w, h;
			public bool rgb;
		}

		private zScaleData zscale;

		/// <summary>
		/// The constructor of the control.
		/// </summary>
		public GraphControl() {
			Model = new GraphModel();
			Model.Invalidated += new GraphModel.InvalidateEventHandler(ModelInvalidated);
			this.Cursor = Cursors.Cross;
			this.BackColor = Color.White;
			sx = sy = -1; sw = sh = 0;
			zoomIn = false;
			zscale.img = null;
			Calc = new CalcThread(this);
			Calc.Step += new CalcStepEventHandler(CalcStep);
		}
		/// <summary>
		/// At the end of the application <c>Dispose</c> must be called to abort all threads.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if (disposing) {
				try {
					Calc.stop = true;
					Calc.thread.Abort();
				} catch {}
			}
			base.Dispose( disposing );
		}

		/// <summary>
		/// Copies from another GraphControl.
		/// </summary>
		/// <param name="graph">The control to copy from.</param>
		public void CopyFrom(GraphControl graph) {
		  sx = graph.sx; sy = graph.sy; sw = graph.sw; sh = graph.sh; zoomIn = graph.zoomIn;
			fx = graph.fx; fy = graph.fy; fw = graph.fw; fh = graph.fh; fd = graph.fd;
			Model = (GraphModel)graph.Model.Clone();
			Calc = (CalcThread)graph.Calc.Clone(this);
			Calc.Step += new CalcStepEventHandler(CalcStep);
		}

		/// <summary>
		/// Creates a deep copy of the control. 
		/// </summary>
		public GraphControl Clone() {
			GraphControl dest = new GraphControl();
			dest.CopyFrom(this);
			return dest;
		}

		#region Properties

		/// <summary>
		/// Represents the left border of the displayed plotting area.
		/// </summary>
		public double x0 {
			get {
				if (Model != null) return Model.x0;
				else return -1;
			}
			set {
				if (Model != null) Model.x0 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the right border of the displayed plotting area.
		/// </summary>
		public double x1 {
			get {
				if (Model != null) return Model.x1;
				else return -1;
			}
			set {
				if (Model != null) Model.x1 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the lower border of the displayed plotting area.
		/// </summary>
		public double y0 {
			get {
				if (Model != null) return Model.y0;
				else return -1;
			}
			set {
				if (Model != null) Model.y0 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the upper border of the displayed plotting area.
		/// </summary>
		public double y1 {
			get {
				if (Model != null) return Model.y1;
				else return -1;
			}
			set {
				if (Model != null) Model.y1 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the lower value of the displayed z-range.
		/// </summary>
		public double z0 {
			get {
				if (Model != null) return Model.z0;
				else return -1;
			}
			set {
				if (Model != null) Model.z0 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// Represents the upper value of the displayed z-range.
		/// </summary>
		public double z1 {
			get {
				if (Model != null) return Model.z1;
				else return -1;
			}
			set {
				if (Model != null) Model.z1 = value;
				ResetRange();
			}
		}

		/// <summary>
		/// If true a x-axis will be drawn at y = 0. The default value is <c>false</c>.
		/// </summary>
		public bool xAxis {
			get {
				if (Model != null) return Model.xAxis;
				else return false;
			}
			set {
				if (Model != null) Model.xAxis = value;
			}
		}

		/// <summary>
		/// If true a y-axis will be drawn at x = 0. The default value is <c>false</c>.
		/// </summary>
		public bool yAxis {
			get {
				if (Model != null) return Model.yAxis;
				else return false;
			}
			set {
				if (Model != null) Model.yAxis = value;
			}
		}
		/// <summary>
		/// If true a box is drawn around the plotting area. The default value is <c>true</c>.
		/// </summary>
		public bool Border {
			get {
				if (Model != null) return Model.Border;
				else return true;
			}
			set {
				if (Model != null) Model.Border = value;
			}
		}
		/// <summary>
		/// If true the y-scale is fixed to the x-scale. The default value is <c>false</c>.
		/// </summary>
		public bool FixYtoX {
			get {
				if (Model != null) return Model.FixYtoX;
				else return false;
			}
			set {
				if (Model != null) Model.FixYtoX = value;
				ResetRange();
			}
		}
		/// <summary>
		/// If true x-rasterlines are drawn. The default value is <c>true</c>.
		/// </summary>
		public bool xRaster {
			get {
				if (Model != null) return Model.xRaster;
				else return true;
			}
			set {
				if (Model != null) Model.xRaster = value;
			}
		}
		/// <summary>
		/// If true y-rasterlines are drawn. The default value is <c>true</c>.
		/// </summary>
		public bool yRaster {
			get {
				if (Model != null) return Model.yRaster;
				else return true;
			}
			set {
				if (Model != null) Model.yRaster = value;
			}
		}
		/// <summary>
		/// If true a legend is drawn. The default value is <c>false</c>.
		/// </summary>
		public bool Legend {
			get {
				if (Model != null) return Model.Legend;
				else return false;
			}
			set {
				if (Model != null) Model.Legend = value;
			}
		}
		/// <summary>
		/// If true, the x-scale is drawn. The default value is <c>true</c>.
		/// </summary>
		public bool xScale {
			get {
				if (Model != null) return Model.xScale;
				else return true;
			}
			set {
				if (Model != null) Model.xScale = value;
			}
		}
		/// <summary>
		/// If true, the y-scale is drawn. The default value is <c>true</c>.
		/// </summary>
		public bool yScale {
			get {
				if (Model != null) return Model.yScale;
				else return true;
			}
			set {
				if (Model != null) Model.yScale = value;
			}
		}
		/// <summary>
		/// If true, a z-scale is drawn. The z-scale is only drawn if a corresponding 2D-function is displayed. The default value is <c>true</c>.
		/// </summary>
		public bool zScale {
			get {
				if (Model != null) return Model.zScale;
				else return true;
			}
			set {
				if (Model != null) Model.zScale = value;
			}
		}
		/// <summary>
		/// If true, the x-grid is drawn. The default value is <c>false</c>.
		/// </summary>
		public bool xGrid {
			get {
				if (Model != null) return Model.xGrid;
				else return false;
			}
			set {
				if (Model != null) Model.xGrid = value;
			}
		}
		/// <summary>
		/// If true, a y-grid is drawn. The default value is <c>false</c>.
		/// </summary>
		public bool yGrid {
			get {
				if (Model != null) return Model.yGrid;
				else return false;
			}
			set {
				if (Model != null) Model.yGrid = value;
			}
		}
		#endregion

		/// <summary>
		/// Returns a System.Drawing.Printing.PrintDocument. You can use this PrintDocument to print the control to a printer.
		/// </summary>
		/// <code>
		/// GraphControl graph;
		/// System.Windows.Forms.PrintDialog dialog = new System.Windows.Forms.PrintDialog();
		/// System.Drawing.Printing.PrintDocument doc = graph.GetPrintDocument();
		/// dialog.Document = doc;
		/// DialogResult res = printDialog.ShowDialog();
		/// if (res == DialogResult.OK) {
		///   doc.Print();
		/// }
		/// </code>
		public GraphPrintDocument GetPrintDocument() {
			return new GraphPrintDocument(this);
		}
		
		/// <summary>
		/// Saves the control to the file specified in <see cref="GraphModel.Filename">Model.Filename</see>
		/// </summary>
		public void SaveToFile() {
			Model.SaveToFile();
		}

		/// <summary>
		/// Loads the controls <see cref="Model">Model</see> from a file.
		/// </summary>
		/// <param name="filename">The file to load.</param>
		/// <returns>Returns false if there was an error during loading.</returns>
		public bool LoadFromFile(string filename) {
			GraphModel m = GraphModel.LoadFromFile(filename);
			if (m == null) {
				MessageBox.Show("Unable to open file " + filename + ".");
			} else {
				Model = m;
				Model.Invalidated += new GraphModel.InvalidateEventHandler(ModelInvalidated);
				SetRange(m.x0, m.x1, m.y0, m.y1);
			}
			return m != null;
		}

		/// <summary>
		/// Sets the witdh of the main raster lines in the x-, y- and z-scale. Throws a
		/// <c>System.ArgumentOutOfRangeException</c> if the arguments are invalid.
		/// </summary>
		public void SetRaster(double rx, double ry, double rz) 
		{
			if (rx <= 0 || ry <= 0 || rz <= 0) throw new System.ArgumentOutOfRangeException();
			Model.rx = rx;
			Model.ry = ry;
			Model.rz = rz;
			Model.Invalidate();
		}

		/// <summary>
		/// Sets the displayed range in the plotting area of the control.
		/// </summary>
		/// <param name="x0">The right border of the plotting area.</param>
		/// <param name="x1">The left border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		/// <param name="z0">The lower z-range of the plotting area.</param>
		/// <param name="z1">The upper z-range of the plotting area.</param>
		public void SetRange(double x0, double x1, double y0, double y1, double z0, double z1) {
			CalcFrame(this.CreateGraphics(), new Rectangle(0, 0, Width, Height));
			try {
				Model.SetRange(x0, x1, y0, y1, z0, z1, fw, fh);
			} catch (ArgumentException ex) {
			}
			CalcFrame(this.CreateGraphics(), new Rectangle(0, 0, Width, Height));
			try {
				Model.SetRange(x0, x1, y0, y1, z0, z1, fw, fh);
			} catch (ArgumentException ex) {
			}
			Model.Invalidate();
		}

		/// <summary>
		/// Sets the displayed range in the plotting area of the control.
		/// </summary>
		/// <param name="x0">The right border of the plotting area.</param>
		/// <param name="x1">The left border of the plotting area.</param>
		/// <param name="y0">The lower border of the plotting area.</param>
		/// <param name="y1">The upper border of the plotting area.</param>
		public void SetRange(double x0, double x1, double y0, double y1) {
			SetRange(x0, x1, y0, y1, Model.z0, Model.z1);
		}

		/// <summary>
		/// Sets the displayed range accoring to the property <c>FixYtoX</c>.
		/// </summary>
		public void ResetRange() {
			SetRange(x0, x1, y0, y1, z0, z1);
		}

		/// <summary>
		/// Moves the displayed range in the plotting area by the specified amount of pixels.
		/// </summary>
		public void MoveGraph(int dx, int dy) {
			Size s = this.Size;
			double x = (Model.x1 - Model.x0)/s.Width;
			double y = (Model.y1 - Model.y0)/s.Height;
			SetRange(Model.x0 - x*dx, Model.x1 - x*dx, Model.y0 + y*dy, Model.y1 + y*dy);
		}

		/// <summary>
		/// Calculates the bounds of the plotting area inside the control.
		/// </summary>
		/// <param name="g">A Graphics object.</param>
		/// <param name="bounds">The bounds of the entire control.</param>
		private void CalcFrame(Graphics g, Rectangle bounds) {
			int w = 0, h = 0, ww, wz = 0, n, W, H;
			double y0 = Model.y0, y1 = Model.y1, x0 = Model.x0, x1 = Model.x1, z0 = Model.z0, z1 = Model.z1,
				rx = Model.rx, ry = Model.ry, rz = Model.rz,
				dy = (y1-y0) / bounds.Height, dz = (z1-z0) / bounds.Height, x, y, z;
			SizeF size;
			string label;
			float Y, Z;

			y = 0.5;
			size = g.MeasureString(y.ToString(GraphModel.NumberFormat(Model.xDigits, Model.xNumberStyle)),
				Model.ScaleFont);
			fd = (int)(size.Height + 0.5F);

			if (Model.Border) {
				
				if (Model.yScale) {
					y = Math.Floor(y0/ry)*ry;
					n = 1;
					while ((y + n*ry) < y1) {
						Y = bounds.Height - 1 - (float)((y + n*ry - y0)/dy);
						if ((int)(bounds.Height - 1 - (float)(-y0/dy) + 0.5F) == (int)(Y+0.5F)) {
							label = "0";
						}	else {
							label = (y + n*ry).ToString(GraphModel.NumberFormat(Model.yDigits, Model.yNumberStyle));
						}
						size = g.MeasureString(label, Model.ScaleFont);
						w = Math.Max(w, (int)(size.Width+0.5));
						n++;
					}
				} else { w = 0; }

				if (Model.xScale)	{
					h = fd;
					x = Math.Floor(x1/rx)*rx;
					label = x.ToString(GraphModel.NumberFormat(Model.xDigits, Model.xNumberStyle));
					size = g.MeasureString(label, Model.ScaleFont);
					ww = (int)(size.Width/2 + 1);
				} else ww = 0;

				if (Model.zScale) {
					Function2D f = null;
					for (int i = 0; i < Model.Items.Count; i++) {
						if (Model.Items[i] is Function2D) f = (Function2D)Model.Items[i];
					}
					if (f != null) {
						z = Math.Floor(z0/rz)*rz;
						n = 1;
						while ((z + n*rz) < z1) {
							Z = bounds.Height - 1 - (float)((z + n*rz - z0)/dz);
							if ((int)(bounds.Height - 1 - (float)(-z0/dz) + 0.5F) == (int)(Z+0.5F)) {
								label = "0";
							}	else {
								label = (z + n*rz).ToString(GraphModel.NumberFormat(Model.zDigits, Model.zNumberStyle));
							}
							size = g.MeasureString(label, Model.ScaleFont);
							wz = Math.Max(wz, (int)(size.Width+0.5));
							n++;
						}
						wz += 3*fd + D;
					} else wz = 0;
				} else wz = 0;

				fx = w + D;
				fy = D/2 + fd/2;
				W = bounds.Width - w - (3*D)/2 - ww - wz;
				H = bounds.Height - h - fd/2 - (3*D)/2;
				if (W != fw && FixYtoX) {
					fw = W; fh = H;
					Model.SetRange(x0, x1, y0, y1, fw, fh);
				}
				fw = W; fh = H;
			} else {
				fx = bounds.X; fy = bounds.Y; fw = bounds.Width - 1; fh = bounds.Height - 2;
			}
		}

		private bool GetZBitmap() {
			Function2D f = null;
			for (int i = 0; i < Model.Items.Count; i++) {
				if (Model.Items[i] is Function2D) f = (Function2D)Model.Items[i];
			}
			if (f != null) {
				if (f.rgb && (zscale.img == null || !zscale.rgb || fd != zscale.w || fh != zscale.h)) {
					zscale.w = fd; zscale.h = fh;
					zscale.img = new Bitmap(zscale.w, zscale.h);
					for (int y = 0; y < zscale.h; y++) {
						for (int x = 0; x < zscale.w; x++) {
							zscale.img.SetPixel(x, zscale.h - 1 - y, f.RGBColor(((double)y)/zscale.h));
						}
					}
				} else if (!f.rgb && (zscale.img == null || zscale.rgb || f.Color != zscale.color ||
						fd != zscale.w || fh != zscale.h)) {
					zscale.w = fd; zscale.h = fh;
					zscale.img = new Bitmap(zscale.w, zscale.h);
					for (int y = 0; y < zscale.h; y++) {
						for (int x = 0; x < zscale.w; x++) {
							zscale.img.SetPixel(x, zscale.h - 1 - y, f.FColor(((double)y)/zscale.h));
						}
					}
				}
				zscale.color = f.Color;
				zscale.rgb = f.rgb;
			}
			return (f != null);
		}

		/// <summary>
		/// Paints the control.
		/// </summary>
		/// <param name="g">The Graphics object to paint to.</param>
		public void PaintGraph(Graphics g) {
			double x, y, z;
			float X, Y, Z, d;
			int n;
			Rectangle frame, bounds = this.Bounds;
			SizeF size;
			Region clip, frameclip, boundsclip;

			double x0 = Model.x0, y0 = Model.y0, z0 = Model.z0, x1 = Model.x1, y1 = Model.y1, z1 = Model.z1,
				rx = Model.rx, ry = Model.ry, rz = Model.rz, srx = rx/5, sry = ry/5, srz = rz/5;	

			CalcFrame(g, bounds);
			g.TranslateTransform(fx, fy, MatrixOrder.Append);
			frame = new Rectangle(0, 0, fw + 1, fh + 1);
			bounds = new Rectangle(-fx, -fy, bounds.Width, bounds.Height);
			clip = g.Clip.Clone();
			frameclip = clip.Clone();
			boundsclip = clip.Clone();
			frameclip.Intersect(frame);
			boundsclip.Intersect(bounds);

			double dx = (x1-x0) / fw;
			double dy = (y1-y0) / fh;
			double dz = (z1-z0) / fh;

			pen.DashStyle = DashStyle.Solid;
			pen.Width = 1;

			g.SetClip(frameclip, CombineMode.Replace);

			//calculate functions
			Calc.Start(frame);

			if (!AsyncDraw) {
				while (!Calc.done) {}
			}

			//draw items
			for (int i = 0; i < Model.Items.Count; i++) {
				Model.Items[i].Draw(g, Calc);
			}

			// draw selection
			if (sw != 0 && sh != 0)	{
				pen.Color = Color.White;
				g.DrawRectangle(pen, Math.Min(sx - fx, sx + sw - fx), Math.Min(sy - fy, sy + sh - fy),
					Math.Abs(sw), Math.Abs(sh));
				pen.DashPattern = new float[2]{3,3};
				pen.DashStyle = DashStyle.Custom;
				pen.Color = Color.Black;
				g.DrawRectangle(pen, Math.Min(sx - fx, sx + sw - fx), Math.Min(sy - fy, sy + sh - fy),
					Math.Abs(sw), Math.Abs(sh));
				pen.DashStyle = DashStyle.Solid;
			}

			// draw raster
			pen.Color = Model.ScaleColor;
			pen.Width = Model.ScaleLineWidth;
			brush.Color = Model.ScaleColor;
			g.DrawRectangle(pen, 0, 0, fw, fh);
			string label;
			x = Math.Floor(x0/rx)*rx + rx;
			n = -4;
			while (x + srx*n < x1) {
				X = (float)((x + srx*n - x0)/dx);
				if (Model.xGrid && n % 5 == 0 && !((int)(-x0/dx+0.5) == (int)(X+0.5F) && Model.yAxis)) {
					pen.Color = Color.FromArgb(64, Model.ScaleColor);
					g.DrawLine(pen, X, 0, X, fh);
					pen.Color = Model.ScaleColor;
				}
				if (Model.xRaster && !((int)(-x0/dx+0.5) == (int)(X+0.5F) && Model.yAxis)) {
					if (n % 5 == 0) d = fd;
					else d = fd/2;
					g.DrawLine(pen, X, 0, X, d);
					g.DrawLine(pen, X, fh - d, X, fh);
				}

				if (Model.xScale && (n % 5 == 0)) {
					if ((int)(-x0/dx+0.5) == (int)(X+0.5F)) {
						label = "0";
					} else {
						label = (x + srx*n).ToString(GraphModel.NumberFormat(Model.xDigits, Model.xNumberStyle));
					}
					size = g.MeasureString(label, Model.ScaleFont);
					g.SetClip(boundsclip, CombineMode.Replace);
					if (Model.Border) {
						g.DrawString(label, Model.ScaleFont, brush, new PointF(X - size.Width/2, fh + D/2));
					} else {
						g.DrawString(label, Model.ScaleFont, brush, new PointF(X - size.Width/2, fh - 2*fd - D/2));
					}
					g.SetClip(frameclip, CombineMode.Replace);
				}
				if (yAxis && (int)(-x0/dx+0.5) == (int)(X+0.5F)) {
					g.DrawLine(pen, X, 0, X, fh);
				}
				n++;
			}

			y = Math.Floor(y0/ry)*ry + ry;
			n = -4;
			while (y + sry*n < y1) {
				Y = fh - 1 - (float)((y + sry*n - y0)/dy);
				if (Model.yGrid && n % 5 == 0 && !((int)(fh - 1 - (float)(-y0/dy) + 0.5F) == (int)(Y+0.5F) && xAxis)) {
					pen.Color = Color.FromArgb(64, Model.ScaleColor);
					g.DrawLine(pen, 0, Y, fw, Y);
					pen.Color = Model.ScaleColor;
				}
				if (Model.yRaster && !((int)(fh - 1 - (float)(-y0/dy) + 0.5F) == (int)(Y+0.5F) && xAxis)) {
					if (n % 5 == 0) d = fd;
					else d = fd/2;
					g.DrawLine(pen, 0, Y, d, Y);
					g.DrawLine(pen, fw-d, Y, fw, Y);
				}
				if (Model.yScale && (n % 5 == 0)) {
					if ((int)(fh - 1 - (float)(-y0/dy) + 0.5F) == (int)(Y+0.5F)) {
						label = "0";
					} else {
						label = (y + sry*n).ToString(GraphModel.NumberFormat(Model.yDigits, Model.yNumberStyle));
					}
					size = g.MeasureString(label, Model.ScaleFont);
					g.SetClip(boundsclip, CombineMode.Replace);
					if (Model.Border) {
						g.DrawString(label, Model.ScaleFont, brush, new PointF(- size.Width - D/2, Y - fd/2));
					} else {
						g.DrawString(label, Model.ScaleFont, brush, new PointF(D/2 + fd, Y - fd/2));
					}
					g.SetClip(frameclip, CombineMode.Replace);
				}
				if (xAxis && (int)(fh - 1 - (float)(-y0/dy) + 0.5F) == (int)(Y+0.5F)) {
					g.DrawLine(pen, 0, Y, fw, Y);
				}
				n++;
			}
 
			//draw z-Scale
			if (Model.zScale && GetZBitmap()) {
				g.SetClip(boundsclip, CombineMode.Replace);
				g.DrawImage(zscale.img, fw + fd, 0, fd, fh);
				g.DrawRectangle(pen, fw + fd, 0, fd, fh);
				
				z = Math.Floor(z0/rz)*rz + rz;
				
				n = -4;
				while (z + srz*n < z1) {
					Z = fh - 1 - (float)((z + srz*n - z0)/dz);
					if (n % 5 == 0) d = fd;
					else d = fd/2;
					g.DrawLine(pen, fw + 2*fd, Z, fw + 2*fd + d, Z);
					if (n % 5 == 0) {
						if ((int)(fh - 1 - (float)(-z0/dz) + 0.5F) == (int)(Z+0.5F)) {
							label = "0";
						} else {
							label = (z + srz*n).ToString(GraphModel.NumberFormat(Model.zDigits, Model.zNumberStyle));
						}
						//size = g.MeasureString(label, Model.ScaleFont);
						g.DrawString(label, Model.ScaleFont, brush, new PointF(fw + D/2 + 3*fd, Z - fd/2));
					}
					n++;
				}
			}

			//draw legend
			if (Model.Legend) {
				float lw = 0;
				int m = 0;
				pen.Color = Model.ScaleColor;
				for (n = 0; n < Model.Items.Count; n++) {
					if (Model.Items[n] is Function1D ||
						(Model.Items[n] is DataItem && ((DataItem)Model.Items[n]).lines)) {
						m++;
						size = g.MeasureString(Model.Items[n].name, Model.ScaleFont);
						lw = Math.Max(lw, size.Width);
					}
				}
				int w = (int)(lw+0.5) + (7*fd)/2, h = fd * m + D;
			
				if (Model.LegendBorder) {
					brush.Color = Color.White;
					g.FillRectangle(brush, fw - 2*fd - w, fy + 2*fd, w, h);
					g.DrawRectangle(pen, fw - 2*fd - w, fy + 2*fd, w, h);
				}
			
				m = 0;
				for (n = 0; n < Model.Items.Count; n++) {
					if (Model.Items[n] is Function1D ||
						(Model.Items[n] is DataItem && ((DataItem)Model.Items[n]).lines)) {

						brush.Color = Model.ScaleColor;
						g.DrawString(Model.Items[n].name, Model.ScaleFont, brush, fw + fd - w, fy + 2*fd + D/2 + m*fd);
						pen.DashStyle = Model.Items[n].lineStyle;
						pen.Width = Model.Items[n].lineWidth;
						g.DrawLine(pen, fw - w - (3*fd)/2, fy + 3*fd + D/2 + m*fd - fd/2,
							fw - w + fd/2, fy + 3*fd + D/2 + m*fd - fd/2);
						m++;
					} 
				}
			}

			//draw status message
			if (!Calc.done) {
				brush.Color = Color.Red;
				g.DrawString("calculating...", Model.ScaleFont, brush, bounds.Width/2, bounds.Height/2);
			} else {
				if (Bar != null) {
					lock(Bar) { Bar.Value = 0; }
				}			
			}

			//reset Graphics
			g.ResetTransform();
			g.SetClip(clip, CombineMode.Replace);
		}

		/// <summary>
		/// The default callback for a <c>Model.Invalidated</c> event.
		/// </summary>
		/// <param name="model">The <see cref="GraphModel">GraphModel</see> that was invalidated.</param>
		public void ModelInvalidated(GraphModel model) {
			Invalidate();
		}

		/// <summary>
		/// Paints the control. Inherited from System.Windows.Forms.Control.
		/// </summary>
		protected override void OnPaint(PaintEventArgs pe)
		{
			//pe.Graphics.FillRectangle(background, pe.ClipRectangle);
			pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			PaintGraph(pe.Graphics);
			base.OnPaint(pe);
		}

		/// <summary>
		/// Resizes the control.
		/// </summary>
		protected override void OnResize(EventArgs e)
		{
			if (Model.FixYtoX) SetRange(Model.x0, Model.x1, Model.y0, Model.y1);
			else Model.Invalidate();
			base.OnResize(e);
		}

		private MouseButtons button = MouseButtons.None;

		/// <summary>
		/// Is called when a mouse button is pressed.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			button = e.Button;
			sx = e.X; sy = e.Y; sw = sh = 0;
			zoomIn = (button == MouseButtons.Left);
		}

		/// <summary>
		/// Is called when the mouse moves over the control.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Middle)
			{
				if (sx != -1) MoveGraph(e.X-sx, e.Y-sy);
				sx = e.X;
				sy = e.Y;
			}
			else if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
			{
				Graphics g = CreateGraphics();

				Region clip;

				if (sx == -1 || sy == -1)	{ sx = e.X; sy = e.Y; }
				
				int dw, dh;

				dw = e.X-sx-sw;
				dh = e.Y-sy-sh;
	
				clip = new Region(new Rectangle(Math.Min(e.X, e.X-dw)-3, Math.Min(sy, sy+sh)-Math.Abs(dh)-3,
					Math.Abs(dw)+6, Math.Abs(sh)+Math.Abs(2*dh)+6));
				clip.Union(new Rectangle(Math.Min(sx, sx+sw)-Math.Abs(dw)-3, Math.Min(e.Y, e.Y-dh)-3,
					Math.Abs(sw)+Math.Abs(2*dw)+6, Math.Abs(dh)+6));
				clip.Union(new Rectangle(Math.Min(sx, e.X), sy, Math.Abs(e.X-sx), 1));
				clip.Union(new Rectangle(sx, Math.Min(sy, e.Y), 1, Math.Abs(e.Y-sy))); 
				sw += dw;
				sh += dh;

				g.SetClip(clip, CombineMode.Intersect);
				Brush white = new SolidBrush(Color.White);
				g.FillRectangle(white, 0, 0, Size.Width, Size.Height);
				g.SmoothingMode = SmoothingMode.AntiAlias;
				PaintGraph(g);
				white.Dispose();
			}

			if (xLabel != null) xLabel.Text = "x = " + GetX(e.X).ToString();
			if (yLabel != null) yLabel.Text = "y = " + GetY(e.Y).ToString();
		}

		/// <summary>
		/// Gets the x-coordinate of the speified x-value of a pixel in the plotting area.
		/// </summary>
		public double GetX(int x) { return Model.x0 + (x - fx)*(Model.x1 - Model.x0)/fw; }

		/// <summary>
		/// Gets the y-coordinate of the speified y-value of a pixel in the plotting area.
		/// </summary>		
		public double GetY(int y) { return Model.y0 + (fy+fh-y)*(Model.y1 - Model.y0)/fh; }

		/// <summary>
		/// Is called when a mouse-button is released.
		/// </summary>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (sx != -1 && sy != -1 && sw != 0 && sh != 0 &&
				Math.Abs(sw) > 10 && Math.Abs(sh) > 10) {
				if (zoomIn) {
					SetRange(GetX(Math.Min(sx, sx+sw)),	GetX(Math.Max(sx, sx+sw)),
						GetY(Math.Max(sy, sy+sh)), GetY(Math.Min(sy, sy+sh)));
				} else {
					double dx = (Model.x1 - Model.x0)/Math.Abs(sw);
					double dy = (Model.y1 - Model.y0)/Math.Abs(sh);
					double y = Model.y0 - (Height - Math.Max(sy, sy+sh))*dy;
					SetRange(Model.x0 - Math.Min(sx, sx+sw)*dx,
						Model.x0 + (Width - Math.Min(sx, sx+sw))*dx, y, Height*dy + y);
				}
			} else if (sx != -1 && sy != -1 && sw != 0 && sh != 0) {
				sx = sy = -1; sw = sh = 0;
				Invalidate();
			}
			sx = sy = -1; sw = sh = 0;
		}

		/// <summary>
		/// Is called when the mouse-wheel is moved.
		/// </summary>
		protected override void OnMouseWheel(MouseEventArgs e)
		{
			double factor = Math.Pow(1.2, Math.Sign(e.Delta));
			if (factor != 1)
			{
				double x = Model.x0 + (1 - factor)*e.X*(Model.x1 - Model.x0)/Width;
				double y = Model.y0 + (1 - factor)*(Height - e.Y)*(Model.y1 - Model.y0)/Height;
				SetRange(x, x + factor*(Model.x1 - Model.x0), y, y + factor*(Model.y1 - Model.y0));
			}
		}

		private void CalcStep(CalcThread t) {
			if (Bar != null) Bar.Value = t.progress;
			t.nt++;
			if (t.nt >= 20) {
				Model.Invalidate();
				t.nt = 0;
			}
		}
	}
}
