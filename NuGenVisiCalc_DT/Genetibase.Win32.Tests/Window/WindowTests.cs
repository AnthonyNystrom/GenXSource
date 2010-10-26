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
	[TestFixture]
	public partial class WindowTests
	{
		private Form _form = null;
		private IntPtr _hWnd = IntPtr.Zero;

		private FormWindowState _maximizedState = FormWindowState.Maximized;
		private FormWindowState _minimizedState = FormWindowState.Minimized;
		private FormWindowState _normalState = FormWindowState.Normal;

		private FormBorderStyle _fixed3D = FormBorderStyle.Fixed3D;
		private FormBorderStyle _fixedDialog = FormBorderStyle.FixedDialog;
		private FormBorderStyle _fixedSingle = FormBorderStyle.FixedSingle;
		private FormBorderStyle _fixedToolWindow = FormBorderStyle.FixedToolWindow;
		private FormBorderStyle _none = FormBorderStyle.None;
		private FormBorderStyle _sizable = FormBorderStyle.Sizable;
		private FormBorderStyle _sizableToolWindow = FormBorderStyle.SizableToolWindow;

		private RightToLeft _ltr = RightToLeft.No;
		private RightToLeft _rtl = RightToLeft.Yes;

		[SetUp]
		public void SetUp()
		{
			_form = new Form();
			_form.Visible = true;
			_hWnd = _form.Handle;
		}

		[TearDown]
		public void TearDown()
		{
			_form.Dispose();
		}
	}
}
