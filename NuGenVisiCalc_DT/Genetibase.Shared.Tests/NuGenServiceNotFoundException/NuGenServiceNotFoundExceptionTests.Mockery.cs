/* -----------------------------------------------
 * NuGenServiceNotFoundExceptionTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		private class ServiceFounder
		{
			public void ThrowException()
			{
				throw new NuGenServiceNotFoundException<IAsserter>();
			}
		}

		private class ServiceFounderExceptionSink : MockObject
		{
			#region Declarations.Fields

			private ServiceFounder _serviceFounder = null;

			#endregion

			#region Properties.Public

			/*
			 * ServiceNotFoundExceptionCount
			 */

			private ExpectationCounter _serviceNotFoundExceptionCount = new ExpectationCounter("ServiceNotFoundExceptionCount");

			public int ExpectedServiceNotFoundExceptionCount
			{
				set
				{
					_serviceNotFoundExceptionCount.Expected = value;
				}
			}

			/*
			 * ServiceNotFoundExceptionTCount
			 */

			private ExpectationCounter _serviceNotFoundExceptionTCount = new ExpectationCounter("ServiceNotFoundExceptionTCount");

			public int ExpectedServiceNotFoundExceptionTCount
			{
				set
				{
					_serviceNotFoundExceptionTCount.Expected = value;
				}
			}

			#endregion

			#region Methods.Public

			public void Run()
			{
				try
				{
					_serviceFounder.ThrowException();
				}
				catch (NuGenServiceNotFoundException)
				{
					_serviceNotFoundExceptionCount.Inc();
				}
			}

			public void RunT()
			{
				try
				{
					_serviceFounder.ThrowException();
				}
				catch (NuGenServiceNotFoundException<IAsserter>)
				{
					_serviceNotFoundExceptionTCount.Inc();
				}
			}

			#endregion

			#region Constructors

			public ServiceFounderExceptionSink(ServiceFounder serviceFounder)
			{
				if (serviceFounder == null)
				{
					Assert.Fail("serviceFounder cannot be null.");
				}

				_serviceFounder = serviceFounder;
			}

			#endregion
		}
	}
}
