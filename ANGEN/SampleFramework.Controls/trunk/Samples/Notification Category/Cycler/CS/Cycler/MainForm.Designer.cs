namespace Cycler
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this._goButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this.nuGenSmoothPanelEx1 = new Genetibase.SmoothControls.NuGenSmoothPanelEx();
			this._marqueeProgressBar = new Genetibase.SmoothControls.NuGenSmoothProgressBar();
			this._vertProgressBar = new Genetibase.SmoothControls.NuGenSmoothProgressBar();
			this._horzProgressBar2 = new Genetibase.SmoothControls.NuGenSmoothProgressBar();
			this._horzProgressBar = new Genetibase.SmoothControls.NuGenSmoothProgressBar();
			this._scrollBar = new Genetibase.SmoothControls.NuGenSmoothScrollBar();
			this._horzTrackBar = new Genetibase.SmoothControls.NuGenSmoothTrackBar();
			this._timer = new System.Windows.Forms.Timer(this.components);
			this.nuGenSmoothPanelEx1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _goButton
			// 
			this._goButton.Location = new System.Drawing.Point(12, 120);
			this._goButton.Name = "_goButton";
			this._goButton.Size = new System.Drawing.Size(180, 41);
			this._goButton.TabIndex = 0;
			this._goButton.Text = "&Start";
			this._goButton.UseVisualStyleBackColor = false;
			this._goButton.Click += new System.EventHandler(this._goButton_Click);
			// 
			// nuGenSmoothPanelEx1
			// 
			this.nuGenSmoothPanelEx1.Controls.Add(this._marqueeProgressBar);
			this.nuGenSmoothPanelEx1.Controls.Add(this._vertProgressBar);
			this.nuGenSmoothPanelEx1.Controls.Add(this._horzProgressBar2);
			this.nuGenSmoothPanelEx1.Controls.Add(this._horzProgressBar);
			this.nuGenSmoothPanelEx1.Controls.Add(this._scrollBar);
			this.nuGenSmoothPanelEx1.Controls.Add(this._horzTrackBar);
			this.nuGenSmoothPanelEx1.Controls.Add(this._goButton);
			this.nuGenSmoothPanelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenSmoothPanelEx1.Location = new System.Drawing.Point(0, 0);
			this.nuGenSmoothPanelEx1.Name = "nuGenSmoothPanelEx1";
			this.nuGenSmoothPanelEx1.Size = new System.Drawing.Size(274, 204);
			this.nuGenSmoothPanelEx1.TabIndex = 1;
			// 
			// _marqueeProgressBar
			// 
			this._marqueeProgressBar.Location = new System.Drawing.Point(12, 167);
			this._marqueeProgressBar.Name = "_marqueeProgressBar";
			this._marqueeProgressBar.Size = new System.Drawing.Size(180, 25);
			this._marqueeProgressBar.Style = Genetibase.Shared.Controls.NuGenProgressBarStyle.Marquee;
			this._marqueeProgressBar.TabIndex = 5;
			// 
			// _vertProgressBar
			// 
			this._vertProgressBar.Location = new System.Drawing.Point(205, 12);
			this._vertProgressBar.Name = "_vertProgressBar";
			this._vertProgressBar.Orientation = Genetibase.Shared.NuGenOrientationStyle.Vertical;
			this._vertProgressBar.Size = new System.Drawing.Size(25, 180);
			this._vertProgressBar.TabIndex = 4;
			// 
			// _horzProgressBar2
			// 
			this._horzProgressBar2.Location = new System.Drawing.Point(12, 43);
			this._horzProgressBar2.Name = "_horzProgressBar2";
			this._horzProgressBar2.Size = new System.Drawing.Size(180, 25);
			this._horzProgressBar2.TabIndex = 3;
			// 
			// _horzProgressBar
			// 
			this._horzProgressBar.Location = new System.Drawing.Point(12, 12);
			this._horzProgressBar.Name = "_horzProgressBar";
			this._horzProgressBar.Size = new System.Drawing.Size(180, 25);
			this._horzProgressBar.Style = Genetibase.Shared.Controls.NuGenProgressBarStyle.Blocks;
			this._horzProgressBar.TabIndex = 3;
			// 
			// _scrollBar
			// 
			this._scrollBar.Location = new System.Drawing.Point(236, 12);
			this._scrollBar.Maximum = 100;
			this._scrollBar.Name = "_scrollBar";
			this._scrollBar.Orientation = Genetibase.Shared.NuGenOrientationStyle.Vertical;
			this._scrollBar.Size = new System.Drawing.Size(25, 180);
			this._scrollBar.TabIndex = 2;
			this._scrollBar.Value = 100;
			this._scrollBar.ValueChanged += new System.EventHandler(this._scrollBar_ValueChanged);
			// 
			// _horzTrackBar
			// 
			this._horzTrackBar.Location = new System.Drawing.Point(12, 74);
			this._horzTrackBar.Minimum = 1;
			this._horzTrackBar.Name = "_horzTrackBar";
			this._horzTrackBar.Size = new System.Drawing.Size(180, 40);
			this._horzTrackBar.TabIndex = 1;
			this._horzTrackBar.Value = 1;
			this._horzTrackBar.ValueChanged += new System.EventHandler(this._horzTrackBar_ValueChanged);
			// 
			// _timer
			// 
			this._timer.Interval = 500;
			this._timer.Tick += new System.EventHandler(this._timer_Tick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(274, 204);
			this.Controls.Add(this.nuGenSmoothPanelEx1);
			this.Name = "MainForm";
			this.Text = "Cycler";
			this.nuGenSmoothPanelEx1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothButton _goButton;
		private Genetibase.SmoothControls.NuGenSmoothPanelEx nuGenSmoothPanelEx1;
		private Genetibase.SmoothControls.NuGenSmoothProgressBar _marqueeProgressBar;
		private Genetibase.SmoothControls.NuGenSmoothProgressBar _vertProgressBar;
		private Genetibase.SmoothControls.NuGenSmoothProgressBar _horzProgressBar;
		private Genetibase.SmoothControls.NuGenSmoothScrollBar _scrollBar;
		private Genetibase.SmoothControls.NuGenSmoothTrackBar _horzTrackBar;
		private Genetibase.SmoothControls.NuGenSmoothProgressBar _horzProgressBar2;
		private System.Windows.Forms.Timer _timer;

	}
}

