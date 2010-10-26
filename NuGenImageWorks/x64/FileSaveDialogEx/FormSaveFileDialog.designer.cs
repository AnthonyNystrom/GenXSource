namespace Genetibase.UI.FileSaveDialogEx
{
    partial class FormSaveFileDialog
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pbxPreview = new System.Windows.Forms.PictureBox();
            this.lblFormat = new System.Windows.Forms.Label();
            this.pnlBgColor = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBgColor)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.pbxPreview);
            this.groupBox1.Location = new System.Drawing.Point(5, 30);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 220);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // pbxPreview
            // 
            this.pbxPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxPreview.Location = new System.Drawing.Point(3, 16);
            this.pbxPreview.Name = "pbxPreview";
            this.pbxPreview.Size = new System.Drawing.Size(220, 201);
            this.pbxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbxPreview.TabIndex = 4;
            this.pbxPreview.TabStop = false;
            // 
            // lblFormat
            // 
            this.lblFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFormat.AutoSize = true;
            this.lblFormat.Location = new System.Drawing.Point(2, 268);
            this.lblFormat.Name = "lblFormat";
            this.lblFormat.Size = new System.Drawing.Size(95, 13);
            this.lblFormat.TabIndex = 4;
            this.lblFormat.Text = "Background Color:";
            // 
            // pnlBgColor
            // 
            this.pnlBgColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlBgColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlBgColor.Location = new System.Drawing.Point(103, 260);
            this.pnlBgColor.Name = "pnlBgColor";
            this.pnlBgColor.Size = new System.Drawing.Size(32, 32);
            this.pnlBgColor.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pnlBgColor.TabIndex = 5;
            this.pnlBgColor.TabStop = false;
            this.pnlBgColor.DoubleClick += new System.EventHandler(this.pnlBgColor_Click);
            this.pnlBgColor.Click += new System.EventHandler(this.pnlBgColor_Click);
            // 
            // FormSaveFileDialog
            // 
            this.Controls.Add(this.pnlBgColor);
            this.Controls.Add(this.lblFormat);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormSaveFileDialog";
            this.Size = new System.Drawing.Size(237, 318);
            this.Load += new System.EventHandler(this.FormSaveFileDialog_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlBgColor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox pbxPreview;
        private System.Windows.Forms.Label lblFormat;
        private System.Windows.Forms.PictureBox pnlBgColor;
    }
}