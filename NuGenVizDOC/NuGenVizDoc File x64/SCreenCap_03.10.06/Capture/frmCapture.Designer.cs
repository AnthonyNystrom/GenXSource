namespace Capture
{
    partial class frmCapture
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
            this.nuGenScreenCap1 = new Genetibase.UI.NuGenScreenCap();
            this.SuspendLayout();
            // 
            // nuGenScreenCap1
            // 
            this.nuGenScreenCap1.AutoSizeParentForm = true;
            this.nuGenScreenCap1.CoordsColor = System.Drawing.Color.White;
            this.nuGenScreenCap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nuGenScreenCap1.Location = new System.Drawing.Point(0, 0);
            this.nuGenScreenCap1.Name = "nuGenScreenCap1";
            this.nuGenScreenCap1.Size = new System.Drawing.Size(292, 273);
            this.nuGenScreenCap1.TabIndex = 0;
            // 
            // frmCapture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.ControlBox = false;
            this.Controls.Add(this.nuGenScreenCap1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmCapture";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "frmCapture";
            this.TopMost = true;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCapture_FormClosed);
            this.VisibleChanged += new System.EventHandler(this.frmCapture_VisibleChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private Genetibase.UI.NuGenScreenCap nuGenScreenCap1;
    }
}