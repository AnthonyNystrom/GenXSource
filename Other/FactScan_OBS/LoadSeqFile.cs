namespace FacScan
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    public class LoadSeqFile : Form
    {
        public LoadSeqFile()
        {
            this.components = null;
            this.state = new LoadSeqState();
            this.InitializeComponent();
            this.txbEnd.Enabled = true;
            this.txbStart.Enabled = false;
            this.txbFrom.Enabled = false;
            this.txbLen.Enabled = false;
            this.ckbAppend.Checked = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.state.Cancel = true;
            this.state.Start = -1;
            this.state.Length = -1;
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.state.Append = this.ckbAppend.Checked;
            this.state.Cancel = false;
            if (this.rbnEnd.Checked)
            {
                this.state.Start = -1;
                this.state.Length = Convert.ToInt32(this.txbEnd.Text);
            }
            else if (this.rbnEntire.Checked)
            {
                this.state.Start = 0;
                this.state.Length = 0;
            }
            else if (this.rbnStart.Checked)
            {
                this.state.Start = 0;
                this.state.Length = Convert.ToInt32(this.txbStart.Text);
            }
            else if (this.rbnMiddle.Checked)
            {
                this.state.Start = Convert.ToInt32(this.txbFrom.Text);
                this.state.Length = Convert.ToInt32(this.txbLen.Text);
            }
            base.Close();
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
			this.ckbAppend = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.txbEnd = new System.Windows.Forms.TextBox();
			this.rbnEnd = new System.Windows.Forms.RadioButton();
			this.rbnEntire = new System.Windows.Forms.RadioButton();
			this.rbnStart = new System.Windows.Forms.RadioButton();
			this.rbnMiddle = new System.Windows.Forms.RadioButton();
			this.txbStart = new System.Windows.Forms.TextBox();
			this.txbFrom = new System.Windows.Forms.TextBox();
			this.txbLen = new System.Windows.Forms.TextBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// ckbAppend
			// 
			this.ckbAppend.Checked = true;
			this.ckbAppend.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckbAppend.Location = new System.Drawing.Point(32, 24);
			this.ckbAppend.Name = "ckbAppend";
			this.ckbAppend.Size = new System.Drawing.Size(176, 24);
			this.ckbAppend.TabIndex = 0;
			this.ckbAppend.Text = "Append to existing gene list";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txbEnd);
			this.groupBox1.Controls.Add(this.rbnEnd);
			this.groupBox1.Controls.Add(this.rbnEntire);
			this.groupBox1.Controls.Add(this.rbnStart);
			this.groupBox1.Controls.Add(this.rbnMiddle);
			this.groupBox1.Controls.Add(this.txbStart);
			this.groupBox1.Controls.Add(this.txbFrom);
			this.groupBox1.Controls.Add(this.txbLen);
			this.groupBox1.Location = new System.Drawing.Point(32, 64);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(328, 176);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Scope";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(204, 139);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 16);
			this.label1.TabIndex = 3;
			this.label1.Text = "Length";
			// 
			// txbEnd
			// 
			this.txbEnd.Location = new System.Drawing.Point(128, 63);
			this.txbEnd.Name = "txbEnd";
			this.txbEnd.Size = new System.Drawing.Size(72, 20);
			this.txbEnd.TabIndex = 2;
			this.txbEnd.Text = "1000";
			// 
			// rbnEnd
			// 
			this.rbnEnd.Checked = true;
			this.rbnEnd.Location = new System.Drawing.Point(16, 61);
			this.rbnEnd.Name = "rbnEnd";
			this.rbnEnd.Size = new System.Drawing.Size(104, 24);
			this.rbnEnd.TabIndex = 1;
			this.rbnEnd.TabStop = true;
			this.rbnEnd.Text = "From end (bp)";
			this.rbnEnd.CheckedChanged += new System.EventHandler(this.rbnEnd_CheckedChanged);
			// 
			// rbnEntire
			// 
			this.rbnEntire.Location = new System.Drawing.Point(16, 24);
			this.rbnEntire.Name = "rbnEntire";
			this.rbnEntire.Size = new System.Drawing.Size(104, 24);
			this.rbnEntire.TabIndex = 0;
			this.rbnEntire.Text = "Entire length";
			this.rbnEntire.CheckedChanged += new System.EventHandler(this.rbnEntire_CheckedChanged);
			// 
			// rbnStart
			// 
			this.rbnStart.Location = new System.Drawing.Point(16, 98);
			this.rbnStart.Name = "rbnStart";
			this.rbnStart.Size = new System.Drawing.Size(104, 24);
			this.rbnStart.TabIndex = 1;
			this.rbnStart.Text = "From start (bp)";
			this.rbnStart.CheckedChanged += new System.EventHandler(this.rbnStart_CheckedChanged);
			// 
			// rbnMiddle
			// 
			this.rbnMiddle.Location = new System.Drawing.Point(16, 132);
			this.rbnMiddle.Name = "rbnMiddle";
			this.rbnMiddle.Size = new System.Drawing.Size(88, 24);
			this.rbnMiddle.TabIndex = 1;
			this.rbnMiddle.Text = "From start";
			this.rbnMiddle.CheckedChanged += new System.EventHandler(this.rbnMiddle_CheckedChanged);
			// 
			// txbStart
			// 
			this.txbStart.Location = new System.Drawing.Point(128, 100);
			this.txbStart.Name = "txbStart";
			this.txbStart.Size = new System.Drawing.Size(72, 20);
			this.txbStart.TabIndex = 2;
			// 
			// txbFrom
			// 
			this.txbFrom.Location = new System.Drawing.Point(128, 135);
			this.txbFrom.Name = "txbFrom";
			this.txbFrom.Size = new System.Drawing.Size(72, 20);
			this.txbFrom.TabIndex = 2;
			// 
			// txbLen
			// 
			this.txbLen.Location = new System.Drawing.Point(250, 136);
			this.txbLen.Name = "txbLen";
			this.txbLen.Size = new System.Drawing.Size(72, 20);
			this.txbLen.TabIndex = 2;
			// 
			// btnOK
			// 
			this.btnOK.Location = new System.Drawing.Point(88, 256);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Location = new System.Drawing.Point(208, 256);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// LoadSeqFile
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(376, 285);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.ckbAppend);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "LoadSeqFile";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.Text = "Load Sequence File Options";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

        }

        private void rbnEnd_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnEnd.Checked)
            {
                this.txbEnd.Enabled = true;
                this.txbStart.Enabled = false;
                this.txbFrom.Enabled = false;
                this.txbLen.Enabled = false;
            }
        }

        private void rbnEntire_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnEntire.Checked)
            {
                this.txbEnd.Enabled = false;
                this.txbStart.Enabled = false;
                this.txbFrom.Enabled = false;
                this.txbLen.Enabled = false;
            }
        }

        private void rbnMiddle_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnMiddle.Checked)
            {
                this.txbEnd.Enabled = false;
                this.txbStart.Enabled = false;
                this.txbFrom.Enabled = true;
                this.txbLen.Enabled = true;
            }
        }

        private void rbnStart_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbnStart.Checked)
            {
                this.txbEnd.Enabled = false;
                this.txbStart.Enabled = true;
                this.txbFrom.Enabled = false;
                this.txbLen.Enabled = false;
            }
        }


        private Button btnCancel;
        private Button btnOK;
        private CheckBox ckbAppend;
        private Container components;
        private GroupBox groupBox1;
        private Label label1;
        private RadioButton rbnEnd;
        private RadioButton rbnEntire;
        private RadioButton rbnMiddle;
        private RadioButton rbnStart;
        public LoadSeqState state;
        private TextBox txbEnd;
        private TextBox txbFrom;
        private TextBox txbLen;
        private TextBox txbStart;
    }
}

