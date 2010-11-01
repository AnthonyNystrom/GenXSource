/* -----------------------------------------------
 * NuGenCompletedTaskTreeNodeComparer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;

using System;
using System.Collections.Generic;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Completed Task > Uncompleted Task. Folder > Task.
	/// </summary>
	public class NuGenCompletedTaskTreeNodeComparer : IComparer<NuGenTaskTreeNodeBase>
	{
		#region IComparer<NuGenTaskTreeNodeBase> Members

		/// <summary>
		/// Compares the value of the <see cref="P:Completed"/> property for <see cref="T:NuGenTaskTreeNode"/>
		/// objects. <see cref="T:NuGenFolderTreeNode"/> is greater than any <see cref="T:NuGenTaskTreeNode"/>.
		/// </summary>
		/// <param name="firstNodeToCompare">Can be <see langword="null"/>.</param>
		/// <param name="secondNodeToCompare">Can be <see langword="null"/>.</param>
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

			if (firstNodeToCompare is NuGenTaskTreeNode && secondNodeToCompare is NuGenTaskTreeNode)
			{
				if (
					((NuGenTaskTreeNode)firstNodeToCompare).Completed
					&& !((NuGenTaskTreeNode)secondNodeToCompare).Completed
					)
				{
					return 1;
				}

				if (
					!((NuGenTaskTreeNode)firstNodeToCompare).Completed
					&& ((NuGenTaskTreeNode)secondNodeToCompare).Completed
					)
				{
					return -1;
				}
			}

			return 0;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCompletedTaskTreeNodeComparer"/> class.
		/// </summary>
		public NuGenCompletedTaskTreeNodeComparer()
		{
				
		}

		#endregion
	}
}
