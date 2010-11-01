using WCL;
namespace Genetibase.NuGenTransform
{
    partial class CoordinatesDialog
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
            this.polar = new System.Windows.Forms.RadioButton();
            this.cartesian = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.xthetalog = new System.Windows.Forms.RadioButton();
            this.xthetalinear = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.yrlog = new System.Windows.Forms.RadioButton();
            this.yrlinear = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.grads = new System.Windows.Forms.RadioButton();
            this.rads = new System.Windows.Forms.RadioButton();
            this.degrees = new System.Windows.Forms.RadioButton();
            this.button1 = new VistaButton();
            this.button2 = new VistaButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.polar);
            this.groupBox1.Controls.Add(this.cartesian);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 73);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Coordinates";
            // 
            // polar
            // 
            this.polar.AutoSize = true;
            this.polar.Location = new System.Drawing.Point(7, 44);
            this.polar.Name = "polar";
            this.polar.Size = new System.Drawing.Size(97, 17);
            this.polar.TabIndex = 1;
            this.polar.TabStop = true;
            this.polar.Text = "Polar (Y and R)";
            this.polar.UseVisualStyleBackColor = true;
            this.polar.CheckedChanged += new System.EventHandler(this.polar_CheckedChanged);
            // 
            // cartesian
            // 
            this.cartesian.AutoSize = true;
            this.cartesian.Location = new System.Drawing.Point(7, 20);
            this.cartesian.Name = "cartesian";
            this.cartesian.Size = new System.Drawing.Size(116, 17);
            this.cartesian.TabIndex = 0;
            this.cartesian.TabStop = true;
            this.cartesian.Text = "Cartesian (X and Y)";
            this.cartesian.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.xthetalog);
            this.groupBox2.Controls.Add(this.xthetalinear);
            this.groupBox2.Location = new System.Drawing.Point(13, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(145, 78);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "X/Theta";
            // 
            // xthetalog
            // 
            this.xthetalog.AutoSize = true;
            this.xthetalog.Location = new System.Drawing.Point(7, 45);
            this.xthetalog.Name = "xthetalog";
            this.xthetalog.Size = new System.Drawing.Size(43, 17);
            this.xthetalog.TabIndex = 0;
            this.xthetalog.TabStop = true;
            this.xthetalog.Text = "Log";
            this.xthetalog.UseVisualStyleBackColor = true;
            // 
            // xthetalinear
            // 
            this.xthetalinear.AutoSize = true;
            this.xthetalinear.Location = new System.Drawing.Point(7, 22);
            this.xthetalinear.Name = "xthetalinear";
            this.xthetalinear.Size = new System.Drawing.Size(54, 17);
            this.xthetalinear.TabIndex = 0;
            this.xthetalinear.TabStop = true;
            this.xthetalinear.Text = "Linear";
            this.xthetalinear.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.yrlog);
            this.groupBox3.Controls.Add(this.yrlinear);
            this.groupBox3.Location = new System.Drawing.Point(173, 92);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(125, 78);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Y/R";
            // 
            // yrlog
            // 
            this.yrlog.AutoSize = true;
            this.yrlog.Location = new System.Drawing.Point(6, 45);
            this.yrlog.Name = "yrlog";
            this.yrlog.Size = new System.Drawing.Size(43, 17);
            this.yrlog.TabIndex = 0;
            this.yrlog.TabStop = true;
            this.yrlog.Text = "Log";
            this.yrlog.UseVisualStyleBackColor = true;
            // 
            // yrlinear
            // 
            this.yrlinear.AutoSize = true;
            this.yrlinear.Location = new System.Drawing.Point(6, 22);
            this.yrlinear.Name = "yrlinear";
            this.yrlinear.Size = new System.Drawing.Size(54, 17);
            this.yrlinear.TabIndex = 0;
            this.yrlinear.TabStop = true;
            this.yrlinear.Text = "Linear";
            this.yrlinear.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.grads);
            this.groupBox4.Controls.Add(this.rads);
            this.groupBox4.Controls.Add(this.degrees);
            this.groupBox4.Location = new System.Drawing.Point(13, 176);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(285, 121);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Theta Units";
            // 
            // grads
            // 
            this.grads.AutoSize = true;
            this.grads.Enabled = false;
            this.grads.Location = new System.Drawing.Point(7, 74);
            this.grads.Name = "grads";
            this.grads.Size = new System.Drawing.Size(67, 17);
            this.grads.TabIndex = 0;
            this.grads.TabStop = true;
            this.grads.Text = "Gradians";
            this.grads.UseVisualStyleBackColor = true;
            // 
            // rads
            // 
            this.rads.AutoSize = true;
            this.rads.Enabled = false;
            this.rads.Location = new System.Drawing.Point(6, 51);
            this.rads.Name = "rads";
            this.rads.Size = new System.Drawing.Size(64, 17);
            this.rads.TabIndex = 0;
            this.rads.TabStop = true;
            this.rads.Text = "Radians";
            this.rads.UseVisualStyleBackColor = true;
            // 
            // degrees
            // 
            this.degrees.AutoSize = true;
            this.degrees.Enabled = false;
            this.degrees.Location = new System.Drawing.Point(6, 28);
            this.degrees.Name = "degrees";
            this.degrees.Size = new System.Drawing.Size(65, 17);
            this.degrees.TabIndex = 0;
            this.degrees.TabStop = true;
            this.degrees.Text = "Degrees";
            this.degrees.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(13, 315);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(147, 315);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // CoordinatesDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 351);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "CoordinatesDialog";
            this.Text = "Coordinates Settings";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton polar;
        private System.Windows.Forms.RadioButton cartesian;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton xthetalog;
        private System.Windows.Forms.RadioButton xthetalinear;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton yrlog;
        private System.Windows.Forms.RadioButton yrlinear;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton grads;
        private System.Windows.Forms.RadioButton rads;
        private System.Windows.Forms.RadioButton degrees;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}