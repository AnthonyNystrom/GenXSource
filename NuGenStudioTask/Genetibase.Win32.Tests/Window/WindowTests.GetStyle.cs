/* -----------------------------------------------
 * WindowTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Genetibase.WinApi.Tests
{
	partial class WindowTests
	{
		[Test]
		public void GetStyleTest()
		{
			Assert.AreEqual(User32.GetWindowLong(_hWnd, WinUser.GWL_STYLE), Window.GetStyle(_hWnd));
		}

		[Test]
		[ExpectedException(typeof(NuGenInvalidHWndException))]
		public void GetStyleInvalidHWndTest()
		{
			Window.GetStyle(IntPtr.Zero);
		}
	}
}
