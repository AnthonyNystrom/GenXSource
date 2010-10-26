/* -----------------------------------------------
 * KBDLLHOOKSTRUCT.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Contains information about a low-level keyboard input event.
	/// </summary>
	public struct KBDLLHOOKSTRUCT
	{
		/// <summary>
		/// Specifies a virtual-key code. The code must be a value in the range 1 to 254.
		/// </summary>
		public Int32 vkCode; 
		
		/// <summary>
		/// Specifies a hardware scan code for the key.
		/// </summary>
		public Int32 scanCode;
		
		/// <summary>
		/// <para>Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
		/// This member is specified as follows.</para>
		/// <para>See MSDN for more info.</para>
		/// </summary>
		public Int32 flags;
		
		/// <summary>
		/// Specifies the time stamp for this message, equivalent to what GetMessageTime would return for this message.
		/// </summary>
		public Int32 time;
		
		/// <summary>
		/// Specifies extra information associated with the message.
		/// </summary>
		public Int32 dwExtraInfo;
	}
}
