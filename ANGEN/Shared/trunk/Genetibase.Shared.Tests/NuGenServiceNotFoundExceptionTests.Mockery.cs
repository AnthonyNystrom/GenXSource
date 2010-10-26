/* -----------------------------------------------
 * NuGenServiceNotFoundExceptionTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	partial class NuGenServiceNotFoundExceptionTests
	{
		private interface IServiceNotExist
		{
		}

		private class ServiceFounder
		{
			public void ThrowException()
			{
				throw new NuGenServiceNotFoundException<IServiceNotExist>();
			}
		}
	}
}
