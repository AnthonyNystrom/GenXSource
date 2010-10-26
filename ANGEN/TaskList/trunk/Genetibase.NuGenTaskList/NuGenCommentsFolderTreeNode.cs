/* -----------------------------------------------
 * NuGenCommentsFolderTreeNode.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Represents a <see cref="NuGenFolderTreeNode"/> containing user code comments.
	/// </summary>
	public class NuGenCommentsFolderTreeNode : NuGenFolderTreeNode
	{
		#region Methods.Public.Overriden

		/*
		 * HasPriority
		 */

		/// <summary>
		/// Gets the value indicating whether a priority may be set for this task.
		/// </summary>
		/// <value></value>
		public override bool HasPriority
		{
			get
			{
				return false;
			}
		}

		/*
		 * IsDescriptionReadonly
		 */

		/// <summary>
		/// Gets the value indicating whether the task description is read-only.
		/// </summary>
		/// <value></value>
		public override bool IsDescriptionReadonly
		{
			get
			{
				return true;
			}
		}

		/*
		 * IsRemovable
		 */

		/// <summary>
		/// Gets the value indicating whether the task is removable from the owner task list.
		/// </summary>
		/// <value></value>
		public override bool IsRemovable
		{
			get
			{
				return false;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommentsFolderTreeNode"/> class.
		/// </summary>
		public NuGenCommentsFolderTreeNode(
			INuGenServiceProvider serviceProvider,
			string folderText
			) : base(serviceProvider, folderText)
		{
			this.Text = folderText;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCommentsFolderTreeNode"/> class.<para/>
		/// Text = "".
		/// </summary>
		public NuGenCommentsFolderTreeNode(
			INuGenServiceProvider serviceProvider
			)
			: this(serviceProvider, "")
		{
		}

		#endregion
	}
}
