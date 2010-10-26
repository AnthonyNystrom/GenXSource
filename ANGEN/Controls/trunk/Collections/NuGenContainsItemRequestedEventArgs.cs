/* -----------------------------------------------
 * NuGenContainsItemRequestedEventArgs.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// The handler should set the value for the <see cref="P:ContainsNode"/> property indicating whether
	/// the specified <see cref="P:NodeToCheck"/> contains within the collection.
	/// </summary>
	public class NuGenContainsItemRequestedEventArgs : EventArgs
	{
		#region Properties.Public

		/*
		 * ContainsNode
		 */

		private bool containsNode = false;

		/// <summary>
		/// </summary>
		public bool ContainsNode
		{
			get
			{
				return this.containsNode;
			}
			set
			{
				this.containsNode = value;
			}
		}

		/*
		 * NodeToCheck
		 */

		private NuGenTreeNode nodeToCheck = null;

		/// <summary>
		/// </summary>
		public NuGenTreeNode NodeToCheck
		{
			get
			{
				return this.nodeToCheck;
			}
			protected set
			{
				this.nodeToCheck = value;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenContainsItemRequestedEventArgs"/> class.
		/// </summary>
		public NuGenContainsItemRequestedEventArgs(NuGenTreeNode nodeToCheck)
		{
			this.nodeToCheck = nodeToCheck;
		}

		#endregion
	}
}
