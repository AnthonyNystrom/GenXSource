/* -----------------------------------------------
 * NuGenUISnap.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenUISnap
	{
		private sealed class WndManager : NativeWindow, IDisposable
		{
			#region ResizeDir

			private enum ResizeDir
			{
				Top = 2,
				Bottom = 4,
				Left = 8,
				Right = 16
			}

			#endregion

			#region OnHandleChange

			[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
			protected override void OnHandleChange()
			{
				if ((int)this.Handle != 0)
				{
					_snaps.Add(_originalForm);
				}
				else
				{
					_snaps.Remove(_originalForm);
				}
			}

			#endregion

			#region WndProc

			/// <summary>
			/// Invokes the default window procedure associated with this window.
			/// </summary>
			/// <param name="m">A <see cref="T:System.Windows.Forms.Message"></see> that is associated with the current Windows message.</param>
			[System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
			protected override void WndProc(ref Message m)
			{
				if (!_messageProcessor(ref m))
					base.WndProc(ref m);
			}

			#endregion

			#region DefaultMsgProcessor

			/// <summary>
			/// Processes messages during normal operations (while the form is not resized or moved)
			/// </summary>
			/// <param name="m"></param>
			/// <returns></returns>
			private bool DefaultMsgProcessor(ref Message m)
			{
				switch (m.Msg)
				{
					case WinUser.WM_NCLBUTTONDOWN:
					{
						_originalForm.Activate();

						_mousePosition.X = (short)Common.LoWord(m.LParam.ToInt32());
						_mousePosition.Y = (short)Common.HiWord(m.LParam.ToInt32());

						if (OnNCLButtonDown(m.WParam.ToInt32(), _mousePosition))
						{
							m.Result = (IntPtr)((_resizingForm || _movingForm) ? 1 : 0);
							return true;
						}
						break;
					}
				}

				return false;
			}

			#endregion

			#region OnNCLButtonDown

			/// <summary>
			/// Checks where the click was in the NC area and starts move or resize operation
			/// </summary>
			/// <param name="iHitTest"></param>
			/// <param name="point"></param>
			/// <returns></returns>
			private bool OnNCLButtonDown(int iHitTest, Point point)
			{
				Rectangle rParent = _originalForm.Bounds;
				_primaryOffset = point;

				switch (iHitTest)
				{
					case WinUser.HTCAPTION:
					{	// request for move
						if (_owner._stickOnMove)
						{
							_primaryOffset.Offset(-rParent.Left, -rParent.Top);
							StartMove();
							return true;
						}
						else
							return false;	// leave default processing

					}

					// requests for resize
					case WinUser.HTTOPLEFT:
					{
						return StartResize(ResizeDir.Top | ResizeDir.Left);
					}
					case WinUser.HTTOP:
					{
						return StartResize(ResizeDir.Top);
					}
					case WinUser.HTTOPRIGHT:
					{
						return StartResize(ResizeDir.Top | ResizeDir.Right);
					}
					case WinUser.HTRIGHT:
					{
						return StartResize(ResizeDir.Right);
					}
					case WinUser.HTBOTTOMRIGHT:
					{
						return StartResize(ResizeDir.Bottom | ResizeDir.Right);
					}
					case WinUser.HTBOTTOM:
					{
						return StartResize(ResizeDir.Bottom);
					}
					case WinUser.HTBOTTOMLEFT:
					{
						return StartResize(ResizeDir.Bottom | ResizeDir.Left);
					}
					case WinUser.HTLEFT:
					{
						return StartResize(ResizeDir.Left);
					}
				}

				return false;
			}

			#endregion

			#region ResizeOperations

			private bool StartResize(ResizeDir resDir)
			{
				if (_owner._stickOnResize)
				{
					_resizeDirection = resDir;
					_formBounds = _originalForm.Bounds;
					_formOriginalBounds = _originalForm.Bounds;	// save the old bounds

					if (!_originalForm.Capture)	// start capturing messages
						_originalForm.Capture = true;

					_messageProcessor = _resizeMessageProcessor;

					return true;	// catch the message
				}
				else
					return false;	// leave default processing !
			}

			private bool ResizeMsgProcessor(ref Message m)
			{
				if (!_originalForm.Capture)
				{
					Cancel();
					return false;
				}

				switch (m.Msg)
				{
					case WinUser.WM_LBUTTONUP:
					{	// ok, resize finished !!!
						EndResize();
						break;
					}
					case WinUser.WM_MOUSEMOVE:
					{
						_mousePosition.X = (short)Common.LoWord(m.LParam.ToInt32());
						_mousePosition.Y = (short)Common.HiWord(m.LParam.ToInt32());
						Resize(_mousePosition);
						break;
					}
					case WinUser.WM_KEYDOWN:
					{
						if (m.WParam.ToInt32() == (int)Keys.Escape)
						{
							_originalForm.Bounds = _formOriginalBounds;	// set back old size
							Cancel();
						}
						break;
					}
				}

				return false;
			}

			private void EndResize()
			{
				Cancel();
			}

			#endregion

			#region Resize Computing

			private void Resize(Point p)
			{
				p = _originalForm.PointToScreen(p);
				Screen activeScr = Screen.FromPoint(p);
				_formBounds = _originalForm.Bounds;

				int iRight = _formBounds.Right;
				int iBottom = _formBounds.Bottom;

				// no normalize required
				// first strech the window to the new position
				if ((_resizeDirection & ResizeDir.Left) == ResizeDir.Left)
				{
					_formBounds.Width = _formBounds.X - p.X + _formBounds.Width;
					_formBounds.X = iRight - _formBounds.Width;
				}
				if ((_resizeDirection & ResizeDir.Right) == ResizeDir.Right)
					_formBounds.Width = p.X - _formBounds.Left;

				if ((_resizeDirection & ResizeDir.Top) == ResizeDir.Top)
				{
					_formBounds.Height = _formBounds.Height - p.Y + _formBounds.Top;
					_formBounds.Y = iBottom - _formBounds.Height;
				}
				if ((_resizeDirection & ResizeDir.Bottom) == ResizeDir.Bottom)
					_formBounds.Height = p.Y - _formBounds.Top;

				// this is the real new position
				// now, try to snap it to different objects (first to screen)

				// CARE !!! We use "Width" and "Height" as Bottom & Right!! (C++ style)
				//formOffsetRect = new Rectangle ( stickGap + 1, stickGap + 1, 0, 0 );
				_formOffsetRect.X = _owner._stickGap + 1;
				_formOffsetRect.Y = _owner._stickGap + 1;
				_formOffsetRect.Height = 0;
				_formOffsetRect.Width = 0;

				if (_owner._stickToScreen)
					Resize_Stick(activeScr.WorkingArea, false);

				if (_owner._stickToOther)
				{
					// now try to stick to other forms
					foreach (Form sw in _snaps)
					{
						if (sw != this._originalForm)
							Resize_Stick(sw.Bounds, true);
					}
				}

				// Fix (clear) the values that were not updated to stick
				if (_formOffsetRect.X == _owner._stickGap + 1)
					_formOffsetRect.X = 0;
				if (_formOffsetRect.Width == _owner._stickGap + 1)
					_formOffsetRect.Width = 0;
				if (_formOffsetRect.Y == _owner._stickGap + 1)
					_formOffsetRect.Y = 0;
				if (_formOffsetRect.Height == _owner._stickGap + 1)
					_formOffsetRect.Height = 0;

				// compute the new form size
				if ((_resizeDirection & ResizeDir.Left) == ResizeDir.Left)
				{	// left resize requires special handling of X & Width acording to MinSize and MinWindowTrackSize
					int iNewWidth = _formBounds.Width + _formOffsetRect.Width + _formOffsetRect.X;

					if (_originalForm.MaximumSize.Width != 0)
						iNewWidth = Math.Min(iNewWidth, _originalForm.MaximumSize.Width);

					iNewWidth = Math.Min(iNewWidth, SystemInformation.MaxWindowTrackSize.Width);
					iNewWidth = Math.Max(iNewWidth, _originalForm.MinimumSize.Width);
					iNewWidth = Math.Max(iNewWidth, SystemInformation.MinWindowTrackSize.Width);

					_formBounds.X = iRight - iNewWidth;
					_formBounds.Width = iNewWidth;
				}
				else
				{	// other resizes
					_formBounds.Width += _formOffsetRect.Width + _formOffsetRect.X;
				}

				if ((_resizeDirection & ResizeDir.Top) == ResizeDir.Top)
				{
					int iNewHeight = _formBounds.Height + _formOffsetRect.Height + _formOffsetRect.Y;

					if (_originalForm.MaximumSize.Height != 0)
						iNewHeight = Math.Min(iNewHeight, _originalForm.MaximumSize.Height);

					iNewHeight = Math.Min(iNewHeight, SystemInformation.MaxWindowTrackSize.Height);
					iNewHeight = Math.Max(iNewHeight, _originalForm.MinimumSize.Height);
					iNewHeight = Math.Max(iNewHeight, SystemInformation.MinWindowTrackSize.Height);

					_formBounds.Y = iBottom - iNewHeight;
					_formBounds.Height = iNewHeight;
				}
				else
				{	// all other resizing are fine 
					_formBounds.Height += _formOffsetRect.Height + _formOffsetRect.Y;
				}

				// Done !!
				_originalForm.Bounds = _formBounds;
			}

			private void Resize_Stick(Rectangle toRect, bool bInsideStick)
			{
				if (_formBounds.Right >= (toRect.Left - _owner._stickGap) && _formBounds.Left <= (toRect.Right + _owner._stickGap))
				{
					if ((_resizeDirection & ResizeDir.Top) == ResizeDir.Top)
					{
						if (Math.Abs(_formBounds.Top - toRect.Bottom) <= Math.Abs(_formOffsetRect.Top) && bInsideStick)
							_formOffsetRect.Y = _formBounds.Top - toRect.Bottom;	// snap top to bottom
						else if (Math.Abs(_formBounds.Top - toRect.Top) <= Math.Abs(_formOffsetRect.Top))
							_formOffsetRect.Y = _formBounds.Top - toRect.Top;		// snap top to top
					}

					if ((_resizeDirection & ResizeDir.Bottom) == ResizeDir.Bottom)
					{
						if (Math.Abs(_formBounds.Bottom - toRect.Top) <= Math.Abs(_formOffsetRect.Bottom) && bInsideStick)
							_formOffsetRect.Height = toRect.Top - _formBounds.Bottom;	// snap Bottom to top
						else if (Math.Abs(_formBounds.Bottom - toRect.Bottom) <= Math.Abs(_formOffsetRect.Bottom))
							_formOffsetRect.Height = toRect.Bottom - _formBounds.Bottom;	// snap bottom to bottom
					}
				}

				if (_formBounds.Bottom >= (toRect.Top - _owner._stickGap) && _formBounds.Top <= (toRect.Bottom + _owner._stickGap))
				{
					if ((_resizeDirection & ResizeDir.Right) == ResizeDir.Right)
					{
						if (Math.Abs(_formBounds.Right - toRect.Left) <= Math.Abs(_formOffsetRect.Right) && bInsideStick)
							_formOffsetRect.Width = toRect.Left - _formBounds.Right;		// Stick right to left
						else if (Math.Abs(_formBounds.Right - toRect.Right) <= Math.Abs(_formOffsetRect.Right))
							_formOffsetRect.Width = toRect.Right - _formBounds.Right;	// Stick right to right
					}

					if ((_resizeDirection & ResizeDir.Left) == ResizeDir.Left)
					{
						if (Math.Abs(_formBounds.Left - toRect.Right) <= Math.Abs(_formOffsetRect.Left) && bInsideStick)
							_formOffsetRect.X = _formBounds.Left - toRect.Right;		// Stick left to right
						else if (Math.Abs(_formBounds.Left - toRect.Left) <= Math.Abs(_formOffsetRect.Left))
							_formOffsetRect.X = _formBounds.Left - toRect.Left;		// Stick left to left
					}
				}
			}

			#endregion

			#region Move Operations

			private void StartMove()
			{
				_formBounds = _originalForm.Bounds;
				_formOriginalBounds = _originalForm.Bounds;	// save original position

				if (!_originalForm.Capture)	// start capturing messages
					_originalForm.Capture = true;

				_messageProcessor = _moveMessageProcessor;
			}

			private bool MoveMsgProcessor(ref Message m)
			{	// internal message loop
				if (!_originalForm.Capture)
				{
					Cancel();
					return false;
				}

				switch (m.Msg)
				{
					case WinUser.WM_LBUTTONUP:
					{	// ok, move finished !!!
						EndMove();
						break;
					}
					case WinUser.WM_MOUSEMOVE:
					{
						_mousePosition.X = (short)Common.LoWord(m.LParam.ToInt32());
						_mousePosition.Y = (short)Common.HiWord(m.LParam.ToInt32());
						Move(_mousePosition);
						break;
					}
					case WinUser.WM_KEYDOWN:
					{
						if (m.WParam.ToInt32() == (int)Keys.Escape)
						{
							_originalForm.Bounds = _formOriginalBounds;	// set back old size
							Cancel();
						}
						break;
					}
				}

				return false;
			}

			private void EndMove()
			{
				Cancel();
			}

			#endregion

			#region Move Computing

			private void Move(Point p)
			{
				p = _originalForm.PointToScreen(p);
				Screen activeScr = Screen.FromPoint(p);	// get the screen from the point !!

				if (!activeScr.WorkingArea.Contains(p))
				{
					p.X = NormalizeInside(p.X, activeScr.WorkingArea.Left, activeScr.WorkingArea.Right);
					p.Y = NormalizeInside(p.Y, activeScr.WorkingArea.Top, activeScr.WorkingArea.Bottom);
				}

				p.Offset(-_primaryOffset.X, -_primaryOffset.Y);

				// p is the exact location of the frame - so we can play with it
				// to detect the new position acording to different bounds
				_formBounds.Location = p;	// this is the new positon of the form

				_formOffsetPoint.X = _owner._stickGap + 1;	// (more than) maximum gaps
				_formOffsetPoint.Y = _owner._stickGap + 1;

				if (_owner._stickToScreen)
					Move_Stick(activeScr.WorkingArea, false);

				// Now try to snap to other windows
				if (_owner._stickToOther)
				{
					foreach (Form sw in _snaps)
					{
						if (sw != this._originalForm)
							Move_Stick(sw.Bounds, true);
					}
				}

				if (_formOffsetPoint.X == _owner._stickGap + 1)
					_formOffsetPoint.X = 0;
				if (_formOffsetPoint.Y == _owner._stickGap + 1)
					_formOffsetPoint.Y = 0;

				_formBounds.Offset(_formOffsetPoint);

				_originalForm.Bounds = _formBounds;
			}

			/// <summary>
			/// </summary>
			/// <param name="toRect">Rect to try to snap to</param>
			/// <param name="bInsideStick">Allow snapping on the inside (eg: window to screen)</param>
			private void Move_Stick(Rectangle toRect, bool bInsideStick)
			{
				// compare distance from toRect to formRect
				// and then with the found distances, compare the most closed position
				if (_formBounds.Bottom >= (toRect.Top - _owner._stickGap) && _formBounds.Top <= (toRect.Bottom + _owner._stickGap))
				{
					if (bInsideStick)
					{
						if ((Math.Abs(_formBounds.Left - toRect.Right) <= Math.Abs(_formOffsetPoint.X)))
						{	// left 2 right
							_formOffsetPoint.X = toRect.Right - _formBounds.Left;
						}
						if ((Math.Abs(_formBounds.Left + _formBounds.Width - toRect.Left) <= Math.Abs(_formOffsetPoint.X)))
						{	// right 2 left
							_formOffsetPoint.X = toRect.Left - _formBounds.Width - _formBounds.Left;
						}
					}

					if (Math.Abs(_formBounds.Left - toRect.Left) <= Math.Abs(_formOffsetPoint.X))
					{	// snap left 2 left
						_formOffsetPoint.X = toRect.Left - _formBounds.Left;
					}
					if (Math.Abs(_formBounds.Left + _formBounds.Width - toRect.Left - toRect.Width) <= Math.Abs(_formOffsetPoint.X))
					{	// snap right 2 right
						_formOffsetPoint.X = toRect.Left + toRect.Width - _formBounds.Width - _formBounds.Left;
					}
				}

				if (_formBounds.Right >= (toRect.Left - _owner._stickGap) && _formBounds.Left <= (toRect.Right + _owner._stickGap))
				{
					if (bInsideStick)
					{
						if (Math.Abs(_formBounds.Top - toRect.Bottom) <= Math.Abs(_formOffsetPoint.Y) && bInsideStick)
						{	// Stick Top to Bottom
							_formOffsetPoint.Y = toRect.Bottom - _formBounds.Top;
						}
						if (Math.Abs(_formBounds.Top + _formBounds.Height - toRect.Top) <= Math.Abs(_formOffsetPoint.Y) && bInsideStick)
						{	// snap Bottom to Top
							_formOffsetPoint.Y = toRect.Top - _formBounds.Height - _formBounds.Top;
						}
					}

					// try to snap top 2 top also
					if (Math.Abs(_formBounds.Top - toRect.Top) <= Math.Abs(_formOffsetPoint.Y))
					{	// top 2 top
						_formOffsetPoint.Y = toRect.Top - _formBounds.Top;
					}
					if (Math.Abs(_formBounds.Top + _formBounds.Height - toRect.Top - toRect.Height) <= Math.Abs(_formOffsetPoint.Y))
					{	// bottom 2 bottom
						_formOffsetPoint.Y = toRect.Top + toRect.Height - _formBounds.Height - _formBounds.Top;
					}
				}
			}

			#endregion

			#region Utilities

			private int NormalizeInside(int iP1, int iM1, int iM2)
			{
				if (iP1 <= iM1)
					return iM1;
				else
					if (iP1 >= iM2)
						return iM2;
				return iP1;
			}

			#endregion

			#region Cancel

			private void Cancel()
			{
				_originalForm.Capture = false;
				_movingForm = false;
				_resizingForm = false;
				_messageProcessor = _defaultMessageProcessor;
			}

			#endregion

			// Internal Message Processor
			private delegate bool ProcessMessage(ref Message m);
			private ProcessMessage _messageProcessor;

			// Messages processors based on type
			private ProcessMessage _defaultMessageProcessor;
			private ProcessMessage _moveMessageProcessor;
			private ProcessMessage _resizeMessageProcessor;

			// Move stuff
			private bool _movingForm;
			private Point _formOffsetPoint;	// calculated offset rect to be added !! (min distances in all directions!!)
			private Point _primaryOffset;

			// Resize stuff
			private bool _resizingForm;
			private ResizeDir _resizeDirection;
			private Rectangle _formOffsetRect;	// calculated rect to fix the size
			private Point _mousePosition;

			// General Stuff
			private Form _originalForm;
			private NuGenUISnap _owner;
			private Rectangle _formBounds;
			private Rectangle _formOriginalBounds;

			/// <summary>
			/// Initializes a new instance of the <see cref="NuGenUISnap"/> class.
			/// </summary>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="stickyForm"/> is <see langword="null"/>.</para>
			/// -or-
			/// <para><paramref name="owner"/> is <see langword="null"/>.</para>
			/// </exception>
			public WndManager(Form stickyForm, NuGenUISnap owner)
			{
				if (stickyForm == null)
				{
					throw new ArgumentNullException("stickyForm");
				}

				if (owner == null)
				{
					throw new ArgumentNullException("owner");
				}

				_resizingForm = false;
				_movingForm = false;

				_originalForm = stickyForm;
				_owner = owner;

				_formBounds = Rectangle.Empty;
				_formOffsetRect = Rectangle.Empty;

				_formOffsetPoint = Point.Empty;
				_primaryOffset = Point.Empty;
				_mousePosition = Point.Empty;

				_defaultMessageProcessor = new ProcessMessage(DefaultMsgProcessor);
				_moveMessageProcessor = new ProcessMessage(MoveMsgProcessor);
				_resizeMessageProcessor = new ProcessMessage(ResizeMsgProcessor);
				_messageProcessor = _defaultMessageProcessor;

				AssignHandle(_originalForm.Handle);

				_originalForm.HandleCreated += delegate
				{
					AssignHandle(_originalForm.Handle);
				};

			}

			private bool _isDisposed;

			/// <summary>
			/// </summary>
			public bool IsDisposed
			{
				get
				{
					return _isDisposed;
				}
			}

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose()
			{
				if (!_isDisposed)
				{
					_isDisposed = true;
					this.ReleaseHandle();
				}
			}
		}
	}
}
