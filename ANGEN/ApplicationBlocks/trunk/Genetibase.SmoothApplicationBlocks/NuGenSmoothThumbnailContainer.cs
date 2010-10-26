/* -----------------------------------------------
 * NuGenSmoothThumbnailContainer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks;
using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothApplicationBlocks
{
	/// <summary>
	/// <seealso cref="NuGenThumbnailContainer"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothThumbnailContainer), "Resources.NuGenIcon.png")]
	public class NuGenSmoothThumbnailContainer : NuGenThumbnailContainer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothThumbnailContainer"/> class.
		/// </summary>
		public NuGenSmoothThumbnailContainer()
			: this(NuGenSmoothServiceManager.ThumbnailServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothThumbnailContainer"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenThumbnailRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothThumbnailContainer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
