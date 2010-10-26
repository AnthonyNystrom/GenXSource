/* -----------------------------------------------
 * NuGenSmoothSwitchButton.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
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
	[ToolboxBitmap(typeof(NuGenSwitchButton), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothSwitchButton : NuGenSwitchButton
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSwitchButton"/> class.
		/// </summary>
		public NuGenSmoothSwitchButton()
			: this(NuGenSmoothServiceManager.SwitcherServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSwitchButton"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothSwitchButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
