namespace Genetibase.NuGenVisiCalc
{
	partial class OutputForm
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
			this._logListBox = new Genetibase.SmoothControls.NuGenSmoothListBox();
			this._contextMenuStrip = new Genetibase.SmoothControls.NuGenSmoothContextMenuStrip();
			this._clearButton = new System.Windows.Forms.ToolStripMenuItem();
			this._contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// _logListBox
			// 
			this._logListBox.ContextMenuStrip = this._contextMenuStrip;
			this._logListBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this._logListBox.FormattingEnabled = true;
			this._logListBox.Location = new System.Drawing.Point(0, 0);
			this._logListBox.Name = "_logListBox";
			this._logListBox.Size = new System.Drawing.Size(592, 124);
			this._logListBox.TabIndex = 1;
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._clearButton});
			this._contextMenuStrip.Name = "_contextMenuStrip";
			this._contextMenuStrip.Size = new System.Drawing.Size(111, 26);
			// 
			// _clearButton
			// 
			this._clearButton.Name = "_clearButton";
			this._clearButton.Size = new System.Drawing.Size(152, 22);
			this._clearButton.Text = "&Clear";
			this._clearButton.Click += new System.EventHandler(this._clearButton_Click);
			// 
			// OutputForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(592, 124);
			this.Controls.Add(this._logListBox);
			this.Name = "OutputForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Output";
			this.Controls.SetChildIndex(this._logListBox, 0);
			this._contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothListBox _logListBox;
		private Genetibase.SmoothControls.NuGenSmoothContextMenuStrip _contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem _clearButton;

	}
}