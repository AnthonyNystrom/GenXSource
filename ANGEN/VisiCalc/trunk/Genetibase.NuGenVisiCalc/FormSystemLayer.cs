/* -----------------------------------------------
 * FormSystemLayer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Genetibase.Shared.Windows;
using Genetibase.WinApi;

namespace Genetibase.NuGenVisiCalc
{
	/// <summary>
	/// Provides special methods and properties that must be implemented in a
	/// system-specific manner. It is implemented as an object that is hosted
	/// by the <see cref="VisiCalcFormBase"/> class. This way there is no inheritance hierarchy 
	/// extending into the SystemLayer assembly.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	internal class FormSystemLayer : Control
	{
		private Boolean _forceActiveTitleBar;

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
				return _forceActiveTitleBar;
			}

			set
			{
				_forceActiveTitleBar = value;
			}
		}

		private FormSystemLayer FindFormSystemLayer(Form host)
		{
			if (host != null)
			{
				Control.ControlCollection controls = host.Controls;

				for (Int32 i = 0; i < controls.Count; ++i)
				{
					FormSystemLayer formSystemLayer = controls[i] as FormSystemLayer;

					if (formSystemLayer != null)
					{
						return formSystemLayer;
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Manages some special handling of window messages.
		/// </summary>
		/// <param name="m"></param>
		/// <returns>true if the message was handled, false if the caller should handle the message.</returns>
		public Boolean HandleParentWndProc(ref Message m)
		{
			Boolean returnVal = true;

			switch (m.Msg)
			{
				case WinUser.WM_NCPAINT:
				{
					goto default;
				}
				case WinUser.WM_NCACTIVATE:
				{
					if (_forceActiveTitleBar && m.WParam == IntPtr.Zero)
					{
						if (_ignoreNcActivate > 0)
						{
							--_ignoreNcActivate;
							goto default;
						}
						else if (Form.ActiveForm != _host ||  // Gets rid of: if you have the form active, then click on the desktop --> desktop refreshes
								 !_host.Visible)              // Gets rid of: desktop refresh on exit
						{
							goto default;
						}
						else
						{
							_parentWndProc(ref m);

							User32.SendMessage(
								_host.Handle
								, WinUser.WM_NCACTIVATE
								, new IntPtr(1)
								, IntPtr.Zero
							);

							break;
						}
					}
					else
					{
						goto default;
					}
				}
				case WinUser.WM_ACTIVATE:
				{
					goto default;
				}
				case WinUser.WM_ACTIVATEAPP:
				{
					_parentWndProc(ref m);
				}

				// Check if the app is being deactivated
				if (_forceActiveTitleBar && m.WParam == IntPtr.Zero)
				{
					// If so, put our titlebar in the inactive state
					User32.PostMessage(_host.Handle, WinUser.WM_NCACTIVATE,
						IntPtr.Zero, IntPtr.Zero);

					++_ignoreNcActivate;
				}

				if (m.WParam == new IntPtr(1))
				{
					foreach (Form childForm in _host.OwnedForms)
					{
						FormSystemLayer childFormEx = FindFormSystemLayer(childForm);

						if (childFormEx != null)
						{
							if (childFormEx.ForceActiveTitleBar && childForm.IsHandleCreated)
							{
								User32.PostMessage(
									childForm.Handle
									, WinUser.WM_NCACTIVATE
									, new IntPtr(1)
									, IntPtr.Zero
								);
							}
						}
					}

					FormSystemLayer ownerEx = FindFormSystemLayer(_host.Owner);

					if (ownerEx != null)
					{
						if (ownerEx.ForceActiveTitleBar && _host.Owner.IsHandleCreated)
						{
							User32.PostMessage(
								_host.Owner.Handle
								, WinUser.WM_NCACTIVATE
								, new IntPtr(1)
								, IntPtr.Zero
							);
						}
					}
				}

				break;

				default:
				returnVal = false;
				break;
			}

			GC.KeepAlive(_host);
			return returnVal;
		}

		private Int32 _ignoreNcActivate;
		private Form _host;
		private NuGenWndProcDelegate _parentWndProc;

		public FormSystemLayer(Form host, NuGenWndProcDelegate parentWndProc)
		{
			_host = host;
			_parentWndProc = parentWndProc;
		}
	}
}
