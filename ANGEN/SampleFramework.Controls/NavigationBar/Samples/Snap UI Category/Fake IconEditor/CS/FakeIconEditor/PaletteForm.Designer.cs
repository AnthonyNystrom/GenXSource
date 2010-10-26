namespace FakeIconEditor
{
	partial class PaletteForm
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
			this._colorHistory = new Genetibase.Shared.Controls.NuGenColorHistory();
			this._colorSelector = new Genetibase.Shared.Controls.NuGenHSVPlainSelector();
			this._snapUI = new Genetibase.Shared.Controls.NuGenUISnap();
			this.SuspendLayout();
			// 
			// _colorHistory
			// 
			this._colorHistory.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._colorHistory.Location = new System.Drawing.Point(0, 188);
			this._colorHistory.Name = "_colorHistory";
			this._colorHistory.Size = new System.Drawing.Size(191, 37);
			this._colorHistory.TabIndex = 0;
			this._colorHistory.Text = "nuGenColorHistory1";
			// 
			// _colorSelector
			// 
			this._colorSelector.Dock = System.Windows.Forms.DockStyle.Fill;
			this._colorSelector.Location = new System.Drawing.Point(0, 0);
			this._colorSelector.Name = "_colorSelector";
			this._colorSelector.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this._colorSelector.Size = new System.Drawing.Size(191, 188);
			this._colorSelector.TabIndex = 1;
			this._colorSelector.Text = "nuGenHSVPlainSelector1";
			this._colorSelector.MouseClick += new System.Windows.Forms.MouseEventHandler(this._colorSelector_MouseClick);
			this._colorSelector.SelectedColorChanged += new System.EventHandler(this._colorSelector_SelectedColorChanged);
			// 
			// _snapUI
			// 
			this._snapUI.HostForm = this;
			// 
			// PaletteForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(191, 225);
			this.Controls.Add(this._colorSelector);
			this.Controls.Add(this._colorHistory);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "PaletteForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Palette";
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.Shared.Controls.NuGenColorHistory _colorHistory;
		private Genetibase.Shared.Controls.NuGenHSVPlainSelector _colorSelector;
		private Genetibase.Shared.Controls.NuGenUISnap _snapUI;
	}
}