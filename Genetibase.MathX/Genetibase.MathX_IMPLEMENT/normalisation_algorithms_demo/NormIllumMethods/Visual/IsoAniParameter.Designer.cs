namespace NormIllumMethods.Visual
{
    partial class IsoAniParameter
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
            this.btnOK = new System.Windows.Forms.Button();
            this.txtParam = new System.Windows.Forms.TextBox();
            this.labConstraint = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(28, 56);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtParam
            // 
            this.txtParam.Location = new System.Drawing.Point(61, 28);
            this.txtParam.Name = "txtParam";
            this.txtParam.Size = new System.Drawing.Size(47, 20);
            this.txtParam.TabIndex = 1;
            // 
            // labConstraint
            // 
            this.labConstraint.AutoSize = true;
            this.labConstraint.Location = new System.Drawing.Point(36, 10);
            this.labConstraint.Name = "labConstraint";
            this.labConstraint.Size = new System.Drawing.Size(114, 13);
            this.labConstraint.TabIndex = 2;
            this.labConstraint.Text = "Smoothness constraint";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(87, 56);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(54, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // IsoAniParameter
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(174, 87);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.labConstraint);
            this.Controls.Add(this.txtParam);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "IsoAniParameter";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Parameter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TextBox txtParam;
        private System.Windows.Forms.Label labConstraint;
        private System.Windows.Forms.Button btnCancel;
    }
}