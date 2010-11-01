#define WINVER4 // Win9x/ME, WinNT_4
#define WINVER5 // Win2000
//#define WINVER501 // WinXP

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using Genetibase.Network.Design;

namespace Genetibase.Network.Ras
{
	#region callback functions RasDialFunc*
	internal delegate void RasDialFunc(
	uint unMsg,                // type of event that has occurred
	Ras.RASCONNSTATE rasconnstate, // connection state about to be entered
	int dwError              // error that may have occurred
	);

	internal delegate void RasDialFunc1(
	IntPtr hrasconn,    // handle to RAS connection
	uint unMsg,           // type of event that has occurred
	Ras.RASCONNSTATE rascs,   // connection state about to be entered
	uint dwError,        // error that may have occurred
	uint dwExtendedError // extended error information for some errors
	);

	internal delegate void RasDialFunc2(
	int dwCallbackId,    // user-defined value specified in
	//  RasDial call
	int dwSubEntry,      // subentry index in multilink connection
	IntPtr hrasconn,     // handle to RAS connection
	uint unMsg,            // type of event that has occurred
	Ras.RASCONNSTATE rascs,    // connection state about to be entered
	uint dwError,         // error that may have occurred
	uint dwExtendedError  // extended error information for
	//  some errors
	);
	#endregion

	/// <summary>
	/// Class represent interface layer to unmanaged API
	/// </summary>
	public sealed class Ras
	{
		#region interop const/enums/structures/classes prototypes
		public const int RAS_MaxDeviceType=16;
		public const int RAS_MaxPhoneNumber=128;
		public const int RAS_MaxIpAddress=15;
		public const int RAS_MaxIpxAddress=21;
#if WINVER4
		public const int RAS_MaxEntryName=256;
		public const int RAS_MaxDeviceName=128;
		public const int RAS_MaxCallbackNumber=RAS_MaxPhoneNumber;
#else
		public const int RAS_MaxEntryName      =20;
		public const int RAS_MaxDeviceName     =32;
		public const int RAS_MaxCallbackNumber =48;
#endif
		public const int RAS_MaxAreaCode=10;
		public const int RAS_MaxPadType=32;
		public const int RAS_MaxX25Address=200;
		public const int RAS_MaxFacilities=200;
		public const int RAS_MaxUserData=200;
		public const int RAS_MaxReplyMessage=1024;
		public const int RAS_MaxDnsSuffix=256;
		public const int UNLEN=256;
		public const int PWLEN=256;
		public const int DNLEN=15;
		public const int RASCS_PAUSED=0x1000;
		public const int RASCS_DONE=0x2000;

		/// <summary>
		/// Enumerates intermediate states to a connection.
		/// </summary>
		public enum RASCONNSTATE 
		{ 
			RASCS_OpenPort=0, 
			RASCS_PortOpened, 
			RASCS_ConnectDevice, 
			RASCS_DeviceConnected, 
			RASCS_AllDevicesConnected, 
			RASCS_Authenticate, 
			RASCS_AuthNotify, 
			RASCS_AuthRetry, 
			RASCS_AuthCallback, 
			RASCS_AuthChangePassword, 
			RASCS_AuthProject, 
			RASCS_AuthLinkSpeed, 
			RASCS_AuthAck, 
			RASCS_ReAuthenticate, 
			RASCS_Authenticated, 
			RASCS_PrepareForCallback, 
			RASCS_WaitForModemReset, 
			RASCS_WaitForCallback,
			RASCS_Projected, 
#if (WINVER4) 
			RASCS_StartAuthentication, // Windows 95 only
			RASCS_CallbackComplete, // Windows 95 only
			RASCS_LogonNetwork, // Windows 95 only
#endif 
			RASCS_SubEntryConnected,
			RASCS_SubEntryDisconnected,
			RASCS_Interactive = RASCS_PAUSED,
			RASCS_RetryAuthentication,
			RASCS_CallbackSetByCaller, 
			RASCS_PasswordExpired,
#if (WINVER5)
			RASCS_InvokeEapUI,
#endif
			RASCS_Connected = RASCS_DONE,
			RASCS_Disconnected
		}

		/// <summary>
		/// Flags for RasConnectionNotification().
		/// </summary>
		public enum RASNOTIFICATION:uint
		{
			RASCN_Connection=0x00000001,
			RASCN_Disconnection=0x00000002,
			RASCN_BandwidthAdded=0x00000004,
			RASCN_BandwidthRemoved=0x00000008,
			RASCN_All = RASCN_Connection|RASCN_Disconnection|RASCN_BandwidthAdded|RASCN_BandwidthRemoved
		}
		
		/// <summary>
		/// RASENTRY 'dwfOptions' bit flags.
		/// </summary>
		[Flags]
		[Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(FlagsConverter))]
		public enum EntryOptions:uint
		{
			RASEO_UseCountryAndAreaCodes=0x00000001,
			RASEO_SpecificIpAddr=0x00000002,
			RASEO_SpecificNameServers=0x00000004,
			RASEO_IpHeaderCompression=0x00000008,
			RASEO_RemoteDefaultGateway=0x00000010,
			RASEO_DisableLcpExtensions=0x00000020,
			RASEO_TerminalBeforeDial=0x00000040,
			RASEO_TerminalAfterDial=0x00000080,
			RASEO_ModemLights=0x00000100,
			RASEO_SwCompression=0x00000200,
			RASEO_RequireEncryptedPw=0x00000400,
			RASEO_RequireMsEncryptedPw=0x00000800,
			RASEO_RequireDataEncryption=0x00001000,
			RASEO_NetworkLogon=0x00002000,
			RASEO_UseLogonCredentials=0x00004000,
			RASEO_PromoteAlternates=0x00008000,
#if WINVER4
			RASEO_SecureLocalFiles=0x00010000,
#endif
#if WINVER5
			RASEO_RequireEAP=0x00020000,
			RASEO_RequirePAP=0x00040000,
			RASEO_RequireSPAP=0x00080000,
			RASEO_Custom=0x00100000,
			RASEO_PreviewPhoneNumber=0x00200000,
			RASEO_SharedPhoneNumbers=0x00800000,
			RASEO_PreviewUserPw=0x01000000,
			RASEO_PreviewDomain=0x02000000,
			RASEO_ShowDialingProgress=0x04000000,
			RASEO_RequireCHAP=0x08000000,
			RASEO_RequireMsCHAP=0x10000000,
			RASEO_RequireMsCHAP2=0x20000000,
			RASEO_RequireW95MSCHAP=0x40000000,
			RASEO_CustomScript=0x80000000
#endif
		}

		/// <summary>
		/// RASENTRY 'szDeviceType' default types.
		/// </summary>
		public enum EntryDeviceTypes:uint
		{
			RASDT_Modem,
			RASDT_Isdn,
			RASDT_X25,
			RASDT_Vpn,
			RASDT_Pad,
			RASDT_Generic,
			RASDT_Serial,
			RASDT_FrameRelay,
			RASDT_Atm,
			RASDT_Sonet,
			RASDT_SW56,
			RASDT_Irda,
			RASDT_Parallel
		}
		
		/// <summary>
		/// RASENTRY 'dwProtocols' bit flags.
		/// </summary>
		[Flags]
		[Editor(typeof(FlagsEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(FlagsConverter))]
		public enum EntryProtocols:uint
		{
			RASNP_NetBEUI=0x00000001,
			RASNP_Ipx=0x00000002,
			RASNP_Ip=0x00000004
		}
		
		/// <summary>
		/// RASENTRY 'dwFramingProtocols' bit flags.
		/// </summary>
		public enum EntryFramingProtocols:uint
		{
			RASFP_Ppp=0x00000001,
			RASFP_Slip=0x00000002
			//RASFP_Ras=0x00000004 - this flag is no longer supported
		}
		
		/// <summary>
		/// RASENTRY 'dwType' member
		/// </summary>
		public enum EntryTypes
		{
			RASET_Phone=1,  // Phone lines: modem, ISDN, X.25, etc
			RASET_Vpn=2,  // Virtual private network
			RASET_Direct=3,  // Direct connect: serial, parallel
			RASET_Internet=4  // BaseCamp internet
#if WINVER501
			RASET_Broadband 5  // Broadband
#endif
		}

		/// <summary>
		/// RASENTRY 'dwEncryptionType' member
		/// </summary>
		public enum EntryEncryptionTypes
		{
			ET_None=0,  // No encryption
			ET_Require=1,  // Require Encryption
			ET_RequireMax=2,  // Require max encryption
			ET_Optional=3  // Do encryption if possible. None Ok
		}

		/// <summary>
		/// RASENTRY 'dwVpnStrategy' member
		/// </summary>
		public enum EntryVpnStrategies
		{
			VS_Default=0, // RAS dials PPTP first. If PPTP fails, L2TP is attempted.
			VS_PptpOnly=1,  // RAS dials only PPTP
			VS_PptpFirst=2,  // RAS always dials PPTP first
			VS_L2tpOnly=3, // RAS dials only L2TP
			VS_L2tpFirst=4 // RAS always dials L2TP first
		}
		

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct GUID
		{
			public uint Data1;
			public ushort Data2;
			public ushort Data3;
			[MarshalAs(UnmanagedType.ByValArray,SizeConst=8)]
			public byte[] Data4;
		}
		[Editor(typeof(IPEditor), typeof(UITypeEditor))]
		[TypeConverter(typeof(IPConverter))]
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct RASIPADDR 
		{
			public byte a;
			public byte b;
			public byte c;
			public byte d;

			public RASIPADDR(byte _a, byte _b, byte _c, byte _d){this.a=_a; this.b=_b; this.c=_c; this.d=_d;}
		}
		[StructLayout(LayoutKind.Sequential)]
		public struct RASEAPINFO 
		{
			public uint dwSizeofEapInfo;
			public int pbEapInfo;
		}
		[StructLayout(LayoutKind.Sequential)]
		public class RASDIALEXTENSIONS 
		{ 
			public readonly int dwSize=Marshal.SizeOf(typeof(RASDIALEXTENSIONS));
			public uint dwfOptions=0;
			public int hwndParent=0;
			public int reserved=0;
			public int reserved1=0;
			public RASEAPINFO RasEapInfo=new RASEAPINFO();
		}
		[StructLayout(LayoutKind.Sequential,CharSet=CharSet.Auto)]
		public struct RASDEVINFO 
		{
			public int dwSize;
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxDeviceType+1)]
			public string szDeviceType;
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxDeviceName+1)]
			public string szDeviceName;
		}
		[StructLayout(LayoutKind.Sequential,CharSet=CharSet.Auto)]
		public class RASDIALPARAMS 
		{ 
			public readonly int dwSize=Marshal.SizeOf(typeof(RASDIALPARAMS));  // size, in bytes, of the RASDIALPARAMS structure
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=RAS_MaxEntryName+1)]
			public string  szEntryName=null; // phone-book entry to use to establish the connection
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=RAS_MaxPhoneNumber+1)]
			public string  szPhoneNumber=null; // leave this blank to indicates that the phone-book entry's phone number should be used
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=RAS_MaxCallbackNumber+1)]
			public string  szCallbackNumber=null; // contains a callback phone number
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=UNLEN+1)]
			public string  szUserName=null; // user's user name
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=PWLEN+1)]
			public string  szPassword=null; // user's password
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=DNLEN+1)]
			public string  szDomain=null; // domain on which authentication is to occur
			public int dwSubEntry=0; // ignores in current version
			public int dwCallbackId=0; // ignores in current version
		}
		[StructLayout(LayoutKind.Sequential,CharSet=CharSet.Auto)]
		public class RASCONNSTATUS 
		{ 
			public readonly int dwSize=Marshal.SizeOf(typeof(RASCONNSTATUS));  // size, in bytes, of the RASCONNSTATUS structure
			public RASCONNSTATE  rasconnstate = RASCONNSTATE.RASCS_OpenPort; // current status of connection
			public int dwError=0; // error code
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=RAS_MaxDeviceType+1)]
			public string szDeviceType=null; // RAS device type referenced by szDeviceName
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=RAS_MaxDeviceName+1)]
			public string szDeviceName=null; // name of a TAPI device to use with this phone-book entry
		}
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct RASENTRY 
		{
			public int dwSize; // size, in bytes, of the RASENTRY structure
			public uint dwfOptions; // set of bit flags that specify connection options
			//
			// Location/phone number.
			//
			public int dwCountryID; // TAPI country identifier
			public int dwCountryCode; // country code portion of the phone number
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxAreaCode+1)]
			public string szAreaCode; // area code as a null-terminated string
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxPhoneNumber+1)]
			public string szLocalPhoneNumber; // device-type specific destination string
			public int dwAlternateOffset; // ignores in current version
			//
			// PPP/Ip
			//
			public RASIPADDR ipaddr; // IP address to be used while this connection is active
			public RASIPADDR ipaddrDns; // IP address of the DNS server to be used while this connection is active
			public RASIPADDR ipaddrDnsAlt; // IP address of a secondary or backup DNS server to be used while this connection is active
			public RASIPADDR ipaddrWins; // IP address of the WINS server to be used while this connection is active
			public RASIPADDR ipaddrWinsAlt; // IP address of a secondary WINS server to be used while this connection is active
			//
			// Framing
			//
			public int dwFrameSize; // network protocol frame size
			public uint dwfNetProtocols; // network protocols to negotiate
			public uint dwFramingProtocol; // framing protocol used by the server
			//
			// Scripting
			//
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=260)]//MAX_PATH
			public string szScript; // contains the name of the script file
			//
			// AutoDial
			//
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=260)]//MAX_PATH
			public string szAutodialDll; // this member is no longer used.
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=260)]//MAX_PATH
			public string szAutodialFunc; // this member is no longer used.
			//
			// Device
			//
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxDeviceType+1)]		
			public string szDeviceType; // RAS device type referenced by szDeviceName
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxDeviceName+1)]
			public string szDeviceName; // name of a TAPI device to use with this phone-book entry
			//
			// X.25
			//
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxPadType+1)]//MAX_PATH
			public string szX25PadType;
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxX25Address+1)]//MAX_PATH
			public string szX25Address;
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxFacilities+1)]//MAX_PATH
			public string szX25Facilities;
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=RAS_MaxUserData+1)]//MAX_PATH
			public string szX25UserData;
			public int dwChannels; // reserved for future use
			//
			// Reserved
			//
			public int dwReserved1;
			public int dwReserved2;
#if WINVER4
			//
			// Multilink and BAP
			//
			public int dwSubEntries;
			public int dwDialMode;
			public int dwDialExtraPercent;
			public int dwDialExtraSampleSeconds;
			public int dwHangUpExtraPercent;
			public int dwHangUpExtraSampleSeconds;
			//
			// Idle time out
			//
			public int dwIdleDisconnectSeconds;
#endif
#if WINVER5
			public uint dwType; // entry type
			public uint dwEncryptionType; // type of encryption to use
			public int     dwCustomAuthKey; // authentication key for EAP
			public GUID guidId; // guid that represents 
			[MarshalAs(UnmanagedType.ByValTStr,SizeConst=260)]//MAX_PATH
			public string szCustomDialDll; // DLL for custom dialing 
			public uint dwVpnStrategy; // specifies type of VPN protocol
#endif
		}
		#endregion
		#region interop functions prototypes
		/// <summary>
		/// Get properties for exist entry
		/// </summary>
		/// <param name="lpszPhonebook">Full path and file name of phone-book file</param>
		/// <param name="lpszEntry">Exist entry name</param>
		/// <param name="lpRasEntry">Buffer that receives entry information</param>
		/// <param name="lpdwEntryInfoSize">Size, in bytes, of the lpRasEntry buffer</param>
		/// <param name="lpbDeviceInfo">(not used in Win2K+)Structure that receives device-specific configuration information</param>
		/// <param name="lpdwDeviceInfoSize">(not used in Win2K+)Size, in bytes, of the lpbDeviceInfo buffer</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasGetEntryProperties(string lpszPhonebook, string lpszEntry, IntPtr lpRasEntry, ref int lpdwEntryInfoSize, int lpbDeviceInfo, int lpdwDeviceInfoSize);

		/// <summary>
		/// Create new or set properties for exist entry
		/// </summary>
		/// <param name="lpszPhonebook">Full path and file name of phone-book file</param>
		/// <param name="lpszEntry">New or exist entry name</param>
		/// <param name="lpRasEntry">Structure that contains entry information</param>
		/// <param name="dwEntryInfoSize">Size, in bytes, of the lpRasEntry buffer</param>
		/// <param name="lpbDeviceInfo">(not used in Win2K+)Structure that contains device-specific configuration information</param>
		/// <param name="dwDeviceInfoSize">(not used in Win2K+)Size, in bytes, of the lpbDeviceInfo buffer</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasSetEntryProperties(string lpszPhonebook, string lpszEntry, ref RASENTRY lpRasEntry, int dwEntryInfoSize, int lpbDeviceInfo, int dwDeviceInfoSize);

		/// <summary>
		/// Establishes a RAS connection between a RAS client and a RAS server.
		/// </summary>
		/// <param name="lpRasDialExtensions">Pointer to a RASDIALEXTENSIONS structure</param>
		/// <param name="lpszPhonebook">Pointer to a null-terminated string that specifies the full path and file name of a phone-book (PBK) file.
		/// If this parameter is NULL, the function uses the current default phone-book file.</param>
		/// <param name="lpRasDialParams">Pointer to a RASDIALPARAMS structure that specifies calling parameters for the RAS connection.</param>
		/// <param name="dwNotifierType">Specifies the nature of the lpvNotifier parameter. If lpvNotifier is NULL, dwNotifierType is ignored.</param>
		/// <param name="lpvNotifier">Specifies a RasDialFunc, RasDialFunc1, or RasDialFunc2 callback function to receive RasDial event notifications.</param>
		/// <param name="lphRasConn">Pointer to a variable. Set it to NULL before calling RasDial. If RasDial succeeds, it stores a handle to the RAS connection by this pointer.</param>
		/// <returns>If the function succeeds, the immediate return value is zero. In addition, the function stores a handle to the RAS connection into the variable pointed to by lphRasConn.
		/// If the function fails, the immediate return value is a nonzero error value, either from the set listed in the RasError.h header file or ERROR_NOT_ENOUGH_MEMORY.</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasDial([In]RASDIALEXTENSIONS lpRasDialExtensions, [In]string lpszPhonebook, [In]RASDIALPARAMS lpRasDialParams, uint dwNotifierType,
			Delegate lpvNotifier, ref IntPtr lphRasConn);

		/// <summary>
		/// Break the RAS connection
		/// </summary>
		/// <param name="hrasconn">Handle to the RAS connection to hang up</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasHangUp(IntPtr hrasconn);
		
		/// <summary>
		/// Return error description by error code
		/// </summary>
		/// <param name="uErrorValue">Error to get string for</param>
		/// <param name="lpszErrorString">Buffer to hold error string</param>
		/// <param name="cBufSize">Size, in characters, of lpszErrorString buffer</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasGetErrorString(uint uErrorValue, StringBuilder lpszErrorString, [In]int cBufSize);
		
		/// <summary>
		/// Get status of RAS connection
		/// </summary>
		/// <param name="hrasconn">Handle to RAS connection of interest</param>
		/// <param name="lprasconnstatus">Buffer to receive status data</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasGetConnectStatus(IntPtr hrasconn,  [In,Out]RASCONNSTATUS lprasconnstatus);
		
		/// <summary>
		/// Set handler for connection's event
		/// </summary>
		/// <param name="hrasconn">Handle to a RAS connection</param>
		/// <param name="hEvent">Handle to an event object</param>
		/// <param name="dwFlags">Type of event to receive notifications for</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasConnectionNotification(IntPtr hrasconn, IntPtr hEvent, Ras.RASNOTIFICATION dwFlags);

		/// <summary>
		/// Set handler for connection's event
		/// </summary>
		/// <param name="hrasconn">Handle to a RAS connection</param>
		/// <param name="hEvent">Handle to an event object</param>
		/// <param name="dwFlags">Type of event to receive notifications for</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasConnectionNotification(IntPtr hrasconn, IntPtr hEvent, uint dwFlags);

		/// <summary>
		/// Validate proposed name for entry
		/// </summary>
		/// <param name="lpszPhonebook">Full path and file name of phone-book file</param>
		/// <param name="lpszEntry">Entry name to validate</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasValidateEntryName(string lpszPhonebook, string lpszEntry);

		/// <summary>
		/// Remove existent entry from phone-book
		/// </summary>
		/// <param name="lpszPhonebook">Full path and file name of phone-book file</param>
		/// <param name="lpszEntry">Name of entry  to remove</param>
		/// <returns>If the function succeeds, the immediate return value is zero, if the function fails, the immediate return value is a nonzero</returns>
		[DllImport("rasapi32.dll", CharSet=CharSet.Auto)]
		public extern static uint RasDeleteEntry(string lpszPhonebook, string lpszEntry);
		#endregion
		#region public methods
		/// <summary>
		/// Check error code number and rise exception if needed
		/// </summary>
		/// <param name="errorCode">Error Code to check</param>
		public static void RasCheck(uint errorCode)
		{
			if(errorCode!=(uint)RasError.SUCCESS)
				throw new RasException(errorCode);
		}
		#endregion
	}

	/// <summary>
	/// Ras exception class
	/// </summary>
	public class RasException : Exception
	{
		private uint _errorCode;
			
		public uint ErrorCode{get{return this._errorCode;	}}

		public RasException(uint errorCode) : base(RasException.Code2RasErrorMessage(errorCode))
		{
			this._errorCode = errorCode;
		}

		public RasException(string errorDesc) : base(string.Format("RAS Error Code: {0} - RAS Error Description: {1}", 1999, errorDesc))
		{
			this._errorCode = 1999;
		}

		internal static string Code2RasErrorMessage(uint errorCode)
		{
			string ret;
			try
			{
				StringBuilder sb=new StringBuilder(512);
				if(Ras.RasGetErrorString(errorCode, sb, sb.Capacity)>0)
					throw new Exception("Unknow RAS exception.");
				ret=string.Format("RAS Error Code: {0} - RAS Error Description: {1}", errorCode, sb.ToString());
			}
			catch(Exception e)
			{
				ret=string.Format("Error Code: {0} - Error Description: {1}", errorCode, e.Message);
			}
			return ret;
		}
	}
}
