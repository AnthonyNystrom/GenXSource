/* -----------------------------------------------
 * NuGenHotKeyPopup.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	partial class NuGenHotKeyPopup
	{
		private sealed class Label : NuGenLabel
		{
			public Label(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.AutoSize = false;
				this.Dock = DockStyle.Fill;
				this.Margin = new Padding(3, 6, 0, 3);
			}
		}
	}
}
