namespace IAP_App
{
    partial class CommandTabSectionButton
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
            this.SuspendLayout();
            // 
            // CommandTabSectionButton
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Size = new System.Drawing.Size(80, 20);
            this.MouseLeave += new System.EventHandler(this.CommandTabSectionButton_MouseLeave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CommandTabSectionButton_MouseDown);
            this.MouseEnter += new System.EventHandler(this.CommandTabSectionButton_MouseEnter);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CommandTabSectionButton_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
