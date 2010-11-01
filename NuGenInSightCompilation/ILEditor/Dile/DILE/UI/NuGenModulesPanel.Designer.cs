namespace Dile.UI
{
	partial class NuGenModulesPanel
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
            Janus.Windows.GridEX.GridEXLayout modulesGrid_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenModulesPanel));
            this.modulesGrid = new Dile.Controls.NuGenCustomDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.modulesGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // modulesGrid
            // 
            this.modulesGrid.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.modulesGrid.ColumnAutoResize = true;
            this.modulesGrid.ColumnAutoSizeMode = Janus.Windows.GridEX.ColumnAutoSizeMode.AllCells;
            modulesGrid_DesignTimeLayout.LayoutString = resources.GetString("modulesGrid_DesignTimeLayout.LayoutString");
            this.modulesGrid.DesignTimeLayout = modulesGrid_DesignTimeLayout;
            this.modulesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.modulesGrid.GroupByBoxVisible = false;
            this.modulesGrid.Location = new System.Drawing.Point(0, 0);
            this.modulesGrid.Name = "modulesGrid";
            this.modulesGrid.Size = new System.Drawing.Size(765, 273);
            this.modulesGrid.TabIndex = 0;
            this.modulesGrid.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.modulesGrid.DoubleClick += new System.EventHandler(this.modulesGrid_CellDoubleClick);
            // 
            // NuGenModulesPanel
            // 
            this.Controls.Add(this.modulesGrid);
            this.Name = "NuGenModulesPanel";
            this.Size = new System.Drawing.Size(765, 273);
            ((System.ComponentModel.ISupportInitialize)(this.modulesGrid)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.NuGenCustomDataGridView modulesGrid;
        //private Janus.Windows.GridEX.GridEXColumn tokenColumn;
        //private Janus.Windows.GridEX.GridEXColumn baseAddressColumn;
        //private Janus.Windows.GridEX.GridEXColumn sizeColumn;
        //private Janus.Windows.GridEX.GridEXColumn isDynamicColumn;
        //private Janus.Windows.GridEX.GridEXColumn isInMemoryColumn;
        //private Janus.Windows.GridEX.GridEXColumn fileNameColumn;
        //private Janus.Windows.GridEX.GridEXColumn nameColumn;
        //private Janus.Windows.GridEX.GridEXColumn assemblyNameColumn;
        //private Janus.Windows.GridEX.GridEXColumn appDomainNameColumn;

	}
}