/* -----------------------------------------------
 * NuGenCodeTaskTreeNode.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Windows.Forms;
using System.Xml;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Represents a task added from Code Editor.
	/// </summary>
	public class NuGenCodeTaskTreeNode : NuGenTaskTreeNodeBase
	{
		#region Properties.Public.Overriden

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

		#region Methods.Public.Overriden

		/*
		 * Load
		 */

		/// <summary>
		/// </summary>
		/// <param name="nodeToLoadFrom"></param>
		public override void Load(XmlNode nodeToLoadFrom)
		{
			return;
		}

		/*
		 * Save
		 */

		/// <summary>
		/// </summary>
		/// <param name="nodeToSaveTo"></param>
		public override void Save(XmlNode nodeToSaveTo)
		{
			return;
		}

		#endregion

		#region Methods.Protected.Overriden

		/// <summary>
		/// Retrieves the width of this <see cref="T:NuGenTaskTreeNodeBase"/> visible text area.
		/// </summary>
		/// <returns></returns>
		protected override int GetVisibleNodeTextMaxLength()
		{
			if (this.TreeView != null)
			{
				int offset = this.Level * this.TreeView.Indent
					+ SystemInformation.IconSize.Width
					+ SystemInformation.IconSize.Width / 2
					;

				return this.TreeView.ClientRectangle.Width - offset;
			}

			return 0;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCodeTaskTreeNode"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public NuGenCodeTaskTreeNode(
			INuGenServiceProvider serviceProvider,
			string codeTaskDescription,
			int imageIndex
			)
			: base(serviceProvider, "", imageIndex, imageIndex)
		{
			this.DefaultImageIndex = imageIndex;
			this.HasCheckBox = false;
			this.Text = codeTaskDescription;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCodeTaskTreeNode"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public NuGenCodeTaskTreeNode(
			INuGenServiceProvider serviceProvider,
			string codeTaskDescription
			)
			: this(serviceProvider, codeTaskDescription, -1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCodeTaskTreeNode"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public NuGenCodeTaskTreeNode(
			INuGenServiceProvider serviceProvider
			)
			: this(serviceProvider, "")
		{
		}

		#endregion
	}
}
