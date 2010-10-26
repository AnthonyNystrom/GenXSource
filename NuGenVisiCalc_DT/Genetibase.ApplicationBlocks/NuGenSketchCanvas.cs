/* -----------------------------------------------
 * NuGenSketchCanvas.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.ApplicationBlocks.Properties;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Represents a class to make sketches on the associated window.
	/// </summary>
	public partial class NuGenSketchCanvas : NuGenEventInitiator
	{
		#region Properties.Public

		/* PenColor */

		private Color _penColor = Color.Empty;

		/// <summary>
		/// </summary>
		public Color PenColor
		{
			get
			{
				if (_penColor.IsEmpty)
				{
					return Color.FromArgb(218, 68, 68);
				}

				return _penColor;
			}
			set
			{
				if (_penColor != value)
				{
					_penColor = value;
					this.Invalidate();
				}
			}
		}

		/* PenWidth */

		private float _penWidth = 3;

		/// <summary>
		/// </summary>
		public float PenWidth
		{
			get
			{
				return _penWidth;
			}
			set
			{
				if (_penWidth != value)
				{
					_penWidth = value;
					this.Invalidate();
				}
			}
		}

		#endregion

		#region Properties.Services

		private INuGenServiceProvider _serviceProvider;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		public void Clear()
		{
			_painter.Clear();
		}

		/// <summary>
		/// </summary>
		public Image GetSketch()
		{
			Rectangle bounds = this.GetBounds();
			Bitmap sketch = new Bitmap(bounds.Width, bounds.Height);
			NuGenControlPaint.DrawToBitmap(_hWnd, sketch);
			return sketch;
		}

		/// <summary>
		/// Indicates the cursor position.
		/// </summary>
		public void ShowPointer()
		{
			this.InvalidatePreviousShowPointerRect();
			_showPointerRect = new Rectangle(Cursor.Position, new Size(1, 1));
			_showPointerRect.Inflate(100, 100);
			_showPointerTimer.Start();
		}

		#endregion

		#region Methods.Graphics

		private Pen CreatePen()
		{
			Pen pen = new Pen(this.PenColor, this.PenWidth);
			pen.StartCap = pen.EndCap = LineCap.Round;
			return pen;
		}

		private void InitializeGraphics(Graphics g)
		{
			Debug.Assert(g != null, "g != null");
			g.PixelOffsetMode = PixelOffsetMode.HighQuality;
			g.SmoothingMode = SmoothingMode.AntiAlias;
		}

		private void Invalidate()
		{
			RECT rect = new RECT();
			User32.GetWindowRect(_hWnd, ref rect);
			this.Invalidate(rect);
		}

		private void Invalidate(Rectangle invalidateRectangle)
		{
			RECT rect = invalidateRectangle;
			User32.InvalidateRect(_hWnd, ref rect, true);
		}

		#endregion

		#region Methods.Private

		private RECT GetBounds()
		{
			RECT rect = new RECT();
			User32.GetWindowRect(_hWnd, ref rect);
			return rect;
		}

		private void InvalidatePreviousShowPointerRect()
		{
			/* If not inflated parts of the previous drawing are left. */
			int inflateBy = (int)this.PenWidth;
			this.Invalidate(Rectangle.Inflate(_showPointerRect, inflateBy, inflateBy));
		}

		#endregion

		#region EventHandlers.MessageFilter

		private void _msgFilter_MouseDown(object sender, MouseEventArgs e)
		{
			_painter.MouseDown(e);
		}

		private void _msgFilter_MouseMove(object sender, MouseEventArgs e)
		{
			_painter.MouseMove(e);
		}

		private void _msgFilter_MouseUp(object sender, MouseEventArgs e)
		{
			_painter.MouseUp(e);
		}

		private void _msgFilter_Paint(object sender, PaintEventArgs e)
		{
			_painter.Paint(e);
			_showPointerRect.Inflate(-15, -15);

			if (_showPointerRect.Width < this.PenWidth)
			{
				_showPointerTimer.Stop();
				return;
			}

			using (Pen pen = new Pen(this.PenColor, this.PenWidth))
			{
				e.Graphics.DrawEllipse(pen, _showPointerRect);
			}
		}

		#endregion

		#region EventHandlers.Timer

		private void _showPointerTimer_Tick(object sender, EventArgs e)
		{
			this.InvalidatePreviousShowPointerRect();
		}

		#endregion

		private Rectangle _showPointerRect;
		private IntPtr _hWnd;
		private MessageFilter _msgFilter;
		private Painter _painter;
		private Timer _showPointerTimer;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSketchCanvas"/> class.
		/// </summary>
		/// <param name="hWnd">Specifies the handle of the window to associate.</param>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// <para><see cref="INuGenButtonStateService"/></para>
		/// <para><see cref="INuGenControlStateService"/></para>
		///	<para><see cref="INuGenButtonLayoutManager"/></para>
		///	<para><see cref="INuGenButtonRenderer"/></para>
		/// <para><see cref="INuGenComboBoxRenderer"/></para>
		/// <para><see cref="INuGenDirectorySelectorRenderer"/></para>
		/// <para><see cref="INuGenImageListService"/></para>
		/// <para><see cref="INuGenPanelRenderer"/></para>
		/// <para><see cref="INuGenProgressBarLayoutManager"/></para>
		/// <para><see cref="INuGenProgressBarRenderer"/></para>
		/// <para><see cref="INuGenScrollBarRenderer"/></para>
		/// <para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// <para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// <para><see cref="INuGenTextBoxRenderer"/></para>
		/// <para><see cref="INuGenTrackBarRenderer"/></para>
		/// <para><see cref="INuGenThumbnailLayoutManager"/></para>
		/// <para><see cref="INuGenThumbnailRenderer"/></para>
		/// <para><see cref="INuGenToolStripRenderer"/></para>
		/// <para><see cref="INuGenValueTrackerService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
		/// </exception>
		/// <exception cref="ArgumentException">
		/// <para>
		///		<paramref name="hWnd"/> does not represent a window.
		/// </para>
		/// </exception>
		public NuGenSketchCanvas(IntPtr hWnd, INuGenServiceProvider serviceProvider)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new ArgumentException(string.Format(Resources.Argument_InvalidHWnd, hWnd.ToInt32()));
			}

			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_hWnd = hWnd;
			_serviceProvider = serviceProvider;
			_painter = new Painter(this);
			_msgFilter = new MessageFilter(_hWnd);
			_msgFilter.MouseDown += _msgFilter_MouseDown;
			_msgFilter.MouseMove += _msgFilter_MouseMove;
			_msgFilter.MouseUp += _msgFilter_MouseUp;
			_msgFilter.Paint += _msgFilter_Paint;

			_showPointerTimer = new Timer();
			_showPointerTimer.Interval = 50;
			_showPointerTimer.Tick += _showPointerTimer_Tick;
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		///		<see langword="true"/> to dispose both managed and unmanaged resources;
		///		<see langword="false"/> to dispose only unmanaged resources.
		/// </param>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_msgFilter != null)
				{
					_msgFilter.ReleaseHandle();
					_msgFilter.MouseDown -= _msgFilter_MouseDown;
					_msgFilter.MouseMove -= _msgFilter_MouseMove;
					_msgFilter.MouseUp -= _msgFilter_MouseUp;
					_msgFilter.Paint -= _msgFilter_Paint;
					_msgFilter = null;
				}

				if (_showPointerTimer != null)
				{
					_showPointerTimer.Tick -= _showPointerTimer_Tick;
					_showPointerTimer.Dispose();
				}
			}

			base.Dispose(disposing);
		}
	}
}
