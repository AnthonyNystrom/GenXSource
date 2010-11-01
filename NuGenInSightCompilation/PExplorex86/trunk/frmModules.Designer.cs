namespace Genetibase.Debug
{
    partial class frmModules
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
            Janus.Windows.GridEX.GridEXLayout lvModules_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmModules));
            Janus.Windows.GridEX.GridEXLayout lvModDetail_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.UI.StatusBar.UIStatusBarPanel uiStatusBarPanel1 = new Janus.Windows.UI.StatusBar.UIStatusBarPanel();
            this.lvModules = new Janus.Windows.GridEX.GridEX();
            this.lvModDetail = new Janus.Windows.GridEX.GridEX();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.uiStatusBar1 = new Janus.Windows.UI.StatusBar.UIStatusBar();
            ((System.ComponentModel.ISupportInitialize)(this.lvModules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvModDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // lvModules
            // 
            this.lvModules.AllowColumnDrag = false;
            this.lvModules.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.lvModules.AutomaticSort = false;
            this.lvModules.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.lvModules.ColumnAutoResize = true;
            lvModules_DesignTimeLayout.LayoutString = resources.GetString("lvModules_DesignTimeLayout.LayoutString");
            this.lvModules.DesignTimeLayout = lvModules_DesignTimeLayout;
            this.lvModules.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvModules.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvModules.GridLines = Janus.Windows.GridEX.GridLines.None;
            this.lvModules.GroupByBoxVisible = false;
            this.lvModules.Location = new System.Drawing.Point(0, 0);
            this.lvModules.Name = "lvModules";
            this.lvModules.Size = new System.Drawing.Size(181, 289);
            this.lvModules.TabIndex = 8;
            this.lvModules.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.lvModules.SelectionChanged += new System.EventHandler(this.lvModules_SelectionChanged);
            // 
            // lvModDetail
            // 
            this.lvModDetail.AllowColumnDrag = false;
            this.lvModDetail.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.lvModDetail.AutomaticSort = false;
            this.lvModDetail.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.lvModDetail.ColumnAutoResize = true;
            lvModDetail_DesignTimeLayout.LayoutString = resources.GetString("lvModDetail_DesignTimeLayout.LayoutString");
            this.lvModDetail.DesignTimeLayout = lvModDetail_DesignTimeLayout;
            this.lvModDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvModDetail.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvModDetail.GridLines = Janus.Windows.GridEX.GridLines.None;
            this.lvModDetail.GroupByBoxVisible = false;
            this.lvModDetail.Location = new System.Drawing.Point(181, 0);
            this.lvModDetail.Name = "lvModDetail";
            this.lvModDetail.Size = new System.Drawing.Size(539, 289);
            this.lvModDetail.TabIndex = 9;
            this.lvModDetail.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(181, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 289);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // uiStatusBar1
            // 
            this.uiStatusBar1.Location = new System.Drawing.Point(184, 262);
            this.uiStatusBar1.Name = "uiStatusBar1";
            uiStatusBarPanel1.BorderColor = System.Drawing.Color.Empty;
            uiStatusBarPanel1.Key = "";
            uiStatusBarPanel1.ProgressBarValue = 0;
            uiStatusBarPanel1.Text = "Ready";
            this.uiStatusBar1.Panels.AddRange(new Janus.Windows.UI.StatusBar.UIStatusBarPanel[] {
            uiStatusBarPanel1});
            this.uiStatusBar1.Size = new System.Drawing.Size(536, 27);
            this.uiStatusBar1.TabIndex = 11;
            this.uiStatusBar1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // frmModules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 289);
            this.Controls.Add(this.uiStatusBar1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.lvModDetail);
            this.Controls.Add(this.lvModules);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmModules";
            this.Text = "Module Detail";
            ((System.ComponentModel.ISupportInitialize)(this.lvModules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvModDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.GridEX.GridEX lvModules;
        private Janus.Windows.GridEX.GridEX lvModDetail;
        private System.Windows.Forms.Splitter splitter1;
        private Janus.Windows.UI.StatusBar.UIStatusBar uiStatusBar1;

    }
}