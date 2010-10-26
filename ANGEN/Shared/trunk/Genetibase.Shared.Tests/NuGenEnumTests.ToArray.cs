/* -----------------------------------------------
 * NuGenEnumTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using Genetibase.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenEnumTests
	{
		enum DummyEnum
		{
			DummyElement,
			DummyElement2,
			DummyElement3
		}

		[TestMethod]
		public void ToArrayTest()
		{
			DummyEnum[] elements = NuGenEnum.ToArray<DummyEnum>();
			Assert.AreEqual(3, elements.Length);
			Assert.AreEqual(DummyEnum.DummyElement, elements[0]);
			Assert.AreEqual(DummyEnum.DummyElement2, elements[1]);
			Assert.AreEqual(DummyEnum.DummyElement3, elements[2]);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ToArrayNullTest()
		{
			Int32[] elements = NuGenEnum.ToArray<Int32>();
		}
	}
}
