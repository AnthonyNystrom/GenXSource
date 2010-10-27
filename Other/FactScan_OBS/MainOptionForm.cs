namespace FacScan
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class MainOptionForm : Form
    {
        public MainOptionForm()
        {
            this.components = null;
            this.iniFile = "FacScan.ini";
            this.InitializeComponent();
            this.txbCFnumber.Text = "2";
            this.readIniFile();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void InitializeComponent()
        {
			this.button1 = new System.Windows.Forms.Button();
			this.ckbShowWizard = new System.Windows.Forms.CheckBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.ckbSearchCommon = new System.Windows.Forms.CheckBox();
			this.ckbSearchSingle = new System.Windows.Forms.CheckBox();
			this.ckbShowLineV = new System.Windows.Forms.CheckBox();
			this.ckbShowSeqV = new System.Windows.Forms.CheckBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.ckbShowSeq = new System.Windows.Forms.CheckBox();
			this.ckbWrap = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.txbCFnumber = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.txbPattLen = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(144, 264);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(88, 24);
			this.button1.TabIndex = 2;
			this.button1.Text = "OK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// ckbShowWizard
			// 
			this.ckbShowWizard.Checked = true;
			this.ckbShowWizard.CheckState = System.Windows.Forms.CheckState.Checked;
			this.ckbShowWizard.Location = new System.Drawing.Point(8, 16);
			this.ckbShowWizard.Name = "ckbShowWizard";
			this.ckbShowWizard.Size = new System.Drawing.Size(144, 24);
			this.ckbShowWizard.TabIndex = 3;
			this.ckbShowWizard.Text = "Show Wizard at Start";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.ckbSearchCommon);
			this.groupBox1.Controls.Add(this.ckbSearchSingle);
			this.groupBox1.Controls.Add(this.ckbShowLineV);
			this.groupBox1.Controls.Add(this.ckbShowSeqV);
			this.groupBox1.Location = new System.Drawing.Point(8, 40);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(200, 152);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Execute following Wizard";
			// 
			// ckbSearchCommon
			// 
			this.ckbSearchCommon.Location = new System.Drawing.Point(8, 50);
			this.ckbSearchCommon.Name = "ckbSearchCommon";
			this.ckbSearchCommon.Size = new System.Drawing.Size(184, 24);
			this.ckbSearchCommon.TabIndex = 7;
			this.ckbSearchCommon.Text = "Search common sites shared";
			// 
			// ckbSearchSingle
			// 
			this.ckbSearchSingle.Location = new System.Drawing.Point(8, 24);
			this.ckbSearchSingle.Name = "ckbSearchSingle";
			this.ckbSearchSingle.Size = new System.Drawing.Size(184, 24);
			this.ckbSearchSingle.TabIndex = 6;
			this.ckbSearchSingle.Text = "Search sites in each gene";
			// 
			// ckbShowLineV
			// 
			this.ckbShowLineV.Location = new System.Drawing.Point(8, 76);
			this.ckbShowLineV.Name = "ckbShowLineV";
			this.ckbShowLineV.Size = new System.Drawing.Size(184, 24);
			this.ckbShowLineV.TabIndex = 7;
			this.ckbShowLineV.Text = "Show line view ";
			// 
			// ckbShowSeqV
			// 
			this.ckbShowSeqV.Location = new System.Drawing.Point(8, 102);
			this.ckbShowSeqV.Name = "ckbShowSeqV";
			this.ckbShowSeqV.Size = new System.Drawing.Size(184, 24);
			this.ckbShowSeqV.TabIndex = 7;
			this.ckbShowSeqV.Text = "Show Sequence view ";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.ckbShowSeq);
			this.groupBox2.Controls.Add(this.ckbWrap);
			this.groupBox2.Location = new System.Drawing.Point(216, 120);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(168, 72);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Search in Genes";
			// 
			// ckbShowSeq
			// 
			this.ckbShowSeq.Location = new System.Drawing.Point(8, 16);
			this.ckbShowSeq.Name = "ckbShowSeq";
			this.ckbShowSeq.Size = new System.Drawing.Size(124, 24);
			this.ckbShowSeq.TabIndex = 5;
			this.ckbShowSeq.Text = "Show Sequence";
			// 
			// ckbWrap
			// 
			this.ckbWrap.Location = new System.Drawing.Point(8, 40);
			this.ckbWrap.Name = "ckbWrap";
			this.ckbWrap.Size = new System.Drawing.Size(124, 24);
			this.ckbWrap.TabIndex = 4;
			this.ckbWrap.Text = "Wrap line";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.txbCFnumber);
			this.groupBox3.Controls.Add(this.label1);
			this.groupBox3.Location = new System.Drawing.Point(8, 200);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(376, 48);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Search Common";
			// 
			// txbCFnumber
			// 
			this.txbCFnumber.Location = new System.Drawing.Point(224, 16);
			this.txbCFnumber.Name = "txbCFnumber";
			this.txbCFnumber.Size = new System.Drawing.Size(80, 20);
			this.txbCFnumber.TabIndex = 3;
			this.txbCFnumber.Text = "2";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(200, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Show Common Factor Occurrence >=";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// groupBox4
			// 
			this.groupBox4.Controls.Add(this.txbPattLen);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.Location = new System.Drawing.Point(216, 40);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(168, 72);
			this.groupBox4.TabIndex = 5;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Sites";
			// 
			// txbPattLen
			// 
			this.txbPattLen.Location = new System.Drawing.Point(72, 40);
			this.txbPattLen.Name = "txbPattLen";
			this.txbPattLen.Size = new System.Drawing.Size(56, 20);
			this.txbPattLen.TabIndex = 5;
			this.txbPattLen.Text = "5";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(144, 24);
			this.label3.TabIndex = 4;
			this.label3.Text = "Skip pattern length (bases)";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(40, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(24, 16);
			this.label2.TabIndex = 4;
			this.label2.Text = "<";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// MainOptionForm
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(394, 303);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.ckbShowWizard);
			this.Controls.Add(this.groupBox4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainOptionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Main Options";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainOptionForm_Closing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);

        }

        private void MainOptionForm_Closing(object sender, CancelEventArgs e)
        {
            try
            {
				using (StreamWriter writer1 = File.CreateText(this.iniFile))
				{
					writer1.WriteLine("Common Factor Occurrence\t{0}", this.txbCFnumber.Text);
					writer1.WriteLine("Show Sequence\t{0}", this.ckbShowSeq.Checked.ToString());
					writer1.WriteLine("Wrap Line\t{0}", this.ckbWrap.Checked.ToString());
					writer1.WriteLine("Show Wizard\t{0}", this.ckbShowWizard.Checked.ToString());
					writer1.WriteLine("Search Single\t{0}", this.ckbSearchSingle.Checked.ToString());
					writer1.WriteLine("Search Common\t{0}", this.ckbSearchCommon.Checked.ToString());
					writer1.WriteLine("Show Ln View\t{0}", this.ckbShowLineV.Checked.ToString());
					writer1.WriteLine("Show Seq View\t{0}", this.ckbShowSeqV.Checked.ToString());
				}
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Save INI File Error: " + exception1.Message);
            }
        }

        private void readIniFile()
        {
            try
            {
                if (File.Exists("FacScan.ini"))
                {
					using (StreamReader reader1 = new StreamReader(this.iniFile))
					{
						while (reader1.Peek() != -1)
						{
							string[] textArray1 = reader1.ReadLine().Trim().Split(new char[] { '\t' });
							string text1 = textArray1[0];
							if (text1 != null)
							{
								text1 = string.IsInterned(text1);
								if (text1 == "Common Factor Occurrence")
								{
									this.txbCFnumber.Text = textArray1[1];
								}
								else
								{
									if (text1 == "Show Sequence")
									{
										if (textArray1[1] == "True")
										{
											this.ckbShowSeq.Checked = true;
											continue;
										}
										this.ckbShowSeq.Checked = false;
										continue;
									} // end if
									if (text1 == "Wrap Line")
									{
										if (textArray1[1] == "True")
										{
											this.ckbWrap.Checked = true;
											continue;
										}
										this.ckbWrap.Checked = false;
										continue;
									} // end if
									if (text1 == "Show Wizard")
									{
										if (textArray1[1] == "True")
										{
											this.ckbShowWizard.Checked = true;
											continue;
										}
										this.ckbShowWizard.Checked = false;
										continue;
									} // end if
									if (text1 == "Search Single")
									{
										if (textArray1[1] == "True")
										{
											this.ckbSearchSingle.Checked = true;
											continue;
										}
										this.ckbSearchSingle.Checked = false;
										continue;
									} // end if
									if (text1 == "Search Common")
									{
										if (textArray1[1] == "True")
										{
											this.ckbSearchCommon.Checked = true;
											continue;
										}
										this.ckbSearchCommon.Checked = false;
										continue;
									} // end if
									if (text1 == "Show Ln View")
									{
										if (textArray1[1] == "True")
										{
											this.ckbShowLineV.Checked = true;
											continue;
										}
										this.ckbShowLineV.Checked = false;
										continue;
									} // end if
									if (text1 == "Show Seq View")
									{
										if (textArray1[1] == "True")
										{
											this.ckbShowSeqV.Checked = true;
										}
										else
										{
											this.ckbShowSeqV.Checked = false;
										}
									} // end if
								} // end else
							} // end if
						} // end while
					}
                }
            }
            catch (Exception exception1)
            {
                MessageBox.Show("Read INI File Error: " + exception1.Message);
            }
        }


        private Button button1;
        public CheckBox ckbSearchCommon;
        public CheckBox ckbSearchSingle;
        public CheckBox ckbShowLineV;
        public CheckBox ckbShowSeq;
        public CheckBox ckbShowSeqV;
        public CheckBox ckbShowWizard;
        public CheckBox ckbWrap;
        private Container components;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private string iniFile;
        private Label label1;
        private Label label2;
        private Label label3;
        public TextBox txbCFnumber;
        public TextBox txbPattLen;
    }
}

