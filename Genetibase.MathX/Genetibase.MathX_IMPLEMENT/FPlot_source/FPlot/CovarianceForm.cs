using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace FPlot
{
	/// <summary>
	/// Summary description for CovarianceForm.
	/// </summary>
	public class CovarianceForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button ok;
		private SourceGrid2.Grid grid;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public CovarianceForm()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(CovarianceForm));
			this.ok = new System.Windows.Forms.Button();
			this.grid = new SourceGrid2.Grid();
			this.SuspendLayout();
			// 
			// ok
			// 
			this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ok.Location = new System.Drawing.Point(8, 260);
			this.ok.Name = "ok";
			this.ok.TabIndex = 1;
			this.ok.Text = "Ok";
			this.ok.Click += new System.EventHandler(this.okClick);
			// 
			// grid
			// 
			this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.grid.AutoSizeMinHeight = 10;
			this.grid.AutoSizeMinWidth = 10;
			this.grid.AutoStretchColumnsToFitWidth = false;
			this.grid.AutoStretchRowsToFitHeight = false;
			this.grid.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None;
			this.grid.CustomSort = false;
			this.grid.GridToolTipActive = true;
			this.grid.Location = new System.Drawing.Point(0, 0);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(312, 248);
			this.grid.SpecialKeys = SourceGrid2.GridSpecialKeys.Default;
			this.grid.TabIndex = 2;
			// 
			// CovarianceForm
			// 
			this.AcceptButton = this.ok;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(312, 291);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.ok);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(200, 200);
			this.Name = "CovarianceForm";
			this.ShowInTaskbar = false;
			this.Text = "Covariance Matrix";
			this.ResumeLayout(false);

		}
		#endregion

		public void Reset(float[,] covar) {
			int x, y, n = covar.GetLength(0), m = covar.GetLength(1);
			grid.RowsCount = m+1;
			grid.ColumnsCount = n+1;
			grid[0, 0] = new SourceGrid2.Cells.Real.Header();
			for (x = 0; x < n; x++) {
				grid[0, x+1] = new  SourceGrid2.Cells.Real.ColumnHeader(x+1);
			}
			for (y = 0; y < m; y++) {
				grid[y+1, 0] = new SourceGrid2.Cells.Real.RowHeader(y+1);
			}
			for (x = 0; x < n; x++) {
				for (y = 0; y < m; y++) {
					grid[x+1, y+1] = new SourceGrid2.Cells.Real.Cell(covar[x, y], typeof(double));
					grid[x+1, y+1].DataModel.EnableEdit = false;
				}
			}
			grid.AutoSize();
		}

		private void okClick(object sender, System.EventArgs e) {
			this.Hide();
		}

	}
}
