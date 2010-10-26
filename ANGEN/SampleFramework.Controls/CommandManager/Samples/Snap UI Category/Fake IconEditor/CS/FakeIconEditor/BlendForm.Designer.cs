namespace FakeIconEditor
{
	partial class BlendForm
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
			this.nuGenBlendSelector1 = new Genetibase.Shared.Controls.NuGenBlendSelector();
			this._snapUI = new Genetibase.Shared.Controls.NuGenUISnap();
			this.SuspendLayout();
			// 
			// nuGenBlendSelector1
			// 
			this.nuGenBlendSelector1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenBlendSelector1.Location = new System.Drawing.Point(0, 0);
			this.nuGenBlendSelector1.LowerColor = System.Drawing.Color.White;
			this.nuGenBlendSelector1.Name = "nuGenBlendSelector1";
			this.nuGenBlendSelector1.Size = new System.Drawing.Size(267, 59);
			this.nuGenBlendSelector1.TabIndex = 0;
			this.nuGenBlendSelector1.UpperColor = System.Drawing.Color.Black;
			this.nuGenBlendSelector1.Value = 0.3F;
			// 
			// _snapUI
			// 
			this._snapUI.HostForm = this;
			// 
			// BlendForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(267, 59);
			this.Controls.Add(this.nuGenBlendSelector1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "BlendForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "BlendForm";
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.Shared.Controls.NuGenBlendSelector nuGenBlendSelector1;
		private Genetibase.Shared.Controls.NuGenUISnap _snapUI;
	}
}
