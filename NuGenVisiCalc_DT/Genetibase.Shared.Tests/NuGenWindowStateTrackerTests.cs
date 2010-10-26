/* -----------------------------------------------
 * NuGenFormStateTracker.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenWindowStateTrackerTests
	{
		private NuGenWindowStateTracker _stateTracker = null;
		private Form _form = null;
		private Form _form2 = null;

		[SetUp]
		public void SetUp()
		{
			_stateTracker = new NuGenWindowStateTracker();
			_form = new Form();
			_form2 = new Form();
		}

		[Test]
		public void WindowStateTest()
		{
			_form.WindowState = FormWindowState.Normal;
			_stateTracker.SetWindowState(_form);
			Assert.AreEqual(FormWindowState.Normal, _stateTracker.GetWindowState(_form));

			_form.WindowState = FormWindowState.Maximized;
			_stateTracker.SetWindowState(_form);
			Assert.AreEqual(FormWindowState.Maximized, _stateTracker.GetWindowState(_form));

			_form.WindowState = FormWindowState.Minimized;
			_stateTracker.SetWindowState(_form);
			Assert.AreEqual(FormWindowState.Maximized, _stateTracker.GetWindowState(_form));

			_form.WindowState = FormWindowState.Normal;
			_stateTracker.SetWindowState(_form);
			_form.WindowState = FormWindowState.Minimized;
			_stateTracker.SetWindowState(_form);
			Assert.AreEqual(FormWindowState.Normal, _stateTracker.GetWindowState(_form));

			Assert.AreEqual(FormWindowState.Normal, _stateTracker.GetWindowState(_form2));

			_form2.WindowState = FormWindowState.Maximized;
			_stateTracker.SetWindowState(_form2);
			Assert.AreEqual(FormWindowState.Maximized, _stateTracker.GetWindowState(_form2));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetWindowStateArgumentNullExceptionTest()
		{
			_stateTracker.GetWindowState(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void SetWindowStateArgumentNullExceptionTest()
		{
			_stateTracker.SetWindowState(null);
		}
	}
}
