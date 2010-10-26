/* -----------------------------------------------
 * NuGenFormMessageProcessor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.UI.NuGenInterface
{
	/// <summary>
	/// Creates a custom shape form with system menu and the ability to maximize properly.
	/// WS_BORDER | WS_CAPTION | WS_DLGFRAME | WS_THICKFRAME styles exclusion doesn't result
	/// in the same behavior.
	/// </summary>
	public class NuGenFormMessageProcessor : INuGenMessageProcessor
	{
		#region INuGenMessageProcessor Members

		/**
		 * MessageMap
		 */

		private NuGenWmHandlerList _messageMap = null;

		/// <summary>
		/// Gets an initialized message map to be linked to the <see cref="INuGenMessageFilter.MessageMap"/>.
		/// </summary>
		/// <value></value>
		public NuGenWmHandlerList MessageMap
		{
			get
			{
				if (_messageMap == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenWmHandlerMapper handlerMapper = this.ServiceProvider.GetService<INuGenWmHandlerMapper>();

					if (handlerMapper == null)
					{
						throw new NuGenServiceNotFoundException<INuGenWmHandlerMapper>();
					}

					_messageMap = handlerMapper.BuildMessageMap(this);
				}

				return _messageMap;
			}
		}

		#endregion

		#region Properties.Protected

		/**
		 * FormProperties
		 */

		private INuGenRibbonFormProperties _formProperties = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected INuGenRibbonFormProperties FormProperties
		{
			get
			{
				if (_formProperties == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					_formProperties = this.ServiceProvider.GetService<INuGenRibbonFormProperties>();

					if (_formProperties == null)
					{
						throw new NuGenServiceNotFoundException<INuGenRibbonFormProperties>();
					}
				}

				return _formProperties;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/**
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		protected virtual INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		#endregion

		#region Methods.Protected.Virtual.Window

		/*
		 * AdjustBounds
		 */

		/// <summary>
		/// Tweaks the window bounds so that it looked properly in any window state.
		/// </summary>
		protected virtual void AdjustBounds(IntPtr hWnd, ref int width, ref int height)
		{
			this.AdjustBounds(hWnd, ref width, ref height, BoundsSpecified.Size);
		}

		/// <summary>
		/// Tweaks the window bounds so that it looked properly in any window state.
		/// </summary>
		/// <exception cref="NuGenInvalidHWndException">
		/// Specified <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		protected virtual void AdjustBounds(IntPtr hWnd, ref int width, ref int height, BoundsSpecified specified)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			RECT rect = new RECT(0, 0, width, height);

			User32.AdjustWindowRectEx(
				ref rect,
				Window.GetStyle(hWnd),
				false,
				Window.GetExStyle(hWnd)
				);

			if ((specified & BoundsSpecified.Height) == BoundsSpecified.Height)
			{
				height -= rect.Height - height;
			}

			if ((specified & BoundsSpecified.Width) == BoundsSpecified.Width)
			{
				width -= rect.Width - width;
			}
		}

		/*
		 * GetClientRectangle
		 */

		/// <summary>
		/// Modifies the specified client rectangle so that the window
		/// looked properly in any window state.
		/// </summary>
		/// <exception cref="NuGenInvalidHWndException">
		/// Specified <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		protected virtual Rectangle GetClientRectangle(IntPtr hWnd, Rectangle rectangleToModify)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			if (Window.GetState(hWnd) == FormWindowState.Maximized)
			{
				RECT clientRect = new RECT(0, 0, 100, 100);

				User32.AdjustWindowRectEx(
					ref clientRect,
					Window.GetStyle(hWnd),
					false,
					Window.GetExStyle(hWnd)
				);

				Rectangle clientRectangle = (Rectangle)clientRect;

				int x = Math.Abs(clientRectangle.X);
				int offset = 2;

				rectangleToModify.X += x - offset;
				rectangleToModify.Width -= ((x * 2) - (offset * 2)) - 1;
				rectangleToModify.Height -= ((x * 2) - (offset * 2)) - 1;
				rectangleToModify.Y += x - offset - 1;

				return rectangleToModify;
			}

			return rectangleToModify;
		}

		/*
		 * IsSizable
		 */

		/// <summary>
		/// Gets the value indicating whether the window is sizable.
		/// </summary>
		/// <exception cref="NuGenInvalidHWndException">
		/// Specified <paramref name="hWnd"/> does not represent a window.
		/// </exception>
		protected virtual bool IsSizable(IntPtr hWnd)
		{
			if (!User32.IsWindow(hWnd))
			{
				throw new NuGenInvalidHWndException(hWnd);
			}

			FormBorderStyle borderStyle = Window.GetBorderStyle(hWnd);
			FormWindowState state = Window.GetState(hWnd);

			switch (borderStyle)
			{
				case FormBorderStyle.Sizable:
				case FormBorderStyle.SizableToolWindow:
				{
					if (state == FormWindowState.Normal)
					{
						return true;
					}

					break;
				}
			}

			return false;
		}

		#endregion

		#region Methods.Protected.Virtual.WinMsg

		/*
		 * OnNonDocumented
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(0xAE)]
		protected virtual void OnNonDocumented(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			/*
			 * Should ignore this non-documented message. Otherwise, caption buttons will appear
			 * under certain circumstances.
			 */
			return;
		}

		/*
		 * OnWmEraseBkgnd
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_ERASEBKGND)]
		protected virtual void OnWmEraseBkgnd(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			m.Result = Common.TRUE; // Non-zero to erase background.
		}

		/*
		 * OnWmInitMenuPopup
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_INITMENUPOPUP)]
		protected virtual void OnWmInitMenuPopup(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			Debug.Assert(m != null, "m != null");
			Debug.Assert(baseWndProc != null, "baseWndProc != null");
			Debug.Assert(User32.IsWindow(m.HWnd), "User32.IsWindow(m.HWnd)");

			baseWndProc(ref m);
			User32.InvalidateRect(m.HWnd, IntPtr.Zero, true);
		}

		/*
		 * OnWmNcActivate
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_NCACTIVATE)]
		protected virtual void OnWmNcActivate(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			m.Result = Common.TRUE;
		}

		/*
		 * OnWmNcCalcSize
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_NCCALCSIZE)]
		protected virtual void OnWmNcCalcSize(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			Debug.Assert(m != null, "m != null");
			Debug.Assert(baseWndProc != null, "baseWndProc != null");
			Debug.Assert(User32.IsWindow(m.HWnd), "User32.IsWindow(m.HWnd)");

			baseWndProc(ref m);

			if (m.WParam == Common.FALSE)
			{
				RECT rect = (RECT)Marshal.PtrToStructure(m.LParam, typeof(RECT));

				Marshal.StructureToPtr(
					(RECT)this.GetClientRectangle(m.HWnd, (Rectangle)rect),
					m.LParam,
					false
				);

				m.Result = Common.FALSE;
			}
			else
			{
				NCCALCSIZE_PARAMS calcSizeParams = (NCCALCSIZE_PARAMS)Marshal.PtrToStructure(m.LParam, typeof(NCCALCSIZE_PARAMS));
				WINDOWPOS wndPos = (WINDOWPOS)Marshal.PtrToStructure(calcSizeParams.lppos, typeof(WINDOWPOS));
				RECT bufferRect = (RECT)this.GetClientRectangle(
					m.HWnd,
					new Rectangle(wndPos.x, wndPos.y, wndPos.width, wndPos.height)
				);

				calcSizeParams.rectProposed = bufferRect;
				calcSizeParams.rectBeforeMove = bufferRect;

				Marshal.StructureToPtr(calcSizeParams, m.LParam, false);

				m.Result = new IntPtr(WinUser.WVR_VALIDRECTS);
			}
		}

		/*
		 * OnWmNcHitTest
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_NCHITTEST)]
		protected virtual void OnWmNcHitTest(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			Debug.Assert(m != null, "m != null");
			Debug.Assert(baseWndProc != null, "baseWndProc != null");
			Debug.Assert(User32.IsWindow(m.HWnd), "User32.IsWindow(m.HWnd)");

			Point testPoint = NuGenControlPaint.BuildMousePos(m.LParam);
			User32.ScreenToClient(m.HWnd, ref testPoint);

			int largeOffset = 4;
			int smallOffset = 1;

			Rectangle bounds = Window.GetBounds(m.HWnd);
			Rectangle testRect = Rectangle.Empty;

			if (this.IsSizable(m.HWnd))
			{
				testRect = new Rectangle(
					0,
					0,
					bounds.Width,
					largeOffset
				);

				if (testRect.Contains(testPoint))
				{
					m.Result = new IntPtr(WinUser.HTTOP);
					return;
				}

				testRect = new Rectangle(
					0,
					bounds.Height - largeOffset,
					bounds.Width,
					largeOffset
				);

				if (testRect.Contains(testPoint))
				{
					m.Result = new IntPtr(WinUser.HTBOTTOM);
					return;
				}

				if (Window.IsRightToLeft(m.HWnd))
				{
					testRect = new Rectangle(
						bounds.Width - this.FormProperties.TopRightCornerSize,
						0,
						this.FormProperties.TopRightCornerSize + smallOffset,
						this.FormProperties.TopRightCornerSize + smallOffset
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTTOPLEFT);
						return;
					}

					testRect = new Rectangle(
						0,
						0,
						this.FormProperties.TopLeftCornerSize + smallOffset,
						this.FormProperties.TopLeftCornerSize + smallOffset
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTTOPRIGHT);
						return;
					}

					testRect = new Rectangle(
						bounds.Width - this.FormProperties.BottomRightCornerSize,
						bounds.Height - this.FormProperties.BottomRightCornerSize,
						this.FormProperties.BottomRightCornerSize + smallOffset,
						this.FormProperties.BottomRightCornerSize + smallOffset
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTBOTTOMLEFT);
						return;
					}

					testRect = new Rectangle(
						0,
						bounds.Height - this.FormProperties.BottomLeftCornerSize,
						this.FormProperties.BottomLeftCornerSize + smallOffset,
						this.FormProperties.BottomLeftCornerSize + smallOffset
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTBOTTOMRIGHT);
						return;
					}

					testRect = new Rectangle(
						bounds.Width - largeOffset,
						0,
						largeOffset,
						bounds.Height
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTLEFT);
						return;
					}

					testRect = new Rectangle(0, 0, largeOffset, bounds.Height);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTRIGHT);
						return;
					}
				}
				else
				{
					testRect = new Rectangle(
						0,
						0,
						this.FormProperties.TopLeftCornerSize + smallOffset,
						this.FormProperties.TopLeftCornerSize + smallOffset
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTTOPLEFT);
						return;
					}

					testRect = new Rectangle(
						bounds.Width - this.FormProperties.TopRightCornerSize,
						0,
						this.FormProperties.TopRightCornerSize + smallOffset,
						this.FormProperties.TopRightCornerSize + smallOffset
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTTOPRIGHT);
						return;
					}

					testRect = new Rectangle(
						0,
						bounds.Height - this.FormProperties.BottomLeftCornerSize,
						this.FormProperties.BottomLeftCornerSize + smallOffset,
						this.FormProperties.BottomLeftCornerSize + smallOffset
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTBOTTOMLEFT);
						return;
					}

					testRect = new Rectangle(
						bounds.Width - this.FormProperties.BottomLeftCornerSize,
						bounds.Height - this.FormProperties.BottomRightCornerSize,
						this.FormProperties.BottomRightCornerSize + smallOffset,
						this.FormProperties.BottomRightCornerSize + smallOffset
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTBOTTOMRIGHT);
						return;
					}

					testRect = new Rectangle(
						0,
						0,
						largeOffset,
						bounds.Height
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTLEFT);
						return;
					}

					testRect = new Rectangle(
						bounds.Width - largeOffset,
						0,
						largeOffset,
						bounds.Height
					);

					if (testRect.Contains(testPoint))
					{
						m.Result = new IntPtr(WinUser.HTRIGHT);
						return;
					}
				}
			}

			// HACK: For testing purposes only.
			m.Result = (IntPtr)WinUser.HTCAPTION;
		}

		/*
		 * OnWmNcPaint
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_NCPAINT)]
		protected virtual void OnWmNcPaint(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			Debug.Assert(m != null, "m != null");
			Debug.Assert(baseWndProc != null, "baseWndProc != null");

			m.Result = Common.FALSE;
		}

		/*
		 * OnWmSetTest
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_SETTEXT)]
		protected virtual void OnWmSetText(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			Debug.Assert(m != null, "m != null");
			Debug.Assert(baseWndProc != null, "baseWndProc != null");
			Debug.Assert(User32.IsWindow(m.HWnd), "User32.IsWindow(m.HWnd)");

			baseWndProc(ref m);
			User32.InvalidateRect(m.HWnd, IntPtr.Zero, true);
		}

		/*
		 * OnWmWindowPosChanged
		 */

		private bool _shouldReduceBounds = false;

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_WINDOWPOSCHANGED)]
		protected virtual void OnWmWindowPosChanged(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			Debug.Assert(m != null, "m != null");
			Debug.Assert(baseWndProc != null, "baseWndProc != null");
			Debug.Assert(User32.IsWindow(m.HWnd), "User32.IsWindow(m.HWnd)");

			baseWndProc(ref m);

			FormWindowState state = Window.GetState(m.HWnd);

			if (
				Window.GetState(m.HWnd) == FormWindowState.Normal
				)
			{
				_shouldReduceBounds = false;
				baseWndProc(ref m);
				return;
			}

			_shouldReduceBounds = true;
			baseWndProc(ref m);
			_shouldReduceBounds = true;
		}

		/*
		 * OnWindowPosChanging
		 */

		/// <summary>
		/// </summary>
		/// <param name="m"></param>
		/// <param name="baseWndProc"></param>
		[NuGenWmHandler(WinUser.WM_WINDOWPOSCHANGING)]
		protected virtual void OnWindowPosChanging(ref Message m, NuGenWndProcDelegate baseWndProc)
		{
			Debug.Assert(m != null, "m != null");
			Debug.Assert(baseWndProc != null, "baseWndProc != null");
			Debug.Assert(User32.IsWindow(m.HWnd), "User32.IsWindow(m.HWnd)");

			WINDOWPOS wndPos = (WINDOWPOS)m.GetLParam(typeof(WINDOWPOS));
			Rectangle bounds = Window.GetBounds(m.HWnd);

			if (
				Window.GetState(m.HWnd) == FormWindowState.Minimized
				|| _shouldReduceBounds
				)
			{
				int width = wndPos.width;
				int height = wndPos.height;

				if (width != bounds.Width || height != bounds.Height)
				{
					this.AdjustBounds(m.HWnd, ref width, ref height);
				}

				wndPos.width = width;
				wndPos.height = height;

				_shouldReduceBounds = true;
			}

			Marshal.StructureToPtr(wndPos, m.LParam, false);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFormMessageProcessor"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// Requires:
		/// <para><see cref="INuGenWmHandlerMapper"/></para>
		/// <para><see cref="INuGenRibbonFormProperties"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// </exception>
		public NuGenFormMessageProcessor(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
		}

		#endregion
	}
}
