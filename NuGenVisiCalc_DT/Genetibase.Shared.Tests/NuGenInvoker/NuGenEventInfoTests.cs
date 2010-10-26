/* -----------------------------------------------
 * NuGenEventInfoTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Reflection;

using NUnit.Framework;

using System;
using System.Reflection;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenEventInfoTests
	{
		private NuGenEventInfo _eventInfo;

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullExceptionTest()
		{
			_eventInfo = new NuGenEventInfo(null, new object());
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ConstructorArgumentNullException2Test()
		{
			EventInfo info = typeof(Control).GetEvent("Click");
			Assert.IsNotNull(info);

			_eventInfo = new NuGenEventInfo(info, null);
		}
	}
}
