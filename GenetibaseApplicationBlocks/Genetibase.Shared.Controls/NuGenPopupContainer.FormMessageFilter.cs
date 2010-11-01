/* -----------------------------------------------
 * NuGenPopupContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.WinApi;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenPopupContainer
	{
		private class FormMessageFilter : NativeWindow
		{
			protected override void WndProc(ref Message m)
			{
				if (m.Msg == WinUser.WM_NCACTIVATE && m.WParam == IntPtr.Zero)
				{
					m.Result = (IntPtr)1;
					return;
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
