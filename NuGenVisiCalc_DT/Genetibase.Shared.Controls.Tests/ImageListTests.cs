/* -----------------------------------------------
 * ImageListTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.Tests
{
	[TestFixture]
	public class ImageListTests
	{
		private ImageList _imageList;

		[SetUp]
		public void SetUp()
		{
			_imageList = new ImageList();
			_imageList.Images.Add(new Bitmap(16, 16));
			_imageList.Images.Add(new Bitmap(16, 16));
		}

		[Test]
		public void ImagesArgumentOutOfRangeExceptionTest()
		{
			Image image = null;

			try
			{
				image = _imageList.Images[-1];
				Assert.Fail();
			}
			catch (ArgumentOutOfRangeException)
			{
			}

			try
			{
				image = _imageList.Images[3];
				Assert.Fail();
			}
			catch (ArgumentOutOfRangeException)
			{
			}
		}
	}
}
