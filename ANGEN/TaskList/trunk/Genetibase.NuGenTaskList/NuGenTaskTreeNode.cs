/* -----------------------------------------------
 * NuGenTaskTreeNode.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Represents a user task within <see cref="T:NuGenTaskTreeView"/>. 
	/// </summary>
	public class NuGenTaskTreeNode : NuGenTaskTreeNodeBase
	{
		#region Properties.Protected.Virtual

		private Dictionary<NuGenTaskPriority, int> priorityImageIndexDictionary = null;

		/// <summary>
		/// Gets the dictionary containing image indecies associated with the current task priority.
		/// </summary>
		protected virtual Dictionary<NuGenTaskPriority, int> PriorityImageIndexDictionary
		{
			get
			{
				if (this.priorityImageIndexDictionary == null)
				{
					this.priorityImageIndexDictionary = new Dictionary<NuGenTaskPriority, int>();
				}

				return this.priorityImageIndexDictionary;
			}
		}

		#endregion

		#region Properties.Public

		/*
		 * Completed
		 */

		/// <summary>
		/// Gets or sets the value indicating whether the task is completed.
		/// </summary>
		public bool Completed
		{
			get
			{
				return this.Checked;
			}
			set
			{
				this.Checked = value;
			}
		}

		/*
		 * TaskPriority
		 */

		private NuGenTaskPriority taskPriority = NuGenTaskPriority.Wanted;

		/// <summary>
		/// </summary>
		public NuGenTaskPriority TaskPriority
		{
			get
			{
				return this.taskPriority;
			}
			set
			{
				this.taskPriority = value;
				this.DefaultImageIndex = this.GetPriorityImageIndex(value);
				this.ExpandedImageIndex = this.GetPriorityImageIndex(value);
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * GetPriorityImageIndex
		 */

		public int GetPriorityImageIndex(NuGenTaskPriority taskPriority)
		{
			if (this.PriorityImageIndexDictionary.ContainsKey(taskPriority))
			{
				return this.PriorityImageIndexDictionary[taskPriority];
			}

			return -1;
		}

		/*
		 * SetPriorityImageIndex
		 */

		public void SetPriorityImageIndex(NuGenTaskPriority taskPriority, int imageIndex)
		{
			if (this.PriorityImageIndexDictionary.ContainsKey(taskPriority))
			{
				this.PriorityImageIndexDictionary[taskPriority] = imageIndex;
			}
			else
			{
				this.PriorityImageIndexDictionary.Add(taskPriority, imageIndex);
			}

			if (this.TaskPriority == taskPriority)
			{
				this.DefaultImageIndex = imageIndex;
				this.ExpandedImageIndex = imageIndex;
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
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="nodeToLoadFrom"/> is <see langword="null"/>.
		/// </exception>
		public override void Load(XmlNode nodeToLoadFrom)
		{
			if (nodeToLoadFrom == null)
			{
				throw new ArgumentNullException("nodeToLoadFrom");
			}

			this.Completed = bool.Parse(
				this.XmlService.GetChildText(nodeToLoadFrom, Resources.XmlTag_Completed, bool.FalseString)
			);

			this.TaskPriority = (NuGenTaskPriority)Enum.Parse(
				typeof(NuGenTaskPriority),
				this.XmlService.GetChildText(nodeToLoadFrom, Resources.XmlTag_TaskPriority, NuGenTaskPriority.Wanted.ToString())
			);

			this.Text = this.XmlService.GetChildText(nodeToLoadFrom, Resources.XmlTag_Text, "");
		}

		/*
		 * Save
		 */

		/// <summary>
		/// </summary>
		/// <param name="nodeToSaveTo"></param>
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
				Resources.XmlTag_Completed,
				this.Completed.ToString()
			);

			this.XmlService.AppendChild(
				nodeToSaveTo,
				Resources.XmlTag_TaskPriority,
				this.TaskPriority.ToString()
			);

			this.XmlService.AppendChild(
				nodeToSaveTo,
				Resources.XmlTag_Text,
				this.Text
			);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskTreeNode"/> class.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public NuGenTaskTreeNode(
			INuGenServiceProvider serviceProvider,
			string taskText,
			NuGenTaskPriority taskPriority
			) : base(serviceProvider, taskText, -1, -1)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			this.TaskPriority = taskPriority;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskTreeNode"/> class. <para/>
		/// <c>TaskPriority = NuGenTaskPriority.Wanted</c>.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public NuGenTaskTreeNode(
			INuGenServiceProvider serviceProvider,
			string taskText
			) : this(serviceProvider, taskText, NuGenTaskPriority.Wanted)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskTreeNode"/> class.<para/>
		/// <c>Text = ""</c>. <c>TaskPriority = NuGenTaskPriority.Wanted</c>.
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public NuGenTaskTreeNode(
			INuGenServiceProvider serviceProvider
			) : this(serviceProvider, "")
		{
		}

		#endregion
	}
}
