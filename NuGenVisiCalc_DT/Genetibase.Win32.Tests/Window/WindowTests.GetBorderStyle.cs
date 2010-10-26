/* -----------------------------------------------
 * WindowTests.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.WinApi.Tests
{
	partial class WindowTests
	{
		[Test]
		public void GetBorderStyleTests()
		{
			_form.FormBorderStyle = _none;
			Assert.AreEqual(_none, Window.GetBorderStyle(_hWnd));

			_form.FormBorderStyle = _fixed3D;
			Assert.AreEqual(_fixed3D, Window.GetBorderStyle(_hWnd));

			_form.FormBorderStyle = _fixedDialog;
			Assert.AreEqual(_fixedDialog, Window.GetBorderStyle(_hWnd));

			_form.FormBorderStyle = _sizable;
			Assert.AreEqual(_sizable, Window.GetBorderStyle(_hWnd));

			_form.FormBorderStyle = _sizableToolWindow;
			Assert.AreEqual(_sizableToolWindow, Window.GetBorderStyle(_hWnd));

			_form.FormBorderStyle = _fixedToolWindow;
			Assert.AreEqual(_fixedToolWindow, Window.GetBorderStyle(_hWnd));

			_form.FormBorderStyle = _fixedSingle;
			Assert.AreEqual(_fixedSingle, Window.GetBorderStyle(_hWnd));
		}

		[Test]
		[ExpectedException(typeof(NuGenInvalidHWndException))]
		public void GetBorderStyleInvalidHWndTest()
		{
			Window.GetBorderStyle(IntPtr.Zero);
		}
	}
}
