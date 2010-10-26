/* -----------------------------------------------
 * NuGenMessageFilterTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenMessageFilterTests
	{
		private MockMessageFilter _msgFilter = null;
		private MessageFilterWmSink _wmSink = null;
		private Message _m;

		private const int WM_LBUTTONDOWN = 0x0201;

		[SetUp]
		public void SetUp()
		{
			_msgFilter = new MockMessageFilter();
			_wmSink = new MessageFilterWmSink(_msgFilter);

			_m = new Message();
			_m.Msg = WM_LBUTTONDOWN;
		}

		[Test]
		public void AddWmHandlerTest()
		{
			_wmSink.ExpectedOnLButtonDownInvokeCount = 1;
			_msgFilter.InvokeWndProc(ref _m);
			_wmSink.Verify();
		}
	}
}
