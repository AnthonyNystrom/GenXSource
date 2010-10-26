/* -----------------------------------------------
 * NuGenDriveCombo.cs
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
	partial class NuGenDriveCombo
	{
		private sealed class DeviceChangeFilter : IMessageFilter
		{
			public event EventHandler DeviceChanged;

			private void OnDeviceChanged(EventArgs e)
			{
				if (this.DeviceChanged != null)
					this.DeviceChanged(this, e);
			}

			public bool PreFilterMessage(ref Message m)
			{
				if (m.Msg == WinUser.WM_DEVICECHANGE)
				{
					this.OnDeviceChanged(EventArgs.Empty);
				}

				return false;
			}

			public DeviceChangeFilter()
			{
			}
		}
	}
}
