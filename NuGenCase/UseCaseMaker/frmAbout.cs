using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace UseCaseMaker
{
	/// <summary>
	/// Descrizione di riepilogo per frmAbout.
	/// </summary>
	public class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Panel pnlSep1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.Label lblUCMWebSiteTitle;
		private System.Windows.Forms.LinkLabel lnkLblUMCCredit1Mail;
		private System.Windows.Forms.Label lblUCMCredit1Name;
		private System.Windows.Forms.Label lblUCMCreditsTitle;
		private System.Windows.Forms.LinkLabel lnkLblUCMWebSite;
		private System.Windows.Forms.LinkLabel lnkLblUCMAuthorMail;
		private System.Windows.Forms.Label lblUCMAuthorName;
		private System.Windows.Forms.Label lblUCMAuthorTitle;
		private System.Windows.Forms.Label lblUCMVersion;
		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmAbout(bool isSplash)
		{
			//
			// Necessario per il supporto di Progettazione Windows Form
			//
			InitializeComponent();

			//
			// TODO: aggiungere il codice del costruttore dopo la chiamata a InitializeComponent
			//
			lblUCMVersion.Text = Application.ProductVersion;
			if(isSplash)
			{
				btnOK.Visible = false;
			}
		}

		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Codice generato da Progettazione Windows Form
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(frmAbout));
			this.pnlSep1 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.lblUCMWebSiteTitle = new System.Windows.Forms.Label();
			this.lnkLblUMCCredit1Mail = new System.Windows.Forms.LinkLabel();
			this.lblUCMCredit1Name = new System.Windows.Forms.Label();
			this.lblUCMCreditsTitle = new System.Windows.Forms.Label();
			this.lnkLblUCMWebSite = new System.Windows.Forms.LinkLabel();
			this.lnkLblUCMAuthorMail = new System.Windows.Forms.LinkLabel();
			this.lblUCMAuthorName = new System.Windows.Forms.Label();
			this.lblUCMAuthorTitle = new System.Windows.Forms.Label();
			this.lblUCMVersion = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.pnlSep1.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlSep1
			// 
			this.pnlSep1.BackColor = System.Drawing.Color.Transparent;
			this.pnlSep1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.pnlSep1.Controls.Add(this.label1);
			this.pnlSep1.Controls.Add(this.linkLabel1);
			this.pnlSep1.Controls.Add(this.lblUCMWebSiteTitle);
			this.pnlSep1.Controls.Add(this.lnkLblUMCCredit1Mail);
			this.pnlSep1.Controls.Add(this.lblUCMCredit1Name);
			this.pnlSep1.Controls.Add(this.lblUCMCreditsTitle);
			this.pnlSep1.Controls.Add(this.lnkLblUCMWebSite);
			this.pnlSep1.Controls.Add(this.lnkLblUCMAuthorMail);
			this.pnlSep1.Controls.Add(this.lblUCMAuthorName);
			this.pnlSep1.Controls.Add(this.lblUCMAuthorTitle);
			this.pnlSep1.Location = new System.Drawing.Point(24, 128);
			this.pnlSep1.Name = "pnlSep1";
			this.pnlSep1.Size = new System.Drawing.Size(504, 136);
			this.pnlSep1.TabIndex = 14;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label1.Location = new System.Drawing.Point(264, 72);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(216, 16);
			this.label1.TabIndex = 23;
			this.label1.Text = "GNU GPL & LGPL licenses:";
			this.label1.UseMnemonic = false;
			// 
			// linkLabel1
			// 
			this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Yellow;
			this.linkLabel1.BackColor = System.Drawing.Color.Transparent;
			this.linkLabel1.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.linkLabel1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(0, 27);
			this.linkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.linkLabel1.LinkColor = System.Drawing.Color.Yellow;
			this.linkLabel1.Location = new System.Drawing.Point(272, 88);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(216, 16);
			this.linkLabel1.TabIndex = 22;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "http://www.fsf.org/licenses";
			this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Yellow;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// lblUCMWebSiteTitle
			// 
			this.lblUCMWebSiteTitle.BackColor = System.Drawing.Color.Transparent;
			this.lblUCMWebSiteTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUCMWebSiteTitle.ForeColor = System.Drawing.Color.White;
			this.lblUCMWebSiteTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblUCMWebSiteTitle.Location = new System.Drawing.Point(264, 8);
			this.lblUCMWebSiteTitle.Name = "lblUCMWebSiteTitle";
			this.lblUCMWebSiteTitle.Size = new System.Drawing.Size(216, 16);
			this.lblUCMWebSiteTitle.TabIndex = 21;
			this.lblUCMWebSiteTitle.Text = "Web Site:";
			// 
			// lnkLblUMCCredit1Mail
			// 
			this.lnkLblUMCCredit1Mail.ActiveLinkColor = System.Drawing.Color.White;
			this.lnkLblUMCCredit1Mail.BackColor = System.Drawing.Color.Transparent;
			this.lnkLblUMCCredit1Mail.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lnkLblUMCCredit1Mail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lnkLblUMCCredit1Mail.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lnkLblUMCCredit1Mail.LinkArea = new System.Windows.Forms.LinkArea(0, 31);
			this.lnkLblUMCCredit1Mail.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.lnkLblUMCCredit1Mail.LinkColor = System.Drawing.Color.White;
			this.lnkLblUMCCredit1Mail.Location = new System.Drawing.Point(24, 104);
			this.lnkLblUMCCredit1Mail.Name = "lnkLblUMCCredit1Mail";
			this.lnkLblUMCCredit1Mail.Size = new System.Drawing.Size(208, 16);
			this.lnkLblUMCCredit1Mail.TabIndex = 20;
			this.lnkLblUMCCredit1Mail.TabStop = true;
			this.lnkLblUMCCredit1Mail.Text = "rufinelli@users.sourceforge.net";
			this.lnkLblUMCCredit1Mail.VisitedLinkColor = System.Drawing.Color.White;
			this.lnkLblUMCCredit1Mail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblUMCCredit1Mail_LinkClicked);
			// 
			// lblUCMCredit1Name
			// 
			this.lblUCMCredit1Name.BackColor = System.Drawing.Color.Transparent;
			this.lblUCMCredit1Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUCMCredit1Name.ForeColor = System.Drawing.Color.Yellow;
			this.lblUCMCredit1Name.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblUCMCredit1Name.Location = new System.Drawing.Point(24, 88);
			this.lblUCMCredit1Name.Name = "lblUCMCredit1Name";
			this.lblUCMCredit1Name.Size = new System.Drawing.Size(216, 16);
			this.lblUCMCredit1Name.TabIndex = 19;
			this.lblUCMCredit1Name.Text = "Marco Rufinelli";
			// 
			// lblUCMCreditsTitle
			// 
			this.lblUCMCreditsTitle.BackColor = System.Drawing.Color.Transparent;
			this.lblUCMCreditsTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUCMCreditsTitle.ForeColor = System.Drawing.Color.White;
			this.lblUCMCreditsTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblUCMCreditsTitle.Location = new System.Drawing.Point(16, 72);
			this.lblUCMCreditsTitle.Name = "lblUCMCreditsTitle";
			this.lblUCMCreditsTitle.Size = new System.Drawing.Size(216, 16);
			this.lblUCMCreditsTitle.TabIndex = 18;
			this.lblUCMCreditsTitle.Text = "Credits:";
			// 
			// lnkLblUCMWebSite
			// 
			this.lnkLblUCMWebSite.ActiveLinkColor = System.Drawing.Color.White;
			this.lnkLblUCMWebSite.BackColor = System.Drawing.Color.Transparent;
			this.lnkLblUCMWebSite.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lnkLblUCMWebSite.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lnkLblUCMWebSite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lnkLblUCMWebSite.LinkArea = new System.Windows.Forms.LinkArea(0, 37);
			this.lnkLblUCMWebSite.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.lnkLblUCMWebSite.LinkColor = System.Drawing.Color.Yellow;
			this.lnkLblUCMWebSite.Location = new System.Drawing.Point(272, 24);
			this.lnkLblUCMWebSite.Name = "lnkLblUCMWebSite";
			this.lnkLblUCMWebSite.Size = new System.Drawing.Size(208, 16);
			this.lnkLblUCMWebSite.TabIndex = 17;
			this.lnkLblUCMWebSite.TabStop = true;
			this.lnkLblUCMWebSite.Text = "http://use-case-maker.sourceforge.net";
			this.lnkLblUCMWebSite.VisitedLinkColor = System.Drawing.Color.White;
			this.lnkLblUCMWebSite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblUCMWebSite_LinkClicked);
			// 
			// lnkLblUCMAuthorMail
			// 
			this.lnkLblUCMAuthorMail.ActiveLinkColor = System.Drawing.Color.White;
			this.lnkLblUCMAuthorMail.BackColor = System.Drawing.Color.Transparent;
			this.lnkLblUCMAuthorMail.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lnkLblUCMAuthorMail.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lnkLblUCMAuthorMail.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lnkLblUCMAuthorMail.LinkArea = new System.Windows.Forms.LinkArea(0, 31);
			this.lnkLblUCMAuthorMail.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
			this.lnkLblUCMAuthorMail.LinkColor = System.Drawing.Color.White;
			this.lnkLblUCMAuthorMail.Location = new System.Drawing.Point(24, 40);
			this.lnkLblUCMAuthorMail.Name = "lnkLblUCMAuthorMail";
			this.lnkLblUCMAuthorMail.Size = new System.Drawing.Size(208, 16);
			this.lnkLblUCMAuthorMail.TabIndex = 16;
			this.lnkLblUCMAuthorMail.TabStop = true;
			this.lnkLblUCMAuthorMail.Text = "gaspardis@users.sourceforge.net";
			this.lnkLblUCMAuthorMail.VisitedLinkColor = System.Drawing.Color.White;
			this.lnkLblUCMAuthorMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLblUCMAuthorMail_LinkClicked);
			// 
			// lblUCMAuthorName
			// 
			this.lblUCMAuthorName.BackColor = System.Drawing.Color.Transparent;
			this.lblUCMAuthorName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUCMAuthorName.ForeColor = System.Drawing.Color.Yellow;
			this.lblUCMAuthorName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblUCMAuthorName.Location = new System.Drawing.Point(24, 24);
			this.lblUCMAuthorName.Name = "lblUCMAuthorName";
			this.lblUCMAuthorName.Size = new System.Drawing.Size(216, 16);
			this.lblUCMAuthorName.TabIndex = 15;
			this.lblUCMAuthorName.Text = "Gabriele Gaspardis";
			// 
			// lblUCMAuthorTitle
			// 
			this.lblUCMAuthorTitle.BackColor = System.Drawing.Color.Transparent;
			this.lblUCMAuthorTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUCMAuthorTitle.ForeColor = System.Drawing.Color.White;
			this.lblUCMAuthorTitle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.lblUCMAuthorTitle.Location = new System.Drawing.Point(16, 8);
			this.lblUCMAuthorTitle.Name = "lblUCMAuthorTitle";
			this.lblUCMAuthorTitle.Size = new System.Drawing.Size(216, 16);
			this.lblUCMAuthorTitle.TabIndex = 14;
			this.lblUCMAuthorTitle.Text = "Author:";
			// 
			// lblUCMVersion
			// 
			this.lblUCMVersion.BackColor = System.Drawing.Color.Transparent;
			this.lblUCMVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.lblUCMVersion.ForeColor = System.Drawing.Color.Navy;
			this.lblUCMVersion.Location = new System.Drawing.Point(472, 64);
			this.lblUCMVersion.Name = "lblUCMVersion";
			this.lblUCMVersion.Size = new System.Drawing.Size(56, 23);
			this.lblUCMVersion.TabIndex = 0;
			this.lblUCMVersion.Text = "[0.0.0.0]";
			this.lblUCMVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// btnOK
			// 
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(448, 312);
			this.btnOK.Name = "btnOK";
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// frmAbout
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.SystemColors.Control;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.ClientSize = new System.Drawing.Size(550, 350);
			this.ControlBox = false;
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.pnlSep1);
			this.Controls.Add(this.lblUCMVersion);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Use Case Maker";
			this.pnlSep1.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void lnkLblUCMAuthorMail_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("mailto:gaspardis@users.sourceforge.net");
		}

		private void lnkLblUMCCredit1Mail_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("mailto:rufinelli@users.sourceforge.net");
		}

		private void lnkLblUCMWebSite_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://use-case-maker.sourceforge.net");
		}

		private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.fsf.org/licenses");
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}
}
