/* -----------------------------------------------
 * NuGenToolTipInfoTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public class NuGenToolTipInfoTests
	{
		private NuGenToolTipInfo _tooltipInfo;

		[SetUp]
		public void SetUp()
		{
			_tooltipInfo = new NuGenToolTipInfo();
		}

		[Test]
		public void ConstructorTest()
		{
			Assert.IsNull(_tooltipInfo.Header);
			Assert.IsNull(_tooltipInfo.Image);
			Assert.IsNull(_tooltipInfo.Text);
			Assert.IsNull(_tooltipInfo.Remarks);
			Assert.IsNull(_tooltipInfo.RemarksHeader);
			Assert.IsNull(_tooltipInfo.RemarksImage);
			Assert.AreEqual(Size.Empty, _tooltipInfo.CustomSize);

			Assert.IsFalse(_tooltipInfo.IsHeaderVisible);
			Assert.IsFalse(_tooltipInfo.IsImageVisible);
			Assert.IsFalse(_tooltipInfo.IsRemarksHeaderVisible);
			Assert.IsFalse(_tooltipInfo.IsRemarksImageVisible);
			Assert.IsFalse(_tooltipInfo.IsRemarksVisible);
			Assert.IsFalse(_tooltipInfo.IsTextVisible);
			Assert.IsFalse(_tooltipInfo.IsCustomSize);
		}
	}
}
