/* -----------------------------------------------
 * VisiCalcFormBase.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

namespace Genetibase.NuGenVisiCalc
{
	/// <summary>
	/// This Form class is used to fix a few bugs in Windows Forms, and to add a few performance
	/// enhancements, such as disabling opacity != 1.0 when running in a remote TS/RD session.
	/// We derive from this class instead of <see cref="System.Windows.Forms.Form"/> directly.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	internal class VisiCalcFormBase : Form
	{
		#region Events

		public event MovingEventHandler Moving;

		protected virtual void OnMoving(MovingEventArgs mea)
		{
			if (Moving != null)
			{
				Moving(this, mea);
			}
		}

		public event CancelEventHandler QueryEndSession;

		protected virtual void OnQueryEndSession(CancelEventArgs e)
		{
			if (QueryEndSession != null)
			{
				QueryEndSession(this, e);
			}
		}

		#endregion

		#region Events.Static

		/*
		 * EnableOpacityChanged
		 */

		public static EventHandler EnableOpacityChanged;

		private static void OnEnableOpacityChanged()
		{
			if (EnableOpacityChanged != null)
			{
				EnableOpacityChanged(null, EventArgs.Empty);
			}
		}

		/*
		 * ForceActiveTitleBar
		 */

		/// <summary>
		/// Gets or sets the titlebar rendering behavior for when the form is deactivated.
		/// </summary>
		/// <remarks>
		/// If this property is false, the titlebar will be rendered in a different color when the form
		/// is inactive as opposed to active. If this property is true, it will always render with the
		/// active style. If the whole application is deactivated, the title bar will still be drawn in
		/// an inactive state.
		/// </remarks>
		public Boolean ForceActiveTitleBar
		{
			get
			{
				return _formSystemLayer.ForceActiveTitleBar;
			}

			set
			{
				_formSystemLayer.ForceActiveTitleBar = value;
			}
		}

		#endregion

		#region Properties.Public

		/*
		 * EnableInstanceOpacity
		 */

		public Boolean EnableInstanceOpacity
		{
			get
			{
				return _instanceEnableOpacity;
			}

			set
			{
				_instanceEnableOpacity = value;
				this.DecideOpacitySetting();
			}
		}

		/*
		 * Opacity
		 */

		/// <summary>
		/// Sets the opacity of the form.
		/// </summary>
		/// <param name="newOpacity">The new opacity value.</param>
		/// <remarks>
		/// Depending on the system configuration, this request may be ignored. For example,
		/// when running within a Terminal Service (or Remote Desktop) session, opacity will
		/// always be set to 1.0 for performance reasons.
		/// </remarks>
		public new Double Opacity
		{
			get
			{
				return _ourOpacity;
			}

			set
			{
				if (_enableOpacity)
				{
					// Bypassing Form.Opacity eliminates a "black flickering" that occurs when
					// the form transitions from Opacity=1.0 to Opacity != 1.0, or vice versa.
					// It appears to be a result of toggling the WS_EX_LAYERED style, or the
					// fact that Form.Opacity re-applies visual styles when this value transition
					// takes place.
					SetFormOpacity(this, value);
				}

				_ourOpacity = value;
			}
		}

		/*
		 * ScreenAspect
		 */

		/// <summary>
		/// </summary>
		public Double ScreenAspect
		{
			get
			{
				Rectangle bounds = System.Windows.Forms.Screen.FromControl(this).Bounds;
				Double aspect = (Double)bounds.Width / (Double)bounds.Height;
				return aspect;
			}
		}

		#endregion

		#region Properties.Public.Static

		/// <summary>
		/// Gets or sets a flag that enables or disables opacity for all PdnBaseForm instances.
		/// If a particular form's EnableInstanceOpacity property is false, that will override
		/// this property being 'true'.
		/// </summary>
		public static Boolean EnableOpacity
		{
			get
			{
				return _globalEnableOpacity;
			}

			set
			{
				_globalEnableOpacity = value;
				OnEnableOpacityChanged();
			}
		}

		#endregion

		#region Methods.Private

		/*
		 * DecideOpacitySettings
		 */

		/// <summary>
		/// Decides whether or not to have opacity be enabled.
		/// </summary>
		private void DecideOpacitySetting()
		{
			if (!VisiCalcFormBase._globalEnableOpacity || !this.EnableInstanceOpacity)
			{
				if (_enableOpacity)
				{
					try
					{
						SetFormOpacity(this, 1.0);
					}

					// This fails in certain odd situations (bug #746), so we just eat the exception.
					catch (System.ComponentModel.Win32Exception)
					{
					}
				}

				_enableOpacity = false;
			}
			else
			{
				if (!_enableOpacity)
				{
					// This fails in certain odd situations, so we just eat the exception.
					try
					{
						SetFormOpacity(this, _ourOpacity);
					}

					catch (System.ComponentModel.Win32Exception)
					{
					}
				}

				_enableOpacity = true;
			}
		}

		/*
		 * OurWndProc
		 */

		private void OurWndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case WinUser.WM_MOVING:
				{
					unsafe
					{
						Int32* p = (Int32*)m.LParam;
						Rectangle rect = Rectangle.FromLTRB(p[0], p[1], p[2], p[3]);

						MovingEventArgs mea = new MovingEventArgs(rect);
						OnMoving(mea);

						p[0] = mea.Rectangle.Left;
						p[1] = mea.Rectangle.Top;
						p[2] = mea.Rectangle.Right;
						p[3] = mea.Rectangle.Bottom;

						m.Result = new IntPtr(1);
					}
					break;
				}
				// WinForms doesn't handle this message correctly and wrongly returns 0 instead of 1.
				case WinUser.WM_QUERYENDSESSION:
				{
					CancelEventArgs e = new CancelEventArgs();
					OnQueryEndSession(e);
					m.Result = e.Cancel ? IntPtr.Zero : new IntPtr(1);
					break;
				}
				default:
				{
					base.WndProc(ref m);
					break;
				}
			}
		}

		/*
		 * ParsePair
		 */

		private void ParsePair(String theString, out Int32 x, out Int32 y)
		{
			String[] split = theString.Split(',');
			x = Int32.Parse(split[0]);
			y = Int32.Parse(split[1]);
		}

		/*
		 * RealWndProc
		 */

		private void RealWndProc(ref Message m)
		{
			OurWndProc(ref m);
		}

		#endregion

		#region Methods.Private.Static

		/// <summary>
		/// </summary>
		/// <param name="form"></param>
		/// <param name="opacity"></param>
		/// <remarks>
		/// Note to implementors: This may be implemented as just "form.Opacity = opacity".
		/// This method works around some visual clumsiness in .NET 2.0 related to
		/// transitioning between opacity == 1.0 and opacity != 1.0.</remarks>
		private static void SetFormOpacity(Form form, Double opacity)
		{
			if (opacity < 0.0 || opacity > 1.0)
			{
				throw new ArgumentOutOfRangeException("opacity", "must be in the range [0, 1]");
			}

			if (System.Environment.OSVersion.Version >= Genetibase.Shared.Environment.NuGenOS.WindowsXPVersion)
			{
				Int32 exStyle = Window.GetExStyle(form.Handle);

				Byte bOldAlpha = 255;

				if ((exStyle & WinUser.GWL_EXSTYLE) != 0)
				{
					Int32 dwOldKey;
					Int32 dwOldFlags;
					Boolean result = User32.GetLayeredWindowAttributes(form.Handle, out dwOldKey, out bOldAlpha, out dwOldFlags);
				}

				Byte bNewAlpha = (Byte)(opacity * 255.0);
				Int32 newExStyle = exStyle;

				if (bNewAlpha != 255)
				{
					newExStyle |= WinUser.WS_EX_LAYERED;
				}

				if (newExStyle != exStyle || (newExStyle & WinUser.WS_EX_LAYERED) != 0)
				{
					if (newExStyle != exStyle)
					{
						User32.SetWindowLong(form.Handle, WinUser.GWL_EXSTYLE, newExStyle);
					}

					if ((newExStyle & WinUser.WS_EX_LAYERED) != 0)
					{
						User32.SetLayeredWindowAttributes(form.Handle, 0, bNewAlpha, WinUser.LWA_ALPHA);
					}
				}

				GC.KeepAlive(form);
			}
			else
			{
				form.Opacity = opacity;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnClosing
		 */

		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			if (!e.Cancel)
			{
				this.ForceActiveTitleBar = false;
			}
		}

		/*
		 * OnHandleCreated
		 */

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			VisiCalcFormBase.EnableOpacityChanged += new EventHandler(_enableOpacityChangedHandler);
			DecideOpacitySetting();
		}

		/*
		 * OnHandleDestroyed
		 */

		protected override void OnHandleDestroyed(EventArgs e)
		{
			base.OnHandleDestroyed(e);
			VisiCalcFormBase.EnableOpacityChanged -= new EventHandler(_enableOpacityChangedHandler);
		}

		/*
		 * WndProc
		 */

		protected override void WndProc(ref Message m)
		{
			if (_formSystemLayer == null)
			{
				base.WndProc(ref m);
			}
			else if (!_formSystemLayer.HandleParentWndProc(ref m))
			{
				OurWndProc(ref m);
			}
		}

		#endregion

		#region EventHandlers

		private void _enableOpacityChangedHandler(Object sender, EventArgs e)
		{
			this.DecideOpacitySetting();
		}

		private void _userSessions_SessionChanged(Object sender, EventArgs e)
		{
			this.DecideOpacitySetting();
		}

		#endregion

		private Boolean _enableOpacity = true;
		private Double _ourOpacity = 1.0; // store opacity setting so that when we go from disabled->enabled opacity we can set the correct value

		private Boolean _instanceEnableOpacity = true;
		private static Boolean _globalEnableOpacity = true;
		private FormSystemLayer _formSystemLayer;

		public VisiCalcFormBase()
		{
			_formSystemLayer = new FormSystemLayer(this, new NuGenWndProcDelegate(this.RealWndProc));
			this.Controls.Add(_formSystemLayer);
			_formSystemLayer.Visible = false;
			DecideOpacitySetting();
			this.ResumeLayout(false);
		}
	}
}
