namespace NavigationBar
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
			this._navigationBar = new Genetibase.SmoothControls.NuGenSmoothNavigationBar();
			this.nuGenSmoothNavigationPane1 = new Genetibase.SmoothControls.NuGenSmoothNavigationPane();
			this.nuGenSmoothNavigationPane2 = new Genetibase.SmoothControls.NuGenSmoothNavigationPane();
			this.nuGenSmoothNavigationPane3 = new Genetibase.SmoothControls.NuGenSmoothNavigationPane();
			this.nuGenSmoothNavigationPane4 = new Genetibase.SmoothControls.NuGenSmoothNavigationPane();
			this._splitContainer = new Genetibase.SmoothControls.NuGenSmoothSplitContainer();
			this._navigationBar.SuspendLayout();
			this._splitContainer.Panel1.SuspendLayout();
			this._splitContainer.SuspendLayout();
			this.SuspendLayout();
			// 
			// _navigationBar
			// 
			this._navigationBar.Dock = System.Windows.Forms.DockStyle.Fill;
			this._navigationBar.GripDistance = 134;
			this._navigationBar.Location = new System.Drawing.Point(0, 0);
			this._navigationBar.Name = "_navigationBar";
			this._navigationBar.NavigationPanes.Add(this.nuGenSmoothNavigationPane1);
			this._navigationBar.NavigationPanes.Add(this.nuGenSmoothNavigationPane2);
			this._navigationBar.NavigationPanes.Add(this.nuGenSmoothNavigationPane3);
			this._navigationBar.NavigationPanes.Add(this.nuGenSmoothNavigationPane4);
			this._navigationBar.Size = new System.Drawing.Size(174, 266);
			this._navigationBar.TabIndex = 0;
			// 
			// nuGenSmoothNavigationPane1
			// 
			this.nuGenSmoothNavigationPane1.Name = "nuGenSmoothNavigationPane1";
			this.nuGenSmoothNavigationPane1.NavigationButtonImage = ((System.Drawing.Image)(resources.GetObject("nuGenSmoothNavigationPane1.NavigationButtonImage")));
			this.nuGenSmoothNavigationPane1.TabIndex = 2;
			this.nuGenSmoothNavigationPane1.Text = "General";
			// 
			// nuGenSmoothNavigationPane2
			// 
			this.nuGenSmoothNavigationPane2.Name = "nuGenSmoothNavigationPane2";
			this.nuGenSmoothNavigationPane2.NavigationButtonImage = ((System.Drawing.Image)(resources.GetObject("nuGenSmoothNavigationPane2.NavigationButtonImage")));
			this.nuGenSmoothNavigationPane2.TabIndex = 3;
			this.nuGenSmoothNavigationPane2.Text = "Cache";
			// 
			// nuGenSmoothNavigationPane3
			// 
			this.nuGenSmoothNavigationPane3.Name = "nuGenSmoothNavigationPane3";
			this.nuGenSmoothNavigationPane3.NavigationButtonImage = ((System.Drawing.Image)(resources.GetObject("nuGenSmoothNavigationPane3.NavigationButtonImage")));
			this.nuGenSmoothNavigationPane3.TabIndex = 4;
			this.nuGenSmoothNavigationPane3.Text = "Network";
			// 
			// nuGenSmoothNavigationPane4
			// 
			this.nuGenSmoothNavigationPane4.Name = "nuGenSmoothNavigationPane4";
			this.nuGenSmoothNavigationPane4.NavigationButtonImage = ((System.Drawing.Image)(resources.GetObject("nuGenSmoothNavigationPane4.NavigationButtonImage")));
			this.nuGenSmoothNavigationPane4.TabIndex = 5;
			this.nuGenSmoothNavigationPane4.Text = "Advanced";
			// 
			// _splitContainer
			// 
			this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this._splitContainer.Location = new System.Drawing.Point(0, 0);
			this._splitContainer.Name = "_splitContainer";
			// 
			// _splitContainer.Panel1
			// 
			this._splitContainer.Panel1.Controls.Add(this._navigationBar);
			this._splitContainer.Size = new System.Drawing.Size(522, 266);
			this._splitContainer.SplitterDistance = 174;
			this._splitContainer.TabIndex = 1;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(522, 266);
			this.Controls.Add(this._splitContainer);
			this.Name = "MainForm";
			this.Text = "NavigationBar";
			this._navigationBar.ResumeLayout(false);
			this._splitContainer.Panel1.ResumeLayout(false);
			this._splitContainer.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothNavigationBar _navigationBar;
		private Genetibase.SmoothControls.NuGenSmoothSplitContainer _splitContainer;
		private Genetibase.SmoothControls.NuGenSmoothNavigationPane nuGenSmoothNavigationPane1;
		private Genetibase.SmoothControls.NuGenSmoothNavigationPane nuGenSmoothNavigationPane2;
		private Genetibase.SmoothControls.NuGenSmoothNavigationPane nuGenSmoothNavigationPane3;
		private Genetibase.SmoothControls.NuGenSmoothNavigationPane nuGenSmoothNavigationPane4;

	}
}

