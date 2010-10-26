namespace Genetibase.UI.NuGenImageWorks
{
    partial class frmFE
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.ribbonButtonOp5Cancel = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonButtonOp5OK = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.ribbonButtonOp5Cancel);
            this.panel1.Controls.Add(this.ribbonButtonOp5OK);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.numericUpDown2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(140, 57);
            this.panel1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Curvature";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.numericUpDown2.Location = new System.Drawing.Point(64, 3);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(70, 20);
            this.numericUpDown2.TabIndex = 17;
            // 
            // ribbonButtonOp5Cancel
            // 
            this.ribbonButtonOp5Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ribbonButtonOp5Cancel.Image = global::Genetibase.UI.NuGenImageWorks.Properties.Resources.Cancel;
            this.ribbonButtonOp5Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButtonOp5Cancel.IsFlat = true;
            this.ribbonButtonOp5Cancel.IsPressed = false;
            this.ribbonButtonOp5Cancel.Location = new System.Drawing.Point(66, 27);
            this.ribbonButtonOp5Cancel.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButtonOp5Cancel.Name = "ribbonButtonOp5Cancel";
            this.ribbonButtonOp5Cancel.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButtonOp5Cancel.Size = new System.Drawing.Size(68, 24);
            this.ribbonButtonOp5Cancel.TabIndex = 22;
            this.ribbonButtonOp5Cancel.Text = "Cancel";
            this.ribbonButtonOp5Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButtonOp5Cancel.Click += new System.EventHandler(this.ribbonButtonOp5Cancel_Click);
            // 
            // ribbonButtonOp5OK
            // 
            this.ribbonButtonOp5OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ribbonButtonOp5OK.Image = global::Genetibase.UI.NuGenImageWorks.Properties.Resources.OK;
            this.ribbonButtonOp5OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButtonOp5OK.IsFlat = true;
            this.ribbonButtonOp5OK.IsPressed = false;
            this.ribbonButtonOp5OK.Location = new System.Drawing.Point(6, 27);
            this.ribbonButtonOp5OK.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButtonOp5OK.Name = "ribbonButtonOp5OK";
            this.ribbonButtonOp5OK.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButtonOp5OK.Size = new System.Drawing.Size(49, 24);
            this.ribbonButtonOp5OK.TabIndex = 21;
            this.ribbonButtonOp5OK.Text = "OK";
            this.ribbonButtonOp5OK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButtonOp5OK.Click += new System.EventHandler(this.ribbonButtonOp5OK_Click);
            // 
            // frmFE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(150, 67);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFE";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmFE";
            this.Load += new System.EventHandler(this.frmFE_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown numericUpDown2;
        private RibbonButton ribbonButtonOp5Cancel;
        private RibbonButton ribbonButtonOp5OK;
    }
}