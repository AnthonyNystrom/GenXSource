/* -----------------------------------------------
 * NuGenTabStateTrackerTests.cs
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
	public class NuGenTabStateTrackerTests
	{
		private NuGenTabStateTracker _stateTracker = null;

		[SetUp]
		public void SetUp()
		{
			_stateTracker = new NuGenTabStateTracker();
		}

		[Test]
		public void StateTests()
		{
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState());

			_stateTracker.MouseEnter();
			Assert.AreEqual(NuGenControlState.Hot, _stateTracker.GetControlState());

			_stateTracker.MouseLeave();
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState());

			_stateTracker.MouseEnter();
			_stateTracker.Select();
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState());

			_stateTracker.MouseLeave();
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState());

			_stateTracker.MouseEnter();
			Assert.AreEqual(NuGenControlState.Pressed, _stateTracker.GetControlState());

			_stateTracker.MouseLeave();
			_stateTracker.Deselect();
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState());
		}
	}
}
