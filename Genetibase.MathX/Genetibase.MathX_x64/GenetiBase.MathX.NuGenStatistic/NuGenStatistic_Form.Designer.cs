namespace GenetiBase.MathX.NuGenStatistic
{
    partial class NuGenStatistic_Form
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
            this.clear = new System.Windows.Forms.Button();
            this.inter_range = new System.Windows.Forms.Button();
            this.corelation_coefficient = new System.Windows.Forms.Button();
            this.index = new System.Windows.Forms.Button();
            this.Standard_Deviation = new System.Windows.Forms.Button();
            this.b_factor = new System.Windows.Forms.Button();
            this.a_factor = new System.Windows.Forms.Button();
            this.mode = new System.Windows.Forms.Button();
            this.length = new System.Windows.Forms.Button();
            this.covarience = new System.Windows.Forms.Button();
            this.mean = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.res = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.l1 = new System.Windows.Forms.TextBox();
            this.range = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.clear);
            this.groupBox1.Controls.Add(this.inter_range);
            this.groupBox1.Controls.Add(this.corelation_coefficient);
            this.groupBox1.Controls.Add(this.index);
            this.groupBox1.Controls.Add(this.Standard_Deviation);
            this.groupBox1.Controls.Add(this.b_factor);
            this.groupBox1.Controls.Add(this.a_factor);
            this.groupBox1.Controls.Add(this.mode);
            this.groupBox1.Controls.Add(this.length);
            this.groupBox1.Controls.Add(this.covarience);
            this.groupBox1.Controls.Add(this.mean);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.button4);
            this.groupBox1.Controls.Add(this.res);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.l1);
            this.groupBox1.Controls.Add(this.range);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(795, 384);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // clear
            // 
            this.clear.BackColor = System.Drawing.SystemColors.GrayText;
            this.clear.Location = new System.Drawing.Point(322, 191);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(139, 30);
            this.clear.TabIndex = 15;
            this.clear.Text = "Clear";
            this.clear.UseVisualStyleBackColor = false;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // inter_range
            // 
            this.inter_range.BackColor = System.Drawing.SystemColors.Window;
            this.inter_range.Location = new System.Drawing.Point(486, 266);
            this.inter_range.Name = "inter_range";
            this.inter_range.Size = new System.Drawing.Size(139, 30);
            this.inter_range.TabIndex = 7;
            this.inter_range.Text = "Inter Quantile Range";
            this.inter_range.UseVisualStyleBackColor = false;
            this.inter_range.Click += new System.EventHandler(this.inter_range_Click);
            // 
            // corelation_coefficient
            // 
            this.corelation_coefficient.BackColor = System.Drawing.SystemColors.Window;
            this.corelation_coefficient.Location = new System.Drawing.Point(629, 314);
            this.corelation_coefficient.Name = "corelation_coefficient";
            this.corelation_coefficient.Size = new System.Drawing.Size(139, 30);
            this.corelation_coefficient.TabIndex = 14;
            this.corelation_coefficient.Text = "Corelation Coefficient";
            this.corelation_coefficient.UseVisualStyleBackColor = false;
            this.corelation_coefficient.Click += new System.EventHandler(this.corelation_coefficient_Click);
            // 
            // index
            // 
            this.index.BackColor = System.Drawing.SystemColors.Window;
            this.index.Location = new System.Drawing.Point(486, 314);
            this.index.Name = "index";
            this.index.Size = new System.Drawing.Size(137, 30);
            this.index.TabIndex = 8;
            this.index.Text = "YULE Index";
            this.index.UseVisualStyleBackColor = false;
            this.index.Click += new System.EventHandler(this.index_Click);
            // 
            // Standard_Deviation
            // 
            this.Standard_Deviation.BackColor = System.Drawing.SystemColors.Window;
            this.Standard_Deviation.Location = new System.Drawing.Point(629, 266);
            this.Standard_Deviation.Name = "Standard_Deviation";
            this.Standard_Deviation.Size = new System.Drawing.Size(139, 30);
            this.Standard_Deviation.TabIndex = 14;
            this.Standard_Deviation.Text = "Standard Deviation";
            this.Standard_Deviation.UseVisualStyleBackColor = false;
            this.Standard_Deviation.Click += new System.EventHandler(this.Standard_Deviation_Click);
            // 
            // b_factor
            // 
            this.b_factor.BackColor = System.Drawing.SystemColors.Window;
            this.b_factor.Location = new System.Drawing.Point(629, 219);
            this.b_factor.Name = "b_factor";
            this.b_factor.Size = new System.Drawing.Size(139, 30);
            this.b_factor.TabIndex = 13;
            this.b_factor.Text = "\"B\" Factor";
            this.b_factor.UseVisualStyleBackColor = false;
            this.b_factor.Click += new System.EventHandler(this.button7_Click);
            // 
            // a_factor
            // 
            this.a_factor.BackColor = System.Drawing.SystemColors.Window;
            this.a_factor.Location = new System.Drawing.Point(486, 222);
            this.a_factor.Name = "a_factor";
            this.a_factor.Size = new System.Drawing.Size(137, 30);
            this.a_factor.TabIndex = 6;
            this.a_factor.Text = "\"A\" Factor";
            this.a_factor.UseVisualStyleBackColor = false;
            this.a_factor.Click += new System.EventHandler(this.a_factor_Click);
            // 
            // mode
            // 
            this.mode.BackColor = System.Drawing.SystemColors.Window;
            this.mode.Location = new System.Drawing.Point(629, 171);
            this.mode.Name = "mode";
            this.mode.Size = new System.Drawing.Size(139, 30);
            this.mode.TabIndex = 12;
            this.mode.Text = "Mode";
            this.mode.UseVisualStyleBackColor = false;
            this.mode.Click += new System.EventHandler(this.mode_Click);
            // 
            // length
            // 
            this.length.BackColor = System.Drawing.SystemColors.Window;
            this.length.Location = new System.Drawing.Point(484, 171);
            this.length.Name = "length";
            this.length.Size = new System.Drawing.Size(139, 30);
            this.length.TabIndex = 5;
            this.length.Text = "Length";
            this.length.UseVisualStyleBackColor = false;
            this.length.Click += new System.EventHandler(this.length_Click);
            // 
            // covarience
            // 
            this.covarience.BackColor = System.Drawing.SystemColors.Window;
            this.covarience.Location = new System.Drawing.Point(629, 124);
            this.covarience.Name = "covarience";
            this.covarience.Size = new System.Drawing.Size(139, 30);
            this.covarience.TabIndex = 11;
            this.covarience.Text = "Covarience For L1";
            this.covarience.UseVisualStyleBackColor = false;
            this.covarience.Click += new System.EventHandler(this.coverience_Click);
            // 
            // mean
            // 
            this.mean.BackColor = System.Drawing.SystemColors.Window;
            this.mean.Location = new System.Drawing.Point(486, 127);
            this.mean.Name = "mean";
            this.mean.Size = new System.Drawing.Size(137, 30);
            this.mean.TabIndex = 4;
            this.mean.Text = "Mean";
            this.mean.UseVisualStyleBackColor = false;
            this.mean.Click += new System.EventHandler(this.mean_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(146, 207);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Result";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.GrayText;
            this.button4.Location = new System.Drawing.Point(322, 127);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(139, 30);
            this.button4.TabIndex = 1;
            this.button4.Text = "Enter List";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // res
            // 
            this.res.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.res.Location = new System.Drawing.Point(220, 200);
            this.res.Name = "res";
            this.res.Size = new System.Drawing.Size(74, 20);
            this.res.TabIndex = 9;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.Window;
            this.button3.Location = new System.Drawing.Point(629, 76);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(139, 30);
            this.button3.TabIndex = 10;
            this.button3.Text = "Max";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.Window;
            this.button2.Location = new System.Drawing.Point(484, 76);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(139, 30);
            this.button2.TabIndex = 3;
            this.button2.Text = "Min";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Window;
            this.button1.Location = new System.Drawing.Point(629, 29);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(139, 30);
            this.button1.TabIndex = 9;
            this.button1.Text = "Middle Of Range";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // l1
            // 
            this.l1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.l1.Location = new System.Drawing.Point(220, 126);
            this.l1.Name = "l1";
            this.l1.Size = new System.Drawing.Size(74, 20);
            this.l1.TabIndex = 0;
            // 
            // range
            // 
            this.range.BackColor = System.Drawing.SystemColors.Window;
            this.range.Location = new System.Drawing.Point(486, 32);
            this.range.Name = "range";
            this.range.Size = new System.Drawing.Size(137, 30);
            this.range.TabIndex = 2;
            this.range.Text = "Range";
            this.range.UseVisualStyleBackColor = false;
            this.range.Click += new System.EventHandler(this.range_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(23, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter The Size Of List for (L1)";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.ForeColor = System.Drawing.SystemColors.Window;
            this.Label3.Location = new System.Drawing.Point(228, 21);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(159, 20);
            this.Label3.TabIndex = 9;
            this.Label3.Text = "Statistic Functions";
            // 
            // NuGenStatistic_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Highlight;
            this.ClientSize = new System.Drawing.Size(819, 454);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.groupBox1);
            this.Location = new System.Drawing.Point(30, 30);
            this.Name = "NuGenStatistic_Form";
            this.Text = "NuGen Statistic";
            this.Load += new System.EventHandler(this.NuGenStatistic_Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox l1;
        private System.Windows.Forms.Button range;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox res;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button Standard_Deviation;
        private System.Windows.Forms.Button b_factor;
        private System.Windows.Forms.Button a_factor;
        private System.Windows.Forms.Button mode;
        private System.Windows.Forms.Button length;
        private System.Windows.Forms.Button covarience;
        private System.Windows.Forms.Button mean;
        private System.Windows.Forms.Button inter_range;
        private System.Windows.Forms.Button corelation_coefficient;
        private System.Windows.Forms.Button index;
        private System.Windows.Forms.Button clear;
        internal System.Windows.Forms.Label Label3;
    }
}