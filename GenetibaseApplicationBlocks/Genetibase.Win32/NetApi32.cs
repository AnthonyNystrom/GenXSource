/* -----------------------------------------------
 * NetApi32.cs
 * Copyright © 2005-2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Runtime.InteropServices;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Imports NetApi32.dll functions.
	/// </summary>
	public static class NetApi32
	{
		#region Methods

		/// <summary>
		/// Frees the memory that the <c>NetApiBufferAllocate</c> function allocates.
		/// Call <c>NetApiBufferFree</c> to free the memory that other network management 
		/// functions return.
		/// </summary>
		/// <param name="buffer">Pointer to a buffer returned previously by another network 
		/// management function.</param>
		[DllImport("NetApi32.dll")]
		public static extern void NetApiBufferFree(IntPtr buffer);

		/// <summary>
		/// Lists all servers of the specified type that are visible in a domain. 
		/// </summary>
		/// <example>An application can call <c>NetServerEnum to list all domain controllers only or
		/// all SQL servers only.</c></example>
		/// <param name="serverName">Reserved; must be <c>System.IntPtr.Zero</c>.</param>
		/// <param name="level">Specifies the information level of the data. 
		/// This parameter can be one of the following values.
		/// <list type="table">
		/// <item><term>100</term>
		/// <description>Return server names and platform information. The <c>buffer</c> parameter 
		/// points to an array of <c>SERVER_INFO_100</c> structures.</description></item>
		/// <item><term>101</term>
		/// <description>Return server names, types, and associated software. The <c>buffer</c> 
		/// parameter points to an array of <c>SERVER_INFO_101</c> structures.</description></item>
		/// </list>
		/// </param>
		/// <param name="buffer">Pointer to the buffer that receives the data. The format of this data 
		/// depends on the value of the <c>level</c> parameter. This buffer is allocated by the system 
		/// and must be freed using the <c>NetApiBufferFree</c> function. Note that you must free the 
		/// buffer even if the function fails with <c>ERROR_MORE_DATA</c>.</param>
		/// <param name="prefMaxLen">Specifies the preferred maximum length of returned data, in bytes. 
		/// If you specify <c>MAX_PREFERRED_LENGTH</c>, the function allocates the amount of memory 
		/// required for the data. If you specify another value in this parameter, it can restrict 
		/// the number of bytes that the function returns. If the buffer size is insufficient to hold 
		/// all entries, the function returns <c>ERROR_MORE_DATA</c>.</param>
		/// <param name="entriesRead">Pointer to a value that receives the count of elements actually 
		/// enumerated.</param>
		/// <param name="totalEntries">Pointer to a value that receives the total number of visible servers 
		/// and workstations on the network. Note that applications should consider this value only as a hint.</param>
		/// <param name="servertype">Specifies a value that filters the server entries to return from the enumeration.</param>
		/// <param name="domain">Pointer to a constant string that specifies the name of the domain for 
		/// which a list of servers is to be returned. The domain name must be a NetBIOS domain 
		/// name (for example, microsoft). NetServerEnum does not support DNS-style names 
		/// (for example, microsoft.com). If this parameter is <c>null</c>, the primary domain is implied.</param>
		/// <param name="resumeHandle">Reserved; must be set to <c>System.IntPtr.Zero</c>.</param>
		/// <returns>
		/// If the function succeeds, the return value is <c>NERR_Success</c>. If the function fails, the 
		/// return value can be one of the following error codes.
		/// <list type="table">
		/// <item>
		/// <term>
		/// ERROR_NO_BROWSER_SERVERS_FOUND
		/// </term>
		/// <description>
		/// No browser servers found.
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// ERROR_MORE_DATA
		/// </term>
		/// <description>
		/// More entries are available. Specify a large enough buffer to receive all entries.
		/// </description>
		/// </item>
		/// </list>
		/// </returns>
		[DllImport("NetApi32.dll")]
		public static extern int NetServerEnum(
			IntPtr serverName,
			uint level,
			ref IntPtr buffer,
			uint prefMaxLen,
			ref uint entriesRead,
			ref uint totalEntries,
			uint servertype,	
			[MarshalAs(UnmanagedType.LPWStr)] string domain,
			IntPtr resumeHandle
			);

		/// <summary>
		/// Retrieves current configuration information for the specified server.
		/// </summary>
		/// <param name="serverName">Pointer to a string that specifies the name of the remote server 
		/// on which the function is to execute. If this parameter is <c>null</c>, the local computer 
		/// is used. In Windows NT this string must begin with \\.</param>
		/// <param name="level">Specifies the information level of the data. This parameter can be one of the following values. 
		/// <list type="table">
		/// <item>
		/// <term>
		/// 100
		/// </term>
		/// <description>
		/// Return the server name and platform information. The <c>buffer</c> parameter points to a 
		/// <c>SERVER_INFO_100</c> structure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// 101
		/// </term>
		/// <description>
		/// Return the server name, type, and associated software. The <c>buffer</c> parameter points to a 
		/// <c>SERVER_INFO_101</c> structure.
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// 102
		/// </term>
		/// <description>
		/// Return the server name, type, associated software, and other attributes. The <c>buffer</c> 
		/// parameter points to a <c>SERVER_INFO_102 structure</c>.
		/// </description>
		/// </item>
		/// </list>
		/// </param>
		/// <param name="buffer">Pointer to the buffer that receives the data. The format of this 
		/// data depends on the value of the level parameter. This buffer is allocated by the system 
		/// and must be freed using the <c>NetApiBufferFree</c> function.</param>
		/// <returns>If the function succeeds, the return value is <c>NERR_Success</c>. 
		/// If the function fails, the return value can be one of the following error codes. 
		/// <list type="table">
		/// <item>
		/// <term>
		/// ERROR_ACCESS_DENIED
		/// </term>
		/// <description>
		/// The user does not have access to the requested information.
		/// </description>
		/// </item>
		/// <item>
		/// <term>
		/// ERROR_INVALID_LEVEL
		/// </term>
		/// <description>
		/// The value specified for the level parameter is invalid.
		/// </description>
		/// </item>
		/// <term>
		/// ERROR_INVALID_PARAMETER
		/// </term>
		/// <description>
		/// The specified parameter is invalid.
		/// </description>
		/// <item>
		/// <term>
		/// ERROR_NOT_ENOUGH_MEMORY
		/// </term>
		/// <description>
		/// Insufficient memory is available.
		/// </description>
		/// </item>
		/// </list>
		/// </returns>
		[DllImport("NetApi32.dll")]		
		public static extern int NetServerGetInfo(
			[MarshalAs(UnmanagedType.LPWStr)] string serverName, 
			int level,
			ref IntPtr buffer
			);

		#endregion
	}
}
