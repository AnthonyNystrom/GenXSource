namespace Genetibase.UI.NuGenImageWorks
{
    partial class frmBF
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
            this.boxFilterCtrl1 = new Genetibase.UI.NuGenImageWorks.BoxFilterCtrl();
            this.ribbonButtonOp5Cancel = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.ribbonButtonOp5OK = new Genetibase.UI.NuGenImageWorks.RibbonButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.panel1.Controls.Add(this.boxFilterCtrl1);
            this.panel1.Controls.Add(this.ribbonButtonOp5Cancel);
            this.panel1.Controls.Add(this.ribbonButtonOp5OK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(270, 105);
            this.panel1.TabIndex = 11;
            // 
            // boxFilterCtrl1
            // 
            this.boxFilterCtrl1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.boxFilterCtrl1.Location = new System.Drawing.Point(3, 3);
            this.boxFilterCtrl1.Name = "boxFilterCtrl1";
            this.boxFilterCtrl1.Size = new System.Drawing.Size(266, 72);
            this.boxFilterCtrl1.TabIndex = 10;
            // 
            // ribbonButtonOp5Cancel
            // 
            this.ribbonButtonOp5Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ribbonButtonOp5Cancel.Image = global::Genetibase.UI.Properties.Resources.Cancel;
            this.ribbonButtonOp5Cancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButtonOp5Cancel.IsFlat = true;
            this.ribbonButtonOp5Cancel.IsPressed = false;
            this.ribbonButtonOp5Cancel.Location = new System.Drawing.Point(61, 79);
            this.ribbonButtonOp5Cancel.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButtonOp5Cancel.Name = "ribbonButtonOp5Cancel";
            this.ribbonButtonOp5Cancel.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButtonOp5Cancel.Size = new System.Drawing.Size(68, 24);
            this.ribbonButtonOp5Cancel.TabIndex = 9;
            this.ribbonButtonOp5Cancel.Text = "Cancel";
            this.ribbonButtonOp5Cancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButtonOp5Cancel.Click += new System.EventHandler(this.ribbonButtonOp5Cancel_Click);
            // 
            // ribbonButtonOp5OK
            // 
            this.ribbonButtonOp5OK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.ribbonButtonOp5OK.Image = global::Genetibase.UI.Properties.Resources.OK;
            this.ribbonButtonOp5OK.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButtonOp5OK.IsFlat = true;
            this.ribbonButtonOp5OK.IsPressed = false;
            this.ribbonButtonOp5OK.Location = new System.Drawing.Point(1, 79);
            this.ribbonButtonOp5OK.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButtonOp5OK.Name = "ribbonButtonOp5OK";
            this.ribbonButtonOp5OK.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButtonOp5OK.Size = new System.Drawing.Size(49, 24);
            this.ribbonButtonOp5OK.TabIndex = 8;
            this.ribbonButtonOp5OK.Text = "OK";
            this.ribbonButtonOp5OK.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ribbonButtonOp5OK.Click += new System.EventHandler(this.ribbonButtonOp5OK_Click);
            // 
            // frmBF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(280, 115);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmBF";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "frmCA";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RibbonButton ribbonButtonOp5Cancel;
        private RibbonButton ribbonButtonOp5OK;
        private BoxFilterCtrl boxFilterCtrl1;
        private System.Windows.Forms.Panel panel1;
    }
}