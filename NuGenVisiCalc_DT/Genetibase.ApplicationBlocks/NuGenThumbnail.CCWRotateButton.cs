/* -----------------------------------------------
 * NuGenThumbnail.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.ApplicationBlocks
{
	partial class NuGenThumbnail
	{
		private sealed class CCWRotateButton : RotateButton
		{
			/// <summary>
			/// Initializes a new instance of the <see cref="CCWRotateButton"/> class.
			/// </summary>
			/// <param name="serviceProvider"><para>Requires:</para>
			/// 	<para><see cref="INuGenButtonStateService"/></para>
			/// 	<para><see cref="INuGenThumbnailRenderer"/></para></param>
			/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
			public CCWRotateButton(INuGenServiceProvider serviceProvider)
				: base(serviceProvider)
			{
				this.Style = ImageRotationStyle.CCW;
			}
		}
	}
}
