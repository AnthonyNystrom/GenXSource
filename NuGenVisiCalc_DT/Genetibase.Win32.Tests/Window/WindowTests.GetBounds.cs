/* -----------------------------------------------
 * WindowTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Windows.Forms;

namespace Genetibase.WinApi.Tests
{
	partial class WindowTests
	{
		[Test]
		public void GetBoundsTest()
		{
			RECT rect = new RECT();
			User32.GetWindowRect(_hWnd, ref rect);

			Assert.AreEqual(rect, Window.GetBounds(_hWnd));
		}

		[Test]
		[ExpectedException(typeof(NuGenInvalidHWndException))]
		public void GetBoundsInvalidHWndTest()
		{
			Window.GetBounds(IntPtr.Zero);
		}
	}
}
