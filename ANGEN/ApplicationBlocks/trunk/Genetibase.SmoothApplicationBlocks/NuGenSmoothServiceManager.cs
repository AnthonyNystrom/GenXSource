/* -----------------------------------------------
 * NuGenSmoothServiceManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.SmoothApplicationBlocks.DialogInternals;
using Genetibase.SmoothApplicationBlocks.FontInternals;
using Genetibase.SmoothApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.SmoothControls;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothApplicationBlocks
{
	internal static class NuGenSmoothServiceManager
	{
		public static readonly INuGenServiceProvider DialogBlockServiceProvider = new NuGenSmoothDialogBlockServiceProvider();
		public static readonly INuGenServiceProvider FontBlockServiceProvider = new NuGenSmoothFontBlockServiceProvider();
		public static readonly INuGenServiceProvider ImageExportServiceProvider = new NuGenSmoothImageExportServiceProvider();
		public static readonly INuGenServiceProvider SmoothServiceProvider = new NuGenSmoothServiceProvider();
		public static readonly INuGenServiceProvider ThumbnailServiceProvider = new NuGenSmoothThumbnailContainerServiceProvider();
	}
}
