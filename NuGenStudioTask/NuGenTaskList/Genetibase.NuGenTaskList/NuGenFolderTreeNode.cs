/* -----------------------------------------------
 * NuGenFolderTreeNode.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Xml;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Represents a folder with tasks within <see cref="T:NuGenTaskTreeView"/>.
	/// </summary>
	public class NuGenFolderTreeNode : NuGenTaskTreeNodeBase
	{
		#region Methods.Public

		/*
		 * Load
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="nodeToLoadFrom"/> is <see langword="null"/>.
		/// </exception>
		public override void Load(XmlNode nodeToLoadFrom)
		{
			if (nodeToLoadFrom == null)
			{
				throw new ArgumentNullException("nodeToLoadFrom");
			}

			this.Text = this.XmlService.GetChildText(
				nodeToLoadFrom,
				Resources.XmlTag_Text,
				""
			);

			this.Nodes.AddNode(new NuGenTreeNode());
			
			bool expanded = bool.Parse(
				this.XmlService.GetChildText(
					nodeToLoadFrom,
					Resources.XmlTag_Expanded,
					bool.FalseString
				)
			);

			if (expanded)
			{
				this.Expand();
			}

			this.Nodes.Clear();
		}

		/*
		 * Save
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="nodeToSaveTo"/> is <see langword="null"/>.
		/// </exception>
		public override void Save(XmlNode nodeToSaveTo)
		{
			if (nodeToSaveTo == null)
			{
				throw new ArgumentNullException("nodeToSaveTo");
			}

			this.XmlService.AppendChild(
				nodeToSaveTo,
				Resources.XmlTag_Text,
				this.Text
			);

			this.XmlService.AppendChild(
				nodeToSaveTo,
				Resources.XmlTag_Expanded,
				this.IsExpanded.ToString()
			);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFolderTreeNode"/> class.<para/>
		/// <c>HasCheckBox = false</c>. <c>Checked = false</c>.
		/// </summary>
		public NuGenFolderTreeNode(
			INuGenServiceProvider serviceProvider,
			string folderText,
			int imageIndex,
			int expandedImageIndex
			) : base(serviceProvider, folderText, imageIndex, expandedImageIndex)
		{
			this.HasCheckBox = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFolderTreeNode"/> class.<para/>
		/// <c>HasCheckBox = false</c>. <c>Checked = false</c>.
		/// </summary>
		public NuGenFolderTreeNode(
			INuGenServiceProvider serviceProvider,
			string folderText
			) : this(serviceProvider, folderText, -1, -1)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFolderTreeNode"/> class.<para/>
		/// <c>Text = ""</c>. <c>HasCheckBox = false</c>. <c>Checked = false</c>.
		/// </summary>
		public NuGenFolderTreeNode(
			INuGenServiceProvider serviceProvider
			)
			: this(serviceProvider, "")
		{
		}

		#endregion
	}
}
