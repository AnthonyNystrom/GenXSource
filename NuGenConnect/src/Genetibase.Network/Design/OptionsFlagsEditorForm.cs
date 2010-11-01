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
	/// Summary description for FlagsEditorControl.
	/// </summary>
	public class OptionsFlagsEditorForm : Form
	{
		private System.Windows.Forms.CheckedListBox _clb;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Ras.Ras.EntryOptions _value;
		private System.Windows.Forms.Label _lb1;
		private System.Windows.Forms.Label _lb2;
		private ArrayList _al = new ArrayList();

		public OptionsFlagsEditorForm()
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
			this._clb.Size = new System.Drawing.Size(200, 154);
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
			this._lb2.Size = new System.Drawing.Size(170, 136);
			this._lb2.TabIndex = 2;
			// 
			// OptionsFlagsEditorForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(370, 159);
			this.Controls.Add(this._lb2);
			this.Controls.Add(this._lb1);
			this.Controls.Add(this._clb);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "OptionsFlagsEditorForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Connection options - press Esc to save & exit";
			this.Load += new System.EventHandler(this.FlagsEditorForm_Load);
			this.Closed += new System.EventHandler(this.FlagsEditorForm_Close);
			this.ResumeLayout(false);

		}
		#endregion

		#region public property
		public Ras.Ras.EntryOptions Val
		{
			get{return this._value;}
			set
			{
				this._value = value;
				this._clb.Items.Clear();
				foreach(uint i in Enum.GetValues(typeof(Ras.Ras.EntryOptions)))
				{
					this._clb.Items.Add( ( Enum.GetName( typeof(Ras.Ras.EntryOptions), i ) ), (i & (uint)this._value)!=0 );
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
			this._value = (Ras.Ras.EntryOptions)n;
		}

		private void _clb_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode==Keys.Escape)
				this.Close();
		}
		#endregion

		private void _clb_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			Ras.Ras.EntryOptions reo = (Ras.Ras.EntryOptions)Enum.Parse(typeof(Ras.Ras.EntryOptions), this._clb.SelectedItem.ToString());
			this._lb1.Text = this._clb.SelectedItem.ToString();
			switch(reo)
			{
				case Ras.Ras.EntryOptions.RASEO_UseCountryAndAreaCodes:
					this._lb2.Text = "If this flag is set, the CountryID, CountryCode, and AreaCode members are used to construct the phone number. If this flag is not set, these members are ignored.";
					break;
				case Ras.Ras.EntryOptions.RASEO_SpecificIpAddr:
					this._lb2.Text = "If this flag is set, RAS tries to use the IP address specified by ipaddr as the IP address for the dial-up connection. If this flag is not set, the value of the IPaddr member is ignored.";
					break;
				case Ras.Ras.EntryOptions.RASEO_SpecificNameServers:
					this._lb2.Text = "If this flag is set, RAS uses the IPaddrDns, IPaddrDnsAlt, IPaddrWins, and IPaddrWinsAlt members to specify the name of the server addresses for the dial-up connection. If this flag is not set, RAS ignores these members.";
					break;
				case Ras.Ras.EntryOptions.RASEO_IpHeaderCompression:
					this._lb2.Text = "If this flag is set, RAS negotiates to use IP header compression on PPP connections. If this flag is not set, IP header compression is not negotiated. It is generally advisable to set this flag.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RemoteDefaultGateway:
					this._lb2.Text = "If this flag is set, the default route for IP packets is through the dial-up adapter when the connection is active. If this flag is clear, the default route is not modified.";
					break;
				case Ras.Ras.EntryOptions.RASEO_DisableLcpExtensions:
					this._lb2.Text = "If this flag is set, RAS disables the PPP LCP extensions defined in RFC 1570. This may be necessary to connect to certain older PPP implementations, but interferes with features such as server callback. Do not set this flag unless specifically required.";
					break;
				case Ras.Ras.EntryOptions.RASEO_TerminalBeforeDial:
					this._lb2.Text = "This flag has no effect in current version.";
					break;
				case Ras.Ras.EntryOptions.RASEO_TerminalAfterDial:
					this._lb2.Text = "This flag has no effect in current version.";
					break;
				case Ras.Ras.EntryOptions.RASEO_ModemLights:
					this._lb2.Text = "If this flag is set, a status monitor is displayed in the task bar.";
					break;
				case Ras.Ras.EntryOptions.RASEO_SwCompression:
					this._lb2.Text = "If this flag is set, software compression is negotiated on the link. Setting this flag causes the PPP driver to attempt to negotiate CCP with the server. This flag should be set by default, but clearing it can reduce the negotiation period if the server does not support a compatible compression protocol.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireEncryptedPw:
					this._lb2.Text = "If this flag is set, only secure password schemes can be used to authenticate the client with the server. Clear this flag for increased interoperability, and set it for increased security. NOTE: Setting this flag also sets RASEO_RequireSPAP, RASEO_RequireCHAP, RASEO_RequireMsCHAP, and RASEO_RequireMsCHAP2.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireMsEncryptedPw:
					this._lb2.Text = "If this flag is set, only the Microsoft secure password scheme, MSCHAP, can be used to authenticate the client with the server. NOTE: Setting this flag also sets RASEO_RequireMsCHAP and RASEO_RequireMsCHAP2.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireDataEncryption:
					this._lb2.Text = "If this flag is set, data encryption must be negotiated successfully or the connection should be dropped. This flag is ignored unless RASEO_RequireMsEncryptedPw is also set.";
					break;
				case Ras.Ras.EntryOptions.RASEO_NetworkLogon:
					this._lb2.Text = "If this flag is set, RAS logs on to the network after the point-to-point connection is established.";
					break;
				case Ras.Ras.EntryOptions.RASEO_UseLogonCredentials:
					this._lb2.Text = "If this flag is set, RAS uses the user name, password, and domain of the currently logged-on user when dialing this entry. This flag is ignored unless RASEO_RequireMsEncryptedPw is also set.";
					break;
				case Ras.Ras.EntryOptions.RASEO_PromoteAlternates:
					this._lb2.Text = "This flag currently has no effect.";
					break;
				case Ras.Ras.EntryOptions.RASEO_SecureLocalFiles:
					this._lb2.Text = "If this flag is set, RAS checks for existing remote file system and remote printer bindings before making a connection with this entry. Typically, set this flag is set on phone-book entries for public networks to remind users to break connections to their private network before connecting to a public network.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireEAP:
					this._lb2.Text = "If this flag is set, an Extensible Authentication Protocol (EAP) must be supported for authentication.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequirePAP:
					this._lb2.Text = "If this flag is set, Password Authentication Protocol must be supported for authentication.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireSPAP:
					this._lb2.Text = "If this flag is set, Shiva's Password Authentication Protocol must be supported for authentication.";
					break;
				case Ras.Ras.EntryOptions.RASEO_Custom:
					this._lb2.Text = "If this flag is set, the connection uses custom encryption.";
					break;
				case Ras.Ras.EntryOptions.RASEO_PreviewPhoneNumber:
					this._lb2.Text = "If this flag is set, the remote access dialer displays the phone number to be dialed.";
					break;
				case Ras.Ras.EntryOptions.RASEO_SharedPhoneNumbers:
					this._lb2.Text = "If this flag is set, all modems installed on the local computer share the same phone number. This option has no effect unless multiple modems are installed.";
					break;
				case Ras.Ras.EntryOptions.RASEO_PreviewUserPw:
					this._lb2.Text = "If this flag is set, the remote access dialer displays the user's name and password prior to dialing.";
					break;
				case Ras.Ras.EntryOptions.RASEO_PreviewDomain:
					this._lb2.Text = "If this flag is set, the remote access dialer displays the domain name prior to dialing.";
					break;
				case Ras.Ras.EntryOptions.RASEO_ShowDialingProgress:
					this._lb2.Text = "If this flag is set, the remote access dialer displays its progress in establishing the connection.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireCHAP:
					this._lb2.Text = "If this flag is set, the Challenge Handshake Authentication Protocol must be supported for authentication.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireMsCHAP:
					this._lb2.Text = "If this flag is set, the Microsoft Challenge Handshake Authentication Protocol must be supported for authentication.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireMsCHAP2:
					this._lb2.Text = "If this flag is set, version 2 of the Microsoft Challenge Handshake Authentication Protocol must be supported for authentication.";
					break;
				case Ras.Ras.EntryOptions.RASEO_RequireW95MSCHAP:
					this._lb2.Text = "If this flag is set, RASEO_RequireMsCHAP must also be set and MS-CHAP must send the LanManager-hashed password.";
					break;
				case Ras.Ras.EntryOptions.RASEO_CustomScript:
					this._lb2.Text = "This flag must be set in order for RAS to invoke a custom-scripting DLL after establishing the connection to the server.";
					break;
			}
		}
	}
}
