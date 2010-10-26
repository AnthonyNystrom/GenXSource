namespace FakeIconEditor
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
			this._colorPanel = new FakeIconEditor.ColorPanel();
			this.SuspendLayout();
			// 
			// _colorPanel
			// 
			this._colorPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._colorPanel.Location = new System.Drawing.Point(0, 0);
			this._colorPanel.Name = "_colorPanel";
			this._colorPanel.SelectedColor = System.Drawing.Color.Transparent;
			this._colorPanel.Size = new System.Drawing.Size(292, 266);
			this._colorPanel.TabIndex = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this._colorPanel);
			this.Location = new System.Drawing.Point(250, 250);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "IconEditor";
			this.ResumeLayout(false);

		}

		#endregion

		private ColorPanel _colorPanel;
	}
}
