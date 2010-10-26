/* -----------------------------------------------
 * NuGenWmHandlerList.cs
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
	[TestFixture]
	public partial class NuGenWmHandlerListTests
	{
		private NuGenWmHandlerList _wmHandlerList = null;
		private Message _m;

		private const int WM_LBUTTONDOWN = 0x0201;
        private const int WM_LBUTTONUP = 0x0202;

		[SetUp]
		public void SetUp()
		{
			_wmHandlerList = new NuGenWmHandlerList();

			_m = new Message();
			_m.HWnd = (IntPtr)0x0001;
			_m.Msg = WM_LBUTTONDOWN;
		}

		[Test]
		public void InvokeHandlerTest()
		{
			WmSink wmSink = new WmSink(_wmHandlerList);

			wmSink.ExpectedWmHandlerInvokeCount = 1;
			wmSink.ExpectedWmHandler2InvokeCount = 1;

			NuGenWmHandler wmHandler = _wmHandlerList[WM_LBUTTONDOWN];
			wmHandler(ref _m, this.BaseWndProc);
			
			wmSink.Verify();
		}

		[Test]
		public void InvokeHandlerAfterRemoveTest()
		{
			WmSinkRemove wmSink = new WmSinkRemove(_wmHandlerList);

			wmSink.ExpectedWmHandlerInvokeCount = 0;
			wmSink.ExpectedWmHandler2InvokeCount = 1;

			NuGenWmHandler wmHandler = _wmHandlerList[WM_LBUTTONDOWN];
			wmHandler(ref _m, this.BaseWndProc);

			wmSink.Verify();
		}

		[Test]
		public void AddWmHandlerTest()
		{
			_wmHandlerList.AddWmHandler(WM_LBUTTONDOWN, this.StubWmHandler);
			Assert.AreEqual(1, _wmHandlerList.Count);

			NuGenWmHandler wmHandler = _wmHandlerList[WM_LBUTTONDOWN];
			Assert.IsNotNull(wmHandler);

			_wmHandlerList.AddWmHandler(WM_LBUTTONDOWN, this.StubWmHandler2);
			Assert.AreEqual(1, _wmHandlerList.Count);
		}

        [Test]
        public void GetEnumeratorTest()
        {
            _wmHandlerList.AddWmHandler(WM_LBUTTONDOWN, this.StubWmHandler);
            _wmHandlerList.AddWmHandler(WM_LBUTTONUP, this.StubWmHandler2);

            NuGenWmHandlerList wmHandlerList2 = new NuGenWmHandlerList();

            wmHandlerList2.AddWmHandler(WM_LBUTTONDOWN, this.StubWmHandler);
            wmHandlerList2.AddWmHandler(WM_LBUTTONUP, this.StubWmHandler2);

            foreach (int wmId in _wmHandlerList)
            {
                Assert.AreEqual(wmHandlerList2[wmId], _wmHandlerList[wmId]);
            }
        }

		[Test]
		public void RemoveWmHandlerTest()
		{
			_wmHandlerList.AddWmHandler(WM_LBUTTONDOWN, this.StubWmHandler);
			_wmHandlerList.AddWmHandler(WM_LBUTTONDOWN, this.StubWmHandler2);
			Assert.AreEqual(1, _wmHandlerList.Count);

			_wmHandlerList.RemoveWmHandler(WM_LBUTTONDOWN, this.StubWmHandler);
			Assert.AreEqual(1, _wmHandlerList.Count);
		}

		[Test]
		public void RemoveWmHandlerNullTest()
		{
			_wmHandlerList.RemoveWmHandler(WM_LBUTTONDOWN, null);
		}

		[Test]
		public void IndexerKeyNotExistTest()
		{
			Assert.IsNull(_wmHandlerList[WM_LBUTTONDOWN]);
			_wmHandlerList[WM_LBUTTONDOWN] = this.StubWmHandler;
		}
	}
}
