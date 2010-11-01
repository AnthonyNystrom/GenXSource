namespace Dile.UI
{
	partial class NuGenBreakpointsPanel
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
            Janus.Windows.GridEX.GridEXLayout breakpointsGrid_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenBreakpointsPanel));
            this.breakpointsGrid = new Dile.Controls.NuGenCustomDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.breakpointsGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // breakpointsGrid
            // 
            this.breakpointsGrid.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.breakpointsGrid.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.breakpointsGrid.ColumnAutoResize = true;
            breakpointsGrid_DesignTimeLayout.LayoutString = resources.GetString("breakpointsGrid_DesignTimeLayout.LayoutString");
            this.breakpointsGrid.DesignTimeLayout = breakpointsGrid_DesignTimeLayout;
            this.breakpointsGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.breakpointsGrid.GroupByBoxVisible = false;
            this.breakpointsGrid.Location = new System.Drawing.Point(0, 0);
            this.breakpointsGrid.Name = "breakpointsGrid";
            this.breakpointsGrid.Size = new System.Drawing.Size(292, 273);
            this.breakpointsGrid.TabIndex = 0;
            this.breakpointsGrid.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.breakpointsGrid.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.breakpointsGrid_CellDoubleClick);
            this.breakpointsGrid.Click += new System.EventHandler(this.breakpointsGrid_CellContentClick);
            // 
            // NuGenBreakpointsPanel
            // 
            this.Controls.Add(this.breakpointsGrid);
            this.Name = "NuGenBreakpointsPanel";
            this.Size = new System.Drawing.Size(292, 273);
            ((System.ComponentModel.ISupportInitialize)(this.breakpointsGrid)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.NuGenCustomDataGridView breakpointsGrid;
        //private Janus.Windows.GridEX.GridEXColumn dataGridViewCheckBoxColumn1;
        //private Janus.Windows.GridEX.GridEXColumn dataGridViewTextBoxColumn1;
        //private Janus.Windows.GridEX.GridEXColumn dataGridViewTextBoxColumn2;
	}
}