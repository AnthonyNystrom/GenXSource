namespace NGVChem
{
    partial class PDBOpenDialogControl
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
            this.panelmain = new System.Windows.Forms.Panel();
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.txtPDBID = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelmain.SuspendLayout();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelmain
            // 
            this.panelmain.Controls.Add(this.groupBox);
            this.panelmain.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelmain.Location = new System.Drawing.Point(0, 17);
            this.panelmain.Name = "panelmain";
            this.panelmain.Size = new System.Drawing.Size(516, 174);
            this.panelmain.TabIndex = 0;
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.btnDownload);
            this.groupBox.Controls.Add(this.txtPDBID);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(516, 174);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "PDB Download and Open";
            // 
            // btnDownload
            // 
            this.btnDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownload.Location = new System.Drawing.Point(410, 52);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(100, 45);
            this.btnDownload.TabIndex = 2;
            this.btnDownload.Text = "&Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // txtPDBID
            // 
            this.txtPDBID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPDBID.Location = new System.Drawing.Point(20, 52);
            this.txtPDBID.Name = "txtPDBID";
            this.txtPDBID.Size = new System.Drawing.Size(384, 96);
            this.txtPDBID.TabIndex = 1;
            this.txtPDBID.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Input PDB ID(Example :1b17) .For multiple,comma separated";
            // 
            // PDBOpenDialogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelmain);
            this.FileDlgStartLocation = AddonWindowLocation.Bottom;
            this.Name = "PDBOpenDialogControl";
            this.Size = new System.Drawing.Size(516, 191);
            this.panelmain.ResumeLayout(false);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelmain;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox txtPDBID;
        private System.Windows.Forms.Button btnDownload;
    }
}
