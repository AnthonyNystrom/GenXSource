/* -----------------------------------------------
 * LMServer.cs
 * Copyright © 2005-2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines constants declared in LMServer.h.
	/// </summary>
	public static class LMServer 
	{
		#region Declarations

		/// <summary>
		/// All workstations.
		/// </summary>
		public const uint SV_TYPE_WORKSTATION = 0x00000001;
		
		/// <summary>
		/// All computers that have the server service running.
		/// </summary>
		public const uint SV_TYPE_SERVER = 0x00000002;
		
		/// <summary>
		/// Any server running with Microsoft SQL Server.
		/// </summary>
		public const uint SV_TYPE_SQLSERVER = 0x00000004;
		
		/// <summary>
		/// Primary domain controller.
		/// </summary>
		public const uint SV_TYPE_DOMAIN_CTRL = 0x00000008;
		
		/// <summary>
		/// Backup domain controller.
		/// </summary>
		public const uint SV_TYPE_DOMAIN_BAKCTRL = 0x00000010;
		
		/// <summary>
		/// Server running the Timesource service.
		/// </summary>
		public const uint SV_TYPE_TIME_SOURCE = 0x00000020;
		
		/// <summary>
		/// Apple File Protocol servers.
		/// </summary>
		public const uint SV_TYPE_AFP = 0x00000040;
		
		/// <summary>
		/// Novell servers.
		/// </summary>
		public const uint SV_TYPE_NOVELL = 0x00000080;
		
		/// <summary>
		/// LAN Manager 2.x domain member.
		/// </summary>
		public const uint SV_TYPE_DOMAIN_MEMBER = 0x00000100;
		
		/// <summary>
		/// Server sharing print queue.
		/// </summary>
		public const uint SV_TYPE_PRINTQ_SERVER = 0x00000200;
		
		/// <summary>
		/// Server running dial-in service.
		/// </summary>
		public const uint SV_TYPE_DIALIN_SERVER = 0x00000400;
		
		/// <summary>
		/// Xenix server.
		/// </summary>
		public const uint SV_TYPE_XENIX_SERVER = 0x00000800;
		
		/// <summary>
		/// Unix server.
		/// </summary>
		public const uint SV_TYPE_SERVER_UNIX = SV_TYPE_XENIX_SERVER;
		
		/// <summary>
		/// Windows NT workstation or server.
		/// </summary>
		public const uint SV_TYPE_NT = 0x00001000;
		
		/// <summary>
		/// Server running Windows for Workgroups.
		/// </summary>
		public const uint SV_TYPE_WFW = 0x00002000;
		
		/// <summary>
		/// Microsoft File and Print for NetWare.
		/// </summary>
		public const uint SV_TYPE_SERVER_MFPN = 0x00004000;
		
		/// <summary>
		/// Server that is not a domain controller.
		/// </summary>
		public const uint SV_TYPE_SERVER_NT = 0x00008000;
		
		/// <summary>
		/// Server that can run the browser service.
		/// </summary>
		public const uint SV_TYPE_POTENTIAL_BROWSER = 0x00010000;
		
		/// <summary>
		/// Server running a browser service as backup.
		/// </summary>
		public const uint SV_TYPE_BACKUP_BROWSER = 0x00020000;
		
		/// <summary>
		/// Server running the master browser service.
		/// </summary>
		public const uint SV_TYPE_MASTER_BROWSER = 0x00040000;
		
		/// <summary>
		/// Server running the domain master browser.
		/// </summary>
		public const uint SV_TYPE_DOMAIN_MASTER = 0x00080000;
		
		/// <summary>
		/// No documentation available.
		/// </summary>
		public const uint SV_TYPE_SERVER_OSF = 0x00100000;
		
		/// <summary>
		/// No documentation available.
		/// </summary>
		public const uint SV_TYPE_SERVER_VMS = 0x00200000;
		
		/// <summary>
		/// Windows 95 or later.
		/// </summary>
		public const uint SV_TYPE_WINDOWS = 0x00400000;  /* Windows95 and above. */
		
		/// <summary>
		/// Root of a DFS tree.
		/// </summary>
		public const uint SV_TYPE_DFS = 0x00800000;  /* Root of a DFS tree. */
		
		/// <summary>
		/// Server clusters available in the domain.
		/// </summary>
		public const uint SV_TYPE_CLUSTER_NT = 0x01000000;  /* NT Cluster. */
		
		/// <summary>
		/// Terminal Server.
		/// </summary>
		public const uint SV_TYPE_TERMINALSERVER = 0x02000000;  /* Terminal Server(Hydra). */
		
		/// <summary>
		/// Cluster virtual servers available in the domain.
		/// </summary>
		/// <remarks>This value is not supported in Windows 2000/NT.</remarks>
		public const uint SV_TYPE_CLUSTER_VS_NT = 0x04000000;  /* NT Cluster Virtual Server Name. */
		
		/// <summary>
		/// IBM DSS (Directory and Security Services) or equivalent.
		/// </summary>
		public const uint SV_TYPE_DCE = 0x10000000;  /* IBM DSS (Directory and Security Services) or equivalent. */
		
		/// <summary>
		/// Return list for alternate transport.
		/// </summary>
		public const uint SV_TYPE_ALTERNATE_XPORT = 0x20000000;  /* Return list for alternate transport. */
		
		/// <summary>
		/// Servers maintained by the browser. See the following Remarks section.
		/// </summary>
		public const uint SV_TYPE_LOCAL_LIST_ONLY = 0x40000000;  /* Return local list only. */
		
		/// <summary>
		/// Primary domain.
		/// </summary>
		public const uint SV_TYPE_DOMAIN_ENUM = 0x80000000;
		
		/// <summary>
		/// All workstations.
		/// </summary>
		public const uint SV_TYPE_ALL = 0xFFFFFFFF; /* Handy for NetServerEnum2. */

		#endregion
	}
}
