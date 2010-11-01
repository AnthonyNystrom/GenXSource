namespace Genetibase.ApplicationBlocks.ImageExport
{
	partial class NuGenResolutionBlock
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
			this.resolutionGroup = new System.Windows.Forms.GroupBox();
			this._heightNumeric = new Genetibase.ApplicationBlocks.ImageExport.NuGenResolutionSpin();
			this._widthNumeric = new Genetibase.ApplicationBlocks.ImageExport.NuGenResolutionSpin();
			this.aspectRatioCheckBox = new System.Windows.Forms.CheckBox();
			this.resolutionGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this._heightNumeric)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this._widthNumeric)).BeginInit();
			this.SuspendLayout();
			// 
			// resolutionGroup
			// 
			this.resolutionGroup.Controls.Add(this._heightNumeric);
			this.resolutionGroup.Controls.Add(this._widthNumeric);
			this.resolutionGroup.Controls.Add(this.aspectRatioCheckBox);
			this.resolutionGroup.Dock = System.Windows.Forms.DockStyle.Fill;
			this.resolutionGroup.Location = new System.Drawing.Point(0, 0);
			this.resolutionGroup.Name = "resolutionGroup";
			this.resolutionGroup.Size = new System.Drawing.Size(138, 100);
			this.resolutionGroup.TabIndex = 20;
			this.resolutionGroup.TabStop = false;
			this.resolutionGroup.Text = "Resolution";
			// 
			// heightNumeric
			// 
			this._heightNumeric.Location = new System.Drawing.Point(12, 68);
			this._heightNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this._heightNumeric.Name = "heightNumeric";
			this._heightNumeric.Size = new System.Drawing.Size(118, 20);
			this._heightNumeric.TabIndex = 20;
			this._heightNumeric.ValueChanged += new System.EventHandler(this.heightNumeric_ValueChanged);
			this._heightNumeric.KeyUp += new System.Windows.Forms.KeyEventHandler(this.heightNumeric_KeyUp);
			// 
			// widthNumeric
			// 
			this._widthNumeric.Location = new System.Drawing.Point(12, 42);
			this._widthNumeric.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
			this._widthNumeric.Name = "widthNumeric";
			this._widthNumeric.Size = new System.Drawing.Size(118, 20);
			this._widthNumeric.TabIndex = 19;
			this._widthNumeric.ValueChanged += new System.EventHandler(this.widthNumeric_ValueChanged);
			this._widthNumeric.KeyUp += new System.Windows.Forms.KeyEventHandler(this.widthNumeric_KeyUp);
			// 
			// aspectRatioCheckBox
			// 
			this.aspectRatioCheckBox.AutoSize = true;
			this.aspectRatioCheckBox.Checked = true;
			this.aspectRatioCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.aspectRatioCheckBox.Location = new System.Drawing.Point(6, 19);
			this.aspectRatioCheckBox.Name = "aspectRatioCheckBox";
			this.aspectRatioCheckBox.Size = new System.Drawing.Size(124, 17);
			this.aspectRatioCheckBox.TabIndex = 18;
			this.aspectRatioCheckBox.Text = "Maintain aspect ratio";
			this.aspectRatioCheckBox.UseVisualStyleBackColor = true;
			this.aspectRatioCheckBox.CheckedChanged += new System.EventHandler(this.aspectRatioCheckBox_CheckedChanged);
			// 
			// NuGenResolutionBlock
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.resolutionGroup);
			this.Name = "NuGenResolutionBlock";
			this.Size = new System.Drawing.Size(138, 100);
			this.resolutionGroup.ResumeLayout(false);
			this.resolutionGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this._heightNumeric)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this._widthNumeric)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox resolutionGroup;
		private System.Windows.Forms.CheckBox aspectRatioCheckBox;
		private NuGenResolutionSpin _heightNumeric;
		private NuGenResolutionSpin _widthNumeric;
	}
}
