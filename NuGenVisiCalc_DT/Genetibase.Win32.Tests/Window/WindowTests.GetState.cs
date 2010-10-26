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
		public void GetStateTest()
		{
			Assert.AreEqual(_normalState, Window.GetState(_hWnd));

			_form.WindowState = _maximizedState;
			Assert.AreEqual(_maximizedState, Window.GetState(_hWnd));

			_form.WindowState = _minimizedState;
			Assert.AreEqual(_minimizedState, Window.GetState(_hWnd));
		}

		[Test]
		[ExpectedException(typeof(NuGenInvalidHWndException))]
		public void GetStateInvalidHWndTest()
		{
			Assert.AreEqual(_normalState, Window.GetState(IntPtr.Zero));
		}
	}
}
