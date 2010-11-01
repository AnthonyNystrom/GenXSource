/* -----------------------------------------------
 * NuGenPriorityTaskTreeNodeComparer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;

using System;
using System.Collections.Generic;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Critical Priority > Maybe Priority. Folder > Task.
	/// </summary>
	public class NuGenPriorityTaskTreeNodeComparer : IComparer<NuGenTaskTreeNodeBase>
	{
		#region IComparer<NuGenTaskTreeNodeBase> Members

		/// <summary>
		/// Compares the value of the <see cref="P:TaskPriority"/> property for <see cref="T:NuGenTaskTreeNode"/>
		/// objects. <see cref="T:NuGenFolderTreeNode"/> is greater than any <see cref="T:NuGenTaskTreeNode"/>.
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

			if (firstNodeToCompare is NuGenTaskTreeNode && secondNodeToCompare is NuGenTaskTreeNode)
			{
				int firstPriority = (int)((NuGenTaskTreeNode)firstNodeToCompare).TaskPriority;
				int secondPriority = (int)((NuGenTaskTreeNode)secondNodeToCompare).TaskPriority;

				if (firstPriority < secondPriority)
				{
					return -1;
				}

				if (firstPriority > secondPriority)
				{
					return 1;
				}
			}

			return 0;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPriorityTaskTreeNodeComparer"/> class.
		/// </summary>
		public NuGenPriorityTaskTreeNodeComparer()
		{

		}

		#endregion
	}
}
