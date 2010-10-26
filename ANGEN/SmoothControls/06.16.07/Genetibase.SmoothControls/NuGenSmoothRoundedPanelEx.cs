/* -----------------------------------------------
 * NuGenSmoothRoundedPanelEx.cs
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
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Represents a <see cref="NuGenPanelEx"/> analogue with rounded corners.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothRoundedPanelEx), "Resources.NuGenIcon.png")]
	public class NuGenSmoothRoundedPanelEx : NuGenPanelEx
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothRoundedPanelEx"/> class.
		/// </summary>
		public NuGenSmoothRoundedPanelEx()
			: this(NuGenSmoothServiceManager.RoundedPanelExServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothRoundedPanelEx"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenPanelExRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothRoundedPanelEx(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
