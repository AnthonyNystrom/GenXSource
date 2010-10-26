namespace Genetibase.NuGenVisiCalc
{
	partial class SaveChangesForm
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
			this._okButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this._cancelButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this._labelQuestion = new Genetibase.Shared.Controls.NuGenLabel();
			this._documents = new Genetibase.Shared.Controls.NuGenTreeView();
			this._abortButton = new Genetibase.SmoothControls.NuGenSmoothButton();
			this._bkgndPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _bkgndPanel
			// 
			this._bkgndPanel.Controls.Add(this._abortButton);
			this._bkgndPanel.Controls.Add(this._documents);
			this._bkgndPanel.Controls.Add(this._labelQuestion);
			this._bkgndPanel.Controls.Add(this._cancelButton);
			this._bkgndPanel.Controls.Add(this._okButton);
			this._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bkgndPanel.DrawBorder = false;
			this._bkgndPanel.ExtendedBackground = true;
			this._bkgndPanel.Location = new System.Drawing.Point(0, 0);
			this._bkgndPanel.Name = "_bkgndPanel";
			this._bkgndPanel.Size = new System.Drawing.Size(418, 325);
			// 
			// _okButton
			// 
			this._okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this._okButton.Location = new System.Drawing.Point(220, 283);
			this._okButton.Name = "_okButton";
			this._okButton.Size = new System.Drawing.Size(90, 30);
			this._okButton.TabIndex = 0;
			this._okButton.Text = "&Ok";
			this._okButton.UseVisualStyleBackColor = false;
			// 
			// _cancelButton
			// 
			this._cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this._cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._cancelButton.Location = new System.Drawing.Point(316, 283);
			this._cancelButton.Name = "_cancelButton";
			this._cancelButton.Size = new System.Drawing.Size(90, 30);
			this._cancelButton.TabIndex = 1;
			this._cancelButton.Text = "&Cancel";
			this._cancelButton.UseVisualStyleBackColor = false;
			// 
			// _labelQuestion
			// 
			this._labelQuestion.Location = new System.Drawing.Point(12, 12);
			this._labelQuestion.Name = "_labelQuestion";
			this._labelQuestion.Size = new System.Drawing.Size(268, 13);
			this._labelQuestion.TabIndex = 3;
			this._labelQuestion.Text = "Which of the following documents should be saved?";
			// 
			// _documents
			// 
			this._documents.CheckBoxes = true;
			this._documents.Location = new System.Drawing.Point(12, 31);
			this._documents.Name = "_documents";
			this._documents.Size = new System.Drawing.Size(394, 238);
			this._documents.TabIndex = 4;
			// 
			// _abortButton
			// 
			this._abortButton.DialogResult = System.Windows.Forms.DialogResult.Abort;
			this._abortButton.Location = new System.Drawing.Point(12, 283);
			this._abortButton.Name = "_abortButton";
			this._abortButton.Size = new System.Drawing.Size(90, 30);
			this._abortButton.TabIndex = 5;
			this._abortButton.Text = "&Don\'t save";
			this._abortButton.UseVisualStyleBackColor = false;
			this._abortButton.Click += new System.EventHandler(this._abortButton_Click);
			// 
			// SaveChangesForm
			// 
			this.AcceptButton = this._okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this._cancelButton;
			this.ClientSize = new System.Drawing.Size(418, 325);
			this.Controls.Add(this._bkgndPanel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SaveChangesForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "NuGenVisiCalc";
			this._bkgndPanel.ResumeLayout(false);
			this._bkgndPanel.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothPanel _bkgndPanel;
		private Genetibase.Shared.Controls.NuGenLabel _labelQuestion;
		private Genetibase.SmoothControls.NuGenSmoothButton _cancelButton;
		private Genetibase.SmoothControls.NuGenSmoothButton _okButton;
		private Genetibase.Shared.Controls.NuGenTreeView _documents;
		private Genetibase.SmoothControls.NuGenSmoothButton _abortButton;
	}
}