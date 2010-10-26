/* -----------------------------------------------
 * NuGenHotKeysLLTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	partial class NuGenHotKeysLLTests
	{
		private sealed class DummyHandler : MockObject
		{
			private ExpectationCounter _cutHandlerCounter = new ExpectationCounter("cutHandlerCounter");

			public int ExpectedCutHandlerCounter
			{
				set
				{
					_cutHandlerCounter.Expected = value;
				}
			}

			public int CutHandlerCounter
			{
				set
				{
					_cutHandlerCounter.Expected = value;
				}
			}

			public void CutHandler()
			{
				_cutHandlerCounter.Inc();
			}

			private ExpectationCounter _copyHandlerCounter = new ExpectationCounter("copyHandlerCounter");

			public int ExpectedCopyHandlerCount
			{
				set
				{
					_copyHandlerCounter.Expected = value;
				}
			}

			public int CopyHandlerCounter
			{
				set
				{
					_copyHandlerCounter.Expected = value;
				}
			}

			public void CopyHandler()
			{
				_copyHandlerCounter.Inc();
			}

			private ExpectationCounter _pasteHandlerCounter = new ExpectationCounter("pasteHandlerCounter");

			public int ExpectedPasteHandlerCount
			{
				set
				{
					_pasteHandlerCounter.Expected = value;
				}
			}

			public void PasteHandler()
			{
				_pasteHandlerCounter.Inc();
			}

			public DummyHandler()
			{
			}
		}
	}
}
