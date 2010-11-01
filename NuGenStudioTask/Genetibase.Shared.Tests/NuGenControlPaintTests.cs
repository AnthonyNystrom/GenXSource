/* -----------------------------------------------
 * NuGenControlPaintTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Drawing;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenControlPaintTests
	{
		private static readonly int _rectHeight = 100;
		private static readonly int _rectWidth = 100;
		private static readonly Size _imageSize = new Size(16, 16);
		private static readonly Rectangle _fitRectangle = new Rectangle(0, 0, _rectWidth, _rectHeight);

		[Test]
		public void ImageBoundsFromContentAlignmentBottomCenterTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.BottomCenter);
			
			Assert.AreEqual(_rectWidth / 2 - 8, imageBounds.Left);
			Assert.AreEqual(_rectHeight - 16, imageBounds.Top);
			Assert.AreEqual(_imageSize.Width, imageBounds.Width);
			Assert.AreEqual(_imageSize.Height, imageBounds.Height);
		}
		
		[Test]
		public void ImageBoundsFromContentAlignmentBottomLeftTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.BottomLeft);

			Assert.AreEqual(0, imageBounds.Left);
			Assert.AreEqual(_rectHeight - _imageSize.Height, imageBounds.Top);
		}

		[Test]
		public void ImageBoundsFromContentAlignmentBottomRightTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.BottomRight);

			Assert.AreEqual(_rectWidth - _imageSize.Width, imageBounds.Left);
			Assert.AreEqual(_rectWidth - _imageSize.Height, imageBounds.Top);
		}

		[Test]
		public void ImageBoundsFromContentAlignmentMiddleCenterTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.MiddleCenter);

			Assert.AreEqual(_rectWidth / 2 - _imageSize.Width / 2, imageBounds.Left);
			Assert.AreEqual(_rectHeight / 2 - _imageSize.Height / 2, imageBounds.Top);
		}

		[Test]
		public void ImageBoundsFromContentAlignmentMiddleLeftTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.MiddleLeft);

			Assert.AreEqual(0, imageBounds.Left);
			Assert.AreEqual(_rectHeight / 2 - _imageSize.Height / 2, imageBounds.Top);
		}

		[Test]
		public void ImageBoundsFromContentAlignmentMiddleRightTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.MiddleRight);

			Assert.AreEqual(_rectWidth - _imageSize.Width, imageBounds.Left);
			Assert.AreEqual(_rectHeight / 2 - _imageSize.Height / 2, imageBounds.Top);
		}

		[Test]
		public void ImageBoundsFromContentAlignmentTopCenterTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.TopCenter);

			Assert.AreEqual(_rectWidth / 2 - _imageSize.Width / 2, imageBounds.Left);
			Assert.AreEqual(0, imageBounds.Top);
		}

		[Test]
		public void ImageBoundsFromContentAlignmentTopLeftTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.TopLeft);

			Assert.AreEqual(0, imageBounds.Left);
			Assert.AreEqual(0, imageBounds.Top);
		}

		[Test]
		public void ImageBoundsFromContentAlignmentTopRightTest()
		{
			Rectangle imageBounds = NuGenControlPaint.ImageBoundsFromContentAlignment(_imageSize, _fitRectangle, ContentAlignment.TopRight);

			Assert.AreEqual(_rectWidth - _imageSize.Width, imageBounds.Left);
			Assert.AreEqual(0, imageBounds.Top);
		}
	}
}
