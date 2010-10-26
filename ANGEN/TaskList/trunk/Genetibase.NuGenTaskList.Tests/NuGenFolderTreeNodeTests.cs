/* -----------------------------------------------
 * NuGenFolderTreeNodeTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.NuGenTaskList.Properties;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;
using System.Xml;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public class NuGenFolderTreeNodeTests
	{
		private INuGenServiceProvider	_ServiceProvider	= null;
		private NuGenFolderTreeNode		_FolderTreeNode		= null;
		private string					_FolderText			= null;
		private XmlDocument				_XmlDoc				= null;
		private XmlNode					_NodeToSaveTo		= null;

		[SetUp]
		public void SetUp()
		{
			this._ServiceProvider = new NuGenTaskServiceProvider();
			
			this._FolderTreeNode = new NuGenFolderTreeNode(this._ServiceProvider);
			this._FolderTreeNode.Nodes.AddNode(new NuGenTreeNode());
			this._FolderTreeNode.Expand();

			this._FolderText = "folder";
			this._FolderTreeNode.Text = this._FolderText;
			
			this._XmlDoc = new XmlDocument();
			this._NodeToSaveTo = this._XmlDoc.CreateElement("Folder");
			this._XmlDoc.AppendChild(this._NodeToSaveTo);
		}

		[Test]
		public void LoadTest()
		{
			this._FolderTreeNode.Save(this._NodeToSaveTo);
			NuGenFolderTreeNode restoredNode = new NuGenFolderTreeNode(this._ServiceProvider);
			NuGenTaskTreeView taskTreeView = new NuGenTaskTreeView();
			taskTreeView.Nodes.AddNode(restoredNode);
			restoredNode.Load(this._NodeToSaveTo);

			Assert.AreEqual(this._FolderText, restoredNode.Text);
			Assert.IsTrue(this._FolderTreeNode.IsExpanded);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadArgumentNullExceptionTest()
		{
			this._FolderTreeNode.Load(null);
		}

		[Test]
		public void SaveTest()
		{
			this._FolderTreeNode.Save(this._NodeToSaveTo);

			INuGenTaskXmlService xmlService = this._ServiceProvider.GetService<INuGenTaskXmlService>();

			if (xmlService == null)
			{
				Assert.Fail("Service of type INuGenTaskXmlService not found.");
			}

			Assert.AreEqual(2, this._NodeToSaveTo.ChildNodes.Count);
			Assert.AreEqual(
				this._FolderText,
				xmlService.GetChildText(this._NodeToSaveTo, Resources.XmlTag_Text, "")
			);
			Assert.IsTrue(
				bool.Parse(
					xmlService.GetChildText(
						this._NodeToSaveTo,
						Resources.XmlTag_Expanded,
						bool.FalseString
					)
				)
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SaveArgumentNullExceptionTest()
		{
			this._FolderTreeNode.Save(null);
		}

		[Test]
		public void ConstructorTest()
		{
			int imageIndex = 0;
			int expandedImageIndex = 1;

			this._FolderTreeNode = new NuGenFolderTreeNode(
				this._ServiceProvider,
				"",
				imageIndex,
				expandedImageIndex
			);

			Assert.IsFalse(this._FolderTreeNode.HasCheckBox);
			Assert.AreEqual(imageIndex, this._FolderTreeNode.ImageIndex);
			Assert.AreEqual(expandedImageIndex, this._FolderTreeNode.ExpandedImageIndex);
		}
	}
}
