namespace SmoothyInterface.Forms
{
    partial class Progress
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
            this.lblLoadedCount = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.btnCancel = new Janus.Windows.EditControls.UIButton();
            this.uiProgressBar1 = new Janus.Windows.EditControls.UIProgressBar();
            this.SuspendLayout();
            // 
            // lblLoadedCount
            // 
            this.lblLoadedCount.AutoSize = true;
            this.lblLoadedCount.Location = new System.Drawing.Point(126, 9);
            this.lblLoadedCount.Name = "lblLoadedCount";
            this.lblLoadedCount.Size = new System.Drawing.Size(13, 13);
            this.lblLoadedCount.TabIndex = 3;
            this.lblLoadedCount.Text = "0";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(12, 9);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(108, 13);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "Processing Item  No :";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(247, 62);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            // 
            // uiProgressBar1
            // 
            this.uiProgressBar1.Location = new System.Drawing.Point(12, 28);
            this.uiProgressBar1.Name = "uiProgressBar1";
            this.uiProgressBar1.ShowPercentage = true;
            this.uiProgressBar1.Size = new System.Drawing.Size(310, 23);
            this.uiProgressBar1.TabIndex = 6;
            this.uiProgressBar1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 91);
            this.ControlBox = false;
            this.Controls.Add(this.uiProgressBar1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblLoadedCount);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Progress";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Progress";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLoadedCount;
        private System.Windows.Forms.Label lblProgress;
        public Janus.Windows.EditControls.UIButton btnCancel;
        private Janus.Windows.EditControls.UIProgressBar uiProgressBar1;
    }
}