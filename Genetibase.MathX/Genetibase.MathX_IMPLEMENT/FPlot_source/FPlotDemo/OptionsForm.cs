using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FPlotLibrary;

namespace FPlotDemo
{
	/// <summary>
	/// Summary description for OptionsForm.
	/// </summary>
	public class OptionsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button fontButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button applyButton;
		private System.Windows.Forms.ComboBox xFormat;
		private System.Windows.Forms.TextBox yDigits;
		private System.Windows.Forms.TextBox xDigits;
		private System.Windows.Forms.CheckBox showYScale;
		private System.Windows.Forms.CheckBox showXScale;
		private System.Windows.Forms.CheckBox showYAxis;
		private System.Windows.Forms.CheckBox showXAxis;
		private System.Windows.Forms.CheckBox showYRaster;
		private System.Windows.Forms.CheckBox showXRaster;
		private System.Windows.Forms.ComboBox yFormat;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.FontDialog scaleFontForm;
		private System.Windows.Forms.Label yFormatLabel;
		private System.Windows.Forms.Label xFormatLabel;
		private System.Windows.Forms.Label yDigitsLabel;
		private System.Windows.Forms.Label xDigitsLabel;
		private System.Windows.Forms.Button colorButton;
		private System.Windows.Forms.ColorDialog colorDialog;
		private System.Windows.Forms.CheckBox border;
		private System.Windows.Forms.CheckBox legend;
		private System.Windows.Forms.CheckBox zScale;
		private System.Windows.Forms.CheckBox legendBox;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.Label colorLabel;
		private System.Windows.Forms.CheckBox xGrid;
		private System.Windows.Forms.CheckBox yGrid;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPage;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox lineWidth;
		private GraphControl graph;

		public void Reset()
		{
			showXScale.Checked = graph.Model.xScale;
			showYScale.Checked = graph.Model.yScale;
			showXAxis.Checked = graph.Model.xAxis;
			showYAxis.Checked = graph.Model.yAxis;
			showXRaster.Checked = graph.Model.xRaster;
			showYRaster.Checked = graph.Model.yRaster;
			xGrid.Checked = graph.Model.xGrid;
			yGrid.Checked = graph.Model.yGrid;
			legend.Checked = graph.Model.Legend;
			legendBox.Checked = graph.Model.LegendBorder;
			border.Checked = graph.Model.Border;
			lineWidth.Text = graph.Model.ScaleLineWidth.ToString();
			zScale.Checked = graph.Model.zScale;
			xDigits.Text = graph.Model.xDigits.ToString();
			yDigits.Text = graph.Model.yDigits.ToString();
			xFormat.SelectedIndex = (int)graph.Model.xNumberStyle;
			yFormat.SelectedIndex = (int)graph.Model.yNumberStyle;
			scaleFontForm.Font = graph.Model.ScaleFont;
			colorDialog.Color = graph.Model.ScaleColor;
			colorLabel.BackColor = graph.Model.ScaleColor;
			xDigits.Enabled = xFormat.Enabled = showXScale.Checked;
			yDigits.Enabled = yFormat.Enabled = showYScale.Checked;
			legendBox.Enabled = legend.Checked;
		}

		private void Apply()
		{
			graph.Model.xScale = showXScale.Checked;
			graph.Model.yScale = showYScale.Checked;
			graph.Model.xAxis = showXAxis.Checked;
			graph.Model.yAxis = showYAxis.Checked;
			graph.Model.xRaster = showXRaster.Checked;
			graph.Model.yRaster = showYRaster.Checked;
			graph.Model.xGrid = xGrid.Checked;
			graph.Model.yGrid = yGrid.Checked;
			graph.Model.Legend = legend.Checked;
			graph.Model.LegendBorder = legendBox.Checked;
			graph.Model.zScale = zScale.Checked;
			graph.Model.Border = border.Checked;
			try {	graph.Model.ScaleLineWidth = float.Parse(lineWidth.Text);
			} catch {graph.Model.ScaleLineWidth = 1;}
 			graph.Model.xNumberStyle = (GraphModel.NumberStyle)xFormat.SelectedIndex;
			graph.Model.yNumberStyle = (GraphModel.NumberStyle)yFormat.SelectedIndex;
			graph.Model.xDigits = int.Parse(xDigits.Text);
			graph.Model.yDigits = int.Parse(yDigits.Text);
			graph.Model.ScaleFont = scaleFontForm.Font;
			graph.Model.ScaleColor = colorDialog.Color;
			Reset();
			graph.Invalidate();
		}

		public OptionsForm(GraphControl graph)
		{
			//
			// Required for Windows Form Designer support
			//
			this.graph = graph;
			
			InitializeComponent();

			xDigits.KeyPress += new KeyPressEventHandler(intKeyPress);
			yDigits.KeyPress += new KeyPressEventHandler(intKeyPress);
			xFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			yFormat.DropDownStyle = ComboBoxStyle.DropDownList;
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
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OptionsForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.yDigits = new System.Windows.Forms.TextBox();
			this.xDigits = new System.Windows.Forms.TextBox();
			this.colorButton = new System.Windows.Forms.Button();
			this.yFormatLabel = new System.Windows.Forms.Label();
			this.yFormat = new System.Windows.Forms.ComboBox();
			this.xFormatLabel = new System.Windows.Forms.Label();
			this.xFormat = new System.Windows.Forms.ComboBox();
			this.yDigitsLabel = new System.Windows.Forms.Label();
			this.xDigitsLabel = new System.Windows.Forms.Label();
			this.fontButton = new System.Windows.Forms.Button();
			this.showYScale = new System.Windows.Forms.CheckBox();
			this.showXScale = new System.Windows.Forms.CheckBox();
			this.showXRaster = new System.Windows.Forms.CheckBox();
			this.showYRaster = new System.Windows.Forms.CheckBox();
			this.showXAxis = new System.Windows.Forms.CheckBox();
			this.showYAxis = new System.Windows.Forms.CheckBox();
			this.scaleFontForm = new System.Windows.Forms.FontDialog();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.applyButton = new System.Windows.Forms.Button();
			this.colorDialog = new System.Windows.Forms.ColorDialog();
			this.border = new System.Windows.Forms.CheckBox();
			this.legend = new System.Windows.Forms.CheckBox();
			this.zScale = new System.Windows.Forms.CheckBox();
			this.legendBox = new System.Windows.Forms.CheckBox();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.xGrid = new System.Windows.Forms.CheckBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.yGrid = new System.Windows.Forms.CheckBox();
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.tabPage4 = new System.Windows.Forms.TabPage();
			this.tabPage = new System.Windows.Forms.TabPage();
			this.lineWidth = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.colorLabel = new System.Windows.Forms.Label();
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.tabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.tabPage3.SuspendLayout();
			this.tabPage4.SuspendLayout();
			this.tabPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Location = new System.Drawing.Point(8, 176);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(288, 8);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// yDigits
			// 
			this.yDigits.Location = new System.Drawing.Point(232, 16);
			this.yDigits.Name = "yDigits";
			this.yDigits.Size = new System.Drawing.Size(32, 20);
			this.yDigits.TabIndex = 7;
			this.yDigits.Text = "";
			// 
			// xDigits
			// 
			this.xDigits.Location = new System.Drawing.Point(232, 16);
			this.xDigits.Name = "xDigits";
			this.xDigits.Size = new System.Drawing.Size(32, 20);
			this.xDigits.TabIndex = 5;
			this.xDigits.Text = "";
			// 
			// colorButton
			// 
			this.colorButton.Location = new System.Drawing.Point(88, 96);
			this.colorButton.Name = "colorButton";
			this.colorButton.Size = new System.Drawing.Size(80, 23);
			this.colorButton.TabIndex = 13;
			this.colorButton.Text = "Color ...";
			this.colorButton.Click += new System.EventHandler(this.colorClick);
			// 
			// yFormatLabel
			// 
			this.yFormatLabel.Location = new System.Drawing.Point(136, 48);
			this.yFormatLabel.Name = "yFormatLabel";
			this.yFormatLabel.Size = new System.Drawing.Size(48, 23);
			this.yFormatLabel.TabIndex = 12;
			this.yFormatLabel.Text = "Format:";
			// 
			// yFormat
			// 
			this.yFormat.Items.AddRange(new object[] {
																								 "Normal",
																								 "Fixedpoint",
																								 "Scientific"});
			this.yFormat.Location = new System.Drawing.Point(184, 48);
			this.yFormat.Name = "yFormat";
			this.yFormat.Size = new System.Drawing.Size(88, 21);
			this.yFormat.TabIndex = 11;
			// 
			// xFormatLabel
			// 
			this.xFormatLabel.Location = new System.Drawing.Point(136, 48);
			this.xFormatLabel.Name = "xFormatLabel";
			this.xFormatLabel.Size = new System.Drawing.Size(48, 23);
			this.xFormatLabel.TabIndex = 10;
			this.xFormatLabel.Text = "Format:";
			// 
			// xFormat
			// 
			this.xFormat.Items.AddRange(new object[] {
																								 "Normal",
																								 "Fixedpoint",
																								 "Scientific"});
			this.xFormat.Location = new System.Drawing.Point(184, 48);
			this.xFormat.Name = "xFormat";
			this.xFormat.Size = new System.Drawing.Size(88, 21);
			this.xFormat.TabIndex = 9;
			// 
			// yDigitsLabel
			// 
			this.yDigitsLabel.Location = new System.Drawing.Point(136, 16);
			this.yDigitsLabel.Name = "yDigitsLabel";
			this.yDigitsLabel.TabIndex = 8;
			this.yDigitsLabel.Text = "Number of digits:";
			// 
			// xDigitsLabel
			// 
			this.xDigitsLabel.Location = new System.Drawing.Point(136, 16);
			this.xDigitsLabel.Name = "xDigitsLabel";
			this.xDigitsLabel.TabIndex = 6;
			this.xDigitsLabel.Text = "Number of digits:";
			// 
			// fontButton
			// 
			this.fontButton.Location = new System.Drawing.Point(8, 96);
			this.fontButton.Name = "fontButton";
			this.fontButton.Size = new System.Drawing.Size(72, 23);
			this.fontButton.TabIndex = 4;
			this.fontButton.Text = "Font ...";
			this.fontButton.Click += new System.EventHandler(this.fontButton_Click);
			// 
			// showYScale
			// 
			this.showYScale.Location = new System.Drawing.Point(8, 8);
			this.showYScale.Name = "showYScale";
			this.showYScale.TabIndex = 1;
			this.showYScale.Text = "Show y-scale";
			this.showYScale.CheckStateChanged += new System.EventHandler(this.yScaleChanged);
			// 
			// showXScale
			// 
			this.showXScale.Location = new System.Drawing.Point(8, 8);
			this.showXScale.Name = "showXScale";
			this.showXScale.TabIndex = 0;
			this.showXScale.Text = "Show x-scale";
			this.showXScale.CheckStateChanged += new System.EventHandler(this.xScaleChanged);
			// 
			// showXRaster
			// 
			this.showXRaster.Location = new System.Drawing.Point(8, 40);
			this.showXRaster.Name = "showXRaster";
			this.showXRaster.Size = new System.Drawing.Size(112, 24);
			this.showXRaster.TabIndex = 0;
			this.showXRaster.Text = "Show x-raster";
			// 
			// showYRaster
			// 
			this.showYRaster.Location = new System.Drawing.Point(8, 40);
			this.showYRaster.Name = "showYRaster";
			this.showYRaster.Size = new System.Drawing.Size(112, 24);
			this.showYRaster.TabIndex = 1;
			this.showYRaster.Text = "Show y-raster";
			// 
			// showXAxis
			// 
			this.showXAxis.Location = new System.Drawing.Point(8, 72);
			this.showXAxis.Name = "showXAxis";
			this.showXAxis.Size = new System.Drawing.Size(112, 24);
			this.showXAxis.TabIndex = 2;
			this.showXAxis.Text = "Show x=0 axis";
			// 
			// showYAxis
			// 
			this.showYAxis.Location = new System.Drawing.Point(8, 72);
			this.showYAxis.Name = "showYAxis";
			this.showYAxis.Size = new System.Drawing.Size(112, 24);
			this.showYAxis.TabIndex = 3;
			this.showYAxis.Text = "Show y=0 axis";
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point(8, 192);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(72, 24);
			this.okButton.TabIndex = 11;
			this.okButton.Text = "Ok";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(184, 192);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(72, 24);
			this.cancelButton.TabIndex = 12;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// applyButton
			// 
			this.applyButton.Location = new System.Drawing.Point(96, 192);
			this.applyButton.Name = "applyButton";
			this.applyButton.Size = new System.Drawing.Size(72, 24);
			this.applyButton.TabIndex = 13;
			this.applyButton.Text = "Apply";
			this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
			// 
			// border
			// 
			this.border.Location = new System.Drawing.Point(8, 8);
			this.border.Name = "border";
			this.border.Size = new System.Drawing.Size(128, 24);
			this.border.TabIndex = 14;
			this.border.Text = "Show bounding box";
			// 
			// legend
			// 
			this.legend.Location = new System.Drawing.Point(8, 8);
			this.legend.Name = "legend";
			this.legend.TabIndex = 15;
			this.legend.Text = "Show legend";
			this.legend.CheckedChanged += new System.EventHandler(this.legendChanged);
			// 
			// zScale
			// 
			this.zScale.Location = new System.Drawing.Point(8, 8);
			this.zScale.Name = "zScale";
			this.zScale.TabIndex = 16;
			this.zScale.Text = "Show z-scale";
			// 
			// legendBox
			// 
			this.legendBox.Location = new System.Drawing.Point(8, 32);
			this.legendBox.Name = "legendBox";
			this.legendBox.Size = new System.Drawing.Size(160, 24);
			this.legendBox.TabIndex = 17;
			this.legendBox.Text = "Show box around legend";
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabPage1);
			this.tabControl.Controls.Add(this.tabPage2);
			this.tabControl.Controls.Add(this.tabPage3);
			this.tabControl.Controls.Add(this.tabPage4);
			this.tabControl.Controls.Add(this.tabPage);
			this.tabControl.Location = new System.Drawing.Point(8, 8);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(288, 160);
			this.tabControl.TabIndex = 18;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.xGrid);
			this.tabPage1.Controls.Add(this.xDigits);
			this.tabPage1.Controls.Add(this.showXScale);
			this.tabPage1.Controls.Add(this.showXRaster);
			this.tabPage1.Controls.Add(this.showXAxis);
			this.tabPage1.Controls.Add(this.xDigitsLabel);
			this.tabPage1.Controls.Add(this.xFormatLabel);
			this.tabPage1.Controls.Add(this.xFormat);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(280, 134);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "x-Axis";
			// 
			// xGrid
			// 
			this.xGrid.Location = new System.Drawing.Point(8, 104);
			this.xGrid.Name = "xGrid";
			this.xGrid.Size = new System.Drawing.Size(112, 24);
			this.xGrid.TabIndex = 11;
			this.xGrid.Text = "Show x-grid";
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.yGrid);
			this.tabPage2.Controls.Add(this.yDigits);
			this.tabPage2.Controls.Add(this.yDigitsLabel);
			this.tabPage2.Controls.Add(this.showYScale);
			this.tabPage2.Controls.Add(this.yFormatLabel);
			this.tabPage2.Controls.Add(this.yFormat);
			this.tabPage2.Controls.Add(this.showYRaster);
			this.tabPage2.Controls.Add(this.showYAxis);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Size = new System.Drawing.Size(280, 134);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "y-Axis";
			// 
			// yGrid
			// 
			this.yGrid.Location = new System.Drawing.Point(8, 104);
			this.yGrid.Name = "yGrid";
			this.yGrid.Size = new System.Drawing.Size(112, 24);
			this.yGrid.TabIndex = 13;
			this.yGrid.Text = "Show y-grid";
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.zScale);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Size = new System.Drawing.Size(280, 134);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "z-Axis";
			// 
			// tabPage4
			// 
			this.tabPage4.Controls.Add(this.legendBox);
			this.tabPage4.Controls.Add(this.legend);
			this.tabPage4.Location = new System.Drawing.Point(4, 22);
			this.tabPage4.Name = "tabPage4";
			this.tabPage4.Size = new System.Drawing.Size(280, 134);
			this.tabPage4.TabIndex = 3;
			this.tabPage4.Text = "Legend";
			// 
			// tabPage
			// 
			this.tabPage.Controls.Add(this.lineWidth);
			this.tabPage.Controls.Add(this.label1);
			this.tabPage.Controls.Add(this.border);
			this.tabPage.Controls.Add(this.fontButton);
			this.tabPage.Controls.Add(this.colorButton);
			this.tabPage.Controls.Add(this.colorLabel);
			this.tabPage.Location = new System.Drawing.Point(4, 22);
			this.tabPage.Name = "tabPage";
			this.tabPage.Size = new System.Drawing.Size(280, 134);
			this.tabPage.TabIndex = 4;
			this.tabPage.Text = "Scale";
			// 
			// lineWidth
			// 
			this.lineWidth.Location = new System.Drawing.Point(72, 64);
			this.lineWidth.Name = "lineWidth";
			this.lineWidth.Size = new System.Drawing.Size(64, 20);
			this.lineWidth.TabIndex = 16;
			this.lineWidth.Text = "1";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 64);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(64, 23);
			this.label1.TabIndex = 15;
			this.label1.Text = "Line width:";
			// 
			// colorLabel
			// 
			this.colorLabel.Location = new System.Drawing.Point(176, 96);
			this.colorLabel.Name = "colorLabel";
			this.colorLabel.Size = new System.Drawing.Size(24, 24);
			this.colorLabel.TabIndex = 19;
			// 
			// helpProvider
			// 
			this.helpProvider.HelpNamespace = "..\\help\\FPlot.chm";
			// 
			// OptionsForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(306, 226);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.applyButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OptionsForm";
			this.ShowInTaskbar = false;
			this.Text = "Options";
			this.tabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage3.ResumeLayout(false);
			this.tabPage4.ResumeLayout(false);
			this.tabPage.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void intKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = !char.IsDigit(e.KeyChar) && ((int)e.KeyChar >= (int)' ');
		}
		
		private void fontButton_Click(object sender, System.EventArgs e)
		{
			scaleFontForm.ShowDialog();
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			Apply();
			this.Hide();
		}

		private void applyButton_Click(object sender, System.EventArgs e)
		{
			Apply();
			Reset();
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			this.Hide();
		}

		private void xScaleChanged(object sender, System.EventArgs e)
		{
			xDigits.Enabled = xFormat.Enabled = showXScale.Checked;
		}

		private void yScaleChanged(object sender, System.EventArgs e)
		{
			yDigits.Enabled = yFormat.Enabled = showYScale.Checked;
		}

		private void colorClick(object sender, System.EventArgs e) {
			colorDialog.ShowDialog();
			colorLabel.BackColor = colorDialog.Color;
		}

		private void legendChanged(object sender, System.EventArgs e) {
			legendBox.Enabled = legend.Checked;
		}

	}
}
