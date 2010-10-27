namespace GMap
{
    partial class ucGMap
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
            this.gMapBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // gMapBrowser
            // 
            this.gMapBrowser.AllowNavigation = false;
            this.gMapBrowser.AllowWebBrowserDrop = false;
            this.gMapBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gMapBrowser.IsWebBrowserContextMenuEnabled = false;
            this.gMapBrowser.Location = new System.Drawing.Point(0, 0);
            this.gMapBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.gMapBrowser.Name = "gMapBrowser";
            this.gMapBrowser.ScrollBarsEnabled = false;
            this.gMapBrowser.Size = new System.Drawing.Size(345, 326);
            this.gMapBrowser.TabIndex = 0;
            this.gMapBrowser.WebBrowserShortcutsEnabled = false;
            // 
            // ucGMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.gMapBrowser);
            this.Name = "ucGMap";
            this.Size = new System.Drawing.Size(345, 326);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser gMapBrowser;
    }
}
