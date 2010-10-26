/* -----------------------------------------------
 * ImageExportDialog.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.DirectorySelectorInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks.ImageExportInternals
{
	partial class ImageExportDialog
	{
		private sealed class PathSelector : NuGenDirectorySelector
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="PathSelector"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			/// 	<para><see cref="INuGenControlStateService"/></para>
			/// 	<para><see cref="INuGenDirectorySelectorRenderer"/></para>
			/// 	<para><see cref="INuGenToolStripRenderer"/></para></param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public PathSelector(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
			}
		}
	}
}
