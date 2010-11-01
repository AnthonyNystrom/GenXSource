/* -----------------------------------------------
 * NuGenTabControlTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Controls.Tests
{
	[TestFixture]
	public partial class NuGenTabControlTests
	{
		private TabControlStub _tabControl = null;
		private NuGenTabControl.TabPageCollection _collection = null;
		private EventSink _eventSink = null;

		[SetUp]
		public void SetUp()
		{
			_tabControl = new TabControlStub();
			_collection = new NuGenTabControl.TabPageCollection(_tabControl);
			_eventSink = new EventSink(_tabControl);
		}

		[Test]
		public void AddRemoveTest()
		{
			NuGenTabPage tabPage = _tabControl.TabPages.Add("");
			NuGenTabPage tabPage2 = _tabControl.TabPages.Add("");
			Assert.AreEqual(2, _tabControl.TabPages.Count);

			_tabControl.TabPages.Remove(tabPage2);
			Assert.AreEqual(1, _tabControl.TabPages.Count);

			NuGenTabPage tabPage3 = _tabControl.TabPages.Add("");
			Assert.AreEqual(2, _tabControl.TabPages.Count);
		}

		[Test]
		public void AddTabPageTest()
		{
			_eventSink.ExpectedSelectedIndexChanged = 3;

			NuGenTabPage tabPage = _tabControl.TabPages.Add("0");
			NuGenTabPage tabPage1 = _tabControl.TabPages.Add("1");
			NuGenTabPage tabPage2 = _tabControl.TabPages.Add("2");

			_eventSink.ExpectedTabPageAdded = 3;

			_eventSink.AddExpectedTabPageAddedIndex(0);
			_eventSink.AddExpectedTabPageAddedIndex(1);
			_eventSink.AddExpectedTabPageAddedIndex(2);

			_eventSink.AddExpectedTabPageAddedObject(tabPage);
			_eventSink.AddExpectedTabPageAddedObject(tabPage1);
			_eventSink.AddExpectedTabPageAddedObject(tabPage2);

			for (int i = 0; i < _tabControl.TabPages.Count; i++)
			{
				Assert.AreEqual(i.ToString(), _tabControl.TabButtons[i].Text);
			}

			_eventSink.Verify();
		}

		[Test]
		public void CloseButtonOnTabTest()
		{
			Assert.IsTrue(_tabControl.CloseButtonOnTab);

			_tabControl.TabPages.Add("");
			_tabControl.TabPages.Add("");
			
			_tabControl.CloseButtonOnTab = false;

			_tabControl.TabPages.Add("");

			foreach (NuGenTabButton tabButton in _tabControl.TabButtons)
			{
				Assert.IsFalse(tabButton.ShowCloseButton);
			}

			_tabControl.CloseButtonOnTab = true;

			foreach (NuGenTabButton tabButton in _tabControl.TabButtons)
			{
				Assert.IsTrue(tabButton.ShowCloseButton);
			}
		}

		[Test]
		public void ControlRemoveTest()
		{
			NuGenTabPage tabPage = _tabControl.TabPages.Add("");
			NuGenTabPage tabPage2 = _tabControl.TabPages.Add("");

			_tabControl.Controls.Remove(tabPage2);

			Assert.AreEqual(1, _tabControl.TabPages.Count);
			Assert.AreEqual(1, _tabControl.TabButtons.Count);
			Assert.AreEqual(tabPage, _tabControl.TabPages[0]);
		}

		[Test]
		public void EnabledTest()
		{
			NuGenTabPage tabPage = _tabControl.TabPages.Add("");
			NuGenTabPage tabPage2 = _tabControl.TabPages.Add("");

			_tabControl.Enabled = false;

			NuGenTabPage tabPage3 = _tabControl.TabPages.Add("");

			foreach (NuGenTabButton tabButton in _tabControl.TabButtons)
			{
				Assert.IsFalse(tabButton.Enabled);
			}
		}

		[Test]
		public void InsertTabButtonTest()
		{
			_tabControl.TabPages.Insert(0, "2");
			_tabControl.TabPages.Insert(0, "1");
			_tabControl.TabPages.Insert(0, "0");

			NuGenTabButton tabButton = _tabControl.TabButtons[0];
			NuGenTabButton tabButton2 = _tabControl.TabButtons[1];
			NuGenTabButton tabButton3 = _tabControl.TabButtons[2];

			for (int i = 0; i < _tabControl.TabPages.Count; i++)
			{
				Assert.AreEqual(i.ToString(), _tabControl.TabButtons[i].Text);
			}
		}

		[Test]
		public void RemoveAllTest()
		{
			NuGenTabPage tabPage = _tabControl.TabPages.Add("");
			NuGenTabPage tabPage2 = _tabControl.TabPages.Add("");

			_eventSink.ExpectedTabPageRemoved = 2;

			_eventSink.AddExpectedTabPageRemovedIndex(1);
			_eventSink.AddExpectedTabPageRemovedIndex(0);

			_eventSink.AddExpectedTabPageRemovedObject(tabPage2);
			_eventSink.AddExpectedTabPageRemovedObject(tabPage);

			_tabControl.TabPages.Remove(tabPage2);
			Assert.AreEqual(1, _tabControl.TabButtons.Count);
			Assert.IsNotNull(_tabControl.SelectedTabButton);

			_tabControl.TabPages.Remove(tabPage);
			Assert.AreEqual(0, _tabControl.TabButtons.Count);
			Assert.IsNull(_tabControl.SelectedTabButton);

			_eventSink.Verify();
		}

		[Test]
		public void SelectTabTest()
		{
			NuGenTabPage tabPage = _tabControl.TabPages.Add("0");
			NuGenTabPage tabPage2 = _tabControl.TabPages.Add("1");

			Assert.AreEqual(tabPage2, _tabControl.SelectedTab);
			Assert.AreEqual(1, _tabControl.SelectedIndex);
			Assert.AreEqual("1", _tabControl.TabButtons[1].Text);
			Assert.IsTrue(_tabControl.TabButtons[1].Selected);
			Assert.IsFalse(_tabControl.TabButtons[0].Selected);

			_tabControl.SelectTab(tabPage);
			Assert.AreEqual(tabPage, _tabControl.SelectedTab);
			Assert.AreEqual(0, _tabControl.SelectedIndex);
			Assert.AreEqual("0", _tabControl.TabButtons[0].Text);
			Assert.IsFalse(_tabControl.TabButtons[1].Selected);
			Assert.IsTrue(_tabControl.TabButtons[0].Selected);

			_tabControl.SelectedTab = tabPage2;
			Assert.AreEqual(tabPage2, _tabControl.SelectedTab);

			_tabControl.SelectedIndex = 0;
			Assert.AreEqual(tabPage, _tabControl.SelectedTab);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void SelectedTabArgumentExceptionTest()
		{
			_tabControl.SelectedTab = new NuGenTabPage(new NuGenTabControlServiceProvider());
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void SelectedTabArgumentOutOfRangeExceptionTest()
		{
			_tabControl.SelectedIndex = -1;
		}

		[Test]
		public void TabCountTest()
		{
			Assert.AreEqual(0, _tabControl.TabCount);
			
			_tabControl.TabPages.Add("0");
			Assert.AreEqual(1, _tabControl.TabCount);

			_tabControl.TabPages.Add("1");
			Assert.AreEqual(2, _tabControl.TabCount);
		}

		[Test]
		public void TabPageEnabledTest()
		{
			NuGenTabPage tabPage = _tabControl.TabPages.Add("0");
			NuGenTabPage tabPage2 = _tabControl.TabPages.Add("1");
			NuGenTabPage tabPage3 = _tabControl.TabPages.Add("2");

			Assert.IsTrue(_tabControl.TabButtons[1].Enabled);
			tabPage2.Enabled = false;
			Assert.IsFalse(_tabControl.TabButtons[1].Enabled);
		}

		[Test]
		public void TabPageImageTest()
		{
			Image img = new Bitmap(1, 1);
			
			NuGenTabPage tabPage = _tabControl.TabPages.Add("0");
			tabPage.TabButtonImage = img;
			
			Assert.AreSame(img, _tabControl.TabButtons[0].Image);
		}
	}
}
