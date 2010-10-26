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
			this._copyrightLabel = new System.Windows.Forms.Label();
			this._captionLabel = new System.Windows.Forms.Label();
			this._versionLabel = new System.Windows.Forms.Label();
			this._licenseLabel = new System.Windows.Forms.Label();
			this._licenseTextBox = new System.Windows.Forms.TextBox();
			this.nuGenSmoothPanel1 = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this._okButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this.nuGenSmoothPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _copyrightLabel
			// 
			this._copyrightLabel.AutoSize = true;
			this._copyrightLabel.Location = new System.Drawing.Point(9, 58);
			this._copyrightLabel.Name = "_copyrightLabel";
			this._copyrightLabel.Size = new System.Drawing.Size(205, 13);
			this._copyrightLabel.TabIndex = 1;
			this._copyrightLabel.Text = "Copyright © 2006-2007 Genetibase, Inc.";
			// 
			// _captionLabel
			// 
			this._captionLabel.AutoSize = true;
			this._captionLabel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._captionLabel.ForeColor = System.Drawing.SystemColors.ControlText;
			this._captionLabel.Location = new System.Drawing.Point(8, 9);
			this._captionLabel.Name = "_captionLabel";
			this._captionLabel.Size = new System.Drawing.Size(149, 23);
			this._captionLabel.TabIndex = 2;
			this._captionLabel.Text = "NuGenVisiCalc";
			// 
			// _versionLabel
			// 
			this._versionLabel.AutoSize = true;
			this._versionLabel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this._versionLabel.Location = new System.Drawing.Point(9, 32);
			this._versionLabel.Name = "_versionLabel";
			this._versionLabel.Size = new System.Drawing.Size(49, 13);
			this._versionLabel.TabIndex = 3;
			this._versionLabel.Text = "Version";
			// 
			// _licenseLabel
			// 
			this._licenseLabel.AutoSize = true;
			this._licenseLabel.Location = new System.Drawing.Point(9, 85);
			this._licenseLabel.Name = "_licenseLabel";
			this._licenseLabel.Size = new System.Drawing.Size(46, 13);
			this._licenseLabel.TabIndex = 4;
			this._licenseLabel.Text = "License:";
			// 
			// _licenseTextBox
			// 
			this._licenseTextBox.BackColor = System.Drawing.SystemColors.Window;
			this._licenseTextBox.ForeColor = System.Drawing.SystemColors.ControlText;
			this._licenseTextBox.Location = new System.Drawing.Point(12, 101);
			this._licenseTextBox.Multiline = true;
			this._licenseTextBox.Name = "_licenseTextBox";
			this._licenseTextBox.ReadOnly = true;
			this._licenseTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this._licenseTextBox.Size = new System.Drawing.Size(376, 236);
			this._licenseTextBox.TabIndex = 5;
			// 
			// nuGenSmoothPanel1
			// 
			this.nuGenSmoothPanel1.Controls.Add(this._okButton);
			this.nuGenSmoothPanel1.Controls.Add(this._copyrightLabel);
			this.nuGenSmoothPanel1.Controls.Add(this._licenseTextBox);
			this.nuGenSmoothPanel1.Controls.Add(this._captionLabel);
			this.nuGenSmoothPanel1.Controls.Add(this._licenseLabel);
			this.nuGenSmoothPanel1.Controls.Add(this._versionLabel);
			this.nuGenSmoothPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenSmoothPanel1.DrawBorder = false;
			this.nuGenSmoothPanel1.ExtendedBackground = true;
			this.nuGenSmoothPanel1.Location = new System.Drawing.Point(0, 0);
			this.nuGenSmoothPanel1.Name = "nuGenSmoothPanel1";
			this.nuGenSmoothPanel1.Size = new System.Drawing.Size(400, 384);
			// 
			// _okButton
			// 
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(301, 343);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(87, 30);
			this._okButton.TabIndex = 6;
			this._okButton.Text = "&Ok";
			this._okButton.UseVisualStyleBackColor = false;
			// 
			// AboutForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(400, 384);
			this.Controls.Add(this.nuGenSmoothPanel1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About NuGenVisiCalc";
			this.nuGenSmoothPanel1.ResumeLayout(false);
			this.nuGenSmoothPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label _copyrightLabel;
		private System.Windows.Forms.Label _captionLabel;
		private System.Windows.Forms.Label _versionLabel;
		private System.Windows.Forms.Label _licenseLabel;
		private System.Windows.Forms.TextBox _licenseTextBox;
		private Genetibase.SmoothControls.NuGenSmoothPanel nuGenSmoothPanel1;
		private Genetibase.SmoothControls.NuGenSmoothButton _okButton;
	}
}