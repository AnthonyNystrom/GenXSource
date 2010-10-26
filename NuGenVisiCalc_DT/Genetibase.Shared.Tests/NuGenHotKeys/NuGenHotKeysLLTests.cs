/* -----------------------------------------------
 * NuGenHotKeysLLTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenHotKeysLLTests
	{
		private NuGenHotKeysLL _hotKeysLL;
		private DummyHandler _handler;

		[SetUp]
		public void SetUp()
		{
			_handler = new DummyHandler();
			_hotKeysLL = new NuGenHotKeysLL();
		}

		[TearDown]
		public void TearDown()
		{
			_handler.Verify();
		}

		[Test]
		public void ProcessTest()
		{
			_handler.ExpectedCutHandlerCounter = 1;

			_hotKeysLL.Operations.Add(new NuGenHotKeyOperation("Cut", _handler.CutHandler, Keys.Control | Keys.X));

			_hotKeysLL.KeyDown(Keys.Control);
			_hotKeysLL.KeyDown(Keys.X);
			_hotKeysLL.KeyUp(Keys.X);
			_hotKeysLL.KeyUp(Keys.Control);
		}

		[Test]
		public void ProcessTest2()
		{
			_handler.ExpectedCopyHandlerCount = 1;
			_handler.ExpectedPasteHandlerCount = 1;

			_hotKeysLL.Operations.Add(new NuGenHotKeyOperation("Copy", _handler.CopyHandler, Keys.Control | Keys.C));
			_hotKeysLL.Operations.Add(new NuGenHotKeyOperation("Paste", _handler.PasteHandler, Keys.Control | Keys.V));

			_hotKeysLL.KeyDown(Keys.Control);
			_hotKeysLL.KeyDown(Keys.Shift);
			_hotKeysLL.KeyDown(Keys.C);
			_hotKeysLL.KeyUp(Keys.C);

			_hotKeysLL.KeyDown(Keys.Control);
			_hotKeysLL.KeyDown(Keys.C);
			_hotKeysLL.KeyUp(Keys.Control);

			_hotKeysLL.KeyDown(Keys.Control);
			_hotKeysLL.KeyDown(Keys.V);
			_hotKeysLL.KeyUp(Keys.V);
		}
	}
}
