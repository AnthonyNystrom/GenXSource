/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class LoupePanel : Panel
		{
			public event EventHandler<ModeEventArgs> ModeChanged;

			private void OnModeChanged(ModeEventArgs e)
			{
				if (this.ModeChanged != null)
				{
					this.ModeChanged(this, e);
				}
			}

			private Image _selectedImage;

			public Image SelectedImage
			{
				get
				{
					return _selectedImage;
				}
				set
				{
					if (value == null)
					{
						throw new ArgumentNullException("value");
					}

					_selectedImage = value;
					this.SetSelectedSlide(_slideManager.GetSlideFromImage(_selectedImage));
				}
			}

			public void RotateSelectedImage90CW()
			{
				if (_selectedSlide != null)
				{
					_selectedSlide.RotateImage90CW();
				}
			}

			public void RotateSelectedImage90CCW()
			{
				if (_selectedSlide != null)
				{
					_selectedSlide.RotateImage90CCW();
				}
			}

			public void ZoomInSelectedImage()
			{
				if (_selectedSlide != null)
				{
					_selectedSlide.ZoomIn();
				}
			}

			public void ZoomOutSelectedImage()
			{
				if (_selectedSlide != null)
				{
					_selectedSlide.ZoomOut();
				}
			}

			protected override void OnControlAdded(ControlEventArgs e)
			{
				base.OnControlAdded(e);
				NuGenSlide slide = e.Control as NuGenSlide;

				if (slide != null)
				{
					slide.Dock = DockStyle.Fill;

					if (_selectedSlide == null)
					{
						_selectedSlide = slide;
					}
				}
			}

			private NuGenSlide _selectedSlide;

			private void SetSelectedSlide(NuGenSlide slideToSelect)
			{
				Debug.Assert(slideToSelect != null, "slideToSelect != null");
				slideToSelect.BringToFront();
				_selectedSlide = slideToSelect;
			}

			private SlideManager _slideManager;

			public LoupePanel(INuGenServiceProvider serviceProvider, ImageTracker imageTracker)
			{
				_slideManager = new SlideManager(this.Controls, imageTracker);

				this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
				this.BackColor = Color.Transparent;
				this.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (_slideManager != null)
					{
						_slideManager.Dispose();
						_slideManager = null;
					}
				}

				base.Dispose(disposing);
			}
		}
	}
}
