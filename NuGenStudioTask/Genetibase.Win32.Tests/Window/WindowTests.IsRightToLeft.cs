/* -----------------------------------------------
 * WindowTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;

namespace Genetibase.WinApi.Tests
{
	partial class WindowTests
	{
		[Test]
		public void IsRightToLeftTest()
		{
			_form.RightToLeft = _ltr;
			Assert.IsFalse(Window.IsRightToLeft(_hWnd));

			/* Note that window handle changes after this operation. */
			_form.RightToLeft = _rtl;
			Assert.IsTrue(Window.IsRightToLeft(_form.Handle));
		}

		[Test]
		[ExpectedException(typeof(NuGenInvalidHWndException))]
		public void IsRightToLeftInvalidHWndTest()
		{
			Window.IsRightToLeft(IntPtr.Zero);
		}
	}
}
