/* -----------------------------------------------
 * TypeLoaderTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Genetibase.NuGenVisiCalc.Types;
using NUnit.Framework;

namespace Genetibase.NuGenVisiCalc.Tests
{
	[TestFixture]
	public class TypeLoaderTests
	{
		[TypeAttribute("Dumb")]
		private class DumbType
		{
		}

		[TypeAttribute("Double")]
		private abstract class DoubleType : NodeBase
		{
		}

		[TypeAttribute("Boolean")]
		private class BooleanType : NodeBase
		{
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadTypesFromAssemblyArgumentNullExceptionTest()
		{
			TypeLoader.LoadTypesFromAssembly(null);
		}

		[Test]
		public void LoadTypesFromAssemblyTest()
		{
			IList<TypeDescriptor> types = TypeLoader.LoadTypesFromAssembly(Assembly.GetExecutingAssembly());
			LoadTypesFromAssemblyTestInternal(types);
		}

		private void LoadTypesFromAssemblyTestInternal(IList<TypeDescriptor> types)
		{
			Assert.AreEqual(1, types.Count);
			Assert.AreEqual("Boolean", types[0].TypeName);
		}
	}
}
