/* -----------------------------------------------
 * NMHDR.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains information about a notification message. 
	/// </summary>
	public struct NMHDR
	{
		/// <summary>
		/// Window handle to the control sending a message.
		/// </summary>
		public IntPtr hwndFrom;

		/// <summary>
		/// Identifier of the control sending a message.
		/// </summary>
		public Int32 idFrom;

		/// <summary>
		/// Notification code. This member can be a control-specific notification code or it can be one of the common notification codes. 
		/// </summary>
		public Int32 code;
	}
}
