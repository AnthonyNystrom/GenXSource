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
	public class frmDeleter : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblUserQuestionTitle;
		public System.Windows.Forms.CheckBox cbDontMark;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmDeleter(Localizer localizer, string elementType)
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
			this.cbDontMark = new System.Windows.Forms.CheckBox();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblUserQuestionTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// cbDontMark
			// 
			this.cbDontMark.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cbDontMark.Location = new System.Drawing.Point(8, 48);
			this.cbDontMark.Name = "cbDontMark";
			this.cbDontMark.Size = new System.Drawing.Size(328, 24);
			this.cbDontMark.TabIndex = 29;
			this.cbDontMark.Text = "[Don\'t mark occurrences as {element name}]";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(48, 80);
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
			this.btnCancel.Location = new System.Drawing.Point(176, 80);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 23);
			this.btnCancel.TabIndex = 28;
			this.btnCancel.Text = "[Cancel]";
			// 
			// lblUserQuestionTitle
			// 
			this.lblUserQuestionTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblUserQuestionTitle.Location = new System.Drawing.Point(8, 8);
			this.lblUserQuestionTitle.Name = "lblUserQuestionTitle";
			this.lblUserQuestionTitle.Size = new System.Drawing.Size(328, 32);
			this.lblUserQuestionTitle.TabIndex = 0;
			this.lblUserQuestionTitle.Text = "[Selected element (and relative children) will be deleted. Continue?]";
			// 
			// frmDeleter
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(346, 112);
			this.Controls.Add(this.lblUserQuestionTitle);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.cbDontMark);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmDeleter";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "[Delete element]";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
