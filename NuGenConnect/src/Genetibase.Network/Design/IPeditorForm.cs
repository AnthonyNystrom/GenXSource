using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Genetibase.Network.Design
{
	/// <summary>
	/// Summary description for IPeditorForm.
	/// </summary>
	public class IPeditorForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox _a;
		private System.Windows.Forms.TextBox _d;
		private System.Windows.Forms.TextBox _c;
		private System.Windows.Forms.TextBox _b;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button _OK;
		private System.Windows.Forms.Button _Cancel;
		private System.ComponentModel.Container components = null;
		private byte _a_, _b_, _c_, _d_;

		#region public properties
		public byte A
		{
			get{return this._a_;}
			set{this._a_=value;}
		}

		public byte B
		{
			get{return this._b_;}
			set{this._b_=value;}
		}
		
		public byte C
		{
			get{return this._c_;}
			set{this._c_=value;}
		}
		
		public byte D
		{
			get{return this._d_;}
			set{this._d_=value;}
		}
		#endregion

		public IPeditorForm()
		{
			InitializeComponent();
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this._a = new System.Windows.Forms.TextBox();
			this._d = new System.Windows.Forms.TextBox();
			this._c = new System.Windows.Forms.TextBox();
			this._b = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this._OK = new System.Windows.Forms.Button();
			this._Cancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// _a
			// 
			this._a.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._a.Location = new System.Drawing.Point(8, 16);
			this._a.Name = "_a";
			this._a.Size = new System.Drawing.Size(40, 22);
			this._a.TabIndex = 0;
			this._a.Text = "";
			this._a.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this._a.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._AllTB_KeyPress);
			this._a.KeyUp += new System.Windows.Forms.KeyEventHandler(this._AllTB_KeyUp);
			// 
			// _d
			// 
			this._d.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._d.Location = new System.Drawing.Point(176, 16);
			this._d.Name = "_d";
			this._d.Size = new System.Drawing.Size(40, 22);
			this._d.TabIndex = 3;
			this._d.Text = "";
			this._d.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this._d.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._AllTB_KeyPress);
			this._d.KeyUp += new System.Windows.Forms.KeyEventHandler(this._AllTB_KeyUp);
			// 
			// _c
			// 
			this._c.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._c.Location = new System.Drawing.Point(120, 16);
			this._c.Name = "_c";
			this._c.Size = new System.Drawing.Size(40, 22);
			this._c.TabIndex = 2;
			this._c.Text = "";
			this._c.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this._c.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._AllTB_KeyPress);
			this._c.KeyUp += new System.Windows.Forms.KeyEventHandler(this._AllTB_KeyUp);
			// 
			// _b
			// 
			this._b.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._b.Location = new System.Drawing.Point(64, 16);
			this._b.Name = "_b";
			this._b.Size = new System.Drawing.Size(40, 22);
			this._b.TabIndex = 1;
			this._b.Text = "";
			this._b.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this._b.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this._AllTB_KeyPress);
			this._b.KeyUp += new System.Windows.Forms.KeyEventHandler(this._AllTB_KeyUp);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label1.Location = new System.Drawing.Point(48, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(16, 32);
			this.label1.TabIndex = 4;
			this.label1.Text = ".";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label2.Location = new System.Drawing.Point(160, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(16, 32);
			this.label2.TabIndex = 5;
			this.label2.Text = ".";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this.label4.Location = new System.Drawing.Point(104, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(16, 32);
			this.label4.TabIndex = 7;
			this.label4.Text = ".";
			// 
			// _OK
			// 
			this._OK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._OK.Location = new System.Drawing.Point(232, 16);
			this._OK.Name = "_OK";
			this._OK.Size = new System.Drawing.Size(64, 24);
			this._OK.TabIndex = 4;
			this._OK.Text = "OK";
			this._OK.Click += new System.EventHandler(this._OK_Click);
			// 
			// _Cancel
			// 
			this._Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this._Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(204)));
			this._Cancel.Location = new System.Drawing.Point(304, 16);
			this._Cancel.Name = "_Cancel";
			this._Cancel.Size = new System.Drawing.Size(64, 24);
			this._Cancel.TabIndex = 5;
			this._Cancel.Text = "Cancel";
			this._Cancel.Click += new System.EventHandler(this._Cancel_Click);
			// 
			// IPeditorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(370, 47);
			this.Controls.Add(this._Cancel);
			this.Controls.Add(this._OK);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this._b);
			this.Controls.Add(this._c);
			this.Controls.Add(this._d);
			this.Controls.Add(this._a);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "IPeditorForm";
			this.Text = "Enter IP address";
			this.Load += new System.EventHandler(this.IPeditorForm_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new IPeditorForm());
		}

		#region events
		private void IPeditorForm_Load(object sender, System.EventArgs e)
		{
			this.Location = new Point(Control.MousePosition.X - this.Width - 5, Control.MousePosition.Y + 10);
			if(this._a_!=0) this._a.Text = this._a_.ToString();
			if(this._b_!=0) this._b.Text = this._b_.ToString();
			if(this._c_!=0) this._c.Text = this._c_.ToString();
			if(this._d_!=0) this._d.Text = this._d_.ToString();
		}

		private void _AllTB_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			TextBox tb = sender as TextBox;
			if(tb.Text.Length==3 && (int)e.KeyCode>=48 && (int)e.KeyCode<=57)
			{
				if(Convert.ToUInt16(tb.Text)>255)
					tb.Select(0, 3);
				else
				{
					Control next = this;
					if(tb==this._a)
						next = this._b;
					else if(tb==this._b)
						next = this._c;
					else if(tb==this._c)
						next = this._d;
					else if(tb==this._d)
						next = this._OK;
					next.Focus();
				}
			}
		}

		private void _AllTB_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			TextBox tb = sender as TextBox;
			if(e.KeyChar>= (char)'0' && e.KeyChar<= (char)'9' && tb.Text.Length>2 && tb.SelectedText.Length==0)
				e.Handled = true;
			else if(e.KeyChar==(char)13) // Enter
				this._OK_Click(null, null);
			else if(e.KeyChar==(char)27) // ESC
				this._Cancel_Click(null, null);
			else if(e.KeyChar!=(char)8 && (e.KeyChar<(char)'0' || e.KeyChar>(char)'9')) // BackSP
				e.Handled = true;
		}

		private void _OK_Click(object sender, System.EventArgs e)
		{
			ushort u;
			u = this._a.Text.Length>0 ? Convert.ToUInt16(this._a.Text) : (ushort)0;
			if(u>255)
			{
				this._a.Select(0, 3);
				this._a.Focus();
				return;
			}
			else
				this._a_ = (byte)u;
			u = this._b.Text.Length>0 ? Convert.ToUInt16(this._b.Text) : (ushort)0;
			if(u>255)
			{
				this._b.Select(0, 3);
				this._b.Focus();
				return;
			}
			else
				this._b_ = (byte)u;
			u = this._c.Text.Length>0 ? Convert.ToUInt16(this._c.Text) : (ushort)0;
			if(u>255)
			{
				this._c.Select(0, 3);
				this._c.Focus();
				return;
			}
			else
				this._c_ = (byte)u;
			u = this._d.Text.Length>0 ? Convert.ToUInt16(this._d.Text) : (ushort)0;
			if(u>255)
			{
				this._d.Select(0, 3);
				this._d.Focus();
				return;
			}
			else
				this._d_ = (byte)u;
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void _Cancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
		#endregion
	}
}
