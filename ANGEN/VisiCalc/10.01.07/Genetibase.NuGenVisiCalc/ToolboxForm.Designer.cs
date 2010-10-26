namespace Genetibase.NuGenVisiCalc
{
	partial class ToolboxForm
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
			this.nuGenSmoothPanel1 = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this._opTreeView = new Genetibase.Shared.Controls.NuGenTreeView();
			this.nuGenSmoothPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// nuGenSmoothPanel1
			// 
			this.nuGenSmoothPanel1.Controls.Add(this._opTreeView);
			this.nuGenSmoothPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.nuGenSmoothPanel1.DrawBorder = false;
			this.nuGenSmoothPanel1.Location = new System.Drawing.Point(0, 0);
			this.nuGenSmoothPanel1.Name = "nuGenSmoothPanel1";
			this.nuGenSmoothPanel1.Size = new System.Drawing.Size(232, 294);
			// 
			// _opTreeView
			// 
			this._opTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this._opTreeView.Location = new System.Drawing.Point(0, 0);
			this._opTreeView.Name = "_opTreeView";
			this._opTreeView.Size = new System.Drawing.Size(232, 294);
			this._opTreeView.TabIndex = 2;
			this._opTreeView.MouseMove += new System.Windows.Forms.MouseEventHandler(this._opTreeView_MouseMove);
			// 
			// ToolboxForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(232, 294);
			this.Controls.Add(this.nuGenSmoothPanel1);
			this.Name = "ToolboxForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Toolbox";
			this.Controls.SetChildIndex(this.nuGenSmoothPanel1, 0);
			this.nuGenSmoothPanel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothPanel nuGenSmoothPanel1;
		private Genetibase.Shared.Controls.NuGenTreeView _opTreeView;
	}
}