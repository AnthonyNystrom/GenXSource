/* -----------------------------------------------
 * WinError.cs
 * Copyright © 2005-2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Defines constants declared in WinError.h.
	/// </summary>
	public static class WinError
	{
		/// <summary>
		/// No browser servers found.
		/// </summary>
		public const Int32 ERROR_NO_BROWSER_SERVERS_FOUND = 6118;

		/// <summary>
		/// More data is available. 
		/// </summary>
		public const Int32 ERROR_MORE_DATA = 234;

		/// <summary>
		/// The user does not have access to the requested information.
		/// </summary>
		public const Int32 ERROR_ACCESS_DENIED = 5;

		/// <summary>
		/// The system call level is not correct.
		/// </summary>
		public const Int32 ERROR_INVALID_LEVEL = 124;

		/// <summary>
		/// The specified parameter is invalid.
		/// </summary>
		public const Int32 ERROR_INVALID_PARAMETER = 87;

		/// <summary>
		/// Insufficient memory is available.
		/// </summary>
		public const Int32 ERROR_NOT_ENOUGH_MEMORY = 8;
	}
}
