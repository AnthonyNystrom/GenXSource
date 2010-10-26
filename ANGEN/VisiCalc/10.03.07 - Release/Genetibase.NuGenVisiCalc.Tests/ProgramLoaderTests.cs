/* -----------------------------------------------
 * ProgramLoaderTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.Programs;
using NUnit.Framework;
using System.Reflection;

namespace Genetibase.NuGenVisiCalc.Tests
{
	[TestFixture]
	public class ProgramLoaderTests
	{
		[Program("WrongProgram")]
		private class WrongProgram
		{
		}

		[Program("AbstractProgram")]
		private abstract class AbstractProgram : NodeBase
		{
		}

		[Program("Program")]
		private class Program : NodeBase
		{
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void LoadProgramFromAssemblyArgumentNullExceptionTest()
		{
			ProgramLoader.LoadProgramsFromAssembly(null);
		}

		[Test]
		public void LoadProgramsFromAssemblyTest()
		{
			IList<ProgramDescriptor> programs = ProgramLoader.LoadProgramsFromAssembly(Assembly.GetExecutingAssembly());
			LoadProgramsInternalTest(programs);
		}

		private void LoadProgramsInternalTest(IList<ProgramDescriptor> programs)
		{
			Assert.AreEqual(1, programs.Count);
			Assert.AreEqual("Program", programs[0].ProgramName);
		}
	}
}
