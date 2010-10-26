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
	[TestFixture]
	public partial class NuGenServiceNotFoundExceptionTests
	{
		private ServiceFounder _serviceFounder = null;
		private ServiceFounderExceptionSink _exceptionSink = null;
		
		[SetUp]
		public void SetUp()
		{
			_serviceFounder = new ServiceFounder();
			_exceptionSink = new ServiceFounderExceptionSink(_serviceFounder);
		}

		[Test]
		public void ExceptionTest()
		{
			_exceptionSink.ExpectedServiceNotFoundExceptionCount = 1;
			_exceptionSink.ExpectedServiceNotFoundExceptionTCount = 1;

			_exceptionSink.Run();
			_exceptionSink.RunT();

			_exceptionSink.Verify();
		}
	}
}
