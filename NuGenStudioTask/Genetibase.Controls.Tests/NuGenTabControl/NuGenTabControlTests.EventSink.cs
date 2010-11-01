/* -----------------------------------------------
 * NuGenTabControlTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.Collections;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.Controls.Tests
{
	partial class NuGenTabControlTests
	{
		class EventSink : MockObject
		{
			#region Expectations

			/*
			 * SelectedIndexChanged
			 */

			private ExpectationCounter _selectedIndexChanged = new ExpectationCounter("selectedIndexChanged");

			public int ExpectedSelectedIndexChanged
			{
				set
				{
					_selectedIndexChanged.Expected = value;
				}
			}

			/*
			 * TabPageAdded
			 */

			private ExpectationCounter _tabPageAdded = new ExpectationCounter("tabPageAdded");

			public int ExpectedTabPageAdded
			{
				set
				{
					_tabPageAdded.Expected = value;
				}
			}

			private ExpectationArrayList _tabPageAddedObjectList = new ExpectationArrayList("tabPageAddedObjectList");

			public void AddExpectedTabPageAddedObject(NuGenTabPage tabPage)
			{
				_tabPageAddedObjectList.AddExpected(tabPage);
			}

			private ExpectationArrayList _tabPageAddedIndexList = new ExpectationArrayList("tabPageAddedIndexList");

			public void AddExpectedTabPageAddedIndex(int index)
			{
				_tabPageAddedIndexList.AddExpected(index);
			}

			/*
			 * TabPageRemoved
			 */

			private ExpectationCounter _tabPageRemoved = new ExpectationCounter("tabPageRemoved");

			public int ExpectedTabPageRemoved
			{
				set
				{
					_tabPageRemoved.Expected = value;
				}
			}

			private ExpectationArrayList _tabPageRemovedObjectList = new ExpectationArrayList("tabPageRemovedObjectList");

			public void AddExpectedTabPageRemovedObject(NuGenTabPage tabPage)
			{
				_tabPageRemovedObjectList.AddExpected(tabPage);
			}

			private ExpectationArrayList _tabPageRemovedIndexList = new ExpectationArrayList("tabPageRemovedIndexList");

			public void AddExpectedTabPageRemovedIndex(int index)
			{
				_tabPageRemovedIndexList.AddExpected(index);
			}

			#endregion

			#region Constructors

			public EventSink(NuGenTabControl tabControl)
			{
				if (tabControl == null)
				{
					Assert.Fail("tabControl cannot be null.");
				}

				tabControl.SelectedIndexChanged += delegate
				{
					_selectedIndexChanged.Inc();
				};

				tabControl.TabPageAdded += delegate(object sender, NuGenCollectionEventArgs<NuGenTabPage> e)
				{
					_tabPageAdded.Inc();
					_tabPageAddedObjectList.AddActual(e.Item);
					_tabPageAddedIndexList.AddActual(e.Index);
				};

				tabControl.TabPageRemoved += delegate(object sender, NuGenCollectionEventArgs<NuGenTabPage> e)
				{
					_tabPageRemoved.Inc();
					_tabPageRemovedObjectList.AddActual(e.Item);
					_tabPageRemovedIndexList.AddActual(e.Index);
				};
			}

			#endregion
		}
	}
}
