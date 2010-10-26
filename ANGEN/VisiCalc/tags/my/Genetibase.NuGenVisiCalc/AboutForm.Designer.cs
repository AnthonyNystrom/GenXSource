namespace Genetibase.NuGenVisiCalc
{
	partial class AboutForm
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
			this.label1 = new System.Windows.Forms.Label();
			this._captionLabel = new System.Windows.Forms.Label();
			this._versionLabel = new System.Windows.Forms.Label();
			this._licenseLabel = new System.Windows.Forms.Label();
			this._okButton = new System.Windows.Forms.Button();
			this.nuGenDialogBlock1 = new Genetibase.Controls.NuGenDialogBlock();
			this._licenseTextBox = new System.Windows.Forms.TextBox();
			this.nuGenDialogBlock1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 67);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(177, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Copyright © 2006 Genetibase, Inc.";
			// 
			// _captionLabel
			// 
			this._captionLabel.AutoSize = true;
			this._captionLabel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._captionLabel.ForeColor = System.Drawing.SystemColors.ControlText;
			this._captionLabel.Location = new System.Drawing.Point(12, 9);
			this._captionLabel.Name = "_captionLabel";
			this._captionLabel.Size = new System.Drawing.Size(149, 23);
			this._captionLabel.TabIndex = 2;
			this._captionLabel.Text = "NuGenVisiCalc";
			// 
			// _versionLabel
			// 
			this._versionLabel.AutoSize = true;
			this._versionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._versionLabel.Location = new System.Drawing.Point(12, 33);
			this._versionLabel.Name = "_versionLabel";
			this._versionLabel.Size = new System.Drawing.Size(49, 13);
			this._versionLabel.TabIndex = 3;
			this._versionLabel.Text = "Version";
			// 
			// _licenseLabel
			// 
			this._licenseLabel.AutoSize = true;
			this._licenseLabel.Location = new System.Drawing.Point(12, 102);
			this._licenseLabel.Name = "_licenseLabel";
			this._licenseLabel.Size = new System.Drawing.Size(46, 13);
			this._licenseLabel.TabIndex = 4;
			this._licenseLabel.Text = "License:";
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this._okButton.Location = new System.Drawing.Point(214, 9);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(75, 23);
			this._okButton.TabIndex = 2;
			this._okButton.Text = "&OK";
			// 
			// nuGenDialogBlock1
			// 
			this.nuGenDialogBlock1.Controls.Add(this._okButton);
			this.nuGenDialogBlock1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.nuGenDialogBlock1.Location = new System.Drawing.Point(0, 310);
			this.nuGenDialogBlock1.Name = "nuGenDialogBlock1";
			this.nuGenDialogBlock1.Size = new System.Drawing.Size(294, 40);
			this.nuGenDialogBlock1.TabIndex = 0;
			// 
			// _licenseTextBox
			// 
			this._licenseTextBox.BackColor = System.Drawing.SystemColors.Window;
			this._licenseTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
			this._licenseTextBox.Location = new System.Drawing.Point(12, 118);
			this._licenseTextBox.Multiline = true;
			this._licenseTextBox.Name = "_licenseTextBox";
			this._licenseTextBox.ReadOnly = true;
			this._licenseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._licenseTextBox.Size = new System.Drawing.Size(270, 181);
			this._licenseTextBox.TabIndex = 5;
			// 
			// AboutForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.CancelButton = this._okButton;
			this.ClientSize = new System.Drawing.Size(294, 350);
			this.Controls.Add(this._licenseTextBox);
			this.Controls.Add(this._licenseLabel);
			this.Controls.Add(this._versionLabel);
			this.Controls.Add(this._captionLabel);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nuGenDialogBlock1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About NuGenVisiCalc";
			this.nuGenDialogBlock1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Genetibase.Controls.NuGenDialogBlock nuGenDialogBlock1;
		private System.Windows.Forms.Button _okButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label _captionLabel;
		private System.Windows.Forms.Label _versionLabel;
		private System.Windows.Forms.Label _licenseLabel;
		private System.Windows.Forms.TextBox _licenseTextBox;
	}
}