namespace Genetibase.NuGenPresenter
{
	partial class OptionsForm
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
			this._breakTimerGroupBox = new Genetibase.SmoothControls.NuGenSmoothGroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this._slideShowCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._slideShowSettingsButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this._minutesLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._intervalLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._intervalSpin = new Genetibase.SmoothControls.NuGenSmoothSpin();
			this._drawingGroupBox = new Genetibase.SmoothControls.NuGenSmoothGroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this._penColorLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._penColorBox = new Genetibase.SmoothControls.NuGenSmoothColorBox();
			this._penWidthLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._penWidthCombo = new Genetibase.SmoothControls.NuGenSmoothComboBox();
			this._hotKeysGroupBox = new Genetibase.SmoothControls.NuGenSmoothGroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this._zoomModeHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._drawModeHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._drawModeLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._zoomModeLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._escapeHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._escapeLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._zoomOutLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._zoomOutHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._clearLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._clearHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._zoomInLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._showPointerLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._saveHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._lockTransformHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._lockTransformLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._showPointerHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._zoomInHotKeys = new Genetibase.SmoothControls.NuGenSmoothHotKeySelector();
			this._saveLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._cancelButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this._okButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this._bkgndPanel.SuspendLayout();
			this._breakTimerGroupBox.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this._drawingGroupBox.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this._hotKeysGroupBox.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _bkgndPanel
			// 
			this._bkgndPanel.Controls.Add(this._breakTimerGroupBox);
			this._bkgndPanel.Controls.Add(this._drawingGroupBox);
			this._bkgndPanel.Controls.Add(this._hotKeysGroupBox);
			this._bkgndPanel.Controls.Add(this._cancelButton);
			this._bkgndPanel.Controls.Add(this._okButton);
			this._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bkgndPanel.DrawBorder = false;
			this._bkgndPanel.ExtendedBackground = true;
			this._bkgndPanel.Location = new System.Drawing.Point(0, 0);
			this._bkgndPanel.Name = "_bkgndPanel";
			this._bkgndPanel.Size = new System.Drawing.Size(440, 449);
			this._bkgndPanel.TabIndex = 1;
			// 
			// _breakTimerGroupBox
			// 
			this._breakTimerGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._breakTimerGroupBox.Controls.Add(this.tableLayoutPanel3);
			this._breakTimerGroupBox.Location = new System.Drawing.Point(12, 349);
			this._breakTimerGroupBox.Name = "_breakTimerGroupBox";
			this._breakTimerGroupBox.Opaque = false;
			this._breakTimerGroupBox.Size = new System.Drawing.Size(305, 87);
			this._breakTimerGroupBox.TabIndex = 30;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 3;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel3.Controls.Add(this._slideShowCheckBox, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this._slideShowSettingsButton, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this._minutesLabel, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this._intervalLabel, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this._intervalSpin, 1, 0);
			this.tableLayoutPanel3.Location = new System.Drawing.Point(9, 21);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(287, 58);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// _slideShowCheckBox
			// 
			this._slideShowCheckBox.AutoSize = false;
			this._slideShowCheckBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._slideShowCheckBox.Location = new System.Drawing.Point(3, 27);
			this._slideShowCheckBox.Name = "_slideShowCheckBox";
			this._slideShowCheckBox.Size = new System.Drawing.Size(137, 28);
			this._slideShowCheckBox.TabIndex = 20;
			this._slideShowCheckBox.UseVisualStyleBackColor = false;
			// 
			// _slideShowSettingsButton
			// 
			this.tableLayoutPanel3.SetColumnSpan(this._slideShowSettingsButton, 2);
			this._slideShowSettingsButton.Dock = System.Windows.Forms.DockStyle.Fill;
			this._slideShowSettingsButton.Location = new System.Drawing.Point(146, 27);
			this._slideShowSettingsButton.Name = "_slideShowSettingsButton";
			this._slideShowSettingsButton.Size = new System.Drawing.Size(138, 28);
			this._slideShowSettingsButton.TabIndex = 30;
			this._slideShowSettingsButton.UseVisualStyleBackColor = false;
			this._slideShowSettingsButton.Click += new System.EventHandler(this._slideShowSettingsButton_Click);
			// 
			// _minutesLabel
			// 
			this._minutesLabel.AutoSize = false;
			this._minutesLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._minutesLabel.Location = new System.Drawing.Point(203, 3);
			this._minutesLabel.Name = "_minutesLabel";
			this._minutesLabel.Size = new System.Drawing.Size(81, 18);
			this._minutesLabel.TabIndex = 42;
			// 
			// _intervalLabel
			// 
			this._intervalLabel.AutoSize = false;
			this._intervalLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._intervalLabel.Location = new System.Drawing.Point(3, 3);
			this._intervalLabel.Name = "_intervalLabel";
			this._intervalLabel.Size = new System.Drawing.Size(137, 18);
			this._intervalLabel.TabIndex = 42;
			// 
			// _intervalSpin
			// 
			this._intervalSpin.Location = new System.Drawing.Point(146, 3);
			this._intervalSpin.Maximum = 99;
			this._intervalSpin.Minimum = 1;
			this._intervalSpin.Name = "_intervalSpin";
			this._intervalSpin.Size = new System.Drawing.Size(50, 20);
			this._intervalSpin.TabIndex = 10;
			this._intervalSpin.Value = 1;
			// 
			// _drawingGroupBox
			// 
			this._drawingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this._drawingGroupBox.Controls.Add(this.tableLayoutPanel2);
			this._drawingGroupBox.Location = new System.Drawing.Point(12, 264);
			this._drawingGroupBox.Name = "_drawingGroupBox";
			this._drawingGroupBox.Opaque = false;
			this._drawingGroupBox.Size = new System.Drawing.Size(305, 79);
			this._drawingGroupBox.TabIndex = 20;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this._penColorLabel, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this._penColorBox, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this._penWidthLabel, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this._penWidthCombo, 1, 1);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(9, 21);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(287, 49);
			this.tableLayoutPanel2.TabIndex = 11;
			// 
			// _penColorLabel
			// 
			this._penColorLabel.AutoSize = false;
			this._penColorLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._penColorLabel.Location = new System.Drawing.Point(3, 3);
			this._penColorLabel.Name = "_penColorLabel";
			this._penColorLabel.Size = new System.Drawing.Size(137, 18);
			this._penColorLabel.TabIndex = 5;
			// 
			// _penColorBox
			// 
			this._penColorBox.Location = new System.Drawing.Point(146, 3);
			this._penColorBox.Name = "_penColorBox";
			this._penColorBox.Size = new System.Drawing.Size(138, 21);
			this._penColorBox.TabIndex = 10;
			// 
			// _penWidthLabel
			// 
			this._penWidthLabel.AutoSize = false;
			this._penWidthLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._penWidthLabel.Location = new System.Drawing.Point(3, 27);
			this._penWidthLabel.Name = "_penWidthLabel";
			this._penWidthLabel.Size = new System.Drawing.Size(137, 19);
			this._penWidthLabel.TabIndex = 7;
			// 
			// _penWidthCombo
			// 
			this._penWidthCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this._penWidthCombo.FormattingEnabled = true;
			this._penWidthCombo.Location = new System.Drawing.Point(146, 27);
			this._penWidthCombo.Name = "_penWidthCombo";
			this._penWidthCombo.Size = new System.Drawing.Size(138, 21);
			this._penWidthCombo.TabIndex = 20;
			// 
			// _hotKeysGroupBox
			// 
			this._hotKeysGroupBox.Controls.Add(this.tableLayoutPanel1);
			this._hotKeysGroupBox.Location = new System.Drawing.Point(12, 12);
			this._hotKeysGroupBox.Name = "_hotKeysGroupBox";
			this._hotKeysGroupBox.Opaque = false;
			this._hotKeysGroupBox.Size = new System.Drawing.Size(305, 246);
			this._hotKeysGroupBox.TabIndex = 10;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this._zoomModeHotKeys, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this._drawModeHotKeys, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this._drawModeLabel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this._zoomModeLabel, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this._escapeHotKeys, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this._escapeLabel, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this._zoomOutLabel, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this._zoomOutHotKeys, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this._clearLabel, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this._clearHotKeys, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this._zoomInLabel, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this._showPointerLabel, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this._saveHotKeys, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this._lockTransformHotKeys, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this._lockTransformLabel, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this._showPointerHotKeys, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this._zoomInHotKeys, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this._saveLabel, 0, 4);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 21);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 9;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(287, 217);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// _zoomModeHotKeys
			// 
			this._zoomModeHotKeys.Location = new System.Drawing.Point(146, 27);
			this._zoomModeHotKeys.Name = "_zoomModeHotKeys";
			this._zoomModeHotKeys.Size = new System.Drawing.Size(138, 21);
			this._zoomModeHotKeys.TabIndex = 20;
			// 
			// _drawModeHotKeys
			// 
			this._drawModeHotKeys.Location = new System.Drawing.Point(146, 3);
			this._drawModeHotKeys.Name = "_drawModeHotKeys";
			this._drawModeHotKeys.Size = new System.Drawing.Size(138, 21);
			this._drawModeHotKeys.TabIndex = 10;
			// 
			// _drawModeLabel
			// 
			this._drawModeLabel.AutoSize = false;
			this._drawModeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._drawModeLabel.Location = new System.Drawing.Point(3, 3);
			this._drawModeLabel.Name = "_drawModeLabel";
			this._drawModeLabel.Size = new System.Drawing.Size(137, 18);
			this._drawModeLabel.TabIndex = 62;
			// 
			// _zoomModeLabel
			// 
			this._zoomModeLabel.AutoSize = false;
			this._zoomModeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._zoomModeLabel.Location = new System.Drawing.Point(3, 27);
			this._zoomModeLabel.Name = "_zoomModeLabel";
			this._zoomModeLabel.Size = new System.Drawing.Size(137, 18);
			this._zoomModeLabel.TabIndex = 42;
			// 
			// _escapeHotKeys
			// 
			this._escapeHotKeys.Location = new System.Drawing.Point(146, 75);
			this._escapeHotKeys.Name = "_escapeHotKeys";
			this._escapeHotKeys.Size = new System.Drawing.Size(138, 21);
			this._escapeHotKeys.TabIndex = 40;
			// 
			// _escapeLabel
			// 
			this._escapeLabel.AutoSize = false;
			this._escapeLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._escapeLabel.Location = new System.Drawing.Point(3, 75);
			this._escapeLabel.Name = "_escapeLabel";
			this._escapeLabel.Size = new System.Drawing.Size(137, 18);
			this._escapeLabel.TabIndex = 41;
			// 
			// _zoomOutLabel
			// 
			this._zoomOutLabel.AutoSize = false;
			this._zoomOutLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._zoomOutLabel.Location = new System.Drawing.Point(3, 195);
			this._zoomOutLabel.Name = "_zoomOutLabel";
			this._zoomOutLabel.Size = new System.Drawing.Size(137, 19);
			this._zoomOutLabel.TabIndex = 22;
			// 
			// _zoomOutHotKeys
			// 
			this._zoomOutHotKeys.Location = new System.Drawing.Point(146, 195);
			this._zoomOutHotKeys.Name = "_zoomOutHotKeys";
			this._zoomOutHotKeys.Size = new System.Drawing.Size(138, 21);
			this._zoomOutHotKeys.TabIndex = 90;
			// 
			// _clearLabel
			// 
			this._clearLabel.AutoSize = false;
			this._clearLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._clearLabel.Location = new System.Drawing.Point(3, 51);
			this._clearLabel.Name = "_clearLabel";
			this._clearLabel.Size = new System.Drawing.Size(137, 18);
			this._clearLabel.TabIndex = 13;
			// 
			// _clearHotKeys
			// 
			this._clearHotKeys.Location = new System.Drawing.Point(146, 51);
			this._clearHotKeys.Name = "_clearHotKeys";
			this._clearHotKeys.Size = new System.Drawing.Size(138, 21);
			this._clearHotKeys.TabIndex = 30;
			// 
			// _zoomInLabel
			// 
			this._zoomInLabel.AutoSize = false;
			this._zoomInLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._zoomInLabel.Location = new System.Drawing.Point(3, 171);
			this._zoomInLabel.Name = "_zoomInLabel";
			this._zoomInLabel.Size = new System.Drawing.Size(137, 18);
			this._zoomInLabel.TabIndex = 14;
			// 
			// _showPointerLabel
			// 
			this._showPointerLabel.AutoSize = false;
			this._showPointerLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._showPointerLabel.Location = new System.Drawing.Point(3, 147);
			this._showPointerLabel.Name = "_showPointerLabel";
			this._showPointerLabel.Size = new System.Drawing.Size(137, 18);
			this._showPointerLabel.TabIndex = 15;
			// 
			// _saveHotKeys
			// 
			this._saveHotKeys.Location = new System.Drawing.Point(146, 99);
			this._saveHotKeys.Name = "_saveHotKeys";
			this._saveHotKeys.Size = new System.Drawing.Size(138, 21);
			this._saveHotKeys.TabIndex = 50;
			// 
			// _lockTransformHotKeys
			// 
			this._lockTransformHotKeys.Location = new System.Drawing.Point(146, 123);
			this._lockTransformHotKeys.Name = "_lockTransformHotKeys";
			this._lockTransformHotKeys.Size = new System.Drawing.Size(138, 21);
			this._lockTransformHotKeys.TabIndex = 60;
			// 
			// _lockTransformLabel
			// 
			this._lockTransformLabel.AutoSize = false;
			this._lockTransformLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._lockTransformLabel.Location = new System.Drawing.Point(3, 123);
			this._lockTransformLabel.Name = "_lockTransformLabel";
			this._lockTransformLabel.Size = new System.Drawing.Size(137, 18);
			this._lockTransformLabel.TabIndex = 12;
			// 
			// _showPointerHotKeys
			// 
			this._showPointerHotKeys.Location = new System.Drawing.Point(146, 147);
			this._showPointerHotKeys.Name = "_showPointerHotKeys";
			this._showPointerHotKeys.Size = new System.Drawing.Size(138, 21);
			this._showPointerHotKeys.TabIndex = 70;
			// 
			// _zoomInHotKeys
			// 
			this._zoomInHotKeys.Location = new System.Drawing.Point(146, 171);
			this._zoomInHotKeys.Name = "_zoomInHotKeys";
			this._zoomInHotKeys.Size = new System.Drawing.Size(138, 21);
			this._zoomInHotKeys.TabIndex = 80;
			// 
			// _saveLabel
			// 
			this._saveLabel.AutoSize = false;
			this._saveLabel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._saveLabel.Location = new System.Drawing.Point(3, 99);
			this._saveLabel.Name = "_saveLabel";
			this._saveLabel.Size = new System.Drawing.Size(137, 18);
			this._saveLabel.TabIndex = 10;
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(328, 48);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(100, 30);
			this._cancelButton.TabIndex = 20;
			this._cancelButton.UseVisualStyleBackColor = false;
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(328, 12);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(100, 30);
			this._okButton.TabIndex = 10;
			this._okButton.UseVisualStyleBackColor = false;
			this._okButton.Click += new System.EventHandler(this._okButton_Click);
			// 
			// OptionsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(440, 449);
			this.Controls.Add(this._bkgndPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "OptionsForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this._bkgndPanel.ResumeLayout(false);
			this._breakTimerGroupBox.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this._drawingGroupBox.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this._hotKeysGroupBox.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothPanel _bkgndPanel;
		private Genetibase.SmoothControls.NuGenSmoothButton _cancelButton;
		private Genetibase.SmoothControls.NuGenSmoothButton _okButton;
		private Genetibase.Shared.Controls.NuGenLabel _penColorLabel;
		private Genetibase.SmoothControls.NuGenSmoothColorBox _penColorBox;
		private Genetibase.Shared.Controls.NuGenLabel _penWidthLabel;
		private Genetibase.SmoothControls.NuGenSmoothComboBox _penWidthCombo;
		private Genetibase.SmoothControls.NuGenSmoothGroupBox _hotKeysGroupBox;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _clearHotKeys;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _zoomOutHotKeys;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _zoomInHotKeys;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _showPointerHotKeys;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _lockTransformHotKeys;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _saveHotKeys;
		private Genetibase.Shared.Controls.NuGenLabel _showPointerLabel;
		private Genetibase.Shared.Controls.NuGenLabel _zoomInLabel;
		private Genetibase.Shared.Controls.NuGenLabel _clearLabel;
		private Genetibase.Shared.Controls.NuGenLabel _lockTransformLabel;
		private Genetibase.Shared.Controls.NuGenLabel _saveLabel;
		private Genetibase.Shared.Controls.NuGenLabel _zoomOutLabel;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private Genetibase.SmoothControls.NuGenSmoothGroupBox _drawingGroupBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _escapeHotKeys;
		private Genetibase.Shared.Controls.NuGenLabel _escapeLabel;
		private Genetibase.SmoothControls.NuGenSmoothGroupBox _breakTimerGroupBox;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private Genetibase.Shared.Controls.NuGenLabel _intervalLabel;
		private Genetibase.SmoothControls.NuGenSmoothSpin _intervalSpin;
		private Genetibase.Shared.Controls.NuGenLabel _minutesLabel;
		private Genetibase.SmoothControls.NuGenSmoothButton _slideShowSettingsButton;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _slideShowCheckBox;
		private Genetibase.Shared.Controls.NuGenLabel _drawModeLabel;
		private Genetibase.Shared.Controls.NuGenLabel _zoomModeLabel;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _zoomModeHotKeys;
		private Genetibase.SmoothControls.NuGenSmoothHotKeySelector _drawModeHotKeys;
	}
}