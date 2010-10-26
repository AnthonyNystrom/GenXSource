namespace CommandManager
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this._toolstripManager = new Genetibase.SmoothControls.NuGenSmoothToolStripManager();
			this._bkgndPanel = new Genetibase.SmoothControls.NuGenSmoothPanel();
			this._contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this._statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this._progressBar = new Genetibase.SmoothControls.SmoothProgressBar();
			this._showSaveCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._enableLocalCopyCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._enableGlobalCopyCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._enableNewCheckBox = new Genetibase.SmoothControls.NuGenSmoothCheckBox();
			this._toolStrip = new System.Windows.Forms.ToolStrip();
			this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.copyToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.pasteToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
			this._trackBar = new Genetibase.SmoothControls.SmoothTrackBar();
			this._menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.printPreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.customizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.contentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.indexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this._commandManager = new Genetibase.Shared.Controls.NuGenCommandManager2Ex(this.components);
			this.commandContextCopy = new Genetibase.Shared.Controls.NuGenApplicationCommand(this.components);
			this.commandNew = new Genetibase.Shared.Controls.NuGenApplicationCommand(this.components);
			this.commandSave = new Genetibase.Shared.Controls.NuGenApplicationCommand(this.components);
			this.commandFileCopy = new Genetibase.Shared.Controls.NuGenApplicationCommand(this.components);
			this.commandTrackBar = new Genetibase.Shared.Controls.NuGenApplicationCommand(this.components);
			this.commandExit = new Genetibase.Shared.Controls.NuGenApplicationCommand(this.components);
			this._bkgndPanel.SuspendLayout();
			this._contextMenuStrip.SuspendLayout();
			this._statusStrip.SuspendLayout();
			this._toolStrip.SuspendLayout();
			this._menuStrip.SuspendLayout();
			this.toolStripContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// _bkgndPanel
			// 
			this._bkgndPanel.ContextMenuStrip = this._contextMenuStrip;
			this._bkgndPanel.Controls.Add(this._statusStrip);
			this._bkgndPanel.Controls.Add(this._showSaveCheckBox);
			this._bkgndPanel.Controls.Add(this._enableLocalCopyCheckBox);
			this._bkgndPanel.Controls.Add(this._enableGlobalCopyCheckBox);
			this._bkgndPanel.Controls.Add(this._enableNewCheckBox);
			this._bkgndPanel.Controls.Add(this._toolStrip);
			this._bkgndPanel.Controls.Add(this._menuStrip);
			this._bkgndPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this._bkgndPanel.DrawBorder = false;
			this._bkgndPanel.Location = new System.Drawing.Point(0, 0);
			this._bkgndPanel.Name = "_bkgndPanel";
			this._bkgndPanel.Size = new System.Drawing.Size(322, 266);
			// 
			// _contextMenuStrip
			// 
			this._contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1});
			this._contextMenuStrip.Name = "_contextMenuStrip";
			this._contextMenuStrip.Size = new System.Drawing.Size(111, 26);
			// 
			// copyToolStripMenuItem1
			// 
			this._commandManager.SetApplicationCommand(this.copyToolStripMenuItem1, this.commandContextCopy);
			this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
			this.copyToolStripMenuItem1.Size = new System.Drawing.Size(110, 22);
			this.copyToolStripMenuItem1.Text = "Copy";
			// 
			// _statusStrip
			// 
			this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this._progressBar});
			this._statusStrip.Location = new System.Drawing.Point(0, 244);
			this._statusStrip.Name = "_statusStrip";
			this._statusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
			this._statusStrip.Size = new System.Drawing.Size(322, 22);
			this._statusStrip.TabIndex = 4;
			this._statusStrip.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this._commandManager.SetApplicationCommand(this.toolStripStatusLabel1, null);
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(49, 17);
			this.toolStripStatusLabel1.Text = "Progress";
			// 
			// _progressBar
			// 
			this._progressBar.MarqueeAnimationSpeed = 100;
			this._progressBar.Maximum = 100;
			this._progressBar.Minimum = 0;
			this._progressBar.Name = "_progressBar";
			this._progressBar.Padding = new System.Windows.Forms.Padding(1);
			this._progressBar.Size = new System.Drawing.Size(100, 20);
			this._progressBar.Step = 10;
			this._progressBar.Style = Genetibase.Shared.Controls.NuGenProgressBarStyle.Continuous;
			this._progressBar.Value = 0;
			// 
			// _showSaveCheckBox
			// 
			this._showSaveCheckBox.AutoSize = false;
			this._showSaveCheckBox.Location = new System.Drawing.Point(12, 136);
			this._showSaveCheckBox.Name = "_showSaveCheckBox";
			this._showSaveCheckBox.Size = new System.Drawing.Size(268, 24);
			this._showSaveCheckBox.TabIndex = 3;
			this._showSaveCheckBox.Text = "Show Save";
			this._showSaveCheckBox.UseVisualStyleBackColor = false;
			// 
			// _enableLocalCopyCheckBox
			// 
			this._enableLocalCopyCheckBox.AutoSize = false;
			this._enableLocalCopyCheckBox.Location = new System.Drawing.Point(12, 113);
			this._enableLocalCopyCheckBox.Name = "_enableLocalCopyCheckBox";
			this._enableLocalCopyCheckBox.Size = new System.Drawing.Size(268, 24);
			this._enableLocalCopyCheckBox.TabIndex = 3;
			this._enableLocalCopyCheckBox.Text = "Enable Local Copy";
			this._enableLocalCopyCheckBox.UseVisualStyleBackColor = false;
			// 
			// _enableGlobalCopyCheckBox
			// 
			this._enableGlobalCopyCheckBox.AutoSize = false;
			this._enableGlobalCopyCheckBox.Location = new System.Drawing.Point(12, 90);
			this._enableGlobalCopyCheckBox.Name = "_enableGlobalCopyCheckBox";
			this._enableGlobalCopyCheckBox.Size = new System.Drawing.Size(268, 24);
			this._enableGlobalCopyCheckBox.TabIndex = 3;
			this._enableGlobalCopyCheckBox.Text = "Enable Global Copy";
			this._enableGlobalCopyCheckBox.UseVisualStyleBackColor = false;
			// 
			// _enableNewCheckBox
			// 
			this._enableNewCheckBox.AutoSize = false;
			this._enableNewCheckBox.Location = new System.Drawing.Point(12, 67);
			this._enableNewCheckBox.Name = "_enableNewCheckBox";
			this._enableNewCheckBox.Size = new System.Drawing.Size(268, 24);
			this._enableNewCheckBox.TabIndex = 2;
			this._enableNewCheckBox.Text = "Enable New";
			this._enableNewCheckBox.UseVisualStyleBackColor = false;
			// 
			// _toolStrip
			// 
			this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator6,
            this.cutToolStripButton,
            this.copyToolStripButton,
            this.pasteToolStripButton,
            this.toolStripSeparator7,
            this.helpToolStripButton,
            this._trackBar});
			this._toolStrip.Location = new System.Drawing.Point(0, 24);
			this._toolStrip.Name = "_toolStrip";
			this._toolStrip.Size = new System.Drawing.Size(322, 33);
			this._toolStrip.TabIndex = 0;
			this._toolStrip.Text = "toolStrip1";
			// 
			// newToolStripButton
			// 
			this._commandManager.SetApplicationCommand(this.newToolStripButton, this.commandNew);
			this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
			this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripButton.Name = "newToolStripButton";
			this.newToolStripButton.Size = new System.Drawing.Size(23, 30);
			this.newToolStripButton.Text = "&New";
			// 
			// openToolStripButton
			// 
			this._commandManager.SetApplicationCommand(this.openToolStripButton, null);
			this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
			this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripButton.Name = "openToolStripButton";
			this.openToolStripButton.Size = new System.Drawing.Size(23, 30);
			this.openToolStripButton.Text = "&Open";
			// 
			// saveToolStripButton
			// 
			this._commandManager.SetApplicationCommand(this.saveToolStripButton, this.commandSave);
			this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
			this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripButton.Name = "saveToolStripButton";
			this.saveToolStripButton.Size = new System.Drawing.Size(23, 30);
			this.saveToolStripButton.Text = "&Save";
			// 
			// printToolStripButton
			// 
			this._commandManager.SetApplicationCommand(this.printToolStripButton, null);
			this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
			this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printToolStripButton.Name = "printToolStripButton";
			this.printToolStripButton.Size = new System.Drawing.Size(23, 30);
			this.printToolStripButton.Text = "&Print";
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(6, 33);
			// 
			// cutToolStripButton
			// 
			this._commandManager.SetApplicationCommand(this.cutToolStripButton, null);
			this.cutToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cutToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripButton.Image")));
			this.cutToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripButton.Name = "cutToolStripButton";
			this.cutToolStripButton.Size = new System.Drawing.Size(23, 30);
			this.cutToolStripButton.Text = "C&ut";
			// 
			// copyToolStripButton
			// 
			this._commandManager.SetApplicationCommand(this.copyToolStripButton, this.commandFileCopy);
			this.copyToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.copyToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripButton.Image")));
			this.copyToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripButton.Name = "copyToolStripButton";
			this.copyToolStripButton.Size = new System.Drawing.Size(23, 30);
			this.copyToolStripButton.Text = "&Copy";
			// 
			// pasteToolStripButton
			// 
			this._commandManager.SetApplicationCommand(this.pasteToolStripButton, null);
			this.pasteToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.pasteToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripButton.Image")));
			this.pasteToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripButton.Name = "pasteToolStripButton";
			this.pasteToolStripButton.Size = new System.Drawing.Size(23, 30);
			this.pasteToolStripButton.Text = "&Paste";
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(6, 33);
			// 
			// helpToolStripButton
			// 
			this._commandManager.SetApplicationCommand(this.helpToolStripButton, null);
			this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
			this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.helpToolStripButton.Name = "helpToolStripButton";
			this.helpToolStripButton.Size = new System.Drawing.Size(23, 30);
			this.helpToolStripButton.Text = "He&lp";
			// 
			// _trackBar
			// 
			this._commandManager.SetApplicationCommand(this._trackBar, this.commandTrackBar);
			this._trackBar.LargeChange = 5;
			this._trackBar.Maximum = 100;
			this._trackBar.Minimum = 0;
			this._trackBar.Name = "_trackBar";
			this._trackBar.Size = new System.Drawing.Size(100, 30);
			this._trackBar.SmallChange = 10;
			this._trackBar.TickStyle = System.Windows.Forms.TickStyle.BottomRight;
			this._trackBar.Value = 0;
			// 
			// _menuStrip
			// 
			this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this._menuStrip.Location = new System.Drawing.Point(0, 0);
			this._menuStrip.Name = "_menuStrip";
			this._menuStrip.Size = new System.Drawing.Size(322, 24);
			this._menuStrip.TabIndex = 1;
			this._menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.fileToolStripMenuItem, null);
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator1,
            this.printToolStripMenuItem,
            this.printPreviewToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.newToolStripMenuItem, this.commandNew);
			this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
			this.newToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.newToolStripMenuItem.Text = "&New";
			// 
			// openToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.openToolStripMenuItem, null);
			this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
			this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.openToolStripMenuItem.Text = "&Open";
			// 
			// toolStripSeparator
			// 
			this.toolStripSeparator.Name = "toolStripSeparator";
			this.toolStripSeparator.Size = new System.Drawing.Size(148, 6);
			// 
			// saveToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.saveToolStripMenuItem, this.commandSave);
			this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
			this.saveToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			// 
			// saveAsToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.saveAsToolStripMenuItem, null);
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(148, 6);
			// 
			// printToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.printToolStripMenuItem, null);
			this.printToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripMenuItem.Image")));
			this.printToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printToolStripMenuItem.Name = "printToolStripMenuItem";
			this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
			this.printToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.printToolStripMenuItem.Text = "&Print";
			// 
			// printPreviewToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.printPreviewToolStripMenuItem, null);
			this.printPreviewToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("printPreviewToolStripMenuItem.Image")));
			this.printPreviewToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.printPreviewToolStripMenuItem.Name = "printPreviewToolStripMenuItem";
			this.printPreviewToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.printPreviewToolStripMenuItem.Text = "Print Pre&view";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(148, 6);
			// 
			// exitToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.exitToolStripMenuItem, this.commandExit);
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			// 
			// editToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.editToolStripMenuItem, null);
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator3,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.toolStripSeparator4,
            this.selectAllToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// undoToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.undoToolStripMenuItem, null);
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.undoToolStripMenuItem.Text = "&Undo";
			// 
			// redoToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.redoToolStripMenuItem, null);
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.redoToolStripMenuItem.Text = "&Redo";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(147, 6);
			// 
			// cutToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.cutToolStripMenuItem, null);
			this.cutToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("cutToolStripMenuItem.Image")));
			this.cutToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
			this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.cutToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.cutToolStripMenuItem.Text = "Cu&t";
			// 
			// copyToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.copyToolStripMenuItem, this.commandFileCopy);
			this.copyToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToolStripMenuItem.Image")));
			this.copyToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
			this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.copyToolStripMenuItem.Text = "&Copy";
			// 
			// pasteToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.pasteToolStripMenuItem, null);
			this.pasteToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("pasteToolStripMenuItem.Image")));
			this.pasteToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
			this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
			this.pasteToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.pasteToolStripMenuItem.Text = "&Paste";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(147, 6);
			// 
			// selectAllToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.selectAllToolStripMenuItem, null);
			this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
			this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
			this.selectAllToolStripMenuItem.Text = "Select &All";
			// 
			// toolsToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.toolsToolStripMenuItem, null);
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customizeToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.toolsToolStripMenuItem.Text = "&Tools";
			// 
			// customizeToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.customizeToolStripMenuItem, null);
			this.customizeToolStripMenuItem.Name = "customizeToolStripMenuItem";
			this.customizeToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.customizeToolStripMenuItem.Text = "&Customize";
			// 
			// optionsToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.optionsToolStripMenuItem, null);
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
			this.optionsToolStripMenuItem.Text = "&Options";
			// 
			// helpToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.helpToolStripMenuItem, null);
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contentsToolStripMenuItem,
            this.indexToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.toolStripSeparator5,
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// contentsToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.contentsToolStripMenuItem, null);
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			this.contentsToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.contentsToolStripMenuItem.Text = "&Contents";
			// 
			// indexToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.indexToolStripMenuItem, null);
			this.indexToolStripMenuItem.Name = "indexToolStripMenuItem";
			this.indexToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.indexToolStripMenuItem.Text = "&Index";
			// 
			// searchToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.searchToolStripMenuItem, null);
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.searchToolStripMenuItem.Text = "&Search";
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(126, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this._commandManager.SetApplicationCommand(this.aboutToolStripMenuItem, null);
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
			this.aboutToolStripMenuItem.Text = "&About...";
			// 
			// toolStripContainer1
			// 
			// 
			// toolStripContainer1.ContentPanel
			// 
			this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(322, 241);
			this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.toolStripContainer1.Name = "toolStripContainer1";
			this.toolStripContainer1.Size = new System.Drawing.Size(322, 266);
			this.toolStripContainer1.TabIndex = 0;
			this.toolStripContainer1.Text = "toolStripContainer1";
			// 
			// _commandManager
			// 
			this._commandManager.ApplicationCommands.AddRange(new Genetibase.Shared.Controls.NuGenApplicationCommand[] {
            this.commandExit,
            this.commandFileCopy,
            this.commandTrackBar,
            this.commandNew,
            this.commandContextCopy,
            this.commandSave});
			this._commandManager.ContextMenus.Add(this._contextMenuStrip);
			this._commandManager.ApplicationCommandUpdate += new System.EventHandler<Genetibase.Shared.Controls.NuGenApplicationCommandEventArgs>(this._commandManager_ApplicationCommandUpdate);
			// 
			// commandContextCopy
			// 
			this.commandContextCopy.ApplicationCommandName = "ContextCopy";
			this.commandContextCopy.CommandManager = this._commandManager;
			this.commandContextCopy.Description = null;
			this.commandContextCopy.UIItems.Add(this.copyToolStripMenuItem1);
			// 
			// commandNew
			// 
			this.commandNew.ApplicationCommandName = "New";
			this.commandNew.CommandManager = this._commandManager;
			this.commandNew.Description = null;
			this.commandNew.UIItems.Add(this.newToolStripMenuItem);
			this.commandNew.UIItems.Add(this.newToolStripButton);
			// 
			// commandSave
			// 
			this.commandSave.ApplicationCommandName = "Save";
			this.commandSave.CommandManager = this._commandManager;
			this.commandSave.Description = null;
			this.commandSave.UIItems.Add(this.saveToolStripMenuItem);
			this.commandSave.UIItems.Add(this.saveToolStripButton);
			// 
			// commandFileCopy
			// 
			this.commandFileCopy.ApplicationCommandName = "FileCopy";
			this.commandFileCopy.CommandManager = this._commandManager;
			this.commandFileCopy.Description = null;
			this.commandFileCopy.UIItems.Add(this.copyToolStripMenuItem);
			this.commandFileCopy.UIItems.Add(this.copyToolStripButton);
			// 
			// commandTrackBar
			// 
			this.commandTrackBar.ApplicationCommandName = "TrackBar";
			this.commandTrackBar.CommandManager = this._commandManager;
			this.commandTrackBar.Description = null;
			this.commandTrackBar.UIItems.Add(this._trackBar);
			// 
			// commandExit
			// 
			this.commandExit.ApplicationCommandName = "Exit";
			this.commandExit.CommandManager = this._commandManager;
			this.commandExit.Description = null;
			this.commandExit.UIItems.Add(this.exitToolStripMenuItem);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(322, 266);
			this.Controls.Add(this._bkgndPanel);
			this.Controls.Add(this.toolStripContainer1);
			this.MainMenuStrip = this._menuStrip;
			this.Name = "MainForm";
			this.Text = "CommandManager";
			this._bkgndPanel.ResumeLayout(false);
			this._bkgndPanel.PerformLayout();
			this._contextMenuStrip.ResumeLayout(false);
			this._statusStrip.ResumeLayout(false);
			this._statusStrip.PerformLayout();
			this._toolStrip.ResumeLayout(false);
			this._toolStrip.PerformLayout();
			this._menuStrip.ResumeLayout(false);
			this._menuStrip.PerformLayout();
			this.toolStripContainer1.ResumeLayout(false);
			this.toolStripContainer1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private Genetibase.SmoothControls.NuGenSmoothToolStripManager _toolstripManager;
		private Genetibase.SmoothControls.NuGenSmoothPanel _bkgndPanel;
		private System.Windows.Forms.StatusStrip _statusStrip;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private Genetibase.SmoothControls.SmoothProgressBar _progressBar;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _showSaveCheckBox;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _enableLocalCopyCheckBox;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _enableGlobalCopyCheckBox;
		private Genetibase.SmoothControls.NuGenSmoothCheckBox _enableNewCheckBox;
		private System.Windows.Forms.ToolStrip _toolStrip;
		private System.Windows.Forms.ToolStripButton newToolStripButton;
		private System.Windows.Forms.ToolStripButton openToolStripButton;
		private System.Windows.Forms.ToolStripButton saveToolStripButton;
		private System.Windows.Forms.ToolStripButton printToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripButton cutToolStripButton;
		private System.Windows.Forms.ToolStripButton copyToolStripButton;
		private System.Windows.Forms.ToolStripButton pasteToolStripButton;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripButton helpToolStripButton;
		private System.Windows.Forms.MenuStrip _menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem customizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem contentsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem indexToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripContainer toolStripContainer1;
		private System.Windows.Forms.ContextMenuStrip _contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
		private Genetibase.SmoothControls.SmoothTrackBar _trackBar;
		private Genetibase.Shared.Controls.NuGenCommandManager2Ex _commandManager;
		private Genetibase.Shared.Controls.NuGenApplicationCommand commandExit;
		private Genetibase.Shared.Controls.NuGenApplicationCommand commandFileCopy;
		private Genetibase.Shared.Controls.NuGenApplicationCommand commandTrackBar;
		private Genetibase.Shared.Controls.NuGenApplicationCommand commandNew;
		private Genetibase.Shared.Controls.NuGenApplicationCommand commandContextCopy;
		private Genetibase.Shared.Controls.NuGenApplicationCommand commandSave;
	}
}

