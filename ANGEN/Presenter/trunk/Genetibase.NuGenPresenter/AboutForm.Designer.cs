namespace Genetibase.NuGenPresenter
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this._bkgndPanel = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this.nuGenLabel2 = new Genetibase.Shared.Controls.NuGenLabel();
			this._okButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this.nuGenLabel1 = new Genetibase.Shared.Controls.NuGenLabel();
			this._captionLabel = new Genetibase.Shared.Controls.NuGenLabel();
			this._siteLabel = new Genetibase.Shared.Controls.NuGenLinkLabel();
			this._bkgndPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _bkgndPanel
			// 
			this._bkgndPanel.Controls.Add(this.nuGenLabel2);
			this._bkgndPanel.Controls.Add(this._okButton);
			this._bkgndPanel.Controls.Add(this.nuGenLabel1);
			this._bkgndPanel.Controls.Add(this._captionLabel);
			this._bkgndPanel.Controls.Add(this._siteLabel);
			this._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bkgndPanel.DrawBorder = false;
			this._bkgndPanel.Location = new System.Drawing.Point(0, 0);
			this._bkgndPanel.Name = "_bkgndPanel";
			this._bkgndPanel.Size = new System.Drawing.Size(302, 116);
			this._bkgndPanel.TabIndex = 0;
			// 
			// nuGenLabel2
			// 
			this.nuGenLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.nuGenLabel2.Location = new System.Drawing.Point(14, 38);
			this.nuGenLabel2.Name = "nuGenLabel2";
			this.nuGenLabel2.Size = new System.Drawing.Size(114, 16);
			this.nuGenLabel2.TabIndex = 4;
			this.nuGenLabel2.Text = "by Alex Nesterov";
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(205, 12);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(85, 30);
			this._okButton.TabIndex = 3;
			this._okButton.Text = "&Ok";
			this._okButton.UseVisualStyleBackColor = false;
			// 
			// nuGenLabel1
			// 
			this.nuGenLabel1.Location = new System.Drawing.Point(12, 66);
			this.nuGenLabel1.Name = "nuGenLabel1";
			this.nuGenLabel1.Size = new System.Drawing.Size(182, 13);
			this.nuGenLabel1.TabIndex = 2;
			this.nuGenLabel1.Text = "Copyright © 2007 Genetibase, Inc.";
			// 
			// _captionLabel
			// 
			this._captionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this._captionLabel.Location = new System.Drawing.Point(12, 12);
			this._captionLabel.Name = "_captionLabel";
			this._captionLabel.Size = new System.Drawing.Size(180, 26);
			this._captionLabel.TabIndex = 1;
			this._captionLabel.Text = "NuGenPresenter";
			// 
			// _siteLabel
			// 
			this._siteLabel.Location = new System.Drawing.Point(12, 85);
			this._siteLabel.Name = "_siteLabel";
			this._siteLabel.Size = new System.Drawing.Size(147, 13);
			this._siteLabel.TabIndex = 0;
			this._siteLabel.Text = "http://www.genetibase.com/";
			this._siteLabel.Click += new System.EventHandler(this._siteLabel_Click);
			// 
			// AboutForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(302, 116);
			this.Controls.Add(this._bkgndPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this._bkgndPanel.ResumeLayout(false);
			this._bkgndPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothPanel _bkgndPanel;
		private Genetibase.Shared.Controls.NuGenLabel nuGenLabel2;
		private Genetibase.SmoothControls.NuGenSmoothButton _okButton;
		private Genetibase.Shared.Controls.NuGenLabel nuGenLabel1;
		private Genetibase.Shared.Controls.NuGenLabel _captionLabel;
		private Genetibase.Shared.Controls.NuGenLinkLabel _siteLabel;
	}
}