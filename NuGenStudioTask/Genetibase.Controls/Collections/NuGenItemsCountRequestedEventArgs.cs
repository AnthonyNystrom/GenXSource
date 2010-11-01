/* -----------------------------------------------
 * NuGenTreeNodeCountRequestedEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// The handler should set the value for the <see cref="P:Count"/> property.
	/// </summary>
	public class NuGenItemsCountRequestedEventArgs : EventArgs
	{
		#region Properties.Public

		private int count = 0;

		/// <summary>
		/// </summary>
		public int Count
		{
			get
			{
				return this.count;
			}
			set
			{
				this.count = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenItemsCountRequestedEventArgs"/> class.
		/// </summary>
		public NuGenItemsCountRequestedEventArgs()
		{
		}

		#endregion
	}
}
