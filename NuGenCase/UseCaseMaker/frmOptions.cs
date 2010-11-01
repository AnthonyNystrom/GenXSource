using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace UseCaseMaker
{
	/// <summary>
	/// Descrizione di riepilogo per frmOptions.
	/// </summary>
	public class frmOptions : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabOptions;
		private System.Windows.Forms.TabPage pgOptLanguages;
		private System.Windows.Forms.Label lblSelectLanguageTitle;
		private System.Windows.Forms.ListView lvOptLanguages;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ColumnHeader chDescription;
		private System.Windows.Forms.ColumnHeader chCountry;
		private System.Windows.Forms.ColumnHeader chRefCode;

		private string previousLanguage = string.Empty;
		public string SelectedLanguage = string.Empty;
		private ApplicationSettings appSettings = null;
		private Localizer localizer = null;

		public frmOptions(ApplicationSettings appSettings, Localizer localizer)
		{
			//
			// Necessario per il supporto di Progettazione Windows Form
			//
			InitializeComponent();

			//
			// TODO: aggiungere il codice del costruttore dopo la chiamata a InitializeComponent
			//
			XmlDocument locals = new XmlDocument();
			try
			{
				locals.Load(appSettings.LanguagesFilePath + Path.DirectorySeparatorChar + "Localization_list.xml");
			}
			catch(Exception)
			{
				MessageBox.Show(
					this,
					"Cannot load the localization list file!",
					Application.ProductName,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
				return;
			}

			try
			{
				XmlNode localsNode = locals.SelectSingleNode("//Localizations");
				foreach(XmlNode node in localsNode.ChildNodes)
				{
					if(node.NodeType == XmlNodeType.Element &&
						node.Name == "Localization")
					{
						if(node.Attributes["Description"] != null &&
							node.Attributes["Country"] != null && 
							node.Attributes["RefCode"] != null)
						{
							ListViewItem lvi = new ListViewItem();
							lvi.Text = node.Attributes["Description"].Value;
							lvi.SubItems.Add(node.Attributes["Country"].Value);
							lvi.SubItems.Add(node.Attributes["RefCode"].Value);
							lvOptLanguages.Items.Add(lvi);
						}
					}
				}
			}
			catch(XmlException)
			{
				MessageBox.Show(
					this,
					"Malformed xml format!",
					Application.ProductName,
					MessageBoxButtons.OK,
					MessageBoxIcon.Hand);
			}

			foreach(ListViewItem lvi in lvOptLanguages.Items)
			{
				if(lvi.SubItems[2].Text == appSettings.UILanguage)
				{
					lvi.Selected = true;
					break;
				}
			}

			this.previousLanguage = appSettings.UILanguage;
			this.appSettings = appSettings;
			this.localizer = localizer;
			localizer.LocalizeControls(this);
		}

		#region Codice generato da Progettazione Windows Form
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			this.tabOptions = new System.Windows.Forms.TabControl();
			this.pgOptLanguages = new System.Windows.Forms.TabPage();
			this.lvOptLanguages = new System.Windows.Forms.ListView();
			this.chDescription = new System.Windows.Forms.ColumnHeader();
			this.chCountry = new System.Windows.Forms.ColumnHeader();
			this.chRefCode = new System.Windows.Forms.ColumnHeader();
			this.lblSelectLanguageTitle = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.tabOptions.SuspendLayout();
			this.pgOptLanguages.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabOptions
			// 
			this.tabOptions.Controls.Add(this.pgOptLanguages);
			this.tabOptions.Location = new System.Drawing.Point(8, 8);
			this.tabOptions.Name = "tabOptions";
			this.tabOptions.SelectedIndex = 0;
			this.tabOptions.Size = new System.Drawing.Size(424, 240);
			this.tabOptions.TabIndex = 0;
			// 
			// pgOptLanguages
			// 
			this.pgOptLanguages.Controls.Add(this.lvOptLanguages);
			this.pgOptLanguages.Controls.Add(this.lblSelectLanguageTitle);
			this.pgOptLanguages.Location = new System.Drawing.Point(4, 22);
			this.pgOptLanguages.Name = "pgOptLanguages";
			this.pgOptLanguages.Size = new System.Drawing.Size(416, 214);
			this.pgOptLanguages.TabIndex = 0;
			this.pgOptLanguages.Text = "[Available languages]";
			// 
			// lvOptLanguages
			// 
			this.lvOptLanguages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																							 this.chDescription,
																							 this.chCountry,
																							 this.chRefCode});
			this.lvOptLanguages.FullRowSelect = true;
			this.lvOptLanguages.GridLines = true;
			this.lvOptLanguages.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvOptLanguages.HideSelection = false;
			this.lvOptLanguages.Location = new System.Drawing.Point(8, 32);
			this.lvOptLanguages.Name = "lvOptLanguages";
			this.lvOptLanguages.Size = new System.Drawing.Size(400, 176);
			this.lvOptLanguages.TabIndex = 1;
			this.lvOptLanguages.View = System.Windows.Forms.View.Details;
			this.lvOptLanguages.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvOptLanguages_MouseUp);
			this.lvOptLanguages.SelectedIndexChanged += new System.EventHandler(this.lvOptLanguages_SelectedIndexChanged);
			// 
			// chDescription
			// 
			this.chDescription.Width = 120;
			// 
			// chCountry
			// 
			this.chCountry.Width = 210;
			// 
			// chRefCode
			// 
			this.chRefCode.Width = 50;
			// 
			// lblSelectLanguageTitle
			// 
			this.lblSelectLanguageTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblSelectLanguageTitle.Location = new System.Drawing.Point(8, 8);
			this.lblSelectLanguageTitle.Name = "lblSelectLanguageTitle";
			this.lblSelectLanguageTitle.Size = new System.Drawing.Size(400, 16);
			this.lblSelectLanguageTitle.TabIndex = 0;
			this.lblSelectLanguageTitle.Text = "[Select the user interface language]";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(223, 256);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 23);
			this.btnCancel.TabIndex = 3;
			this.btnCancel.Text = "[Cancel]";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(95, 256);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(120, 23);
			this.btnOK.TabIndex = 2;
			this.btnOK.Text = "[OK]";
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// frmOptions
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(439, 288);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.tabOptions);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmOptions";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "[Options]";
			this.tabOptions.ResumeLayout(false);
			this.pgOptLanguages.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		private void lvOptLanguages_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lvOptLanguages.SelectedIndices.Count == 0)
			{
				this.SelectedLanguage = string.Empty;
				btnOK.Enabled = false;
			}
			else
			{
				this.SelectedLanguage = lvOptLanguages.SelectedItems[0].SubItems[2].Text;
				btnOK.Enabled = true;
			} 
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			string localPath =
				this.appSettings.LanguagesFilePath +
				Path.DirectorySeparatorChar +
				this.appSettings.LanguageFileNamePrefix +
				this.SelectedLanguage  + ".xml";

			if(!File.Exists(localPath))
			{
				MessageBox.Show(
					this,
					this.localizer.GetValue("UserMessages","cannotOpenFile"),
					Application.ProductName,
					MessageBoxButtons.OK,
					MessageBoxIcon.Stop);
				this.SelectedLanguage = this.previousLanguage;
			}
		}

		private void lvOptLanguages_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(lvOptLanguages.SelectedIndices.Count == 0)
			{
				if(lvOptLanguages.Items.Count > 0)
				{
					lvOptLanguages.Items[lvOptLanguages.Items.Count-1].Selected = true;
				}
				return;
			}		
		}
	}
}
