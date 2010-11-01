namespace Dile.UI
{
	partial class NuGenCallStackPanel
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
			this.callStackView = new Dile.Controls.NuGenCustomListView();
			this.methodColumn = new System.Windows.Forms.ColumnHeader();
			this.SuspendLayout();
			// 
			// callStackView
			// 
			this.callStackView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.methodColumn});
			this.callStackView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.callStackView.FullRowSelect = true;
			this.callStackView.GridLines = true;
			this.callStackView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.callStackView.Location = new System.Drawing.Point(0, 0);
			this.callStackView.MultiSelect = false;
			this.callStackView.Name = "callStackView";
			this.callStackView.ShowGroups = false;
			this.callStackView.ShowItemToolTips = true;
			this.callStackView.Size = new System.Drawing.Size(292, 273);
			this.callStackView.TabIndex = 1;
			this.callStackView.UseCompatibleStateImageBehavior = false;
			this.callStackView.View = System.Windows.Forms.View.Details;
			this.callStackView.DoubleClick += new System.EventHandler(this.callStackView_DoubleClick);
			this.callStackView.Resize += new System.EventHandler(this.callStackView_Resize);
			this.callStackView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.callStackView_KeyDown);
			// 
			// methodColumn
			// 
			this.methodColumn.Text = "Method name";
			this.methodColumn.Width = 287;
			// 
			// CallStackPanel
			// 			
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.callStackView);
			this.Name = "CallStackPanel";
			this.Text = "Call Stack Panel";
			this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.NuGenCustomListView callStackView;
		private System.Windows.Forms.ColumnHeader methodColumn;
	}
}