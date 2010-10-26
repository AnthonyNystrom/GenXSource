/* -----------------------------------------------
 * NuGenSmoothSwitcher.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
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
	[ToolboxBitmap(typeof(NuGenSmoothSwitcher), "Resources.NuGenIcon.png")]
	public class NuGenSmoothSwitcher : NuGenSwitcher
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSwitcher"/> class.
		/// </summary>
		public NuGenSmoothSwitcher()
			: this(NuGenSmoothServiceManager.SwitcherServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSwitcher"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothSwitcher(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
