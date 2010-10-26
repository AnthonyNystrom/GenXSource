/* -----------------------------------------------
 * NuGenTaskTreeViewTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;
using DotNetMock.Dynamic;

using EnvDTE;

using Genetibase.Controls;
using Genetibase.NuGenTaskList;
using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;
using System.Xml;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public partial class NuGenTaskTreeViewTests
	{
		private NuGenTaskTreeView _TaskTreeView = null;
		private TaskTreeViewEventSink _EventSink = null;
		private INuGenServiceProvider _ServiceProvider = null;
		private XmlDocument _XmlDoc = null;

		private DynamicMock _TaskItemMock = null;
		private string _TaskItemDescription = "";
		private TaskItem _TaskItem = null;

		private string _TaskText = "task";
		private NuGenTaskPriority _TaskPriority = NuGenTaskPriority.Critical;

		private string _Task2Text = "task2";
		private NuGenTaskPriority _Task2Priority = NuGenTaskPriority.Maybe;

		private string _FolderText = "folder";
		private string _FolderChildText = "folderChild";
		private NuGenTaskPriority _FolderChildPriority = NuGenTaskPriority.Required;

		private int _InitialCount = 0;

		[SetUp]
		public void SetUp()
		{
			_TaskTreeView = new NuGenTaskTreeView();
			_InitialCount = _TaskTreeView.Nodes.Count;
			_EventSink = new TaskTreeViewEventSink(_TaskTreeView);
			_ServiceProvider = new NuGenTaskServiceProvider();
			_XmlDoc = new XmlDocument();

			_TaskItemDescription = "Description";
			_TaskItemMock = new DynamicMock(typeof(TaskItem));
			_TaskItemMock.SetValue("Description", _TaskItemDescription);
			_TaskItem = (TaskItem)_TaskItemMock.Object;

			Assert.IsFalse(_TaskTreeView.LabelEdit);
		}

		[Test]
		public void LoadTest()
		{
			this.PopulateTaskTreeView();
			this._TaskTreeView.Save(this._XmlDoc);
			this._TaskTreeView = new NuGenTaskTreeView();
			this._TaskTreeView.Load(this._XmlDoc);

			Assert.AreEqual(this._InitialCount + 3, this._TaskTreeView.Nodes.Count);

			Assert.IsTrue(this._TaskTreeView.Nodes[this._InitialCount] is NuGenTaskTreeNode);
			Assert.IsTrue(this._TaskTreeView.Nodes[this._InitialCount + 1] is NuGenTaskTreeNode);
			Assert.IsTrue(this._TaskTreeView.Nodes[this._InitialCount + 2] is NuGenFolderTreeNode);

			NuGenTaskTreeNode task = (NuGenTaskTreeNode)this._TaskTreeView.Nodes[this._InitialCount];
			NuGenTaskTreeNode task2 = (NuGenTaskTreeNode)this._TaskTreeView.Nodes[this._InitialCount + 1];

			Assert.IsTrue(task.Completed);
			Assert.AreEqual(this._TaskText, task.Text);
			Assert.AreEqual(this._TaskText, ((NuGenTreeNode)task).Text);
			Assert.AreEqual(this._TaskPriority, task.TaskPriority);

			Assert.IsFalse(task2.Completed);
			Assert.AreEqual(this._Task2Text, task2.Text);
			Assert.AreEqual(this._Task2Priority, task2.TaskPriority);

			NuGenTreeNode folderTreeNode = this._TaskTreeView.Nodes[this._InitialCount + 2];
			Assert.AreEqual(1, folderTreeNode.Nodes.Count);
			Assert.IsTrue(folderTreeNode.Nodes[0] is NuGenTaskTreeNode);

			NuGenTaskTreeNode folderChild = (NuGenTaskTreeNode)folderTreeNode.Nodes[0];

			Assert.IsTrue(folderChild.Completed);
			Assert.AreEqual(_FolderChildPriority, folderChild.TaskPriority);
			Assert.AreEqual(_FolderChildText, folderChild.Text);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadArgumentNullExceptionTest()
		{
			this._TaskTreeView.Load((XmlDocument)null);
		}

		[Test]
		public void SaveTest()
		{
			this.PopulateTaskTreeView();
			this._TaskTreeView.Save(this._XmlDoc);

			Assert.AreEqual(2, this._XmlDoc.ChildNodes.Count);
			
			XmlNode childNode = this._XmlDoc.ChildNodes[1];
			Assert.AreEqual(3, childNode.ChildNodes.Count);

			XmlNode taskNode = childNode.ChildNodes[0];
			Assert.AreEqual(Resources.XmlTag_Task, taskNode.Name);
			Assert.AreEqual(_TaskText, taskNode[Resources.XmlTag_Text].InnerText);
			Assert.AreEqual(_TaskPriority, Enum.Parse(typeof(NuGenTaskPriority), taskNode[Resources.XmlTag_TaskPriority].InnerText));
			Assert.IsTrue(bool.Parse(taskNode[Resources.XmlTag_Completed].InnerText));

			XmlNode taskNode2 = childNode.ChildNodes[1];
			Assert.AreEqual(Resources.XmlTag_Task, taskNode2.Name);
			Assert.AreEqual(_Task2Text, taskNode2[Resources.XmlTag_Text].InnerText);
			Assert.AreEqual(_Task2Priority, Enum.Parse(typeof(NuGenTaskPriority), taskNode2[Resources.XmlTag_TaskPriority].InnerText));
			Assert.IsFalse(bool.Parse(taskNode2[Resources.XmlTag_Completed].InnerText));

			XmlNode folderNode = childNode.ChildNodes[2];
			Assert.AreEqual(Resources.XmlTag_Folder, folderNode.Name);
			Assert.AreEqual(_FolderText, folderNode[Resources.XmlTag_Text].InnerText);
			Assert.AreEqual(3, folderNode.ChildNodes.Count);

			XmlNode folderChildNode = folderNode.ChildNodes[2];
			Assert.AreEqual(Resources.XmlTag_Task, folderChildNode.Name);
			Assert.AreEqual(_FolderChildText, folderChildNode[Resources.XmlTag_Text].InnerText);
			Assert.AreEqual(_FolderChildPriority, Enum.Parse(typeof(NuGenTaskPriority), folderChildNode[Resources.XmlTag_TaskPriority].InnerText));
			Assert.IsTrue(bool.Parse(folderChildNode[Resources.XmlTag_Completed].InnerText));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SaveArgumentNullExceptionTest()
		{
			this._TaskTreeView.Save((XmlDocument)null);
		}

		private void PopulateTaskTreeView()
		{
			NuGenTaskTreeNode task = new NuGenTaskTreeNode(
				this._ServiceProvider,
				this._TaskText,
				this._TaskPriority
			);

			task.Text = this._TaskText;
			task.Checked = true;

			NuGenTaskTreeNode task2 = new NuGenTaskTreeNode(
				this._ServiceProvider,
				this._Task2Text,
				this._Task2Priority
			);

			task2.Text = this._Task2Text;

			NuGenFolderTreeNode folder = new NuGenFolderTreeNode(
				this._ServiceProvider,
				this._FolderText
			);

			folder.Text = this._FolderText;

			NuGenTaskTreeNode folderChild = new NuGenTaskTreeNode(
				this._ServiceProvider,
				this._FolderChildText,
				this._FolderChildPriority
			);

			folderChild.Checked = true;
			folderChild.Text = this._FolderChildText;
			folder.Nodes.AddNode(folderChild);

			this._TaskTreeView.Nodes.AddNodeRange(new NuGenTreeNode[] { task, task2, folder });
		}
	}
}
