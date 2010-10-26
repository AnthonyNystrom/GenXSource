/* -----------------------------------------------
 * NuGenPositiveInt32Tests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	[TestClass]
	public partial class NuGenPositiveInt32Tests
	{
		private NuGenPositiveInt32 _int;

		[TestInitialize]
		public void SetUp()
		{
			_int = new NuGenPositiveInt32();
		}

		[TestMethod]
		public void EqualsTest()
		{
			_eventSink.ExpectedValueChangedCount = 2;

			NuGenPositiveInt32 compared = new NuGenPositiveInt32();
			compared.Value = 20;

			_int.Value = 20;
			Assert.IsTrue(_int.Equals(compared));

			_int.Value = 30;
			Assert.IsFalse(_int.Equals(compared));
			Assert.IsFalse(_int.Equals(null));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ValueArgumentExceptionTest()
		{
			Int32 ValueChangedCalls = 0;

			_int.Value = -10;
			_int.ValueChanged += delegate
			{
				ValueChangedCalls++;
			};

			Assert.AreEqual(0, ValueChangedCalls);
		}
	}
}
