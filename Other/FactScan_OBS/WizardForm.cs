namespace FacScan
{
	using System;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Windows.Forms;

	public partial class WizardForm : Form
	{
		public bool cancel;
		private string cust;
		private string SeqFile;
		private string TFD;
		private string TransFact;

		public WizardForm()
		{
			this.TFD = "TFDSitesMamm.fdb";
			this.cust = "SampleSites42.fdb";
			this.TransFact = "TransFact6Human.fdb";
			this.SeqFile = "SampleSequences19.sdb";
			this.InitializeComponent();
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.cancel = true;
			base.Close();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			this.cancel = false;
			base.Hide();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (File.Exists(@"help\Help.htm"))
			{
				Process.Start(@"help\Help.htm");
			}
			else
			{
				MessageBox.Show("Can not find help file - Help.htm");
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.opnFD.Filter = "Sites (*.fdb)|*.fdb|Text (*.txt)|*.txt|All Files (*.*)|*.*";
			this.opnFD.DefaultExt = "Sites (*.fdb)|*.fdb";
			if (this.opnFD.ShowDialog(this) == DialogResult.OK)
			{
				this.txbFactFile.Text = this.opnFD.FileName;
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			this.opnFD.Filter = "Sequences (*.sdb)|*.sdb|Text (*.txt)|*.txt|All Files (*.*)|*.*";
			this.opnFD.DefaultExt = "Text (*.txt)|*.txt";
			if (this.opnFD.ShowDialog(this) == DialogResult.OK)
			{
				this.txbSeqFile.Text = this.opnFD.FileName;
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				this.VisitLink();
			}
			catch (Exception exception1)
			{
				MessageBox.Show("Unable to open link that was clicked." + exception1.Message);
			}
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			this.txbFactFile.Text = this.TransFact;
			this.txbFactFile.Enabled = false;
			this.button3.Enabled = false;
		}

		private void radioButton3_CheckedChanged(object sender, EventArgs e)
		{
			this.txbFactFile.Text = this.cust;
			this.txbFactFile.Enabled = true;
			this.button3.Enabled = true;
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			this.txbSeqFile.Enabled = true;
			this.button4.Enabled = true;
			this.txbSeq.Enabled = false;
			this.txbSeq.Text = "Paste seuqences in FASTA format";
		}

		private void radioButton5_CheckedChanged(object sender, EventArgs e)
		{
			this.txbSeq.Enabled = true;
			this.txbSeqFile.Enabled = false;
			this.button4.Enabled = false;
		}

		private void rbnTFD_CheckedChanged(object sender, EventArgs e)
		{
			this.txbFactFile.Text = this.TFD;
			this.txbFactFile.Enabled = false;
			this.button3.Enabled = false;
		}

		private void rbnUseDB_CheckedChanged(object sender, EventArgs e)
		{
			this.txbSeqFile.Enabled = false;
			this.txbSeq.Enabled = false;
			this.txbSeq.Text = "Paste seuqences in FASTA format";
		}

		private void textBox2_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.txbSeq.Text == "Paste seuqences in FASTA format")
			{
				this.txbSeq.Text = "";
			}
		}

		private void txbFactFile_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.txbFactFile.Text == this.cust)
			{
				this.txbFactFile.Text = "";
			}
		}

		private void VisitLink()
		{
			this.linkLabel1.LinkVisited = true;
			Process.Start("http://www.ncbi.nlm.nih.gov/BLAST/fasta.shtml");
		}

		private void WizardForm_Closed(object sender, EventArgs e)
		{
			this.cancel = true;
		}

		private void WizardForm_Load(object sender, EventArgs e)
		{
			this.rbnTFD.Checked = false;
			this.rbnTransFact.Checked = false;
			this.rbnCust.Checked = true;
			this.txbFactFile.Enabled = true;
			this.button3.Enabled = true;
			this.rbnSeqFile.Checked = true;
			this.txbSeq.Enabled = false;
			this.txbFactFile.Text = this.cust;
			this.txbSeqFile.Text = this.SeqFile;
			this.btnOK.Focus();
		}
	}
}

