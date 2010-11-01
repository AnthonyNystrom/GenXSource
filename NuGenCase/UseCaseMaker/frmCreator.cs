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
	public class frmCreator : System.Windows.Forms.Form
	{
		private Localizer localizer = null;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblNameTitle;
		public System.Windows.Forms.TextBox tbName;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmCreator(Localizer localizer, string elementType)
		{
			//
			// Necessario per il supporto di Progettazione Windows Form
			//
			InitializeComponent();

			//
			// TODO: aggiungere il codice del costruttore dopo la chiamata a InitializeComponent
			//
			this.localizer = localizer;
			this.localizer.LocalizeControls(this);
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.tbName = new System.Windows.Forms.TextBox();
			this.lblNameTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Enabled = false;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(80, 40);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(120, 23);
			this.btnOK.TabIndex = 1;
			this.btnOK.Text = "[OK]";
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(208, 40);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "[Cancel]";
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(128, 8);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(272, 20);
			this.tbName.TabIndex = 0;
			this.tbName.Text = "";
			this.tbName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbName_KeyPress);
			this.tbName.Validating += new System.ComponentModel.CancelEventHandler(this.tbName_Validating);
			this.tbName.TextChanged += new System.EventHandler(this.tbName_TextChanged);
			// 
			// lblNameTitle
			// 
			this.lblNameTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblNameTitle.Location = new System.Drawing.Point(8, 8);
			this.lblNameTitle.Name = "lblNameTitle";
			this.lblNameTitle.Size = new System.Drawing.Size(104, 16);
			this.lblNameTitle.TabIndex = 29;
			this.lblNameTitle.Text = "[Element name]";
			// 
			// frmCreator
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(410, 72);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.lblNameTitle);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmCreator";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "[Create an element]";
			this.Load += new System.EventHandler(this.frmCreator_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void tbName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if(tbName.Text == string.Empty)
			{
				return;
			}
			if(((frmMain)this.Owner).Model.FindElementByName(tbName.Text) != null)
			{
				e.Cancel = true;
				// [Name already in use!]
				MessageBox.Show(
					this,
					this.localizer.GetValue("UserMessages","nameAlreadyInUse"),
					Application.ProductName);
			}		
		}

		private void tbName_TextChanged(object sender, System.EventArgs e)
		{
			if(tbName.Text.Length > 0)
			{
				btnOK.Enabled = true;
			}
			else
			{
				btnOK.Enabled = false;
			}			
		}

		private void tbName_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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

		private void frmCreator_Load(object sender, System.EventArgs e)
		{
			this.ImeMode = ImeMode.On;
		}
	}
}
