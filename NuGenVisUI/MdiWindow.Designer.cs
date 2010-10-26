namespace Genetibase.VisUI.Controls
{
    partial class MdiWindow
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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MdiWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.IsMdiContainer = true;
            this.Name = "MdiWindow";
            this.MdiChildActivate += new System.EventHandler(this.MdiWindow_MdiChildActivate);
            this.ResumeLayout(false);

        }
    }
}