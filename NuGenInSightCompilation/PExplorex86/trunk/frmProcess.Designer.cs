namespace Genetibase.Debug
{
    partial class frmProcess
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
            this.components = new System.ComponentModel.Container();
            Janus.Windows.GridEX.GridEXLayout lvProcesses_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProcess));
            Janus.Windows.GridEX.GridEXLayout lvProcessDetail_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout lvThreads_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.ctxView = new System.Windows.Forms.ContextMenu();
            this.ctxViewMods = new System.Windows.Forms.MenuItem();
            this.MenuItem1 = new System.Windows.Forms.MenuItem();
            this.ctxRefresh = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.ctxKillProc = new System.Windows.Forms.MenuItem();
            this.splHor = new System.Windows.Forms.Splitter();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.lvProcesses = new Janus.Windows.GridEX.GridEX();
            this.lvProcessDetail = new Janus.Windows.GridEX.GridEX();
            this.lvThreads = new Janus.Windows.GridEX.GridEX();
            this.uiCommandManager1 = new Janus.Windows.UI.CommandBars.UICommandManager(this.components);
            this.BottomRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
            this.uiCommandBar1 = new Janus.Windows.UI.CommandBars.UICommandBar();
            this.Command01 = new Janus.Windows.UI.CommandBars.UICommand("Command0");
            this.Command11 = new Janus.Windows.UI.CommandBars.UICommand("Command1");
            this.Command21 = new Janus.Windows.UI.CommandBars.UICommand("Command2");
            this.Command0 = new Janus.Windows.UI.CommandBars.UICommand("Command0");
            this.Command1 = new Janus.Windows.UI.CommandBars.UICommand("Command1");
            this.Command2 = new Janus.Windows.UI.CommandBars.UICommand("Command2");
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.LeftRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
            this.RightRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
            this.TopRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvProcesses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvProcessDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvThreads)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).BeginInit();
            this.TopRebar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctxView
            // 
            this.ctxView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.ctxViewMods,
            this.MenuItem1,
            this.ctxRefresh,
            this.menuItem3,
            this.ctxKillProc});
            // 
            // ctxViewMods
            // 
            this.ctxViewMods.Index = 0;
            this.ctxViewMods.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
            this.ctxViewMods.Text = "View &Modules";
            this.ctxViewMods.Click += new System.EventHandler(this.ctxViewMods_Click);
            // 
            // MenuItem1
            // 
            this.MenuItem1.Index = 1;
            this.MenuItem1.Text = "-";
            // 
            // ctxRefresh
            // 
            this.ctxRefresh.Index = 2;
            this.ctxRefresh.Shortcut = System.Windows.Forms.Shortcut.F5;
            this.ctxRefresh.Text = "&Refresh";
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 3;
            this.menuItem3.Text = "-";
            // 
            // ctxKillProc
            // 
            this.ctxKillProc.Index = 4;
            this.ctxKillProc.Text = "&Kill Process";
            this.ctxKillProc.Click += new System.EventHandler(this.ctxKillProc_Click);
            // 
            // splHor
            // 
            this.splHor.Dock = System.Windows.Forms.DockStyle.Top;
            this.splHor.Location = new System.Drawing.Point(0, 28);
            this.splHor.Name = "splHor";
            this.splHor.Size = new System.Drawing.Size(916, 3);
            this.splHor.TabIndex = 24;
            this.splHor.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 31);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvThreads);
            this.splitContainer1.Size = new System.Drawing.Size(916, 621);
            this.splitContainer1.SplitterDistance = 469;
            this.splitContainer1.TabIndex = 25;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.lvProcesses);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.lvProcessDetail);
            this.splitContainer2.Size = new System.Drawing.Size(916, 469);
            this.splitContainer2.SplitterDistance = 575;
            this.splitContainer2.TabIndex = 0;
            // 
            // lvProcesses
            // 
            this.lvProcesses.AllowColumnDrag = false;
            this.lvProcesses.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.lvProcesses.AlternatingColors = true;
            this.lvProcesses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvProcesses.AutomaticSort = false;
            this.lvProcesses.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.lvProcesses.ColumnAutoResize = true;
            lvProcesses_DesignTimeLayout.LayoutString = resources.GetString("lvProcesses_DesignTimeLayout.LayoutString");
            this.lvProcesses.DesignTimeLayout = lvProcesses_DesignTimeLayout;
            this.lvProcesses.EmptyRows = true;
            this.lvProcesses.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.lvProcesses.GroupByBoxVisible = false;
            this.lvProcesses.Location = new System.Drawing.Point(3, 3);
            this.lvProcesses.Name = "lvProcesses";
            this.lvProcesses.Size = new System.Drawing.Size(569, 463);
            this.lvProcesses.TabIndex = 16;
            this.lvProcesses.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.lvProcesses.SelectionChanged += new System.EventHandler(this.lvProcesses_SelectedIndexChanged);
            // 
            // lvProcessDetail
            // 
            this.lvProcessDetail.AllowColumnDrag = false;
            this.lvProcessDetail.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.lvProcessDetail.AlternatingColors = true;
            this.lvProcessDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvProcessDetail.AutomaticSort = false;
            this.lvProcessDetail.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.lvProcessDetail.ColumnAutoResize = true;
            lvProcessDetail_DesignTimeLayout.LayoutString = resources.GetString("lvProcessDetail_DesignTimeLayout.LayoutString");
            this.lvProcessDetail.DesignTimeLayout = lvProcessDetail_DesignTimeLayout;
            this.lvProcessDetail.EmptyRows = true;
            this.lvProcessDetail.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.lvProcessDetail.GroupByBoxVisible = false;
            this.lvProcessDetail.Location = new System.Drawing.Point(3, 3);
            this.lvProcessDetail.Name = "lvProcessDetail";
            this.lvProcessDetail.Size = new System.Drawing.Size(331, 464);
            this.lvProcessDetail.TabIndex = 17;
            this.lvProcessDetail.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // lvThreads
            // 
            this.lvThreads.AllowColumnDrag = false;
            this.lvThreads.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.lvThreads.AlternatingColors = true;
            this.lvThreads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvThreads.AutomaticSort = false;
            this.lvThreads.BoundMode = Janus.Windows.GridEX.BoundMode.Unbound;
            this.lvThreads.ColumnAutoResize = true;
            lvThreads_DesignTimeLayout.LayoutString = resources.GetString("lvThreads_DesignTimeLayout.LayoutString");
            this.lvThreads.DesignTimeLayout = lvThreads_DesignTimeLayout;
            this.lvThreads.EmptyRows = true;
            this.lvThreads.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.lvThreads.GroupByBoxVisible = false;
            this.lvThreads.Location = new System.Drawing.Point(3, 5);
            this.lvThreads.Name = "lvThreads";
            this.lvThreads.Size = new System.Drawing.Size(910, 139);
            this.lvThreads.TabIndex = 18;
            this.lvThreads.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiCommandManager1
            // 
            this.uiCommandManager1.BottomRebar = this.BottomRebar1;
            this.uiCommandManager1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar1});
            this.uiCommandManager1.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Command0,
            this.Command1,
            this.Command2});
            this.uiCommandManager1.ContainerControl = this;
            this.uiCommandManager1.Id = new System.Guid("95b3b0a0-5494-461e-995d-ac5ef977818c");
            this.uiCommandManager1.ImageList = this.imageList1;
            this.uiCommandManager1.LeftRebar = this.LeftRebar1;
            this.uiCommandManager1.RightRebar = this.RightRebar1;
            this.uiCommandManager1.TopRebar = this.TopRebar1;
            this.uiCommandManager1.CommandClick += new Janus.Windows.UI.CommandBars.CommandEventHandler(this.uiCommandManager1_CommandClick);
            // 
            // BottomRebar1
            // 
            this.BottomRebar1.CommandManager = this.uiCommandManager1;
            this.BottomRebar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomRebar1.Location = new System.Drawing.Point(0, 652);
            this.BottomRebar1.Name = "BottomRebar1";
            this.BottomRebar1.Size = new System.Drawing.Size(916, 0);
            // 
            // uiCommandBar1
            // 
            this.uiCommandBar1.AllowClose = Janus.Windows.UI.InheritableBoolean.False;
            this.uiCommandBar1.AllowCustomize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiCommandBar1.CommandManager = this.uiCommandManager1;
            this.uiCommandBar1.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Command01,
            this.Command11,
            this.Command21});
            this.uiCommandBar1.FullRow = true;
            this.uiCommandBar1.Key = "CommandBar1";
            this.uiCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.uiCommandBar1.Name = "uiCommandBar1";
            this.uiCommandBar1.RowIndex = 0;
            this.uiCommandBar1.ShowAddRemoveButton = Janus.Windows.UI.InheritableBoolean.False;
            this.uiCommandBar1.ShowCustomizeButton = Janus.Windows.UI.InheritableBoolean.False;
            this.uiCommandBar1.ShowToolTips = Janus.Windows.UI.InheritableBoolean.False;
            this.uiCommandBar1.Size = new System.Drawing.Size(916, 28);
            this.uiCommandBar1.Text = "CommandBar1";
            this.uiCommandBar1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // Command01
            // 
            this.Command01.ImageIndex = 0;
            this.Command01.Key = "Command0";
            this.Command01.Name = "Command01";
            // 
            // Command11
            // 
            this.Command11.ImageIndex = 1;
            this.Command11.Key = "Command1";
            this.Command11.Name = "Command11";
            // 
            // Command21
            // 
            this.Command21.ImageIndex = 2;
            this.Command21.Key = "Command2";
            this.Command21.Name = "Command21";
            // 
            // Command0
            // 
            this.Command0.Key = "Command0";
            this.Command0.Name = "Command0";
            this.Command0.Text = "Run As";
            // 
            // Command1
            // 
            this.Command1.Key = "Command1";
            this.Command1.Name = "Command1";
            this.Command1.Text = "View Modules";
            // 
            // Command2
            // 
            this.Command2.Key = "Command2";
            this.Command2.Name = "Command2";
            this.Command2.Text = "Refresh";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "NavigateForward.png");
            this.imageList1.Images.SetKeyName(1, "OpenFile.png");
            this.imageList1.Images.SetKeyName(2, "Refresh.png");
            // 
            // LeftRebar1
            // 
            this.LeftRebar1.CommandManager = this.uiCommandManager1;
            this.LeftRebar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftRebar1.Location = new System.Drawing.Point(0, 28);
            this.LeftRebar1.Name = "LeftRebar1";
            this.LeftRebar1.Size = new System.Drawing.Size(0, 624);
            // 
            // RightRebar1
            // 
            this.RightRebar1.CommandManager = this.uiCommandManager1;
            this.RightRebar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightRebar1.Location = new System.Drawing.Point(916, 28);
            this.RightRebar1.Name = "RightRebar1";
            this.RightRebar1.Size = new System.Drawing.Size(0, 624);
            // 
            // TopRebar1
            // 
            this.TopRebar1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar1});
            this.TopRebar1.CommandManager = this.uiCommandManager1;
            this.TopRebar1.Controls.Add(this.uiCommandBar1);
            this.TopRebar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopRebar1.Location = new System.Drawing.Point(0, 0);
            this.TopRebar1.Name = "TopRebar1";
            this.TopRebar1.Size = new System.Drawing.Size(916, 28);
            // 
            // frmProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 652);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splHor);
            this.Controls.Add(this.LeftRebar1);
            this.Controls.Add(this.RightRebar1);
            this.Controls.Add(this.TopRebar1);
            this.Controls.Add(this.BottomRebar1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmProcess";
            this.Text = "PExplore";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvProcesses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvProcessDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lvThreads)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).EndInit();
            this.TopRebar1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenu ctxView;
        private System.Windows.Forms.MenuItem ctxViewMods;
        private System.Windows.Forms.MenuItem MenuItem1;
        private System.Windows.Forms.MenuItem ctxRefresh;
        private System.Windows.Forms.Splitter splHor;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.MenuItem menuItem3;
        private System.Windows.Forms.MenuItem ctxKillProc;
        private Janus.Windows.GridEX.GridEX lvProcesses;
        private Janus.Windows.GridEX.GridEX lvProcessDetail;
        private Janus.Windows.GridEX.GridEX lvThreads;
        private Janus.Windows.UI.CommandBars.UICommandManager uiCommandManager1;
        private Janus.Windows.UI.CommandBars.UIRebar BottomRebar1;
        private Janus.Windows.UI.CommandBars.UICommandBar uiCommandBar1;
        private Janus.Windows.UI.CommandBars.UICommand Command01;
        private Janus.Windows.UI.CommandBars.UICommand Command11;
        private Janus.Windows.UI.CommandBars.UICommand Command21;
        private Janus.Windows.UI.CommandBars.UICommand Command0;
        private Janus.Windows.UI.CommandBars.UICommand Command1;
        private Janus.Windows.UI.CommandBars.UICommand Command2;
        private Janus.Windows.UI.CommandBars.UIRebar LeftRebar1;
        private Janus.Windows.UI.CommandBars.UIRebar RightRebar1;
        private Janus.Windows.UI.CommandBars.UIRebar TopRebar1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

