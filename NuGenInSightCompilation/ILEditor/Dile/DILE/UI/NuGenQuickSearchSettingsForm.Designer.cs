namespace Dile.UI
{
	partial class NuGenQuickSearchSettingsForm
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
			this.panel = new System.Windows.Forms.Panel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.assembliesListBox = new System.Windows.Forms.ListBox();
			this.searchOptionsListBox = new System.Windows.Forms.CheckedListBox();
			this.panel.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel
			// 
			this.panel.Controls.Add(this.cancelButton);
			this.panel.Controls.Add(this.okButton);
			this.panel.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel.Location = new System.Drawing.Point(0, 330);
			this.panel.Name = "panel";
			this.panel.Size = new System.Drawing.Size(514, 32);
			this.panel.TabIndex = 3;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(427, 3);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "&Cancel";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(336, 3);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 0;
			this.okButton.Text = "&OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// assembliesListBox
			// 
			this.assembliesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.assembliesListBox.FormattingEnabled = true;
			this.assembliesListBox.Location = new System.Drawing.Point(0, 8);
			this.assembliesListBox.Name = "assembliesListBox";
			this.assembliesListBox.Size = new System.Drawing.Size(326, 316);
			this.assembliesListBox.TabIndex = 4;
			this.assembliesListBox.SelectedIndexChanged += new System.EventHandler(this.assembliesListBox_SelectedIndexChanged);
			// 
			// searchOptionsListBox
			// 
			this.searchOptionsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.searchOptionsListBox.CheckOnClick = true;
			this.searchOptionsListBox.FormattingEnabled = true;
			this.searchOptionsListBox.Location = new System.Drawing.Point(332, 8);
			this.searchOptionsListBox.Name = "searchOptionsListBox";
			this.searchOptionsListBox.Size = new System.Drawing.Size(182, 319);
			this.searchOptionsListBox.TabIndex = 5;
			this.searchOptionsListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.searchOptionsListBox_ItemCheck);
			// 
			// QuickSearchSettingsForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(514, 362);
			this.Controls.Add(this.searchOptionsListBox);
			this.Controls.Add(this.assembliesListBox);
			this.Controls.Add(this.panel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "QuickSearchSettingsForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Quick Search Settings";
			this.Load += new System.EventHandler(this.QuickFinderSettingsForm_Load);
			this.panel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.ListBox assembliesListBox;
		private System.Windows.Forms.CheckedListBox searchOptionsListBox;
	}
}