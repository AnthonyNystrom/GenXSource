/* -----------------------------------------------
 * NuGenButtonStateTrackerTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenButtonStateTrackerTests
	{
		private NuGenButtonStateTracker _stateTracker = null;
		private Control _ctrl = null;
		private Control _ctrl2 = null;

		[SetUp]
		public void SetUp()
		{
			_stateTracker = new NuGenButtonStateTracker();
			_ctrl = new Control();
			_ctrl2 = new Control();
		}

		[Test]
		public void StatesTest()
		{
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseEnter(_ctrl);
			Assert.AreEqual(NuGenControlState.Hot, _stateTracker.GetControlState(_ctrl));

			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState(_ctrl2));

			_stateTracker.MouseLeave(_ctrl);
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseEnter(_ctrl);
			_stateTracker.MouseDown(_ctrl);
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseUp(_ctrl);
			Assert.AreEqual(NuGenControlState.Hot, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseLeave(_ctrl);
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState(_ctrl));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetControlStateArgumentNullExceptionTest()
		{
			_stateTracker.GetControlState(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MouseDownArgumentNullExceptionTest()
		{
			_stateTracker.MouseDown(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MouseEnterArgumentNullExceptionTest()
		{
			_stateTracker.MouseEnter(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MouseLeaveArgumentNullExceptionTest()
		{
			_stateTracker.MouseLeave(null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void MouseUpArgumentNullExceptionTest()
		{
			_stateTracker.MouseUp(null);
		}
	}
}
