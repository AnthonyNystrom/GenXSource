using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace UseCaseMaker
{
	/// <summary>
	/// Descrizione di riepilogo per ModelExplorer.
	/// </summary>
	public class frmNameChanger : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label lblOldNameTitle;
		public System.Windows.Forms.Label lblOldName;
		private System.Windows.Forms.Label lblNewNameTitle;
		public System.Windows.Forms.TextBox tbNewName;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.CheckBox cbNoReplace;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmNameChanger(Localizer localizer, string elementType)
		{
			//
			// Necessario per il supporto di Progettazione Windows Form
			//
			InitializeComponent();

			//
			// TODO: aggiungere il codice del costruttore dopo la chiamata a InitializeComponent
			//
			localizer.LocalizeControls(this);
			this.Text += ": " + elementType;
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
			this.lblOldNameTitle = new System.Windows.Forms.Label();
			this.lblOldName = new System.Windows.Forms.Label();
			this.lblNewNameTitle = new System.Windows.Forms.Label();
			this.tbNewName = new System.Windows.Forms.TextBox();
			this.cbNoReplace = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// lblOldNameTitle
			// 
			this.lblOldNameTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblOldNameTitle.Location = new System.Drawing.Point(8, 8);
			this.lblOldNameTitle.Name = "lblOldNameTitle";
			this.lblOldNameTitle.Size = new System.Drawing.Size(104, 16);
			this.lblOldNameTitle.TabIndex = 0;
			this.lblOldNameTitle.Text = "[Old name]";
			// 
			// lblOldName
			// 
			this.lblOldName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.lblOldName.Location = new System.Drawing.Point(128, 8);
			this.lblOldName.Name = "lblOldName";
			this.lblOldName.Size = new System.Drawing.Size(272, 20);
			this.lblOldName.TabIndex = 1;
			// 
			// lblNewNameTitle
			// 
			this.lblNewNameTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblNewNameTitle.Location = new System.Drawing.Point(8, 32);
			this.lblNewNameTitle.Name = "lblNewNameTitle";
			this.lblNewNameTitle.Size = new System.Drawing.Size(104, 16);
			this.lblNewNameTitle.TabIndex = 2;
			this.lblNewNameTitle.Text = "[New name]";
			// 
			// tbNewName
			// 
			this.tbNewName.Location = new System.Drawing.Point(128, 32);
			this.tbNewName.Name = "tbNewName";
			this.tbNewName.Size = new System.Drawing.Size(272, 20);
			this.tbNewName.TabIndex = 3;
			this.tbNewName.Text = "";
			this.tbNewName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbNewName_KeyPress);
			this.tbNewName.Validating += new System.ComponentModel.CancelEventHandler(this.tbNewName_Validating);
			this.tbNewName.TextChanged += new System.EventHandler(this.tbNewName_TextChanged);
			// 
			// cbNoReplace
			// 
			this.cbNoReplace.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbNoReplace.Location = new System.Drawing.Point(128, 56);
			this.cbNoReplace.Name = "cbNoReplace";
			this.cbNoReplace.Size = new System.Drawing.Size(272, 24);
			this.cbNoReplace.TabIndex = 4;
			this.cbNoReplace.Text = "[Don\'t replace occurrences]";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Enabled = false;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(80, 88);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(120, 23);
			this.btnOK.TabIndex = 27;
			this.btnOK.Text = "[OK]";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(208, 88);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 23);
			this.btnCancel.TabIndex = 28;
			this.btnCancel.Text = "[Cancel]";
			// 
			// frmNameChanger
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(410, 118);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cbNoReplace);
			this.Controls.Add(this.tbNewName);
			this.Controls.Add(this.lblNewNameTitle);
			this.Controls.Add(this.lblOldName);
			this.Controls.Add(this.lblOldNameTitle);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmNameChanger";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "[Rename element]";
			this.Load += new System.EventHandler(this.frmNameChanger_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void tbNewName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(tbNewName.Text == string.Empty)
			{
				return;
			}
			if(((frmMain)this.Owner).Model.FindElementByName(tbNewName.Text) != null)
			{
				e.Cancel = true;
				MessageBox.Show(this,"[Name already in use!]",Application.ProductName);
			}
		}

		private void tbNewName_TextChanged(object sender, System.EventArgs e)
		{
			if(tbNewName.Text.Length > 0)
			{
				btnOK.Enabled = true;
			}
			else
			{
				btnOK.Enabled = false;
			}	
		}

		private void tbNewName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if((e.KeyChar == '.' || e.KeyChar == '"'))
			{
				e.Handled = true;
			}
			else
			{
				e.Handled = false;
			}
		}

		private void frmNameChanger_Load(object sender, System.EventArgs e)
		{
			this.ImeMode = ImeMode.On;
		}
	}
}
