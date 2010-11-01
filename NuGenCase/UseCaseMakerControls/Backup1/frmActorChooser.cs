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
	public class frmActorChooser : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label lblChooseActorTitle;
		public System.Windows.Forms.ListBox lbActors;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmActorChooser(String [] actors, Localizer localizer)
		{
			//
			// Necessario per il supporto di Progettazione Windows Form
			//
			InitializeComponent();

			//
			// TODO: aggiungere il codice del costruttore dopo la chiamata a InitializeComponent
			//
			localizer.LocalizeControls(this);
			lbActors.Items.Clear();
			foreach(String actor in actors)
			{
				lbActors.Items.Add(actor);
			}

			if(lbActors.Items.Count == 0)
			{
				btnOK.Enabled = false;
			}
			else
			{
				btnOK.Enabled = true;
				lbActors.SelectedIndex = 0;
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
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.lblChooseActorTitle = new System.Windows.Forms.Label();
			this.lbActors = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(48, 120);
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
			this.btnCancel.Location = new System.Drawing.Point(176, 120);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "[Cancel]";
			// 
			// lblChooseActorTitle
			// 
			this.lblChooseActorTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.lblChooseActorTitle.Location = new System.Drawing.Point(8, 8);
			this.lblChooseActorTitle.Name = "lblChooseActorTitle";
			this.lblChooseActorTitle.Size = new System.Drawing.Size(328, 16);
			this.lblChooseActorTitle.TabIndex = 0;
			this.lblChooseActorTitle.Text = "[Select an actor]";
			// 
			// lbActors
			// 
			this.lbActors.Location = new System.Drawing.Point(8, 32);
			this.lbActors.Name = "lbActors";
			this.lbActors.Size = new System.Drawing.Size(328, 82);
			this.lbActors.TabIndex = 0;
			// 
			// frmActorChooser
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(346, 152);
			this.Controls.Add(this.lbActors);
			this.Controls.Add(this.lblChooseActorTitle);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmActorChooser";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "[Actors list]";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
