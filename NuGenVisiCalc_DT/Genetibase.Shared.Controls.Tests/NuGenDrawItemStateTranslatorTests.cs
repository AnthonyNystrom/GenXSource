/* -----------------------------------------------
 * NuGenDrawItemStateTranslatorTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public class NuGenDrawItemStateTranslatorTests
	{
		[Test]
		public void ToControlStateTest()
		{
			Assert.AreEqual(NuGenControlState.Disabled, this.GetControlState(DrawItemState.Disabled));
			Assert.AreEqual(NuGenControlState.Disabled, this.GetControlState(DrawItemState.Grayed));
			Assert.AreEqual(NuGenControlState.Hot, this.GetControlState(DrawItemState.HotLight));
			Assert.AreEqual(NuGenControlState.Hot, this.GetControlState(DrawItemState.Inactive));
			Assert.AreEqual(NuGenControlState.Normal, this.GetControlState(DrawItemState.Checked));
			Assert.AreEqual(NuGenControlState.Normal, this.GetControlState(DrawItemState.ComboBoxEdit));
			Assert.AreEqual(NuGenControlState.Normal, this.GetControlState(DrawItemState.Default));
			Assert.AreEqual(NuGenControlState.Normal, this.GetControlState(DrawItemState.Focus));
			Assert.AreEqual(NuGenControlState.Normal, this.GetControlState(DrawItemState.NoAccelerator));
			Assert.AreEqual(NuGenControlState.Normal, this.GetControlState(DrawItemState.NoFocusRect));
			Assert.AreEqual(NuGenControlState.Normal, this.GetControlState(DrawItemState.None));
			Assert.AreEqual(NuGenControlState.Pressed, this.GetControlState(DrawItemState.Selected));
		}

		private NuGenControlState GetControlState(DrawItemState itemState)
		{
			return NuGenDrawItemStateTranslator.ToControlState(itemState);
		}
	}
}
