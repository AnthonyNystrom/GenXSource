using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Threading;
using System.Reflection;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design.Serialization;

using Genetibase.Network.Design;


namespace Genetibase.Network.Ras
{
	/// <summary>
	/// Represent connection that can be established/severed by RAS API.
	/// </summary>
	/// 

//	[System.ComponentModel.LicenseProvider( typeof( Xheo.Licensing.ExtendedLicenseProvider ) )]
	[System.Drawing.ToolboxBitmap(typeof(NuGenConnect))]
	public class NuGenConnect : System.ComponentModel.Component
	{
//		private System.ComponentModel.License _license;
		#region fields
		private Thread _watchThread;
		private System.ComponentModel.ISynchronizeInvoke _synchronizingObject;
		private ManualResetEvent _disconnEvent=new ManualResetEvent(false);  // notifies waiting thread that an disconnection event has occurred
		private ManualResetEvent _stopEvent=new ManualResetEvent(false);  // notifies waiting thread that tracking for conn/disconn events need no more
		private Ras.RASDIALPARAMS _params=new Ras.RASDIALPARAMS();
		private Ras.RASENTRY _ent=new Ras.RASENTRY();
		private IntPtr _handle;
		private string _phonebook=string.Empty;
		/// <summary>
		/// Event raised on connection established
		/// </summary>
		[Description("Occurs when connection successfully establish")]
		public event EventHandler Connected;
		/// <summary>
		/// Event raised on connection break
		/// </summary>
		[Description("Occurs when connection become disconnected")]
		public event EventHandler Disconnected;
		#endregion

		/// <summary>
		/// Initializes a new instance of the NuGenConnect class.
		/// </summary>
		public NuGenConnect()
		{
			// set the unmanaged size, in bytes, for structure Ras.RASENTRY
			this._ent.dwSize = Marshal.SizeOf(typeof(Ras.RASENTRY));
			// set default values
			this._ent.dwVpnStrategy=(uint)Ras.EntryVpnStrategies.VS_PptpOnly;
			this._ent.dwEncryptionType=(uint)Ras.EntryEncryptionTypes.ET_Optional;
			this._ent.szDeviceType="vpn";
			this._ent.dwFrameSize=1500;
			this._ent.dwfNetProtocols=(uint)Ras.EntryProtocols.RASNP_Ip;
			this._ent.dwFramingProtocol=(uint)Ras.EntryFramingProtocols.RASFP_Ppp;
			this._ent.dwType=(uint)Ras.EntryTypes.RASET_Vpn;
			this._ent.dwfOptions=0x25100230;
		
			frmNag frmnag = new frmNag();
			frmnag.ShowDialog();
			frmnag.Close();
			frmnag.Dispose();

		}
		
		#region protected override methods
		// recommend to call xxx.Dispose() (where xxx is component/control that contain NuGenConnect component) when xxx not need anymore
		// rasConnection1.Disconnect() can't substitute for such call
		protected override void Dispose(bool disposing)
		{
			this.Disconnect();
			if(disposing)
			{
				this.StopWatch();
			}
			base.Dispose(disposing);
//			if( _license != null ) { _license.Dispose(); _license = null; }
		}
		#endregion
		#region private methods
		// create new entry and set it's properties from _ent structure of this class
		private void SetEntry()
		{
			Ras.RasCheck(Ras.RasSetEntryProperties(null, this._params.szEntryName, ref this._ent, this._ent.dwSize, 0, 0));
		}
		// get properties of existent phone-book entry and fill _ent structure of this class
		private void GetEntry()
		{
			int entrySize = 0;
			IntPtr buff = IntPtr.Zero;
			Ras.RasGetEntryProperties(null, this._params.szEntryName, buff, ref entrySize, 0, 0);
			buff = Marshal.AllocHGlobal(entrySize);
			Marshal.WriteInt32(buff, this._ent.dwSize);
			Ras.RasCheck(Ras.RasGetEntryProperties(null, this._params.szEntryName, buff, ref entrySize, 0, 0));
			this._ent = (Ras.RASENTRY)Marshal.PtrToStructure(buff, typeof(Ras.RASENTRY));
			Marshal.FreeHGlobal(buff);
		}
		// delete entry that was created before connection
		private void DeleteEntry()
		{
			Ras.RasCheck(Ras.RasDeleteEntry(null, this._params.szEntryName));
		}
		// called on connection established
		private void OnConnected()
		{
			if(this.Connected!=null)
				this.Connected(this, new EventArgs());
		}

		// called on connection break
		private void OnDisconnected()
		{
			this.Disconnect();

			if(this.Disconnected==null)
				return;
			EventArgs args=new EventArgs();
			if((this.SynchronizingObject!=null) && this.SynchronizingObject.InvokeRequired)
			{
				this.SynchronizingObject.Invoke(this.Disconnected, new object[]{this,args}); // call from other thread than component itself located
			}
			else
			{
				this.Disconnected(this, args); // call from same thread
			}		
		}

		// setup new thread that will wait conn/disconn event
		private void StartWatch()
		{
			this.StopWatch();
			this._watchThread=new Thread(new ThreadStart(this.RunWatch));
			this._watchThread.Start();
		}

		// stop & terminate watching thread
		private void StopWatch()
		{
			if(this._watchThread==null)
				return;
			if(this._watchThread.IsAlive)
			{
				this._stopEvent.Set(); // now is signaled state
				this._watchThread.Join();
			}
			this._watchThread=null;
		}

		// watching thread itself
		private void RunWatch()
		{
			WaitHandle[] handles=new WaitHandle[]{this._stopEvent, this._disconnEvent};
			int ret;
			this._stopEvent.Reset(); // now is nonsignaled state
			this._disconnEvent.Reset(); // now is nonsignaled state
			while(true)
			{
				ret=WaitHandle.WaitAny(handles); //waits for any of 2 events go to signaled state
				if (ret >= 128)
					ret -= 128; // see MSDN: WaitHandle.WaitAny Method (WaitHandle[]), Note section
				if(ret==1) // disconnect occur
				{
					this.OnDisconnected();
					break;
				}
				else if(ret==0) // just not need this thread any more 
					break;
			}
		}
		#endregion
		#region public properties
		#region properties inaccessible in design-time, i.e. run-time only
		/// <summary>
		/// Get handle of current connection for interop with unmanaged code.
		/// </summary>
		[Browsable(false)]
		public IntPtr Handle	{get{return this._handle;}}
		
		/// <summary>
		/// Gets or sets the object used to marshal the event handler calls issued as a result of connection break.
		/// </summary>
		[Browsable(false)]
		public ISynchronizeInvoke SynchronizingObject
		{
			get
			{
				if(this._synchronizingObject==null)
				{
					if(this.DesignMode)
					{
						IDesignerHost dh=this.GetService(typeof(IDesignerHost)) as IDesignerHost;
						if(dh!=null)
						{
							this._synchronizingObject=dh.RootComponent as ISynchronizeInvoke;
						}
					}
				}
				return this._synchronizingObject;
			}
			set
			{
				this._synchronizingObject=value;
			}
		}

		/// <summary>
		/// Get the name of a TAPI device to use with created phone-book entry.
		/// </summary>
		[Browsable(false)]
		public string DeviceName{get{return this._ent.szDeviceName;}}
		#endregion
		#region primary properties
		// properties needed by RASDIALPARAMS structure
		/// <summary>
		/// Gets or sets a string that contains the phone-book entry's name to use to establish the connection.
		/// </summary>
		[Description("Specifies a string that contains the phone-book entry's name to use to establish the connection. Avoid assigning the same value of this property for two distinct connections")]
		[DefaultValue(null)]
		[Category("Primary Properties")]
		public string EntryName
		{
			get{return this._params.szEntryName;}
			set{this._params.szEntryName=value;}
		}

		/// <summary>
		/// Gets or sets a string that contains the user's user name.
		/// </summary>
		[Description("Specifies a string that contains the user's user name. This string is used to authenticate the user's access to the remote access server")]
		[DefaultValue(null)]
		[Category("Primary Properties")]
		public string UserName	
		{
			get{return this._params.szUserName;}
			set{this._params.szUserName=value;}
		}

		/// <summary>
		/// Gets or sets a string that contains the user's password.
		/// </summary>
		[Description("Specifies a string that contains the user's password. This string is used to authenticate the user's access to the remote access server")]
		[DefaultValue(null)]
		[Category("Primary Properties")]
		public string Password
		{
			get{return this._params.szPassword;}
			set{this._params.szPassword=value;}
		}

		/// <summary>
		/// Gets or sets a string that contains the domain on which authentication is to occur.
		/// </summary>
		[Description("Specifies a string that contains the domain on which authentication is to occur. A default empty string specifies the domain in which the remote access server is a member")]
		[DefaultValue(null)]
		[Category("Primary Properties")]
		public string Domain
		{
			get{return this._params.szDomain;}
			set{this._params.szDomain=value;}
		}
		// properties needed by RASENTRY structure
		/// <summary>
		/// Gets or sets a set of bit flags that specify connection options.
		/// </summary>
		[Description("A set of bit flags that specify connection options")]
		[DefaultValue(Ras.EntryOptions.RASEO_RequireMsCHAP2 | Ras.EntryOptions.RASEO_ShowDialingProgress | Ras.EntryOptions.RASEO_PreviewUserPw | Ras.EntryOptions.RASEO_Custom | Ras.EntryOptions.RASEO_SwCompression | Ras.EntryOptions.RASEO_RemoteDefaultGateway | Ras.EntryOptions.RASEO_DisableLcpExtensions)]
		[Category("Primary Properties")]
		public Ras.EntryOptions Options
		{
			get{return (Ras.EntryOptions)this._ent.dwfOptions;}
			set{this._ent.dwfOptions=(uint)value;}
		}

		/// <summary>
		/// Gets or sets a device-type specific destination string. It may be telephone number/service name/DNS name/IP address.
		/// </summary>
		[Description("Specifies a device-type specific destination string. For various device types(props. DeviceType/EntryType) it may be telephone number/service name/DNS name/IP address")]
		[DefaultValue(null)]
		[Category("Primary Properties")]
		public string Destination
		{
			get{return this._ent.szLocalPhoneNumber;}
			set{this._ent.szLocalPhoneNumber=value;}
		}

		/// <summary>
		/// Gets or sets the framing protocol used by the server.
		/// </summary>
		[Description("Specifies the framing protocol used by the server. PPP is the emerging standard. SLIP is used mainly in UNIX environments")]
		[DefaultValue(Ras.EntryFramingProtocols.RASFP_Ppp)]
		[Category("Primary Properties")]
		public Ras.EntryFramingProtocols FramingProtocol
		{
			get{return (Ras.EntryFramingProtocols)this._ent.dwFramingProtocol;}
			set{this._ent.dwFramingProtocol=(uint)value;}
		}

		/// <summary>
		/// Gets or sets the type of phone-book entry.
		/// </summary>
		[Description("The type of phone-book entry")]
		[DefaultValue(Ras.EntryTypes.RASET_Vpn)]
		[Category("Primary Properties")]
		public Ras.EntryTypes EntryType
		{
			get{return (Ras.EntryTypes)this._ent.dwType;}
			set{this._ent.dwType=(uint)value;}
		}

		/// <summary>
		/// Gets or sets the VPN strategy to use when dialing a VPN connection.
		/// </summary>
		[Description("The VPN strategy to use when dialing a VPN connection")]
		[DefaultValue(Ras.EntryVpnStrategies.VS_PptpOnly)]
		[Category("Primary Properties")]
		public Ras.EntryVpnStrategies VpnStrategy
		{
			get{return (Ras.EntryVpnStrategies)this._ent.dwVpnStrategy;}
			set{this._ent.dwVpnStrategy=(uint)value;}
		}

		/// <summary>
		/// Gets or sets the RAS device type.
		/// </summary>
		[Description("Specifies the RAS device type")]
		[DefaultValue(Ras.EntryDeviceTypes.RASDT_Vpn)]
		[Category("Primary Properties")]
		public Ras.EntryDeviceTypes DeviceType
		{
			get
			{
				switch(this._ent.szDeviceType)
				{
					case "modem":
						return Ras.EntryDeviceTypes.RASDT_Modem;
					case "isdn":
						return Ras.EntryDeviceTypes.RASDT_Isdn;
					case "x25":
						return Ras.EntryDeviceTypes.RASDT_X25;
					case "vpn":
						return Ras.EntryDeviceTypes.RASDT_Vpn;
					case "pad":
						return Ras.EntryDeviceTypes.RASDT_Pad;
					case "GENERIC":
						return Ras.EntryDeviceTypes.RASDT_Generic;
					case "SERIAL":
						return Ras.EntryDeviceTypes.RASDT_Serial;
					case "FRAMERELAY":
						return Ras.EntryDeviceTypes.RASDT_FrameRelay;
					case "ATM":
						return Ras.EntryDeviceTypes.RASDT_Atm;
					case "SONET":
						return Ras.EntryDeviceTypes.RASDT_Sonet;
					case "SW56":
						return Ras.EntryDeviceTypes.RASDT_SW56;
					case "IRDA":
						return Ras.EntryDeviceTypes.RASDT_Irda;
					case "PARALLEL":
						return Ras.EntryDeviceTypes.RASDT_Parallel;
					default: // assume VPN as default type
						return Ras.EntryDeviceTypes.RASDT_Vpn;
				}
			}
			set
			{
				switch(value)
				{
					case Ras.EntryDeviceTypes.RASDT_Modem:
						this._ent.szDeviceType="modem";
						break;
					case Ras.EntryDeviceTypes.RASDT_Isdn:
						this._ent.szDeviceType="isdn";
						break;
					case Ras.EntryDeviceTypes.RASDT_X25:
						this._ent.szDeviceType="x25";
						break;
					case Ras.EntryDeviceTypes.RASDT_Vpn:
						this._ent.szDeviceType="vpn";
						break;
					case Ras.EntryDeviceTypes.RASDT_Pad:
						this._ent.szDeviceType="pad";
						break;
					case Ras.EntryDeviceTypes.RASDT_Generic:
						this._ent.szDeviceType="GENERIC";
						break;
					case Ras.EntryDeviceTypes.RASDT_Serial:
						this._ent.szDeviceType="SERIAL";
						break;
					case Ras.EntryDeviceTypes.RASDT_FrameRelay:
						this._ent.szDeviceType="FRAMERELAY";
						break;
					case Ras.EntryDeviceTypes.RASDT_Atm:
						this._ent.szDeviceType="ATM";
						break;
					case Ras.EntryDeviceTypes.RASDT_Sonet:
						this._ent.szDeviceType="SONET";
						break;
					case Ras.EntryDeviceTypes.RASDT_SW56:
						this._ent.szDeviceType="SW56";
						break;
					case Ras.EntryDeviceTypes.RASDT_Irda:
						this._ent.szDeviceType="IRDA";
						break;
					case Ras.EntryDeviceTypes.RASDT_Parallel:
						this._ent.szDeviceType="PARALLEL";
						break;
				}
			}
		}
		#endregion
		#region others properties
		// properties needed by RASDIALPARAMS structure
		/// <summary>
		/// Gets or sets a string that contains a callback phone number.
		/// </summary>
		[Description("Specifies a string that contains a callback phone number. An empty default string indicates that callback should not be used")]
		[DefaultValue(null)]
		[Category("Others Properties")]
		public string CallbackNumber
		{
			get{return this._params.szCallbackNumber;}
			set{this._params.szCallbackNumber=value;}
		}
		// properties needed by RASENTRY structure
		/// <summary>
		/// Gets or sets the TAPI country identifier.
		/// </summary>
		[Description("Specifies the TAPI country identifier. This value is ignored unless the Options property specifies the RASEO_UseCountryAndAreaCodes flag")]
		[DefaultValue(0)]
		[Category("Others Properties")]
		public int CountryID
		{
			get{return this._ent.dwCountryID;}
			set{this._ent.dwCountryID=value;}
		}
		/// <summary>
		/// Gets or sets the country code portion of the phone number.
		/// </summary>
		[Description("Specifies the country code portion of the phone number. The country code must correspond to the country identifier specified by CountryID. This value is ignored unless the Options property specifies the RASEO_UseCountryAndAreaCodes flag")]
		[DefaultValue(0)]
		[Category("Others Properties")]
		public int CountryCode
		{
			get{return this._ent.dwCountryCode;}
			set{this._ent.dwCountryCode=value;}
		}
		/// <summary>
		/// Gets or sets the area code.
		/// </summary>
		[Description("Specifies the area code. If the dialing location does not have an area code, specify an empty string. Do not include parentheses or other delimiters in the area code string. For example, \"206\" is a valid area code; \"(206)\" is not. This value is ignored unless the Options property specifies the RASEO_UseCountryAndAreaCodes flag")]
		[DefaultValue(null)]
		[Category("Others Properties")]
		public string AreaCode
		{
			get{return this._ent.szAreaCode;}
			set{this._ent.szAreaCode=value;}
		}
		/// <summary>
		/// Gets or sets the IP address to be used while this connection is active.
		/// </summary>
		[Description("Specifies the IP address to be used while this connection is active. This value is ignored unless the Options property specifies the RASEO_SpecificIpAddr flag")]
		[Category("Others Properties")]
		public Ras.RASIPADDR IPaddr
		{
			get{return this._ent.ipaddr;}
			set{this._ent.ipaddr=value;}
		}
		/// <summary>
		/// Gets or sets the IP address of the DNS server to be used while this connection is active.
		/// </summary>
		[Description("Specifies the IP address of the DNS server to be used while this connection is active. This value is ignored unless the Options property specifies the RASEO_SpecificNameServers flag")]
		[Category("Others Properties")]
		public Ras.RASIPADDR IPaddrDns
		{
			get{return this._ent.ipaddrDns;}
			set{this._ent.ipaddrDns=value;}
		}
		/// <summary>
		/// Gets or sets the IP address of a secondary or backup DNS server to be used while this connection is active.
		/// </summary>
		[Description("Specifies the IP address of a secondary or backup DNS server to be used while this connection is active. This value is ignored unless the Options property specifies the RASEO_SpecificNameServers flag")]
		[Category("Others Properties")]
		public Ras.RASIPADDR IPaddrDnsAlt
		{
			get{return this._ent.ipaddrDnsAlt;}
			set{this._ent.ipaddrDnsAlt=value;}
		}
		/// <summary>
		/// Gets or sets the IP address of the WINS server to be used while this connection is active.
		/// </summary>
		[Description("Specifies the IP address of the WINS server to be used while this connection is active. This value is ignored unless the Options property specifies the RASEO_SpecificNameServers flag")]
		[Category("Others Properties")]
		public Ras.RASIPADDR IPaddrWins
		{
			get{return this._ent.ipaddrWins;}
			set{this._ent.ipaddrWins=value;}
		}
		/// <summary>
		/// Gets or sets the IP address of a secondary WINS server to be used while this connection is active.
		/// </summary>
		[Description("Specifies the IP address of a secondary WINS server to be used while this connection is active. This value is ignored unless the Options property specifies the RASEO_SpecificNameServers flag")]
		[Category("Others Properties")]
		public Ras.RASIPADDR IPaddrWinsAlt
		{
			get{return this._ent.ipaddrWinsAlt;}
			set{this._ent.ipaddrWinsAlt=value;}
		}
		/// <summary>
		/// Gets or sets the network protocol frame size.
		/// </summary>
		[Description("Specifies the network protocol frame size. The value should be either 1006 or 1500. This value is ignored unless FramingProtocol property specifies the RASFP_Slip flag")]
		[DefaultValue(1500)]
		[Category("Others Properties")]
		public int FrameSize
		{
			get{return this._ent.dwFrameSize;}
			set{this._ent.dwFrameSize=value;}
		}
		/// <summary>
		/// Gets or sets the network protocols to negotiate.
		/// </summary>
		[Description("Specifies the network protocols to negotiate")]
		[DefaultValue(Ras.EntryProtocols.RASNP_Ip)]
		[Category("Others Properties")]
		public Ras.EntryProtocols NetProtocols
		{
			get{return (Ras.EntryProtocols)this._ent.dwfNetProtocols;}
			set{this._ent.dwfNetProtocols=(uint)value;}
		}
		/// <summary>
		/// Gets or sets the name of the script file.
		/// </summary>
		[Description("Specifies the name of the script file. The file name should be a full path. This field is only used for analog dial-up connections")]
		[DefaultValue(null)]
		[Editor(typeof(ScriptEditor), typeof(UITypeEditor))]
		[Category("Others Properties")]
		public string Script
		{
			get{return this._ent.szScript;}
			set{this._ent.szScript=value;}
		}
		/// <summary>
		/// Gets or sets string that identifies the X.25 PAD type.
		/// </summary>
		[Description("String that identifies the X.25 PAD type. Set this property to default empty string unless the entry should dial using an X.25 PAD. The X25PadType string maps to a section name in PAD.INF")]
		[DefaultValue(null)]
		[Category("Others Properties")]
		public string X25PadType
		{
			get{return this._ent.szX25PadType;}
			set{this._ent.szX25PadType=value;}
		}
		/// <summary>
		/// Gets or sets string that identifies the X.25 address to which to connect.
		/// </summary>
		[Description("String that identifies the X.25 address to which to connect. Set this property to default empty string unless the entry should dial using an X.25 PAD or native X.25 device")]
		[DefaultValue(null)]
		[Category("Others Properties")]
		public string X25Address
		{
			get{return this._ent.szX25Address;}
			set{this._ent.szX25Address=value;}
		}
		/// <summary>
		/// Gets or sets string that specifies the facilities to request from the X.25 host at connection.
		/// </summary>
		[Description("String that specifies the facilities to request from the X.25 host at connection. This member is ignored if X25Address is an empty string")]
		[DefaultValue(null)]
		[Category("Others Properties")]
		public string X25Facilities
		{
			get{return this._ent.szX25Facilities;}
			set{this._ent.szX25Facilities=value;}
		}
		/// <summary>
		/// Gets or sets string that specifies additional connection information supplied to the X.25 host at connection.
		/// </summary>
		[Description("String that specifies additional connection information supplied to the X.25 host at connection. This member is ignored if X25Address is an empty string")]
		[DefaultValue(null)]
		[Category("Others Properties")]
		public string X25UserData
		{
			get{return this._ent.szX25UserData;}
			set{this._ent.szX25UserData=value;}
		}
		/// <summary>
		/// Gets or sets the number of seconds after which the connection is terminated due to inactivity.
		/// </summary>
		[Description("Specifies the number of seconds after which the connection is terminated due to inactivity. Note that unless the idle time out is disabled, the entire connection is terminated if the connection is idle for the specified interval. This member can specify a number of seconds, or 0 to specify that there is no idle time out for this connection.")]
		[DefaultValue(0)]
		[Category("Others Properties")]
		public int IdleDisconnectSeconds
		{
			get{return this._ent.dwIdleDisconnectSeconds;}
			set{this._ent.dwIdleDisconnectSeconds=value;}
		}
		/// <summary>
		/// Gets or sets the type of encryption to use with the connection.
		/// </summary>
		[Description("The type of encryption to use with the connection. The encryption is either provided by IPSec (for L2TP/IPSec connections) or by Microsoft Point-to-Point Encryption (MPPE).")]
		[DefaultValue(Ras.EntryEncryptionTypes.ET_Optional)]
		[Category("Others Properties")]
		public Ras.EntryEncryptionTypes EncryptionType
		{
			get{return (Ras.EntryEncryptionTypes)this._ent.dwEncryptionType;}
			set{this._ent.dwEncryptionType=(uint)value;}
		}
		/// <summary>
		/// Gets or sets the authentication key provided to the EAP vendor.
		/// </summary>
		[Description("This member is used for Extensible Authentication Protocol (EAP). This member contains the authentication key provided to the EAP vendor. In most cases there is not needed to change default value")]
		[DefaultValue(0)]
		[Category("Others Properties")]
		public int CustomAuthKey
		{
			get{return this._ent.dwCustomAuthKey;}
			set{this._ent.dwCustomAuthKey=value;}
		}
		/// <summary>
		/// Gets or sets the full path and file name for the dynamic link library (DLL) that implements the custom-dialing functions.
		/// </summary>
		[Description("Contains the full path and file name for the dynamic link library (DLL) that implements the custom-dialing functions")]
		[DefaultValue(null)]
		[Editor(typeof(DialDLLEditor), typeof(UITypeEditor))]
		[Category("Others Properties")]
		public string CustomDialDll
		{
			get{return this._ent.szCustomDialDll;}
			set{this._ent.szCustomDialDll=value;}
		}
		#endregion
		#endregion
		#region public methods
		/// <summary>
		/// Create new entry in phone-book, then establish connection. <see cref="Disconnect()"/> method must be called in any case to balance call of this method.
		/// </summary>
		public void Connect()
		{
			if(this._handle!=IntPtr.Zero)
			{
				Ras.RASCONNSTATUS status = new Ras.RASCONNSTATUS();
				uint res=Ras.RasGetConnectStatus(this._handle, status);
				if(res==6) // i.e. ERROR_INVALID_HANDLE
					this._handle = IntPtr.Zero;
				else
					return; // already established
			}
			uint validateName = Ras.RasValidateEntryName(null, this._params.szEntryName);
			if(validateName!=(uint)RasError.SUCCESS)
				throw new RasException("Entry name " + ((this._params.szEntryName==null || this._params.szEntryName.Length<=0) ? "<Empty Name>" : this._params.szEntryName) + " not valid or already presented in phone-book.");
			// 1) Create new entry
			this._ent.dwDialMode = 1;
			this._ent.dwDialExtraPercent = 75;
			this._ent.dwDialExtraSampleSeconds = 120;
			this._ent.dwHangUpExtraPercent = 10;
			this._ent.dwHangUpExtraSampleSeconds = 120;
			this.SetEntry();
			// just test that entry was written successfully
			this.GetEntry();
			// 2) Use created entry to establish connection
			try
			{
				Ras.RasCheck(Ras.RasDial(null, (this._phonebook!=null && this._phonebook.Length>0) ? this._phonebook : null, this._params, 0, null, ref this._handle));
			}
			catch (Exception ex)
			{
				this.Disconnect();
				throw ex;
			}
			
			this.OnConnected();
			
			this.StartWatch();
			Ras.RasConnectionNotification(this._handle, this._disconnEvent.Handle, Ras.RASNOTIFICATION.RASCN_Disconnection);
			
		}
		
		/// <summary>
		/// Remove entry from phone-book, then break connection established early.
		/// </summary>
		public void Disconnect()
		{
			if(this._handle==IntPtr.Zero)
				return;
			
			Ras.RASCONNSTATUS status = new Ras.RASCONNSTATUS();

			// force hangup
			try
			{
				Ras.RasCheck(Ras.RasHangUp(this._handle));
			}
			catch (RasException ex)
			{
				if (ex.ErrorCode!=(uint)RasError.ERROR_NO_CONNECTION)
					throw ex;
			}

			while(6!=Ras.RasGetConnectStatus(this._handle, status))
			{
				Thread.Sleep(0);
			}

			this.DeleteEntry();
			this._handle = IntPtr.Zero;

		}
		#endregion
		#region methods for design-time support
		// --- ShouldSerialize... Methods ---
		private bool ShouldSerializeIPaddr()
		{
			return((this._ent.ipaddr.a!=0) || (this._ent.ipaddr.b!=0) || (this._ent.ipaddr.c!=0) || (this._ent.ipaddr.d!=0));
		}
		private bool ShouldSerializeIPaddrDns()
		{
			return((this._ent.ipaddrDns.a!=0) || (this._ent.ipaddrDns.b!=0) || (this._ent.ipaddrDns.c!=0) || (this._ent.ipaddrDns.d!=0));
		}
		private bool ShouldSerializeIPaddrDnsAlt()
		{
			return((this._ent.ipaddrDnsAlt.a!=0) || (this._ent.ipaddrDnsAlt.b!=0) || (this._ent.ipaddrDnsAlt.c!=0) || (this._ent.ipaddrDnsAlt.d!=0));
		}
		private bool ShouldSerializeIPaddrWins()
		{
			return((this._ent.ipaddrWins.a!=0) || (this._ent.ipaddrWins.b!=0) || (this._ent.ipaddrWins.c!=0) || (this._ent.ipaddrWins.d!=0));
		}
		private bool ShouldSerializeIPaddrWinsAlt()
		{
			return((this._ent.ipaddrWinsAlt.a!=0) || (this._ent.ipaddrWinsAlt.b!=0) || (this._ent.ipaddrWinsAlt.c!=0) || (this._ent.ipaddrWinsAlt.d!=0));
		}
		// --- Reset... Methods ---
		private void ResetIPaddr()
		{
			this._ent.ipaddr.a=this._ent.ipaddr.b=this._ent.ipaddr.c=this._ent.ipaddr.d=0;
		}
		private void ResetIPaddrDns() 
		{
			this._ent.ipaddrDns.a=this._ent.ipaddrDns.b=this._ent.ipaddrDns.c=this._ent.ipaddrDns.d=0;
		}
		private void ResetIPaddrDnsAlt()
		{
			this._ent.ipaddrDnsAlt.a=this._ent.ipaddrDnsAlt.b=this._ent.ipaddrDnsAlt.c=this._ent.ipaddrDnsAlt.d=0;
		}
		private void ResetIPaddrWins()
		{
			this._ent.ipaddrWins.a=this._ent.ipaddrWins.b=this._ent.ipaddrWins.c=this._ent.ipaddrWins.d=0;
		}
		private void ResetIPaddrWinsAlt()
		{
			this._ent.ipaddrWinsAlt.a=this._ent.ipaddrWinsAlt.b=this._ent.ipaddrWinsAlt.c=this._ent.ipaddrWinsAlt.d=0;
		}
		#endregion
	}

	#region design-time support
	public class FlagsConverter : TypeConverter 
	{
		public override bool CanConvertFrom(	ITypeDescriptorContext context, Type sourceType)
		{
			// We don't want convert from a string
			if(sourceType == typeof(string)) {return false;}
			return base.CanConvertFrom(context, sourceType);
		}

	}

	public class IPConverter : TypeConverter 
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			// We don't want convert from a string
			if(sourceType==typeof(string)) {return false;}
			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if(destinationType==typeof(string) || destinationType==typeof(InstanceDescriptor)) {return true;}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if( value is Ras.RASIPADDR && (destinationType == typeof(string)) )
			{
				Ras.RASIPADDR i = (Ras.RASIPADDR)value;
				return string.Format("{0}.{1}.{2}.{3}", i.a.ToString(), i.b.ToString(), i.c.ToString(), i.d.ToString());
			}
			else if( value is Ras.RASIPADDR && (destinationType == typeof(InstanceDescriptor)) )
			{
				Ras.RASIPADDR i = (Ras.RASIPADDR)value;
				byte[] values = new byte[4];
				Type[] types = new Type[4];
				types[0] = types[1] = types[2] = types[3] = typeof(byte);
				values[0] = i.a; values[1] = i.b; values[2] = i.c; values[3] = i.d;
				ConstructorInfo ci = typeof(Ras.RASIPADDR).GetConstructor(types);
				return new InstanceDescriptor(ci, values);
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}

	public class FlagsEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if(context != null)
				return UITypeEditorEditStyle.DropDown;
			return base.GetEditStyle(context);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if((context != null) && (provider != null)) 
			{
				// Access the Property Browser's UI display service
				IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)	provider.GetService(typeof(IWindowsFormsEditorService));
				if( editorService!=null && value.GetType()==typeof(Ras.EntryOptions) ) // edit Options flags
				{
					// Create an instance of the UI editor control
					OptionsFlagsEditorForm ed = new OptionsFlagsEditorForm();
					// Pass the UI editor control the current property value
					ed.Val = (Ras.EntryOptions)value;
					// Display the UI editor control
					editorService.ShowDialog(ed);
					// Return the new property value from the UI editor control
					return ed.Val;
				}
				else if( editorService!=null && value.GetType()==typeof(Ras.EntryProtocols) ) // edit Protocols flags
				{
					// Create an instance of the UI editor control
					ProtocolsFlagsEditorForm ed = new ProtocolsFlagsEditorForm();
					// Pass the UI editor control the current property value
					ed.Val = (Ras.EntryProtocols)value;
					// Display the UI editor control
					editorService.ShowDialog(ed);
					// Return the new property value from the UI editor control
					return ed.Val;
				}
			}
			return base.EditValue(context, provider, value);
		}
	}

	public class DialDLLEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if(context != null)
				return UITypeEditorEditStyle.Modal;
			return base.GetEditStyle(context);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if((context != null) && (provider != null)) 
			{
				// Access the Property Browser's UI display service
				IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)	provider.GetService(typeof(IWindowsFormsEditorService));
				if(editorService!=null)
				{
					// Create an instance of the UI editor control
					OpenFileDialog of = new OpenFileDialog();
					of.InitialDirectory = "c:\\" ;
					of.Filter = "DLL files (*.dll)|*.dll|All files (*.*)|*.*" ;
					of.RestoreDirectory = true ;
					// Display the UI editor control
					if(of.ShowDialog()==DialogResult.OK)
						return (of.FileName.Length>0 ? of.FileName : null);
					else
						return null;
				}
			}
			return base.EditValue(context, provider, value);
		}
	}

	public class ScriptEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if(context != null)
				return UITypeEditorEditStyle.Modal;
			return base.GetEditStyle(context);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if((context != null) && (provider != null)) 
			{
				// Access the Property Browser's UI display service
				IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)	provider.GetService(typeof(IWindowsFormsEditorService));
				if(editorService!=null)
				{
					// Create an instance of the UI editor control
					OpenFileDialog of = new OpenFileDialog();
					of.InitialDirectory = "c:\\" ;
					of.Filter = "Script files (*.scp)|*.scp|All files (*.*)|*.*" ;
					of.RestoreDirectory = true ;
					// Display the UI editor control
					if(of.ShowDialog()==DialogResult.OK)
						return (of.FileName.Length>0 ? of.FileName : null);
					else
						return null;
				}
			}
			return base.EditValue(context, provider, value);
		}
	}

	public class IPEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if(context != null)
				return UITypeEditorEditStyle.Modal;
			return base.GetEditStyle(context);
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if((context != null) && (provider != null)) 
			{
				// Access the Property Browser's UI display service
				IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)	provider.GetService(typeof(IWindowsFormsEditorService));
				if(editorService!=null)
				{
					// Create an instance of the UI editor control
					IPeditorForm ipe = new IPeditorForm();
					// Pass the UI editor control the current property value
					Ras.RASIPADDR i = (Ras.RASIPADDR)value;
					ipe.A = i.a;
					ipe.B = i.b;
					ipe.C = i.c;
					ipe.D = i.d;
					// Display the UI editor control
					if(editorService.ShowDialog(ipe)==DialogResult.OK)
					{
						i.a = ipe.A;
						i.b = ipe.B;
						i.c = ipe.C;
						i.d = ipe.D;
						return i;
					}
				}
			}
			return base.EditValue(context, provider, value);
		}
	}
	#endregion
}
