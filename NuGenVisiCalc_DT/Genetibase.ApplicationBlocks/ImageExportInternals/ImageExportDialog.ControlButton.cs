/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	partial class ImageExportDialog
	{
		private sealed class ControlButton : NuGenButton
		{
			private ControlAction _action;

			public ControlAction Action
			{
				get
				{
					return _action;
				}
			}

			public ControlButton(INuGenServiceProvider serviceProvider, ControlAction action)
				: base(serviceProvider)
			{
				_action = action;
			}
		}
	}
}
