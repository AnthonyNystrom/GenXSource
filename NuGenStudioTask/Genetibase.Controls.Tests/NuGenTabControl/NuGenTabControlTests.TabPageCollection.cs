/* -----------------------------------------------
 * NuGenTabControlTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.Controls.Tests
{
	partial class NuGenTabControlTests
	{
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			NuGenTabControl.TabPageCollection collection = new NuGenTabControl.TabPageCollection(null);
		}

		[Test]
		public void AddTest()
		{
			string tabPageText = "abc";

			NuGenTabPage tabPage = _collection.Add(tabPageText);

			Assert.AreEqual(tabPageText, tabPage.Text);
			Assert.AreEqual(1, _collection.Count);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddArgumentNullExceptionOnTabPageTest()
		{
			_collection.Add((NuGenTabPage)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddArgumentNullExceptionOnTextTest()
		{
			_collection.Add((string)null);
		}

		[Test]
		public void ClearTest()
		{
			_collection.Add("");
			_collection.Add("");

			Assert.AreEqual(2, _collection.Count);

			_collection.Clear();

			Assert.AreEqual(0, _collection.Count);
		}

		[Test]
		public void IndexOfTest()
		{
			NuGenTabPage tabPage = _collection.Insert(0, "");
			NuGenTabPage tabPage2 = _collection.Insert(0, "");

			Assert.AreEqual(0, _collection.IndexOf(tabPage2));
			Assert.AreEqual(1, _collection.IndexOf(tabPage));
			Assert.AreEqual(-1, _collection.IndexOf(null));
		}

		[Test]
		public void IndexerTest()
		{
			NuGenTabPage tabPage = _collection.Insert(0, "");
			Assert.AreEqual(tabPage, _collection[0]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void IndexerArgumentOutOfRangeExceptionTest()
		{
			NuGenTabPage tabPage = _collection[0];
		}

		[Test]
		public void InsertTest()
		{
			NuGenTabPage tabPage = _collection.Insert(0, "");
			NuGenTabPage tabPage2 = _collection.Insert(0, "");

			Assert.AreEqual(2, _collection.Count);
			Assert.AreEqual(tabPage, _collection[1]);
			Assert.AreEqual(tabPage2, _collection[0]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InsertArgumentNullExceptionOnTabPageTest()
		{
			_collection.Insert(0, (NuGenTabPage)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InsertArgumentNullExceptionOnTextTest()
		{
			_collection.Insert(0, (string)null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void InsertArgumentOutOfRangeExceptionTest()
		{
			_collection.Insert(-1, "");
		}

		[Test]
		public void RemoveTest()
		{
			NuGenTabPage tabPage = _collection.Add("");
			NuGenTabPage tabPage2 = _collection.Add("");

			Assert.AreEqual(2, _collection.Count);

			_collection.Remove(tabPage);
			Assert.AreEqual(1, _collection.Count);

			_collection.Remove(tabPage2);
			Assert.AreEqual(0, _collection.Count);
		}
	}
}
