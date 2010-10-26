/* -----------------------------------------------
 * NuGenObjectDescriptorTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenObjectDescriptorTests
	{
		private NuGenObjectDescriptor _objectDescriptor;
		private DummyClass _dummyClass;

		[SetUp]
		public void SetUp()
		{
			_dummyClass = new DummyClass();
			_objectDescriptor = new NuGenObjectDescriptor(_dummyClass);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorTest()
		{
			_objectDescriptor = new NuGenObjectDescriptor(null);
		}

		[Test]
		public void SetValueTest()
		{
			Assert.AreEqual(0, _dummyClass.MyProperty);
			_objectDescriptor.Properties["MyProperty"].SetValue(5);
			Assert.AreEqual(5, _dummyClass.MyProperty);
		}
	}
}
