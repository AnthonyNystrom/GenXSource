/* -----------------------------------------------
 * NuGenWmHandlerAttributeTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenWmHandlerAttributeTests
	{
		private const int WM_LBUTTONDOWN = 0x0201;

		[Test]
		public void ConstructorTest()
		{
			NuGenWmHandlerAttribute wmHandlerAttribute = new NuGenWmHandlerAttribute(WM_LBUTTONDOWN);

			Assert.IsTrue(wmHandlerAttribute is Attribute);
			Assert.AreEqual(WM_LBUTTONDOWN, wmHandlerAttribute.WmId);
		}
	}
}
