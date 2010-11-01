namespace Genetibase.ApplicationBlocks.Demo
{
	partial class MainForm
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
			this.openFileSelector = new Genetibase.Shared.Controls.NuGenOpenFileSelector();
			this.goButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// openFileSelector
			// 
			this.openFileSelector.Filter = "JPEG| *.jpg;*.jpeg";
			this.openFileSelector.FilterIndex = 1;
			this.openFileSelector.Guid = "941b5c54-6de3-4bbf-9b98-7e8cae791db2";
			this.openFileSelector.Location = new System.Drawing.Point(12, 12);
			this.openFileSelector.Name = "openFileSelector";
			this.openFileSelector.Size = new System.Drawing.Size(250, 25);
			this.openFileSelector.TabIndex = 0;
			this.openFileSelector.Title = "";
			// 
			// goButton
			// 
			this.goButton.Location = new System.Drawing.Point(268, 13);
			this.goButton.Name = "goButton";
			this.goButton.Size = new System.Drawing.Size(75, 23);
			this.goButton.TabIndex = 1;
			this.goButton.Text = "&Go";
			this.goButton.UseVisualStyleBackColor = true;
			this.goButton.Click += new System.EventHandler(this.goButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(356, 49);
			this.Controls.Add(this.goButton);
			this.Controls.Add(this.openFileSelector);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.Shared.Controls.NuGenOpenFileSelector openFileSelector;
		private System.Windows.Forms.Button goButton;
	}
}