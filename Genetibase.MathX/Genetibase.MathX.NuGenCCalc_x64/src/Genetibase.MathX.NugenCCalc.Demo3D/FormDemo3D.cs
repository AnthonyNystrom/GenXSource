using System;
using System.Reflection;
using System.Xml.Serialization;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Genetibase.MathX.NugenCCalc;

namespace MathX_Demo
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class FormDemo3D : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Button btPlot;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Splitter splitter1;
		private Genetibase.MathX.NugenCCalc.NugenCCalc3D nugenCCalcComponent1;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ImageList imageList1;
        private ListView lvSources;
		private Control c1Chart = null;

		public FormDemo3D()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			if (!this.DesignMode)
			{
				CreateAndInitChartControl();
			}
		}

		public void CreateAndInitChartControl()
		{
			c1Chart = (Control)Assembly.LoadWithPartialName("C1.Win.C1Chart3D, Culture=neutral, PublicKeyToken=a22e16972c085838").CreateInstance("C1.Win.C1Chart3D.C1Chart3D");
			((System.ComponentModel.ISupportInitialize)(this.c1Chart)).BeginInit();
			c1Chart.Location = new System.Drawing.Point(8, 8);
			c1Chart.Name = "c1Chart1";
		
			c1Chart.GetType().InvokeMember("PropBag", BindingFlags.SetProperty,
				null, c1Chart, new object[]{"<?xml version=\"1.0\"?><Chart3DPropBag Version=\"\"><View IsInteractive=\"True\" /><Cha" +
											   "rtGroupsCollection><Chart3DGroup><Elevation IsMeshed=\"False\" /><Contour IsContou" +
											   "red=\"True\" IsZoned=\"True\" /><ChartData><Set type=\"Chart3DDataSetGrid\" RowCount=\"" +
											   "11\" ColumnCount=\"11\" RowDelta=\"1\" ColumnDelta=\"1\" RowOrigin=\"0\" ColumnOrigin=\"0\"" +
											   " Hole=\"3.4028234663852886E+38\"><Data>4.499999888241291 3.599999874830246 2.89999" +
											   "98643994331 2.3999998569488525 2.0999998524785042 1.9999998509883881 2.099999852" +
											   "4785042 2.3999998569488525 2.8999998643994331 3.599999874830246 4.49999988824129" +
											   "1 8.0999999418854713 7.1999999284744263 6.4999999180436134 5.9999999105930328 5." +
											   "6999999061226845 5.5999999046325684 5.6999999061226845 5.9999999105930328 6.4999" +
											   "999180436134 7.1999999284744263 8.0999999418854713 10.899999983608723 9.99999997" +
											   "01976776 9.2999999597668648 8.7999999523162842 8.4999999478459358 8.399999946355" +
											   "82 8.4999999478459358 8.7999999523162842 9.2999999597668648 9.9999999701976776 1" +
											   "0.899999983608723 12.900000013411045 12 11.299999989569187 10.799999982118607 10" +
											   ".499999977648258 10.399999976158142 10.499999977648258 10.799999982118607 11.299" +
											   "999989569187 12 12.900000013411045 14.100000031292439 13.200000017881393 12.5000" +
											   "00007450581 12 11.699999995529652 11.599999994039536 11.699999995529652 12 12.50" +
											   "0000007450581 13.200000017881393 14.100000031292439 14.500000037252903 13.600000" +
											   "023841858 12.900000013411045 12.400000005960465 12.100000001490116 12 12.1000000" +
											   "01490116 12.400000005960465 12.900000013411045 13.600000023841858 14.50000003725" +
											   "2903 14.100000031292439 13.200000017881393 12.500000007450581 12 11.699999995529" +
											   "652 11.599999994039536 11.699999995529652 12 12.500000007450581 13.2000000178813" +
											   "93 14.100000031292439 12.900000013411045 12 11.299999989569187 10.79999998211860" +
											   "7 10.499999977648258 10.399999976158142 10.499999977648258 10.799999982118607 11" +
											   ".299999989569187 12 12.900000013411045 10.899999983608723 9.9999999701976776 9.2" +
											   "999999597668648 8.7999999523162842 8.4999999478459358 8.39999994635582 8.4999999" +
											   "478459358 8.7999999523162842 9.2999999597668648 9.9999999701976776 10.8999999836" +
											   "08723 8.0999999418854713 7.1999999284744263 6.4999999180436134 5.999999910593032" +
											   "8 5.6999999061226845 5.5999999046325684 5.6999999061226845 5.9999999105930328 6." +
											   "4999999180436134 7.1999999284744263 8.0999999418854713 4.499999888241291 3.59999" +
											   "9874830246 2.8999998643994331 2.3999998569488525 2.0999998524785042 1.9999998509" +
											   "883881 2.0999998524785042 2.3999998569488525 2.8999998643994331 3.59999987483024" +
											   "6 4.499999888241291</Data></Set></ChartData></Chart3DGroup></ChartGroupsCollecti" +
											   "on><StyleCollection><NamedStyle Name=\"Legend\" ParentName=\"Legend.default\" /><Nam" +
											   "edStyle Name=\"Footer\" ParentName=\"Control\" /><NamedStyle Name=\"Area\" ParentName=" +
											   "\"Area.default\" /><NamedStyle Name=\"Control\" ParentName=\"Control.default\" /><Name" +
											   "dStyle Name=\"LabelStyleDefault\" ParentName=\"Control\" StyleData=\"BackColor=Transp" +
											   "arent;\" /><NamedStyle Name=\"Legend.default\" ParentName=\"Control\" StyleData=\"Wrap" +
											   "=False;AlignVert=Top;\" /><NamedStyle Name=\"Header\" ParentName=\"Control\" /><Named" +
											   "Style Name=\"Control.default\" ParentName=\"\" StyleData=\"ForeColor=ControlText;Bord" +
											   "er=None,Black,1;BackColor=Control;\" /><NamedStyle Name=\"Area.default\" ParentName" +
											   "=\"Control\" StyleData=\"AlignVert=Top;\" /></StyleCollection><LegendData Compass=\"E" +
											   "ast\" /><FooterData Visible=\"True\" Compass=\"South\" /><HeaderData Visible=\"True\" C" +
											   "ompass=\"North\" /></Chart3DPropBag>"});



			//				object area = c1Chart.GetType().InvokeMember("ChartArea", BindingFlags.GetProperty,
			//					null, c1Chart, null);
			//
			//				
			//				object axes = area.GetType().InvokeMember("Axes", BindingFlags.GetProperty,
			//					null, area, null);
			//
			//				foreach(object o in (ICollection)axes)
			//				{
			//					o.GetType().InvokeMember("AutoMax", BindingFlags.SetProperty,
			//						null, o, new object[]{true});
			//					o.GetType().InvokeMember("AutoMin", BindingFlags.SetProperty,
			//						null, o, new object[]{true});
			//				}

			((System.ComponentModel.ISupportInitialize)(this.c1Chart)).EndInit();

			panel2.Controls.Add(c1Chart);
			c1Chart.Dock = DockStyle.Fill;
			this.components = new System.ComponentModel.Container();
			this.nugenCCalcComponent1 = new Genetibase.MathX.NugenCCalc.NugenCCalc3D(this.components);
            nugenCCalcComponent1.Owner = this;
			nugenCCalcComponent1.ChartControl = c1Chart;
			this.Refresh();
			this.Update();
            
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Explicit Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Implicit Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Parametric Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Parametric Surface", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDemo3D));
            this.panel1 = new System.Windows.Forms.Panel();
            this.lvSources = new System.Windows.Forms.ListView();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btPlot = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lvSources);
            this.panel1.Controls.Add(this.propertyGrid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(256, 502);
            this.panel1.TabIndex = 0;
            // 
            // lvSources
            // 
            this.lvSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSources.FullRowSelect = true;
            this.lvSources.GridLines = true;
            listViewGroup1.Header = "Explicit Functions";
            listViewGroup1.Name = "Explicit Functions";
            listViewGroup2.Header = "Implicit Functions";
            listViewGroup2.Name = "Implicit Functions";
            listViewGroup3.Header = "Parametric Functions";
            listViewGroup3.Name = "Parametric Functions";
            listViewGroup4.Header = "Parametric Surface";
            listViewGroup4.Name = "Parametric Surface";
            this.lvSources.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.lvSources.Location = new System.Drawing.Point(8, 12);
            this.lvSources.Name = "lvSources";
            this.lvSources.Size = new System.Drawing.Size(240, 206);
            this.lvSources.TabIndex = 2;
            this.lvSources.TileSize = new System.Drawing.Size(168, 14);
            this.lvSources.UseCompatibleStateImageBehavior = false;
            this.lvSources.View = System.Windows.Forms.View.Tile;
            this.lvSources.DoubleClick += new System.EventHandler(this.lvSources_DoubleClick);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(8, 224);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(240, 272);
            this.propertyGrid1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btPlot);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(256, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(400, 502);
            this.panel2.TabIndex = 1;
            // 
            // btPlot
            // 
            this.btPlot.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.btPlot.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btPlot.Location = new System.Drawing.Point(8, 470);
            this.btPlot.Name = "btPlot";
            this.btPlot.Size = new System.Drawing.Size(384, 23);
            this.btPlot.TabIndex = 1;
            this.btPlot.Text = "Plot";
            this.btPlot.Click += new System.EventHandler(this.btPlot_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(656, 502);
            this.panel3.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(256, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 502);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // FormDemo3D
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(656, 502);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDemo3D";
            this.Text = "MathX Demo 3D";
            this.Load += new System.EventHandler(this.FormDemo2D_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]

		static void Main() 
		{
			try
			{
				Application.EnableVisualStyles();
				Application.DoEvents();
				Application.Run(new FormDemo3D());
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}


		private void FormDemo2D_Load(object sender, System.EventArgs e)
		{
			FillListView();
			this.propertyGrid1.SelectedObject = this.nugenCCalcComponent1;
		}


		private void FillListView()
		{
			this.lvSources.Items.Clear();

			ListViewItem item = null;

            item = new ListViewItem();
            item.Group = lvSources.Groups["Explicit Functions"];
			item.Text = "Paraboloid";
			item.Tag = new Explicit3DParameters("x*x+y*y");
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Group = lvSources.Groups["Explicit Functions"];
            item.Text = "Paraboloid 2";
			item.Tag = new Explicit3DParameters("x*x-y*y");
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Group = lvSources.Groups["Implicit Functions"];
            item.Text = "Sphere";
			item.Tag = new Implicit3DParameters("x*x+y*y+z*z-25");
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Group = lvSources.Groups["Implicit Functions"];
            item.Text = "Heart surface";
			item.Tag = new Implicit3DParameters("(2*x*x + y*y + z*z - 1)*(2*x*x + y*y + z*z - 1)*(2*x*x + y*y + z*z - 1) - x*x*z*z*z / 10 - y*y*z*z*z");
			((Implicit3DParameters)item.Tag).GridFactor = 200;
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);

			try
			{
                item = new ListViewItem();
                item.Group = lvSources.Groups["Implicit Functions"];
                item.Text = "Glob teardrop";
				item.Tag = new Implicit3DParameters("0,5*x*x*x*x*x + 0,5*x*x*x*x  - y*y - z*z");
				item.ImageIndex = 2;
				this.lvSources.Items.Add(item);
			}
			catch
			{
				
			}

            item = new ListViewItem();
            item.Group = lvSources.Groups["Implicit Functions"];
            item.Text = "Torus";
			item.Tag = new Implicit3DParameters("(sqrt(x*x+y*y)-10)*(sqrt(x*x+y*y)-10)+z*z-3");
			((Implicit3DParameters)item.Tag).GridFactor = 100;
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);


            item = new ListViewItem();
            item.Group = lvSources.Groups["Implicit Functions"];
            item.Text = "Ellipsoid";
			item.Tag = new Implicit3DParameters("x*x/16+y*y/4+z*z/4-1");
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Group = lvSources.Groups["Explicit Functions"];
            item.Text = "Sin(x)+y";
			item.Tag = new Explicit3DParameters("Sin(x)+y");
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Group = lvSources.Groups["Explicit Functions"];
            item.Text = "sin(x)+cos(y)";
			item.Tag = new Explicit3DParameters("sin(x)+cos(y)");
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);


            item = new ListViewItem();
            item.Group = lvSources.Groups["Implicit Functions"];
            item.Text = "Bifolia Surface";
			item.Tag = new Implicit3DParameters("(x*x + y*y + z*z)*(x*x + y*y + z*z) - 3*y*(x*x + z*z)");
			((Implicit3DParameters)item.Tag).GridFactor = 300;
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Group = lvSources.Groups["Implicit Functions"];
            item.Text = "The Blob";
			item.Tag = new Implicit3DParameters("x*x+y*y+z*z+sin(4*x)+sin(4*y)+sin(4*z)-1");
			((Implicit3DParameters)item.Tag).GridFactor = 200;
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);


            item = new ListViewItem();
            item.Group = lvSources.Groups["Implicit Functions"];
            item.Text = "Hunt Surface";
			item.Tag = new Implicit3DParameters("4*(x*x+y*y+z*z - 13)*(x*x+y*y+z*z - 13)*(x*x+y*y+z*z - 13)+27*(3*x*x+y*y-4*z*z-12)*(3*x*x+y*y-4*z*z-12)*(3*x*x+y*y-4*z*z-12)");
			item.ImageIndex = 2;
			this.lvSources.Items.Add(item);


            item = new ListViewItem();
            item.Group = lvSources.Groups["Parametric Surface"];
            item.Text = "Klein Bottle";
			item.Tag = new ParametricSurfaceParameters(
				"cos(u)*(4+sin(v)*cos(u/2)-sin(2*v)*sin(u/2)/2)",
				"sin(u)*(4+sin(v)*cos(u/2)-sin(2*v)*sin(u/2)/2)",
				"sin(u/2)*sin(v)+cos(u/2)*sin(2*v)/2",-3.14, 3.14,-3.14,3.14,5000);
			item.ImageIndex = 3;
			this.lvSources.Items.Add(item);


            item = new ListViewItem();
            item.Group = lvSources.Groups["Parametric Surface"]; item.Text = "Helicoid";
			item.Tag = new ParametricSurfaceParameters("u*cos(v)","u*sin(v)","v",-3.14, 3.14,-3.14,3.14,3000);
			item.ImageIndex = 3;
			this.lvSources.Items.Add(item);		
		}




		private void btPlot_Click(object sender, System.EventArgs e)
		{
			if (nugenCCalcComponent1.FunctionParameters == null || nugenCCalcComponent1.FunctionParameters.Code =="")
			{
				MessageBox.Show(this,
					"Please, select some function", 
					"MathX Demo 3D", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Warning);

				return;
			}
			try
			{
				nugenCCalcComponent1.Plot();
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,
					"Error:"+ex.Message, 
					"MathX Demo 2D", 
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error);
			}
		}

		private void lvSources_DoubleClick(object sender, System.EventArgs e)
		{
			Point mouseCoord = lvSources.PointToClient(Cursor.Position);
			ListViewItem activeNode = lvSources.GetItemAt(mouseCoord.X, mouseCoord.Y);
			if (activeNode != null && activeNode.Tag != null)
			{
				PlotFunction((Function3DParameters)activeNode.Tag);
			}
			this.propertyGrid1.Refresh();
		}

		private void PlotFunction(Function3DParameters functionParameters)
		{
			if (nugenCCalcComponent1.FunctionParameters == null)
			{
				MessageBox.Show(this,
					"Please, select some function", "MathX Demo 3D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
	
			try
			{
				nugenCCalcComponent1.FunctionParameters = functionParameters;
				object area = c1Chart.GetType().InvokeMember("ChartArea", BindingFlags.GetProperty,
					null, c1Chart, null);

				object size = area.GetType().InvokeMember("Size", BindingFlags.GetProperty,
					null, area, null);
				
				if (nugenCCalcComponent1.FunctionParameters is Explicit3DParameters)
				{
					((Explicit3DParameters)nugenCCalcComponent1.FunctionParameters).AreaSize = (Size)size;
				}
				//

				nugenCCalcComponent1.Plot();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Error: " + ex.Message, "Math X Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

	}
}
