using Janus.Windows.GridEX;
namespace Dile.UI
{
	partial class NuGenAttachProcessDialog
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
            Janus.Windows.GridEX.GridEXLayout managedProcessesGrid_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenAttachProcessDialog));
            this.managedProcessesGrid = new Dile.Controls.NuGenCustomDataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.okButton = new Janus.Windows.EditControls.UIButton();
            this.refreshButton = new Janus.Windows.EditControls.UIButton();
            ((System.ComponentModel.ISupportInitialize)(this.managedProcessesGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // managedProcessesGrid
            // 
            this.managedProcessesGrid.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.managedProcessesGrid.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.managedProcessesGrid.ColumnAutoResize = true;
            managedProcessesGrid_DesignTimeLayout.LayoutString = resources.GetString("managedProcessesGrid_DesignTimeLayout.LayoutString");
            this.managedProcessesGrid.DesignTimeLayout = managedProcessesGrid_DesignTimeLayout;
            this.managedProcessesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.managedProcessesGrid.GroupByBoxVisible = false;
            this.managedProcessesGrid.Location = new System.Drawing.Point(0, 0);
            this.managedProcessesGrid.Name = "managedProcessesGrid";
            this.managedProcessesGrid.Size = new System.Drawing.Size(689, 266);
            this.managedProcessesGrid.TabIndex = 6;
            this.managedProcessesGrid.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.managedProcessesGrid.DoubleClick += new System.EventHandler(this.managedProcessesGrid_CellDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.uiButton1);
            this.panel1.Controls.Add(this.okButton);
            this.panel1.Controls.Add(this.refreshButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 266);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(689, 35);
            this.panel1.TabIndex = 7;
            // 
            // uiButton1
            // 
            this.uiButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.uiButton1.Location = new System.Drawing.Point(609, 5);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(75, 23);
            this.uiButton1.TabIndex = 10;
            this.uiButton1.Text = "Cancel";
            this.uiButton1.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(528, 5);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 10;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(21, 5);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 9;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // NuGenAttachProcessDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 301);
            this.Controls.Add(this.managedProcessesGrid);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NuGenAttachProcessDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Attach to process";
            this.Load += new System.EventHandler(this.AttachProcessDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.managedProcessesGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private Dile.Controls.NuGenCustomDataGridView managedProcessesGrid;
        private System.Windows.Forms.Panel panel1;
        //private Janus.Windows.GridEX.GridEXColumn idColumn;
        //private Janus.Windows.GridEX.GridEXColumn nameColumn;
        //private Janus.Windows.GridEX.GridEXColumn FrameworkVersion;
        private Janus.Windows.EditControls.UIButton refreshButton;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.EditControls.UIButton okButton;
	}
}