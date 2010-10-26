/* -----------------------------------------------
 * NuGenControlImageDescriptorTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenControlImageDescriptorTests
	{
		private NuGenControlImageDescriptor _descriptor;
		private DescriptorEventSink _eventSink;
		private Bitmap _sampleBmp;
		private ImageList _sampleImageList;

		[SetUp]
		public void SetUp()
		{
			_descriptor = new NuGenControlImageDescriptor();
			_eventSink = new DescriptorEventSink(_descriptor);
			_sampleBmp = new Bitmap(1, 1);
			_sampleImageList = new ImageList();
			_sampleImageList.Images.Add(_sampleBmp);
		}

		[TearDown]
		public void TearDown()
		{
			_eventSink.Verify();
			_sampleBmp.Dispose();
		}

		[Test]
		public void BasicStateTest()
		{
			Assert.IsNull(_descriptor.Image);
			Assert.IsNull(_descriptor.ImageList);
			Assert.AreEqual(-1, _descriptor.ImageIndex);
		}

		[Test]
		public void ImageTest()
		{
			_eventSink.ExpectedImageChangedCount = 1;

			_descriptor.ImageList = _sampleImageList;
			_sampleImageList.Images.Add(_sampleBmp);
			Assert.IsNull(_descriptor.Image);
			_descriptor.ImageIndex = 0;
			Assert.IsNotNull(_descriptor.Image);

			_descriptor.Image = null;
			Assert.IsNotNull(_descriptor.Image);

			_descriptor.Image = _sampleBmp;
			Assert.AreEqual(-1, _descriptor.ImageIndex);
			Assert.IsNull(_descriptor.ImageList);

			_descriptor.Image = _sampleBmp;
		}

		[Test]
		public void ImageIndexTest()
		{
			_eventSink.ExpectedImageIndexChangedCount = 2;

			_descriptor.Image = _sampleBmp;
			_descriptor.ImageIndex = -1;
			Assert.IsNotNull(_descriptor.Image);

			_descriptor.ImageIndex = 0;
			Assert.IsNull(_descriptor.Image);

			_descriptor.ImageIndex = 25;
			Assert.AreEqual(25, _descriptor.ImageIndex);

			_descriptor.ImageList = _sampleImageList;
			Assert.AreEqual(0, _descriptor.ImageIndex);

			_descriptor.ImageList.Images.Clear();
			Assert.AreEqual(-1, _descriptor.ImageIndex);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void ImageIndexArgumentExceptionTest()
		{
			_descriptor.ImageIndex = -2;
		}

		[Test]
		public void ImageListDisposeTest()
		{
			_descriptor.ImageList = _sampleImageList;
			_sampleImageList.Dispose();
			Assert.IsNull(_descriptor.ImageList);
		}

		[Test]
		public void ImageListChangedTest()
		{
			_descriptor.Image = _sampleBmp;
			Assert.IsNotNull(_descriptor.Image);
			_eventSink.ExpectedImageListChangedCount = 1;
			_descriptor.ImageList = _sampleImageList;
			Assert.IsNull(_descriptor.Image);
		}
	}
}
