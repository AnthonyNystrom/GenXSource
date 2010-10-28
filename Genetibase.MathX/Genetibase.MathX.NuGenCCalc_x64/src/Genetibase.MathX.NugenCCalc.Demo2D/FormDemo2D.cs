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
	public class FormDemo2D : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Button btPlot;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Splitter splitter1;
		private Genetibase.MathX.NugenCCalc.NugenCCalc2D nugenCCalcComponent1;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ImageList imageList1;
        private ListView lvSources;

		private Control c1Chart = null;

		public FormDemo2D()
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

		private void CreateAndInitChartControl()
		{
			c1Chart = (Control)Assembly.LoadWithPartialName("C1.Win.C1Chart, Culture=neutral, PublicKeyToken=a22e16972c085838").CreateInstance("C1.Win.C1Chart.C1Chart");
			((System.ComponentModel.ISupportInitialize)(this.c1Chart)).BeginInit();
			c1Chart.Location = new System.Drawing.Point(8, 8);
			c1Chart.Name = "c1Chart1";
			((System.ComponentModel.ISupportInitialize)(this.c1Chart)).EndInit();

			panel2.Controls.Add(c1Chart);
			c1Chart.Dock = DockStyle.Fill;
			this.components = new System.ComponentModel.Container();
			this.nugenCCalcComponent1 = new Genetibase.MathX.NugenCCalc.NugenCCalc2D(this.components);
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
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Polar Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Parametric Functions", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormDemo2D));
            this.panel1 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btPlot = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lvSources = new System.Windows.Forms.ListView();
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
            // propertyGrid1
            // 
            this.propertyGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.propertyGrid1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(8, 232);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(240, 256);
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
            // lvSources
            // 
            this.lvSources.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.lvSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvSources.AutoArrange = false;
            this.lvSources.FullRowSelect = true;
            this.lvSources.GridLines = true;
            listViewGroup1.Header = "Explicit Functions";
            listViewGroup1.Name = "Explicit Functions";
            listViewGroup2.Header = "Implicit Functions";
            listViewGroup2.Name = "Implicit Functions";
            listViewGroup3.Header = "Polar Functions";
            listViewGroup3.Name = "Polar Functions";
            listViewGroup4.Header = "Parametric Functions";
            listViewGroup4.Name = "Parametric Functions";
            this.lvSources.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4});
            this.lvSources.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvSources.LabelWrap = false;
            this.lvSources.Location = new System.Drawing.Point(8, 12);
            this.lvSources.MultiSelect = false;
            this.lvSources.Name = "lvSources";
            this.lvSources.Size = new System.Drawing.Size(240, 214);
            this.lvSources.TabIndex = 2;
            this.lvSources.TileSize = new System.Drawing.Size(230, 14);
            this.lvSources.UseCompatibleStateImageBehavior = false;
            this.lvSources.View = System.Windows.Forms.View.Tile;
            this.lvSources.DoubleClick += new System.EventHandler(this.lvSources_DoubleClick);
            // 
            // FormDemo2D
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(656, 502);
            this.Controls.Add(this.panel3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormDemo2D";
            this.Text = "MathX Demo 2D";
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
				Application.Run(new FormDemo2D());
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


		public void FillListView()
		{
            this.lvSources.Items.Clear();

            ListViewItem item = null;

            item = new ListViewItem();
            item.Group = this.lvSources.Groups["Explicit Functions"];
            item.Text = "Sin";
            item.Tag = new Explicit2DParameters("Sin(x)");
            item.ImageIndex = 2;
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Group = this.lvSources.Groups["Explicit Functions"];
            item.Text = "Parabola";
            item.Tag = new Explicit2DParameters("x*x");
            item.ImageIndex = 2;
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Group = this.lvSources.Groups["Implicit Functions"];
            item.Text = "Circle";
            item.Tag = new Implicit2DParameters("x*x+y*y-25");
            item.ImageIndex = 2;
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Text = "Rose";
            item.Tag = new Explicit2DParameters("3*sin(4*x)", PlotMode.ByNumPoints, -10, 10, 0.1, 2000, true);
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Polar Functions"];
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Text = "Verziera";
            item.Tag = new Explicit2DParameters("5*5*5/(x*x+5*5)");
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Explicit Functions"];
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Text = "Cardioid";
            item.Tag = new Implicit2DParameters("(x*x+y*y)*(x*x+y*y)-10*x*(x*x+y*y)- 25*y*y");
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Implicit Functions"];
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Text = "Archimedes Spiral";
            Explicit2DParameters polarParam = new Explicit2DParameters("x");
            polarParam.IsPolar = true;
            item.Tag = polarParam;
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Polar Functions"];
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Text = "Lemniscate Bernoulli";
            item.Tag = new Implicit2DParameters("(x*x+y*y)*(x*x+y*y)-50*(x*x-y*y)");
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Implicit Functions"];
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Text = "Folium";
            item.Tag = new Implicit2DParameters("x*x*x+y*y*y-3*x*y");
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Implicit Functions"];
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Text = "Hyperbola";
            item.Tag = new Implicit2DParameters("x*x/3*3-y*y/3*3-1");
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Implicit Functions"];
            this.lvSources.Items.Add(item);


            item = new ListViewItem();
            item.Text = "Chrysanthemum Curve";
            Explicit2DParameters polarParam1 = new Explicit2DParameters("5*(1 + sin(11*x / 5)) - 4*sin(17*x / 3)*sin(17*x / 3)*sin(17*x / 3)*sin(17*x / 3)*sin(2*cos(3*x) - 28*x)*sin(2*cos(3*x) - 28*x)*sin(2*cos(3*x) - 28*x)*sin(2*cos(3*x) - 28*x)*sin(2*cos(3*x) - 28*x)*sin(2*cos(3*x) - 28*x)*sin(2*cos(3*x) - 28*x)*sin(2*cos(3*x) - 28*x)");
            polarParam1.IsPolar = true;
            item.Tag = polarParam1;
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Polar Functions"];
            this.lvSources.Items.Add(item);

            try
            {
                item = new ListViewItem();
                item.Text = "Astroid";
                item.Tag = new Implicit2DParameters("(1,01 + x*x + y*y)*(1,01 + x*x + y*y) - 4*1,01*1,01*x*x - 1");
                item.ImageIndex = 2;
                item.Group = this.lvSources.Groups["Implicit Functions"];
                this.lvSources.Items.Add(item);
            }
            catch
            {

            }

            item = new ListViewItem();
            item.Text = "Tanh Spiral";
            item.Tag = new Parametric2DParameters("sinh(2*t)/(cos(4*t)+cosh(2*t))", "sin(4*t)/(cos(4*t) + cosh(2*t))", PlotMode.ByNumPoints, -3, 3, 0.1, 2000);
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Parametric Functions"];
            this.lvSources.Items.Add(item);


            item = new ListViewItem();
            item.Text = "Coth Spiral";
            item.Tag = new Parametric2DParameters("-sinh(2*t)/(cos(8*t)-cosh(2*t))", "sin(8*t)/(cos(8*t)-cosh(2*t))", PlotMode.ByNumPoints, -1.57, 1.57, 0.1, 2000);
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Parametric Functions"];
            this.lvSources.Items.Add(item);

            item = new ListViewItem();
            item.Text = "Butterfly Curve";
            item.Tag = new Parametric2DParameters("cos(t)*(exp(cos(t))-2*cos(4*t)-sin(t/12)*sin(t/12)*sin(t/12)*sin(t/12)*sin(t/12))", "sin(t)*(exp(cos(t))-2*cos(4*t)-sin(t/12)*sin(t/12)*sin(t/12)*sin(t/12)*sin(t/12))", PlotMode.ByNumPoints, -5, 5, 0.1, 2000);
            item.ImageIndex = 2;
            item.Group = this.lvSources.Groups["Parametric Functions"];
            this.lvSources.Items.Add(item);			 
		}


		private void btPlot_Click(object sender, System.EventArgs e)
		{
			if (nugenCCalcComponent1.FunctionParameters == null || nugenCCalcComponent1.FunctionParameters.Code =="")
			{
				MessageBox.Show(this,
					"Please, select some function", 
					"MathX Demo 2D", 
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


		private void PlotFunction(Function2DParameters functionParameters)
		{
			if (nugenCCalcComponent1.FunctionParameters == null)
			{
				MessageBox.Show(this,
					"Please, select some function", "MathX Demo 2D", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
	
			try
			{
				nugenCCalcComponent1.FunctionParameters = functionParameters;
				object area = c1Chart.GetType().InvokeMember("ChartArea", BindingFlags.GetProperty,
					null, c1Chart, null);

				object size = area.GetType().InvokeMember("Size", BindingFlags.GetProperty,
					null, area, null);
				
				if (nugenCCalcComponent1.FunctionParameters is Implicit2DParameters)
				{
					((Implicit2DParameters)nugenCCalcComponent1.FunctionParameters).AreaSize = (Size)size;
				}
				//

				nugenCCalcComponent1.Plot();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Error: " + ex.Message, "Math X Demo", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void lvSources_DoubleClick(object sender, System.EventArgs e)
		{
			Point mouseCoord = lvSources.PointToClient(Cursor.Position);
			ListViewItem activeNode = lvSources.GetItemAt(mouseCoord.X, mouseCoord.Y);
			if (activeNode != null && activeNode.Tag != null)
			{
				PlotFunction((Function2DParameters)activeNode.Tag);
			}
			this.propertyGrid1.Refresh();
		}


	}
}
