using System;
using System.Reflection;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.MathX.NugenCCalc.Adapters;
using Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter;

namespace Genetibase.MathX.NugenCCalc.Design.Controls
{
	public class DestinationView : Genetibase.MathX.NugenCCalc.Design.Controls.PropertyView
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
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox tbNewSeriesName;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.RadioButton rbCreareNewSeries;
		private System.Windows.Forms.RadioButton rbUseExistSeries;
		private Genetibase.MathX.NugenCCalc.Design.SeriesListBox lbSeriesList;
		private System.ComponentModel.IContainer components = null;

		new private NugenCCalc2D _component = null;

		public DestinationView(NugenCCalc2D component)
		{
			_component = component;

			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}

		public DestinationView()
		{
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DestinationView));
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
            this.lbSeriesList = new Genetibase.MathX.NugenCCalc.Design.SeriesListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbNewSeriesName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbCreareNewSeries = new System.Windows.Forms.RadioButton();
            this.rbUseExistSeries = new System.Windows.Forms.RadioButton();
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
            this.label2.ImageIndex = 0;
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
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "");
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
            this.groupBox2.Controls.Add(this.lbSeriesList);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.tbNewSeriesName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.rbCreareNewSeries);
            this.groupBox2.Controls.Add(this.rbUseExistSeries);
            this.groupBox2.Controls.Add(this.chkAutoRefresh);
            this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox2.Location = new System.Drawing.Point(224, 120);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(256, 272);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Destination properties";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // lbSeriesList
            // 
            this.lbSeriesList.BackColor = System.Drawing.Color.AliceBlue;
            this.lbSeriesList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbSeriesList.Location = new System.Drawing.Point(24, 96);
            this.lbSeriesList.Name = "lbSeriesList";
            this.lbSeriesList.Size = new System.Drawing.Size(224, 88);
            this.lbSeriesList.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(24, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 16);
            this.label5.TabIndex = 6;
            this.label5.Text = "New series name";
            // 
            // tbNewSeriesName
            // 
            this.tbNewSeriesName.Enabled = false;
            this.tbNewSeriesName.Location = new System.Drawing.Point(24, 224);
            this.tbNewSeriesName.Name = "tbNewSeriesName";
            this.tbNewSeriesName.Size = new System.Drawing.Size(224, 21);
            this.tbNewSeriesName.TabIndex = 5;
            this.tbNewSeriesName.Text = "New Series Name";
            // 
            // label3
            // 
            this.label3.Enabled = false;
            this.label3.Location = new System.Drawing.Point(24, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 16);
            this.label3.TabIndex = 4;
            this.label3.Text = "Series name";
            // 
            // rbCreareNewSeries
            // 
            this.rbCreareNewSeries.Checked = true;
            this.rbCreareNewSeries.Location = new System.Drawing.Point(8, 184);
            this.rbCreareNewSeries.Name = "rbCreareNewSeries";
            this.rbCreareNewSeries.Size = new System.Drawing.Size(120, 24);
            this.rbCreareNewSeries.TabIndex = 2;
            this.rbCreareNewSeries.TabStop = true;
            this.rbCreareNewSeries.Text = "Create new series";
            this.rbCreareNewSeries.CheckedChanged += new System.EventHandler(this.rbCreareNewSeries_CheckedChanged);
            // 
            // rbUseExistSeries
            // 
            this.rbUseExistSeries.Location = new System.Drawing.Point(8, 56);
            this.rbUseExistSeries.Name = "rbUseExistSeries";
            this.rbUseExistSeries.Size = new System.Drawing.Size(104, 24);
            this.rbUseExistSeries.TabIndex = 1;
            this.rbUseExistSeries.Text = "Use exist";
            this.rbUseExistSeries.CheckedChanged += new System.EventHandler(this.rbUseExistSeries_CheckedChanged);
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
            // DestinationView
            // 
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblDestination);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lvCharts);
            this.Controls.Add(this.label2);
            this.Name = "DestinationView";
            this.Size = new System.Drawing.Size(488, 400);
            this.Load += new System.EventHandler(this.DestinationView_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void DestinationView_Load(object sender, System.EventArgs e)
		{
			Init();
		}

		public override void SaveData()
		{

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

        private Image GetChartImage(Type chartType)
        {
            AttributeCollection attrCol = TypeDescriptor.GetAttributes(chartType);

            ToolboxBitmapAttribute imageAttr = (ToolboxBitmapAttribute)attrCol[typeof(ToolboxBitmapAttribute)];
            if (imageAttr != null)
            {
                return imageAttr.GetImage(chartType.GetType(), false);//false - 16x16, true - 32x32
            }
            return new Bitmap(16,16);
        }

        public bool DesignMode
        {
            get
            {
                if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
                    return true;
                else
                    return false;
            }
        }


		private void Init()
		{
            this.lvCharts.Items.Clear();
			this.imageListCharts.Images.Clear();
			lvCharts.LargeImageList = imageListCharts;
			lvCharts.SmallImageList = imageListCharts;

            if (DesignMode)
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

		private void InitSeriesList()
		{
			if (this._component.CurrentAdapter != null)
			{
				this.lbSeriesList.SelectedIndexChanged -= new System.EventHandler(this.lbSeriesList_SelectedIndexChanged);
				this.tbNewSeriesName.TextChanged -= new System.EventHandler(this.tbNewSeriesName_TextChanged);

				this.lbSeriesList.Items.Clear();
				this.lbSeriesList.Items.AddRange(this._component.CurrentAdapter.GetSeries());
				if (this.lbSeriesList.Items.Count != 0 )
					this.lbSeriesList.SelectedIndex = 0;


				int selIndex = -1;
				int i = 0 ;
				foreach(Series series in this.lbSeriesList.Items)
				{
					if (series.Index == _component.Series.Index && series.Label == _component.Series.Label)
					{
						selIndex = i;
					}
					i++;
				}

				if (selIndex > -1)
				{
					this.lbSeriesList.SelectedIndex = selIndex;
					this.rbUseExistSeries.Checked = true;
					UseExistSeries();
				}
				else
				{
					this.tbNewSeriesName.Text = _component.Series.Label;
					this.rbCreareNewSeries.Checked = true;
					UseNewSeries();
				}

				this.lbSeriesList.SelectedIndexChanged += new System.EventHandler(this.lbSeriesList_SelectedIndexChanged);
				this.tbNewSeriesName.TextChanged += new System.EventHandler(this.tbNewSeriesName_TextChanged);
			}
		}

		private void lvCharts_DoubleClick(object sender, System.EventArgs e)
		{

			Point pos = lvCharts.PointToClient(Cursor.Position);
			ListViewItem item = lvCharts.GetItemAt(pos.X, pos.Y);
			if (item != null && item.Tag != null)
			{
				_component.ChartControl = item.Tag;
                SetSelectedChart();
			};
		}

        private void SetSelectedChart()
        {
            if (_component.ChartControl != null)
            {

                lblDestination.Text = ((Control)_component.ChartControl).Name;

                try
                {
                    Image resultImage = null;

                    AttributeCollection attrCol = TypeDescriptor.GetAttributes(_component.CurrentAdapter.GetType());
                    ToolboxBitmapAttribute imageAttr = (ToolboxBitmapAttribute)attrCol[typeof(ToolboxBitmapAttribute)];
                    if (imageAttr != null)
                    {
                        resultImage = imageAttr.GetImage(_component.CurrentAdapter.GetType(), false);//false - 16x16, true - 32x32
                        int index = 0;
                        imageListCharts.Images.Add(resultImage);
                        index = imageListCharts.Images.Count;

                        lblDestination.ImageIndex = index;
                    }
                }
                catch
                {

                }

                InitSeriesList();

            }
        }

		private void UseExistSeries()
		{
			this.lbSeriesList.Enabled = true;
			this.label3.Enabled = true;

			this.tbNewSeriesName.Enabled = false;
			this.label5.Enabled = false;
		}

		private void UseNewSeries()
		{
			this.lbSeriesList.Enabled = false;
			this.label3.Enabled = false;
			this.tbNewSeriesName.Enabled = true;
			this.label5.Enabled = true;	
		}

		private void rbUseExistSeries_CheckedChanged(object sender, System.EventArgs e)
		{
			UseExistSeries();
			_component.Series = (Series)this.lbSeriesList.SelectedItem;
		}

		private void rbCreareNewSeries_CheckedChanged(object sender, System.EventArgs e)
		{
			UseNewSeries();
			_component.Series = new Series(this.tbNewSeriesName.Text, new Color());
		}


		private void lbSeriesList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			_component.Series = (Series)this.lbSeriesList.SelectedItem;
		}

		private void tbNewSeriesName_TextChanged(object sender, System.EventArgs e)
		{
			_component.Series = new Series(this.tbNewSeriesName.Text, new Color());
		}

		private void groupBox2_Enter(object sender, System.EventArgs e)
		{
		
		}

		private void chkAutoRefresh_CheckedChanged(object sender, System.EventArgs e)
		{
			_component.AutomaticMode = chkAutoRefresh.Checked;
		}

	}
}

