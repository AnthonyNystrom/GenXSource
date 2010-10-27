namespace FacScan
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class ImageOptionForm : Form
    {
        public ImageOptionForm()
        {
            this.components = null;
            this.InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void cbxLineSpace_CheckedChanged(object sender, EventArgs e)
        {
            if (this.cbxLineSpace.Checked)
            {
                this.txbLineSpace.Enabled = false;
            }
            else
            {
                this.txbLineSpace.Enabled = true;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
			this.cbxShowLabel = new System.Windows.Forms.CheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txbAngle = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.txbFontSize = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.txbSymbolSize = new System.Windows.Forms.TextBox();
			this.txbTrunc = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbxShowPos = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbxLineSpace = new System.Windows.Forms.CheckBox();
			this.txbLineSpace = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txbTopSpace = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txbLeftSpace = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.txbRightSpace = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.txbBottomSpace = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txbScale = new System.Windows.Forms.TextBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// cbxShowLabel
			// 
			this.cbxShowLabel.Checked = true;
			this.cbxShowLabel.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbxShowLabel.Location = new System.Drawing.Point(24, 16);
			this.cbxShowLabel.Name = "cbxShowLabel";
			this.cbxShowLabel.Size = new System.Drawing.Size(104, 24);
			this.cbxShowLabel.TabIndex = 0;
			this.cbxShowLabel.Text = "Show Label";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(176, 272);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(72, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Label angle";
			// 
			// txbAngle
			// 
			this.txbAngle.Location = new System.Drawing.Point(144, 70);
			this.txbAngle.Name = "txbAngle";
			this.txbAngle.Size = new System.Drawing.Size(40, 20);
			this.txbAngle.TabIndex = 3;
			this.txbAngle.Text = "60";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(24, 144);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(72, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Font Size";
			// 
			// txbFontSize
			// 
			this.txbFontSize.Location = new System.Drawing.Point(144, 142);
			this.txbFontSize.Name = "txbFontSize";
			this.txbFontSize.Size = new System.Drawing.Size(40, 20);
			this.txbFontSize.TabIndex = 3;
			this.txbFontSize.Text = "10";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(24, 176);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Image Symbol Size ";
			// 
			// txbSymbolSize
			// 
			this.txbSymbolSize.Location = new System.Drawing.Point(144, 174);
			this.txbSymbolSize.Name = "txbSymbolSize";
			this.txbSymbolSize.Size = new System.Drawing.Size(40, 20);
			this.txbSymbolSize.TabIndex = 3;
			this.txbSymbolSize.Text = "14";
			// 
			// txbTrunc
			// 
			this.txbTrunc.Location = new System.Drawing.Point(144, 96);
			this.txbTrunc.Name = "txbTrunc";
			this.txbTrunc.Size = new System.Drawing.Size(40, 20);
			this.txbTrunc.TabIndex = 3;
			this.txbTrunc.Text = "8";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(24, 98);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(96, 16);
			this.label4.TabIndex = 2;
			this.label4.Text = "Truncate label at";
			// 
			// cbxShowPos
			// 
			this.cbxShowPos.Checked = true;
			this.cbxShowPos.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbxShowPos.Location = new System.Drawing.Point(24, 40);
			this.cbxShowPos.Name = "cbxShowPos";
			this.cbxShowPos.Size = new System.Drawing.Size(104, 24);
			this.cbxShowPos.TabIndex = 0;
			this.cbxShowPos.Text = "Show Position";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.cbxLineSpace);
			this.groupBox1.Controls.Add(this.txbLineSpace);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.txbTopSpace);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.txbLeftSpace);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.txbRightSpace);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.txbBottomSpace);
			this.groupBox1.Location = new System.Drawing.Point(208, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(232, 200);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Sequence View Spacing";
			// 
			// cbxLineSpace
			// 
			this.cbxLineSpace.Checked = true;
			this.cbxLineSpace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbxLineSpace.Location = new System.Drawing.Point(176, 24);
			this.cbxLineSpace.Name = "cbxLineSpace";
			this.cbxLineSpace.Size = new System.Drawing.Size(48, 24);
			this.cbxLineSpace.TabIndex = 6;
			this.cbxLineSpace.Text = "Auto";
			this.cbxLineSpace.CheckedChanged += new System.EventHandler(this.cbxLineSpace_CheckedChanged);
			// 
			// txbLineSpace
			// 
			this.txbLineSpace.Enabled = false;
			this.txbLineSpace.Location = new System.Drawing.Point(128, 24);
			this.txbLineSpace.Name = "txbLineSpace";
			this.txbLineSpace.Size = new System.Drawing.Size(40, 20);
			this.txbLineSpace.TabIndex = 5;
			this.txbLineSpace.Text = "60";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 26);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(96, 16);
			this.label5.TabIndex = 4;
			this.label5.Text = "Line Space";
			// 
			// txbTopSpace
			// 
			this.txbTopSpace.Location = new System.Drawing.Point(128, 60);
			this.txbTopSpace.Name = "txbTopSpace";
			this.txbTopSpace.Size = new System.Drawing.Size(40, 20);
			this.txbTopSpace.TabIndex = 5;
			this.txbTopSpace.Text = "30";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 62);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(96, 16);
			this.label6.TabIndex = 4;
			this.label6.Text = "Top Space";
			// 
			// txbLeftSpace
			// 
			this.txbLeftSpace.Location = new System.Drawing.Point(128, 96);
			this.txbLeftSpace.Name = "txbLeftSpace";
			this.txbLeftSpace.Size = new System.Drawing.Size(40, 20);
			this.txbLeftSpace.TabIndex = 5;
			this.txbLeftSpace.Text = "60";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(16, 98);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(96, 16);
			this.label9.TabIndex = 4;
			this.label9.Text = "Left Space";
			// 
			// txbRightSpace
			// 
			this.txbRightSpace.Location = new System.Drawing.Point(128, 132);
			this.txbRightSpace.Name = "txbRightSpace";
			this.txbRightSpace.Size = new System.Drawing.Size(40, 20);
			this.txbRightSpace.TabIndex = 5;
			this.txbRightSpace.Text = "50";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(16, 134);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(96, 16);
			this.label10.TabIndex = 4;
			this.label10.Text = "Right Space";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(16, 168);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(96, 16);
			this.label8.TabIndex = 4;
			this.label8.Text = "Bottom Space";
			// 
			// txbBottomSpace
			// 
			this.txbBottomSpace.Location = new System.Drawing.Point(128, 168);
			this.txbBottomSpace.Name = "txbBottomSpace";
			this.txbBottomSpace.Size = new System.Drawing.Size(40, 20);
			this.txbBottomSpace.TabIndex = 5;
			this.txbBottomSpace.Text = "60";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(24, 208);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(96, 16);
			this.label7.TabIndex = 2;
			this.label7.Text = "Scale tick (Bases)";
			// 
			// txbScale
			// 
			this.txbScale.Location = new System.Drawing.Point(144, 208);
			this.txbScale.Name = "txbScale";
			this.txbScale.Size = new System.Drawing.Size(40, 20);
			this.txbScale.TabIndex = 3;
			this.txbScale.Text = "100";
			this.txbScale.TextChanged += new System.EventHandler(this.txbListSymbolSize_TextChanged);
			// 
			// ImageOptionForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(448, 309);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.txbAngle);
			this.Controls.Add(this.txbFontSize);
			this.Controls.Add(this.txbSymbolSize);
			this.Controls.Add(this.txbTrunc);
			this.Controls.Add(this.txbScale);
			this.Controls.Add(this.cbxShowLabel);
			this.Controls.Add(this.cbxShowPos);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label7);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ImageOptionForm";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Image Options";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        private void txbListSymbolSize_TextChanged(object sender, EventArgs e)
        {
        }


        private Button button1;
        public CheckBox cbxLineSpace;
        public CheckBox cbxShowLabel;
        public CheckBox cbxShowPos;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private Label label10;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        public TextBox txbAngle;
        public TextBox txbBottomSpace;
        public TextBox txbFontSize;
        public TextBox txbLeftSpace;
        public TextBox txbLineSpace;
        public TextBox txbRightSpace;
        public TextBox txbScale;
        public TextBox txbSymbolSize;
        public TextBox txbTopSpace;
        public TextBox txbTrunc;
    }
}

