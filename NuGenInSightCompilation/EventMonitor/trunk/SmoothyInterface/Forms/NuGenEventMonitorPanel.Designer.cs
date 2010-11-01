namespace SmoothyInterface
{
	partial class NuGenEventMonitorPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NuGenEventMonitorPanel));
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.treeViewLogs = new System.Windows.Forms.TreeView();
            this.treeImageList = new System.Windows.Forms.ImageList(this.components);
            this.treeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.clearEventLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteEventLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.computerContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshEventLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newEventLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.treeContextMenu.SuspendLayout();
            this.computerContextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.SuspendLayout();
            // 
            // treeViewLogs
            // 
            this.treeViewLogs.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeViewLogs.ImageIndex = 0;
            this.treeViewLogs.ImageList = this.treeImageList;
            this.treeViewLogs.Location = new System.Drawing.Point(0, 0);
            this.treeViewLogs.Name = "treeViewLogs";
            this.treeViewLogs.SelectedImageIndex = 0;
            this.treeViewLogs.Size = new System.Drawing.Size(204, 453);
            this.treeViewLogs.TabIndex = 4;
            this.treeViewLogs.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewLogs_NodeMouseDoubleClick);
            this.treeViewLogs.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLogs_AfterSelect);
            this.treeViewLogs.KeyUp += new System.Windows.Forms.KeyEventHandler(this.treeViewLogs_KeyUp);
            this.treeViewLogs.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeViewLogs_NodeMouseClick);
            // 
            // treeImageList
            // 
            this.treeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImageList.ImageStream")));
            this.treeImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.treeImageList.Images.SetKeyName(0, "LcdScreen.png");
            this.treeImageList.Images.SetKeyName(1, "Cabinet.png");
            this.treeImageList.Images.SetKeyName(2, "Box.png");
            // 
            // treeContextMenu
            // 
            this.treeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem1,
            this.clearEventLogToolStripMenuItem,
            this.deleteEventLogToolStripMenuItem});
            this.treeContextMenu.Name = "treeContextMenu";
            this.treeContextMenu.Size = new System.Drawing.Size(108, 70);
            this.treeContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.treeContextMenu_Opening);
            // 
            // openToolStripMenuItem1
            // 
            this.openToolStripMenuItem1.Image = global::EventMonitor.Properties.Resources.link_go;
            this.openToolStripMenuItem1.Name = "openToolStripMenuItem1";
            this.openToolStripMenuItem1.Size = new System.Drawing.Size(107, 22);
            this.openToolStripMenuItem1.Text = "O&pen";
            this.openToolStripMenuItem1.Click += new System.EventHandler(this.OpenLog);
            // 
            // clearEventLogToolStripMenuItem
            // 
            this.clearEventLogToolStripMenuItem.Image = global::EventMonitor.Properties.Resources.flag_orange;
            this.clearEventLogToolStripMenuItem.Name = "clearEventLogToolStripMenuItem";
            this.clearEventLogToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.clearEventLogToolStripMenuItem.Text = "&Clear";
            this.clearEventLogToolStripMenuItem.Click += new System.EventHandler(this.clearEventLogToolStripMenuItem_Click);
            // 
            // deleteEventLogToolStripMenuItem
            // 
            this.deleteEventLogToolStripMenuItem.Image = global::EventMonitor.Properties.Resources.link_error;
            this.deleteEventLogToolStripMenuItem.Name = "deleteEventLogToolStripMenuItem";
            this.deleteEventLogToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.deleteEventLogToolStripMenuItem.Text = "&Delete";
            this.deleteEventLogToolStripMenuItem.Click += new System.EventHandler(this.deleteEventLogToolStripMenuItem_Click);
            // 
            // computerContextMenuStrip
            // 
            this.computerContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshEventLogsToolStripMenuItem,
            this.newEventLogToolStripMenuItem,
            this.clearAllToolStripMenuItem});
            this.computerContextMenuStrip.Name = "treeContextMenu";
            this.computerContextMenuStrip.Size = new System.Drawing.Size(174, 70);
            // 
            // refreshEventLogsToolStripMenuItem
            // 
            this.refreshEventLogsToolStripMenuItem.Image = global::EventMonitor.Properties.Resources.action_refresh_blue;
            this.refreshEventLogsToolStripMenuItem.Name = "refreshEventLogsToolStripMenuItem";
            this.refreshEventLogsToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.refreshEventLogsToolStripMenuItem.Text = "Refresh Event Logs";
            this.refreshEventLogsToolStripMenuItem.Click += new System.EventHandler(this.refreshEventLogsToolStripMenuItem_Click);
            // 
            // newEventLogToolStripMenuItem
            // 
            this.newEventLogToolStripMenuItem.Image = global::EventMonitor.Properties.Resources.link_add;
            this.newEventLogToolStripMenuItem.Name = "newEventLogToolStripMenuItem";
            this.newEventLogToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.newEventLogToolStripMenuItem.Text = "&New Event Log";
            // 
            // clearAllToolStripMenuItem
            // 
            this.clearAllToolStripMenuItem.Image = global::EventMonitor.Properties.Resources.flag_red;
            this.clearAllToolStripMenuItem.Name = "clearAllToolStripMenuItem";
            this.clearAllToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.clearAllToolStripMenuItem.Text = "&Clear All Logs";
            this.clearAllToolStripMenuItem.Click += new System.EventHandler(this.clearAllToolStripMenuItem_Click);
            // 
            // uiTab1
            // 
            this.uiTab1.BackColor = System.Drawing.Color.Transparent;
            this.uiTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTab1.Location = new System.Drawing.Point(204, 0);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.ShowCloseButton = true;
            this.uiTab1.Size = new System.Drawing.Size(428, 453);
            this.uiTab1.TabIndex = 6;
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2007;
            this.uiTab1.TabClosed += new Janus.Windows.UI.Tab.TabEventHandler(this.uiTab1_TabClosed);
            this.uiTab1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.uiTab1_MouseClick);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(204, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 453);
            this.splitter1.TabIndex = 7;
            this.splitter1.TabStop = false;
            // 
            // NuGenEventMonitorPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.uiTab1);
            this.Controls.Add(this.treeViewLogs);
            this.Name = "NuGenEventMonitorPanel";
            this.Size = new System.Drawing.Size(632, 453);
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.treeContextMenu.ResumeLayout(false);
            this.computerContextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

        private System.Windows.Forms.ToolTip ToolTip;
		private System.Windows.Forms.TreeView treeViewLogs;
		private System.Windows.Forms.ImageList treeImageList;
		private System.Windows.Forms.ContextMenuStrip treeContextMenu;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem clearEventLogToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteEventLogToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip computerContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newEventLogToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshEventLogsToolStripMenuItem;
        private Janus.Windows.UI.Tab.UITab uiTab1;
        private System.Windows.Forms.Splitter splitter1;
	}
}



