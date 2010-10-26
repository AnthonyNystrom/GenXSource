/* -----------------------------------------------
 * NuGenServiceNotFoundExceptionTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Genetibase.Shared.Tests
{
	[TestClass]
	public partial class NuGenServiceNotFoundExceptionTests
	{
		private ServiceFounder _serviceFounder;
		
		[TestInitialize]
		public void SetUp()
		{
			_serviceFounder = new ServiceFounder();
		}

		[TestMethod]
		public void ExceptionTest()
		{
			try
			{
				_serviceFounder.ThrowException();
				Assert.Fail();
			}
			catch (NuGenServiceNotFoundException)
			{
			}

			try
			{
				_serviceFounder.ThrowException();
			}
			catch (NuGenServiceNotFoundException<IServiceNotExist>)
			{
				Assert.Fail();
			}
		}
	}
}
