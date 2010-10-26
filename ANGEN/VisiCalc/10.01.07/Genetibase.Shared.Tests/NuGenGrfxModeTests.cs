/* -----------------------------------------------
 * NuGenGrfxModeTests.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Genetibase.Shared.Tests
{
	[TestFixture]
	public class NuGenGrfxModeTests
	{
		private Image _dummyImage;
		private Graphics _grfx;

		[SetUp]
		public void SetUp()
		{
			_dummyImage = new Bitmap(1, 1);
			_grfx = Graphics.FromImage(_dummyImage);
		}

		[TearDown]
		public void TearDown()
		{
			_dummyImage.Dispose();
		}

		[Test]
		public void PixelOffsetModeTest()
		{
			_grfx.SmoothingMode = SmoothingMode.AntiAlias;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(_grfx))
			{
				_grfx.PixelOffsetMode = PixelOffsetMode.HighSpeed;
			}

			Assert.AreEqual(SmoothingMode.AntiAlias, _grfx.SmoothingMode);
		}

		[Test]
		public void PixelOffsetModeEmptyBlockTest()
		{
			_grfx.PixelOffsetMode = PixelOffsetMode.HighQuality;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(_grfx))
			{
			}

			Assert.AreEqual(PixelOffsetMode.HighQuality, _grfx.PixelOffsetMode);
		}

		[Test]
		public void SmoothingModeTest()
		{
			_grfx.SmoothingMode = SmoothingMode.AntiAlias;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(_grfx))
			{
				_grfx.SmoothingMode = SmoothingMode.None;
			}

			Assert.AreEqual(SmoothingMode.AntiAlias, _grfx.SmoothingMode);
		}

		[Test]
		public void SmoothingModeEmptyTest()
		{
			_grfx.SmoothingMode = SmoothingMode.AntiAlias;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(_grfx))
			{
			}

			Assert.AreEqual(SmoothingMode.AntiAlias, _grfx.SmoothingMode);
		}

		[Test]
		public void TextRenderingHintTest()
		{
			_grfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(_grfx))
			{
				_grfx.TextRenderingHint = TextRenderingHint.SystemDefault;
			}

			Assert.AreEqual(TextRenderingHint.ClearTypeGridFit, _grfx.TextRenderingHint);
		}

		[Test]
		public void TextRenderingHintEmptyTest()
		{
			_grfx.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			using (NuGenGrfxMode mode = new NuGenGrfxMode(_grfx))
			{
			}

			Assert.AreEqual(TextRenderingHint.ClearTypeGridFit, _grfx.TextRenderingHint);
		}
	}
}
