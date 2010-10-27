namespace FacScan
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Threading;
    using System.Windows.Forms;

    public class GeneForm9999 : Form
    {
        public GeneForm9999(TransFactor t)
        {
            this.components = null;
            this.dt = new DataTable("uni");
            this.InitializeComponent();
            base.StartPosition = FormStartPosition.CenterScreen;
            this.tf = t;
        }

        private void addToTable(string map, string s)
        {
            string text1 = this.textBox2.Text;
            string text2 = "";
            string text3 = "";
            try
            {
                text2 = map.Substring(map.IndexOf("..") + 2);
                int num1 = (Convert.ToInt32(text2) / 0x270f) - 1;
                text3 = this.readSeq(text1, num1);
                DataRow row1 = this.sq.NewRow();
                row1[1] = s;
                row1[2] = text3.Substring(text3.Length - Convert.ToInt32(this.txbBP.Text.ToString()), Convert.ToInt32(this.txbLen.Text.ToString()));
                this.sq.Rows.Add(row1);
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Add to Table Error: " + exception1.Message, "Error");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.textBox1.Text != "")
            {
                int num1 = Convert.ToInt32(this.txbBP.Text);
                int num2 = Convert.ToInt32(this.txbLen.Text);
                if (((num1 <= 0) || (num1 > 0x270f)) || (((num2 < 0) || (num2 > 0x270f)) || (num2 > num1)))
                {
                    MessageBox.Show("Error in Start Position and Length for search.Maximum Start and Length is 9999. Try again.", "Error");
                }
                else
                {
                    try
                    {
                        ArrayList list1 = new ArrayList();
                        ArrayList list2 = new ArrayList();
                        list2 = this.parseInput(this.textBox1.Text);
                        Thread thread1 = new Thread(new ThreadStart(this.ORFthread));
                        thread1.Start();
                        thread1.Join();
                        if (this.dt != null)
                        {
                            this.dt.PrimaryKey = new DataColumn[] { this.dt.Columns["Systematic Name"] };
                            splitTable table1 = new splitTable();
                            table1.TableSplit(this.dt, "Common");
                            DataTable table2 = table1.dtUnique;
                            DataTable table3 = table1.dtMultiple;
                            table1 = new splitTable();
                            table1.TableSplit(this.dt, "Genbank");
                            DataTable table4 = table1.dtUnique;
                            DataTable table5 = table1.dtMultiple;
                            this.sq = this.tf.Sequence.Clone();
                            string text1 = "";
                            int num3 = 0;
                            this.pgrBar.Maximum = list2.Count;
                            this.pgrBar.Value = 0;
                            foreach (string text2 in list2)
                            {
                                DataRow row1;
                                this.pgrBar.Value++;
                                if (this.rbnName.Checked)
                                {
                                    row1 = table2.Rows.Find(text2);
                                    if (row1 != null)
                                    {
                                        text1 = row1["Map"].ToString();
                                        this.addToTable(text1, text2);
                                        continue;
                                    }
                                    text1 = "";
                                    for (int num4 = 0; num4 < table3.Rows.Count; num4++)
                                    {
                                        if (text2 == table3.Rows[num4]["Common"].ToString())
                                        {
                                            text1 = table3.Rows[num4]["Map"].ToString();
                                            string text3 = text2 + " _ " + num3.ToString();
                                            num3++;
                                            this.addToTable(text1, text3);
                                        }
                                    }
                                    if (text1 == "")
                                    {
                                        list1.Add(text2);
                                    }
                                }
                                else if (this.rbnGB.Checked)
                                {
                                    row1 = table4.Rows.Find(text2);
                                    if (row1 != null)
                                    {
                                        text1 = row1["Map"].ToString();
                                        this.addToTable(text1, text2);
                                        continue;
                                    }
                                    text1 = "";
                                    for (int num5 = 0; num5 < table5.Rows.Count; num5++)
                                    {
                                        if (text2 == table5.Rows[num5]["Genbank"].ToString())
                                        {
                                            this.addToTable(text1, text2);
                                        }
                                    }
                                    if (text1 == "")
                                    {
                                        list1.Add(text2);
                                    }
                                }
                            }
                            this.pgrBar.Value = this.pgrBar.Maximum;
                            if (list1.Count > 0)
                            {
                                string text4 = "";
                                foreach (string text5 in list1)
                                {
                                    text4 = text4 + text5 + "\t";
                                }
                                StreamWriter writer1 = new StreamWriter("FactScan.log", true);
                                writer1.WriteLine(DateTime.Now.ToString());
                                writer1.WriteLine("Can not find the following {0} genes in 9999 database:\r\n\r\n" + text4, list1.Count);
                                writer1.WriteLine();
                                writer1.Close();
                                MessageBox.Show("Can not find the following " + list1.Count.ToString() + " genes:\r\n\r\n" + text4, "Not found items");
                            }
                            this.btnCancel.Text = "Finish";
                            this.btnCancel.Focus();
                            MessageBox.Show("Search Completed successfully!\r\n" + this.sq.Rows.Count.ToString() + " sequences added!", "Done");
                        }
                    }
                    catch (Exception exception1)
                    {
                        MessageBox.Show("Read 9999 Sequence Database Error: " + exception1.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ofDlg.Filter = "Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.ofDlg.DefaultExt = "Text (*.txt)|*.txt";
            if (this.ofDlg.ShowDialog() == DialogResult.OK)
            {
                this.textBox2.Text = this.ofDlg.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.ofDlg.Filter = "Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.ofDlg.DefaultExt = "Text (*.txt)|*.txt";
            if (this.ofDlg.ShowDialog() == DialogResult.OK)
            {
                this.textBox3.Text = this.ofDlg.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.ofDlg.Filter = "Text (*.txt)|*.txt|All Files (*.*)|*.*";
            this.ofDlg.DefaultExt = "Text (*.txt)|*.txt";
            try
            {
                if (this.ofDlg.ShowDialog() == DialogResult.OK)
                {
                    StreamReader reader1 = new StreamReader(this.ofDlg.FileName);
                    this.textBox1.Text = "";
                    while (reader1.Peek() >= 0)
                    {
                        string text1 = reader1.ReadLine().Trim();
                        this.textBox1.Text = this.textBox1.Text + text1 + "\r\n";
                    }
                    reader1.Close();
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Read Gene Name Error: " + exception1.Message);
            }
        }

        private string ConvertToString(byte[] c)
        {
            string text1 = "";
            for (int num1 = 0; num1 < c.Length; num1++)
            {
                char ch1 = (char) c[num1];
                text1 = text1 + ch1.ToString().Trim();
            }
            return text1;
        }

        private string ConvertToString(char[] c)
        {
            string text1 = "";
            for (int num1 = 0; num1 < c.Length; num1++)
            {
                text1 = text1 + c[num1].ToString().Trim();
            }
            return text1;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void GeneForm_Load(object sender, EventArgs e)
        {
            this.textBox2.Text = @"C:\Program Files\SiliconGenetics\GeneSpring\data\HumanGenome9999\Homo sapiens 9999.seq";
            this.textBox3.Text = @"C:\Program Files\SiliconGenetics\GeneSpring\data\HumanGenome9999\HumanGenome9_ORFs.txt";
            this.label2.Text = "Names or Accession numbers should be seperated by space, tab, new line or semicolon. Use ^M (Key Combination: Ctrl + M) to add a new line in the text box above";
            this.btnCancel.Text = "Cancel";
        }

        private void InitializeComponent()
        {
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.ofDlg = new System.Windows.Forms.OpenFileDialog();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.rbnGB = new System.Windows.Forms.RadioButton();
			this.rbnName = new System.Windows.Forms.RadioButton();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.txbBP = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.txbLen = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.pgrBar = new System.Windows.Forms.ProgressBar();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(136, 432);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(80, 32);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(360, 432);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(80, 32);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(128, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(336, 32);
			this.label1.TabIndex = 5;
			this.label1.Text = "Paste Gene Names or Accession Numbers";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(64, 280);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(472, 32);
			this.label2.TabIndex = 7;
			this.label2.Text = "Names or Accession numbers should be seperated by space, tab, new line or semicol" +
				"on.";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.textBox1);
			this.groupBox1.Location = new System.Drawing.Point(16, 56);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(264, 216);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "1. Paste gene names";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 56);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(152, 23);
			this.label5.TabIndex = 15;
			this.label5.Text = "Or paste names here";
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(16, 24);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(128, 23);
			this.button3.TabIndex = 14;
			this.button3.Text = "Import from file";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(8, 80);
			this.textBox1.Multiline = true;
			this.textBox1.Name = "textBox1";
			this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox1.Size = new System.Drawing.Size(248, 128);
			this.textBox1.TabIndex = 13;
			this.textBox1.WordWrap = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.rbnGB);
			this.groupBox2.Controls.Add(this.rbnName);
			this.groupBox2.Location = new System.Drawing.Point(296, 56);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(272, 112);
			this.groupBox2.TabIndex = 14;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "2. Type of the input";
			// 
			// rbnGB
			// 
			this.rbnGB.Location = new System.Drawing.Point(24, 64);
			this.rbnGB.Name = "rbnGB";
			this.rbnGB.Size = new System.Drawing.Size(112, 24);
			this.rbnGB.TabIndex = 8;
			this.rbnGB.Text = "GeneBank Acc";
			// 
			// rbnName
			// 
			this.rbnName.Checked = true;
			this.rbnName.Location = new System.Drawing.Point(24, 32);
			this.rbnName.Name = "rbnName";
			this.rbnName.Size = new System.Drawing.Size(88, 24);
			this.rbnName.TabIndex = 7;
			this.rbnName.TabStop = true;
			this.rbnName.Text = "Gene Name";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.button1);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Controls.Add(this.textBox2);
			this.groupBox3.Controls.Add(this.textBox3);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.button2);
			this.groupBox3.Location = new System.Drawing.Point(16, 312);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(552, 88);
			this.groupBox3.TabIndex = 15;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "4. Database location";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(492, 24);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(24, 24);
			this.button1.TabIndex = 16;
			this.button1.Text = "...";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 24);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 16);
			this.label3.TabIndex = 14;
			this.label3.Text = "Sequence Database";
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(132, 24);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(352, 20);
			this.textBox2.TabIndex = 11;
			this.textBox2.Text = "textBox2";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(132, 56);
			this.textBox3.Name = "textBox3";
			this.textBox3.Size = new System.Drawing.Size(352, 20);
			this.textBox3.TabIndex = 12;
			this.textBox3.Text = "textBox3";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 56);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(112, 16);
			this.label4.TabIndex = 13;
			this.label4.Text = "Description Database";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(492, 56);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(24, 24);
			this.button2.TabIndex = 15;
			this.button2.Text = "...";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.label6);
			this.groupBox4.Controls.Add(this.txbBP);
			this.groupBox4.Controls.Add(this.label7);
			this.groupBox4.Controls.Add(this.txbLen);
			this.groupBox4.Controls.Add(this.label8);
			this.groupBox4.Controls.Add(this.label9);
			this.groupBox4.Location = new System.Drawing.Point(296, 176);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(272, 96);
			this.groupBox4.TabIndex = 16;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "3. Basepairs to be searched";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(8, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(32, 16);
			this.label6.TabIndex = 1;
			this.label6.Text = "From";
			// 
			// txbBP
			// 
			this.txbBP.Location = new System.Drawing.Point(64, 24);
			this.txbBP.Name = "txbBP";
			this.txbBP.Size = new System.Drawing.Size(64, 20);
			this.txbBP.TabIndex = 0;
			this.txbBP.Text = "1000";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(8, 56);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(48, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Length";
			// 
			// txbLen
			// 
			this.txbLen.Location = new System.Drawing.Point(64, 56);
			this.txbLen.Name = "txbLen";
			this.txbLen.Size = new System.Drawing.Size(64, 20);
			this.txbLen.TabIndex = 0;
			this.txbLen.Text = "1000";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(136, 24);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(120, 16);
			this.label8.TabIndex = 1;
			this.label8.Text = "bp before Start Codon";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(136, 56);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(48, 16);
			this.label9.TabIndex = 1;
			this.label9.Text = "bp";
			// 
			// pgrBar
			// 
			this.pgrBar.Location = new System.Drawing.Point(16, 408);
			this.pgrBar.Name = "pgrBar";
			this.pgrBar.Size = new System.Drawing.Size(552, 10);
			this.pgrBar.TabIndex = 17;
			// 
			// GeneForm9999
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(594, 468);
			this.Controls.Add(this.pgrBar);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(600, 500);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(600, 500);
			this.Name = "GeneForm9999";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Gene name input";
			this.Load += new System.EventHandler(this.GeneForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);

        }

        private void ORFthread()
        {
            this.dt = this.readORF(this.textBox3.Text);
        }

        private ArrayList parseInput(string input)
        {
            ArrayList list1 = new ArrayList();
            string text1 = "";
            input = input + " ";
            for (int num1 = 0; num1 < input.Length; num1++)
            {
                string text2 = input.Substring(num1, 1).Trim();
                if (((text2 == "") || (text2 == ";")) && (text1 != ""))
                {
                    if (!list1.Contains(text1))
                    {
                        list1.Add(text1.ToUpper());
                    }
                    text1 = "";
                }
                else
                {
                    text1 = text1 + text2;
                }
            }
            return list1;
        }

        private DataTable readORF(string file)
        {
            DataTable table1 = new DataTable("orf");
            try
            {
                StreamReader reader1 = new StreamReader(file);
                string text1 = reader1.ReadLine().Trim();
                if (text1 != "# GeneSpring v.7.2")
                {
                    MessageBox.Show("Error: This is not GeneSpring ORF file. First line should be # GeneSpring v.7.2");
                    reader1.Close();
                    return null;
                }
                text1 = reader1.ReadLine().Trim();
                if (text1.Substring(0, 0x29) != "# Master Table of Genes (ORFs) for genome")
                {
                    DialogResult result1 = MessageBox.Show("Error: This is not GeneSpring Sequence Database.Second line should start with # Master Table of Genes (ORFs) for genome ....Genome9999. Continue anyway?", "Error", MessageBoxButtons.YesNo);
                    if (result1 != DialogResult.Yes)
                    {
                        reader1.Close();
                        return null;
                    }
                }
                string[] textArray1 = reader1.ReadLine().Trim().Split(new char[] { '\t' });
                for (int num1 = 0; num1 < textArray1.Length; num1++)
                {
                    table1.Columns.Add(textArray1[num1]);
                }
                this.pgrBar.Minimum = 0;
                this.pgrBar.Maximum = 0x55f0;
                while (reader1.Peek() >= 0)
                {
                    textArray1 = reader1.ReadLine().Trim().Split(new char[] { '\t' });
                    table1.Rows.Add(textArray1);
                    this.pgrBar.Value++;
                }
                reader1.Close();
                this.pgrBar.Value = this.pgrBar.Maximum;
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Read ORF Table Error: " + exception1.Message);
            }
            return table1;
        }

        private string readSeq(string file, int index)
        {
            string text1 = "";
            int num1 = 0x278c;
            try
            {
                FileStream stream1 = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] buffer1 = new byte[3];
                stream1.Read(buffer1, 0, 3);
                text1 = this.ConvertToString(buffer1).Trim();
                if (text1 != ">1")
                {
                    MessageBox.Show("Error: This is not GeneSpring Sequence Database. First line should be >1");
                    stream1.Close();
                    return null;
                }
                byte[] buffer2 = new byte[num1];
                stream1.Seek((long) ((num1 * index) + 3), SeekOrigin.Begin);
                stream1.Read(buffer2, 0, num1);
                text1 = this.ConvertToString(buffer2).Trim();
                stream1.Close();
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Read Sequence Database Error: " + exception1.Message);
            }
            return text1.ToUpper();
        }


        private Button btnCancel;
        private Button btnOK;
        private Button button1;
        private Button button2;
        private Button button3;
        private Container components;
        private DataTable dt;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private OpenFileDialog ofDlg;
        private ProgressBar pgrBar;
        private RadioButton rbnGB;
        private RadioButton rbnName;
        public DataTable sq;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TransFactor tf;
        private TextBox txbBP;
        private TextBox txbLen;


        private class splitTable
        {
            public void TableSplit(DataTable dt, string colName)
            {
                this.dtMult = dt.Clone();
                this.dtUniq = dt.Clone();
                this.dtUniq.PrimaryKey = new DataColumn[] { this.dtUniq.Columns[colName] };
                for (int num1 = 0; num1 < dt.Rows.Count; num1++)
                {
                    string text1 = dt.Rows[num1][colName].ToString().Trim();
                    if (text1 != "")
                    {
                        DataRow row1 = this.dtUniq.Rows.Find(text1);
                        if (row1 == null)
                        {
                            this.dtUniq.Rows.Add(dt.Rows[num1].ItemArray);
                        }
                        else
                        {
                            this.dtMult.Rows.Add(row1.ItemArray);
                            this.dtMult.Rows.Add(dt.Rows[num1].ItemArray);
                            this.dtUniq.Rows.Remove(row1);
                        }
                    }
                }
            }


            public DataTable dtMultiple
            {
                get
                {
                    return this.dtMult;
                }
                set
                {
                    this.dtMult = value;
                }
            }

            public DataTable dtUnique
            {
                get
                {
                    return this.dtUniq;
                }
                set
                {
                    this.dtUniq = value;
                }
            }


            private DataTable dtMult;
            private DataTable dtUniq;
        }
    }
}

