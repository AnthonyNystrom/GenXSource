namespace Genetibase.ApplicationBlocks.ImageExport
{
	partial class NuGenOutputBlock
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
			this.directorySelector = new Genetibase.Shared.Controls.NuGenDirectorySelector();
			this.filenameLabel = new System.Windows.Forms.Label();
			this.filenameTextBox = new System.Windows.Forms.TextBox();
			this.destinationLabel = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// directorySelector
			// 
			this.directorySelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.directorySelector.Description = "";
			this.directorySelector.Location = new System.Drawing.Point(70, 3);
			this.directorySelector.Name = "directorySelector";
			this.directorySelector.Size = new System.Drawing.Size(291, 24);
			this.directorySelector.TabIndex = 0;
			// 
			// filenameLabel
			// 
			this.filenameLabel.Location = new System.Drawing.Point(3, 27);
			this.filenameLabel.Name = "filenameLabel";
			this.filenameLabel.Size = new System.Drawing.Size(62, 30);
			this.filenameLabel.TabIndex = 16;
			this.filenameLabel.Text = "Filename";
			this.filenameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// filenameTextBox
			// 
			this.filenameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.filenameTextBox.Location = new System.Drawing.Point(70, 33);
			this.filenameTextBox.Name = "filenameTextBox";
			this.filenameTextBox.Size = new System.Drawing.Size(288, 20);
			this.filenameTextBox.TabIndex = 1;
			// 
			// destinationLabel
			// 
			this.destinationLabel.Location = new System.Drawing.Point(3, 0);
			this.destinationLabel.Name = "destinationLabel";
			this.destinationLabel.Size = new System.Drawing.Size(62, 27);
			this.destinationLabel.TabIndex = 18;
			this.destinationLabel.Text = "Destination";
			this.destinationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// NuGenOutputBlock
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.directorySelector);
			this.Controls.Add(this.filenameLabel);
			this.Controls.Add(this.filenameTextBox);
			this.Controls.Add(this.destinationLabel);
			this.Name = "NuGenOutputBlock";
			this.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.Size = new System.Drawing.Size(366, 58);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Genetibase.Shared.Controls.NuGenDirectorySelector directorySelector;
		private System.Windows.Forms.Label filenameLabel;
		private System.Windows.Forms.TextBox filenameTextBox;
		private System.Windows.Forms.Label destinationLabel;

	}
}
