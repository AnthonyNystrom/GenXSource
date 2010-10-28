using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using FPlotLibrary;

namespace FPlotDemo {
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class MainForm : System.Windows.Forms.Form {
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button options;
		private System.Windows.Forms.Button sin;
		private System.Windows.Forms.Button cos;
		private System.Windows.Forms.Button data;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ProgressBar progressBar1;
		private FPlotLibrary.GraphControl graph;
		private System.Windows.Forms.Button loadwav;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MainForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			graph.Bar = progressBar1;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if (components != null) {
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
		private void InitializeComponent() {
			this.graph = new FPlotLibrary.GraphControl();
			this.button1 = new System.Windows.Forms.Button();
			this.options = new System.Windows.Forms.Button();
			this.sin = new System.Windows.Forms.Button();
			this.cos = new System.Windows.Forms.Button();
			this.data = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.loadwav = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// graph
			// 
			this.graph.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.graph.BackColor = System.Drawing.Color.White;
			this.graph.Border = true;
			this.graph.Cursor = System.Windows.Forms.Cursors.Cross;
			this.graph.FixYtoX = false;
			this.graph.Legend = false;
			this.graph.Location = new System.Drawing.Point(0, 0);
			this.graph.Name = "graph";
			this.graph.Size = new System.Drawing.Size(312, 328);
			this.graph.TabIndex = 0;
			this.graph.Text = "graph";
			this.graph.x0 = -4;
			this.graph.x1 = 4;
			this.graph.xAxis = false;
			this.graph.xGrid = true;
			this.graph.xRaster = true;
			this.graph.xScale = true;
			this.graph.y0 = -4.1973373870943052;
			this.graph.y1 = 4.2374452216013472;
			this.graph.yAxis = false;
			this.graph.yGrid = true;
			this.graph.yRaster = true;
			this.graph.yScale = true;
			this.graph.z0 = 0;
			this.graph.z1 = 20;
			this.graph.zScale = true;
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button1.Location = new System.Drawing.Point(328, 264);
			this.button1.Name = "button1";
			this.button1.TabIndex = 1;
			this.button1.Text = "Quit";
			this.button1.Click += new System.EventHandler(this.quitClick);
			// 
			// options
			// 
			this.options.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.options.Location = new System.Drawing.Point(328, 24);
			this.options.Name = "options";
			this.options.Size = new System.Drawing.Size(104, 24);
			this.options.TabIndex = 2;
			this.options.Text = "Options...";
			this.options.Click += new System.EventHandler(this.optionsClick);
			// 
			// sin
			// 
			this.sin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.sin.Location = new System.Drawing.Point(328, 56);
			this.sin.Name = "sin";
			this.sin.Size = new System.Drawing.Size(104, 23);
			this.sin.TabIndex = 3;
			this.sin.Text = "Add sin(x)...";
			this.sin.Click += new System.EventHandler(this.sinClick);
			// 
			// cos
			// 
			this.cos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cos.Location = new System.Drawing.Point(328, 88);
			this.cos.Name = "cos";
			this.cos.Size = new System.Drawing.Size(104, 24);
			this.cos.TabIndex = 4;
			this.cos.Text = "Add cos(x)...";
			this.cos.Click += new System.EventHandler(this.cosClick);
			// 
			// data
			// 
			this.data.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.data.Location = new System.Drawing.Point(328, 184);
			this.data.Name = "data";
			this.data.Size = new System.Drawing.Size(104, 23);
			this.data.TabIndex = 5;
			this.data.Text = "Load ASCII data...";
			this.data.Click += new System.EventHandler(this.asciiClick);
			// 
			// button3
			// 
			this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button3.Location = new System.Drawing.Point(328, 120);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(104, 24);
			this.button3.TabIndex = 7;
			this.button3.Text = "Add gaussian...";
			this.button3.Click += new System.EventHandler(this.gaussClick);
			// 
			// button4
			// 
			this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button4.Location = new System.Drawing.Point(328, 152);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(104, 24);
			this.button4.TabIndex = 8;
			this.button4.Text = "Add Mandelbrot...";
			this.button4.Click += new System.EventHandler(this.mandelbrotClick);
			// 
			// progressBar1
			// 
			this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.progressBar1.Location = new System.Drawing.Point(328, 304);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(104, 16);
			this.progressBar1.TabIndex = 9;
			// 
			// loadwav
			// 
			this.loadwav.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.loadwav.Location = new System.Drawing.Point(328, 216);
			this.loadwav.Name = "loadwav";
			this.loadwav.Size = new System.Drawing.Size(104, 23);
			this.loadwav.TabIndex = 10;
			this.loadwav.Text = "Load WAV file...";
			this.loadwav.Click += new System.EventHandler(this.wavClick);
			// 
			// MainForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(440, 325);
			this.Controls.Add(this.loadwav);
			this.Controls.Add(this.progressBar1);
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.data);
			this.Controls.Add(this.cos);
			this.Controls.Add(this.sin);
			this.Controls.Add(this.options);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.graph);
			this.Name = "MainForm";
			this.Text = "FPlot Demo";
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[MTAThread]
		static void Main() {
			Application.Run(new MainForm());
		}

		private void quitClick(object sender, System.EventArgs e) {
			Application.Exit();
		}

		private void optionsClick(object sender, System.EventArgs e) {
			OptionsForm form = new OptionsForm(graph);
			form.Reset();
			form.ShowDialog();
		}

		private void sinClick(object sender, System.EventArgs e) {
			Function1D sin = new Function1D();
			//The source represents the body of the following function:
			//double[] p, dfdp;
			//double f(double x) {
			//  ...
			//}
			sin.source = "return sin(x);";
			sin.Compile(true);
			sin.Color = Color.Blue;
			sin.lineWidth = 2;
			graph.Model.Items.Add(sin);
			graph.Invalidate();
		}

		private void cosClick(object sender, System.EventArgs e) {
			Function1D cos = new Function1D();
			cos.source = "return cos(x);";
			cos.Compile(true);
			cos.Color = Color.Red;
			cos.lineWidth = 2;
			cos.lineStyle = DashStyle.Dash; 
			graph.Model.Items.Add(cos);
			graph.Invalidate();
		}

		private void gaussClick(object sender, System.EventArgs e) {
			Function1D gauss = new Function1D();
      //Here the source accesses the array p, an array of function parameters.
			//When you compile the item, the size of p is automatically set to the
			//highest element referred to in the source.
			gauss.source = "double arg = (x-p[0])/p[1]; return p[2]*exp(-arg*arg);";
			gauss.Compile(true);
			gauss.p[0] = 1;
			gauss.p[1] = 1;
			gauss.p[2] = 4;
			graph.Model.Items.Add(gauss);
			graph.Invalidate();
		}

		private void mandelbrotClick(object sender, System.EventArgs e) {
			Function2D m = new Function2D();
			//The source represents the body of the following function:
			//double[] p, dfdp;
			//double f(double x, double y) {
			//  ...
			//}
			m.source = "double xn = 0, yn = 0, x2 = 0, y2 = 0;" +
				"for (int n = 0; n < 500; n++) {" +
				"  yn = 2*xn*yn + y;" +
				"  xn = x2 - y2 + x;" +
				"  x2 = xn*xn; y2 = yn*yn;" + 
				"  if (x2 + y2 > 4) return n;" +
				"} return 0;";
				m.Compile(true);
			graph.SetRange(graph.x0, graph.x1, graph.y0, graph.y1, 0, 20);  
			graph.Model.Items.Add(m);
			graph.Invalidate();
		}

		private void asciiClick(object sender, System.EventArgs e) {
			DataItem data = new DataItem();
			//The loadsource represents the body of the following function:
			//void Load(System.IO.FileStream stream) {
			//  ...
			//}
			//The function loads the data from the stream and sets the arrays x, y, dx and dy accordingly.
			//The size of the arrays is automatically adjusted upon access.
			data.loadsource = 
				"using (StreamReader r = new StreamReader(stream)) {" +
				"  int n = 0; string line; string[] tokens;" +
				"  char[] separator = \";,|\".ToCharArray();" +
				"  while ((line = r.ReadLine()) != null) {" +
				"    tokens = line.Split(separator);" +
				"    try {x[n] = double.Parse(tokens[0]);} catch {x[n] = 0;}" +
				"    try {y[n] = double.Parse(tokens[1]);} catch {y[n] = 0;}" +
				"    try {dx[n] = double.Parse(tokens[2]);} catch {dx[n] = 0;}" +
				"    try {dy[n] = double.Parse(tokens[3]);} catch {dy[n] = 0;}" +
				"    n++;" +
		    "  }" +
		    "}";
			data.Compile(true);
			if (!data.compiled) MessageBox.Show("Error in sourcecode:\n" + data.errors[0]);
			try {
				data.LoadFromFile("data.csv");
			} catch (Exception ex) {
				MessageBox.Show("Could not open the file data.csv\n" + ex.Message);
			}
			graph.Model.Items.Add(data);
			Console.WriteLine("data Length: {0}", data.Length);
			Console.WriteLine("x     y     dx     dy");
			for (int n = 0; n < data.Length; n++) {
				Console.WriteLine("{0}, {1}, {2}, {3}", data.x[n], data.y[n], data.dx[n], data.dy[n]);
			}
			graph.Invalidate();
		}

		private void wavClick(object sender, System.EventArgs e) {
			try {
				WAVLoader.LoadFile(graph, "test.wav");
			} catch (Exception ex){
				MessageBox.Show("Could not open the file test.wav\n" + ex.Message);
			}
			graph.SetRange(0, 1, 0, 1);
			graph.Invalidate();
		}

	}
}
