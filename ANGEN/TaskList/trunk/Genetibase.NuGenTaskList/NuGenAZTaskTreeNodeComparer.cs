/* -----------------------------------------------
 * NuGenAZTaskTreeNodeComparer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.Controls.Collections;

using System;
using System.Collections.Generic;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// A > Z. Folder > Task.
	/// </summary>
	public class NuGenAZTaskTreeNodeComparer : IComparer<NuGenTaskTreeNodeBase>
	{
		#region IComparer<NuGenTaskTreeNodeBase> Members

		/// <summary>
		/// Compares text of the specified <see cref="T:NuGenTreeNode"/> objects for A > Z sorting.
		/// <see cref="T:NuGenFolderTreeNode"/> is greater than any <see cref="T:NuGenTaskTreNode"/>.
		/// </summary>
		/// <param name="firstNodeToCompare">Can be <see langword="null"/>.</param>
		/// <param name="secondNodeToCompare">Can be <see langword="null"/>.</param>
		/// <returns></returns>
		public int Compare(NuGenTaskTreeNodeBase firstNodeToCompare, NuGenTaskTreeNodeBase secondNodeToCompare)
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

			if (firstNodeToCompare is NuGenFolderTreeNode && secondNodeToCompare is NuGenTaskTreeNode)
			{
				return -1;
			}

			if (firstNodeToCompare is NuGenTaskTreeNode && secondNodeToCompare is NuGenFolderTreeNode)
			{
				return 1;
			}

			return new NuGenAZTreeNodeComparer().Compare(firstNodeToCompare, secondNodeToCompare);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenAZTaskTreeNodeComparer"/> class.
		/// </summary>
		public NuGenAZTaskTreeNodeComparer()
		{

		}

		#endregion
	}
}
