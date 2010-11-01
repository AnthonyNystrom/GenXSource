/* -----------------------------------------------
 * NuGenListTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.Collections;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.Shared.Tests
{
	partial class NuGenListTests
	{
		class ListEventSink<T> : MockObject
		{
			#region Expectations.Add

			/*
			 * AddedInvokeCount
			 */

			private ExpectationCounter _addedInvokeCount = new ExpectationCounter("addedInvokeCount");

			public int ExpectedAddedInvokeCount
			{
				set
				{
					_addedInvokeCount.Expected = value;
				}
			}

			/*
			 * AddedIndexList
			 */

			private ExpectationArrayList _addedIndexList = new ExpectationArrayList("addedIndexList");

			public void AddExpectedAddedIndex(int index)
			{
				_addedIndexList.AddExpected(index);
			}

			/*
			 * AddedItems
			 */

			private ExpectationArrayList _addedItems = new ExpectationArrayList("addedItems");

			public void AddExpectedAddedItems(T item)
			{
				_addedItems.AddExpected(item);
			}

			#endregion

			#region Expectations.Insert

			/*
			 * InsertInvokeCount
			 */

			private ExpectationCounter _insertedInvokeCount = new ExpectationCounter("insertedInvokeCount");

			public int ExpectedInsertedInvokeCount
			{
				set
				{
					_insertedInvokeCount.Expected = value;
				}
			}

			/*
			 * InsertedIndexList
			 */

			private ExpectationArrayList _insertedIndexList = new ExpectationArrayList("insertedIndexList");

			public void AddExpectedInsertedIndex(int index)
			{
				_insertedIndexList.AddExpected(index);
			}

			/*
			 * InsertedItems
			 */

			private ExpectationArrayList _insertedItems = new ExpectationArrayList("insertedItems");

			public void AddExpectedInsertedItems(T item)
			{
				_insertedItems.AddExpected(item);
			}

			#endregion

			#region Expectations.Remove

			/*
			 * RemovedInvokeCount
			 */

			private ExpectationCounter _removedInvokeCount = new ExpectationCounter("removedInvokeCount");

			public int ExpectedRemovedInvokeCount
			{
				set
				{
					_removedInvokeCount.Expected = value;
				}
			}

			/*
			 * RemovedIndexList
			 */

			private ExpectationArrayList _removedIndexList = new ExpectationArrayList("expectedIndexList");

			public void AddExpectedRemovedIndex(int index)
			{
				_removedIndexList.AddExpected(index);
			}

			/*
			 * RemovedItems
			 */

			private ExpectationArrayList _removedItems = new ExpectationArrayList("removedItems");

			public void AddExpectedRemovedItems(T item)
			{
				_removedItems.AddExpected(item);
			}

			#endregion

			#region Constructors

			public ListEventSink(NuGenList<T> list)
			{
				if (list == null)
				{
					Assert.Fail("list cannot be null.");
				}

				list.Added += delegate(object sender, NuGenCollectionEventArgs<T> e)
				{
					_addedInvokeCount.Inc();
					_addedItems.AddActual(e.Item);
					_addedIndexList.AddActual(e.Index);
				};

				list.Inserted += delegate(object sender, NuGenCollectionEventArgs<T> e)
				{
					_insertedInvokeCount.Inc();
					_insertedItems.AddActual(e.Item);
					_insertedIndexList.AddActual(e.Index);
				};

				list.Removed += delegate(object sender, NuGenCollectionEventArgs<T> e)
				{
					_removedInvokeCount.Inc();
					_removedItems.AddActual(e.Item);
					_removedIndexList.AddActual(e.Index);
				};
			}

			#endregion
		}
	}
}
