namespace FacScan
{
    using System;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class ImportSigScan : Form
    {
        public ImportSigScan(TransFactor t)
        {
            this.tf = new TransFactor();
            this.components = null;
            this.InitializeComponent();
            base.StartPosition = FormStartPosition.CenterScreen;
            this.tf = t;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string text1 = this.textBox1.Text;
            if (text1 == "")
            {
                return;
            }
            this.ofDlg.Filter = "Web Page (*.htm)|*.htm|Web Page (*.html)|*.html|Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.ofDlg.DefaultExt = "Text (*.txt)|*.txt";
            if ((this.ofDlg.ShowDialog() == DialogResult.OK) && (this.ofDlg.FileName != ""))
            {
                switch (MessageBox.Show("Append to existing Factor-Position table?", "New Factor-Position Loading...", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        this.readSigScan(this.ofDlg.FileName, text1);
                        goto Label_00B4;

                    case DialogResult.No:
                        this.tf = new TransFactor();
                        this.tf.creatDataTables();
                        this.readSigScan(this.ofDlg.FileName, text1);
                        break;
                }
            }
        Label_00B4:
            base.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
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

        private void Genename_Load(object sender, EventArgs e)
        {
            this.textBox2.Text = "WWW SignalScan at http://thr.cit.nih.gov/molbio/signal/\r\nSelect \"grouped by signal\" and signal classes you wish to.\r\nCopy the result WWW page and paste into a text file.";
            this.Text = "Loading SigScan Result";
            this.textBox1.Capture = true;
        }

        private void InitializeComponent()
        {
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ofDlg = new System.Windows.Forms.OpenFileDialog();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(56, 176);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.TabIndex = 1;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(192, 176);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.TabIndex = 2;
			this.button2.Text = "Cancel";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(56, 144);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(232, 20);
			this.textBox1.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(48, 112);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(136, 24);
			this.label2.TabIndex = 4;
			this.label2.Text = "Give a name for this gene:";
			// 
			// textBox2
			// 
			this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox2.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.textBox2.Location = new System.Drawing.Point(0, 24);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(344, 80);
			this.textBox2.TabIndex = 5;
			this.textBox2.TabStop = false;
			this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// ImportSigScan
			// 
			this.AcceptButton = this.button1;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(342, 216);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(350, 250);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(350, 250);
			this.Name = "ImportSigScan";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Genename";
			this.Load += new System.EventHandler(this.Genename_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        private void readSigScan(string file, string gene)
        {
            string text2 = "";
            int num1 = 0;
            try
            {
                if (!File.Exists(file))
                {
                    MessageBox.Show("Can not find file!");
                }
                else
                {
                    StreamReader reader1 = new StreamReader(file);
                    while (reader1.Peek() != -1)
                    {
                        string text1 = reader1.ReadLine().Trim();
                        DataRow row1 = this.tf.Factor.NewRow();
                        DataRow row2 = this.tf.FactorPosition.NewRow();
                        if (text1 == "Processed sequence:")
                        {
                            text1 = reader1.ReadLine().Trim();
                            text1 = reader1.ReadLine().Trim();
                            text2 = "";
                            for (int num2 = 0; num2 < text1.Length; num2++)
                            {
                                if (char.IsLetter(text1, num2))
                                {
                                    text2 = text2 + text1.Substring(num2, 1);
                                }
                            }
                            if (!this.tf.Sequence.Rows.Contains(gene))
                            {
                                DataRow row3 = this.tf.Sequence.NewRow();
                                row3[1] = gene;
                                row3[2] = text2;
                                if (!this.tf.Sequence.Rows.Contains(gene))
                                {
                                    this.tf.Sequence.Rows.Add(row3);
                                }
                                else
                                {
                                    MessageBox.Show("This gene already loaded!");
                                    return;
                                }
                            }
                        }
                        if (text1.Trim().Length >= 0x4c)
                        {
                            text2 = text1.Substring(0x16, 4).Trim();
                            if ((text2 == "site") || (text2 == "fact"))
                            {
                                if (text2 == "site")
                                {
                                    row1[2] = text1.Substring(0, 0x16).Trim();
                                }
                                else if (text2 == "fact")
                                {
                                    row1[1] = text1.Substring(0, 0x16).Trim();
                                }
                                row2[3] = text1.Substring(0x22, 3) + text1.Substring(0x1a, 8).Trim();
                                row1[3] = text1.Substring(0x26, 0x1f).Trim();
                                row1[5] = text1.Substring(70, 6).Trim();
                                row2[2] = row1[5];
                                row2[1] = gene;
                                if (!this.tf.Factor.Rows.Contains(row1[5]))
                                {
                                    this.tf.Factor.Rows.Add(row1);
                                }
                                this.tf.FactorPosition.Rows.Add(row2);
                                num1++;
                            }
                        }
                    }
                    reader1.Close();
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Read SigScan WWW Search Result Error: " + exception1.Message);
            }
        }


        private Button button1;
        private Button button2;
        private Container components;
        private Label label2;
        private OpenFileDialog ofDlg;
        public TextBox textBox1;
        private TextBox textBox2;
        public TransFactor tf;
    }
}

