/* -----------------------------------------------
 * NuGenSmoothTitle.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TitleInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothTitle), "Resources.NuGenIcon.png")]
	public class NuGenSmoothTitle : NuGenTitle
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTitle"/> class.
		/// </summary>
		public NuGenSmoothTitle()
			: this(NuGenSmoothServiceManager.TitleServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTitle"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenTitleRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothTitle(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
