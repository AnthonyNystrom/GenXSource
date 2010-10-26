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

		[SetUp]
		public void SetUp()
		{
			_stateTracker = new NuGenButtonStateTracker();
		}

		[Test]
		public void DisabledStatesTest()
		{
			_stateTracker.Enabled(false);
			Assert.AreEqual(NuGenControlState.Disabled, _stateTracker.GetControlState());

			_stateTracker.Enabled(true);
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState());
		}

		[Test]
		public void FocusedStatesTest()
		{
			_stateTracker.GotFocus();
			Assert.AreEqual(NuGenControlState.Focused, _stateTracker.GetControlState());

			_stateTracker.MouseEnter();
			Assert.AreEqual(NuGenControlState.Hot, _stateTracker.GetControlState());

			_stateTracker.MouseDown();
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState());

			_stateTracker.MouseUp();
			_stateTracker.MouseLeave();
			Assert.AreEqual(NuGenControlState.Focused, _stateTracker.GetControlState());
		}

		[Test]
		public void StatesTest()
		{
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState());

			_stateTracker.MouseEnter();
			Assert.AreEqual(NuGenControlState.Hot, _stateTracker.GetControlState());

			_stateTracker.MouseLeave();
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState());

			_stateTracker.MouseEnter();
			_stateTracker.MouseDown();
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState());

			_stateTracker.MouseUp();
			Assert.AreEqual(NuGenControlState.Hot, _stateTracker.GetControlState());

			_stateTracker.MouseLeave();
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState());
		}
	}
}
