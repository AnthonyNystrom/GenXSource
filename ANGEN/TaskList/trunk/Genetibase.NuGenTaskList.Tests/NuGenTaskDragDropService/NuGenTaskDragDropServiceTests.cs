/* -----------------------------------------------
 * NuGenTaskDragDropServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Controls;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.NuGenTaskList.Tests
{
	[TestFixture]
	public partial class NuGenTaskDragDropServiceTests
	{
		private NuGenTaskDragDropService _DropService = null;
		private NuGenTaskTreeView _TaskTreeView = null;
		private INuGenServiceProvider _ServiceProvider = null;
		private int _InitialCount = 0;

		[SetUp]
		public void SetUp()
		{
			this._DropService = new NuGenTaskDragDropService();
			this._TaskTreeView = new NuGenTaskTreeView();
			this._ServiceProvider = new NuGenTaskServiceProvider();
			this._InitialCount = this._TaskTreeView.Nodes.Count;

			for (int i = 0; i < 10; i++)
			{
				NuGenFolderTreeNode folderNode = new NuGenFolderTreeNode(this._ServiceProvider);
				this._TaskTreeView.AddFolder(folderNode);

				for (int j = 0; j < 10; j++)
				{
					folderNode.Nodes.AddNode(new NuGenTaskTreeNode(this._ServiceProvider));
				}
			}

			for (int i = 0; i < 10; i++)
			{
				this._TaskTreeView.AddTask(new NuGenTaskTreeNode(this._ServiceProvider));
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DoDragArgumentNullExceptionTest()
		{
			this._DropService.DoDrop(new NuGenTreeNode(), null, NuGenDropPosition.Inside);
		}
	}
}
