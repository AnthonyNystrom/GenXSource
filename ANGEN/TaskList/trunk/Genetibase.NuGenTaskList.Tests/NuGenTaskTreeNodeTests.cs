/* -----------------------------------------------
 * NuGenTaskTreeNodeTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;
using System.Windows.Forms;
using System.Xml;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public class NuGenTaskTreeNodeTests
	{
		private INuGenServiceProvider serviceProvider = null;
		private NuGenTaskTreeNode taskTreeNode = null;
		private XmlDocument xmlDoc = null;
		private XmlNode nodeToSaveTo = null;
		
		private string firstLine = "task";
		private string secondLine = "tree node";
		private string taskText = "";

		private int wantedImageIndex = 1;
		private int wouldBeNiceImageIndex = 2;
		private int maybeImageIndex = 3;
		private int requiredImageIndex = 4;
		private int criticalImageIndex = 5;

		[SetUp]
		public void SetUp()
		{
			this.serviceProvider = new NuGenTaskServiceProvider();

			this.xmlDoc = new XmlDocument();
			this.nodeToSaveTo = this.xmlDoc.CreateElement("Task");
			this.xmlDoc.AppendChild(this.nodeToSaveTo);

			this.taskText = string.Concat(this.firstLine, Environment.NewLine, this.secondLine);

			this.taskTreeNode = new NuGenTaskTreeNode(this.serviceProvider, this.taskText, NuGenTaskPriority.Wanted);

			this.taskTreeNode.SetPriorityImageIndex(NuGenTaskPriority.Wanted, this.wantedImageIndex);
			this.taskTreeNode.SetPriorityImageIndex(NuGenTaskPriority.WouldBeNice, this.wouldBeNiceImageIndex);
			this.taskTreeNode.SetPriorityImageIndex(NuGenTaskPriority.Maybe, this.maybeImageIndex);
			this.taskTreeNode.SetPriorityImageIndex(NuGenTaskPriority.Required, this.requiredImageIndex);
			this.taskTreeNode.SetPriorityImageIndex(NuGenTaskPriority.Critical, this.criticalImageIndex);
		}

		[Test]
		public void LoadTest()
		{
			NuGenTaskPriority taskPriority = NuGenTaskPriority.Required;
			this.taskTreeNode = new NuGenTaskTreeNode(this.serviceProvider, this.taskText, taskPriority);
			this.taskTreeNode.Text = this.taskText;
			this.taskTreeNode.Completed = true;

			this.taskTreeNode.Save(this.nodeToSaveTo);

			NuGenTaskTreeNode restoredTreeNode = new NuGenTaskTreeNode(this.serviceProvider);
			NuGenTaskTreeView taskTreeView = new NuGenTaskTreeView();
			taskTreeView.Nodes.AddNode(restoredTreeNode);

			restoredTreeNode.Load(this.nodeToSaveTo);

			Assert.AreEqual(this.taskText, restoredTreeNode.Text);
			Assert.AreEqual(this.firstLine, ((TreeNode)restoredTreeNode).Text);
			Assert.AreEqual(true, this.taskTreeNode.Completed);
			Assert.AreEqual(taskPriority, this.taskTreeNode.TaskPriority);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadArgumentNullExceptionTest()
		{
			this.taskTreeNode.Load(null);
		}

		[Test]
		public void SaveTest()
		{
			NuGenTaskPriority taskPriority = NuGenTaskPriority.Required;
			this.taskTreeNode = new NuGenTaskTreeNode(this.serviceProvider, this.taskText, taskPriority);
			this.taskTreeNode.Text = this.taskText;
			
			this.taskTreeNode.Save(this.nodeToSaveTo);

			INuGenTaskXmlService xmlService = this.serviceProvider.GetService<INuGenTaskXmlService>();

			if (xmlService == null)
			{
				Assert.Fail("Service of type INuGenTaskXmlService not found.");
			}

			Assert.AreEqual(
				this.taskText,
				xmlService.GetChildText(
					this.nodeToSaveTo,
					Resources.XmlTag_Text,
					""
				)
			);

			Assert.AreEqual(
				taskPriority,
				Enum.Parse(
					typeof(NuGenTaskPriority),
					xmlService.GetChildText(
						this.nodeToSaveTo,
						Resources.XmlTag_TaskPriority,
						NuGenTaskPriority.Wanted.ToString()
					)
				)
			);
			
			Assert.AreEqual(
				false,
				bool.Parse(
					xmlService.GetChildText(
						this.nodeToSaveTo,
						Resources.XmlTag_Completed,
						bool.FalseString
					)
				)
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SaveArgumentNullExceptionTest()
		{
			this.taskTreeNode.Save(null);
		}

		[Test]
		public void ConstructorTest()
		{
			Assert.AreEqual("", this.taskTreeNode.Text);
			Assert.AreEqual(this.taskText, ((TreeNode)this.taskTreeNode).Text);
			Assert.IsTrue(this.taskTreeNode.HasCheckBox);
			Assert.IsFalse(this.taskTreeNode.Checked);

			Assert.AreEqual(this.maybeImageIndex, this.taskTreeNode.GetPriorityImageIndex(NuGenTaskPriority.Maybe));
			Assert.AreEqual(this.wouldBeNiceImageIndex, this.taskTreeNode.GetPriorityImageIndex(NuGenTaskPriority.WouldBeNice));
			Assert.AreEqual(this.wantedImageIndex, this.taskTreeNode.GetPriorityImageIndex(NuGenTaskPriority.Wanted));
			Assert.AreEqual(this.requiredImageIndex, this.taskTreeNode.GetPriorityImageIndex(NuGenTaskPriority.Required));
			Assert.AreEqual(this.criticalImageIndex, this.taskTreeNode.GetPriorityImageIndex(NuGenTaskPriority.Critical));

			Assert.IsTrue(this.taskTreeNode.HasCheckBox);
			Assert.IsFalse(this.taskTreeNode.Checked);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			this.taskTreeNode = new NuGenTaskTreeNode(null);
		}

		[Test]
		public void CompletedTest()
		{
			this.taskTreeNode.Checked = true;
			Assert.IsTrue(this.taskTreeNode.Completed);

			this.taskTreeNode.Checked = false;
			Assert.IsFalse(this.taskTreeNode.Completed);

			this.taskTreeNode.Completed = true;
			Assert.IsTrue(this.taskTreeNode.Checked);

			this.taskTreeNode.Completed = false;
			Assert.IsFalse(this.taskTreeNode.Checked);
		}

		[Test]
		public void TaskPriorityTest()
		{
			Assert.AreEqual(this.wantedImageIndex, this.taskTreeNode.ImageIndex);
			Assert.AreEqual(this.wantedImageIndex, this.taskTreeNode.SelectedImageIndex);

			this.taskTreeNode.TaskPriority = NuGenTaskPriority.Critical;
			Assert.AreEqual(NuGenTaskPriority.Critical, this.taskTreeNode.TaskPriority);

			Assert.AreEqual(this.criticalImageIndex, this.taskTreeNode.ImageIndex);
			Assert.AreEqual(this.criticalImageIndex, this.taskTreeNode.SelectedImageIndex);
		}

		[Test]
		public void TaskTextTest()
		{
			this.taskTreeNode.Text = null;
			Assert.AreEqual("", this.taskTreeNode.Text);
		}
	}
}
