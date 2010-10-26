/* -----------------------------------------------
 * NuGenImageListServiceTests.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Drawing;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public partial class NuGenImageListServiceTests
	{
		private ImageList _imageList = null;
		private NuGenImageListService _imageListService = null;
		private Image _bmp = null;
		private string _bmpKey = "Default BMP";

		[SetUp]
		public void SetUp()
		{
			_imageList = new ImageList();
			_imageListService = new NuGenImageListService();
			_bmp = new Bitmap(1, 1);
		}

		[TearDown]
		public void TearDown()
		{
			_bmp.Dispose();
		}

		[Test]
		public void AddImagesTest()
		{
			_imageListService.AddImages(
				_imageList,
				new NuGenImageDescriptor[]
				{
					new NuGenImageDescriptor(_bmp, _bmpKey)
				}
			);

			Assert.AreEqual(1, _imageList.Images.Count);
			Assert.IsNotNull(_imageList.Images[_bmpKey]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddImagesArgumentNullExceptionTest()
		{
			_imageListService.AddImages(_imageList, null);
		}

		[Test]
		public void AddImageTest()
		{
			_imageListService.AddImage(_imageList, _bmp, _bmpKey);

			Assert.AreEqual(1, _imageList.Images.Count);
			Assert.IsNotNull(_imageList.Images[_bmpKey]);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddImageArgumentNullExceptionOnImageTest()
		{
			_imageListService.AddImage(_imageList, null, "");
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddImageArgumentNullExceptionOnImageListTest()
		{
			_imageListService.AddImage(null, _bmp, "");
		}

		[Test]
		public void FindImageAtIndexTest()
		{
			_imageList.Images.Add(_bmp);
			_imageList.Images.Add(_bmp);

			Image image = _imageListService.FindImageAtIndex(_imageList, -1);
			Assert.IsNull(image);

			image = _imageListService.FindImageAtIndex(_imageList, 0);
			Assert.IsNotNull(image);

			image = _imageListService.FindImageAtIndex(_imageList, 3);
			Assert.IsNull(image);
		}

		[Test]
		public void FindImageAtIndexArgumentNullExceptionTest()
		{
			Assert.IsNull(_imageListService.FindImageAtIndex(null, 0));
		}

		[Test]
		public void GetImageIndexTest()
		{
			_imageList.Images.Add(_bmpKey, _bmp);
			Assert.AreEqual(0, _imageListService.GetImageIndex(_imageList, _bmpKey));
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void GetImageIndexArgumentNullExceptionImageListTest()
		{
			_imageListService.GetImageIndex(null, "");
		}
	}
}
