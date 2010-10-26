/* -----------------------------------------------
 * NuGenSlide.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	[System.ComponentModel.DesignerCategory("Code")]
	internal sealed class NuGenSlide : Panel
	{
		public Image Image
		{
			get
			{
				return _viewer.Image;
			}
			set
			{
				_viewer.Image = value;
			}
		}

		public void RotateImage90CW()
		{
			ImageRotator.RotateImage(this.Image, ImageRotationStyle.CW);
			this.Invalidate();
		}

		public void RotateImage90CCW()
		{
			ImageRotator.RotateImage(this.Image, ImageRotationStyle.CCW);
			this.Invalidate();
		}

		public void ZoomIn()
		{
			_viewer.ZoomIn();
		}

		public void ZoomOut()
		{
			_viewer.ZoomOut();
		}

		private SlideViewer _viewer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSlide"/> class.
		/// </summary>
		public NuGenSlide()
		{
			this.SetStyle(ControlStyles.Opaque, true);
			_viewer = new SlideViewer(this);
			_viewer.MinZoom = 0.1f;
			_viewer.MaxZoom = 3.0f;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_viewer != null)
				{
					_viewer.Dispose();
					_viewer = null;
				}
			}

			base.Dispose(disposing);
		}
	}
}
