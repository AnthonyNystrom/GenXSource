/* -----------------------------------------------
 * NuGenControlPaintTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;
using Genetibase.WinApi;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	partial class NuGenControlPaintTests
	{
		private static readonly Point _mouseLocation = new Point(25, 36);

		[Test]
		public void BuildMouseEventArgsTest()
		{
			IntPtr wParam = (IntPtr)WinUser.MK_LBUTTON;
			IntPtr lParam = Common.MakeLParam(_mouseLocation.X, _mouseLocation.Y);

			MouseEventArgs e = NuGenControlPaint.BuildMouseEventArgs(wParam, lParam);
			Assert.AreEqual(MouseButtons.Left, e.Button);
			Assert.AreEqual(1, e.Clicks);
			this.AssertMouseLocation(_mouseLocation, e);

			wParam = (IntPtr)WinUser.MK_MBUTTON;
			e = NuGenControlPaint.BuildMouseEventArgs(wParam, lParam);
			Assert.AreEqual(MouseButtons.Middle, e.Button);
			Assert.AreEqual(1, e.Clicks);
			this.AssertMouseLocation(_mouseLocation, e);

			wParam = (IntPtr)WinUser.MK_RBUTTON;
			e = NuGenControlPaint.BuildMouseEventArgs(wParam, lParam);
			Assert.AreEqual(MouseButtons.Right, e.Button);
			Assert.AreEqual(1, e.Clicks);
			this.AssertMouseLocation(_mouseLocation, e);

			wParam = (IntPtr)WinUser.MK_XBUTTON1;
			e = NuGenControlPaint.BuildMouseEventArgs(wParam, lParam);
			Assert.AreEqual(MouseButtons.XButton1, e.Button);
			Assert.AreEqual(1, e.Clicks);
			this.AssertMouseLocation(_mouseLocation, e);

			wParam = (IntPtr)WinUser.MK_XBUTTON2;
			e = NuGenControlPaint.BuildMouseEventArgs(wParam, lParam);
			Assert.AreEqual(MouseButtons.XButton2, e.Button);
			Assert.AreEqual(1, e.Clicks);
			this.AssertMouseLocation(_mouseLocation, e);

			wParam = (IntPtr)(WinUser.MK_RBUTTON | WinUser.MK_LBUTTON);
			e = NuGenControlPaint.BuildMouseEventArgs(wParam, lParam);
			Assert.AreEqual(MouseButtons.Right | MouseButtons.Left, e.Button);
			Assert.AreEqual(1, e.Clicks);
			this.AssertMouseLocation(_mouseLocation, e);

			wParam = IntPtr.Zero;
			e = NuGenControlPaint.BuildMouseEventArgs(wParam, lParam, 0);
			Assert.AreEqual(MouseButtons.None, e.Button);
			Assert.AreEqual(0, e.Clicks);
			this.AssertMouseLocation(_mouseLocation, e);
		}

		private void AssertMouseLocation(Point mouseLocation, MouseEventArgs e)
		{
			Assert.AreEqual(mouseLocation, e.Location);
		}
	}
}
