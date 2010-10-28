using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public class DestinationView3D : Genetibase.MathX.NugenCCalc.Design.Controls.PropertyView
	{
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ImageList imageList;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lblDestination;
		private System.Windows.Forms.ImageList imageListCharts;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ListView lvCharts;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox chkAutoRefresh;
		private System.ComponentModel.IContainer components = null;

		new private NugenCCalc3D _component = null;


		public DestinationView3D(NugenCCalc3D component)
		{
			_component = component;

			// This call is required by the Windows Form Designer.
			InitializeComponent();
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

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("chartControl2");
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("chartControl3");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("chart4");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("chart11");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("chartControl1 (current)");
            this.label2 = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.lblDestination = new System.Windows.Forms.Label();
            this.imageListCharts = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.lvCharts = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkAutoRefresh = new System.Windows.Forms.CheckBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Tahoma", 21.75F);
            this.label2.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.label2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label2.ImageList = this.imageList;
            this.label2.Location = new System.Drawing.Point(288, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 32);
            this.label2.TabIndex = 18;
            this.label2.Text = "Destination";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageList
            // 
            this.imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList.ImageSize = new System.Drawing.Size(32, 32);
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label4
            // 
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label4.Location = new System.Drawing.Point(0, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(128, 16);
            this.label4.TabIndex = 22;
            this.label4.Text = "Available chart on form:";
            // 
            // lblDestination
            // 
            this.lblDestination.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDestination.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblDestination.ImageList = this.imageListCharts;
            this.lblDestination.Location = new System.Drawing.Point(224, 88);
            this.lblDestination.Name = "lblDestination";
            this.lblDestination.Size = new System.Drawing.Size(256, 24);
            this.lblDestination.TabIndex = 21;
            this.lblDestination.Text = "No seleceted chart";
            this.lblDestination.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // imageListCharts
            // 
            this.imageListCharts.ColorDepth = System.Windows.Forms.ColorDepth.Depth16Bit;
            this.imageListCharts.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListCharts.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(224, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 20;
            this.label1.Text = "Current destination chart:";
            // 
            // lvCharts
            // 
            this.lvCharts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCharts.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.lvCharts.LargeImageList = this.imageListCharts;
            this.lvCharts.Location = new System.Drawing.Point(0, 72);
            this.lvCharts.Name = "lvCharts";
            this.lvCharts.Size = new System.Drawing.Size(216, 320);
            this.lvCharts.SmallImageList = this.imageListCharts;
            this.lvCharts.TabIndex = 19;
            this.lvCharts.UseCompatibleStateImageBehavior = false;
            this.lvCharts.View = System.Windows.Forms.View.List;
            this.lvCharts.DoubleClick += new System.EventHandler(this.lvCharts_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(0, 40);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 8);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkAutoRefresh);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(224, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 64);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Destination properties";
            // 
            // chkAutoRefresh
            // 
            this.chkAutoRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkAutoRefresh.Location = new System.Drawing.Point(8, 24);
            this.chkAutoRefresh.Name = "chkAutoRefresh";
            this.chkAutoRefresh.Size = new System.Drawing.Size(128, 24);
            this.chkAutoRefresh.TabIndex = 0;
            this.chkAutoRefresh.Text = "Automatic mode";
            this.chkAutoRefresh.CheckedChanged += new System.EventHandler(this.chkAutoRefresh_CheckedChanged);
            // 
            // DestinationView3D
            // 
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvCharts);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label2);
            this.Name = "DestinationView3D";
            this.Size = new System.Drawing.Size(488, 400);
            this.Load += new System.EventHandler(this.DestinationView_Load);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void DestinationView_Load(object sender, System.EventArgs e)
		{
			Init();
		}

        private void FillChartList_DesignMode()
        {
            foreach (IComponent component in this._component.Site.Container.Components)
            {
                try
                {
                    AddChartToList(component);
                }
                catch { }
            }
        }

        private void FillChartList_Runtime()
        {
            IterateControls((this._component.Owner as Form).Controls);
        }

        private void IterateControls(Control.ControlCollection controls)
        {
            foreach (Control child in controls)
            {
                try
                {
                    AddChartToList(child);
                }
                catch { }
                if (child.HasChildren)
                    IterateControls(child.Controls);
            }
        }

        private Image GetChartImage(Type chartType)
        {
            AttributeCollection attrCol = TypeDescriptor.GetAttributes(chartType);

            ToolboxBitmapAttribute imageAttr = (ToolboxBitmapAttribute)attrCol[typeof(ToolboxBitmapAttribute)];
            if (imageAttr != null)
            {
                return imageAttr.GetImage(chartType.GetType(), false);//false - 16x16, true - 32x32
            }
            return new Bitmap(16, 16);
        }

        private void AddChartToList(IComponent chart)
        {
            if (_component.ValidateControl(chart))
            {
                ListViewItem item = new ListViewItem(((Control)chart).Name, 0);
                item.Tag = chart;
                this.lvCharts.Items.Add(item);

                IChartAdapter _adp = _component.GetChartAdapter(chart);

                Image resultImage = GetChartImage(_adp.GetType());

                if (resultImage != null)
                {
                    int index = 0;
                    imageListCharts.Images.Add(resultImage);
                    index = imageListCharts.Images.Count;

                    item.ImageIndex = index - 1;
                }
            }
        }

		private void Init()
		{
            this.lvCharts.Items.Clear();
            this.imageListCharts.Images.Clear();
            lvCharts.LargeImageList = imageListCharts;
            lvCharts.SmallImageList = imageListCharts;
            if (this._component.Site.Container.GetType().ToString().Contains("DesignerHost"))
            {
                FillChartList_DesignMode();
            }
            else
            {
                FillChartList_Runtime();
            }
            this.chkAutoRefresh.Checked = _component.AutomaticMode;
            SetSelectedChart();
		}

        private void SetSelectedChart()
        {
            if (_component.ChartControl != null)
            {
                lblDestination.Text = ((Control)_component.ChartControl).Name;

                try
                {
                    Image resultImage = GetChartImage(_component.CurrentAdapter.GetType());

                    if (resultImage != null)
                    {
                        int index = 0;
                        imageListCharts.Images.Add(resultImage);
                        index = imageListCharts.Images.Count;

                        lblDestination.ImageIndex = index;
                    }
                }
                catch
                {

                }
            }
        }

		private void lvCharts_DoubleClick(object sender, System.EventArgs e)
		{
			Point pos = lvCharts.PointToClient(Cursor.Position);
			ListViewItem item = lvCharts.GetItemAt(pos.X, pos.Y);
			if (item != null)
			{
				_component.ChartControl = item.Tag;
                SetSelectedChart();
			};
		}


		private void chkAutoRefresh_CheckedChanged(object sender, System.EventArgs e)
		{
			_component.AutomaticMode = chkAutoRefresh.Checked;
		}

	}
}

