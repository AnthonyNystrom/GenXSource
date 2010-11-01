using System.ComponentModel;

namespace Genetibase.Network.Ras
{
	/// <summary>
	/// RasErrors codes and description
	/// </summary>
	internal enum RasError:uint
	{
		[Description("Ras errors base.")]
		RASBASE =600,

		[Description("Successed.")]
		SUCCESS =0,

		[Description("An operation is pending.")]
		PENDING                              =RASBASE+0,

		[Description("An invalid port handle was detected.")]
		ERROR_INVALID_PORT_HANDLE            =RASBASE+1,

		[Description("The specified port is already open.")]
		ERROR_PORT_ALREADY_OPEN              =RASBASE+2,

		[Description("The caller's buffer is too small.")]
		ERROR_BUFFER_TOO_SMALL               =RASBASE+3,

		[Description("Incorrect information was specified.")]
		ERROR_WRONG_INFO_SPECIFIED           =RASBASE+4,

		[Description("The port information cannot be set.")]
		ERROR_CANNOT_SET_PORT_INFO           =RASBASE+5,

		[Description("The specified port is not connected.")]
		ERROR_PORT_NOT_CONNECTED             =RASBASE+6,

		[Description("An invalid event was detected.")]
		ERROR_EVENT_INVALID                  =RASBASE+7,

		[Description("A device was specified that does not exist.")]
		ERROR_DEVICE_DOES_NOT_EXIST          =RASBASE+8,

		[Description("A device type was specified that does not exist.")]
		ERROR_DEVICETYPE_DOES_NOT_EXIST      =RASBASE+9,

		[Description("An invalid buffer was specified.")]
		ERROR_BUFFER_INVALID                 =RASBASE+10,

		[Description("A route was specified that is not available.")]
		ERROR_ROUTE_NOT_AVAILABLE            =RASBASE+11,

		[Description("A route was specified that is not allocated.")]
		ERROR_ROUTE_NOT_ALLOCATED            =RASBASE+12,

		[Description("An invalid compression was specified.")]
		ERROR_INVALID_COMPRESSION_SPECIFIED  =RASBASE+13,

		[Description("There were insufficient buffers available.")]
		ERROR_OUT_OF_BUFFERS                 =RASBASE+14,

		[Description("The specified port was not found.")]
		ERROR_PORT_NOT_FOUND                 =RASBASE+15,

		[Description("An asynchronous request is pending.")]
		ERROR_ASYNC_REQUEST_PENDING          =RASBASE+16,

		[Description("The modem (or other connecting device), is already disconnecting.")]
		ERROR_ALREADY_DISCONNECTING          =RASBASE+17,

		[Description("The specified port is not open.")]
		ERROR_PORT_NOT_OPEN                  =RASBASE+18,

		[Description("A connection to the remote computer could not be established, so the port used for this connection was closed. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_PORT_DISCONNECTED              =RASBASE+19,

		[Description("No endpoints could be determined.")]
		ERROR_NO_ENDPOINTS                   =RASBASE+20,

		[Description("The system could not open the phone book file.")]
		ERROR_CANNOT_OPEN_PHONEBOOK          =RASBASE+21,

		[Description("The system could not load the phone book file.")]
		ERROR_CANNOT_LOAD_PHONEBOOK          =RASBASE+22,

		[Description("The system could not find the phone book entry for this connection.")]
		ERROR_CANNOT_FIND_PHONEBOOK_ENTRY    =RASBASE+23,

		[Description("The system could not update the phone book file.")]
		ERROR_CANNOT_WRITE_PHONEBOOK         =RASBASE+24,

		[Description("The system found invalid information in the phone book file.")]
		ERROR_CORRUPT_PHONEBOOK              =RASBASE+25,

		[Description("A string could not be loaded.")]
		ERROR_CANNOT_LOAD_STRING             =RASBASE+26,

		[Description("A key could not be found.")]
		ERROR_KEY_NOT_FOUND                  =RASBASE+27,

		[Description("The connection was terminated by the remote computer before it could be completed. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_DISCONNECTION                  =RASBASE+28,

		[Description("The connection was closed by the remote computer.")]
		ERROR_REMOTE_DISCONNECTION           =RASBASE+29,

		[Description("The modem (or other connecting device, was disconnected due to hardware failure.")]
		ERROR_HARDWARE_FAILURE               =RASBASE+30,

		[Description("The user disconnected the modem (or other connecting device,.")]
		ERROR_USER_DISCONNECTION             =RASBASE+31,

		[Description("An incorrect structure size was detected.")]
		ERROR_INVALID_SIZE                   =RASBASE+32,

		[Description("The modem (or other connecting device, is already in use or is not configured properly.%")]
		ERROR_PORT_NOT_AVAILABLE             =RASBASE+33,

		[Description("Your computer could not be registered on the remote network.")]
		ERROR_CANNOT_PROJECT_CLIENT          =RASBASE+34,

		[Description("There was an unknown error.")]
		ERROR_UNKNOWN                        =RASBASE+35,

		[Description("The device attached to the port is not the one expected.")]
		ERROR_WRONG_DEVICE_ATTACHED          =RASBASE+36,

		[Description("A string was detected that could not be converted.")]
		ERROR_BAD_STRING                     =RASBASE+37,

		[Description("The request has timed out.")]
		ERROR_REQUEST_TIMEOUT                =RASBASE+38,

		[Description("No asynchronous net is available.")]
		ERROR_CANNOT_GET_LANA                =RASBASE+39,

		[Description("An error has occurred involving NetBIOS.")]
		ERROR_NETBIOS_ERROR                  =RASBASE+40,

		[Description("The server cannot allocate NetBIOS resources needed to support the client.")]
		ERROR_SERVER_OUT_OF_RESOURCES        =RASBASE+41,

		[Description("One of your computer's NetBIOS names is already registered on the remote network.")]
		ERROR_NAME_EXISTS_ON_NET             =RASBASE+42,

		[Description("A network adapter at the server failed.")]
		ERROR_SERVER_GENERAL_NET_FAILURE     =RASBASE+43,

		[Description("You will not receive network message popups.")]
		WARNING_MSG_ALIAS_NOT_ADDED          =RASBASE+44,

		[Description("There was an internal authentication error.")]
		ERROR_AUTH_INTERNAL                  =RASBASE+45,

		[Description("The account is not permitted to log on at this time of day.")]
		ERROR_RESTRICTED_LOGON_HOURS         =RASBASE+46,

		[Description("The account is disabled.")]
		ERROR_ACCT_DISABLED                  =RASBASE+47,

		[Description("The password for this account has expired.")]
		ERROR_PASSWD_EXPIRED                 =RASBASE+48,

		[Description("The account does not have permission to dial in.")]
		ERROR_NO_DIALIN_PERMISSION           =RASBASE+49,

		[Description("The remote access server is not responding.")]
		ERROR_SERVER_NOT_RESPONDING          =RASBASE+50,

		[Description("The modem (or other connecting device, has reported an error.")]
		ERROR_FROM_DEVICE                    =RASBASE+51,

		[Description("There was an unrecognized response from the modem (or other connecting device,.")]
		ERROR_UNRECOGNIZED_RESPONSE          =RASBASE+52,

		[Description("A macro required by the modem (or other connecting device, was not found in the device.INF file.")]
		ERROR_MACRO_NOT_FOUND                =RASBASE+53,

		[Description("A command or response in the device.INF file section refers to an undefined macro.")]
		ERROR_MACRO_NOT_DEFINED              =RASBASE+54,

		[Description("The <message> macro was not found in the device.INF file section.")]
		ERROR_MESSAGE_MACRO_NOT_FOUND        =RASBASE+55,

		[Description("The <defaultoff> macro in the device.INF file section contains an undefined macro.")]
		ERROR_DEFAULTOFF_MACRO_NOT_FOUND     =RASBASE+56,

		[Description("The device.INF file could not be opened.")]
		ERROR_FILE_COULD_NOT_BE_OPENED       =RASBASE+57,

		[Description("The device name in the device.INF or media.INI file is too long.")]
		ERROR_DEVICENAME_TOO_LONG            =RASBASE+58,

		[Description("The media.INI file refers to an unknown device name.")]
		ERROR_DEVICENAME_NOT_FOUND           =RASBASE+59,

		[Description("The device.INF file contains no responses for the command.")]
		ERROR_NO_RESPONSES                   =RASBASE+60,

		[Description("The device.INF file is missing a command.")]
		ERROR_NO_COMMAND_FOUND               =RASBASE+61,

		[Description("There was an attempt to set a macro not listed in device.INF file section.")]
		ERROR_WRONG_KEY_SPECIFIED            =RASBASE+62,

		[Description("The media.INI file refers to an unknown device type.")]
		ERROR_UNKNOWN_DEVICE_TYPE            =RASBASE+63,

		[Description("The system has run out of memory.")]
		ERROR_ALLOCATING_MEMORY              =RASBASE+64,

		[Description("The modem (or other connecting device, is not properly configured.")]
		ERROR_PORT_NOT_CONFIGURED            =RASBASE+65,

		[Description("The modem (or other connecting device, is not functioning.")]
		ERROR_DEVICE_NOT_READY               =RASBASE+66,

		[Description("The system was unable to read the media.INI file.")]
		ERROR_READING_INI_FILE               =RASBASE+67,

		[Description("The connection was terminated.")]
		ERROR_NO_CONNECTION                  =RASBASE+68,

		[Description("The usage parameter in the media.INI file is invalid.")]
		ERROR_BAD_USAGE_IN_INI_FILE          =RASBASE+69,

		[Description("The system was unable to read the section name from the media.INI file.")]
		ERROR_READING_SECTIONNAME            =RASBASE+70,

		[Description("The system was unable to read the device type from the media.INI file.")]
		ERROR_READING_DEVICETYPE             =RASBASE+71,

		[Description("The system was unable to read the device name from the media.INI file.")]
		ERROR_READING_DEVICENAME             =RASBASE+72,

		[Description("The system was unable to read the usage from the media.INI file.")]
		ERROR_READING_USAGE                  =RASBASE+73,

		[Description("The system was unable to read the maximum connection BPS rate from the media.INI file.")]
		ERROR_READING_MAXCONNECTBPS          =RASBASE+74,

		[Description("The system was unable to read the maximum carrier connection speed from the media.INI file.")]
		ERROR_READING_MAXCARRIERBPS          =RASBASE+75,

		[Description("The phone line is busy.")]
		ERROR_LINE_BUSY                      =RASBASE+76,

		[Description("A person answered instead of a modem (or other connecting device,.")]
		ERROR_VOICE_ANSWER                   =RASBASE+77,

		[Description("The remote computer did not respond. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_NO_ANSWER                      =RASBASE+78,

		[Description("The system could not detect the carrier.")]
		ERROR_NO_CARRIER                     =RASBASE+79,

		[Description("There was no dial tone.")]
		ERROR_NO_DIALTONE                    =RASBASE+80,

		[Description("The modem (or other connecting device, reported a general error.")]
		ERROR_IN_COMMAND                     =RASBASE+81,

		[Description("There was an error in writing the section name.")]
		ERROR_WRITING_SECTIONNAME            =RASBASE+82,

		[Description("There was an error in writing the device type.")]
		ERROR_WRITING_DEVICETYPE             =RASBASE+83,

		[Description("There was an error in writing the device name.")]
		ERROR_WRITING_DEVICENAME             =RASBASE+84,

		[Description("There was an error in writing the maximum connection speed.")]
		ERROR_WRITING_MAXCONNECTBPS          =RASBASE+85,

		[Description("There was an error in writing the maximum carrier speed.")]
		ERROR_WRITING_MAXCARRIERBPS          =RASBASE+86,

		[Description("There was an error in writing the usage.")]
		ERROR_WRITING_USAGE                  =RASBASE+87,

		[Description(" There was an error in writing the default-off.")]
		ERROR_WRITING_DEFAULTOFF             =RASBASE+88,

		[Description(" There was an error in reading the default-off.")]
		ERROR_READING_DEFAULTOFF             =RASBASE+89,

		[Description("ERROR_EMPTY_INI_FILE")]
		ERROR_EMPTY_INI_FILE                 =RASBASE+90,

		[Description("Access was denied because the username and/or password was invalid on the domain.")]
		ERROR_AUTHENTICATION_FAILURE         =RASBASE+91,

		[Description("There was a hardware failure in the modem (or other connecting device,.")]
		ERROR_PORT_OR_DEVICE                 =RASBASE+92,

		[Description("ERROR_NOT_BINARY_MACRO")]
		ERROR_NOT_BINARY_MACRO               =RASBASE+93,

		[Description("ERROR_DCB_NOT_FOUND")]
		ERROR_DCB_NOT_FOUND                  =RASBASE+94,

		[Description("The state machines are not started.")]
		ERROR_STATE_MACHINES_NOT_STARTED     =RASBASE+95,

		[Description("The state machines are already started.")]
		ERROR_STATE_MACHINES_ALREADY_STARTED =RASBASE+96,

		[Description("The response looping did not complete.")]
		ERROR_PARTIAL_RESPONSE_LOOPING       =RASBASE+97,

		[Description("A response keyname in the device.INF file is not in the expected format.")]
		ERROR_UNKNOWN_RESPONSE_KEY           =RASBASE+98,

		[Description("The modem (or other connecting device, response caused a buffer overflow.")]
		ERROR_RECV_BUF_FULL                  =RASBASE+99,

		[Description("The expanded command in the device.INF file is too long.")]
		ERROR_CMD_TOO_LONG                   =RASBASE+100,

		[Description("The modem moved to a connection speed not supported by the COM driver.")]
		ERROR_UNSUPPORTED_BPS                =RASBASE+101,

		[Description("Device response received when none expected.")]
		ERROR_UNEXPECTED_RESPONSE            =RASBASE+102,

		[Description("The connection needs information from you, but the application does not allow user interaction.")]
		ERROR_INTERACTIVE_MODE               =RASBASE+103,

		[Description("The callback number is invalid.")]
		ERROR_BAD_CALLBACK_NUMBER            =RASBASE+104,

		[Description("The authorization state is invalid.")]
		ERROR_INVALID_AUTH_STATE             =RASBASE+105,

		[Description("ERROR_WRITING_INITBPS")]
		ERROR_WRITING_INITBPS                =RASBASE+106,

		[Description("There was an error related to the X.25 protocol.")]
		ERROR_X25_DIAGNOSTIC                 =RASBASE+107,

		[Description("The account has expired.")]
		ERROR_ACCT_EXPIRED                   =RASBASE+108,

		[Description("There was an error changing the password on the domain.  The password might have been too short or might have matched a previously used password.")]
		ERROR_CHANGING_PASSWORD              =RASBASE+109,

		[Description("Serial overrun errors were detected while communicating with the modem.")]
		ERROR_OVERRUN                        =RASBASE+110,

		[Description("A configuration error on this computer is preventing this connection. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_RASMAN_CANNOT_INITIALIZE	     =RASBASE+111,

		[Description("The two-way port is initializing.  Wait a few seconds and redial.")]
		ERROR_BIPLEX_PORT_NOT_AVAILABLE      =RASBASE+112,

		[Description("No active ISDN lines are available.")]
		ERROR_NO_ACTIVE_ISDN_LINES           =RASBASE+113,

		[Description("No ISDN channels are available to make the call.")]
		ERROR_NO_ISDN_CHANNELS_AVAILABLE     =RASBASE+114,

		[Description("Too many errors occurred because of poor phone line quality.")]
		ERROR_TOO_MANY_LINE_ERRORS           =RASBASE+115,

		[Description("The Remote Access Service IP configuration is unusable.")]
		ERROR_IP_CONFIGURATION               =RASBASE+116,

		[Description("No IP addresses are available in the static pool of Remote Access Service IP addresses.")]
		ERROR_NO_IP_ADDRESSES                =RASBASE+117,

		[Description("The connection was terminated because the remote computer did not respond in a timely manner. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_PPP_TIMEOUT                    =RASBASE+118,

		[Description("The connection was terminated by the remote computer.")]
		ERROR_PPP_REMOTE_TERMINATED          =RASBASE+119,

		[Description("A connection to the remote computer could not be established. You might need to change the network settings for this connection. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_PPP_NO_PROTOCOLS_CONFIGURED    =RASBASE+120,

		[Description("The remote computer did not respond. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_PPP_NO_RESPONSE                =RASBASE+121,

		[Description("Invalid data was received from the remote computer. This data was ignored.")]
		ERROR_PPP_INVALID_PACKET             =RASBASE+122,

		[Description("The phone number, including prefix and suffix, is too long.")]
		ERROR_PHONE_NUMBER_TOO_LONG          =RASBASE+123,

		[Description("The IPX protocol cannot dial out on the modem (or other connecting device, because this computer is not configured for dialing out (it is an IPX router,.")]
		ERROR_IPXCP_NO_DIALOUT_CONFIGURED    =RASBASE+124,

		[Description("The IPX protocol cannot dial in on the modem (or other connecting device, because this computer is not configured for dialing in (the IPX router is not installed,.")]
		ERROR_IPXCP_NO_DIALIN_CONFIGURED     =RASBASE+125,

		[Description("The IPX protocol cannot be used for dialing out on more than one modem (or other connecting device, at a time.")]
		ERROR_IPXCP_DIALOUT_ALREADY_ACTIVE   =RASBASE+126,

		[Description("Cannot access TCPCFG.DLL.")]
		ERROR_ACCESSING_TCPCFGDLL            =RASBASE+127,

		[Description("The system cannot find an IP adapter.")]
		ERROR_NO_IP_RAS_ADAPTER              =RASBASE+128,

		[Description("SLIP cannot be used unless the IP protocol is installed.")]
		ERROR_SLIP_REQUIRES_IP               =RASBASE+129,

		[Description("Computer registration is not complete.")]
		ERROR_PROJECTION_NOT_COMPLETE        =RASBASE+130,

		[Description("The protocol is not configured.")]
		ERROR_PROTOCOL_NOT_CONFIGURED        =RASBASE+131,

		[Description("Your computer and the remote computer could not agree on PPP control protocols.")]
		ERROR_PPP_NOT_CONVERGING             =RASBASE+132,

		[Description("A connection to the remote computer could not be completed. You might need to adjust the protocols on this computer. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_PPP_CP_REJECTED                =RASBASE+133,

		[Description("The PPP link control protocol was terminated.")]
		ERROR_PPP_LCP_TERMINATED             =RASBASE+134,

		[Description("The requested address was rejected by the server.")]
		ERROR_PPP_REQUIRED_ADDRESS_REJECTED  =RASBASE+135,

		[Description("The remote computer terminated the control protocol.")]
		ERROR_PPP_NCP_TERMINATED             =RASBASE+136,

		[Description("Loopback was detected.")]
		ERROR_PPP_LOOPBACK_DETECTED          =RASBASE+137,

		[Description("The server did not assign an address.")]
		ERROR_PPP_NO_ADDRESS_ASSIGNED        =RASBASE+138,

		[Description("The authentication protocol required by the remote server cannot use the stored password.  Redial, entering the password explicitly.")]
		ERROR_CANNOT_USE_LOGON_CREDENTIALS   =RASBASE+139,

		[Description("An invalid dialing rule was detected.")]
		ERROR_TAPI_CONFIGURATION             =RASBASE+140,

		[Description("The local computer does not support the required data encryption type.")]
		ERROR_NO_LOCAL_ENCRYPTION            =RASBASE+141,

		[Description("The remote computer does not support the required data encryption type.")]
		ERROR_NO_REMOTE_ENCRYPTION           =RASBASE+142,

		[Description("The remote computer requires data encryption.")]
		ERROR_REMOTE_REQUIRES_ENCRYPTION     =RASBASE+143,

		[Description("The system cannot use the IPX network number assigned by the remote computer.  Additional information is provided in the event log.")]
		ERROR_IPXCP_NET_NUMBER_CONFLICT      =RASBASE+144,

		[Description("ERROR_INVALID_SMM")]
		ERROR_INVALID_SMM                    =RASBASE+145,

		[Description("ERROR_SMM_UNINITIALIZED")]
		ERROR_SMM_UNINITIALIZED              =RASBASE+146,

		[Description("ERROR_NO_MAC_FOR_PORT")]
		ERROR_NO_MAC_FOR_PORT                =RASBASE+147,

		[Description("ERROR_SMM_TIMEOUT")]
		ERROR_SMM_TIMEOUT                    =RASBASE+148,

		[Description("ERROR_BAD_PHONE_NUMBER")]
		ERROR_BAD_PHONE_NUMBER               =RASBASE+149,

		[Description("ERROR_WRONG_MODULE")]
		ERROR_WRONG_MODULE                   =RASBASE+150,

		[Description("The callback number contains an invalid character.  Only the following 18 characters are allowed:  0 to 9, T, P, W, (, ,, -, @, and space.")]
		ERROR_INVALID_CALLBACK_NUMBER        =RASBASE+151,

		[Description("A syntax error was encountered while processing a script.")]
		ERROR_SCRIPT_SYNTAX                  =RASBASE+152,

		[Description("The connection could not be disconnected because it was created by the multi-protocol router.")]
		ERROR_HANGUP_FAILED                  =RASBASE+153,

		[Description("The system could not find the multi-link bundle.")]
		ERROR_BUNDLE_NOT_FOUND               =RASBASE+154,

		[Description("The system cannot perform automated dial because this connection  has a custom dialer specified.")]
		ERROR_CANNOT_DO_CUSTOMDIAL           =RASBASE+155,

		[Description("This connection is already being dialed.")]
		ERROR_DIAL_ALREADY_IN_PROGRESS      =RASBASE+156,

		[Description("Remote Access Services could not be started automatically. Additional information is provided in the event log.")]
		ERROR_RASAUTO_CANNOT_INITIALIZE	    =RASBASE+157,

		[Description("Internet Connection Sharing is already enabled on the connection.")]
		ERROR_CONNECTION_ALREADY_SHARED     =RASBASE+158,

		[Description("An error occurred while the existing Internet Connection Sharisettings were being changed.")]
		ERROR_SHARING_CHANGE_FAILED         =RASBASE+159,

		[Description("An error occurred while routing capabilities were being enabled.")]
		ERROR_SHARING_ROUTER_INSTALL        =RASBASE+160,

		[Description("An error occurred while Internet Connection Sharing was being enable for the connection.")]
		ERROR_SHARE_CONNECTION_FAILED       =RASBASE+161,

		[Description("An error occurred while the local network was being configured for sharing.")]
		ERROR_SHARING_PRIVATE_INSTALL       =RASBASE+162,

		[Description("Internet Connection Sharing cannot be enabled.\nThere is more than one LAN connection other than the connection\nto be shared.")]
		ERROR_CANNOT_SHARE_CONNECTION       =RASBASE+163,

		[Description("No smart card reader is installed.")]
		ERROR_NO_SMART_CARD_READER          =RASBASE+164,

		[Description("Internet Connection Sharing cannot be enabled.\nA LAN connection is already configured with the IP address\nthat is required for automatic IP addressing.")]
		ERROR_SHARING_ADDRESS_EXISTS        =RASBASE+165,

		[Description("A certificate could not be found. Connections that use the L2TP protocol over IPSec require the installation of a machine certificate, also known as a computer certificate.")]
		ERROR_NO_CERTIFICATE                =RASBASE+166,

		[Description("Internet Connection Sharing cannot be enabled.The LAN connection selected\nas the private network has more than one IP address configured.\nPlease reconfigure the LAN connection with a single IP address\nbefore enabling Internet Connection Sharing.")]
		ERROR_SHARING_MULTIPLE_ADDRESSES    =RASBASE+167,

		[Description("The connection attempt failed because of failure to encrypt data.")]
		ERROR_FAILED_TO_ENCRYPT             =RASBASE+168,

		[Description("The specified destination is not reachable.")]
		ERROR_BAD_ADDRESS_SPECIFIED         =RASBASE+169,

		[Description("The remote computer rejected the connection attempt.")]
		ERROR_CONNECTION_REJECT             =RASBASE+170,

		[Description("The connection attempt failed because the network is busy.")]
		ERROR_CONGESTION                    =RASBASE+171,

		[Description("The remote computer's network hardware is incompatible with the type of call requested.")]
		ERROR_INCOMPATIBLE                  =RASBASE+172,

		[Description("The connection attempt failed because the destination number has changed.")]
		ERROR_NUMBERCHANGED                 =RASBASE+173,

		[Description("The connection attempt failed because of a temporary failure.  Try connecting again.")]
		ERROR_TEMPFAILURE                   =RASBASE+174,

		[Description("The call was blocked by the remote computer.")]
		ERROR_BLOCKED                       =RASBASE+175,

		[Description("The call could not be connected because the remote computer has invoked the Do Not Disturb feature.")]
		ERROR_DONOTDISTURB                  =RASBASE+176,

		[Description("The connection attempt failed because the modem (or other connecting device, on the remote computer is out of order.")]
		ERROR_OUTOFORDER                    =RASBASE+177,

		[Description("It was not possible to verify the identity of the server.")]
		ERROR_UNABLE_TO_AUTHENTICATE_SERVER =RASBASE+178,

		[Description("To dial out using this connection you must use a smart card.")]
		ERROR_SMART_CARD_REQUIRED           =RASBASE+179,

		[Description("An attempted function is not valid for this connection.")]
		ERROR_INVALID_FUNCTION_FOR_ENTRY    =RASBASE+180,

		[Description("The connection requires a certificate, and no valid certificate was found. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_CERT_FOR_ENCRYPTION_NOT_FOUND =RASBASE+181,

		[Description("Network Address Translation (NAT, is currently installed as a routing protocol, and must be \nremoved before enabling Internet Connection Sharing and/or Internet Connection Firewall.")]
		ERROR_SHARING_RRAS_CONFLICT         =RASBASE+182,

		[Description("Internet Connection Sharing cannot be enabled. The LAN connection selected\nas the private network is either not present, or is disconnected\nfrom the network. Please ensure that the LAN adapter is connected\nbefore enabling Internet Connection Sharing.")]
		ERROR_SHARING_NO_PRIVATE_LAN        =RASBASE+183,

		[Description("You cannot dial using this connection at logon time, because it is\nconfigured to use a user name different than the one on the smart\ncard. If you want to use it at logon time, you must configure it to\nuse the user name on the smart card.")]
		ERROR_NO_DIFF_USER_AT_LOGON         =RASBASE+184,

		[Description("You cannot dial using this connection at logon time, because it is\nnot configured to use a smart card. If you want to use it at logon\ntime, you must edit the properties of this connection so that it uses\na smart card.")]
		ERROR_NO_REG_CERT_AT_LOGON          =RASBASE+185,

		[Description("The L2TP connection attempt failed because there is no valid machine certificate on your computer for security authentication.")]
		ERROR_OAKLEY_NO_CERT                =RASBASE+186,

		[Description("The L2TP connection attempt failed because the security layer could not authenticate the remote computer.")]
		ERROR_OAKLEY_AUTH_FAIL              =RASBASE+187,

		[Description("The L2TP connection attempt failed because the security layer could not negotiate compatible parameters with the remote computer.")]
		ERROR_OAKLEY_ATTRIB_FAIL            =RASBASE+188,

		[Description("The L2TP connection attempt failed because the security layer encountered a processing error during initial negotiations with the remote computer.")]
		ERROR_OAKLEY_GENERAL_PROCESSING     =RASBASE+189,

		[Description("The L2TP connection attempt failed because certificate validation on the remote computer failed.")]
		ERROR_OAKLEY_NO_PEER_CERT           =RASBASE+190,

		[Description("The L2TP connection attempt failed because security policy for the connection was not found.")]
		ERROR_OAKLEY_NO_POLICY              =RASBASE+191,

		[Description("The L2TP connection attempt failed because security negotiation timed out.")]
		ERROR_OAKLEY_TIMED_OUT              =RASBASE+192,

		[Description("The L2TP connection attempt failed because an error occurred while negotiating security.")]
		ERROR_OAKLEY_ERROR                  =RASBASE+193,

		[Description("The Framed Protocol RADIUS attribute for this user is not PPP.")]
		ERROR_UNKNOWN_FRAMED_PROTOCOL       =RASBASE+194,

		[Description("The Tunnel Type RADIUS attribute for this user is not correct.")]
		ERROR_WRONG_TUNNEL_TYPE             =RASBASE+195,

		[Description("The Service Type RADIUS attribute for this user is neither Framed nor Callback Framed.")]
		ERROR_UNKNOWN_SERVICE_TYPE          =RASBASE+196,

		[Description("A connection to the remote computer could not be established because the modem was not found or was busy. For further assistance, click More Info or search Help and Support Center for this error number.")]
		ERROR_CONNECTING_DEVICE_NOT_FOUND   =RASBASE+197,

		[Description("A certificate could not be found that can be used with this Extensible Authentication Protocol.")]
		ERROR_NO_EAPTLS_CERTIFICATE          =RASBASE+198,

		[Description("Internet Connection Sharing (ICS) cannot be enabled due to an IP \naddress conflict on the network. ICS requires the host be configured to use \n192.168.0.1. Please ensure that no other client on the network is\nconfigured to use 192.168.0.1.")]
		ERROR_SHARING_HOST_ADDRESS_CONFLICT =RASBASE+199,

		[Description("Unable to establish the VPN connection.  The VPN server may be unreachable, or security parameters may not be configured properly for this connection.")]
		ERROR_AUTOMATIC_VPN_FAILED          =RASBASE+200,

		[Description("This connection is configured to validate the identity of the access server, but Windows cannot verify the digital certificate sent by the server")]
		ERROR_VALIDATING_SERVER_CERT        =RASBASE+201,

		[Description("Ras baseend.")]
		RASBASEEND                          =RASBASE+201,

		ERROR_INVALID_NAME	=0x123,
		ERROR_ALREADY_EXISTS=0x183
	}
}
