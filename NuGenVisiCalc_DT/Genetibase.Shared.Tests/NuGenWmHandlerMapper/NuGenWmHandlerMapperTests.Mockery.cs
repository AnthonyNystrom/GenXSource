/* -----------------------------------------------
 * NuGenWmHandlerMapperTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenWmHandlerMapperTests
	{
		private class MockMessageProcessor : INuGenMessageProcessor
		{
			private NuGenWmHandlerList _messageMap = null;

			public NuGenWmHandlerList MessageMap
			{
				get
				{
					if (_messageMap == null)
					{
						_messageMap = new NuGenWmHandlerList();
					}

					return _messageMap;
				}
			}

			[NuGenWmHandler(WM_PAINT)]
			protected virtual void OnWmPaint(ref Message m, NuGenWndProcDelegate baseWndProc)
			{
			}

			[NuGenWmHandler(WM_LBUTTONDOWN)]
			[NuGenWmHandler(WM_LBUTTONUP)]
			protected virtual void OnWmLButtonClick(ref Message m, NuGenWndProcDelegate baseWndProc)
			{
			}
		}

		private class BackMockMessageProcessor : MockMessageProcessor
		{
			[NuGenWmHandler(WM_LBUTTONUP)]
			protected virtual void OnWmLButtonUp()
			{
			}
		}
	}
}
