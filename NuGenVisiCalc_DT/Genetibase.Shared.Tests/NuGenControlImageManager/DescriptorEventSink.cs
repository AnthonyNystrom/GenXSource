/* -----------------------------------------------
 * DescriptorEventSink.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using DotNetMock;

using Genetibase.Shared.Drawing;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	class DescriptorEventSink : MockObject
	{
		private ExpectationCounter _imageChangedCount = new ExpectationCounter("imageChangedCount");

		public int ExpectedImageChangedCount
		{
			set
			{
				_imageChangedCount.Expected = value;
			}
		}

		private ExpectationCounter _imageIndexChangedCount = new ExpectationCounter("imageIndexChangedCount");

		public int ExpectedImageIndexChangedCount
		{
			set
			{
				_imageIndexChangedCount.Expected = value;
			}
		}

		private ExpectationCounter _imageListChangedCount = new ExpectationCounter("imageListChangedCount");

		public int ExpectedImageListChangedCount
		{
			set
			{
				_imageListChangedCount.Expected = value;
			}
		}

		public DescriptorEventSink(NuGenControlImageDescriptor descriptor)
		{
			Assert.IsNotNull(descriptor);

			descriptor.ImageChanged += delegate
			{
				_imageChangedCount.Inc();
			};

			descriptor.ImageIndexChanged += delegate
			{
				_imageIndexChangedCount.Inc();
			};

			descriptor.ImageListChanged += delegate
			{
				_imageListChangedCount.Inc();
			};
		}
	}
}
