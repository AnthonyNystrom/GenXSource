using Netron.Diagramming.Core;
namespace Netron.Cobalt
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
            this.stripContainer = new System.Windows.Forms.ToolStripContainer();
            this.StatusStrip = new System.Windows.Forms.StatusStrip();
            this.InfoStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StatusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.DockPanel = new Netron.Neon.WinFormsUI.DockPanel();
            this.RootMenuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.diagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shapesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.outputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.applicationSettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BrowserMainMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backwardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.netronGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aPIDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aPISamplesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DiagramMainMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDiagramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.netronEULAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classDocumentationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusImageList = new System.Windows.Forms.ImageList(this.components);
            this.StatusTimer = new System.Windows.Forms.Timer(this.components);
            this.stripContainer.BottomToolStripPanel.SuspendLayout();
            this.stripContainer.ContentPanel.SuspendLayout();
            this.stripContainer.TopToolStripPanel.SuspendLayout();
            this.stripContainer.SuspendLayout();
            this.StatusStrip.SuspendLayout();
            this.RootMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // stripContainer
            // 
            // 
            // stripContainer.BottomToolStripPanel
            // 
            this.stripContainer.BottomToolStripPanel.Controls.Add(this.StatusStrip);
            // 
            // stripContainer.ContentPanel
            // 
            this.stripContainer.ContentPanel.AutoScroll = true;
            this.stripContainer.ContentPanel.Controls.Add(this.DockPanel);
            this.stripContainer.ContentPanel.Size = new System.Drawing.Size(891, 660);
            this.stripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stripContainer.Location = new System.Drawing.Point(0, 0);
            this.stripContainer.Name = "stripContainer";
            this.stripContainer.Size = new System.Drawing.Size(891, 706);
            this.stripContainer.TabIndex = 1;
            this.stripContainer.Text = "toolStripContainer1";
            // 
            // stripContainer.TopToolStripPanel
            // 
            this.stripContainer.TopToolStripPanel.Controls.Add(this.RootMenuStrip);
            // 
            // StatusStrip
            // 
            this.StatusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InfoStatusLabel,
            this.StatusProgressBar});
            this.StatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.StatusStrip.Location = new System.Drawing.Point(0, 0);
            this.StatusStrip.Name = "StatusStrip";
            this.StatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.StatusStrip.ShowItemToolTips = true;
            this.StatusStrip.Size = new System.Drawing.Size(891, 22);
            this.StatusStrip.TabIndex = 1;
            this.StatusStrip.Text = "statusStrip1";
            // 
            // InfoStatusLabel
            // 
            this.InfoStatusLabel.Image = global::Netron.Cobalt.Properties.Resources.info;
            this.InfoStatusLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.InfoStatusLabel.Name = "InfoStatusLabel";
            this.InfoStatusLabel.Size = new System.Drawing.Size(16, 17);
            this.InfoStatusLabel.Visible = false;
            // 
            // StatusProgressBar
            // 
            this.StatusProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.StatusProgressBar.AutoToolTip = true;
            this.StatusProgressBar.Name = "StatusProgressBar";
            this.StatusProgressBar.Size = new System.Drawing.Size(150, 16);
            this.StatusProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.StatusProgressBar.Visible = false;
            // 
            // DockPanel
            // 
            this.DockPanel.ActiveAutoHideContent = null;
            this.DockPanel.BackColor = System.Drawing.Color.WhiteSmoke;
            this.DockPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DockPanel.DocumentStyle = Netron.Neon.WinFormsUI.DocumentStyles.DockingWindow;
            this.DockPanel.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.DockPanel.Location = new System.Drawing.Point(0, 0);
            this.DockPanel.Name = "DockPanel";
            this.DockPanel.Size = new System.Drawing.Size(891, 660);
            this.DockPanel.TabIndex = 6;
            this.DockPanel.ActiveDocumentChanged += new System.EventHandler(this.dockPanel_ActiveDocumentChanged);
            // 
            // RootMenuStrip
            // 
            this.RootMenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.RootMenuStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.RootMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.EditToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.BrowserMainMenu,
            this.DiagramMainMenuItem,
            this.helpToolStripMenuItem});
            this.RootMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.RootMenuStrip.Name = "RootMenuStrip";
            this.RootMenuStrip.ShowItemToolTips = true;
            this.RootMenuStrip.Size = new System.Drawing.Size(891, 24);
            this.RootMenuStrip.TabIndex = 0;
            this.RootMenuStrip.Text = "Main menu";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitMenuItem});
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileMenuItem.Text = "&File";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(103, 22);
            this.exitMenuItem.Text = "E&xit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // EditToolStripMenuItem
            // 
            this.EditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoToolStripMenuItem,
            this.RedoToolStripMenuItem,
            this.toolStripMenuItem2,
            this.cutToolStripMenuItem,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.deleteToolStripMenuItem});
            this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            this.EditToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.EditToolStripMenuItem.Text = "&Edit";
            // 
            // UndoToolStripMenuItem
            // 
            this.UndoToolStripMenuItem.Enabled = false;
            this.UndoToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.Edit_UndoHS;
            this.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem";
            this.UndoToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.UndoToolStripMenuItem.Text = "&Undo";
            this.UndoToolStripMenuItem.Click += new System.EventHandler(this.undoButton_Click);
            // 
            // RedoToolStripMenuItem
            // 
            this.RedoToolStripMenuItem.Enabled = false;
            this.RedoToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.Edit_RedoHS;
            this.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem";
            this.RedoToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.RedoToolStripMenuItem.Text = "&Redo";
            this.RedoToolStripMenuItem.Click += new System.EventHandler(this.redoButton_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(113, 6);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Enabled = false;
            this.cutToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.CutHS;
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Enabled = false;
            this.copyToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.CopyHS;
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Enabled = false;
            this.pasteToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.PasteHS;
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.DeleteHS;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.deleteToolStripMenuItem.Text = "Delete";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.diagramToolStripMenuItem,
            this.shapesToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.browserToolStripMenuItem,
            this.outputToolStripMenuItem,
            this.toolStripMenuItem5,
            this.applicationSettingsMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "&View";
            // 
            // diagramToolStripMenuItem
            // 
            this.diagramToolStripMenuItem.Name = "diagramToolStripMenuItem";
            this.diagramToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.diagramToolStripMenuItem.Text = "Diagram";
            this.diagramToolStripMenuItem.Click += new System.EventHandler(this.diagramToolStripMenuItem_Click);
            // 
            // shapesToolStripMenuItem
            // 
            this.shapesToolStripMenuItem.Name = "shapesToolStripMenuItem";
            this.shapesToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.shapesToolStripMenuItem.Text = "Shapes";
            this.shapesToolStripMenuItem.Click += new System.EventHandler(this.shapesToolStripMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // browserToolStripMenuItem
            // 
            this.browserToolStripMenuItem.Name = "browserToolStripMenuItem";
            this.browserToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.browserToolStripMenuItem.Text = "Browser";
            this.browserToolStripMenuItem.Click += new System.EventHandler(this.browserToolStripMenuItem1_Click);
            // 
            // outputToolStripMenuItem
            // 
            this.outputToolStripMenuItem.Name = "outputToolStripMenuItem";
            this.outputToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.outputToolStripMenuItem.Text = "Output";
            this.outputToolStripMenuItem.Click += new System.EventHandler(this.outputToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(175, 6);
            // 
            // applicationSettingsMenuItem
            // 
            this.applicationSettingsMenuItem.Name = "applicationSettingsMenuItem";
            this.applicationSettingsMenuItem.Size = new System.Drawing.Size(178, 22);
            this.applicationSettingsMenuItem.Text = "Application settings";
            this.applicationSettingsMenuItem.Click += new System.EventHandler(this.applicationSettingsMenuItem_Click);
            // 
            // BrowserMainMenu
            // 
            this.BrowserMainMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.homeToolStripMenuItem,
            this.forwardToolStripMenuItem,
            this.backwardToolStripMenuItem,
            this.toolStripMenuItem1,
            this.netronGuideToolStripMenuItem,
            this.aPIDocumentationToolStripMenuItem,
            this.aPISamplesToolStripMenuItem});
            this.BrowserMainMenu.Name = "BrowserMainMenu";
            this.BrowserMainMenu.Size = new System.Drawing.Size(58, 20);
            this.BrowserMainMenu.Text = "Browser";
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.homeToolStripMenuItem.Text = "Home";
            // 
            // forwardToolStripMenuItem
            // 
            this.forwardToolStripMenuItem.Name = "forwardToolStripMenuItem";
            this.forwardToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.forwardToolStripMenuItem.Text = "Forward";
            // 
            // backwardToolStripMenuItem
            // 
            this.backwardToolStripMenuItem.Name = "backwardToolStripMenuItem";
            this.backwardToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.backwardToolStripMenuItem.Text = "Backward";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(173, 6);
            // 
            // netronGuideToolStripMenuItem
            // 
            this.netronGuideToolStripMenuItem.Name = "netronGuideToolStripMenuItem";
            this.netronGuideToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.netronGuideToolStripMenuItem.Text = "Netron guide";
            // 
            // aPIDocumentationToolStripMenuItem
            // 
            this.aPIDocumentationToolStripMenuItem.Name = "aPIDocumentationToolStripMenuItem";
            this.aPIDocumentationToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.aPIDocumentationToolStripMenuItem.Text = "API documentation";
            // 
            // aPISamplesToolStripMenuItem
            // 
            this.aPISamplesToolStripMenuItem.Name = "aPISamplesToolStripMenuItem";
            this.aPISamplesToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.aPISamplesToolStripMenuItem.Text = "API samples";
            // 
            // DiagramMainMenuItem
            // 
            this.DiagramMainMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveDiagramToolStripMenuItem,
            this.printToolStripMenuItem,
            this.propertiesToolStripMenuItem1});
            this.DiagramMainMenuItem.Name = "DiagramMainMenuItem";
            this.DiagramMainMenuItem.Size = new System.Drawing.Size(58, 20);
            this.DiagramMainMenuItem.Text = "Diagram";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newDiagramToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // newDiagramToolStripMenuItem
            // 
            this.newDiagramToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.DocumentHS;
            this.newDiagramToolStripMenuItem.Name = "newDiagramToolStripMenuItem";
            this.newDiagramToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.newDiagramToolStripMenuItem.Text = "Diagram";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDiagramToolStripMenuItem});
            this.openToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.openfolderHS;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.openToolStripMenuItem.Text = "&Open";
            // 
            // openDiagramToolStripMenuItem
            // 
            this.openDiagramToolStripMenuItem.Enabled = false;
            this.openDiagramToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.openfolderHS;
            this.openDiagramToolStripMenuItem.Name = "openDiagramToolStripMenuItem";
            this.openDiagramToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.openDiagramToolStripMenuItem.Text = "Diagram";
            // 
            // saveDiagramToolStripMenuItem
            // 
            this.saveDiagramToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.saveHS;
            this.saveDiagramToolStripMenuItem.Name = "saveDiagramToolStripMenuItem";
            this.saveDiagramToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.saveDiagramToolStripMenuItem.Text = "&Save diagram";
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Enabled = false;
            this.printToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.PrintPreviewHS;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.printToolStripMenuItem.Text = "&Print";
            // 
            // propertiesToolStripMenuItem1
            // 
            this.propertiesToolStripMenuItem1.Image = global::Netron.Cobalt.Properties.Resources.PropertiesHS;
            this.propertiesToolStripMenuItem1.Name = "propertiesToolStripMenuItem1";
            this.propertiesToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.propertiesToolStripMenuItem1.Text = "Properties";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.netronEULAToolStripMenuItem,
            this.classDocumentationToolStripMenuItem,
            this.toolStripMenuItem3,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // netronEULAToolStripMenuItem
            // 
            this.netronEULAToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.copyright;
            this.netronEULAToolStripMenuItem.Name = "netronEULAToolStripMenuItem";
            this.netronEULAToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.netronEULAToolStripMenuItem.Text = "Netron &EULA";
            this.netronEULAToolStripMenuItem.Click += new System.EventHandler(this.netronEULAToolStripMenuItem_Click);
            // 
            // classDocumentationToolStripMenuItem
            // 
            this.classDocumentationToolStripMenuItem.Image = global::Netron.Cobalt.Properties.Resources.Book_openHS;
            this.classDocumentationToolStripMenuItem.Name = "classDocumentationToolStripMenuItem";
            this.classDocumentationToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.classDocumentationToolStripMenuItem.Text = "Class documentation";
            this.classDocumentationToolStripMenuItem.Click += new System.EventHandler(this.classDocumentationToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(181, 6);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // statusImageList
            // 
            this.statusImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("statusImageList.ImageStream")));
            this.statusImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.statusImageList.Images.SetKeyName(0, "INFO.ICO");
            // 
            // StatusTimer
            // 
            this.StatusTimer.Interval = 5000;
            this.StatusTimer.Tick += new System.EventHandler(this.statusTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 706);
            this.Controls.Add(this.stripContainer);
            this.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.RootMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Netron Light 2006 demo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.stripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.stripContainer.BottomToolStripPanel.PerformLayout();
            this.stripContainer.ContentPanel.ResumeLayout(false);
            this.stripContainer.TopToolStripPanel.ResumeLayout(false);
            this.stripContainer.TopToolStripPanel.PerformLayout();
            this.stripContainer.ResumeLayout(false);
            this.stripContainer.PerformLayout();
            this.StatusStrip.ResumeLayout(false);
            this.StatusStrip.PerformLayout();
            this.RootMenuStrip.ResumeLayout(false);
            this.RootMenuStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer stripContainer;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem diagramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shapesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browserToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem netronEULAToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem classDocumentationToolStripMenuItem;
        private System.Windows.Forms.ImageList statusImageList;
        public System.Windows.Forms.StatusStrip StatusStrip;
        public System.Windows.Forms.ToolStripProgressBar StatusProgressBar;
        public System.Windows.Forms.ToolStripMenuItem BrowserMainMenu;
        private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem applicationSettingsMenuItem;
        public System.Windows.Forms.ToolStripMenuItem EditToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem UndoToolStripMenuItem;
        public System.Windows.Forms.ToolStripMenuItem RedoToolStripMenuItem;
        public System.Windows.Forms.ToolStripStatusLabel InfoStatusLabel;
        public System.Windows.Forms.Timer StatusTimer;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDiagramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDiagramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveDiagramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem1;
        public System.Windows.Forms.ToolStripMenuItem DiagramMainMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forwardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem backwardToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem netronGuideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aPIDocumentationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aPISamplesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outputToolStripMenuItem;
        public System.Windows.Forms.MenuStrip RootMenuStrip;
        public Netron.Neon.WinFormsUI.DockPanel DockPanel;
    }
}