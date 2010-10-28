using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using FPlotLibrary;

namespace FPlot
{
	/// <summary>
	/// Summary description for ParForm.
	/// </summary>
	public class ParForm : System.Windows.Forms.Form {
		private System.Windows.Forms.Button ok;
		private System.Windows.Forms.Button cancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private SourceGrid2.Grid grid;
		private FunctionItem item;

		public ParForm() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing ) {
			if( disposing ) {
				if(components != null) {
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
		private void InitializeComponent() {
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ParForm));
			this.ok = new System.Windows.Forms.Button();
			this.cancel = new System.Windows.Forms.Button();
			this.grid = new SourceGrid2.Grid();
			this.SuspendLayout();
			// 
			// ok
			// 
			this.ok.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.ok.Location = new System.Drawing.Point(8, 261);
			this.ok.Name = "ok";
			this.ok.Size = new System.Drawing.Size(72, 23);
			this.ok.TabIndex = 1;
			this.ok.Text = "Ok";
			this.ok.Click += new System.EventHandler(this.okClick);
			// 
			// cancel
			// 
			this.cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cancel.Location = new System.Drawing.Point(88, 261);
			this.cancel.Name = "cancel";
			this.cancel.Size = new System.Drawing.Size(72, 23);
			this.cancel.TabIndex = 3;
			this.cancel.Text = "Cancel";
			this.cancel.Click += new System.EventHandler(this.cancelClick);
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
			this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.grid.ContextMenuStyle = SourceGrid2.ContextMenuStyle.None;
			this.grid.CustomSort = false;
			this.grid.GridToolTipActive = true;
			this.grid.Location = new System.Drawing.Point(0, 0);
			this.grid.Name = "grid";
			this.grid.Size = new System.Drawing.Size(168, 256);
			this.grid.SpecialKeys = SourceGrid2.GridSpecialKeys.Default;
			this.grid.TabIndex = 4;
			// 
			// ParForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(168, 288);
			this.Controls.Add(this.grid);
			this.Controls.Add(this.cancel);
			this.Controls.Add(this.ok);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "ParForm";
			this.ShowInTaskbar = false;
			this.Text = "ParForm";
			this.ResumeLayout(false);

		}
		#endregion

		public void Reset(FunctionItem item) {
			this.item = item;


			grid.ColumnsCount = 2;
			grid.RowsCount = item.p.Length + 1;
			grid[0,0] = new SourceGrid2.Cells.Real.Header("n");
			grid[0,1] = new SourceGrid2.Cells.Real.ColumnHeader("p[n]");
			for (int r = 0; r < item.p.Length; r++) {
				grid[r+1,0] = new SourceGrid2.Cells.Real.RowHeader(r);
				grid[r+1,1] = new SourceGrid2.Cells.Real.Cell(item.p[r], typeof(double));
			}

			grid.Columns[0].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			grid.Columns[1].AutoSizeMode = SourceGrid2.AutoSizeMode.MinimumSize;
			grid.AutoSize();
		}

		public void Apply() {
			for (int r = 0; r < item.p.Length; r++) {
				item.p[r] = (double)grid[r+1, 1].Value;
			}
		}

		private void okClick(object sender, System.EventArgs e) {
			Apply();
			this.Hide();
		}

		private void cancelClick(object sender, System.EventArgs e) {
			this.Hide();
		}

	}
}
