/* -----------------------------------------------
 * NuGenHotKeysTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenHotKeysTests
	{
		private DummyHandler _handler;

		[SetUp]
		public void SetUp()
		{
			_handler = new DummyHandler();
		}

		[TearDown]
		public void TearDown()
		{
			_handler.Verify();
		}

		[Test]
		public void ProcessTest()
		{
			_handler.ExpectedCutCount = 1;
			_handler.ExpectedEraseCount = 1;

			KeyEventArgs e = new KeyEventArgs(Keys.Control | Keys.X);
			Assert.IsTrue(e.Control);
			_handler.InvokeKeyDown(e);
			_handler.InvokeKeyDown(new KeyEventArgs(Keys.E));
			_handler.InvokeKeyDown(new KeyEventArgs(Keys.C));
			_handler.InvokeKeyDown(new KeyEventArgs(Keys.Alt | Keys.E));
		}
	}
}
