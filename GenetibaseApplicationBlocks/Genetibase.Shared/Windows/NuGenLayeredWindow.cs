/* -----------------------------------------------
 * NuGenLayeredWindow.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Represents a base layered window form.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenLayeredWindow : Form
	{
		#region Properties.Public

		/*
		 * HitTestEnabled
		 */
		
		/// <summary>
		/// Gets or sets the value indicating whether this layered window receives mouse messages.
		/// </summary>
		public bool HitTestEnabled
		{
			get
			{
				return !this.GetValue(WinUser.WS_EX_TRANSPARENT);
			}
			set
			{
				this.SetValue(WinUser.WS_EX_TRANSPARENT, !value);
			}
		}

		/*
		 * LayeredMode
		 */

		/// <summary>
		/// Gets or sets the value indicating whether this layered window is in the layered mode.
		/// </summary>
		public bool LayeredMode
		{
			get
			{
				return this.GetValue(WinUser.WS_EX_LAYERED);
			}
			set
			{
				this.SetValue(WinUser.WS_EX_LAYERED, value);
			}
		}

		/*
		 * Opacity
		 */

		private int _opacity;

		/// <summary>
		/// Gets or sets the opacity level for this layered window. 0 = Opaque. 100 = Transparent.
		/// </summary>
		public new int Opacity
		{
			get
			{
				return _opacity;
			}
			set
			{
				if (_opacity != value)
				{
					_opacity = value;
					this.Invalidate();

					this.OnOpacityChanged(EventArgs.Empty);
				}
			}
		}

		private static readonly object _opacityChanged = new object();

		/// <summary>
		/// Occurs when the value of the <see cref="Genetibase.Shared.Windows.NuGenLayeredWindow.Opacity"/>
		/// property changes.
		/// </summary>
		public event EventHandler OpacityChanged
		{
			add
			{
				this.Events.AddHandler(_opacityChanged, value);
			}
			remove
			{
				this.Events.RemoveHandler(_opacityChanged, value);
			}
		}

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Windows.NuGenLayeredWindow.OpacityChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected virtual void OnOpacityChanged(EventArgs e)
		{
			this.Initiator.InvokePropertyChanged(_opacityChanged, e);
		}

		#endregion

		#region Properties.Protected

		private bool _suppressPaint;

		/// <summary>
		/// Gets or sets the value indicating whether it is necessary to suppress the
		/// <see cref="Genetibase.Shared.Windows.NuGenLayeredWindow.Paint"/> event.
		/// Use this to increase performance.
		/// </summary>
		protected bool SuppressPaint
		{
			get
			{
				return _suppressPaint;
			}
			set
			{
				_suppressPaint = value;
			}
		}

		#endregion

		#region Properties.Protected.Overriden

		/// <summary>
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WinUser.WS_EX_TOPMOST;
				return cp;
			}
		}

		#endregion

		#region Properties.Service

		/*
		 * Initiator
		 */

		private NuGenEventInitiatorService _initiator;

		/// <summary>
		/// </summary>
		protected NuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}

		#endregion

		#region Methods.Private

		private bool GetValue(int exStyle)
		{
			int currentStyle = User32.GetWindowLong(this.Handle, WinUser.GWL_EXSTYLE);
			return (currentStyle & exStyle) != 0;
		}

		private void SetValue(int exStyle, bool value)
		{
			int currentStyle = User32.GetWindowLong(this.Handle, WinUser.GWL_EXSTYLE);

			if (value)
				currentStyle |= exStyle;
			else
				currentStyle &= ~exStyle;

			User32.SetWindowLong(this.Handle, WinUser.GWL_EXSTYLE, currentStyle);
		}

		#endregion

		#region Methods.Invalidate

		/// <summary>
		/// Invalidates the drawing surface.
		/// </summary>
		public new virtual void Invalidate()
		{
			if (this.Visible)
			{
				this.Invalidate(this.Bounds);
			}
		}

		private new void Invalidate(Rectangle rect)
		{
			Bitmap memoryBitmap = new Bitmap(
				rect.Size.Width,
				rect.Size.Height,
				PixelFormat.Format32bppArgb
				);

			using (Graphics g = Graphics.FromImage(memoryBitmap))
			{
				Rectangle area = new Rectangle(0, 0, rect.Size.Width, rect.Size.Height);
				this.RaisePaintEvent(g, area);
				IntPtr hDC = User32.GetDC(IntPtr.Zero);
				IntPtr memoryDC = Gdi32.CreateCompatibleDC(hDC);
				IntPtr hBitmap = memoryBitmap.GetHbitmap(Color.FromArgb(0));
				IntPtr oldBitmap = Gdi32.SelectObject(memoryDC, hBitmap);

				SIZE size;
				size.cx = rect.Size.Width;
				size.cy = rect.Size.Height;

				POINT location;
				location.x = rect.Location.X;
				location.y = rect.Location.Y;

				POINT sourcePoint;
				sourcePoint.x = 0;
				sourcePoint.y = 0;

				BLENDFUNCTION blend = new BLENDFUNCTION();
				blend.AlphaFormat = (byte)WinGdi.AC_SRC_ALPHA;
				blend.BlendFlags = 0;
				blend.BlendOp = (byte)WinGdi.AC_SRC_OVER;
				blend.SourceConstantAlpha = (byte)(255 - ((this.Opacity * 255) / 100));

				User32.UpdateLayeredWindow(
					this.Handle,
					hDC,
					ref location,
					ref size,
					memoryDC,
					ref sourcePoint,
					0,
					ref blend,
					WinUser.ULW_ALPHA
					);

				Gdi32.SelectObject(memoryDC, oldBitmap);

				User32.ReleaseDC(IntPtr.Zero, hDC);
				Gdi32.DeleteObject(hBitmap);
				Gdi32.DeleteDC(memoryDC);
			}
		}

		#endregion

		#region Methods.Paint

		private PaintEventHandler _paintHandler;

		/// <summary>
		/// Occurs when the control is redrawn.
		/// </summary>
		public new event PaintEventHandler Paint
		{
			add
			{
				bool repaint = (_paintHandler == null);
				_paintHandler += value;

				if (repaint)
				{
					this.Invalidate();
				}
			}
			remove
			{
				_paintHandler -= value;
			}
		}

		private bool _paintEnabled = true;

		private void RaisePaintEvent(Graphics g, Rectangle clipRect)
		{
			if (_paintEnabled)
			{
				PaintEventArgs e = new PaintEventArgs(g, clipRect);
				this.OnPaint(e);
			}
		}

		#endregion

		#region Methods.Protected.Overriden

		/*
		 * OnStyleChanged
		 */

		/// <summary>
		/// Will bubble the <see cref="Genetibase.Shared.Windows.NuGenLayeredWindow.StyleChanged"/> event.
		/// </summary>
		/// <param name="e"></param>
		protected override void OnStyleChanged(EventArgs e)
		{
			base.OnStyleChanged(e);
			this.Invalidate();
		}

		/*
		 * SetBoundsCore
		 */

		/// <summary>
		/// </summary>
		protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
		{
			_paintEnabled = false;
			base.SetBoundsCore(x, y, width, height, specified);
			_paintEnabled = true;

			if (!this.SuppressPaint || specified != BoundsSpecified.Location)
			{
				this.Invalidate(new Rectangle(x, y, width, height));
			}
		}

		/*
		 * WndProc
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				/*
				 * Allows the form to redraw itself properly while resizing.
				 */
				case WinUser.WM_NCCALCSIZE:
				{
					m.Result = (IntPtr)(
						WinUser.WVR_ALIGNBOTTOM
						| WinUser.WVR_ALIGNLEFT
						| WinUser.WVR_ALIGNRIGHT
						| WinUser.WVR_ALIGNTOP
						);
					return;
				}
				/*
				 * Doesn't allow the host form to lose the focus.
				 */
				case WinUser.WM_MOUSEACTIVATE:
				{
					m.Result = (IntPtr)WinUser.MA_NOACTIVATE;
					return;
				}
			}

			base.WndProc(ref m);
		}

		#endregion

		#region Methods.Public

		/// <summary>
		/// </summary>
		public new void Show()
		{
			User32.ShowWindow(this.Handle, WinUser.SW_SHOWNA);
		}

		/// <summary>
		/// </summary>
		/// <param name="location"></param>
		public void Show(Point location)
		{
			this.Location = location;
			this.Show();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLayeredWindow"/> class.
		/// </summary>
		public NuGenLayeredWindow()
		{
			this.FormBorderStyle = FormBorderStyle.None;
			this.ShowInTaskbar = false;
			this.LayeredMode = true;

			this.Invalidate();
		}

		#endregion
	}
}
