/* -----------------------------------------------
 * NuGenMenuItemCheckedTrackerTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;
using DotNetMock.Dynamic;

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenMenuItemCheckedTrackerTests
	{
		private NuGenMenuItemCheckedTracker _checkedTracker = null;

		private ToolStripMenuItem _menuItem = null;
		private ToolStripMenuItem _menuItem2 = null;
		private ToolStripMenuItem _menuItem3 = null;

		[SetUp]
		public void SetUp()
		{
			_checkedTracker = new NuGenMenuItemCheckedTracker();

			_menuItem = new ToolStripMenuItem();
			_menuItem2 = new ToolStripMenuItem();
			_menuItem3 = new ToolStripMenuItem();
		}
		
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CreateGroupArgumentNullExceptionTest()
		{
			_checkedTracker.CreateGroup(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CheckedChangedArgumentNullExceptionOnGroupTest()
		{
			_checkedTracker.ChangeChecked(null, _menuItem);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CheckedChangedArgumentNullExceptionOnMenuItemTest()
		{
			DynamicMock groupMock = new DynamicMock(typeof(INuGenMenuItemGroup));
			_checkedTracker.ChangeChecked((INuGenMenuItemGroup)groupMock.Object, null);
		}

		[Test]
		public void CheckedChangedTest()
		{
			INuGenMenuItemGroup group = _checkedTracker.CreateGroup(
				new ToolStripMenuItem[] {
					_menuItem,
					_menuItem2,
					_menuItem3
				}
			);

			Assert.AreEqual(3, group.Items.Count);

			for (int i = 0; i < 2; i++)
			{
				_checkedTracker.ChangeChecked(group, _menuItem);

				Assert.IsTrue(_menuItem.Checked);
				Assert.IsFalse(_menuItem2.Checked);
				Assert.IsFalse(_menuItem3.Checked);
			}

			_checkedTracker.ChangeChecked(group, _menuItem2);

			Assert.IsFalse(_menuItem.Checked);
			Assert.IsTrue(_menuItem2.Checked);
			Assert.IsFalse(_menuItem3.Checked);
		}
	}
}
