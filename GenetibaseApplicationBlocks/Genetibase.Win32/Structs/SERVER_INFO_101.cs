/* -----------------------------------------------
 * SERVER_INFO_101.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains information about the specified server, including name, platform, type of server, 
	/// and associated software.
	/// </summary>
	public struct SERVER_INFO_101
	{
		/// <summary>
		/// Specifies the information level to use for platform-specific information. This member can 
		/// be one of the following values, defined in the lmcons.h file: 
		/// PLATFORM_ID_DOS, PLATFORM_ID_OS2, PLATFORM_ID_NT, PLATFORM_ID_OSF, or PLATFORM_ID_VMS.
		/// </summary>
		public int sv101_platform_id;

		/// <summary>
		/// Pointer to a Unicode string specifying the name of a server.
		/// </summary>
		public IntPtr sv101_name;

		/// <summary>
		/// Specifies, in the least significant 4 bits of the byte, the major release version number of the operating system. The most significant 4 bits of the byte specifies the server type. 
		/// The mask MAJOR_VERSION_MASK should be used to ensure correct results.
		/// </summary>
		public int sv101_version_major;

		/// <summary>
		/// Specifies the minor release version number of the operating system.
		/// </summary>
		public int sv101_version_minor;

		/// <summary>
		/// Specifies the type of software the computer is running.
		/// </summary>
		public int sv101_type;

		/// <summary>
		/// Pointer to a Unicode string specifying a comment describing the server.
		/// The comment can be <see langword="null"/>.
		/// </summary>
		public IntPtr sv101_comment;
	}
}
