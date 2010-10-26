/* -----------------------------------------------
 * NuGenThumbnailSelectionServiceTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.Tests
{
	[TestFixture]
	public class NuGenThumbnailSelectionServiceTests
	{
		private ThumbnailSelectionService _selectionService;
		private Bitmap _bmp, _bmp2, _bmp3;

		private Bitmap CreateBitmap()
		{
			return new Bitmap(100, 100);
		}

		[SetUp]
		public void SetUp()
		{
			NuGenThumbnailContainer.ImageCollection imageCollection = new NuGenThumbnailContainer.ImageCollection(new NuGenThumbnailContainer.ImageTracker());
			_selectionService = new ThumbnailSelectionService(imageCollection);

			_bmp = this.CreateBitmap();
			_bmp2 = this.CreateBitmap();
			_bmp3 = this.CreateBitmap();

			imageCollection.AddRange(new Image[] { _bmp, _bmp2, _bmp3 });
		}

		[TearDown]
		public void TearDown()
		{
			_bmp.Dispose();
			_bmp2.Dispose();
			_bmp3.Dispose();
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void AddImageArgumentNullExceptionTest()
		{
			_selectionService.AddImage(null, Keys.None, MouseButtons.None);
		}

		[Test]
		public void ClearSelectionTest()
		{
			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Left);
			Assert.AreEqual(1, _selectionService.SelectedImages.Count);
			_selectionService.ClearSelection();
			Assert.AreEqual(0, _selectionService.SelectedImages.Count);
		}

		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CtorArgumentNullExceptionTest()
		{
			_selectionService = new ThumbnailSelectionService(null);
		}

		[Test]
		public void ControlSelectionTest()
		{
			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Left);
			_selectionService.AddImage(_bmp2, Keys.Control, MouseButtons.Left);
			Assert.AreEqual(2, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp, _selectionService.SelectedImages[0]);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[1]);
		}

		[Test]
		public void ControlSimpleSelectionTest()
		{
			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Left);
			_selectionService.AddImage(_bmp, Keys.Control, MouseButtons.Left);
			Assert.AreEqual(0, _selectionService.SelectedImages.Count);
		}

		[Test]
		public void SingleSelectionTest()
		{
			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Left);
			Assert.AreEqual(1, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp, _selectionService.SelectedImages[0]);

			_selectionService.AddImage(_bmp2, Keys.None, MouseButtons.Left);
			Assert.AreEqual(1, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[0]);
		}

		[Test]
		public void SingleSelectionRightButtonTest()
		{
			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Right);
			Assert.AreEqual(0, _selectionService.SelectedImages.Count);

			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Left);
			_selectionService.AddImage(_bmp2, Keys.None, MouseButtons.Right);
			Assert.AreEqual(1, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp, _selectionService.SelectedImages[0]);
		}

		[Test]
		public void ShiftLowerUpperIndexSelectionTest()
		{
			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Left);
			_selectionService.AddImage(_bmp3, Keys.Shift, MouseButtons.Left);
			Assert.AreEqual(3, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp, _selectionService.SelectedImages[0]);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[1]);
			Assert.AreEqual(_bmp3, _selectionService.SelectedImages[2]);
		}

		[Test]
		public void ShiftUpperLowerSelectionTest()
		{
			_selectionService.AddImage(_bmp3, Keys.None, MouseButtons.Left);
			_selectionService.AddImage(_bmp, Keys.Shift, MouseButtons.Left);
			Assert.AreEqual(3, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp, _selectionService.SelectedImages[0]);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[1]);
			Assert.AreEqual(_bmp3, _selectionService.SelectedImages[2]);
		}

		[Test]
		public void ShiftMixedSelectionTest()
		{
			_selectionService.AddImage(_bmp2, Keys.Shift, MouseButtons.Left);
			_selectionService.AddImage(_bmp3, Keys.Shift, MouseButtons.Left);
			Assert.AreEqual(2, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[0]);
			Assert.AreEqual(_bmp3, _selectionService.SelectedImages[1]);
			_selectionService.AddImage(_bmp, Keys.Shift, MouseButtons.Left);
			Assert.AreEqual(2, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp, _selectionService.SelectedImages[0]);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[1]);
		}

		[Test]
		public void ShiftControlSelectionTest()
		{
			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Left);
			_selectionService.AddImage(_bmp3, Keys.Shift, MouseButtons.Left);
			Assert.AreEqual(3, _selectionService.SelectedImages.Count);
			_selectionService.AddImage(_bmp, Keys.Control, MouseButtons.Left);
			Assert.AreEqual(2, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[0]);
			Assert.AreEqual(_bmp3, _selectionService.SelectedImages[1]);
			_selectionService.AddImage(_bmp, Keys.Shift, MouseButtons.Left);
			Assert.AreEqual(2, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp, _selectionService.SelectedImages[0]);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[1]);
			_selectionService.AddImage(_bmp3, Keys.Shift, MouseButtons.Left);
			Assert.AreEqual(2, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp2, _selectionService.SelectedImages[0]);
			Assert.AreEqual(_bmp3, _selectionService.SelectedImages[1]);
		}

		[Test]
		public void ShiftSimpleSelectionTest()
		{
			_selectionService.AddImage(_bmp, Keys.None, MouseButtons.Left);
			_selectionService.AddImage(_bmp, Keys.Shift, MouseButtons.Left);
			Assert.AreEqual(1, _selectionService.SelectedImages.Count);
			Assert.AreEqual(_bmp, _selectionService.SelectedImages[0]);
		}
	}
}
