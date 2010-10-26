namespace Genetibase.NuGenVisiCalc
{
	partial class CanvasSizeControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._maintainAspectRatioCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._widthSpin = new Genetibase.SmoothControls.NuGenSmoothSpin();
			this._heightSpin = new Genetibase.SmoothControls.NuGenSmoothSpin();
			this._widthLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._heightLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._heightUnitsPixels = new Genetibase.Shared.Controls.NuGenLabel();
			this._widthUnitsLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._okLinkLabel = new Genetibase.Shared.Controls.NuGenLinkLabel();
			this._cancelLinkLabel = new Genetibase.Shared.Controls.NuGenLinkLabel();
			this.SuspendLayout();
			// 
			// _maintainAspectRatioCheckBox
			// 
			this._maintainAspectRatioCheckBox.Location = new System.Drawing.Point(3, 3);
			this._maintainAspectRatioCheckBox.Name = "_maintainAspectRatioCheckBox";
			this._maintainAspectRatioCheckBox.Size = new System.Drawing.Size(124, 17);
			this._maintainAspectRatioCheckBox.TabIndex = 10;
			this._maintainAspectRatioCheckBox.Text = "&Maintain aspect ratio";
			this._maintainAspectRatioCheckBox.UseVisualStyleBackColor = false;
			this._maintainAspectRatioCheckBox.CheckedChanged += new System.EventHandler(this._maintainAspectRatioCheckBox_CheckedChanged);
			// 
			// _widthSpin
			// 
			this._widthSpin.Location = new System.Drawing.Point(57, 26);
			this._widthSpin.Maximum = 4096;
			this._widthSpin.Minimum = 1;
			this._widthSpin.Name = "_widthSpin";
			this._widthSpin.Size = new System.Drawing.Size(70, 20);
			this._widthSpin.TabIndex = 20;
			this._widthSpin.Value = 1;
			this._widthSpin.ValueChanged += new System.EventHandler(this._widthSpin_ValueChanged);
			// 
			// _heightSpin
			// 
			this._heightSpin.Location = new System.Drawing.Point(57, 52);
			this._heightSpin.Maximum = 4096;
			this._heightSpin.Minimum = 1;
			this._heightSpin.Name = "_heightSpin";
			this._heightSpin.Size = new System.Drawing.Size(70, 20);
			this._heightSpin.TabIndex = 30;
			this._heightSpin.Value = 1;
			this._heightSpin.ValueChanged += new System.EventHandler(this._heightSpin_ValueChanged);
			// 
			// _widthLabel
			// 
			this._widthLabel.Location = new System.Drawing.Point(14, 30);
			this._widthLabel.Name = "_widthLabel";
			this._widthLabel.Size = new System.Drawing.Size(37, 13);
			this._widthLabel.TabIndex = 4;
			this._widthLabel.TabStop = false;
			this._widthLabel.Text = "Width";
			// 
			// _heightLabel
			// 
			this._heightLabel.Location = new System.Drawing.Point(14, 56);
			this._heightLabel.Name = "_heightLabel";
			this._heightLabel.Size = new System.Drawing.Size(41, 13);
			this._heightLabel.TabIndex = 5;
			this._heightLabel.TabStop = false;
			this._heightLabel.Text = "Height";
			// 
			// _heightUnitsPixels
			// 
			this._heightUnitsPixels.Location = new System.Drawing.Point(136, 56);
			this._heightUnitsPixels.Name = "_heightUnitsPixels";
			this._heightUnitsPixels.Size = new System.Drawing.Size(37, 13);
			this._heightUnitsPixels.TabIndex = 6;
			this._heightUnitsPixels.TabStop = false;
			this._heightUnitsPixels.Text = "pixels";
			// 
			// _widthUnitsLabel
			// 
			this._widthUnitsLabel.Location = new System.Drawing.Point(136, 30);
			this._widthUnitsLabel.Name = "_widthUnitsLabel";
			this._widthUnitsLabel.Size = new System.Drawing.Size(37, 13);
			this._widthUnitsLabel.TabIndex = 7;
			this._widthUnitsLabel.TabStop = false;
			this._widthUnitsLabel.Text = "pixels";
			// 
			// _okLinkLabel
			// 
			this._okLinkLabel.Location = new System.Drawing.Point(14, 83);
			this._okLinkLabel.Name = "_okLinkLabel";
			this._okLinkLabel.Size = new System.Drawing.Size(23, 13);
			this._okLinkLabel.TabIndex = 40;
			this._okLinkLabel.Target = null;
			this._okLinkLabel.Text = "Ok";
			this._okLinkLabel.Click += new System.EventHandler(this._okLinkLabel_Click);
			// 
			// _cancelLinkLabel
			// 
			this._cancelLinkLabel.Location = new System.Drawing.Point(57, 83);
			this._cancelLinkLabel.Name = "_cancelLinkLabel";
			this._cancelLinkLabel.Size = new System.Drawing.Size(43, 13);
			this._cancelLinkLabel.TabIndex = 50;
			this._cancelLinkLabel.Target = null;
			this._cancelLinkLabel.Text = "Cancel";
			this._cancelLinkLabel.Click += new System.EventHandler(this._cancelLinkLabel_Click);
			// 
			// CanvasSizeControl
			// 
			this.BackColor = System.Drawing.SystemColors.Control;
			this.Controls.Add(this._cancelLinkLabel);
			this.Controls.Add(this._okLinkLabel);
			this.Controls.Add(this._widthUnitsLabel);
			this.Controls.Add(this._heightUnitsPixels);
			this.Controls.Add(this._heightLabel);
			this.Controls.Add(this._widthLabel);
			this.Controls.Add(this._heightSpin);
			this.Controls.Add(this._widthSpin);
			this.Controls.Add(this._maintainAspectRatioCheckBox);
			this.Name = "CanvasSizeControl";
			this.Size = new System.Drawing.Size(170, 100);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothCheckBox _maintainAspectRatioCheckBox;
		private Genetibase.SmoothControls.NuGenSmoothSpin _widthSpin;
		private Genetibase.SmoothControls.NuGenSmoothSpin _heightSpin;
		private Genetibase.Shared.Controls.NuGenLabel _widthLabel;
		private Genetibase.Shared.Controls.NuGenLabel _heightLabel;
		private Genetibase.Shared.Controls.NuGenLabel _heightUnitsPixels;
		private Genetibase.Shared.Controls.NuGenLabel _widthUnitsLabel;
		private Genetibase.Shared.Controls.NuGenLinkLabel _okLinkLabel;
		private Genetibase.Shared.Controls.NuGenLinkLabel _cancelLinkLabel;
	}
}
