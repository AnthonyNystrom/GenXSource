/* -----------------------------------------------
 * NuGenTabItemStateTranslatorTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.TabControlInternals;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public class NuGenTabItemStateTranslatorTests
	{
		[Test]
		public void FromControlStateTest()
		{
			Assert.AreEqual(TabItemState.Disabled, this.GetTabItemState(NuGenControlState.Disabled));
			Assert.AreEqual(TabItemState.Hot, this.GetTabItemState(NuGenControlState.Hot));
			Assert.AreEqual(TabItemState.Selected, this.GetTabItemState(NuGenControlState.Pressed));
			Assert.AreEqual(TabItemState.Normal, this.GetTabItemState(NuGenControlState.Normal));
			Assert.AreEqual(TabItemState.Normal, this.GetTabItemState(NuGenControlState.Focused));
		}

		[Test]
		public void ToControlStateTest()
		{
			Assert.AreEqual(NuGenControlState.Disabled, this.GetControlState(TabItemState.Disabled));
			Assert.AreEqual(NuGenControlState.Hot, this.GetControlState(TabItemState.Hot));
			Assert.AreEqual(NuGenControlState.Normal, this.GetControlState(TabItemState.Normal));
			Assert.AreEqual(NuGenControlState.Pressed, this.GetControlState(TabItemState.Selected));
		}

		private TabItemState GetTabItemState(NuGenControlState ctrlState)
		{
			return NuGenTabItemStateTranslator.FromControlState(ctrlState);
		}

		private NuGenControlState GetControlState(TabItemState tabItemState)
		{
			return NuGenTabItemStateTranslator.ToControlState(tabItemState);
		}
	}
}
