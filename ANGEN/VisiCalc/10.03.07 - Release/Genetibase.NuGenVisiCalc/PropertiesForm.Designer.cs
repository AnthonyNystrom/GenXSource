namespace Genetibase.NuGenVisiCalc
{
	partial class PropertiesForm
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
			this._propertyGrid = new Genetibase.SmoothControls.NuGenSmoothPropertyGrid();
			this._bkgndPanel = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this._snapUI = new Genetibase.Shared.Controls.NuGenUISnap();
			this._bkgndPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// _propertyGrid
			// 
			this._propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this._propertyGrid.Location = new System.Drawing.Point(0, 0);
			this._propertyGrid.Name = "_propertyGrid";
			this._propertyGrid.Size = new System.Drawing.Size(232, 294);
			this._propertyGrid.TabIndex = 0;
			// 
			// _bkgndPanel
			// 
			this._bkgndPanel.Controls.Add(this._propertyGrid);
			this._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bkgndPanel.DrawBorder = false;
			this._bkgndPanel.Location = new System.Drawing.Point(0, 0);
			this._bkgndPanel.Name = "_bkgndPanel";
			this._bkgndPanel.Size = new System.Drawing.Size(232, 294);
			// 
			// _snapUI
			// 
			this._snapUI.HostForm = this;
			// 
			// PropertiesForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(232, 294);
			this.Controls.Add(this._bkgndPanel);
			this.Name = "PropertiesForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Properties";
			this.Controls.SetChildIndex(this._bkgndPanel, 0);
			this._bkgndPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothPropertyGrid _propertyGrid;
		private Genetibase.SmoothControls.NuGenSmoothPanel _bkgndPanel;
		private Genetibase.Shared.Controls.NuGenUISnap _snapUI;
	}
}