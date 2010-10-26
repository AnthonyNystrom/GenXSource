/* -----------------------------------------------
 * NuGenEnumeratorRequestEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// The handler should set the value for the <see cref="T:RequestedEnumerator"/> property.
	/// </summary>
	public class NuGenEnumeratorRequestedEventArgs : EventArgs
	{
		#region Properties.Public

		private IEnumerator requestedEnumerator = null;

		/// <summary>
		/// </summary>
		public IEnumerator RequestedEnumerator
		{
			get
			{
				return this.requestedEnumerator;
			}
			set
			{
				this.requestedEnumerator = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEnumeratorRequestedEventArgs"/> class.
		/// </summary>
		public NuGenEnumeratorRequestedEventArgs()
		{
		}

		#endregion
	}
}
