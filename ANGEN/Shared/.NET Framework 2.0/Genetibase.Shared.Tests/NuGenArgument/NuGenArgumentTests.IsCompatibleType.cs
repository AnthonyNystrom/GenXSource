/* -----------------------------------------------
 * NuGenArgument.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		[Test]
		public void IsCompatibleTypeInterfaceTest()
		{
			MockDataTarget dataTarget = new MockDataTarget();
			Assert.IsTrue(NuGenArgument.IsCompatibleType(dataTarget, typeof(IMockDataTarget)));
			Assert.IsFalse(NuGenArgument.IsCompatibleType(dataTarget, typeof(IDataObject)));
		}

		[Test]
		public void IsCompatibleTypeEqualTypeTest()
		{
			Assert.IsTrue(NuGenArgument.IsCompatibleType(new Button(), typeof(Button)));
			Assert.IsFalse(NuGenArgument.IsCompatibleType(new TextBox(), typeof(Button)));
			Assert.IsFalse(NuGenArgument.IsCompatibleType(null, typeof(Control)));
		}

		[Test]
		public void IsCompatibleTypeInheritenceTest()
		{
			Assert.IsTrue(NuGenArgument.IsCompatibleType(new Button(), typeof(Control)));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void IsCompatibleTypeArgumentNullExceptionTestOnCompatibleType()
		{
			NuGenArgument.IsCompatibleType(new Button(), null);
		}
	}
}
