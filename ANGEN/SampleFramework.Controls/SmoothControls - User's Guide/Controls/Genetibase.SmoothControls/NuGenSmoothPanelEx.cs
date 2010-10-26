/* -----------------------------------------------
 * NuGenSmoothRoundedPanel.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelExInternals;
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
	[ToolboxBitmap(typeof(NuGenSmoothPanelEx), "Resources.NuGenIcon.png")]
	public class NuGenSmoothPanelEx : NuGenPanelEx
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPanelEx"/> class.
		/// </summary>
		public NuGenSmoothPanelEx()
			: this(NuGenSmoothServiceManager.PanelExServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPanelEx"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothPanelEx(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
