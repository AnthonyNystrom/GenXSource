/* -----------------------------------------------
 * NuGenControlImageManagerTests.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenControlImageManagerTests
	{
		private INuGenControlImageManager _manager;

		[SetUp]
		public void SetUp()
		{
			_manager = new NuGenControlImageManager();
		}

		[Test]
		public void IndexerTest()
		{
			INuGenControlImageDescriptor descriptor = _manager.CreateImageDescriptor();
			Assert.IsNotNull(descriptor);
			Assert.IsNull(descriptor.Image);
			Assert.AreEqual(-1, descriptor.ImageIndex);
			Assert.IsNull(descriptor.ImageList);
		}
	}
}
