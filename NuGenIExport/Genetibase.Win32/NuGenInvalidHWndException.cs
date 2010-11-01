/* -----------------------------------------------
 * NuGenInvalidHWndException.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi.Properties;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Genetibase.WinApi
{
	/// <summary>
	/// Thrown if the specified handle does not represent a window.
	/// </summary>
	[Serializable]
	public class NuGenInvalidHWndException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInvalidHWndException"/> class.
		/// </summary>
		public NuGenInvalidHWndException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInvalidHWndException"/> class.
		/// </summary>
		public NuGenInvalidHWndException(IntPtr invalidHWnd)
			: base(string.Format(Resources.Message_InvalidHWnd, invalidHWnd.ToInt32()))
		{
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInvalidHWndException"/> class.
		/// </summary>
		public NuGenInvalidHWndException(IntPtr invalidHWnd, Exception inner)
			: base(string.Format(Resources.Message_InvalidHWnd, invalidHWnd.ToInt32()), inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenInvalidHWndException"/> class.
		/// </summary>
		protected NuGenInvalidHWndException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
