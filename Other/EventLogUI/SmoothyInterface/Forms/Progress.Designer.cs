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
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblLoadedCount = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(126, 67);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblLoadedCount
            // 
            this.lblLoadedCount.AutoSize = true;
            this.lblLoadedCount.Location = new System.Drawing.Point(123, 9);
            this.lblLoadedCount.Name = "lblLoadedCount";
            this.lblLoadedCount.Size = new System.Drawing.Size(13, 13);
            this.lblLoadedCount.TabIndex = 3;
            this.lblLoadedCount.Text = "0";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(9, 9);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(108, 13);
            this.lblProgress.TabIndex = 4;
            this.lblProgress.Text = "Processing Item  No :";            
            // 
            // Progress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 97);
            this.ControlBox = false;            
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.lblLoadedCount);
            this.Controls.Add(this.btnCancel);
            this.Name = "Progress";
            this.ShowInTaskbar = false;
            this.Text = "Progress";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblLoadedCount;
        private System.Windows.Forms.Label lblProgress;
    }
}