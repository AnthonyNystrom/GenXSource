using WCL;
namespace Genetibase.NuGenTransform
{
    partial class ExportSettingsDialog
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new VistaButton();
            this.button1 = new VistaButton();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rawXY = new System.Windows.Forms.RadioButton();
            this.intYGridline = new System.Windows.Forms.RadioButton();
            this.intYXFirst = new System.Windows.Forms.RadioButton();
            this.intYXall = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.oneEachLine = new System.Windows.Forms.RadioButton();
            this.allOneLine = new System.Windows.Forms.RadioButton();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tabs = new System.Windows.Forms.RadioButton();
            this.spaces = new System.Windows.Forms.RadioButton();
            this.commas = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.gnuplot = new System.Windows.Forms.RadioButton();
            this.simple = new System.Windows.Forms.RadioButton();
            this.none = new System.Windows.Forms.RadioButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new VistaButton();
            this.button4 = new VistaButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.listBox2);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(362, 171);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Curves Selection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(262, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Not Included";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Included";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(136, 108);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "Exclude>>";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(136, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "<<Include";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(230, 39);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(126, 121);
            this.listBox2.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(7, 40);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(114, 121);
            this.listBox1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rawXY);
            this.groupBox2.Controls.Add(this.intYGridline);
            this.groupBox2.Controls.Add(this.intYXFirst);
            this.groupBox2.Controls.Add(this.intYXall);
            this.groupBox2.Location = new System.Drawing.Point(20, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(355, 113);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Points Selection";
            // 
            // rawXY
            // 
            this.rawXY.AutoSize = true;
            this.rawXY.Location = new System.Drawing.Point(7, 90);
            this.rawXY.Name = "rawXY";
            this.rawXY.Size = new System.Drawing.Size(102, 17);
            this.rawXY.TabIndex = 2;
            this.rawXY.TabStop = true;
            this.rawXY.Text = "Raw X\'s and Y\'s";
            this.rawXY.UseVisualStyleBackColor = true;
            this.rawXY.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // intYGridline
            // 
            this.intYGridline.AutoSize = true;
            this.intYGridline.Location = new System.Drawing.Point(7, 67);
            this.intYGridline.Name = "intYGridline";
            this.intYGridline.Size = new System.Drawing.Size(157, 17);
            this.intYGridline.TabIndex = 2;
            this.intYGridline.TabStop = true;
            this.intYGridline.Text = "Interpolate Y\'s at gridline X\'s";
            this.intYGridline.UseVisualStyleBackColor = true;
            this.intYGridline.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // intYXFirst
            // 
            this.intYXFirst.AutoSize = true;
            this.intYXFirst.Location = new System.Drawing.Point(6, 43);
            this.intYXFirst.Name = "intYXFirst";
            this.intYXFirst.Size = new System.Drawing.Size(202, 17);
            this.intYXFirst.TabIndex = 1;
            this.intYXFirst.TabStop = true;
            this.intYXFirst.Text = "Interpolate Y\'s and X\'s from first curve";
            this.intYXFirst.UseVisualStyleBackColor = true;
            this.intYXFirst.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // intYXall
            // 
            this.intYXall.AutoSize = true;
            this.intYXall.Location = new System.Drawing.Point(7, 20);
            this.intYXall.Name = "intYXall";
            this.intYXall.Size = new System.Drawing.Size(192, 17);
            this.intYXall.TabIndex = 0;
            this.intYXall.TabStop = true;
            this.intYXall.Text = "Interpolate Y\'s at X\'s from all curves";
            this.intYXall.UseVisualStyleBackColor = true;
            this.intYXall.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.oneEachLine);
            this.groupBox3.Controls.Add(this.allOneLine);
            this.groupBox3.Location = new System.Drawing.Point(20, 304);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(355, 72);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Layout";
            // 
            // oneEachLine
            // 
            this.oneEachLine.AutoSize = true;
            this.oneEachLine.Location = new System.Drawing.Point(6, 42);
            this.oneEachLine.Name = "oneEachLine";
            this.oneEachLine.Size = new System.Drawing.Size(136, 17);
            this.oneEachLine.TabIndex = 2;
            this.oneEachLine.TabStop = true;
            this.oneEachLine.Text = "One curve on each line";
            this.oneEachLine.UseVisualStyleBackColor = true;
            this.oneEachLine.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // allOneLine
            // 
            this.allOneLine.AutoSize = true;
            this.allOneLine.Location = new System.Drawing.Point(6, 19);
            this.allOneLine.Name = "allOneLine";
            this.allOneLine.Size = new System.Drawing.Size(132, 17);
            this.allOneLine.TabIndex = 2;
            this.allOneLine.TabStop = true;
            this.allOneLine.Text = "All curves on each line";
            this.allOneLine.UseVisualStyleBackColor = true;
            this.allOneLine.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.tabs);
            this.groupBox4.Controls.Add(this.spaces);
            this.groupBox4.Controls.Add(this.commas);
            this.groupBox4.Location = new System.Drawing.Point(20, 382);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(163, 89);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Delimeters";
            // 
            // tabs
            // 
            this.tabs.AutoSize = true;
            this.tabs.Location = new System.Drawing.Point(7, 65);
            this.tabs.Name = "tabs";
            this.tabs.Size = new System.Drawing.Size(49, 17);
            this.tabs.TabIndex = 2;
            this.tabs.TabStop = true;
            this.tabs.Text = "Tabs";
            this.tabs.UseVisualStyleBackColor = true;
            this.tabs.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // spaces
            // 
            this.spaces.AutoSize = true;
            this.spaces.Location = new System.Drawing.Point(7, 42);
            this.spaces.Name = "spaces";
            this.spaces.Size = new System.Drawing.Size(61, 17);
            this.spaces.TabIndex = 2;
            this.spaces.TabStop = true;
            this.spaces.Text = "Spaces";
            this.spaces.UseVisualStyleBackColor = true;
            this.spaces.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // commas
            // 
            this.commas.AutoSize = true;
            this.commas.Location = new System.Drawing.Point(6, 19);
            this.commas.Name = "commas";
            this.commas.Size = new System.Drawing.Size(65, 17);
            this.commas.TabIndex = 2;
            this.commas.TabStop = true;
            this.commas.Text = "Commas";
            this.commas.UseVisualStyleBackColor = true;
            this.commas.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.gnuplot);
            this.groupBox5.Controls.Add(this.simple);
            this.groupBox5.Controls.Add(this.none);
            this.groupBox5.Location = new System.Drawing.Point(189, 382);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(180, 89);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Header";
            // 
            // gnuplot
            // 
            this.gnuplot.AutoSize = true;
            this.gnuplot.Location = new System.Drawing.Point(6, 65);
            this.gnuplot.Name = "gnuplot";
            this.gnuplot.Size = new System.Drawing.Size(63, 17);
            this.gnuplot.TabIndex = 2;
            this.gnuplot.TabStop = true;
            this.gnuplot.Text = "GnuPlot";
            this.gnuplot.UseVisualStyleBackColor = true;
            this.gnuplot.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // simple
            // 
            this.simple.AutoSize = true;
            this.simple.Location = new System.Drawing.Point(6, 42);
            this.simple.Name = "simple";
            this.simple.Size = new System.Drawing.Size(56, 17);
            this.simple.TabIndex = 2;
            this.simple.TabStop = true;
            this.simple.Text = "Simple";
            this.simple.UseVisualStyleBackColor = true;
            this.simple.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // none
            // 
            this.none.AutoSize = true;
            this.none.Location = new System.Drawing.Point(6, 19);
            this.none.Name = "none";
            this.none.Size = new System.Drawing.Size(51, 17);
            this.none.TabIndex = 2;
            this.none.TabStop = true;
            this.none.Text = "None";
            this.none.UseVisualStyleBackColor = true;
            this.none.CheckedChanged += new System.EventHandler(AnythingChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(20, 478);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(355, 146);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button2.Location = new System.Drawing.Point(20, 652);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(120, 652);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 3;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // ExportSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 682);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "ExportSettingsDialog";
            this.Text = "ExportSettingsDialog";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rawXY;
        private System.Windows.Forms.RadioButton intYGridline;
        private System.Windows.Forms.RadioButton intYXFirst;
        private System.Windows.Forms.RadioButton intYXall;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton tabs;
        private System.Windows.Forms.RadioButton spaces;
        private System.Windows.Forms.RadioButton commas;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton gnuplot;
        private System.Windows.Forms.RadioButton simple;
        private System.Windows.Forms.RadioButton none;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.RadioButton oneEachLine;
        private System.Windows.Forms.RadioButton allOneLine;
    }
}