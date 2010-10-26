/* -----------------------------------------------
 * NuGenEventsTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenEventsTests
	{
		private NuGenEvents _events;

		[SetUp]
		public void SetUp()
		{
			_events = new NuGenEvents(new int());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			_events = new NuGenEvents(null);
		}

		[Test]
		[ExpectedException(typeof(NuGenEventNotFoundException))]
		public void EventNotFoundExceptionTest()
		{
			NuGenEventInfo eventInfo = _events["EventNotExist"];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentNullExceptionTest()
		{
			NuGenEventInfo eventInfo = _events[null];
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentNullException2Test()
		{
			NuGenEventInfo eventInfo = _events[""];
		}
	}
}
