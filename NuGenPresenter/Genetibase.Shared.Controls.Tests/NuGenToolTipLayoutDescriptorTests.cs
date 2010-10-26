/* -----------------------------------------------
 * NuGenToolTipLayoutDescriptorTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ToolTipInternals;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public class NuGenToolTipLayoutDescriptorTests
	{
		[Test]
		public void EqualTest()
		{
			Rectangle rect = new Rectangle(0, 0, 100, 100);
			Rectangle rect2 = new Rectangle(10, 10, 90, 90);
			Size size = new Size(20, 20);
			Size size2 = new Size(30, 30);
			NuGenToolTipLayoutDescriptor descriptor = new NuGenToolTipLayoutDescriptor();
			NuGenToolTipLayoutDescriptor descriptor2 = new NuGenToolTipLayoutDescriptor();
			descriptor.BevelBounds = descriptor2.BevelBounds = rect;
			descriptor.HeaderBounds = descriptor2.HeaderBounds = rect;
			descriptor.ImageBounds = descriptor2.ImageBounds = rect;
			descriptor.RemarksBounds = descriptor2.RemarksBounds = rect;
			descriptor.RemarksHeaderBounds = descriptor2.RemarksHeaderBounds = rect;
			descriptor.RemarksImageBounds = descriptor2.RemarksImageBounds = rect;
			descriptor.TextBounds = descriptor2.TextBounds = rect;
			descriptor.TooltipSize = descriptor2.TooltipSize = size;

			NuGenToolTipLayoutDescriptor descriptor3 = new NuGenToolTipLayoutDescriptor();
			descriptor3.BevelBounds = rect;
			descriptor3.HeaderBounds = rect;
			descriptor3.ImageBounds = rect2;
			descriptor3.RemarksBounds = rect2;
			descriptor3.RemarksHeaderBounds = rect;
			descriptor3.RemarksImageBounds = rect;
			descriptor3.TextBounds = rect;
			descriptor3.TooltipSize = size2;

			Assert.AreEqual(descriptor, descriptor2);
			Assert.IsTrue(descriptor == descriptor2);
			Assert.IsTrue(descriptor != descriptor3);
			Assert.AreNotEqual(descriptor, 25);
		}
	}
}
