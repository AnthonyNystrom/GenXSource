/* -----------------------------------------------
 * NuGenEnumTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using NUnit.Framework;

using System;

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

		[Test]
		public void ToArrayTest()
		{
			DummyEnum[] elements = NuGenEnum.ToArray<DummyEnum>();
			Assert.AreEqual(3, elements.Length);
			Assert.AreEqual(DummyEnum.DummyElement, elements[0]);
			Assert.AreEqual(DummyEnum.DummyElement2, elements[1]);
			Assert.AreEqual(DummyEnum.DummyElement3, elements[2]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ToArrayNullTest()
		{
			int[] elements = NuGenEnum.ToArray<int>();
		}
	}
}
