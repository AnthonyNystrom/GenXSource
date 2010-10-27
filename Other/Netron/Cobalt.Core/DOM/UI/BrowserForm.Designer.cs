namespace Netron.Cobalt
{
    partial class BrowserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserForm));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.WebBrowser = new System.Windows.Forms.WebBrowser();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.browserToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BrowserStrip = new System.Windows.Forms.ToolStrip();
            this.backNavigateButton = new System.Windows.Forms.ToolStripButton();
            this.forwardNavigateButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.UrlBox = new System.Windows.Forms.ToolStripComboBox();
            this.GoUrlButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.stopNavigateButton = new System.Windows.Forms.ToolStripButton();
            this.homeNavigateButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.netronNavigateButton = new System.Windows.Forms.ToolStripButton();
            this.feedbackButton = new System.Windows.Forms.ToolStripButton();
            this.GuideButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.BrowserStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.WebBrowser);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(813, 342);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(813, 367);
            this.toolStripContainer1.TabIndex = 1;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.ContextMenuStrip = this.contextMenuStrip1;
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.BrowserStrip);
            // 
            // WebBrowser
            // 
            this.WebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WebBrowser.Location = new System.Drawing.Point(0, 0);
            this.WebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.WebBrowser.Name = "WebBrowser";
            this.WebBrowser.Size = new System.Drawing.Size(813, 342);
            this.WebBrowser.TabIndex = 1;
            this.WebBrowser.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.webBrowser_Navigated);
            this.WebBrowser.ProgressChanged += new System.Windows.Forms.WebBrowserProgressChangedEventHandler(this.webBrowser_ProgressChanged);
            this.WebBrowser.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser_Navigating);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browserToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            this.contextMenuStrip1.Click += new System.EventHandler(this.browserToolStripMenuItem_Click);
            // 
            // browserToolStripMenuItem
            // 
            this.browserToolStripMenuItem.Checked = true;
            this.browserToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.browserToolStripMenuItem.Name = "browserToolStripMenuItem";
            this.browserToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.browserToolStripMenuItem.Text = "Browser";
            // 
            // BrowserStrip
            // 
            this.BrowserStrip.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::Netron.Cobalt.Properties.Settings.Default, "BrowserStripLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.BrowserStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.BrowserStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backNavigateButton,
            this.forwardNavigateButton,
            this.toolStripSeparator10,
            this.UrlBox,
            this.GoUrlButton,
            this.toolStripSeparator9,
            this.stopNavigateButton,
            this.homeNavigateButton,
            this.toolStripSeparator4,
            this.netronNavigateButton,
            this.feedbackButton,
            this.GuideButton});
            this.BrowserStrip.Location = global::Netron.Cobalt.Properties.Settings.Default.BrowserStripLocation;
            this.BrowserStrip.Name = "BrowserStrip";
            this.BrowserStrip.Size = new System.Drawing.Size(581, 25);
            this.BrowserStrip.TabIndex = 9;
            // 
            // backNavigateButton
            // 
            this.backNavigateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.backNavigateButton.Image = global::Netron.Cobalt.Properties.Resources.GoRtlHS;
            this.backNavigateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.backNavigateButton.Name = "backNavigateButton";
            this.backNavigateButton.Size = new System.Drawing.Size(23, 22);
            this.backNavigateButton.Text = "Back";
            this.backNavigateButton.Click += new System.EventHandler(this.backNavigateButton_Click);
            // 
            // forwardNavigateButton
            // 
            this.forwardNavigateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.forwardNavigateButton.Image = global::Netron.Cobalt.Properties.Resources.GoLtrHS;
            this.forwardNavigateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.forwardNavigateButton.Name = "forwardNavigateButton";
            this.forwardNavigateButton.Size = new System.Drawing.Size(23, 22);
            this.forwardNavigateButton.Text = "Forward";
            this.forwardNavigateButton.Click += new System.EventHandler(this.forwardNavigateButton_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // UrlBox
            // 
            this.UrlBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.UrlBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.UrlBox.Items.AddRange(new object[] {
            "Cobalt\'s Welcome page",
            "API Guide",
            "The Netron Project site",
            "Netron Forums",
            "Your system\'s homepage"});
            this.UrlBox.Name = "UrlBox";
            this.UrlBox.Size = new System.Drawing.Size(350, 25);
            this.UrlBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.urlBox_KeyDown);
            this.UrlBox.SelectedIndexChanged += new System.EventHandler(this.UrlBox_SelectedIndexChanged);
            // 
            // GoUrlButton
            // 
            this.GoUrlButton.Image = global::Netron.Cobalt.Properties.Resources.GoLtrHS;
            this.GoUrlButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GoUrlButton.Name = "GoUrlButton";
            this.GoUrlButton.Size = new System.Drawing.Size(40, 22);
            this.GoUrlButton.Text = "Go";
            this.GoUrlButton.Click += new System.EventHandler(this.GoUrlButton_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // stopNavigateButton
            // 
            this.stopNavigateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.stopNavigateButton.Image = global::Netron.Cobalt.Properties.Resources.RighsRestrictedHS;
            this.stopNavigateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stopNavigateButton.Name = "stopNavigateButton";
            this.stopNavigateButton.Size = new System.Drawing.Size(23, 22);
            this.stopNavigateButton.Text = "Stop";
            this.stopNavigateButton.Click += new System.EventHandler(this.stopNavigateButton_Click);
            // 
            // homeNavigateButton
            // 
            this.homeNavigateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.homeNavigateButton.Image = global::Netron.Cobalt.Properties.Resources.HomeHS;
            this.homeNavigateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.homeNavigateButton.Name = "homeNavigateButton";
            this.homeNavigateButton.Size = new System.Drawing.Size(23, 22);
            this.homeNavigateButton.Text = "Home";
            this.homeNavigateButton.Click += new System.EventHandler(this.homeNavigateButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // netronNavigateButton
            // 
            this.netronNavigateButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.netronNavigateButton.Image = global::Netron.Cobalt.Properties.Resources.PublishToWebHS;
            this.netronNavigateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.netronNavigateButton.Name = "netronNavigateButton";
            this.netronNavigateButton.Size = new System.Drawing.Size(23, 22);
            this.netronNavigateButton.Text = "The Netron Project";
            this.netronNavigateButton.Click += new System.EventHandler(this.netronNavigateButton_Click);
            // 
            // feedbackButton
            // 
            this.feedbackButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.feedbackButton.Image = global::Netron.Cobalt.Properties.Resources.eps_closedHS;
            this.feedbackButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.feedbackButton.Name = "feedbackButton";
            this.feedbackButton.Size = new System.Drawing.Size(23, 22);
            this.feedbackButton.Text = "Feedback";
            this.feedbackButton.Click += new System.EventHandler(this.feedbackButton_Click);
            // 
            // GuideButton
            // 
            this.GuideButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.GuideButton.Image = global::Netron.Cobalt.Properties.Resources.Help;
            this.GuideButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.GuideButton.Name = "GuideButton";
            this.GuideButton.Size = new System.Drawing.Size(23, 22);
            this.GuideButton.Text = "Netron Guide";
            this.GuideButton.Click += new System.EventHandler(this.GuideButton_Click);
            // 
            // BrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 367);
            this.Controls.Add(this.toolStripContainer1);
            this.DockableAreas = ((Netron.Neon.WinFormsUI.DockAreas)((((Netron.Neon.WinFormsUI.DockAreas.Float | Netron.Neon.WinFormsUI.DockAreas.DockLeft)
                        | Netron.Neon.WinFormsUI.DockAreas.DockRight)
                        | Netron.Neon.WinFormsUI.DockAreas.Document)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BrowserForm";
            this.ShowHint = Netron.Neon.WinFormsUI.DockState.Document;
            this.TabText = "Browser";
            this.Text = "BrowserForm";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.BrowserStrip.ResumeLayout(false);
            this.BrowserStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        public System.Windows.Forms.WebBrowser WebBrowser;
        public System.Windows.Forms.ToolStrip BrowserStrip;
        private System.Windows.Forms.ToolStripButton backNavigateButton;
        private System.Windows.Forms.ToolStripButton forwardNavigateButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton GoUrlButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton stopNavigateButton;
        private System.Windows.Forms.ToolStripButton homeNavigateButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton netronNavigateButton;
        private System.Windows.Forms.ToolStripButton feedbackButton;
        private System.Windows.Forms.ToolStripButton GuideButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem browserToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox UrlBox;
    }
}