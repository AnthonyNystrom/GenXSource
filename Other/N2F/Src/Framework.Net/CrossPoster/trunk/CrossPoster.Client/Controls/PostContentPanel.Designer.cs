namespace Next2Friends.CrossPoster.Client.Controls
{
    partial class PostContentPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._borderContainer = new Next2Friends.CrossPoster.Client.Controls.BorderContainer();
            this._separatorLine = new System.Windows.Forms.Panel();
            this._webBrowser = new System.Windows.Forms.WebBrowser();
            this._postMetadataPanel = new Next2Friends.CrossPoster.Client.Controls.PostMetadataPanel();
            this._borderContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // _borderContainer
            // 
            this._borderContainer.Controls.Add(this._separatorLine);
            this._borderContainer.Controls.Add(this._webBrowser);
            this._borderContainer.Controls.Add(this._postMetadataPanel);
            this._borderContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this._borderContainer.Location = new System.Drawing.Point(0, 0);
            this._borderContainer.Name = "_borderContainer";
            this._borderContainer.Size = new System.Drawing.Size(527, 251);
            this._borderContainer.TabIndex = 3;
            // 
            // _separatorLine
            // 
            this._separatorLine.BackColor = System.Drawing.SystemColors.ControlDark;
            this._separatorLine.Dock = System.Windows.Forms.DockStyle.Top;
            this._separatorLine.Location = new System.Drawing.Point(0, 52);
            this._separatorLine.Name = "_separatorLine";
            this._separatorLine.Size = new System.Drawing.Size(523, 1);
            this._separatorLine.TabIndex = 3;
            // 
            // _webBrowser
            // 
            this._webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this._webBrowser.IsWebBrowserContextMenuEnabled = false;
            this._webBrowser.Location = new System.Drawing.Point(0, 52);
            this._webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this._webBrowser.Name = "_webBrowser";
            this._webBrowser.Size = new System.Drawing.Size(523, 195);
            this._webBrowser.TabIndex = 0;
            // 
            // _postMetadataPanel
            // 
            this._postMetadataPanel.BackColor = System.Drawing.SystemColors.Control;
            this._postMetadataPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this._postMetadataPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this._postMetadataPanel.Location = new System.Drawing.Point(0, 0);
            this._postMetadataPanel.Name = "_postMetadataPanel";
            this._postMetadataPanel.Sender = null;
            this._postMetadataPanel.Size = new System.Drawing.Size(523, 52);
            this._postMetadataPanel.Subject = null;
            this._postMetadataPanel.TabIndex = 1;
            // 
            // PostContentPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._borderContainer);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "PostContentPanel";
            this.Size = new System.Drawing.Size(527, 251);
            this._borderContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.WebBrowser _webBrowser;
        private PostMetadataPanel _postMetadataPanel;
        private BorderContainer _borderContainer;
        private System.Windows.Forms.Panel _separatorLine;
    }
}
