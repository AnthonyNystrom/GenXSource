namespace MiniBar
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.nuGenGradientPanel1 = new Genetibase.Shared.Controls.NuGenGradientPanel();
			this.nuGenMiniBar1 = new Genetibase.Shared.Controls.NuGenMiniBar();
			this._newButton = new Genetibase.Shared.Controls.NuGenMiniBarButton();
			this._openButton = new Genetibase.Shared.Controls.NuGenMiniBarButton();
			this._saveButton = new Genetibase.Shared.Controls.NuGenMiniBarButton();
			this.nuGenMiniBarSpace1 = new Genetibase.Shared.Controls.NuGenMiniBarSpace();
			this._label = new Genetibase.Shared.Controls.NuGenMiniBarLabel();
			this.nuGenGradientPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// nuGenGradientPanel1
			// 
			this.nuGenGradientPanel1.Controls.Add(this.nuGenMiniBar1);
			this.nuGenGradientPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenGradientPanel1.Location = new System.Drawing.Point(0, 0);
			this.nuGenGradientPanel1.Name = "nuGenGradientPanel1";
			this.nuGenGradientPanel1.Size = new System.Drawing.Size(177, 254);
			this.nuGenGradientPanel1.Watermark = ((System.Drawing.Image)(resources.GetObject("nuGenGradientPanel1.Watermark")));
			this.nuGenGradientPanel1.WatermarkSize = new System.Drawing.Size(100, 100);
			// 
			// nuGenMiniBar1
			// 
			this.nuGenMiniBar1.BackColor = System.Drawing.Color.Transparent;
			this.nuGenMiniBar1.Buttons.AddRange(new Genetibase.Shared.Controls.NuGenMiniBarControl[] {
            this._newButton,
            this._openButton,
            this._saveButton,
            this.nuGenMiniBarSpace1,
            this._label});
			this.nuGenMiniBar1.Dock = System.Windows.Forms.DockStyle.Top;
			this.nuGenMiniBar1.Location = new System.Drawing.Point(0, 0);
			this.nuGenMiniBar1.Name = "nuGenMiniBar1";
			this.nuGenMiniBar1.Size = new System.Drawing.Size(177, 29);
			this.nuGenMiniBar1.TabIndex = 0;
			// 
			// _newButton
			// 
			this._newButton.Glyph = ((System.Drawing.Bitmap)(resources.GetObject("_newButton.Glyph")));
			this._newButton.Height = 13;
			this._newButton.Size = new System.Drawing.Size(13, 13);
			this._newButton.Width = 13;
			this._newButton.X = 0;
			this._newButton.Y = 0;
			// 
			// _openButton
			// 
			this._openButton.Glyph = ((System.Drawing.Bitmap)(resources.GetObject("_openButton.Glyph")));
			this._openButton.Height = 13;
			this._openButton.Size = new System.Drawing.Size(13, 13);
			this._openButton.Width = 13;
			this._openButton.X = 13;
			this._openButton.Y = 0;
			// 
			// _saveButton
			// 
			this._saveButton.Glyph = ((System.Drawing.Bitmap)(resources.GetObject("_saveButton.Glyph")));
			this._saveButton.Height = 13;
			this._saveButton.Size = new System.Drawing.Size(13, 13);
			this._saveButton.Width = 13;
			this._saveButton.X = 26;
			this._saveButton.Y = 0;
			// 
			// nuGenMiniBarSpace1
			// 
			this.nuGenMiniBarSpace1.Height = 13;
			this.nuGenMiniBarSpace1.Size = new System.Drawing.Size(40, 13);
			this.nuGenMiniBarSpace1.Width = 40;
			this.nuGenMiniBarSpace1.X = 39;
			this.nuGenMiniBarSpace1.Y = 0;
			// 
			// _label
			// 
			this._label.Height = 13;
			this._label.Size = new System.Drawing.Size(100, 13);
			this._label.Text = "Genetibase, Inc.";
			this._label.Width = 100;
			this._label.X = 79;
			this._label.Y = 0;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(177, 254);
			this.Controls.Add(this.nuGenGradientPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.nuGenGradientPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.Shared.Controls.NuGenGradientPanel nuGenGradientPanel1;
		private Genetibase.Shared.Controls.NuGenMiniBar nuGenMiniBar1;
		private Genetibase.Shared.Controls.NuGenMiniBarButton _newButton;
		private Genetibase.Shared.Controls.NuGenMiniBarButton _openButton;
		private Genetibase.Shared.Controls.NuGenMiniBarButton _saveButton;
		private Genetibase.Shared.Controls.NuGenMiniBarSpace nuGenMiniBarSpace1;
		private Genetibase.Shared.Controls.NuGenMiniBarLabel _label;
	}
}

