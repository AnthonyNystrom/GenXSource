namespace SmoothyInterface.Forms
{
	partial class EventLogViewer
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
            Janus.Windows.GridEX.GridEXLayout dgEvents_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventLogViewer));
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.dgEvents = new Janus.Windows.GridEX.GridEX();
            this.tbEventDetail = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCopy = new System.Windows.Forms.Button();
            this.eventLogEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uiCommandManager1 = new Janus.Windows.UI.CommandBars.UICommandManager(this.components);
            this.BottomRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
            this.uiCommandBar1 = new Janus.Windows.UI.CommandBars.UICommandBar();
            this.Command01 = new Janus.Windows.UI.CommandBars.UICommand("Command0");
            this.Separator1 = new Janus.Windows.UI.CommandBars.UICommand("Separator");
            this.Command11 = new Janus.Windows.UI.CommandBars.UICommand("Command1");
            this.Separator2 = new Janus.Windows.UI.CommandBars.UICommand("Separator");
            this.Command21 = new Janus.Windows.UI.CommandBars.UICommand("Command2");
            this.Command31 = new Janus.Windows.UI.CommandBars.UICommand("Command3");
            this.Command41 = new Janus.Windows.UI.CommandBars.UICommand("Command4");
            this.Command51 = new Janus.Windows.UI.CommandBars.UICommand("Command5");
            this.Command61 = new Janus.Windows.UI.CommandBars.UICommand("Command6");
            this.uiCommandBar2 = new Janus.Windows.UI.CommandBars.UICommandBar();
            this.Command71 = new Janus.Windows.UI.CommandBars.UICommand("Command7");
            this.Command81 = new Janus.Windows.UI.CommandBars.UICommand("Command8");
            this.Separator3 = new Janus.Windows.UI.CommandBars.UICommand("Separator");
            this.Command91 = new Janus.Windows.UI.CommandBars.UICommand("Command9");
            this.Command101 = new Janus.Windows.UI.CommandBars.UICommand("Command10");
            this.Command111 = new Janus.Windows.UI.CommandBars.UICommand("Command11");
            this.Command121 = new Janus.Windows.UI.CommandBars.UICommand("Command12");
            this.refreshCommand = new Janus.Windows.UI.CommandBars.UICommand("Command0");
            this.clearCommand = new Janus.Windows.UI.CommandBars.UICommand("Command1");
            this.infoCommand = new Janus.Windows.UI.CommandBars.UICommand("Command2");
            this.warningsCommand = new Janus.Windows.UI.CommandBars.UICommand("Command3");
            this.errorsCommand = new Janus.Windows.UI.CommandBars.UICommand("Command4");
            this.successCommand = new Janus.Windows.UI.CommandBars.UICommand("Command5");
            this.failureCommand = new Janus.Windows.UI.CommandBars.UICommand("Command6");
            this.Command7 = new Janus.Windows.UI.CommandBars.UICommand("Command7");
            this.filterCombo = new Janus.Windows.UI.CommandBars.UICommand("Command8");
            this.Command9 = new Janus.Windows.UI.CommandBars.UICommand("Command9");
            this.searchTextBox = new Janus.Windows.UI.CommandBars.UICommand("Command10");
            this.searchCommand = new Janus.Windows.UI.CommandBars.UICommand("Command11");
            this.clearSearchCommand = new Janus.Windows.UI.CommandBars.UICommand("Command12");
            this.LeftRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
            this.RightRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
            this.TopRebar1 = new Janus.Windows.UI.CommandBars.UIRebar();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEvents)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.eventLogEntryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).BeginInit();
            this.TopRebar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 56);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.toolStripContainer1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tbEventDetail);
            this.splitContainer.Panel2.Controls.Add(this.panel1);
            this.splitContainer.Size = new System.Drawing.Size(763, 502);
            this.splitContainer.SplitterDistance = 376;
            this.splitContainer.TabIndex = 3;
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.dgEvents);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(763, 351);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(763, 376);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // dgEvents
            // 
            this.dgEvents.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.dgEvents.AlternatingColors = true;
            this.dgEvents.ColumnAutoResize = true;
            this.dgEvents.ColumnAutoSizeMode = Janus.Windows.GridEX.ColumnAutoSizeMode.ColumnHeader;
            dgEvents_DesignTimeLayout.LayoutString = resources.GetString("dgEvents_DesignTimeLayout.LayoutString");
            this.dgEvents.DesignTimeLayout = dgEvents_DesignTimeLayout;
            this.dgEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgEvents.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.dgEvents.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.dgEvents.GroupByBoxVisible = false;
            this.dgEvents.ImageList = this.imageList1;
            this.dgEvents.Location = new System.Drawing.Point(0, 0);
            this.dgEvents.Name = "dgEvents";
            this.dgEvents.Size = new System.Drawing.Size(763, 351);
            this.dgEvents.TabIndex = 2;
            this.dgEvents.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.dgEvents.SelectionChanged += new System.EventHandler(this.dgEvents_SelectionChanged);
            this.dgEvents.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgEvents_KeyUp);
            // 
            // tbEventDetail
            // 
            this.tbEventDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbEventDetail.Location = new System.Drawing.Point(0, 0);
            this.tbEventDetail.Name = "tbEventDetail";
            this.tbEventDetail.ReadOnly = true;
            this.tbEventDetail.Size = new System.Drawing.Size(726, 122);
            this.tbEventDetail.TabIndex = 2;
            this.tbEventDetail.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCopy);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(726, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(37, 122);
            this.panel1.TabIndex = 1;
            // 
            // btnCopy
            // 
            this.btnCopy.Image = global::EventMonitor.Properties.Resources.page_copy;
            this.btnCopy.Location = new System.Drawing.Point(5, 5);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(28, 28);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // eventLogEntryBindingSource
            // 
            this.eventLogEntryBindingSource.DataSource = typeof(System.Diagnostics.EventLogEntry);
            // 
            // uiCommandManager1
            // 
            this.uiCommandManager1.BottomRebar = this.BottomRebar1;
            this.uiCommandManager1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar1,
            this.uiCommandBar2});
            this.uiCommandManager1.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.refreshCommand,
            this.clearCommand,
            this.infoCommand,
            this.warningsCommand,
            this.errorsCommand,
            this.successCommand,
            this.failureCommand,
            this.Command7,
            this.filterCombo,
            this.Command9,
            this.searchTextBox,
            this.searchCommand,
            this.clearSearchCommand});
            this.uiCommandManager1.ContainerControl = this;
            this.uiCommandManager1.Id = new System.Guid("e0cdc07f-4f7d-4498-8dcf-99bd13a233ed");
            this.uiCommandManager1.LeftRebar = this.LeftRebar1;
            this.uiCommandManager1.RightRebar = this.RightRebar1;
            this.uiCommandManager1.TopRebar = this.TopRebar1;
            this.uiCommandManager1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiCommandManager1.CommandClick += new Janus.Windows.UI.CommandBars.CommandEventHandler(this.uiCommandManager1_CommandClick);
            // 
            // BottomRebar1
            // 
            this.BottomRebar1.CommandManager = this.uiCommandManager1;
            this.BottomRebar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BottomRebar1.Location = new System.Drawing.Point(0, 558);
            this.BottomRebar1.Name = "BottomRebar1";
            this.BottomRebar1.Size = new System.Drawing.Size(763, 0);
            // 
            // uiCommandBar1
            // 
            this.uiCommandBar1.CommandManager = this.uiCommandManager1;
            this.uiCommandBar1.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Command01,
            this.Separator1,
            this.Command11,
            this.Separator2,
            this.Command21,
            this.Command31,
            this.Command41,
            this.Command51,
            this.Command61});
            this.uiCommandBar1.Key = "CommandBar1";
            this.uiCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.uiCommandBar1.Name = "uiCommandBar1";
            this.uiCommandBar1.RowIndex = 0;
            this.uiCommandBar1.Size = new System.Drawing.Size(629, 28);
            this.uiCommandBar1.Text = "CommandBar1";
            // 
            // Command01
            // 
            this.Command01.Key = "Command0";
            this.Command01.Name = "Command01";
            // 
            // Separator1
            // 
            this.Separator1.CommandType = Janus.Windows.UI.CommandBars.CommandType.Separator;
            this.Separator1.Key = "Separator";
            this.Separator1.Name = "Separator1";
            // 
            // Command11
            // 
            this.Command11.Key = "Command1";
            this.Command11.Name = "Command11";
            // 
            // Separator2
            // 
            this.Separator2.CommandType = Janus.Windows.UI.CommandBars.CommandType.Separator;
            this.Separator2.Key = "Separator";
            this.Separator2.Name = "Separator2";
            // 
            // Command21
            // 
            this.Command21.Key = "Command2";
            this.Command21.Name = "Command21";
            // 
            // Command31
            // 
            this.Command31.Key = "Command3";
            this.Command31.Name = "Command31";
            // 
            // Command41
            // 
            this.Command41.Key = "Command4";
            this.Command41.Name = "Command41";
            // 
            // Command51
            // 
            this.Command51.Key = "Command5";
            this.Command51.Name = "Command51";
            // 
            // Command61
            // 
            this.Command61.Key = "Command6";
            this.Command61.Name = "Command61";
            // 
            // uiCommandBar2
            // 
            this.uiCommandBar2.CommandManager = this.uiCommandManager1;
            this.uiCommandBar2.Commands.AddRange(new Janus.Windows.UI.CommandBars.UICommand[] {
            this.Command71,
            this.Command81,
            this.Separator3,
            this.Command91,
            this.Command101,
            this.Command111,
            this.Command121});
            this.uiCommandBar2.Key = "CommandBar2";
            this.uiCommandBar2.Location = new System.Drawing.Point(0, 28);
            this.uiCommandBar2.Name = "uiCommandBar2";
            this.uiCommandBar2.RowIndex = 1;
            this.uiCommandBar2.Size = new System.Drawing.Size(574, 28);
            this.uiCommandBar2.Text = "CommandBar2";
            // 
            // Command71
            // 
            this.Command71.Key = "Command7";
            this.Command71.Name = "Command71";
            // 
            // Command81
            // 
            this.Command81.Key = "Command8";
            this.Command81.Name = "Command81";
            // 
            // Separator3
            // 
            this.Separator3.CommandType = Janus.Windows.UI.CommandBars.CommandType.Separator;
            this.Separator3.Key = "Separator";
            this.Separator3.Name = "Separator3";
            // 
            // Command91
            // 
            this.Command91.Key = "Command9";
            this.Command91.Name = "Command91";
            // 
            // Command101
            // 
            this.Command101.ControlValue = "";
            this.Command101.Key = "Command10";
            this.Command101.Name = "Command101";
            // 
            // Command111
            // 
            this.Command111.Image = ((System.Drawing.Image)(resources.GetObject("Command111.Image")));
            this.Command111.Key = "Command11";
            this.Command111.Name = "Command111";
            // 
            // Command121
            // 
            this.Command121.Image = ((System.Drawing.Image)(resources.GetObject("Command121.Image")));
            this.Command121.Key = "Command12";
            this.Command121.Name = "Command121";
            // 
            // refreshCommand
            // 
            this.refreshCommand.Image = ((System.Drawing.Image)(resources.GetObject("refreshCommand.Image")));
            this.refreshCommand.Key = "Command0";
            this.refreshCommand.Name = "refreshCommand";
            this.refreshCommand.Text = "Refresh";
            // 
            // clearCommand
            // 
            this.clearCommand.Image = ((System.Drawing.Image)(resources.GetObject("clearCommand.Image")));
            this.clearCommand.Key = "Command1";
            this.clearCommand.Name = "clearCommand";
            this.clearCommand.Text = "Clear";
            // 
            // infoCommand
            // 
            this.infoCommand.Checked = Janus.Windows.UI.InheritableBoolean.True;
            this.infoCommand.CommandType = Janus.Windows.UI.CommandBars.CommandType.ToggleButton;
            this.infoCommand.Image = ((System.Drawing.Image)(resources.GetObject("infoCommand.Image")));
            this.infoCommand.Key = "Command2";
            this.infoCommand.Name = "infoCommand";
            this.infoCommand.Text = "0 Information";
            // 
            // warningsCommand
            // 
            this.warningsCommand.Checked = Janus.Windows.UI.InheritableBoolean.True;
            this.warningsCommand.CommandType = Janus.Windows.UI.CommandBars.CommandType.ToggleButton;
            this.warningsCommand.Image = ((System.Drawing.Image)(resources.GetObject("warningsCommand.Image")));
            this.warningsCommand.Key = "Command3";
            this.warningsCommand.Name = "warningsCommand";
            this.warningsCommand.Text = "0 Warnings";
            // 
            // errorsCommand
            // 
            this.errorsCommand.Checked = Janus.Windows.UI.InheritableBoolean.True;
            this.errorsCommand.CommandType = Janus.Windows.UI.CommandBars.CommandType.ToggleButton;
            this.errorsCommand.Image = ((System.Drawing.Image)(resources.GetObject("errorsCommand.Image")));
            this.errorsCommand.Key = "Command4";
            this.errorsCommand.Name = "errorsCommand";
            this.errorsCommand.Text = "0 Errors";
            // 
            // successCommand
            // 
            this.successCommand.Checked = Janus.Windows.UI.InheritableBoolean.True;
            this.successCommand.CommandType = Janus.Windows.UI.CommandBars.CommandType.ToggleButton;
            this.successCommand.Image = ((System.Drawing.Image)(resources.GetObject("successCommand.Image")));
            this.successCommand.Key = "Command5";
            this.successCommand.Name = "successCommand";
            this.successCommand.Text = "0 Success Audit";
            // 
            // failureCommand
            // 
            this.failureCommand.Checked = Janus.Windows.UI.InheritableBoolean.True;
            this.failureCommand.CommandType = Janus.Windows.UI.CommandBars.CommandType.ToggleButton;
            this.failureCommand.Image = ((System.Drawing.Image)(resources.GetObject("failureCommand.Image")));
            this.failureCommand.Key = "Command6";
            this.failureCommand.Name = "failureCommand";
            this.failureCommand.Text = "0 Failure Audit";
            // 
            // Command7
            // 
            this.Command7.CommandType = Janus.Windows.UI.CommandBars.CommandType.Label;
            this.Command7.Key = "Command7";
            this.Command7.Name = "Command7";
            this.Command7.Text = "Filter Source :";
            // 
            // filterCombo
            // 
            this.filterCombo.CommandType = Janus.Windows.UI.CommandBars.CommandType.ComboBoxCommand;
            this.filterCombo.IsEditableControl = Janus.Windows.UI.InheritableBoolean.True;
            this.filterCombo.Key = "Command8";
            this.filterCombo.Name = "filterCombo";
            this.filterCombo.ControlValueChanged += new Janus.Windows.UI.CommandBars.CommandEventHandler(this.filterCombo_ControlValueChanged);
            // 
            // Command9
            // 
            this.Command9.CommandType = Janus.Windows.UI.CommandBars.CommandType.Label;
            this.Command9.Key = "Command9";
            this.Command9.Name = "Command9";
            this.Command9.Text = "Search Messages :";
            // 
            // searchTextBox
            // 
            this.searchTextBox.CommandType = Janus.Windows.UI.CommandBars.CommandType.TextBoxCommand;
            this.searchTextBox.ControlValue = "";
            this.searchTextBox.IsEditableControl = Janus.Windows.UI.InheritableBoolean.True;
            this.searchTextBox.Key = "Command10";
            this.searchTextBox.Name = "searchTextBox";
            // 
            // searchCommand
            // 
            this.searchCommand.Key = "Command11";
            this.searchCommand.Name = "searchCommand";
            this.searchCommand.Text = "Search";
            // 
            // clearSearchCommand
            // 
            this.clearSearchCommand.CommandType = Janus.Windows.UI.CommandBars.CommandType.ToggleButton;
            this.clearSearchCommand.Key = "Command12";
            this.clearSearchCommand.Name = "clearSearchCommand";
            this.clearSearchCommand.Text = "Clear Search";
            // 
            // LeftRebar1
            // 
            this.LeftRebar1.CommandManager = this.uiCommandManager1;
            this.LeftRebar1.Dock = System.Windows.Forms.DockStyle.Left;
            this.LeftRebar1.Location = new System.Drawing.Point(0, 56);
            this.LeftRebar1.Name = "LeftRebar1";
            this.LeftRebar1.Size = new System.Drawing.Size(0, 502);
            // 
            // RightRebar1
            // 
            this.RightRebar1.CommandManager = this.uiCommandManager1;
            this.RightRebar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.RightRebar1.Location = new System.Drawing.Point(763, 56);
            this.RightRebar1.Name = "RightRebar1";
            this.RightRebar1.Size = new System.Drawing.Size(0, 502);
            // 
            // TopRebar1
            // 
            this.TopRebar1.CommandBars.AddRange(new Janus.Windows.UI.CommandBars.UICommandBar[] {
            this.uiCommandBar1,
            this.uiCommandBar2});
            this.TopRebar1.CommandManager = this.uiCommandManager1;
            this.TopRebar1.Controls.Add(this.uiCommandBar1);
            this.TopRebar1.Controls.Add(this.uiCommandBar2);
            this.TopRebar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopRebar1.Location = new System.Drawing.Point(0, 0);
            this.TopRebar1.Name = "TopRebar1";
            this.TopRebar1.Size = new System.Drawing.Size(763, 56);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Info.png");
            this.imageList1.Images.SetKeyName(1, "Remember.png");
            this.imageList1.Images.SetKeyName(2, "Exit2.png");
            this.imageList1.Images.SetKeyName(3, "Radioactive.png");
            this.imageList1.Images.SetKeyName(4, "Key.png");
            // 
            // EventLogViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.TopRebar1);
            this.Name = "EventLogViewer";
            this.Size = new System.Drawing.Size(763, 558);
            this.Load += new System.EventHandler(this.EventLogViewer_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EventLogViewer_KeyUp);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgEvents)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.eventLogEntryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BottomRebar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiCommandBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LeftRebar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RightRebar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TopRebar1)).EndInit();
            this.TopRebar1.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.BindingSource eventLogEntryBindingSource;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private Janus.Windows.GridEX.GridEX dgEvents;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RichTextBox tbEventDetail;
		private System.Windows.Forms.Button btnCopy;
        private Janus.Windows.UI.CommandBars.UICommandManager uiCommandManager1;
        private Janus.Windows.UI.CommandBars.UIRebar BottomRebar1;
        private Janus.Windows.UI.CommandBars.UICommandBar uiCommandBar1;
        private Janus.Windows.UI.CommandBars.UICommandBar uiCommandBar2;
        private Janus.Windows.UI.CommandBars.UICommand refreshCommand;
        private Janus.Windows.UI.CommandBars.UICommand clearCommand;
        private Janus.Windows.UI.CommandBars.UIRebar LeftRebar1;
        private Janus.Windows.UI.CommandBars.UIRebar RightRebar1;
        private Janus.Windows.UI.CommandBars.UIRebar TopRebar1;
        private Janus.Windows.UI.CommandBars.UICommand Command01;
        private Janus.Windows.UI.CommandBars.UICommand Separator1;
        private Janus.Windows.UI.CommandBars.UICommand Command11;
        private Janus.Windows.UI.CommandBars.UICommand Separator2;
        private Janus.Windows.UI.CommandBars.UICommand Command21;
        private Janus.Windows.UI.CommandBars.UICommand Command31;
        private Janus.Windows.UI.CommandBars.UICommand Command41;
        private Janus.Windows.UI.CommandBars.UICommand Command51;
        private Janus.Windows.UI.CommandBars.UICommand Command61;
        private Janus.Windows.UI.CommandBars.UICommand infoCommand;
        private Janus.Windows.UI.CommandBars.UICommand warningsCommand;
        private Janus.Windows.UI.CommandBars.UICommand errorsCommand;
        private Janus.Windows.UI.CommandBars.UICommand successCommand;
        private Janus.Windows.UI.CommandBars.UICommand failureCommand;
        private Janus.Windows.UI.CommandBars.UICommand Command71;
        private Janus.Windows.UI.CommandBars.UICommand Command81;
        private Janus.Windows.UI.CommandBars.UICommand Separator3;
        private Janus.Windows.UI.CommandBars.UICommand Command91;
        private Janus.Windows.UI.CommandBars.UICommand Command101;
        private Janus.Windows.UI.CommandBars.UICommand Command7;
        private Janus.Windows.UI.CommandBars.UICommand filterCombo;
        private Janus.Windows.UI.CommandBars.UICommand Command9;
        private Janus.Windows.UI.CommandBars.UICommand searchTextBox;
        private Janus.Windows.UI.CommandBars.UICommand Command111;
        private Janus.Windows.UI.CommandBars.UICommand Command121;
        private Janus.Windows.UI.CommandBars.UICommand searchCommand;
        private Janus.Windows.UI.CommandBars.UICommand clearSearchCommand;
        private System.Windows.Forms.ImageList imageList1;
	}
}
