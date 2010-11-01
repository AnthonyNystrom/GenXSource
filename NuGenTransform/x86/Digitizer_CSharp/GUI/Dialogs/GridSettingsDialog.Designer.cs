using WCL;
namespace Genetibase.NuGenTransform
{
    partial class GridSettingsDialog
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
            this.xStep = new System.Windows.Forms.TextBox();
            this.xStop = new System.Windows.Forms.TextBox();
            this.xStart = new System.Windows.Forms.TextBox();
            this.xCount = new System.Windows.Forms.TextBox();
            this.xDisableCombo = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.yStep = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.yStop = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.yStart = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.yCount = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.yDisableCombo = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.button1 = new VistaButton();
            this.button2 = new VistaButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.xStep);
            this.groupBox1.Controls.Add(this.xStop);
            this.groupBox1.Controls.Add(this.xStart);
            this.groupBox1.Controls.Add(this.xCount);
            this.groupBox1.Controls.Add(this.xDisableCombo);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(264, 231);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X Grid Lines";
            // 
            // xStep
            // 
            this.xStep.Location = new System.Drawing.Point(128, 194);
            this.xStep.Name = "xStep";
            this.xStep.Size = new System.Drawing.Size(121, 20);
            this.xStep.TabIndex = 2;
            this.xStep.TextChanged += new System.EventHandler(this.XTextChanged);
            // 
            // xStop
            // 
            this.xStop.Location = new System.Drawing.Point(128, 154);
            this.xStop.Name = "xStop";
            this.xStop.Size = new System.Drawing.Size(121, 20);
            this.xStop.TabIndex = 2;
            this.xStop.TextChanged += new System.EventHandler(this.XTextChanged);
            // 
            // xStart
            // 
            this.xStart.Location = new System.Drawing.Point(128, 115);
            this.xStart.Name = "xStart";
            this.xStart.Size = new System.Drawing.Size(121, 20);
            this.xStart.TabIndex = 2;
            this.xStart.TextChanged += new System.EventHandler(this.XTextChanged);
            // 
            // xCount
            // 
            this.xCount.Location = new System.Drawing.Point(128, 77);
            this.xCount.Name = "xCount";
            this.xCount.Size = new System.Drawing.Size(121, 20);
            this.xCount.TabIndex = 2;
            this.xCount.TextChanged += new System.EventHandler(this.XTextChanged);
            // 
            // xDisableCombo
            // 
            this.xDisableCombo.FormattingEnabled = true;
            this.xDisableCombo.Location = new System.Drawing.Point(128, 39);
            this.xDisableCombo.Name = "xDisableCombo";
            this.xDisableCombo.Size = new System.Drawing.Size(121, 21);
            this.xDisableCombo.TabIndex = 1;
            this.xDisableCombo.SelectedIndexChanged += new System.EventHandler(this.xDisableCombo_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 197);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "XStep";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "XStop";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "XStart";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "XCount";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Disable";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.yStep);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.yStop);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.yStart);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.yCount);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.yDisableCombo);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(283, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(272, 231);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Y Grid Lines";
            // 
            // yStep
            // 
            this.yStep.Location = new System.Drawing.Point(128, 191);
            this.yStep.Name = "yStep";
            this.yStep.Size = new System.Drawing.Size(121, 20);
            this.yStep.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Disable";
            // 
            // yStop
            // 
            this.yStop.Location = new System.Drawing.Point(128, 151);
            this.yStop.Name = "yStop";
            this.yStop.Size = new System.Drawing.Size(121, 20);
            this.yStop.TabIndex = 2;
            this.yStep.TextChanged += new System.EventHandler(this.YTextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "YCount";
            // 
            // yStart
            // 
            this.yStart.Location = new System.Drawing.Point(128, 112);
            this.yStart.Name = "yStart";
            this.yStart.Size = new System.Drawing.Size(121, 20);
            this.yStart.TabIndex = 2;
            this.yStart.TextChanged += new System.EventHandler(this.YTextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "YStart";
            // 
            // yCount
            // 
            this.yCount.Location = new System.Drawing.Point(128, 74);
            this.yCount.Name = "yCount";
            this.yCount.Size = new System.Drawing.Size(121, 20);
            this.yCount.TabIndex = 2;
            this.yCount.TextChanged += new System.EventHandler(this.YTextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 154);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "YStop";
            // 
            // yDisableCombo
            // 
            this.yDisableCombo.FormattingEnabled = true;
            this.yDisableCombo.Location = new System.Drawing.Point(128, 36);
            this.yDisableCombo.Name = "yDisableCombo";
            this.yDisableCombo.Size = new System.Drawing.Size(121, 21);
            this.yDisableCombo.TabIndex = 1;
            this.yDisableCombo.SelectedIndexChanged += new System.EventHandler(this.yDisableCombo_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 194);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "YStep";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(22, 266);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(125, 266);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // GridSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(547, 291);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GridSettingsDialog";
            this.Text = "GridSettingsDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox xStep;
        private System.Windows.Forms.TextBox xStop;
        private System.Windows.Forms.TextBox xStart;
        private System.Windows.Forms.TextBox xCount;
        private System.Windows.Forms.ComboBox xDisableCombo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox yStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox yStop;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox yStart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox yCount;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox yDisableCombo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}