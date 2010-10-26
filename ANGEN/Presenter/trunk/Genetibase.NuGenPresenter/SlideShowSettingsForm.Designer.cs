namespace Genetibase.NuGenPresenter
{
	partial class SlideShowSettingsForm
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
			this._bkgndPanel = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this._fitScreenCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._secondsLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._latencySpin = new Genetibase.SmoothControls.NuGenSmoothSpin();
			this._latencyLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._randomCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._imageAlignLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._stretchCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._alignSelector = new Genetibase.SmoothControls.NuGenSmoothAlignSelector();
			this._bkgndLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._colorBox = new Genetibase.SmoothControls.NuGenSmoothColorBox();
			this._sourceLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._cancelButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this._okButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this._directorySelector = new Genetibase.SmoothControls.NuGenSmoothDirectorySelector();
			this._bkgndPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _bkgndPanel
			// 
			this._bkgndPanel.Controls.Add(this._fitScreenCheckBox);
			this._bkgndPanel.Controls.Add(this._secondsLabel);
			this._bkgndPanel.Controls.Add(this._latencySpin);
			this._bkgndPanel.Controls.Add(this._latencyLabel);
			this._bkgndPanel.Controls.Add(this._randomCheckBox);
			this._bkgndPanel.Controls.Add(this._imageAlignLabel);
			this._bkgndPanel.Controls.Add(this._stretchCheckBox);
			this._bkgndPanel.Controls.Add(this._alignSelector);
			this._bkgndPanel.Controls.Add(this._bkgndLabel);
			this._bkgndPanel.Controls.Add(this._colorBox);
			this._bkgndPanel.Controls.Add(this._sourceLabel);
			this._bkgndPanel.Controls.Add(this._cancelButton);
			this._bkgndPanel.Controls.Add(this._okButton);
			this._bkgndPanel.Controls.Add(this._directorySelector);
			this._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bkgndPanel.DrawBorder = false;
			this._bkgndPanel.ExtendedBackground = true;
			this._bkgndPanel.Location = new System.Drawing.Point(0, 0);
			this._bkgndPanel.Name = "_bkgndPanel";
			this._bkgndPanel.Size = new System.Drawing.Size(447, 384);
			this._bkgndPanel.TabIndex = 0;
			// 
			// _fitScreenCheckBox
			// 
			this._fitScreenCheckBox.AutoSize = false;
			this._fitScreenCheckBox.Checked = true;
			this._fitScreenCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this._fitScreenCheckBox.Enabled = false;
			this._fitScreenCheckBox.Location = new System.Drawing.Point(158, 162);
			this._fitScreenCheckBox.Name = "_fitScreenCheckBox";
			this._fitScreenCheckBox.Size = new System.Drawing.Size(170, 24);
			this._fitScreenCheckBox.TabIndex = 17;
			this._fitScreenCheckBox.UseVisualStyleBackColor = false;
			// 
			// _secondsLabel
			// 
			this._secondsLabel.AutoSize = false;
			this._secondsLabel.Location = new System.Drawing.Point(264, 72);
			this._secondsLabel.Name = "_secondsLabel";
			this._secondsLabel.Size = new System.Drawing.Size(64, 21);
			this._secondsLabel.TabIndex = 16;
			// 
			// _latencySpin
			// 
			this._latencySpin.Location = new System.Drawing.Point(158, 72);
			this._latencySpin.Maximum = 999;
			this._latencySpin.Minimum = 1;
			this._latencySpin.Name = "_latencySpin";
			this._latencySpin.Size = new System.Drawing.Size(100, 20);
			this._latencySpin.TabIndex = 15;
			this._latencySpin.Value = 5;
			// 
			// _latencyLabel
			// 
			this._latencyLabel.AutoSize = false;
			this._latencyLabel.Location = new System.Drawing.Point(12, 72);
			this._latencyLabel.Name = "_latencyLabel";
			this._latencyLabel.Size = new System.Drawing.Size(140, 24);
			this._latencyLabel.TabIndex = 14;
			// 
			// _randomCheckBox
			// 
			this._randomCheckBox.AutoSize = false;
			this._randomCheckBox.Checked = true;
			this._randomCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this._randomCheckBox.Enabled = false;
			this._randomCheckBox.Location = new System.Drawing.Point(158, 132);
			this._randomCheckBox.Name = "_randomCheckBox";
			this._randomCheckBox.Size = new System.Drawing.Size(170, 24);
			this._randomCheckBox.TabIndex = 13;
			this._randomCheckBox.UseVisualStyleBackColor = false;
			// 
			// _imageAlignLabel
			// 
			this._imageAlignLabel.AutoSize = false;
			this._imageAlignLabel.Location = new System.Drawing.Point(12, 102);
			this._imageAlignLabel.Name = "_imageAlignLabel";
			this._imageAlignLabel.Size = new System.Drawing.Size(140, 24);
			this._imageAlignLabel.TabIndex = 12;
			// 
			// _stretchCheckBox
			// 
			this._stretchCheckBox.AutoSize = false;
			this._stretchCheckBox.Checked = true;
			this._stretchCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this._stretchCheckBox.Location = new System.Drawing.Point(158, 102);
			this._stretchCheckBox.Name = "_stretchCheckBox";
			this._stretchCheckBox.Size = new System.Drawing.Size(170, 24);
			this._stretchCheckBox.TabIndex = 7;
			this._stretchCheckBox.UseVisualStyleBackColor = false;
			this._stretchCheckBox.CheckedChanged += new System.EventHandler(this._stretchCheckBox_CheckedChanged);
			// 
			// _alignSelector
			// 
			this._alignSelector.BackColor = System.Drawing.Color.Transparent;
			this._alignSelector.Enabled = false;
			this._alignSelector.Location = new System.Drawing.Point(158, 192);
			this._alignSelector.Name = "_alignSelector";
			this._alignSelector.Size = new System.Drawing.Size(170, 139);
			this._alignSelector.TabIndex = 11;
			// 
			// _bkgndLabel
			// 
			this._bkgndLabel.AutoSize = false;
			this._bkgndLabel.Location = new System.Drawing.Point(12, 42);
			this._bkgndLabel.Name = "_bkgndLabel";
			this._bkgndLabel.Size = new System.Drawing.Size(140, 24);
			this._bkgndLabel.TabIndex = 6;
			// 
			// _colorBox
			// 
			this._colorBox.Location = new System.Drawing.Point(158, 45);
			this._colorBox.Name = "_colorBox";
			this._colorBox.SelectedColor = System.Drawing.Color.Black;
			this._colorBox.Size = new System.Drawing.Size(170, 21);
			this._colorBox.TabIndex = 5;
			// 
			// _sourceLabel
			// 
			this._sourceLabel.AutoSize = false;
			this._sourceLabel.Location = new System.Drawing.Point(12, 12);
			this._sourceLabel.Name = "_sourceLabel";
			this._sourceLabel.Size = new System.Drawing.Size(140, 24);
			this._sourceLabel.TabIndex = 4;
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(335, 342);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(100, 30);
			this._cancelButton.TabIndex = 3;
			this._cancelButton.UseVisualStyleBackColor = false;
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(228, 342);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(100, 30);
			this._okButton.TabIndex = 2;
			this._okButton.UseVisualStyleBackColor = false;
			// 
			// _directorySelector
			// 
			this._directorySelector.CanChooseDirectory = true;
			this._directorySelector.Location = new System.Drawing.Point(158, 12);
			this._directorySelector.Name = "_directorySelector";
			this._directorySelector.Size = new System.Drawing.Size(276, 24);
			this._directorySelector.TabIndex = 0;
			// 
			// SlideShowSettingsForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(447, 384);
			this.Controls.Add(this._bkgndPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SlideShowSettingsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this._bkgndPanel.ResumeLayout(false);
			this._bkgndPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothPanel _bkgndPanel;
		private Genetibase.SmoothControls.NuGenSmoothDirectorySelector _directorySelector;
		private Genetibase.SmoothControls.NuGenSmoothButton _cancelButton;
		private Genetibase.SmoothControls.NuGenSmoothButton _okButton;
		private Genetibase.Shared.Controls.NuGenLabel _sourceLabel;
		private Genetibase.Shared.Controls.NuGenLabel _bkgndLabel;
		private Genetibase.SmoothControls.NuGenSmoothColorBox _colorBox;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _stretchCheckBox;
		private Genetibase.SmoothControls.NuGenSmoothAlignSelector _alignSelector;
		private Genetibase.Shared.Controls.NuGenLabel _imageAlignLabel;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _randomCheckBox;
		private Genetibase.Shared.Controls.NuGenLabel _secondsLabel;
		private Genetibase.SmoothControls.NuGenSmoothSpin _latencySpin;
		private Genetibase.Shared.Controls.NuGenLabel _latencyLabel;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _fitScreenCheckBox;
	}
}