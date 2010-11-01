namespace Dile.UI
{
	partial class NuGenThreadsPanel
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
            Janus.Windows.GridEX.GridEXLayout threadsGrid_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenThreadsPanel));
            this.threadsGrid = new Dile.Controls.NuGenCustomDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.threadsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // threadsGrid
            // 
            this.threadsGrid.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.threadsGrid.ColumnAutoResize = true;
            threadsGrid_DesignTimeLayout.LayoutString = resources.GetString("threadsGrid_DesignTimeLayout.LayoutString");
            this.threadsGrid.DesignTimeLayout = threadsGrid_DesignTimeLayout;
            this.threadsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.threadsGrid.GroupByBoxVisible = false;
            this.threadsGrid.Location = new System.Drawing.Point(0, 0);
            this.threadsGrid.Name = "threadsGrid";
            this.threadsGrid.Size = new System.Drawing.Size(381, 273);
            this.threadsGrid.TabIndex = 1;
            this.threadsGrid.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.threadsGrid.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.threadsGrid_CellDoubleClick);
            // 
            // NuGenThreadsPanel
            // 
            this.Controls.Add(this.threadsGrid);
            this.Name = "NuGenThreadsPanel";
            this.Size = new System.Drawing.Size(381, 273);
            ((System.ComponentModel.ISupportInitialize)(this.threadsGrid)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.NuGenCustomDataGridView threadsGrid;
        //private Janus.Windows.GridEX.GridEXColumn idColumn;
        //private Janus.Windows.GridEX.GridEXColumn threadNameColumn;
        //private Janus.Windows.GridEX.GridEXColumn appDomainColumn;

	}
}