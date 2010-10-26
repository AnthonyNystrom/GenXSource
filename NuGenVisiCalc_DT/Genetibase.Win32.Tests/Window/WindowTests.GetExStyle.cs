/* -----------------------------------------------
 * WindowTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi.Tests
{
	partial class WindowTests
	{
		[Test]
		public void GetExStyleTest()
		{
			Assert.AreEqual(User32.GetWindowLong(_hWnd, WinUser.GWL_EXSTYLE), Window.GetExStyle(_hWnd));
		}

		[Test]
		[ExpectedException(typeof(NuGenInvalidHWndException))]
		public void GetExStyleInvalidHWndTest()
		{
			Window.GetExStyle(IntPtr.Zero);
		}
	}
}
