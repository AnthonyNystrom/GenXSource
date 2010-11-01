/* -----------------------------------------------
 * NuGenArgumentTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenArgumentTests
	{
		private DataObject _dataObject = null;

		[SetUp]
		public void SetUp()
		{
			_dataObject = new DataObject(new Button());
		}

		[Test]
		public void GetCompatibleDataObjectTypeInterfaceTest()
		{
			MockDataTarget mockDataTarget = new MockDataTarget();
			
			Assert.AreEqual(
				typeof(MockDataTarget),
				NuGenArgument.GetCompatibleDataObjectType(new DataObject(mockDataTarget), typeof(IMockDataTarget))
			);

			Assert.IsNull(
				NuGenArgument.GetCompatibleDataObjectType(_dataObject, typeof(IMockObject))
			);
		}

		[Test]
		public void GetCompatibleDataObjectTypeEqualTypeTest()
		{
			Assert.AreEqual(
				typeof(Button),
				NuGenArgument.GetCompatibleDataObjectType(_dataObject, typeof(Button))
			);

			Assert.IsNull(
				NuGenArgument.GetCompatibleDataObjectType(_dataObject, typeof(TextBox))
			);
		}

		[Test]
		public void GetCompatibleDataObjectTypeInheritanceTest()
		{
			Assert.AreEqual(
				typeof(Button),
				NuGenArgument.GetCompatibleDataObjectType(_dataObject, typeof(Control))
			);

			Assert.IsNull(
				NuGenArgument.GetCompatibleDataObjectType(_dataObject, typeof(TextBox))
			);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetCompatibleDataObjectTypeArgumentNullExceptionOnDataObject()
		{
			NuGenArgument.GetCompatibleDataObjectType(null, typeof(IMockDataTarget));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetCompatibleDataObjectTypeArgumentNullExceptionOnCompatibleType()
		{
			NuGenArgument.GetCompatibleDataObjectType(_dataObject, null);
		}
	}
}
