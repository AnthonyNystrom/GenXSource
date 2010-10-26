/* -----------------------------------------------
 * NuGenPopupContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Windows;
using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenPopupContainer
	{
		[SecurityPermission(SecurityAction.LinkDemand)]
		[SecurityPermission(SecurityAction.InheritanceDemand)]
		private sealed class FormMessageFilter : NuGenNativeWindow
		{
			private static readonly object _hostClosed = new object();

			public event EventHandler HostClosed
			{
				add
				{
					this.Events.AddHandler(_hostClosed, value);
				}
				remove
				{
					this.Events.RemoveHandler(_hostClosed, value);
				}
			}

			private void OnHostClosed(EventArgs e)
			{
				this.Initiator.InvokeEventHandler(_hostClosed, e);
			}

			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WinUser.WM_NCACTIVATE && m.WParam == IntPtr.Zero)
				{
					m.Result = (IntPtr)1;
					return;
				}
				else if (m.Msg == WinUser.WM_DESTROY)
				{
					this.OnHostClosed(EventArgs.Empty);
				}

				base.WndProc(ref m);
			}

			public FormMessageFilter(Control popupHostControl)
			{
				Debug.Assert(popupHostControl != null, "popupHostControl != null");

				Control topLevelControl = popupHostControl.TopLevelControl;

				if (topLevelControl != null)
				{
					if (topLevelControl.IsHandleCreated)
					{
						this.AssignHandle(topLevelControl.Handle);
					}
					else
					{
						topLevelControl.HandleCreated += delegate
						{
							this.AssignHandle(topLevelControl.Handle);
						};
					}

					topLevelControl.HandleDestroyed += delegate
					{
						this.ReleaseHandle();
					};
				}
			}
		}
	}
}
