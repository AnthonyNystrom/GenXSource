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
			this._blendSelector = new Genetibase.Shared.Controls.NuGenBlendSelector();
			this._snapUI = new Genetibase.Shared.Controls.NuGenUISnap();
			this.SuspendLayout();
			// 
			// _blendSelector
			// 
			this._blendSelector.Dock = System.Windows.Forms.DockStyle.Fill;
			this._blendSelector.Location = new System.Drawing.Point(0, 0);
			this._blendSelector.LowerColor = System.Drawing.Color.White;
			this._blendSelector.Name = "_blendSelector";
			this._blendSelector.Size = new System.Drawing.Size(267, 72);
			this._blendSelector.TabIndex = 0;
			this._blendSelector.UpperColor = System.Drawing.Color.Black;
			this._blendSelector.Value = 0.3F;
			this._blendSelector.ColorChanged += new System.EventHandler(this._blendSelector_ColorChanged);
			this._blendSelector.ColorChanging += new System.EventHandler(this._blendSelector_ColorChanged);
			// 
			// _snapUI
			// 
			this._snapUI.HostForm = this;
			// 
			// BlendForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(267, 72);
			this.Controls.Add(this._blendSelector);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Name = "BlendForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "BlendForm";
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.Shared.Controls.NuGenBlendSelector _blendSelector;
		private Genetibase.Shared.Controls.NuGenUISnap _snapUI;
	}
}
