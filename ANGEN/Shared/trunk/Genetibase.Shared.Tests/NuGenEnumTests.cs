/* -----------------------------------------------
 * NuGenEnumTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	[TestClass]
	public partial class NuGenEnumTests
	{
		enum SimpleEnum
		{
			SimpleElementNone,
			SimpleElementOne,
			SimpleElementTwo
		}

		[Flags]
		enum DefaultEnum
		{
			DefaultElementNone = 0x0000,
			DefaultElementOne = 0x0001,
			DefaultElementTwo = 0x0002,
			DefaultElementThree = 0x0004
		}

		[Flags]
		enum LongEnum : long
		{
			LongElementNone = 0x0000,
			LongElementOne = 0x0001,
			LongElementTwo = 0x0002,
			LongElementThree = 0x0004
		}

		[Flags]
		enum LongNegativeEnum : long
		{
			LongNegativeEnumNone = 0x0000,
			LongNegativeEnumOne = -0x0001,
			LongNegativeEnumTwo = -0x0002,
			LongNegativeEnumThree = -0x0004
		}
	}
}
