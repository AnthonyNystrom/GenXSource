/* -----------------------------------------------
 * NuGeHotKeysTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenHotKeysTests
	{
		private sealed class DummyHandler : MockObject
		{
			private ExpectationCounter _cutCount = new ExpectationCounter("cutCount");

			public int ExpectedCutCount
			{
				set
				{
					_cutCount.Expected = value;
				}
			}

			private ExpectationCounter _eraseCount = new ExpectationCounter("eraseCount");

			public int ExpectedEraseCount
			{
				set
				{
					_eraseCount.Expected = value;
				}
			}

			public void InvokeKeyDown(KeyEventArgs e)
			{
				_hotKeys.Process(e);
			}

			private void Cut()
			{
				_cutCount.Inc();
			}

			private void Erase()
			{
				_eraseCount.Inc();
			}

			private NuGenHotKeys _hotKeys;

			public DummyHandler()
			{
				_hotKeys = new NuGenHotKeys();

				NuGenHotKeyOperation cutOp = new NuGenHotKeyOperation("Cut", this.Cut, Keys.Control | Keys.X);
				NuGenHotKeyOperation eraseOp = new NuGenHotKeyOperation("Erase", this.Erase, Keys.E);

				_hotKeys.Operations.Add(cutOp);
				_hotKeys.Operations.Add(eraseOp);
			}
		}
	}
}
