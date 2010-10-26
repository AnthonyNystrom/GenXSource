namespace ChemDevEnv
{
    partial class LoadingProgressDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.uiProgressBar1 = new Janus.Windows.EditControls.UIProgressBar();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.uiProgressBar2 = new Janus.Windows.EditControls.UIProgressBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Process:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(57, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(283, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "label3";
            // 
            // uiProgressBar1
            // 
            this.uiProgressBar1.Location = new System.Drawing.Point(6, 64);
            this.uiProgressBar1.Name = "uiProgressBar1";
            this.uiProgressBar1.Size = new System.Drawing.Size(318, 37);
            this.uiProgressBar1.TabIndex = 9;
            this.uiProgressBar1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // uiButton1
            // 
            this.uiButton1.Location = new System.Drawing.Point(330, 34);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(75, 67);
            this.uiButton1.TabIndex = 10;
            this.uiButton1.Text = "Cancel";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.uiProgressBar2);
            this.panel1.Controls.Add(this.uiProgressBar1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.uiButton1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(7, 8);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(415, 112);
            this.panel1.TabIndex = 11;
            // 
            // uiProgressBar2
            // 
            this.uiProgressBar2.Location = new System.Drawing.Point(6, 34);
            this.uiProgressBar2.Name = "uiProgressBar2";
            this.uiProgressBar2.Size = new System.Drawing.Size(318, 23);
            this.uiProgressBar2.TabIndex = 11;
            this.uiProgressBar2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // LoadingProgressDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 128);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LoadingProgressDlg";
            this.Text = "Loading File";
            this.Load += new System.EventHandler(this.LoadingProgressDlg_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private Janus.Windows.EditControls.UIProgressBar uiProgressBar1;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private System.Windows.Forms.Panel panel1;
        private Janus.Windows.EditControls.UIProgressBar uiProgressBar2;
    }
}