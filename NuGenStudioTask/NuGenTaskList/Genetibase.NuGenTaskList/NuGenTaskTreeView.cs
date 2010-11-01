/* -----------------------------------------------
 * NuGenTaskTreeView.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using EnvDTE;

using Genetibase.Controls;
using Genetibase.Controls.Collections;
using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Xml;
using Genetibase.WinApi;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace Genetibase.NuGenTaskList
{
	/// <summary>
	/// Represents tasks in a multilevel form.
	/// </summary>
	public class NuGenTaskTreeView : NuGenTreeView, INuGenDEHClient
	{
		#region Declaratioins.Consts

		private const string FOLDER = "Folder";
		private const string FOLDER_OPEN = "Folder_Open";
		private const string PRIORITY_CRITICAL = "Priority_Critical";
		private const string PRIORITY_REQUIRED = "Priority_Required";
		private const string PRIORITY_WANTED = "Priority_Wanted";
		private const string PRIORITY_WOULDBENICE = "Priority_WouldBeNice";
		private const string PRIORITY_MAYBE = "Priority_MayBe";

		#endregion

		#region Declarations.Fields

		private IContainer _Components = null;
		private ImageList _TaskImageList = null;
		private ContextMenuStrip _ContextMenu = null;

		#endregion

		#region INuGenDEHClient Members

		private static readonly object _EventToBeDelayed = new object();

		/// <summary>
		/// </summary>
		public event NuGenDEHEventHandler EventToBeDelayed
		{
			add
			{
				this.Events.AddHandler(_EventToBeDelayed, value);
			}
			remove
			{
				this.Events.RemoveHandler(_EventToBeDelayed, value);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnEventToBeDelayed(INuGenDEHEventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeEventToBeDelayed(_EventToBeDelayed, e);
		}

		/// <summary>
		/// </summary>
		public void HandleDelayedEvent(object sender, INuGenDEHEventArgs e)
		{
			if (e is NuGenSelectedTaskChangedEventArgs)
			{
				NuGenSelectedTaskChangedEventArgs eventArgs = (NuGenSelectedTaskChangedEventArgs)e;
				string treeNodeText = eventArgs.TaskText != null ? eventArgs.TaskText : "";

				if (this.IsHandleCreated)
				{
					this.BeginInvoke(
						new MethodInvoker(
							delegate
							{
								if (
									this.SelectedNode != null
									/* Otherwise, not appropriate nodes change its text when multiselected. */
									&& this.SelectedNodes.Count < 2
									)
								{
									if (this.SelectedNode is NuGenTaskTreeNodeBase)
									{
										((NuGenTaskTreeNodeBase)this.SelectedNode).Text = treeNodeText;
									}
									else
									{
										this.SelectedNode.Text = treeNodeText;
									}
								}
							}
						)
					);
				}
			}
		}

		#endregion

		#region Properties.Protected

		/*
		 * CodeTasks
		 */

		private Dictionary<NuGenTreeNode, TaskItem> _CodeTasks = null;

		/// <summary>
		/// </summary>
		protected Dictionary<NuGenTreeNode, TaskItem> CodeTasks
		{
			get
			{
				if (_CodeTasks == null)
				{
					_CodeTasks = new Dictionary<NuGenTreeNode, TaskItem>();
				}

				return _CodeTasks;
			}
		}

		/*
		 * CommentsFolder
		 */

		private NuGenCommentsFolderTreeNode _CommentsFolder = null;

		/// <summary>
		/// Gets the folder containing code tasks.
		/// </summary>
		protected NuGenCommentsFolderTreeNode CommentsFolder
		{
			get
			{
				return _CommentsFolder;
			}
		}

		/*
		 * ImageListService
		 */

		private INuGenImageListService _ImageListService = null;

		/// <summary>
		/// </summary>
		protected INuGenImageListService ImageListService
		{
			get
			{
				if (_ImageListService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_ImageListService = this.ServiceProvider.GetService<INuGenImageListService>();

					if (_ImageListService == null)
					{
						throw new InvalidOperationException(
							string.Format(Resources.InvalidOperation_NotServiceExist, typeof(INuGenImageListService).ToString())
						);
					}
				}

				return _ImageListService;
			}
		}

		/*
		 * XmlService
		 */

		private INuGenTaskXmlService _XmlService = null;

		/// <summary>
		/// </summary>
		protected INuGenTaskXmlService XmlService
		{
			get
			{
				if (_XmlService == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_XmlService = this.ServiceProvider.GetService<INuGenTaskXmlService>();

					if (_XmlService == null)
					{
						throw new InvalidOperationException(
							string.Format(Resources.InvalidOperation_NotServiceExist, typeof(INuGenTaskXmlService).ToString())
						);
					}
				}

				return _XmlService;
			}
		}

		#endregion

		#region Properties.Protected.Virtual
		
		/*
		 * Initiator
		 */

		private NuGenDEHEventInitiator _Initiator = null;

		/// <summary>
		/// </summary>
		protected virtual NuGenDEHEventInitiator Initiator
		{
			get
			{
				if (_Initiator == null)
				{
					_Initiator = new NuGenDEHEventInitiator(this, this.Events);
				}

				return _Initiator;
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		/*
		 * DefaultSize
		 */

		/// <summary>
		/// Gets the default size for this <see cref="T:NuGenTaskTreeView"/>.
		/// </summary>
		protected override Size DefaultSize
		{
			get
			{
				return new Size(100, 100);
			}
		}

		#endregion

		#region Methods.Public.AddFolder

		/*
		 * AddFolder
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="folderNodeToAdd"/> is <see langword="null"/>.
		/// </exception>
		public void AddFolder(NuGenFolderTreeNode folderNodeToAdd)
		{
			if (folderNodeToAdd == null)
			{
				throw new ArgumentNullException("folderNodeToAdd");
			}

			NuGenTreeNode selectedNode = (NuGenTreeNode)this.SelectedNode;

			if (selectedNode != null && selectedNode.Parent != null)
			{
				if (
					selectedNode.Parent is NuGenTaskTreeNodeBase
					&& ((NuGenTaskTreeNodeBase)selectedNode.Parent).IsRemovable
					)
				{
					selectedNode.Parent.Nodes.AddNode(folderNodeToAdd);
				}
				else
				{
					this.Nodes.AddNode(folderNodeToAdd);
				}
			}
			else
			{
				this.Nodes.AddNode(folderNodeToAdd);
			}

			this.SelectedNode = folderNodeToAdd;
			this.OnFolderAdded(EventArgs.Empty);
			this.OnEventToBeDelayed(new NuGenTaskAddedEventArgs(folderNodeToAdd.Text));
		}

		/// <summary>
		/// </summary>
		/// <param name="folderText"></param>
		public void AddFolder(string folderText)
		{
			NuGenFolderTreeNode folderNode = new NuGenFolderTreeNode(this.ServiceProvider, folderText);
			this.InitializeFolder(folderNode);
			this.AddFolder(folderNode);
		}

		/// <summary>
		/// Text = "".
		/// </summary>
		public void AddFolder()
		{
			this.AddFolder("");
		}

		private static readonly object _FolderAdded = new object();

		/// <summary>
		/// </summary>
		public event EventHandler<EventArgs> FolderAdded
		{
			add
			{
				this.Events.AddHandler(_FolderAdded, value);
			}
			remove
			{
				this.Events.RemoveHandler(_FolderAdded, value);
			}
		}

		/// <summary>
		/// Bubbles <see cref="E:FolderAdded"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnFolderAdded(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeActionT<EventArgs>(_FolderAdded, e);
		}

		#endregion

		#region Methods.Public.AddTask

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="taskNodeToAdd"/> is <see langword="null"/>.
		/// </exception>
		public void AddTask(NuGenTaskTreeNode taskNodeToAdd)
		{
			if (taskNodeToAdd == null)
			{
				throw new ArgumentNullException("taskNodeToAdd");
			}

			NuGenTaskTreeNodeBase selectedNode = (NuGenTaskTreeNodeBase)this.SelectedNode;

			if (
				selectedNode is NuGenFolderTreeNode
				&& selectedNode.IsRemovable
				)
			{
				selectedNode.Nodes.AddNode(taskNodeToAdd);
			}
			else if (
				selectedNode != null
				&& selectedNode.Parent != null
				)
			{
				if (
					selectedNode.Parent is NuGenTaskTreeNodeBase
					&& ((NuGenTaskTreeNodeBase)selectedNode.Parent).IsRemovable
					)
				{
					selectedNode.Parent.Nodes.AddNode(taskNodeToAdd);
				}
				else
				{
					this.Nodes.AddNode(taskNodeToAdd);
				}
			}
			else
			{
				this.Nodes.AddNode(taskNodeToAdd);
			}

			this.SelectedNode = taskNodeToAdd;
			this.OnTaskAdded(EventArgs.Empty);
			this.OnEventToBeDelayed(new NuGenTaskAddedEventArgs(taskNodeToAdd.Text));
		}

		/// <summary>
		/// </summary>
		/// <param name="taskText"></param>
		public void AddTask(string taskText)
		{
			NuGenTaskTreeNode taskNode = new NuGenTaskTreeNode(this.ServiceProvider, taskText);
			this.InitializeTask(taskNode);
			this.AddTask(taskNode);
		}

		/// <summary>
		/// Text = "".
		/// </summary>
		public void AddTask()
		{
			this.AddTask("");
		}

		private static readonly object _TaskAdded = new object();

		/// <summary>
		/// </summary>
		public event EventHandler<EventArgs> TaskAdded
		{
			add
			{
				this.Events.AddHandler(_TaskAdded, value);
			}
			remove
			{
				this.Events.RemoveHandler(_TaskAdded, value);
			}
		}

		/// <summary>
		/// Bubbles <see cref="T:TaskAdded"/> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
		protected virtual void OnTaskAdded(EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeActionT<EventArgs>(_TaskAdded, e);
		}

		#endregion

		#region Methods.Public.Clear

		/*
		 * Clear
		 */

		/// <summary>
		/// Clears only removable folders and tasks.
		/// </summary>
		public void Clear()
		{
			IEnumerator enumerator = this.Nodes.GetEnumerator();
			Debug.Assert(enumerator != null, "enumerator != null");

			while (enumerator.MoveNext())
			{
				Debug.Assert(enumerator.Current is NuGenTreeNode);
				this.DeleteTask((NuGenTreeNode)enumerator.Current);
			}
		}

		#endregion

		#region Methods.Public.CodeTask

		/*
		 * AddCodeTask
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="taskItemToAdd"/> is <see langword="null"/>.
		/// </exception>
		public void AddCodeTask(TaskItem taskItemToAdd)
		{
			if (taskItemToAdd == null)
			{
				throw new ArgumentNullException("taskItemToAdd");
			}

			NuGenCodeTaskTreeNode codeTaskNode = new NuGenCodeTaskTreeNode(
				this.ServiceProvider
			);
			
			this.InitializeCodeTask(codeTaskNode);

			Debug.Assert(this.CodeTasks != null, "this.CodeTasks != null");
			this.CodeTasks.Add(codeTaskNode, taskItemToAdd);

			Debug.Assert(this.CommentsFolder != null, "this.CommentsFolder != null");
			this.CommentsFolder.Nodes.AddNode(codeTaskNode);

			codeTaskNode.Text = taskItemToAdd.Description;
		}

		/*
		 * ClearCodeTasks
		 */

		/// <summary>
		/// Clears Comments folder.
		/// </summary>
		public void ClearCodeTasks()
		{
			Debug.Assert(this.CommentsFolder != null, "this.CommentsFolder != null");

			foreach (NuGenTreeNode treeNode in this.CommentsFolder.Nodes)
			{
				if (this.CodeTasks.ContainsKey(treeNode))
				{
					this.CodeTasks.Remove(treeNode);
				}

				treeNode.Remove();
			}
		}

		#endregion

		#region Methods.Public.DeleteTask

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="treeNode"/> is <see langword="null"/>.
		/// </exception>
		public void DeleteTask(NuGenTreeNode treeNode)
		{
			if (treeNode == null)
			{
				throw new ArgumentNullException("treeNode");
			}

			if (treeNode is NuGenTaskTreeNodeBase)
			{
				if (!((NuGenTaskTreeNodeBase)treeNode).IsRemovable)
				{
					return;
				}
			}

			this.Nodes.RemoveNode(treeNode);
			this.SelectionService.RemoveSelectedNode(treeNode);
		}

		/// <summary>
		/// </summary>
		public void DeleteSelectedTasks()
		{
			/* List<NuGenTreeNode> buffer = new List<NuGenTreeNode>(this.SelectedNodes);

			foreach (NuGenTreeNode treeNode in buffer)
			{
				this.DeleteTask(treeNode);
			}

			buffer.Clear(); */
			
			if (this.SelectedNode != null)
			{
				if (this.SelectedNode is NuGenTreeNode)
				{
					this.DeleteTask((NuGenTreeNode)this.SelectedNode);
				}
				else 
				{
					this.SelectedNode.Remove();
				}
			}
		}

		#endregion

		#region Methods.Public.Load

		/// <summary>
		/// </summary>
		/// <param name="path"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="path"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="T:System.IO.FileNotFoundException">
		/// The file at the specified <paramref name="path"/> cannot be found.
		/// </exception>
		public void Load(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			if (!File.Exists(path))
			{
				throw new FileNotFoundException(
					Resources.FileNotFound_LoadPath, path
				);
			}

			using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				this.Load(fileStream);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="streamToLoadFrom"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="streamToLoadFrom"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:System.FormatException">
		/// <paramref name="xmlDocumentToLoadFrom"/> was not in the correct format. TaskList node not found
		/// </exception>
		public void Load(Stream streamToLoadFrom)
		{
			if (streamToLoadFrom == null)
			{
				throw new ArgumentNullException("streamToLoadFrom");
			}

			XmlDocument xmlDoc = new XmlDocument();

			try
			{
				xmlDoc.Load(streamToLoadFrom);
			}
			catch (XmlException)
			{
				this.Clear();
			}

			try
			{
				this.Load(xmlDoc);
			}
			catch (FormatException)
			{
			}
		}
			
		/// <summary>
		/// </summary>
		/// <param name="xmlDocumentToLoadFrom"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="xmlDocumentToLoadFrom"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="T:System.FormatException">
		/// <paramref name="xmlDocumentToLoadFrom"/> was not in the correct format. TaskList node not found.
		/// </exception>
		public void Load(XmlDocument xmlDocumentToLoadFrom)
		{
			this.Clear();
			this.SuspendLayout();

			if (xmlDocumentToLoadFrom == null)
			{
				throw new ArgumentNullException("xmlDocumentToLoadFrom");
			}

			if (xmlDocumentToLoadFrom.HasChildNodes)
			{
				XmlNode rootNode = xmlDocumentToLoadFrom.SelectSingleNode(Resources.XmlTag_TaskList);

				if (rootNode == null)
				{
					throw new FormatException(
						string.Format(Resources.Format_InvalidXmlDocument, Resources.XmlTag_TaskList)
					);
				}

				foreach (XmlNode childNode in rootNode.ChildNodes)
				{
					this.LoadNode(childNode, this.Nodes);
				}
			}

			this.ResumeLayout();
		}

		/// <summary>
		/// </summary>
		/// <param name="xmlNodeToLoadFrom"></param>
		/// <param name="parentNodeCollection"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="xmlNodeToLoadFrom"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="parentNodeCollection"/> is <see langword="null"/>.
		/// </exception>
		protected void LoadNode(XmlNode xmlNodeToLoadFrom, NuGenTreeNodeCollection parentNodeCollection)
		{
			if (xmlNodeToLoadFrom == null)
			{
				throw new ArgumentNullException("xmlNodeToLoadFrom");
			}

			if (parentNodeCollection == null)
			{
				throw new ArgumentNullException("parentNodeCollection");
			}

			NuGenTaskTreeNodeBase treeNodeToAdd = null;

			if (xmlNodeToLoadFrom.Name == Resources.XmlTag_Folder)
			{
				treeNodeToAdd = new NuGenFolderTreeNode(this.ServiceProvider);
				this.InitializeFolder((NuGenFolderTreeNode)treeNodeToAdd);
			}
			else if (xmlNodeToLoadFrom.Name == Resources.XmlTag_Task)
			{
				treeNodeToAdd = new NuGenTaskTreeNode(this.ServiceProvider);
				this.InitializeTask((NuGenTaskTreeNode)treeNodeToAdd);
			}
			else
			{
				return;
			}

			parentNodeCollection.AddNode(treeNodeToAdd);
			treeNodeToAdd.Load(xmlNodeToLoadFrom);

			foreach (XmlNode childNode in xmlNodeToLoadFrom.ChildNodes)
			{
				this.LoadNode(childNode, treeNodeToAdd.Nodes);
			}
		}

		#endregion

		#region Methods.Public.Save

		/// <summary>
		/// </summary>
		/// <param name="path"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="path"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="path"/> is an empty string.
		/// </exception>
		/// <exception cref="T:System.IO.DirectoryNotFoundException">
		/// </exception>
		public void Save(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				throw new ArgumentNullException("path");
			}

			using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				this.Save(fileStream);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="streamToSaveTo"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="streamToSaveTo"/> is <see langword="null"/>.
		/// </exception>
		public void Save(Stream streamToSaveTo)
		{
			if (streamToSaveTo == null)
			{
				throw new ArgumentNullException("streamToSaveTo");
			}

			XmlDocument xmlDoc = new XmlDocument();
			this.Save(xmlDoc);
			xmlDoc.Save(streamToSaveTo);
		}

		/// <summary>
		/// </summary>
		/// <param name="xmlDocumentToSaveTo"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="xmlDocumentToSaveTo"/> is <see langword="null"/>.
		/// </exception>
		public void Save(XmlDocument xmlDocumentToSaveTo)
		{
			if (xmlDocumentToSaveTo == null)
			{
				throw new ArgumentNullException("xmlDocumentToSaveTo");
			}

			xmlDocumentToSaveTo.AppendChild(
				xmlDocumentToSaveTo.CreateXmlDeclaration("1.0", "UTF-8", "yes")
			);

			XmlNode rootNode = xmlDocumentToSaveTo.CreateElement(Resources.XmlTag_TaskList);
			Debug.Assert(rootNode != null, "rootNode != null");

			xmlDocumentToSaveTo.AppendChild(rootNode);

			foreach (NuGenTreeNode node in this.Nodes)
			{
				if (node is NuGenTaskTreeNodeBase)
				{
					this.SaveNode(rootNode, (NuGenTaskTreeNodeBase)node);
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="xmlNodeToSaveTo"></param>
		/// <param name="taskNodeToSave"></param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="xmlNodeToSaveTo"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="taskNodeToSave"/> is <see langword="null"/>.
		/// </exception>
		protected void SaveNode(XmlNode xmlNodeToSaveTo, NuGenTaskTreeNodeBase taskNodeToSave)
		{
			if (xmlNodeToSaveTo == null)
			{
				throw new ArgumentNullException("xmlNodeToSaveTo");
			}

			if (taskNodeToSave == null)
			{
				throw new ArgumentNullException("xmlNodeToSave");
			}

			if (taskNodeToSave.IsRemovable)
			{
				string xmlNodeName = (taskNodeToSave is NuGenTaskTreeNode)
					? Resources.XmlTag_Task
					: Resources.XmlTag_Folder
					;

				XmlNode xmlTaskNode = this.XmlService.AppendChild(xmlNodeToSaveTo, xmlNodeName);
				taskNodeToSave.Save(xmlTaskNode);

				foreach (NuGenTaskTreeNodeBase childNode in taskNodeToSave.Nodes)
				{
					this.SaveNode(xmlTaskNode, childNode);
				}
			}
		}

		#endregion

		#region Methods.Public.SetTaskPriority

		/*
		 * SetTaskPriority
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="targetTaskNode"/> is <see langword="null"/>.
		/// </exception>
		public void SetTaskPriority(NuGenTreeNode targetTaskNode, NuGenTaskPriority priorityToSet)
		{
			if (targetTaskNode == null)
			{
				throw new ArgumentNullException("targetTaskNode");
			}

			if (targetTaskNode is NuGenTaskTreeNode)
			{
				((NuGenTaskTreeNode)targetTaskNode).TaskPriority = priorityToSet;
			}
			else if (targetTaskNode is NuGenFolderTreeNode)
			{
				foreach (NuGenTreeNode treeNode in targetTaskNode.Nodes)
				{
					this.SetTaskPriority(treeNode, priorityToSet);
				}
			}
		}

		/// <summary>
		/// </summary>
		public void SetSelectedTasksPriority(NuGenTaskPriority priorityToSet)
		{
			foreach (NuGenTreeNode treeNode in this.SelectedNodes)
			{
				if (treeNode != null)
				{
					this.SetTaskPriority(treeNode, priorityToSet);
				}
			}
		}

		#endregion

		#region Methods.Protected.Virtual.InitializeCodeTask

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///		<paramref name="codeTaskNodeToInitialize"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void InitializeCodeTask(NuGenCodeTaskTreeNode codeTaskNodeToInitialize)
		{
			if (codeTaskNodeToInitialize == null)
			{
				throw new ArgumentNullException("codeTaskNodeToInitialize");
			}

			Debug.Assert(this.ImageListService != null, "this.ImageListService != null");
			Debug.Assert(this.ImageList != null, "this.ImageList != null");

			codeTaskNodeToInitialize.DefaultImageIndex = this.ImageListService.GetImageIndex(this.ImageList, PRIORITY_WANTED); 
		}

		#endregion

		#region Methods.Protected.Virtual.InitializeFolder

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="folderNodeToInitialize"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void InitializeFolder(NuGenFolderTreeNode folderNodeToInitialize)
		{
			if (folderNodeToInitialize == null)
			{
				throw new ArgumentNullException("folderNodeToInitialize");
			}

			Debug.Assert(this.ImageListService != null, "this.ImageListService != null");
			Debug.Assert(this.ImageList != null, "this.ImageList != null");

			folderNodeToInitialize.ImageIndex = this.ImageListService.GetImageIndex(this.ImageList, FOLDER);
			folderNodeToInitialize.ExpandedImageIndex = this.ImageListService.GetImageIndex(this.ImageList, FOLDER_OPEN);
		}

		#endregion

		#region Methods.Protected.Virtual.InitializeTask

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="taskNodeToInitialize"/> is <see langword="null"/>.
		/// </exception>
		protected virtual void InitializeTask(NuGenTaskTreeNode taskNodeToInitialize)
		{
			if (taskNodeToInitialize == null)
			{
				throw new ArgumentNullException("taskNodeToInitialize");
			}

			Debug.Assert(this.ImageListService != null, "this.ImageListService != null");
			Debug.Assert(this.ImageList != null, "this.ImageList != null");

			taskNodeToInitialize.SetPriorityImageIndex(
				NuGenTaskPriority.Critical,
				this.ImageListService.GetImageIndex(this.ImageList, PRIORITY_CRITICAL)
			);

			taskNodeToInitialize.SetPriorityImageIndex(
				NuGenTaskPriority.Maybe,
				this.ImageListService.GetImageIndex(this.ImageList, PRIORITY_MAYBE)
			);

			taskNodeToInitialize.SetPriorityImageIndex(
				NuGenTaskPriority.Required,
				this.ImageListService.GetImageIndex(this.ImageList, PRIORITY_REQUIRED)
			);

			taskNodeToInitialize.SetPriorityImageIndex(
				NuGenTaskPriority.Wanted,
				this.ImageListService.GetImageIndex(this.ImageList, PRIORITY_WANTED)
			);

			taskNodeToInitialize.SetPriorityImageIndex(
				NuGenTaskPriority.WouldBeNice,
				this.ImageListService.GetImageIndex(this.ImageList, PRIORITY_WOULDBENICE)
			);
		}

		#endregion

		#region Methods.Protected.Overriden.DragDrop

		/*
		 * OnItemDrag
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.TreeView.ItemDrag"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.Windows.Forms.ItemDragEventArgs"></see> that contains the event data.</param>
		protected override void OnItemDrag(ItemDragEventArgs e)
		{
			if (e.Button == MouseButtons.Left && e.Item is NuGenTaskTreeNodeBase)
			{
				this.DoDragDrop(e.Item, DragDropEffects.Move);
			}

			base.OnItemDrag(e);
		}

		/*
		 * OnDragEnter
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragEnter"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
		protected override void OnDragEnter(DragEventArgs e)
		{
			if (NuGenArgument.GetCompatibleDataObjectType(e.Data, typeof(NuGenTaskTreeNodeBase)) != null)
			{
				e.Effect = DragDropEffects.Move;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}

			base.OnDragEnter(e);
		}

		/*
		 * OnDragDrop
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DragDrop"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.DragEventArgs"></see> that contains the event data.</param>
		protected override void OnDragDrop(DragEventArgs e)
		{
			/* WORKAROUND: this.GetNodeAt(e.X, e.Y) always returns null. */

			Point clientCursorPosition = this.PointToClient(Cursor.Position);
			NuGenTreeNode targetTreeNode = this.GetNodeAt(clientCursorPosition);

			NuGenTreeNode selectedNode = null;

			if (this.SelectedNodes.Count > 0)
			{
				selectedNode = this.SelectedNodes[0];
			}

			this.DragDropService.DoDrop(
				targetTreeNode,
				this.SelectedNodes,
				this.DragDropService.GetDropPosition(targetTreeNode, clientCursorPosition)
			);

			this.SelectedNode = selectedNode;
			base.OnDragDrop(e);
		}

		#endregion

		#region Methods.Protected.Overriden.Mouse

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.DoubleClick"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnDoubleClick(EventArgs e)
		{
			base.OnDoubleClick(e);

			NuGenTreeNode treeNode = this.GetNodeAt(this.PointToClient(Cursor.Position));

			if (treeNode is NuGenCodeTaskTreeNode)
			{
				if (this.CodeTasks.ContainsKey(treeNode))
				{
					TaskItem taskItemToNavigate = this.CodeTasks[treeNode];

					if (taskItemToNavigate != null)
					{
						taskItemToNavigate.Navigate();
					}
				}
			}
		}

		#endregion

		#region Methods.Protected.Overriden.NodeOperations

		/*
		 * OnAfterCheck
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.TreeView.AfterCheck"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs"></see> that contains the event data.</param>
		protected override void OnAfterCheck(TreeViewEventArgs e)
		{
			base.OnAfterCheck(e);

			if (e.Node.Checked)
			{
				e.Node.NodeFont = new Font(this.Font, FontStyle.Strikeout);
			}
			else
			{
				e.Node.NodeFont = new Font(this.Font, FontStyle.Regular);
			}
		}

		/*
		 * OnAfterSelect
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.TreeView.AfterSelect"></see> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.TreeViewEventArgs"></see> that contains the event data.</param>
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			base.OnAfterSelect(e);

			if (e.Node is NuGenTaskTreeNodeBase)
			{
				this.OnEventToBeDelayed(
					new NuGenSelectedTaskChangedEventArgs(
						((NuGenTaskTreeNodeBase)e.Node).Text,
						((NuGenTaskTreeNodeBase)e.Node).IsDescriptionReadonly
					)
				);
			}
			else if (e.Node != null)
			{
				this.OnEventToBeDelayed(new NuGenSelectedTaskChangedEventArgs(e.Node.Text));
			}
		}

		#endregion

		#region Methods.Private.Initialization

		/**
		 * InitializeCommentsFolder
		 */

		private void InitializeCommentsFolder(ref NuGenCommentsFolderTreeNode folderNodeToInitialize)
		{
			Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");

			folderNodeToInitialize = new NuGenCommentsFolderTreeNode(
				this.ServiceProvider,
				Resources.Folder_Comments
			);

			this.InitializeFolder(folderNodeToInitialize);
			this.Nodes.AddNode(folderNodeToInitialize);
		}

		/**
		 * InitializeContextMenu
		 */

		private void InitializeContextMenu(ref ContextMenuStrip contextMenuToInitialize, ref IContainer componentsToAddContextMenuTo)
		{
			if (componentsToAddContextMenuTo == null)
			{
				componentsToAddContextMenuTo = new Container();
			}

			contextMenuToInitialize = new ContextMenuStrip(componentsToAddContextMenuTo);
			contextMenuToInitialize.RenderMode = ToolStripRenderMode.System;

			List<ToolStripMenuItem> priorityItems = new List<ToolStripMenuItem>();

			priorityItems.Add(
				new ToolStripMenuItem(
					Resources.CriticalMenuItem_Text,
					Resources.Priority_Critical,
					this.criticalMenuItem_Click
				)
			);

			priorityItems.Add(
					new ToolStripMenuItem(
					Resources.RequiredMenuItem_Text,
					Resources.Priority_Required,
					this.requiredMenuItem_Click
				)
			);

			priorityItems.Add(
					new ToolStripMenuItem(
					Resources.WantedMenuItem_Text,
					Resources.Priority_Wanted,
					this.wantedMenuItem_Click
				)
			);

			priorityItems.Add(
					new ToolStripMenuItem(
					Resources.WouldBeNiceMenuItem_Text,
					Resources.Priority_WouldBeNice,
					this.wouldBeNiceMenuItem_Click
				)
			);

			priorityItems.Add(
				new ToolStripMenuItem(
					Resources.MaybeMenuItem_Text,
					Resources.Priority_Maybe,
					this.maybeMenuItem_Click
				)
			);

			ToolStripMenuItem deleteMenuItem = new ToolStripMenuItem(
				Resources.DeleteMenuItem_Text,
				Resources.Delete,
				this.deleteMenuItem_Click,
				Keys.Delete
			);

			ToolStripMenuItem selectAllMenuItem = new ToolStripMenuItem(
				Resources.SelectAllMenuItem_Text,
				null,
				this.selectAllMenuItem_Click,
				Keys.Control | Keys.A
			);


			foreach (ToolStripMenuItem priorityItem in priorityItems)
			{
				contextMenuToInitialize.Items.Add(priorityItem);
			}

			contextMenuToInitialize.Items.Add("-");
			contextMenuToInitialize.Items.Add(deleteMenuItem);
			/* TURNED OFF: contextMenuToInitialize.Items.Add("-");
			contextMenuToInitialize.Items.Add(selectAllMenuItem); */

			contextMenuToInitialize.Opening += delegate
			{
				if (this.SelectedNode is NuGenTaskTreeNodeBase)
				{
					NuGenTaskTreeNodeBase selectedNode = (NuGenTaskTreeNodeBase)this.SelectedNode;
					
					bool hasPriority = selectedNode.HasPriority;
					bool isRemovable = selectedNode.IsRemovable;

					deleteMenuItem.Enabled = isRemovable;

					foreach (ToolStripMenuItem priorityItem in priorityItems)
					{
						priorityItem.Enabled = hasPriority;
					}
				}
				else
				{
					deleteMenuItem.Enabled = true;

					foreach (ToolStripMenuItem priorityItem in priorityItems)
					{
						priorityItem.Enabled = true;
					}
				}
			};
		}

		/*
		 * InitializeTaskImageList
		 */

		/// <summary>
		/// </summary>
		/// 
		/// <param name="imageListToInitialize">
		/// Can be <see langword="null"/>. The instance will be created automatically.
		/// </param>
		/// 
		/// <param name="componentsToAddImageListTo">
		/// Can be <see langword="null"/>. The instance will be created automatically.
		/// </param>
		private void InitializeTaskImageList(ref ImageList imageListToInitialize, ref IContainer componentsToAddImageListTo)
		{
			if (componentsToAddImageListTo == null)
			{
				componentsToAddImageListTo = new Container();
			}

			imageListToInitialize = new ImageList(componentsToAddImageListTo);
			imageListToInitialize.ColorDepth = ColorDepth.Depth32Bit;

			this.ImageListService.AddImages(
				imageListToInitialize,
				new NuGenImageDescriptor[]
				{
					new NuGenImageDescriptor(Resources.Folder, FOLDER),
					new NuGenImageDescriptor(Resources.Folder_Open, FOLDER_OPEN),
					new NuGenImageDescriptor(Resources.Priority_Critical, PRIORITY_CRITICAL),
					new NuGenImageDescriptor(Resources.Priority_Required, PRIORITY_REQUIRED),
					new NuGenImageDescriptor(Resources.Priority_Wanted, PRIORITY_WANTED),
					new NuGenImageDescriptor(Resources.Priority_WouldBeNice, PRIORITY_WOULDBENICE),
					new NuGenImageDescriptor(Resources.Priority_Maybe, PRIORITY_MAYBE)
				}
			);
		}

		#endregion

		#region EventHandlers.ContextMenu

		private void deleteMenuItem_Click(object sender, EventArgs e)
		{
			this.DeleteSelectedTasks();
		}

		private void selectAllMenuItem_Click(object sender, EventArgs e)
		{
			this.SelectAllNodes();
		}

		private void criticalMenuItem_Click(object sender, EventArgs e)
		{
			this.SetSelectedTasksPriority(NuGenTaskPriority.Critical);
		}

		private void maybeMenuItem_Click(object sender, EventArgs e)
		{
			this.SetSelectedTasksPriority(NuGenTaskPriority.Maybe);
		}

		private void requiredMenuItem_Click(object sender, EventArgs e)
		{
			this.SetSelectedTasksPriority(NuGenTaskPriority.Required);
		}

		private void wantedMenuItem_Click(object sender, EventArgs e)
		{
			this.SetSelectedTasksPriority(NuGenTaskPriority.Wanted);
		}

		private void wouldBeNiceMenuItem_Click(object sender, EventArgs e)
		{
			this.SetSelectedTasksPriority(NuGenTaskPriority.WouldBeNice);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskTreeView"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Required:<para/>
		/// <see cref="T:INuGenImageListService"/><para/>
		/// <see cref="T:INuGenTreeViewSelectionService"/><para/>
		/// <see cref="T:INuGenTreeViewDragDropService"/><para/>
		/// <see cref="T:INuGenTreeViewSelectionService"/><para/>
		/// <see cref="T:INuGenTreeNodeSorter"/><para/>
		/// <see cref="T:INuGenTaskXmlService"/><para/>
		/// <see cref="T:INuGenStringProcessor"/><para/>
		/// </param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </exception>
		public NuGenTaskTreeView(
			INuGenServiceProvider serviceProvider
			) : base(serviceProvider)
		{
			_Components = new Container();

			this.InitializeContextMenu(ref _ContextMenu, ref _Components);
			this.InitializeTaskImageList(ref _TaskImageList, ref _Components);

			Debug.Assert(_ContextMenu != null, "_ContextMenu != null");
			Debug.Assert(_TaskImageList != null, "_TaskImageList != null");
			Debug.Assert(_Components != null, "_Components != null");

			this.AllowDrop = true;
			this.CheckBoxes = true;
			this.ContextMenuStrip = _ContextMenu;
			this.FullRowSelect = true;
			this.HideSelection = false;
			this.ImageList = _TaskImageList;
			this.ShowLines = false;

			this.InitializeCommentsFolder(ref _CommentsFolder);
			Debug.Assert(_CommentsFolder != null, "_CommentsFolder != null");
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTaskTreeView"/> class.
		/// </summary>
		public NuGenTaskTreeView()
			: this(new NuGenTaskServiceProvider())
		{
		}

		#endregion

		#region Dispose

		/// <summary>
		/// Releases the unmanaged resources used by the <see cref="T:System.Windows.Forms.TreeView"></see> and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing"></param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_Components != null)
				{
					_Components.Dispose();
				}
			}

			base.Dispose(disposing);
		}

		#endregion
	}
}
