namespace Dile.UI
{
	partial class NuGenObjectsPanel
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
            Janus.Windows.GridEX.GridEXLayout objectsGrid_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenObjectsPanel));
            this.objectsGrid = new Dile.Controls.NuGenCustomDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.objectsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // objectsGrid
            // 
            this.objectsGrid.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.objectsGrid.ColumnAutoResize = true;
            objectsGrid_DesignTimeLayout.LayoutString = resources.GetString("objectsGrid_DesignTimeLayout.LayoutString");
            this.objectsGrid.DesignTimeLayout = objectsGrid_DesignTimeLayout;
            this.objectsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectsGrid.GroupByBoxVisible = false;
            this.objectsGrid.Location = new System.Drawing.Point(0, 0);
            this.objectsGrid.Name = "objectsGrid";
            this.objectsGrid.Size = new System.Drawing.Size(446, 158);
            this.objectsGrid.TabIndex = 7;
            this.objectsGrid.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.objectsGrid.CellEdited += new Janus.Windows.GridEX.ColumnActionEventHandler(this.objectsGrid_CellEndEdit);
            this.objectsGrid.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.objectsGrid_CellDoubleClick);
            // 
            // NuGenObjectsPanel
            // 
            this.Controls.Add(this.objectsGrid);
            this.Name = "NuGenObjectsPanel";
            this.Size = new System.Drawing.Size(446, 158);
            ((System.ComponentModel.ISupportInitialize)(this.objectsGrid)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.NuGenCustomDataGridView objectsGrid;
        //private Janus.Windows.GridEX.GridEXColumn valueNameColumn;
        //private Janus.Windows.GridEX.GridEXColumn valueColumn;
	}
}