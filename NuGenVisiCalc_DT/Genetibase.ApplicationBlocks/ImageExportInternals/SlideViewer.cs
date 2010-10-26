/* -----------------------------------------------
 * SlideViewer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	internal sealed class SlideViewer : IDisposable
	{
		public float CurrentZoom
		{
			get
			{
				return _zoomFactor;
			}
			set
			{
				_zoomFactor = value;
				this.DrawImage(_host.Width / 2, _host.Height / 2);
			}
		}

		public Image Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (_image != value)
				{
					_image = value;
					_host.Invalidate();
				}
			}
		}

		public float MaxZoom
		{
			get
			{
				return _maxZ;
			}
			set
			{
				_maxZ = value;
			}
		}

		public float MinZoom
		{
			get
			{
				return _minZ;
			}
			set
			{
				_minZ = value;
			}
		}

		/// <summary>
		/// </summary>
		public void ZoomIn()
		{
			if (_zoomFactor > _maxZ)
			{
				return;
			}

			_oldZFactor = _zoomFactor;
			_zoomFactor = _zoomFactor * 1.3f;

			_host.Invalidate();
		}

		/// <summary>
		/// </summary>
		public void ZoomOut()
		{
			if (_zoomFactor < _minZ)
			{
				return;
			}

			_oldZFactor = _zoomFactor;
			_zoomFactor = _zoomFactor / 1.3f;

			_host.Invalidate();
		}

		private void DrawImage(float zoomX, float zoomY)
		{
			if (_image == null)
			{
				return;
			}

			_xOut = false;
			_yOut = false;

			if (_host.Width > _image.Width * _zoomFactor)
			{
				_mrec.X = 0;
				_mrec.Width = _image.Width;
				_brec.X = (_host.Width - _image.Width * _zoomFactor) / 2;
				_brec.Width = _image.Width * _zoomFactor;
			}
			else
			{
				_mrec.X = _mrec.X + ((_host.Width / _oldZFactor - _host.Width / _zoomFactor) / ((_host.Width + 0.001f) / zoomX));
				_mrec.Width = _host.Width / _zoomFactor;
				_brec.X = 0;
				_brec.Width = _host.Width;
			}

			if (_host.Height > _image.Height * _zoomFactor)
			{
				_mrec.Y = 0;
				_mrec.Height = _image.Height;
				_brec.Y = (_host.Height - _image.Height * _zoomFactor) / 2;
				_brec.Height = _image.Height * _zoomFactor;
			}
			else
			{
				_mrec.Y = _mrec.Y + ((_host.Height / _oldZFactor - _host.Height / _zoomFactor) / ((_host.Height + 0.001f) / zoomY));
				_mrec.Height = _host.Height / _zoomFactor;
				_brec.Y = 0;
				_brec.Height = _host.Height;
			}

			_oldZFactor = _zoomFactor;

			if (_mrec.Right > _image.Width)
			{
				_xOut = true;
				_mrec.X = _image.Width - _mrec.Width;
			}

			if (_mrec.X < 0)
			{
				_xOut = true;
				_mrec.X = 0;
			}

			if (_mrec.Bottom > _image.Height)
			{
				_yOut = true;
				_mrec.Y = _image.Height - _mrec.Height;
			}

			if (_mrec.Y < 0)
			{
				_yOut = true;
				_mrec.Y = 0;
			}

			using (Graphics g = _host.CreateGraphics())
			{
				int brecX = (int)_brec.X;
				int brecY = (int)_brec.Y;
				int brecWidth = (int)_brec.Width;
				int brecHeight = (int)_brec.Height;

				int mrecX = (int)_mrec.X;
				int mrecY = (int)_mrec.Y;
				int mrecWidth = (int)_mrec.Width;
				int mrecHeight = (int)_mrec.Height;

				g.DrawImage(
					_image
					, new Rectangle(brecX, brecY, brecWidth, brecHeight)
					, new Rectangle(mrecX, mrecY, mrecWidth, mrecHeight)
					, GraphicsUnit.Pixel
				);

				int leftRectWidth = brecX - _host.Left;
				int topRectHeight = brecY - _host.Top;
				int topRectWidth = _host.Width - leftRectWidth;
				int rightRectLeft = _host.Left + leftRectWidth + brecWidth;

				g.FillRectangle(Brushes.Black, new Rectangle(0, 0, leftRectWidth, _host.Height));
				g.FillRectangle(Brushes.Black, new Rectangle(leftRectWidth, 0, topRectWidth, topRectHeight));
				g.FillRectangle(Brushes.Black, Rectangle.FromLTRB(rightRectLeft, topRectHeight, _host.Right, _host.Bottom));
				g.FillRectangle(Brushes.Black, Rectangle.FromLTRB(leftRectWidth, _host.Top + topRectHeight + brecHeight, rightRectLeft, _host.Bottom));
			}
		}

		private void _host_MouseDown(object sender, MouseEventArgs e)
		{
			if (_image != null)
			{
				_p.X = e.X;
				_p.Y = e.Y;
				_cp.X = 0;
				_cp.Y = 0;
				_cs.X = e.X;
				_cs.Y = e.Y;
				_downPress = true;
			}
		}

		private void _host_MouseMove(object sender, MouseEventArgs e)
		{
			if (_image != null)
			{
				if (_downPress)
				{
					_host.Cursor = Cursors.NoMove2D;

					// Accelerated scrolling when right click drag.
					if (e.Button == MouseButtons.Right)
					{
						_cp.X = (_p.X - e.X) * (_image.Width / 2000);
						_cp.Y = (_p.Y - e.Y) * (_image.Height / 2000);
					}

					_mrec.X = ((_p.X - e.X) / _zoomFactor) + _mrec.X + _cp.X;
					_mrec.Y = ((_p.Y - e.Y) / _zoomFactor) + _mrec.Y + _cp.Y;
					this.DrawImage(0, 0);

					if (!_xOut)
					{
						_p.X = e.X;
					}

					if (!_yOut)
					{
						_p.X = e.Y;
					}
				}
			}
		}

		private void _host_MouseUp(object sender, MouseEventArgs e)
		{
			if (_image != null)
			{
				_downPress = false;
				_host.Cursor = Cursors.Arrow;

				if (_cs.X == e.X && _cs.Y == e.Y)
				{
					if (e.Button == MouseButtons.Left)
					{
						this.ZoomIn();
					}
					else if (e.Button == MouseButtons.Right)
					{
						this.ZoomOut();
					}
				}
				else
				{
					_host.Invalidate();
				}
			}
		}

		private void _host_Paint(object sender, PaintEventArgs e)
		{
			this.DrawImage(0, 0);
		}

		private void _host_Resize(object sender, EventArgs e)
		{
			if (_hostLoadComplete)
			{
				this.DrawImage(0, 0);
			}
		}

		private Control _host;
		private Image _image;
		private PointF _p;
		private PointF _cp;
		private PointF _cs;
		private RectangleF _mrec; // Main rectangle.
		private RectangleF _brec; // Boundary rectangle.
		private float _zoomFactor = 1; // Current zoom.
		private float _minZ = 0.05f; // Minimum zoom.
		private float _maxZ = 20.0f; // Maximum zoom.
		private float _oldZFactor = 1; // Previous zoom (bigger means zoom in).
		private bool _xOut;
		private bool _yOut;
		private bool _hostLoadComplete;
		private bool _downPress;

		/// <summary>
		/// Initializes a new instance of the <see cref="SlideViewer"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="hostControl"/> is <see langword="null"/>.</para>
		/// </exception>
		public SlideViewer(Control hostControl)
		{
			if (hostControl == null)
			{
				throw new ArgumentNullException("hostControl");
			}

			_host = hostControl;

			_host.MouseDown += _host_MouseDown;
			_host.MouseMove += _host_MouseMove;
			_host.MouseUp += _host_MouseUp;
			_host.Paint += _host_Paint;
			_host.Resize += _host_Resize;

			_hostLoadComplete = true;
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			if (_host != null)
			{
				_host.MouseDown -= _host_MouseDown;
				_host.MouseMove -= _host_MouseMove;
				_host.MouseUp -= _host_MouseUp;
				_host.Paint -= _host_Paint;
				_host.Resize -= _host_Resize;
			}
		}
	}
}
