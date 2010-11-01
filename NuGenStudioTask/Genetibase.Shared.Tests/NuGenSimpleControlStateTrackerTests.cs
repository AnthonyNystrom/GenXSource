/* -----------------------------------------------
 * NuGenSimpleControlStateTracker.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenSimpleControlStateTrackerTests
	{
		private INuGenControlStateTracker _stateTracker = null;
		private Control _ctrl = null;

		[SetUp]
		public void SetUp()
		{
			_stateTracker = new NuGenSimpleControlStateTracker();
			_ctrl = new Control();
		}

		[Test]
		public void DisabledTests()
		{
			_ctrl.Enabled = true;
			_stateTracker.Enabled(_ctrl);
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState(_ctrl));

			_ctrl.Enabled = false;
			_stateTracker.Disabled(_ctrl);
			Assert.AreEqual(NuGenControlState.Disabled, _stateTracker.GetControlState(_ctrl));
		}
	}
}
