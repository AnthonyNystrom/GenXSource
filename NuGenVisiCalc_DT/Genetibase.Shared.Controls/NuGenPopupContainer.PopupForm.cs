/* -----------------------------------------------
 * NuGenPopupContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Environment;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenPopupContainer
	{
		private sealed class PopupForm : Form, IMessageFilter
		{
			#region IMessageFilter Members

			/// <summary>
			/// Filters out a message before it is dispatched.
			/// </summary>
			/// <param name="m">The message to be dispatched. You cannot modify this message.</param>
			/// <returns>
			/// true to filter the message and stop it from being dispatched; false to allow the message to continue to the next filter or control.
			/// </returns>
			public bool PreFilterMessage(ref Message m)
			{
				switch (m.Msg)
				{
					case WinUser.WM_LBUTTONDOWN:
					case WinUser.WM_MBUTTONDOWN:
					case WinUser.WM_RBUTTONDOWN:
					case WinUser.WM_XBUTTONDOWN:
					case WinUser.WM_NCLBUTTONDOWN:
					case WinUser.WM_NCMBUTTONDOWN:
					case WinUser.WM_NCRBUTTONDOWN:
					case WinUser.WM_NCXBUTTONDOWN:
					{
						if (
							!this.Bounds.Contains(Cursor.Position)
							&& User32.GetParent(m.HWnd) != User32.GetDesktopWindow() /* True for e.g. combo box drop-down. */
							)
						{
							this.Close();
						}

						break;
					}
				}

				return false;
			}

			#endregion

			#region Properties.Protected.Overridden

			/*
			 * CreateParams
			 */

			/// <summary>
			/// </summary>
			protected override CreateParams CreateParams
			{
				get
				{
					CreateParams cp = base.CreateParams;

					if (PopupForm.IsDropShadowStyleAvailable())
					{
						cp.ClassStyle |= WinUser.CS_DROPSHADOW;
					}

					return cp;
				}
			}

			/*
			 * DefaultSize
			 */

			private static readonly Size _defaultSize = new Size(50, 50);

			/// <summary>
			/// </summary>
			protected override Size DefaultSize
			{
				get
				{
					return _defaultSize;
				}
			}

			#endregion

			#region Properties.Services

			/*
			 * Renderer
			 */

			private INuGenPanelRenderer _renderer;

			/// <summary>
			/// </summary>
			/// <exception cref="NuGenServiceNotFoundException"/>
			private INuGenPanelRenderer Renderer
			{
				get
				{
					if (_renderer == null)
					{
						Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
						_renderer = this.ServiceProvider.GetService<INuGenPanelRenderer>();

						if (_renderer == null)
						{
							throw new NuGenServiceNotFoundException<INuGenPanelRenderer>();
						}
					}

					return _renderer;
				}
			}

			/*
			 * ServiceProvider
			 */

			private INuGenServiceProvider _serviceProvider;

			private INuGenServiceProvider ServiceProvider
			{
				get
				{
					return _serviceProvider;
				}
			}

			#endregion

			#region Methods.Protected.Overridden

			/*
			 * OnClosed
			 */

			/// <summary>
			/// Raises the <see cref="E:System.Windows.Forms.Form.Closed"></see> event.
			/// </summary>
			/// <param name="e">The <see cref="T:System.EventArgs"></see> that contains the event data.</param>
			protected override void OnClosed(EventArgs e)
			{
				this.Controls.Clear(); // To not dispose popup control.
				base.OnClosed(e);
			}

			/*
			 * OnHandleCreated
			 */

			/// <summary>
			/// </summary>
			/// <param name="e"></param>
			protected override void OnHandleCreated(EventArgs e)
			{
				base.OnHandleCreated(e);

				if (!base.DesignMode)
				{
					Application.AddMessageFilter(this);
				}
			}

			/*
			 * OnHandleDestroyed
			 */

			/// <summary>
			/// </summary>
			/// <param name="e"></param>
			protected override void OnHandleDestroyed(EventArgs e)
			{
				base.OnHandleDestroyed(e);

				if (!base.DesignMode)
				{
					Application.RemoveMessageFilter(this);
				}
			}

			/*
			 * OnPaint
			 */

			/// <summary>
			/// Raises the <see cref="System.Windows.Forms.Control.Paint"/> event.
			/// </summary>
			/// <param name="e">The <see cref="System.Windows.Forms.PaintEventArgs"/> instance containing the event data.</param>
			protected override void OnPaint(PaintEventArgs e)
			{
				NuGenBorderPaintParams paintParams = new NuGenBorderPaintParams(e.Graphics);
				paintParams.Bounds = this.ClientRectangle;
				paintParams.DrawBorder = true;
				paintParams.State = NuGenControlState.Normal;
				
				this.Renderer.DrawBackground(paintParams);
				this.Renderer.DrawBorder(paintParams);
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
					case WinUser.WM_ACTIVATEAPP:
					{
						this.Close();
						return;
					}
				}

				base.WndProc(ref m);
			}

			#endregion

			#region Methods.Private

			private static bool IsDropShadowStyleAvailable()
			{
				return NuGenOS.IsWindowsXP
					| NuGenOS.IsWindows2003
					| NuGenOS.IsVista
					;
			}

			#endregion

			/// <summary>
			/// Initializes a new instance of the <see cref="PopupForm"/> class.
			/// </summary>
			/// <param name="serviceProvider">
			/// <para>Requires:</para>
			/// <para><see cref="INuGenPanelRenderer"/></para>
			/// </param>
			/// <exception cref="ArgumentNullException">
			/// <para><paramref name="serviceProvider"/> is <see langword="null"/>.</para>
			/// </exception>
			public PopupForm(INuGenServiceProvider serviceProvider)
			{
				if (serviceProvider == null)
				{
					throw new ArgumentNullException("serviceProvider");
				}

				_serviceProvider = serviceProvider;

				this.ControlBox = false;
				this.FormBorderStyle = FormBorderStyle.FixedSingle;
				this.ShowInTaskbar = false;
				this.StartPosition = FormStartPosition.Manual;
			}
		}
	}
}
