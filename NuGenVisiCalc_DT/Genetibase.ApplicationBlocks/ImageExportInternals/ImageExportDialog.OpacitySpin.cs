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
		private sealed class OpacitySpin : NuGenSpin
		{
			public OpacitySpin(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.Minimum = 0;
				this.Maximum = 100;
				this.Value = 100;
			}
		}
	}
}
