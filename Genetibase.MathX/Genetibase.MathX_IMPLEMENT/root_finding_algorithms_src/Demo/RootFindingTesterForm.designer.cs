namespace RootFinding
{
	partial class RootFindingTesterForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components=null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if(disposing&&(components!=null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.NumericUpDownDistributionSize=new System.Windows.Forms.NumericUpDown();
			this.RichPanelGeneration=new KIS.Controls.Windows.HeaderPanel();
			this.textBox3=new System.Windows.Forms.TextBox();
			this.label3=new System.Windows.Forms.Label();
			this.textBox2=new System.Windows.Forms.TextBox();
			this.label1=new System.Windows.Forms.Label();
			this.textBox1=new System.Windows.Forms.TextBox();
			this.label2=new System.Windows.Forms.Label();
			this.label26=new System.Windows.Forms.Label();
			this.ButtonGenerateDistribution=new System.Windows.Forms.Button();
			this.RichPanelTests=new KIS.Controls.Windows.HeaderPanel();
			this.RichTextBoxTestsResult=new System.Windows.Forms.RichTextBox();
			this.splitContainer1=new System.Windows.Forms.SplitContainer();
			this.Chart=new ZedGraph.ZedGraphControl();
			((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDistributionSize)).BeginInit();
			this.RichPanelGeneration.SuspendLayout();
			this.RichPanelTests.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// NumericUpDownDistributionSize
			// 
			this.NumericUpDownDistributionSize.Location=new System.Drawing.Point(77,34);
			this.NumericUpDownDistributionSize.Maximum=new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
			this.NumericUpDownDistributionSize.Minimum=new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.NumericUpDownDistributionSize.Name="NumericUpDownDistributionSize";
			this.NumericUpDownDistributionSize.Size=new System.Drawing.Size(141,23);
			this.NumericUpDownDistributionSize.TabIndex=9;
			this.NumericUpDownDistributionSize.ThousandsSeparator=true;
			this.NumericUpDownDistributionSize.Value=new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// RichPanelGeneration
			// 
			this.RichPanelGeneration.BackColor=System.Drawing.Color.WhiteSmoke;
			this.RichPanelGeneration.BorderColor=System.Drawing.SystemColors.ActiveCaption;
			this.RichPanelGeneration.BorderStyle=KIS.Controls.BorderStyles.Shadow;
			this.RichPanelGeneration.CaptionBeginColor=System.Drawing.SystemColors.InactiveCaption;
			this.RichPanelGeneration.CaptionEndColor=System.Drawing.SystemColors.ActiveCaption;
			this.RichPanelGeneration.CaptionGradientDirection=System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.RichPanelGeneration.CaptionHeight=26;
			this.RichPanelGeneration.CaptionPosition=KIS.Controls.CaptionPositions.Top;
			this.RichPanelGeneration.CaptionText="Settings";
			this.RichPanelGeneration.CaptionVisible=true;
			this.RichPanelGeneration.Controls.Add(this.textBox3);
			this.RichPanelGeneration.Controls.Add(this.label3);
			this.RichPanelGeneration.Controls.Add(this.textBox2);
			this.RichPanelGeneration.Controls.Add(this.label1);
			this.RichPanelGeneration.Controls.Add(this.textBox1);
			this.RichPanelGeneration.Controls.Add(this.NumericUpDownDistributionSize);
			this.RichPanelGeneration.Controls.Add(this.label2);
			this.RichPanelGeneration.Controls.Add(this.label26);
			this.RichPanelGeneration.Controls.Add(this.ButtonGenerateDistribution);
			this.RichPanelGeneration.Dock=System.Windows.Forms.DockStyle.Top;
			this.RichPanelGeneration.Font=new System.Drawing.Font("Trebuchet MS",9.75F,System.Drawing.FontStyle.Bold);
			this.RichPanelGeneration.GradientDirection=System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.RichPanelGeneration.GradientEnd=System.Drawing.Color.WhiteSmoke;
			this.RichPanelGeneration.GradientStart=System.Drawing.Color.WhiteSmoke;
			this.RichPanelGeneration.Location=new System.Drawing.Point(0,0);
			this.RichPanelGeneration.Name="RichPanelGeneration";
			this.RichPanelGeneration.PanelIcon=null;
			this.RichPanelGeneration.PanelIconVisible=false;
			this.RichPanelGeneration.Size=new System.Drawing.Size(325,147);
			this.RichPanelGeneration.TabIndex=23;
			this.RichPanelGeneration.TextAntialias=true;
			// 
			// textBox3
			// 
			this.textBox3.Font=new System.Drawing.Font("Microsoft Sans Serif",8.25F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.textBox3.Location=new System.Drawing.Point(77,60);
			this.textBox3.Name="textBox3";
			this.textBox3.Size=new System.Drawing.Size(141,20);
			this.textBox3.TabIndex=14;
			this.textBox3.Text="10E-04";
			// 
			// label3
			// 
			this.label3.AutoSize=true;
			this.label3.Location=new System.Drawing.Point(13,63);
			this.label3.Name="label3";
			this.label3.Size=new System.Drawing.Size(74,18);
			this.label3.TabIndex=13;
			this.label3.Text="Accuracy :";
			// 
			// textBox2
			// 
			this.textBox2.Location=new System.Drawing.Point(77,86);
			this.textBox2.Name="textBox2";
			this.textBox2.Size=new System.Drawing.Size(141,23);
			this.textBox2.TabIndex=12;
			this.textBox2.Text="0;1";
			// 
			// label1
			// 
			this.label1.AutoSize=true;
			this.label1.Location=new System.Drawing.Point(13,89);
			this.label1.Name="label1";
			this.label1.Size=new System.Drawing.Size(54,18);
			this.label1.TabIndex=11;
			this.label1.Text="Range :";
			// 
			// textBox1
			// 
			this.textBox1.Font=new System.Drawing.Font("MS Reference Sans Serif",8.25F,System.Drawing.FontStyle.Regular,System.Drawing.GraphicsUnit.Point,((byte)(0)));
			this.textBox1.Location=new System.Drawing.Point(77,7);
			this.textBox1.Name="textBox1";
			this.textBox1.Size=new System.Drawing.Size(234,21);
			this.textBox1.TabIndex=10;
			this.textBox1.Text="f(x)=x*x-1";
			// 
			// label2
			// 
			this.label2.AutoSize=true;
			this.label2.Location=new System.Drawing.Point(13,11);
			this.label2.Name="label2";
			this.label2.Size=new System.Drawing.Size(72,18);
			this.label2.TabIndex=6;
			this.label2.Text="Function :";
			// 
			// label26
			// 
			this.label26.AutoSize=true;
			this.label26.Location=new System.Drawing.Point(13,37);
			this.label26.Name="label26";
			this.label26.Size=new System.Drawing.Size(76,18);
			this.label26.TabIndex=8;
			this.label26.Text="Iterations :";
			// 
			// ButtonGenerateDistribution
			// 
			this.ButtonGenerateDistribution.Location=new System.Drawing.Point(236,32);
			this.ButtonGenerateDistribution.Name="ButtonGenerateDistribution";
			this.ButtonGenerateDistribution.Size=new System.Drawing.Size(75,74);
			this.ButtonGenerateDistribution.TabIndex=5;
			this.ButtonGenerateDistribution.Text="Compute";
			this.ButtonGenerateDistribution.UseVisualStyleBackColor=true;
			this.ButtonGenerateDistribution.Click+=new System.EventHandler(this.OnButtonGenerateDistributionClick);
			// 
			// RichPanelTests
			// 
			this.RichPanelTests.BackColor=System.Drawing.Color.WhiteSmoke;
			this.RichPanelTests.BorderColor=System.Drawing.SystemColors.ActiveCaption;
			this.RichPanelTests.BorderStyle=KIS.Controls.BorderStyles.Shadow;
			this.RichPanelTests.CaptionBeginColor=System.Drawing.SystemColors.InactiveCaption;
			this.RichPanelTests.CaptionEndColor=System.Drawing.SystemColors.ActiveCaption;
			this.RichPanelTests.CaptionGradientDirection=System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.RichPanelTests.CaptionHeight=26;
			this.RichPanelTests.CaptionPosition=KIS.Controls.CaptionPositions.Top;
			this.RichPanelTests.CaptionText="Tests";
			this.RichPanelTests.CaptionVisible=true;
			this.RichPanelTests.Controls.Add(this.RichTextBoxTestsResult);
			this.RichPanelTests.Dock=System.Windows.Forms.DockStyle.Fill;
			this.RichPanelTests.Font=new System.Drawing.Font("Trebuchet MS",9.75F,System.Drawing.FontStyle.Bold);
			this.RichPanelTests.GradientDirection=System.Drawing.Drawing2D.LinearGradientMode.Vertical;
			this.RichPanelTests.GradientEnd=System.Drawing.Color.WhiteSmoke;
			this.RichPanelTests.GradientStart=System.Drawing.Color.WhiteSmoke;
			this.RichPanelTests.Location=new System.Drawing.Point(0,147);
			this.RichPanelTests.Name="RichPanelTests";
			this.RichPanelTests.PanelIcon=null;
			this.RichPanelTests.PanelIconVisible=false;
			this.RichPanelTests.Size=new System.Drawing.Size(325,407);
			this.RichPanelTests.TabIndex=21;
			this.RichPanelTests.TextAntialias=true;
			// 
			// RichTextBoxTestsResult
			// 
			this.RichTextBoxTestsResult.AutoWordSelection=true;
			this.RichTextBoxTestsResult.BackColor=System.Drawing.Color.Gainsboro;
			this.RichTextBoxTestsResult.BorderStyle=System.Windows.Forms.BorderStyle.None;
			this.RichTextBoxTestsResult.DetectUrls=false;
			this.RichTextBoxTestsResult.Dock=System.Windows.Forms.DockStyle.Fill;
			this.RichTextBoxTestsResult.Location=new System.Drawing.Point(0,0);
			this.RichTextBoxTestsResult.Name="RichTextBoxTestsResult";
			this.RichTextBoxTestsResult.ReadOnly=true;
			this.RichTextBoxTestsResult.Size=new System.Drawing.Size(318,374);
			this.RichTextBoxTestsResult.TabIndex=18;
			this.RichTextBoxTestsResult.Text="";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock=System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel=System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location=new System.Drawing.Point(0,0);
			this.splitContainer1.Name="splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.RichPanelTests);
			this.splitContainer1.Panel1.Controls.Add(this.RichPanelGeneration);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.Chart);
			this.splitContainer1.Size=new System.Drawing.Size(815,554);
			this.splitContainer1.SplitterDistance=325;
			this.splitContainer1.TabIndex=15;
			// 
			// Chart
			// 
			this.Chart.Dock=System.Windows.Forms.DockStyle.Fill;
			this.Chart.IsEnableHPan=true;
			this.Chart.IsEnableVPan=true;
			this.Chart.IsEnableZoom=true;
			this.Chart.IsScrollY2=false;
			this.Chart.IsShowContextMenu=true;
			this.Chart.IsShowHScrollBar=false;
			this.Chart.IsShowPointValues=false;
			this.Chart.IsShowVScrollBar=false;
			this.Chart.IsZoomOnMouseCenter=false;
			this.Chart.Location=new System.Drawing.Point(0,0);
			this.Chart.Name="Chart";
			this.Chart.PanButtons=System.Windows.Forms.MouseButtons.Left;
			this.Chart.PanButtons2=System.Windows.Forms.MouseButtons.Middle;
			this.Chart.PanModifierKeys2=System.Windows.Forms.Keys.None;
			this.Chart.PointDateFormat="g";
			this.Chart.PointValueFormat="G";
			this.Chart.ScrollMaxX=0;
			this.Chart.ScrollMaxY=0;
			this.Chart.ScrollMaxY2=0;
			this.Chart.ScrollMinX=0;
			this.Chart.ScrollMinY=0;
			this.Chart.ScrollMinY2=0;
			this.Chart.Size=new System.Drawing.Size(486,554);
			this.Chart.TabIndex=0;
			this.Chart.ZoomButtons=System.Windows.Forms.MouseButtons.Left;
			this.Chart.ZoomButtons2=System.Windows.Forms.MouseButtons.None;
			this.Chart.ZoomModifierKeys=System.Windows.Forms.Keys.None;
			this.Chart.ZoomModifierKeys2=System.Windows.Forms.Keys.None;
			this.Chart.ZoomStepFraction=0.1;
			// 
			// RootFindingTesterForm
			// 
			this.AutoScaleDimensions=new System.Drawing.SizeF(6F,13F);
			this.AutoScaleMode=System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize=new System.Drawing.Size(815,554);
			this.Controls.Add(this.splitContainer1);
			this.FormBorderStyle=System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name="RootFindingTesterForm";
			this.ShowIcon=false;
			this.Text="RootFindingTesterForm";
			((System.ComponentModel.ISupportInitialize)(this.NumericUpDownDistributionSize)).EndInit();
			this.RichPanelGeneration.ResumeLayout(false);
			this.RichPanelGeneration.PerformLayout();
			this.RichPanelTests.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.NumericUpDown NumericUpDownDistributionSize;
		private KIS.Controls.Windows.HeaderPanel RichPanelGeneration;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Button ButtonGenerateDistribution;
		private KIS.Controls.Windows.HeaderPanel RichPanelTests;
		private System.Windows.Forms.RichTextBox RichTextBoxTestsResult;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox3;
		private ZedGraph.ZedGraphControl Chart;
	}
}