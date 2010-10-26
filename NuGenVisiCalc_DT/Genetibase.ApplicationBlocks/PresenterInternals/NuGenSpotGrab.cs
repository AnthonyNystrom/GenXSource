/* -----------------------------------------------
 * NuGenSpotGrab.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.PresenterInternals
{
	internal sealed class NuGenSpotGrab : NuGenEventInitiator
	{
		private Bitmap _currentSpotBmp;

		public Bitmap CurrentSpotBmp
		{
			get
			{
				return _currentSpotBmp;
			}
			set
			{
				_currentSpotBmp = value;
				this.OnCurrentSpotBmpSrcChanged(EventArgs.Empty);
			}
		}

		private static readonly object _currentSpotBmpSrcChanged = new object();

		public event EventHandler CurrentSpotBmpSrcChanged
		{
			add
			{
				this.Events.AddHandler(_currentSpotBmpSrcChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_currentSpotBmpSrcChanged, value);
			}
		}

		private void OnCurrentSpotBmpSrcChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_currentSpotBmpSrcChanged, e);
		}

		private Point _cursorPosition;

		public Point CursorPosition
		{
			get
			{
				return _cursorPosition;
			}
			set
			{
				if (_cursorPosition != value)
				{
					_cursorPosition = value;

					_posX = value.X;
					_posY = value.Y;
					_originX = value.X - _bumpX;
					_originY = value.Y - _bumpY;

					this.SetSpotInfo();
				}
			}
		}

		private int _spotHeight;

		public int SpotHeight
		{
			get
			{
				return _spotHeight;
			}

			set
			{
				if (_spotHeight != value)
				{
					_spotHeight = value;
					_bumpY = (int)(value / 2d);
					_originY = this.CursorPosition.Y - _bumpY;

					if (_spotWidth > 0 && _spotHeight > 0)
					{
						this.SetSpotInfo();
					}
				}
			}
		}

		public Size SpotSize
		{
			get
			{
				return new Size(this.SpotWidth, this.SpotHeight);
			}
			set
			{
				if (_spotWidth != value.Width || _spotHeight != value.Height)
				{
					_spotWidth = value.Width;
					_spotHeight = value.Height;
					_bumpX = (int)(value.Width / 2d);
					_bumpY = (int)(value.Height / 2d);
					_originX = this.CursorPosition.X - _bumpX;
					_originY = this.CursorPosition.Y - _bumpY;

					this.SetSpotInfo();
				}
			}
		}

		private int _spotWidth;

		public int SpotWidth
		{
			get
			{
				return _spotWidth;
			}

			set
			{
				if (_spotWidth != value)
				{
					_spotWidth = value;
					_bumpX = (int)(value / 2d);
					_originX = this.CursorPosition.X - _bumpX;

					if (_spotWidth > 0 && _spotHeight > 0)
					{
						this.SetSpotInfo();
					}
				}
			}
		}

		public void Capture()
		{
			this.SetSpotInfo();
		}

		private void SetSpotInfo()
		{
			int x = _originX;
			int y = _originY;

			IntPtr hwnd = User32.GetDesktopWindow();
			IntPtr dc = User32.GetWindowDC(hwnd);
			IntPtr memDC = Gdi32.CreateCompatibleDC(dc);
			IntPtr hbm = Gdi32.CreateCompatibleBitmap(dc, _spotWidth, _spotHeight);
			IntPtr oldbm = Gdi32.SelectObject(memDC, hbm);
			Gdi32.BitBlt(
					memDC,
					0, 0,
					this.SpotWidth, this.SpotHeight,
					dc,
					x, y,
					0x40CC0020
				);

			_currentSpotBmp = Image.FromHbitmap(hbm);

			Gdi32.SelectObject(memDC, oldbm);
			Gdi32.DeleteObject(hbm);
			Gdi32.DeleteDC(memDC);
			User32.ReleaseDC(hwnd, dc);
		}

		private int _posX;
		private int _posY;
		private int _bumpX;
		private int _bumpY;
		private int _originX;
		private int _originY;

		public NuGenSpotGrab(Point cursorPosition, int spotWidth, int spotHeight)
		{
			this.SpotWidth = spotWidth;
			this.SpotHeight = spotHeight;
			this.CursorPosition = cursorPosition;
		}
	}
}
