namespace ScreenCapture
{
    partial class Form2
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
            this.nuGenScreenCap1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nuGenScreenCap1.Location = new System.Drawing.Point(0, 0);
            this.nuGenScreenCap1.Name = "nuGenScreenCap1";
            this.nuGenScreenCap1.Size = new System.Drawing.Size(1024, 768);
            this.nuGenScreenCap1.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1024, 768);
            this.ControlBox = false;
            this.Controls.Add(this.nuGenScreenCap1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.Text = "Form2";
            this.ResumeLayout(false);

        }

        #endregion

        private Genetibase.UI.NuGenScreenCap nuGenScreenCap1;



    }
}