using System;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Forms;

namespace UseCaseMaker
{
	/// <summary>
	/// Descrizione di riepilogo per frmReorder.
	/// </summary>
	public class frmReorder : System.Windows.Forms.Form
	{
		private string prefix;

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnMoveDown;
		private System.Windows.Forms.ColumnHeader chIndex;
		private System.Windows.Forms.ColumnHeader chValue;
		private System.Windows.Forms.ListView lvElements;
		private System.Windows.Forms.Button btnMoveUp;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmReorder(Localizer localizer, string prefix)
		{
			//
			// Necessario per il supporto di Progettazione Windows Form
			//
			InitializeComponent();

			//
			// TODO: aggiungere il codice del costruttore dopo la chiamata a InitializeComponent
			//
			this.prefix = prefix;
			localizer.LocalizeControls(this);

			btnMoveDown.Enabled = false;
			btnMoveUp.Enabled = false;
		}

		public string Prefix
		{
			get
			{
				return this.prefix;
			}

			set
			{
				this.prefix = value;
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
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOK = new System.Windows.Forms.Button();
			this.lvElements = new System.Windows.Forms.ListView();
			this.chIndex = new System.Windows.Forms.ColumnHeader();
			this.chValue = new System.Windows.Forms.ColumnHeader();
			this.btnMoveUp = new System.Windows.Forms.Button();
			this.btnMoveDown = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.CausesValidation = false;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnCancel.Location = new System.Drawing.Point(336, 128);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(120, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "[Cancel]";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnOK.Location = new System.Drawing.Point(336, 96);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(120, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "[OK]";
			// 
			// lvElements
			// 
			this.lvElements.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
																						 this.chIndex,
																						 this.chValue});
			this.lvElements.FullRowSelect = true;
			this.lvElements.GridLines = true;
			this.lvElements.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.lvElements.HideSelection = false;
			this.lvElements.Location = new System.Drawing.Point(8, 8);
			this.lvElements.MultiSelect = false;
			this.lvElements.Name = "lvElements";
			this.lvElements.Size = new System.Drawing.Size(320, 144);
			this.lvElements.TabIndex = 0;
			this.lvElements.View = System.Windows.Forms.View.Details;
			this.lvElements.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lvElements_MouseUp);
			this.lvElements.SelectedIndexChanged += new System.EventHandler(this.lvElements_SelectedIndexChanged);
			// 
			// chIndex
			// 
			this.chIndex.Width = 40;
			// 
			// chValue
			// 
			this.chValue.Width = 260;
			// 
			// btnMoveUp
			// 
			this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnMoveUp.Location = new System.Drawing.Point(336, 8);
			this.btnMoveUp.Name = "btnMoveUp";
			this.btnMoveUp.Size = new System.Drawing.Size(120, 23);
			this.btnMoveUp.TabIndex = 1;
			this.btnMoveUp.Text = "[Move Up]";
			this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
			// 
			// btnMoveDown
			// 
			this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.btnMoveDown.Location = new System.Drawing.Point(336, 40);
			this.btnMoveDown.Name = "btnMoveDown";
			this.btnMoveDown.Size = new System.Drawing.Size(120, 23);
			this.btnMoveDown.TabIndex = 2;
			this.btnMoveDown.Text = "[Move Down]";
			this.btnMoveDown.Click += new System.EventHandler(this.btnMoveDown_Click);
			// 
			// frmReorder
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(466, 160);
			this.Controls.Add(this.btnMoveDown);
			this.Controls.Add(this.btnMoveUp);
			this.Controls.Add(this.lvElements);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmReorder";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "[Reorder Elements]";
			this.ResumeLayout(false);

		}
		#endregion

		private void lvElements_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(lvElements.SelectedIndices.Count == 0)
			{
				return;
			}

			if(lvElements.SelectedIndices[0] == 0)
			{
				btnMoveUp.Enabled = false;
			}
			else
			{
				btnMoveUp.Enabled = true;
			}

			if(lvElements.SelectedIndices[0] == lvElements.Items.Count - 1)
			{
				btnMoveDown.Enabled = false;
			}
			else
			{
				btnMoveDown.Enabled = true;
			}
		}

		private void btnMoveUp_Click(object sender, System.EventArgs e)
		{
			string text;
			int selectedIndex;

			selectedIndex = lvElements.SelectedIndices[0];

			ListViewItem lvi1 = lvElements.Items[selectedIndex];
			text = lvi1.SubItems[1].Text;
			ListViewItem lvi2 = lvElements.Items[selectedIndex - 1];
			lvi1.SubItems[1].Text = lvi2.SubItems[1].Text;
			lvi2.SubItems[1].Text = text;
			lvi2.Selected = true;
		}

		private void btnMoveDown_Click(object sender, System.EventArgs e)
		{
			string text;
			int selectedIndex;

			selectedIndex = lvElements.SelectedIndices[0];

			ListViewItem lvi1 = lvElements.Items[selectedIndex];
			text = lvi1.SubItems[1].Text;
			ListViewItem lvi2 = this.lvElements.Items[selectedIndex + 1];
			lvi1.SubItems[1].Text = lvi2.SubItems[1].Text;
			lvi2.SubItems[1].Text = text;
			lvi2.Selected = true;
		}

		private void lvElements_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if(lvElements.SelectedIndices.Count == 0)
			{
				btnMoveDown.Enabled = false;
				btnMoveUp.Enabled = false;
			}		
		}

		public void AddNameToList(string name)
		{
			ListViewItem lvi = new ListViewItem();
			lvi.Text =  this.prefix + (this.lvElements.Items.Count + 1).ToString();
			lvi.SubItems.Add(name);
			this.lvElements.Items.Add(lvi);
		}

		public string [] GetOrderedNames()
		{
			string [] names = new string[this.lvElements.Items.Count];
			for(int counter = 0; counter < this.lvElements.Items.Count; counter++)
			{
				names[counter] = lvElements.Items[counter].SubItems[1].Text;
			}
			return names;
		}
	}
}
