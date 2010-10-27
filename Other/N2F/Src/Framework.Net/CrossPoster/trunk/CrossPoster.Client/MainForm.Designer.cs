namespace Next2Friends.CrossPoster.Client
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
            this._blogList = new System.Windows.Forms.TreeView();
            this._blogListContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this._blogPropertiesItem = new System.Windows.Forms.ToolStripMenuItem();
            this._blogListImages = new System.Windows.Forms.ImageList(this.components);
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._splitContainer2 = new System.Windows.Forms.SplitContainer();
            this._blogEntries = new System.Windows.Forms.ListView();
            this._subjectColumn = new System.Windows.Forms.ColumnHeader();
            this._senderColumn = new System.Windows.Forms.ColumnHeader();
            this._dateColumn = new System.Windows.Forms.ColumnHeader();
            this._statusStrip = new System.Windows.Forms.StatusStrip();
            this._statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this._statusProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this._postContentPanel = new Next2Friends.CrossPoster.Client.Controls.PostContentPanel();
            this._downloadButton = new System.Windows.Forms.ToolStripSplitButton();
            this._composeButton = new System.Windows.Forms.ToolStripButton();
            this._blogActionButton = new System.Windows.Forms.ToolStripDropDownButton();
            this._addBlogItem = new System.Windows.Forms.ToolStripMenuItem();
            this._removeBlogItem = new System.Windows.Forms.ToolStripMenuItem();
            this._separatorItem = new System.Windows.Forms.ToolStripSeparator();
            this._propertiesItem = new System.Windows.Forms.ToolStripMenuItem();
            this._blogListContextMenu.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this._splitContainer2.Panel1.SuspendLayout();
            this._splitContainer2.Panel2.SuspendLayout();
            this._splitContainer2.SuspendLayout();
            this._statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // _blogList
            // 
            this._blogList.ContextMenuStrip = this._blogListContextMenu;
            this._blogList.Dock = System.Windows.Forms.DockStyle.Fill;
            this._blogList.FullRowSelect = true;
            this._blogList.HideSelection = false;
            this._blogList.ImageIndex = 0;
            this._blogList.ImageList = this._blogListImages;
            this._blogList.Location = new System.Drawing.Point(0, 0);
            this._blogList.Name = "_blogList";
            this._blogList.SelectedImageIndex = 0;
            this._blogList.ShowLines = false;
            this._blogList.ShowPlusMinus = false;
            this._blogList.ShowRootLines = false;
            this._blogList.Size = new System.Drawing.Size(200, 408);
            this._blogList.TabIndex = 2;
            // 
            // _blogListContextMenu
            // 
            this._blogListContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._blogPropertiesItem});
            this._blogListContextMenu.Name = "_blogListContextMenu";
            this._blogListContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._blogListContextMenu.Size = new System.Drawing.Size(147, 26);
            // 
            // _blogPropertiesItem
            // 
            this._blogPropertiesItem.Name = "_blogPropertiesItem";
            this._blogPropertiesItem.Size = new System.Drawing.Size(146, 22);
            this._blogPropertiesItem.Text = "&Properties...";
            // 
            // _blogListImages
            // 
            this._blogListImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("_blogListImages.ImageStream")));
            this._blogListImages.TransparentColor = System.Drawing.Color.Transparent;
            this._blogListImages.Images.SetKeyName(0, "Blogger.png");
            this._blogListImages.Images.SetKeyName(1, "WordPress.png");
            this._blogListImages.Images.SetKeyName(2, "LiveJournal.png");
            // 
            // _toolStrip
            // 
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._downloadButton,
            this._composeButton,
            this._blogActionButton});
            this._toolStrip.Location = new System.Drawing.Point(0, 0);
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this._toolStrip.Size = new System.Drawing.Size(736, 25);
            this._toolStrip.TabIndex = 3;
            // 
            // _splitContainer
            // 
            this._splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer.Location = new System.Drawing.Point(0, 25);
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._blogList);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._splitContainer2);
            this._splitContainer.Size = new System.Drawing.Size(736, 408);
            this._splitContainer.SplitterDistance = 200;
            this._splitContainer.TabIndex = 4;
            // 
            // _splitContainer2
            // 
            this._splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitContainer2.Location = new System.Drawing.Point(0, 0);
            this._splitContainer2.Name = "_splitContainer2";
            this._splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitContainer2.Panel1
            // 
            this._splitContainer2.Panel1.Controls.Add(this._blogEntries);
            // 
            // _splitContainer2.Panel2
            // 
            this._splitContainer2.Panel2.Controls.Add(this._postContentPanel);
            this._splitContainer2.Size = new System.Drawing.Size(532, 408);
            this._splitContainer2.SplitterDistance = 167;
            this._splitContainer2.TabIndex = 0;
            // 
            // _blogEntries
            // 
            this._blogEntries.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._subjectColumn,
            this._senderColumn,
            this._dateColumn});
            this._blogEntries.Dock = System.Windows.Forms.DockStyle.Fill;
            this._blogEntries.FullRowSelect = true;
            this._blogEntries.HideSelection = false;
            this._blogEntries.Location = new System.Drawing.Point(0, 0);
            this._blogEntries.Name = "_blogEntries";
            this._blogEntries.Size = new System.Drawing.Size(532, 167);
            this._blogEntries.TabIndex = 0;
            this._blogEntries.UseCompatibleStateImageBehavior = false;
            this._blogEntries.View = System.Windows.Forms.View.Details;
            this._blogEntries.SelectedIndexChanged += new System.EventHandler(this._blogEntries_SelectedIndexChanged);
            // 
            // _subjectColumn
            // 
            this._subjectColumn.Text = "Subject";
            this._subjectColumn.Width = 253;
            // 
            // _senderColumn
            // 
            this._senderColumn.Text = "Sender";
            this._senderColumn.Width = 139;
            // 
            // _dateColumn
            // 
            this._dateColumn.Text = "Date";
            this._dateColumn.Width = 134;
            // 
            // _statusStrip
            // 
            this._statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._statusLabel,
            this._statusProgressBar});
            this._statusStrip.Location = new System.Drawing.Point(0, 433);
            this._statusStrip.Name = "_statusStrip";
            this._statusStrip.Size = new System.Drawing.Size(736, 22);
            this._statusStrip.TabIndex = 5;
            this._statusStrip.Text = "statusStrip1";
            // 
            // _statusLabel
            // 
            this._statusLabel.AutoSize = false;
            this._statusLabel.BackColor = System.Drawing.SystemColors.Control;
            this._statusLabel.Name = "_statusLabel";
            this._statusLabel.Size = new System.Drawing.Size(200, 17);
            this._statusLabel.Text = "Ready";
            this._statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _statusProgressBar
            // 
            this._statusProgressBar.Name = "_statusProgressBar";
            this._statusProgressBar.Size = new System.Drawing.Size(100, 16);
            this._statusProgressBar.Visible = false;
            // 
            // _postContentPanel
            // 
            this._postContentPanel.Content = null;
            this._postContentPanel.Date = null;
            this._postContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._postContentPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._postContentPanel.Location = new System.Drawing.Point(0, 0);
            this._postContentPanel.Name = "_postContentPanel";
            this._postContentPanel.Sender = null;
            this._postContentPanel.Size = new System.Drawing.Size(532, 237);
            this._postContentPanel.Subject = null;
            this._postContentPanel.TabIndex = 0;
            // 
            // _downloadButton
            // 
            this._downloadButton.Image = ((System.Drawing.Image)(resources.GetObject("_downloadButton.Image")));
            this._downloadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._downloadButton.Name = "_downloadButton";
            this._downloadButton.Size = new System.Drawing.Size(86, 22);
            this._downloadButton.Text = "&Download";
            this._downloadButton.ButtonClick += new System.EventHandler(this._downloadButton_ButtonClick);
            // 
            // _composeButton
            // 
            this._composeButton.Enabled = false;
            this._composeButton.Image = global::Next2Friends.CrossPoster.Client.Properties.Resources.New;
            this._composeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._composeButton.Name = "_composeButton";
            this._composeButton.Size = new System.Drawing.Size(83, 22);
            this._composeButton.Text = "&Compose...";
            this._composeButton.ToolTipText = "Start working with selected blog";
            this._composeButton.Click += new System.EventHandler(this._composeButton_Click);
            // 
            // _blogActionButton
            // 
            this._blogActionButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._addBlogItem,
            this._removeBlogItem,
            this._separatorItem,
            this._propertiesItem});
            this._blogActionButton.Image = global::Next2Friends.CrossPoster.Client.Properties.Resources.Schema;
            this._blogActionButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this._blogActionButton.Name = "_blogActionButton";
            this._blogActionButton.Size = new System.Drawing.Size(56, 22);
            this._blogActionButton.Text = "&Blog";
            // 
            // _addBlogItem
            // 
            this._addBlogItem.Name = "_addBlogItem";
            this._addBlogItem.Size = new System.Drawing.Size(146, 22);
            this._addBlogItem.Text = "&Add...";
            this._addBlogItem.Click += new System.EventHandler(this._addBlogItem_Click);
            // 
            // _removeBlogItem
            // 
            this._removeBlogItem.Enabled = false;
            this._removeBlogItem.Name = "_removeBlogItem";
            this._removeBlogItem.Size = new System.Drawing.Size(146, 22);
            this._removeBlogItem.Text = "&Remove";
            this._removeBlogItem.Click += new System.EventHandler(this._removeBlogItem_Click);
            // 
            // _separatorItem
            // 
            this._separatorItem.Name = "_separatorItem";
            this._separatorItem.Size = new System.Drawing.Size(143, 6);
            // 
            // _propertiesItem
            // 
            this._propertiesItem.Name = "_propertiesItem";
            this._propertiesItem.Size = new System.Drawing.Size(146, 22);
            this._propertiesItem.Text = "&Properties...";
            this._propertiesItem.Click += new System.EventHandler(this._propertiesItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(736, 455);
            this.Controls.Add(this._splitContainer);
            this.Controls.Add(this._statusStrip);
            this.Controls.Add(this._toolStrip);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CrossPoster";
            this._blogListContextMenu.ResumeLayout(false);
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            this._splitContainer.ResumeLayout(false);
            this._splitContainer2.Panel1.ResumeLayout(false);
            this._splitContainer2.Panel2.ResumeLayout(false);
            this._splitContainer2.ResumeLayout(false);
            this._statusStrip.ResumeLayout(false);
            this._statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView _blogList;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _composeButton;
        private System.Windows.Forms.ImageList _blogListImages;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.SplitContainer _splitContainer2;
        private System.Windows.Forms.ListView _blogEntries;
        private System.Windows.Forms.ColumnHeader _subjectColumn;
        private System.Windows.Forms.ColumnHeader _senderColumn;
        private System.Windows.Forms.ColumnHeader _dateColumn;
        private Next2Friends.CrossPoster.Client.Controls.PostContentPanel _postContentPanel;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
        private System.Windows.Forms.ToolStripProgressBar _statusProgressBar;
        private System.Windows.Forms.ContextMenuStrip _blogListContextMenu;
        private System.Windows.Forms.ToolStripMenuItem _blogPropertiesItem;
        private System.Windows.Forms.ToolStripSplitButton _downloadButton;
        private System.Windows.Forms.ToolStripDropDownButton _blogActionButton;
        private System.Windows.Forms.ToolStripMenuItem _addBlogItem;
        private System.Windows.Forms.ToolStripMenuItem _removeBlogItem;
        private System.Windows.Forms.ToolStripSeparator _separatorItem;
        private System.Windows.Forms.ToolStripMenuItem _propertiesItem;
    }
}

