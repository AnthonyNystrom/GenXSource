/* -----------------------------------------------
 * NuGenMessageFilterTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenMessageFilterTests
	{
		private class MockMessageFilter : NuGenMessageFilter
		{
			public void InvokeWndProc(ref Message m)
			{
				base.WndProc(ref m);
			}

			public MockMessageFilter()
			{
			}
		}

		private class MessageFilterWmSink : MockObject
		{
			private ExpectationCounter _onLButtonDownInvokeCount = new ExpectationCounter("OnLButtonDownInvokeCount");

			public int ExpectedOnLButtonDownInvokeCount
			{
				set
				{
					_onLButtonDownInvokeCount.Expected = value;
				}
			}

			private void OnLButtonDown(ref Message m, NuGenWndProcDelegate baseWndProc)
			{
				_onLButtonDownInvokeCount.Inc();
			}

			public MessageFilterWmSink(MockMessageFilter msgFilter)
			{
				if (msgFilter == null)
				{
					Assert.Fail("msgFilter cannot be null.");
				}

				msgFilter.MessageMap.AddWmHandler(WM_LBUTTONDOWN, this.OnLButtonDown);
			}
		}
	}
}
