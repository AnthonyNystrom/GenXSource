/* -----------------------------------------------
 * NuGenTypeDescriptorTests.cs
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
	public class NuGenTypeDescriptorTests
	{
		private NuGenObjectDescriptorTests.DummyClass _dummyClass;

		[SetUp]
		public void SetUp()
		{
			_dummyClass = new NuGenObjectDescriptorTests.DummyClass();
		}

		[Test]
		public void ObjectDescriptorTest()
		{
			Assert.AreEqual(0, _dummyClass.MyProperty);
			NuGenTypeDescriptor.Instance(_dummyClass).Properties["MyProperty"].SetValue(5);
			Assert.AreEqual(5, _dummyClass.MyProperty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void InstanceArgumentNullExceptionTest()
		{
			NuGenObjectDescriptor descriptor = NuGenTypeDescriptor.Instance(null);
		}
	}
}
