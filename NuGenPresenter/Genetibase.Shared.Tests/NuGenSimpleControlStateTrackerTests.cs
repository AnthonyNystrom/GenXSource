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

		[SetUp]
		public void SetUp()
		{
			_stateTracker = new NuGenControlStateTracker();
		}

		[Test]
		public void DisabledTests()
		{
			_stateTracker.Enabled(true);
			Assert.AreEqual(NuGenControlState.Normal, _stateTracker.GetControlState());

			_stateTracker.Enabled(false);
			Assert.AreEqual(NuGenControlState.Disabled, _stateTracker.GetControlState());
		}
	}
}
