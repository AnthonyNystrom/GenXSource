using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.Debug
{
	/// <summary>
	/// Zusammendfassende Beschreibung für NetDevice.
	/// </summary>
	public class NetDevice : System.Windows.Forms.Form
	{
    public System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.TextBox textBox1;
		/// <summary>
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public NetDevice( ref perfo p)
		{
			//
			// Erforderlich für die Windows Form-Designerunterstützung
			//
			InitializeComponent();

      comboBox1_Fill(ref p);
      comboBox1.Text = p.NetDevice;

		}

		/// <summary>
		/// Die verwendeten Ressourcen bereinigen.
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.SuspendLayout();
      // 
      // comboBox1
      // 
      this.comboBox1.BackColor = System.Drawing.SystemColors.Control;
      this.comboBox1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.comboBox1.Location = new System.Drawing.Point(8, 8);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(376, 21);
      this.comboBox1.TabIndex = 0;
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // button1
      // 
      this.button1.BackColor = System.Drawing.SystemColors.Control;
      this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.button1.Location = new System.Drawing.Point(8, 88);
      this.button1.Name = "button1";
      this.button1.TabIndex = 1;
      this.button1.Text = "OK";
      // 
      // button2
      // 
      this.button2.BackColor = System.Drawing.SystemColors.Control;
      this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.button2.ForeColor = System.Drawing.SystemColors.ControlText;
      this.button2.Location = new System.Drawing.Point(304, 88);
      this.button2.Name = "button2";
      this.button2.TabIndex = 2;
      this.button2.Text = "Cancel";
      // 
      // textBox1
      // 
      this.textBox1.BackColor = System.Drawing.SystemColors.Control;
      this.textBox1.ForeColor = System.Drawing.SystemColors.ControlText;
      this.textBox1.Location = new System.Drawing.Point(8, 32);
      this.textBox1.Multiline = true;
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(376, 48);
      this.textBox1.TabIndex = 3;
      this.textBox1.Text = "";
      // 
      // NetDevice
      // 
      this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
      this.ClientSize = new System.Drawing.Size(394, 120);
      this.Controls.AddRange(new System.Windows.Forms.Control[] {
                                                                  this.textBox1,
                                                                  this.button2,
                                                                  this.button1,
                                                                  this.comboBox1});
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
      this.Name = "NetDevice";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Choose Network Device";
      this.Load += new System.EventHandler(this.NetDevice_Load);
      this.ResumeLayout(false);

    }
		#endregion

    private void comboBox1_Fill(ref perfo p)
    {
      comboBox1.Items.Clear();
      string [] s = p.GetAllInstanceNames();
      foreach(string x in s)
      {
        comboBox1.Items.Add(x);
      }
      comboBox1.Text = (string)comboBox1.Items[0];
    }

    private void NetDevice_Load(object sender, System.EventArgs e)
    {
      
    }

    private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
    {
      textBox1.Text =  comboBox1.Text ;
    }
	}
}
