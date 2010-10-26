/* -----------------------------------------------
 * NuGenThumbnailContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared;
using Genetibase.Shared.Controls;
using Genetibase.Shared.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnailContainer
	{
		private sealed class GridPanel : FlowLayoutPanel
		{
			public event EventHandler<ModeEventArgs> ModeChanged;

			private void OnModeChanged(ModeEventArgs e)
			{
				if (this.ModeChanged != null)
				{
					this.ModeChanged(this, e);
				}
			}

			public IList<Image> SelectedImages
			{
				get
				{
					return _selectionService.SelectedImages;
				}
			}

			private NuGenPositiveInt32 _thumbnailSizeInternal;

			private NuGenPositiveInt32 ThumbnailSizeInternal
			{
				get
				{
					if (_thumbnailSizeInternal == null)
					{
						_thumbnailSizeInternal = new NuGenPositiveInt32();
						_thumbnailSizeInternal.Value = this.DefaultThumbnailSize;
					}

					return _thumbnailSizeInternal;
				}
			}

			public int ThumbnailSize
			{
				get
				{
					return this.ThumbnailSizeInternal.Value;
				}
				set
				{
					if (this.ThumbnailSizeInternal.Value != value)
					{
						this.ThumbnailSizeInternal.Value = value;
						this.OnThumbnailSizeChanged(EventArgs.Empty);
						this.RebuildLayout();
					}
				}
			}

			internal int DefaultThumbnailSize
			{
				get
				{
					return 150;
				}
			}

			public event EventHandler ThumbnailSizeChanged;

			private void OnThumbnailSizeChanged(EventArgs e)
			{
				if (this.ThumbnailSizeChanged != null)
				{
					this.ThumbnailSizeChanged(this, e);
				}
			}

			public void RebuildLayout()
			{
				int preferredThumbnailSize = this.ThumbnailSize;
				int width = this.DisplayRectangle.Width;
				int thumbnailCount = Math.Max(1, width / preferredThumbnailSize);

				int actualThumbnailSize = Math.Min(
					width - _thumbnailMargin * 2
					, width / thumbnailCount - _thumbnailMargin * (thumbnailCount + 1)
				);

				Size thumbnailSize = new Size(actualThumbnailSize, actualThumbnailSize);

				foreach (Control ctrl in this.Controls)
				{
					NuGenThumbnail thumbnail = ctrl as NuGenThumbnail;

					if (thumbnail != null)
					{
						thumbnail.Size = thumbnailSize;
					}
				}

				this.RefreshThumbnailNums();
			}

			protected override void OnClick(EventArgs e)
			{
				base.OnClick(e);
				_selectionService.ClearSelection();
				this.RefreshThumbnailSelection();
			}

			protected override void OnControlAdded(ControlEventArgs e)
			{
				base.OnControlAdded(e);
				NuGenThumbnail thumbnail = e.Control as NuGenThumbnail;

				if (thumbnail != null)
				{
					thumbnail.Rotate90CWButtonClick += _thumbnail_Rotate90CWButtonClick;
					thumbnail.Rotate90CCWButtonClick += _thumbnail_Rotate90CCWButtonClick;
					thumbnail.DoubleClick += _thumbnail_DoubleClick;
					thumbnail.MouseDown += _thumbnail_MouseDown;
				}
			}

			protected override void OnControlRemoved(ControlEventArgs e)
			{
				base.OnControlRemoved(e);
				NuGenThumbnail thumbnail = e.Control as NuGenThumbnail;

				if (thumbnail != null)
				{
					thumbnail.Rotate90CWButtonClick -= _thumbnail_Rotate90CWButtonClick;
					thumbnail.Rotate90CCWButtonClick -= _thumbnail_Rotate90CCWButtonClick;
					thumbnail.DoubleClick -= _thumbnail_DoubleClick;
					thumbnail.MouseDown -= _thumbnail_MouseDown;
				}
			}

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WinUser.WM_NCCALCSIZE)
				{
					User32.ShowScrollBar(m.HWnd, WinUser.SB_BOTH, false);
				}

				base.WndProc(ref m);
			}

			private void RefreshThumbnailSelection()
			{
				foreach (Image image in _thumbnailManager.Images)
				{
					NuGenThumbnail thumbnail = _thumbnailManager.GetThumbnailFromImage(image);
					thumbnail.Checked = false;
				}

				foreach (Image image in _selectionService.SelectedImages)
				{
					NuGenThumbnail selectedThumbnail = _thumbnailManager.GetThumbnailFromImage(image);
					selectedThumbnail.Checked = true;
				}
			}

			private void RefreshThumbnailNums()
			{
				for (int i = 0; i < _thumbnailManager.Images.Count; i++)
				{
					NuGenThumbnail currentThumb = _thumbnailManager.GetThumbnailFromImage(_thumbnailManager.Images[i]);
					currentThumb.Text = (i + 1).ToString(CultureInfo.CurrentUICulture);
				}
			}

			private void _thumbnail_Rotate90CWButtonClick(object sender, EventArgs e)
			{
				NuGenThumbnail thumbnail = (NuGenThumbnail)sender;

				if (_selectionService.SelectedImages.Contains(thumbnail.Image))
				{
					foreach (Image selectedImage in _selectionService.SelectedImages)
					{
						NuGenThumbnail selectedThumbnail = _thumbnailManager.GetThumbnailFromImage(selectedImage);
						selectedThumbnail.RotateImage90CW();
					}
				}
				else
				{
					thumbnail.RotateImage90CW();
				}
			}

			private void _thumbnail_Rotate90CCWButtonClick(object sender, EventArgs e)
			{
				NuGenThumbnail thumbnail = (NuGenThumbnail)sender;

				if (_selectionService.SelectedImages.Contains(thumbnail.Image))
				{
					foreach (Image selectedImage in _selectionService.SelectedImages)
					{
						NuGenThumbnail selectedThumbnail = _thumbnailManager.GetThumbnailFromImage(selectedImage);
						selectedThumbnail.RotateImage90CCW();
					}
				}
				else
				{
					thumbnail.RotateImage90CCW();
				}
			}

			private void _thumbnail_DoubleClick(object sender, EventArgs e)
			{
				this.OnModeChanged(new ModeEventArgs(NuGenThumbnailMode.LoupeView));
			}

			private void _thumbnail_MouseDown(object sender, MouseEventArgs e)
			{
				NuGenThumbnail activeThumbnail = (NuGenThumbnail)sender;
				_selectionService.AddImage(activeThumbnail.Image, Control.ModifierKeys, Control.MouseButtons);
				this.RefreshThumbnailSelection();
			}

			private ThumbnailManager _thumbnailManager;
			private ThumbnailSelectionService _selectionService;
			private const int _thumbnailMargin = 5;

			public GridPanel(INuGenServiceProvider serviceProvider, ImageTracker imageTracker)
			{
				Debug.Assert(imageTracker != null, "imageTracker != null");
				_thumbnailManager = new ThumbnailManager(
					serviceProvider
					, this.Controls
					, imageTracker
				);

				_selectionService = new ThumbnailSelectionService(_thumbnailManager.Images);

				this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
				
				this.AutoScroll = true;
				this.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
				this.BackColor = Color.Transparent;
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
				{
					if (_thumbnailManager != null)
					{
						_thumbnailManager.Dispose();
						_thumbnailManager = null;
					}
				}

				base.Dispose(disposing);
			}
		}
	}
}
