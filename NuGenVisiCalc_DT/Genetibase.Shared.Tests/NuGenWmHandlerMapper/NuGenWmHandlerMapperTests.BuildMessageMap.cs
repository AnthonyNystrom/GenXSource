/* -----------------------------------------------
 * NuGenWmHandlerMapperTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenWmHandlerMapperTests
	{
		[Test]
		public void BuildMessageMapTest()
		{
			NuGenWmHandlerList mappedHandlers = _handlerMapper.BuildMessageMap(_messageProcessor);
			Assert.AreEqual(3, mappedHandlers.Count);
		}

		[Test]
		[ExpectedException(typeof(NuGenWmHandlerSignatureException))]
		public void BuildMessageMapBadSignatureTest()
		{
			NuGenWmHandlerList mappedHandlers = _handlerMapper.BuildMessageMap(_badMessageProcessor);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void BuldMessageMapArgumentNullExceptionTest()
		{
			_handlerMapper.BuildMessageMap(null);
		}
	}
}
