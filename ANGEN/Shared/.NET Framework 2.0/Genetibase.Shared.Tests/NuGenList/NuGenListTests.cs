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
	[TestFixture]
	public partial class NuGenListTests
	{
		private NuGenList<int> _list = null;
		private ListEventSink<int> _eventSink = null;

		[SetUp]
		public void SetUp()
		{
			_list = new NuGenList<int>();
			_eventSink = new ListEventSink<int>(_list);
		}

		[Test]
		public void AddRangeTest()
		{
			_eventSink.ExpectedAddedInvokeCount = 3;

			int[] values = new int[] { 1, 2, 3 };
			_list.AddRange(values);
			Assert.AreEqual(3, _list.Count);

			for (int i = 0; i < values.Length; i++)
			{
				Assert.AreEqual(values[i], _list[i]);
			}

			_eventSink.Verify();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddRangeArgumentNullExceptionTest()
		{
			_list.AddRange(null);
		}

		[Test]
		public void ClearTest()
		{
			_list.Add(1);
			_list.Add(2);

			Assert.AreEqual(2, _list.Count);

			_list.Clear();

			Assert.AreEqual(0, _list.Count);
		}

		[Test]
		public void EnumeratorTest()
		{
			int[] values = new int[] { 1, 2, 3 };

			for (int i = 0; i < values.Length; i++)
			{
				_list.Add(values[i]);
			}

			int j = 0;

			foreach (int item in _list)
			{
				Assert.AreEqual(values[j++], item);
			}
		}

		[Test]
		public void IndexerTest()
		{
			int[] values = new int[] { 1, 2, 3 };

			for (int i = 0; i < values.Length; i++)
			{
				_list.Add(values[i]);
			}

			for (int i = 0; i < values.Length; i++)
			{
				Assert.AreEqual(values[i], _list[i]);
			}
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IndexerArgumentOutOfRangeExceptionTest()
		{
			int i = _list[1];
		}

		[Test]
		public void InsertTest()
		{
			_eventSink.ExpectedInsertedInvokeCount = 2;
			_eventSink.AddExpectedInsertedIndex(0);
			_eventSink.AddExpectedInsertedIndex(0);

			_list.Insert(0, 5);
			Assert.AreEqual(1, _list.Count);

			_list.Insert(0, 6);
			Assert.AreEqual(2, _list.Count);

			_eventSink.Verify();
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void InsertArgumentOutOfRangeExceptionTest()
		{
			_list.Insert(2, 5);
		}

		[Test]
		public void RemoveTest()
		{
			Assert.AreEqual(0, _list.Count);

			_eventSink.ExpectedAddedInvokeCount = 3;
			_eventSink.ExpectedRemovedInvokeCount = 3;

			_eventSink.AddExpectedAddedItems(4);
			_eventSink.AddExpectedAddedItems(5);
			_eventSink.AddExpectedAddedItems(6);

			_eventSink.AddExpectedRemovedItems(6);
			_eventSink.AddExpectedRemovedItems(4);
			_eventSink.AddExpectedRemovedItems(5);

			_eventSink.AddExpectedAddedIndex(0);
			_eventSink.AddExpectedAddedIndex(1);
			_eventSink.AddExpectedAddedIndex(2);

			_eventSink.AddExpectedRemovedIndex(2);
			_eventSink.AddExpectedRemovedIndex(0);
			_eventSink.AddExpectedRemovedIndex(0);

			_list.Add(4);
			_list.Add(5);
			_list.Add(6);

			Assert.AreEqual(3, _list.Count);

			_list.Remove(6);
			Assert.AreEqual(2, _list.Count);

			_list.Remove(4);
			Assert.AreEqual(1, _list.Count);

			_list.Remove(5);
			Assert.AreEqual(0, _list.Count);

			_list.Remove(7);
			Assert.AreEqual(0, _list.Count);

			_eventSink.Verify();
		}
	}
}
