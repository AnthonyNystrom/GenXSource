/* -----------------------------------------------
 * ParamLoaderTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Genetibase.NuGenVisiCalc.Params;
using NUnit.Framework;

namespace Genetibase.NuGenVisiCalc.Tests
{
	[TestFixture]
	public class ParamLoaderTests
	{
		[Param("Dumb")]
		private class DumbParameter
		{
		}

		[Param("Abstract")]
		private abstract class AbstractParam : NodeBase
		{
		}

		[Param("Boolean")]
		private class BooleanParam : NodeBase
		{
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadParamsFromAssemblyArgumentNullExceptionTest()
		{
			ParamLoader.LoadParamsFromAssembly(null);
		}

		[Test]
		public void LoadParamsFromAssemblyTest()
		{
			IList<ParamDescriptor> parameters = ParamLoader.LoadParamsFromAssembly(Assembly.GetExecutingAssembly());
			LoadParamsFromAssemblyTestInternal(parameters);
		}

		private void LoadParamsFromAssemblyTestInternal(IList<ParamDescriptor> parameters)
		{
			Assert.AreEqual(1, parameters.Count);
			Assert.AreEqual("Boolean", parameters[0].ParamName);
		}
	}
}
