/* -----------------------------------------------
 * AspectRatioTests.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.ApplicationBlocks.ImageExport;

using NUnit.Framework;

using System;
using System.Drawing;

namespace Genetibase.ApplicationBlocks.Tests
{
	[TestFixture]
	public partial class NuGenRatioSizeTrackerTests
	{
		[Test]
		public void SizeTrackerEmptyTest()
		{
			NuGenRatioSizeTracker sizeTracker = new NuGenRatioSizeTracker(Size.Empty);
			Assert.AreEqual(1.0, sizeTracker.Ratio);
			
			sizeTracker.Height = 0;
			sizeTracker.Width = 0;
			Assert.AreEqual(1.0, sizeTracker.Ratio);

			sizeTracker.Width = 5;
			Assert.AreEqual(1.0, sizeTracker.Ratio);
		}

		[Test]
		public void SizeTrackerNegativeTest()
		{
			NuGenRatioSizeTracker sizeTracker = new NuGenRatioSizeTracker(new Size(-5, -10));
			Assert.AreEqual(0.5, sizeTracker.Ratio);

			sizeTracker.Width = 20;
			Assert.AreEqual(40, sizeTracker.Height);
		}

		[Test]
		public void SizeTrackerMaintainAspectRatioTest()
		{
			NuGenRatioSizeTracker sizeTracker = new NuGenRatioSizeTracker(new Size(800, 600));

			Assert.AreEqual(800.0 / 600.0, sizeTracker.Ratio);

			sizeTracker.Height = 480;
			Assert.AreEqual(640, sizeTracker.Size.Width);

			sizeTracker.Width = 1280;
			Assert.AreEqual(960, sizeTracker.Size.Height);

			sizeTracker.Height = 240;
			Assert.AreEqual(320, sizeTracker.Size.Width);

			sizeTracker.Width = 100;
			Assert.AreEqual(75, sizeTracker.Size.Height);
		}

		[Test]
		public void SizeTrackerDoNotMaintainAspectRatioTest()
		{
			NuGenRatioSizeTracker sizeTracker = new NuGenRatioSizeTracker(new Size(800, 600), false);

			sizeTracker.Height = 480;
			Assert.AreEqual(800, sizeTracker.Width);

			sizeTracker.Width = 100;
			Assert.AreEqual(480, sizeTracker.Height);
		}

		[Test]
		public void SizeTrackerEventTest()
		{
			NuGenRatioSizeTracker sizeTracker = new NuGenRatioSizeTracker(new Size(800, 600));
			SizeTrackerEventSink eventSink = new SizeTrackerEventSink(sizeTracker);

			eventSink.ExpectedWidth = 640;
			eventSink.ExpectedHeight = 480;
			eventSink.ExpectedHeightChangedCallsCount = 1;
			eventSink.ExpectedWidthChangedCallsCount = 0;

			sizeTracker.Width = 640;
			Assert.AreEqual(new Size(640, 480), sizeTracker.Size);

			eventSink.Verify();
		}

		[Test]
		public void SizeTrackerDoNotMaintainAspectRatioEventTest()
		{
			NuGenRatioSizeTracker sizeTracker = new NuGenRatioSizeTracker(new Size(800, 600), false);
			SizeTrackerEventSink eventSink = new SizeTrackerEventSink(sizeTracker);

			eventSink.ExpectedHeightChangedCallsCount = 0;
			eventSink.ExpectedWidthChangedCallsCount = 0;

			sizeTracker.Height = 800;
			Assert.AreEqual(new Size(800, 800), sizeTracker.Size);

			sizeTracker.Width = 480;
			Assert.AreEqual(new Size(480, 800), sizeTracker.Size);

			eventSink.Verify();
		}
	}
}
