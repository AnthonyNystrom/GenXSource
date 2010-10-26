/* -----------------------------------------------
 * NuGenSketchCanvas.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenSketchCanvas
	{
		private sealed class MessageFilter : NuGenNativeWindow
		{
			#region Events

			/* MouseDown */

			private static readonly object _mouseDown = new object();

			public event MouseEventHandler MouseDown
			{
				add
				{
					this.Events.AddHandler(_mouseDown, value);
				}
				remove
				{
					this.Events.RemoveHandler(_mouseDown, value);
				}
			}

			private void OnMouseDown(MouseEventArgs e)
			{
				this.Initiator.InvokeMouseEventHandler(_mouseDown, e);
			}

			/* MouseMove */

			private static readonly object _mouseMove = new object();

			public event MouseEventHandler MouseMove
			{
				add
				{
					this.Events.AddHandler(_mouseMove, value);
				}
				remove
				{
					this.Events.RemoveHandler(_mouseMove, value);
				}
			}

			private void OnMouseMove(MouseEventArgs e)
			{
				this.Initiator.InvokeMouseEventHandler(_mouseMove, e);
			}

			/* MouseUp */

			private static readonly object _mouseUp = new object();

			public event MouseEventHandler MouseUp
			{
				add
				{
					this.Events.AddHandler(_mouseUp, value);
				}
				remove
				{
					this.Events.RemoveHandler(_mouseUp, value);
				}
			}

			private void OnMouseUp(MouseEventArgs e)
			{
				this.Initiator.InvokeMouseEventHandler(_mouseUp, e);
			}

			/* Paint */

			private static readonly object _paint = new object();

			public event PaintEventHandler Paint
			{
				add
				{
					this.Events.AddHandler(_paint, value);
				}
				remove
				{
					this.Events.RemoveHandler(_paint, value);
				}
			}

			private void OnPaint(PaintEventArgs e)
			{
				this.Initiator.InvokePaintEventHandler(_paint, e);
			}

			#endregion

			protected override void WndProc(ref Message m)
			{
				switch (m.Msg)
				{
					case WinUser.WM_LBUTTONDOWN:
					case WinUser.WM_RBUTTONDOWN:
					case WinUser.WM_MBUTTONDOWN:
					case WinUser.WM_XBUTTONDOWN:
					{
						base.WndProc(ref m);
						this.OnMouseDown(this.BuildOneClickMouseEventArgs(m.WParam, m.LParam));
						break;
					}
					case WinUser.WM_LBUTTONUP:
					case WinUser.WM_RBUTTONUP:
					case WinUser.WM_MBUTTONUP:
					case WinUser.WM_XBUTTONUP:
					{
						base.WndProc(ref m);
						this.OnMouseUp(this.BuildOneClickMouseEventArgs(m.WParam, m.LParam));
						break;
					}
					case WinUser.WM_MOUSEMOVE:
					{
						base.WndProc(ref m);
						this.OnMouseMove(this.BuildZeroClickMouseEventArgs(m.WParam, m.LParam));
						break;
					}
					case WinUser.WM_PRINT:
					case WinUser.WM_PAINT:
					{
						base.WndProc(ref m);

						using (Graphics g = m.WParam != IntPtr.Zero ? Graphics.FromHdc(m.WParam) : Graphics.FromHwnd(this.Handle))
						{
							this.OnPaint(new PaintEventArgs(g, Rectangle.Empty));
						}
						break;
					}
					default:
					{
						base.WndProc(ref m);
						break;
					}
				}
			}

			private MouseEventArgs BuildZeroClickMouseEventArgs(IntPtr wParam, IntPtr lParam)
			{
				return NuGenControlPaint.BuildMouseEventArgs(wParam, lParam, 0);
			}

			private MouseEventArgs BuildOneClickMouseEventArgs(IntPtr wParam, IntPtr lParam)
			{
				return NuGenControlPaint.BuildMouseEventArgs(wParam, lParam, 1);
			}

			public MessageFilter(IntPtr hWnd)
			{
				Debug.Assert(User32.IsWindow(hWnd), "User32.IsWindow(hWnd)");
				this.AssignHandle(hWnd);
			}
		}
	}
}
