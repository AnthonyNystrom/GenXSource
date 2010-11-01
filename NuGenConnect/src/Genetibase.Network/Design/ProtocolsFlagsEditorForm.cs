using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Genetibase.Network.Ras;

namespace Genetibase.Network.Design
{
	/// <summary>
	/// Summary description for FlagsEditorControl.
	/// </summary>
	public class ProtocolsFlagsEditorForm : Form
	{
		private System.Windows.Forms.CheckedListBox _clb;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Ras.Ras.EntryProtocols _value;
		private System.Windows.Forms.Label _lb1;
		private System.Windows.Forms.Label _lb2;
		private ArrayList _al = new ArrayList();

		public ProtocolsFlagsEditorForm()
		{
			InitializeComponent();

		}
		/// <summary> 
		/// Clean up any resources being used.
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._clb = new System.Windows.Forms.CheckedListBox();
			this._lb1 = new System.Windows.Forms.Label();
			this._lb2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// _clb
			// 
			this._clb.CheckOnClick = true;
			this._clb.Dock = System.Windows.Forms.DockStyle.Left;
			this._clb.Location = new System.Drawing.Point(0, 0);
			this._clb.Name = "_clb";
			this._clb.Size = new System.Drawing.Size(200, 79);
			this._clb.TabIndex = 0;
			this._clb.KeyDown += new System.Windows.Forms.KeyEventHandler(this._clb_KeyDown);
			this._clb.SelectedIndexChanged += new System.EventHandler(this._clb_SelectedIndexChanged);
			// 
			// _lb1
			// 
			this._lb1.Dock = System.Windows.Forms.DockStyle.Top;
			this._lb1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._lb1.ForeColor = System.Drawing.Color.DodgerBlue;
			this._lb1.Location = new System.Drawing.Point(200, 0);
			this._lb1.Name = "_lb1";
			this._lb1.Size = new System.Drawing.Size(170, 16);
			this._lb1.TabIndex = 1;
			this._lb1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// _lb2
			// 
			this._lb2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this._lb2.Location = new System.Drawing.Point(200, 23);
			this._lb2.Name = "_lb2";
			this._lb2.Size = new System.Drawing.Size(170, 64);
			this._lb2.TabIndex = 2;
			// 
			// ProtocolsFlagsEditorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(370, 87);
			this.Controls.Add(this._lb2);
			this.Controls.Add(this._lb1);
			this.Controls.Add(this._clb);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ProtocolsFlagsEditorForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Connection protocols - press Esc to save & exit";
			this.Load += new System.EventHandler(this.FlagsEditorForm_Load);
			this.Closed += new System.EventHandler(this.FlagsEditorForm_Close);
			this.ResumeLayout(false);

		}
		#endregion

		#region public property
		public Ras.Ras.EntryProtocols Val
		{
			get{return this._value;}
			set
			{
				this._value = value;
				this._clb.Items.Clear();
				foreach(uint i in Enum.GetValues(typeof(Ras.Ras.EntryProtocols)))
				{
					this._clb.Items.Add( ( Enum.GetName( typeof(Ras.Ras.EntryProtocols), i ) ), (i & (uint)this._value)!=0 );
					this._al.Add(i);
				}
			}
		}
		#endregion

		#region events
		private void FlagsEditorForm_Load(object sender, System.EventArgs e)
		{
			this.Location = new Point(Control.MousePosition.X - this.Width - 5, Control.MousePosition.Y + 10);
			this._clb.SelectedIndex = 0;
		}

		private void FlagsEditorForm_Close(object sender, System.EventArgs e)
		{
			uint n = 0;
			uint[] narr = new uint[this._al.Count];
			this._al.CopyTo(narr);
			foreach(int i in this._clb.CheckedIndices)
				n |= narr[i];
			// accept new value and close editor
			this._value = (Ras.Ras.EntryProtocols)n;
		}

		private void _clb_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode==Keys.Escape)
				this.Close();
		}
		#endregion

		private void _clb_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Ras.Ras.EntryProtocols rep = (Ras.Ras.EntryProtocols)Enum.Parse(typeof(Ras.Ras.EntryProtocols), this._clb.SelectedItem.ToString());
			this._lb1.Text = this._clb.SelectedItem.ToString();
			switch(rep)
			{
				case Ras.Ras.EntryProtocols.RASNP_NetBEUI:
					this._lb2.Text = "The RASNP_NetBEUI flag is no longer supported.";
					break;
				case Ras.Ras.EntryProtocols.RASNP_Ipx:
					this._lb2.Text = "Specifies the network protocols to negotiate. This flag forced negotiate the IPX protocol. You can combine this protocol with others.";
					break;
				case Ras.Ras.EntryProtocols.RASNP_Ip:
					this._lb2.Text = "Specifies the network protocols to negotiate. This flag forced negotiate the TCP/IP protocol. You can combine this protocol with others.";
					break;
			}
		}
	}
}
