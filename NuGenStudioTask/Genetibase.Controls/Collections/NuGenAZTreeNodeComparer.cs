/* -----------------------------------------------
 * NuGenAZTreeNodeComparer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections;
using System.Collections.Generic;

namespace Genetibase.Controls.Collections
{
	/// <summary>
	/// A > Z.
	/// </summary>
	public class NuGenAZTreeNodeComparer : IComparer<NuGenTreeNode>
	{
		#region IComparer<NuGenTreeNode> Members

		/// <summary>
		/// Compares text of the specified <see cref="T:NuGenTreeNode"/> objects for A > Z sorting.
		/// </summary>
		/// <param name="firstNodeToCompare">Can be <see langword="null"/>.</param>
		/// <param name="secondNodeToCompare">Can be <see langword="null"/>.</param>
		public int Compare(NuGenTreeNode firstNodeToCompare, NuGenTreeNode secondNodeToCompare)
		{
			if (firstNodeToCompare == secondNodeToCompare)
			{
				return 0;
			}

			if (firstNodeToCompare == null && secondNodeToCompare != null)
			{
				return -1;
			}

			if (firstNodeToCompare != null && secondNodeToCompare == null)
			{
				return 1;
			}

			if (firstNodeToCompare.Text == null && secondNodeToCompare.Text != null)
			{
				return -1;
			}

			if (firstNodeToCompare.Text != null && secondNodeToCompare.Text == null)
			{
				return 1;
			}

			return firstNodeToCompare.Text.CompareTo(secondNodeToCompare.Text);
		}

		#endregion
	}
}
