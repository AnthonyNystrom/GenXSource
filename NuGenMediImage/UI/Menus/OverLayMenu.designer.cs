using Genetibase.NuGenMediImage.UI.Controls;

namespace Genetibase.NuGenMediImage.UI.Menus
{
    partial class OverLayMenu
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.ribbonButton2 = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.ribbonButton3 = new Genetibase.NuGenMediImage.UI.Controls.RibbonButton();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(250)))));
            this.flowLayoutPanel1.Controls.Add(this.ribbonButton2);
            this.flowLayoutPanel1.Controls.Add(this.ribbonButton3);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(5, 5);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(79, 52);
            this.flowLayoutPanel1.TabIndex = 2;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // ribbonButton2
            // 
            this.ribbonButton2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ribbonButton2.ForeColor = System.Drawing.Color.Black;
            this.ribbonButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton2.IsFlat = true;
            this.ribbonButton2.IsPressed = false;
            this.ribbonButton2.Location = new System.Drawing.Point(1, 1);
            this.ribbonButton2.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton2.Name = "ribbonButton2";
            this.ribbonButton2.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton2.Size = new System.Drawing.Size(76, 20);
            this.ribbonButton2.TabIndex = 1;
            this.ribbonButton2.Text = "Black";
            this.ribbonButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton2.Click += new System.EventHandler(this.ribbonButton2_Click);
            // 
            // ribbonButton3
            // 
            this.ribbonButton3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ribbonButton3.ForeColor = System.Drawing.Color.Black;
            this.ribbonButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ribbonButton3.IsFlat = true;
            this.ribbonButton3.IsPressed = false;
            this.ribbonButton3.Location = new System.Drawing.Point(1, 23);
            this.ribbonButton3.Margin = new System.Windows.Forms.Padding(1);
            this.ribbonButton3.Name = "ribbonButton3";
            this.ribbonButton3.Padding = new System.Windows.Forms.Padding(2);
            this.ribbonButton3.Size = new System.Drawing.Size(76, 20);
            this.ribbonButton3.TabIndex = 2;
            this.ribbonButton3.Text = "White";
            this.ribbonButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.ribbonButton3.Click += new System.EventHandler(this.ribbonButton3_Click);
            // 
            // OverLayMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(83)))), ((int)(((byte)(83)))), ((int)(((byte)(83)))));
            this.ClientSize = new System.Drawing.Size(89, 62);
            this.Controls.Add(this.flowLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(89, 62);
            this.MinimumSize = new System.Drawing.Size(89, 62);
            this.Name = "OverLayMenu";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "OverLayMenu";
            this.Deactivate += new System.EventHandler(this.OverLayMenu_Deactivate);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OverLayMenu_Paint);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private RibbonButton ribbonButton2;
        private RibbonButton ribbonButton3;

    }
}
