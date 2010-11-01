/* -----------------------------------------------
 * NuGenResolutionBlockTests.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExport;

using NUnit.Framework;

using System;
using System.Drawing;

namespace Genetibase.ApplicationBlocks.Tests
{
	[TestFixture]
	public class NuGenResolutionBlockTests
	{
		private NuGenResolutionBlock resolutionBlock = null;

		[SetUp]
		public void SetUp()
		{
			this.resolutionBlock = new NuGenResolutionBlock();
		}

		[Test]
		public void SetResolutionFromImageTest()
		{
			this.resolutionBlock.SetResolutionFromImage(new Bitmap(640, 480));
			Assert.AreEqual(new Size(640, 480), this.resolutionBlock.Resolution);
		}

		[Test]
		public void SetResolutionFromImageEnabledFalseTest()
		{
			this.resolutionBlock.SetResolutionFromImage(null);
			Assert.AreEqual(false, this.resolutionBlock.Enabled);
		}
	}
}
