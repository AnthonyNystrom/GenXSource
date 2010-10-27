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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventLogViewer));
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.dgEvents = new System.Windows.Forms.DataGridView();
			this.TypeImage = new System.Windows.Forms.DataGridViewImageColumn();
			this.EntryType = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.TimeWritten = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.EventID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.buttonsToolStrip = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonRefresh = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonAutoRefresh = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButtonClearEventLog = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.btnInformation = new System.Windows.Forms.ToolStripButton();
			this.btnWarnings = new System.Windows.Forms.ToolStripButton();
			this.btnErrors = new System.Windows.Forms.ToolStripButton();
			this.btnSuccessAudit = new System.Windows.Forms.ToolStripButton();
			this.btnFailureAudit = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.tsFilterSearch = new System.Windows.Forms.ToolStrip();
			this.ToolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
			this.dlFilterSource = new System.Windows.Forms.ToolStripComboBox();
			this.FindSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.ToolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
			this.tbSearch = new System.Windows.Forms.ToolStripTextBox();
			this.btnSearch = new System.Windows.Forms.ToolStripButton();
			this.btnClearSearch = new System.Windows.Forms.ToolStripButton();
			this.tbEventDetail = new System.Windows.Forms.RichTextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btnCopy = new System.Windows.Forms.Button();
			this.eventLogEntryBindingSource = new System.Windows.Forms.BindingSource(this.components);
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.toolStripContainer1.ContentPanel.SuspendLayout();
			this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgEvents)).BeginInit();
			this.buttonsToolStrip.SuspendLayout();
			this.tsFilterSearch.SuspendLayout();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.eventLogEntryBindingSource)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 0);
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
			this.splitContainer.Size = new System.Drawing.Size(763, 558);
			this.splitContainer.SplitterDistance = 419;
			this.splitContainer.TabIndex = 3;
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Controls.Add(this.dgEvents);
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(763, 369);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(763, 419);
			this.toolStripContainer1.TabIndex = 3;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// toolStripContainer1.TopToolStripPanel
			// 
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.buttonsToolStrip);
			this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.tsFilterSearch);
			// 
			// dgEvents
			// 
			this.dgEvents.AllowUserToAddRows = false;
			this.dgEvents.AllowUserToDeleteRows = false;
			this.dgEvents.AllowUserToOrderColumns = true;
			this.dgEvents.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dgEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgEvents.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TypeImage,
            this.EntryType,
            this.TimeWritten,
            this.Category,
            this.Source,
            this.EventID,
            this.Message});
			this.dgEvents.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgEvents.Location = new System.Drawing.Point(0, 0);
			this.dgEvents.MultiSelect = false;
			this.dgEvents.Name = "dgEvents";
			this.dgEvents.ReadOnly = true;
			this.dgEvents.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			this.dgEvents.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dgEvents.Size = new System.Drawing.Size(763, 369);
			this.dgEvents.TabIndex = 2;
			this.dgEvents.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgEvents_KeyUp);
			this.dgEvents.SelectionChanged += new System.EventHandler(this.dgEvents_SelectionChanged);
			// 
			// TypeImage
			// 
			this.TypeImage.HeaderText = "";
			this.TypeImage.Name = "TypeImage";
			this.TypeImage.ReadOnly = true;
			// 
			// EntryType
			// 
			this.EntryType.DataPropertyName = "EntryType";
			this.EntryType.HeaderText = "Entry Type";
			this.EntryType.Name = "EntryType";
			this.EntryType.ReadOnly = true;
			// 
			// TimeWritten
			// 
			this.TimeWritten.DataPropertyName = "TimeWritten";
			this.TimeWritten.HeaderText = "Time Written";
			this.TimeWritten.Name = "TimeWritten";
			this.TimeWritten.ReadOnly = true;
			// 
			// Category
			// 
			this.Category.DataPropertyName = "Category";
			this.Category.HeaderText = "Category";
			this.Category.Name = "Category";
			this.Category.ReadOnly = true;
			// 
			// Source
			// 
			this.Source.DataPropertyName = "Source";
			this.Source.HeaderText = "Source";
			this.Source.Name = "Source";
			this.Source.ReadOnly = true;
			// 
			// EventID
			// 
			this.EventID.DataPropertyName = "EventID";
			this.EventID.HeaderText = "Event ID";
			this.EventID.Name = "EventID";
			this.EventID.ReadOnly = true;
			// 
			// Message
			// 
			this.Message.DataPropertyName = "Message";
			this.Message.HeaderText = "Message";
			this.Message.Name = "Message";
			this.Message.ReadOnly = true;
			this.Message.Visible = false;
			// 
			// buttonsToolStrip
			// 
			this.buttonsToolStrip.Dock = System.Windows.Forms.DockStyle.None;
			this.buttonsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonRefresh,
            this.toolStripButtonAutoRefresh,
            this.toolStripSeparator3,
            this.toolStripButtonClearEventLog,
            this.toolStripSeparator2,
            this.btnInformation,
            this.btnWarnings,
            this.btnErrors,
            this.btnSuccessAudit,
            this.btnFailureAudit,
            this.toolStripSeparator1});
			this.buttonsToolStrip.Location = new System.Drawing.Point(3, 0);
			this.buttonsToolStrip.Name = "buttonsToolStrip";
			this.buttonsToolStrip.Size = new System.Drawing.Size(512, 25);
			this.buttonsToolStrip.TabIndex = 0;
			this.buttonsToolStrip.Text = "toolStrip1";
			// 
			// toolStripButtonRefresh
			// 
			this.toolStripButtonRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRefresh.Image = global::EventMonitor.Properties.Resources.arrow_refresh_small;
			this.toolStripButtonRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
			this.toolStripButtonRefresh.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonRefresh.Text = "toolStripButton1";
			this.toolStripButtonRefresh.ToolTipText = "Refresh";
			this.toolStripButtonRefresh.Click += new System.EventHandler(this.toolStripButtonRefresh_Click);
			// 
			// toolStripButtonAutoRefresh
			// 
			this.toolStripButtonAutoRefresh.CheckOnClick = true;
			this.toolStripButtonAutoRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonAutoRefresh.Image = global::EventMonitor.Properties.Resources.arrow_refresh;
			this.toolStripButtonAutoRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonAutoRefresh.Name = "toolStripButtonAutoRefresh";
			this.toolStripButtonAutoRefresh.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonAutoRefresh.Text = "Auto Refresh";
			this.toolStripButtonAutoRefresh.ToolTipText = "Enable/Disable Auto Refresh";
			this.toolStripButtonAutoRefresh.Visible = false;
			this.toolStripButtonAutoRefresh.Click += new System.EventHandler(this.toolStripButtonAutoRefresh_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButtonClearEventLog
			// 
			this.toolStripButtonClearEventLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonClearEventLog.Image = global::EventMonitor.Properties.Resources.flag_orange;
			this.toolStripButtonClearEventLog.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonClearEventLog.Name = "toolStripButtonClearEventLog";
			this.toolStripButtonClearEventLog.Size = new System.Drawing.Size(23, 22);
			this.toolStripButtonClearEventLog.Text = "Clear Event Log";
			this.toolStripButtonClearEventLog.Click += new System.EventHandler(this.toolStripButtonClearEventLog_Click_1);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// btnInformation
			// 
			this.btnInformation.Checked = true;
			this.btnInformation.CheckOnClick = true;
			this.btnInformation.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnInformation.Image = global::EventMonitor.Properties.Resources.information;
			this.btnInformation.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnInformation.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnInformation.Name = "btnInformation";
			this.btnInformation.Size = new System.Drawing.Size(92, 22);
			this.btnInformation.Text = "0 Information";
			this.btnInformation.ToolTipText = "0 Messages";
			this.btnInformation.CheckedChanged += new System.EventHandler(this.On_FilterButton_CheckChanged);
			// 
			// btnWarnings
			// 
			this.btnWarnings.Checked = true;
			this.btnWarnings.CheckOnClick = true;
			this.btnWarnings.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnWarnings.Image = global::EventMonitor.Properties.Resources.warning;
			this.btnWarnings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnWarnings.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnWarnings.Name = "btnWarnings";
			this.btnWarnings.Size = new System.Drawing.Size(81, 22);
			this.btnWarnings.Text = "0 Warnings";
			this.btnWarnings.ToolTipText = "0 Warnings";
			this.btnWarnings.CheckedChanged += new System.EventHandler(this.On_FilterButton_CheckChanged);
			// 
			// btnErrors
			// 
			this.btnErrors.Checked = true;
			this.btnErrors.CheckOnClick = true;
			this.btnErrors.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnErrors.Image = global::EventMonitor.Properties.Resources.cancel;
			this.btnErrors.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnErrors.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnErrors.Name = "btnErrors";
			this.btnErrors.Size = new System.Drawing.Size(65, 22);
			this.btnErrors.Text = "0 Errors";
			this.btnErrors.ToolTipText = "0 Errors";
			this.btnErrors.CheckedChanged += new System.EventHandler(this.On_FilterButton_CheckChanged);
			// 
			// btnSuccessAudit
			// 
			this.btnSuccessAudit.Checked = true;
			this.btnSuccessAudit.CheckOnClick = true;
			this.btnSuccessAudit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnSuccessAudit.Image = global::EventMonitor.Properties.Resources.key;
			this.btnSuccessAudit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnSuccessAudit.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSuccessAudit.Name = "btnSuccessAudit";
			this.btnSuccessAudit.Size = new System.Drawing.Size(102, 22);
			this.btnSuccessAudit.Text = "0 Success Audit";
			this.btnSuccessAudit.ToolTipText = "0 Success Audit";
			this.btnSuccessAudit.CheckedChanged += new System.EventHandler(this.On_FilterButton_CheckChanged);
			// 
			// btnFailureAudit
			// 
			this.btnFailureAudit.Checked = true;
			this.btnFailureAudit.CheckOnClick = true;
			this.btnFailureAudit.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnFailureAudit.Image = global::EventMonitor.Properties.Resources.silk_lock;
			this.btnFailureAudit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnFailureAudit.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnFailureAudit.Name = "btnFailureAudit";
			this.btnFailureAudit.Size = new System.Drawing.Size(96, 22);
			this.btnFailureAudit.Text = "0 Failure Audit";
			this.btnFailureAudit.ToolTipText = "0 Failure Audit";
			this.btnFailureAudit.CheckedChanged += new System.EventHandler(this.On_FilterButton_CheckChanged);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// tsFilterSearch
			// 
			this.tsFilterSearch.Dock = System.Windows.Forms.DockStyle.None;
			this.tsFilterSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripLabel5,
            this.dlFilterSource,
            this.FindSeparator,
            this.ToolStripLabel3,
            this.tbSearch,
            this.btnSearch,
            this.btnClearSearch});
			this.tsFilterSearch.Location = new System.Drawing.Point(3, 25);
			this.tsFilterSearch.Name = "tsFilterSearch";
			this.tsFilterSearch.Size = new System.Drawing.Size(575, 25);
			this.tsFilterSearch.TabIndex = 1;
			// 
			// ToolStripLabel5
			// 
			this.ToolStripLabel5.Name = "ToolStripLabel5";
			this.ToolStripLabel5.Size = new System.Drawing.Size(74, 22);
			this.ToolStripLabel5.Text = "Filter Source :";
			// 
			// dlFilterSource
			// 
			this.dlFilterSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.dlFilterSource.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.dlFilterSource.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.dlFilterSource.Items.AddRange(new object[] {
            " "});
			this.dlFilterSource.Margin = new System.Windows.Forms.Padding(1, 0, 2, 0);
			this.dlFilterSource.Name = "dlFilterSource";
			this.dlFilterSource.Size = new System.Drawing.Size(200, 25);
			this.dlFilterSource.Sorted = true;
			this.dlFilterSource.SelectedIndexChanged += new System.EventHandler(this.dlFilterSource_SelectedIndexChanged);
			// 
			// FindSeparator
			// 
			this.FindSeparator.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
			this.FindSeparator.Name = "FindSeparator";
			this.FindSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// ToolStripLabel3
			// 
			this.ToolStripLabel3.Name = "ToolStripLabel3";
			this.ToolStripLabel3.Size = new System.Drawing.Size(97, 22);
			this.ToolStripLabel3.Text = "Search Messages :";
			// 
			// tbSearch
			// 
			this.tbSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.tbSearch.ForeColor = System.Drawing.SystemColors.WindowText;
			this.tbSearch.Name = "tbSearch";
			this.tbSearch.Padding = new System.Windows.Forms.Padding(2, 0, 0, 0);
			this.tbSearch.Size = new System.Drawing.Size(136, 25);
			this.tbSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbSearch_KeyPress);
			this.tbSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tbSearch_KeyUp);
			// 
			// btnSearch
			// 
			this.btnSearch.CheckOnClick = true;
			this.btnSearch.Image = global::EventMonitor.Properties.Resources.script_code;
			this.btnSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnSearch.Name = "btnSearch";
			this.btnSearch.Size = new System.Drawing.Size(23, 22);
			this.btnSearch.ToolTipText = "Search";
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			// 
			// btnClearSearch
			// 
			this.btnClearSearch.Checked = true;
			this.btnClearSearch.CheckOnClick = true;
			this.btnClearSearch.CheckState = System.Windows.Forms.CheckState.Checked;
			this.btnClearSearch.Image = global::EventMonitor.Properties.Resources.script_code_red;
			this.btnClearSearch.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.btnClearSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btnClearSearch.Name = "btnClearSearch";
			this.btnClearSearch.Size = new System.Drawing.Size(23, 22);
			this.btnClearSearch.ToolTipText = "Clear Search";
			this.btnClearSearch.Click += new System.EventHandler(this.btnClearSearch_Click);
			// 
			// tbEventDetail
			// 
			this.tbEventDetail.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tbEventDetail.Location = new System.Drawing.Point(0, 0);
			this.tbEventDetail.Name = "tbEventDetail";
			this.tbEventDetail.ReadOnly = true;
			this.tbEventDetail.Size = new System.Drawing.Size(726, 135);
			this.tbEventDetail.TabIndex = 2;
			this.tbEventDetail.Text = "";
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btnCopy);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.panel1.Location = new System.Drawing.Point(726, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(37, 135);
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
			// EventLogViewer
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(763, 558);
			this.Controls.Add(this.splitContainer);			
			this.Name = "EventLogViewer";
			this.Text = "Form1";
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EventLogViewer_KeyUp);
			this.Load += new System.EventHandler(this.EventLogViewer_Load);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.ResumeLayout(false);
			this.toolStripContainer1.ContentPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer1.TopToolStripPanel.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgEvents)).EndInit();
			this.buttonsToolStrip.ResumeLayout(false);
			this.buttonsToolStrip.PerformLayout();
			this.tsFilterSearch.ResumeLayout(false);
			this.tsFilterSearch.PerformLayout();
			this.panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.eventLogEntryBindingSource)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.BindingSource eventLogEntryBindingSource;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.DataGridView dgEvents;
		private System.Windows.Forms.ToolStrip buttonsToolStrip;
		private System.Windows.Forms.ToolStripButton toolStripButtonClearEventLog;
		internal System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		internal System.Windows.Forms.ToolStripButton btnInformation;
		internal System.Windows.Forms.ToolStripButton btnWarnings;
		internal System.Windows.Forms.ToolStripButton btnErrors;
		internal System.Windows.Forms.ToolStripButton btnSuccessAudit;
		internal System.Windows.Forms.ToolStripButton btnFailureAudit;
		internal System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStrip tsFilterSearch;
		internal System.Windows.Forms.ToolStripLabel ToolStripLabel5;
		internal System.Windows.Forms.ToolStripComboBox dlFilterSource;
		internal System.Windows.Forms.ToolStripSeparator FindSeparator;
		internal System.Windows.Forms.ToolStripLabel ToolStripLabel3;
		internal System.Windows.Forms.ToolStripTextBox tbSearch;
		private System.Windows.Forms.ToolStripButton toolStripButtonRefresh;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		internal System.Windows.Forms.ToolStripButton btnSearch;
		internal System.Windows.Forms.ToolStripButton btnClearSearch;
		private System.Windows.Forms.DataGridViewImageColumn TypeImage;
		private System.Windows.Forms.DataGridViewTextBoxColumn EntryType;
		private System.Windows.Forms.DataGridViewTextBoxColumn TimeWritten;
		private System.Windows.Forms.DataGridViewTextBoxColumn Category;
		private System.Windows.Forms.DataGridViewTextBoxColumn Source;
		private System.Windows.Forms.DataGridViewTextBoxColumn EventID;
		private System.Windows.Forms.DataGridViewTextBoxColumn Message;
		private System.Windows.Forms.ToolStripButton toolStripButtonAutoRefresh;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RichTextBox tbEventDetail;
		private System.Windows.Forms.Button btnCopy;
	}
}