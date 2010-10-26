/* -----------------------------------------------
 * NuGenArgument.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows;
using System.Windows.Controls;
using Genetibase.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[TestMethod]
		public void IsCompatibleTypeInterfaceTest()
		{
			MockDataTarget dataTarget = new MockDataTarget();
			Assert.IsTrue(NuGenArgument.IsCompatibleType(dataTarget, typeof(IMockDataTarget)));
			Assert.IsFalse(NuGenArgument.IsCompatibleType(dataTarget, typeof(IDataObject)));
		}

		[TestMethod]
		public void IsCompatibleTypeEqualTypeTest()
		{
			Assert.IsTrue(NuGenArgument.IsCompatibleType(new Button(), typeof(Button)));
			Assert.IsFalse(NuGenArgument.IsCompatibleType(new TextBox(), typeof(Button)));
			Assert.IsFalse(NuGenArgument.IsCompatibleType(null, typeof(Control)));
		}

		[TestMethod]
		public void IsCompatibleTypeInheritenceTest()
		{
			Assert.IsTrue(NuGenArgument.IsCompatibleType(new Button(), typeof(Control)));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsCompatibleTypeArgumentNullExceptionTestOnCompatibleType()
		{
			NuGenArgument.IsCompatibleType(new Button(), null);
		}
	}
}
