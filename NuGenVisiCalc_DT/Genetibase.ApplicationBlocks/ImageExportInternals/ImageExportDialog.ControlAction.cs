/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	partial class ImageExportDialog
	{
		internal enum ControlAction
		{
			Back
			, Cancel
			, CancelExport
			, Close
			, Export
			, Next
		}
	}
}
