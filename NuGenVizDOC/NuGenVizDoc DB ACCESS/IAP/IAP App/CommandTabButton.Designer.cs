namespace IAP_App
{
    partial class CommandTabButton
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
            // CommandTabButton
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.MidnightBlue;
            this.MouseLeave += new System.EventHandler(this.CommandTabButton_MouseLeave);
            this.Click += new System.EventHandler(this.CommandTabButton_Click);
            this.MouseEnter += new System.EventHandler(this.CommandTabButton_MouseEnter);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
