/* -----------------------------------------------
 * NuGenWmHandlerListTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenWmHandlerListTests
	{
		private void BaseWndProc(ref Message m)
		{
		}

		private void StubWmHandler(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
		}

		private void StubWmHandler2(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
		}

		private class WmSink : MockObject
		{
			#region Properties.Public

			private ExpectationCounter _wmHandlerInvokeCount = new ExpectationCounter("WmHandlerInvokeCount");

			public int ExpectedWmHandlerInvokeCount
			{
				set
				{
					_wmHandlerInvokeCount.Expected = value;
				}
			}

			private ExpectationCounter _wmHandler2InvokeCount = new ExpectationCounter("WmHandler2InvokeCount");

			public int ExpectedWmHandler2InvokeCount
			{
				set
				{
					_wmHandler2InvokeCount.Expected = value;
				}
			}

			#endregion

			#region WmHandlers

			protected void WmHandler(ref Message m, NuGenWndProcDelegate baseWndProc)
			{
				_wmHandlerInvokeCount.Inc();
			}

			protected void WmHandler2(ref Message m, NuGenWndProcDelegate baseWndProc)
			{
				_wmHandler2InvokeCount.Inc();
			}

			#endregion

			#region Constructor

			public WmSink(NuGenWmHandlerList wmHandlerList)
			{
				if (wmHandlerList == null)
				{
					Assert.Fail("wmHandlerList cannot be null.");
				}

				wmHandlerList.AddWmHandler(WM_LBUTTONDOWN, this.WmHandler);
				wmHandlerList.AddWmHandler(WM_LBUTTONDOWN, this.WmHandler2);
			}

			#endregion
		}

		private class WmSinkRemove : WmSink
		{
			public WmSinkRemove(NuGenWmHandlerList wmHandlerList)
				: base(wmHandlerList)
			{
				if (wmHandlerList == null)
				{
					Assert.Fail("wmHandlerList cannot be null.");
				}

				wmHandlerList.RemoveWmHandler(WM_LBUTTONDOWN, this.WmHandler);
			}
		}
	}
}
