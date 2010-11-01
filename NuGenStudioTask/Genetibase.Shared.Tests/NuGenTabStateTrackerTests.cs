/* -----------------------------------------------
 * NuGenTabStateTrackerTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenTabStateTrackerTests
	{
		private Control _ctrl = null;
		private NuGenTabStateTracker _stateTracker = null;

		[SetUp]
		public void SetUp()
		{
			_ctrl = new Control();
			_stateTracker = new NuGenTabStateTracker();
		}

		[Test]
		public void StateTests()
		{
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseEnter(_ctrl);
			Assert.AreEqual(NuGenControlState.Hot, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseLeave(_ctrl);
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseEnter(_ctrl);
			_stateTracker.Select(_ctrl);
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseLeave(_ctrl);
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseEnter(_ctrl);
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState(_ctrl));

			_stateTracker.MouseLeave(_ctrl);
			_stateTracker.Deselect(_ctrl);
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState(_ctrl));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void DeselectArgumentNullExceptionTest()
		{
			_stateTracker.Deselect(null);
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
		public void SelectArgumentNullExceptionTest()
		{
			_stateTracker.Select(null);
		}
	}
}
