/* -----------------------------------------------
 * NuGenSmoothAlignSelector.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="NuGenAlignSelector"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenAlignSelector), "Resources.NuGenIcon.png")]
	public class NuGenSmoothAlignSelector : NuGenAlignSelector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothAlignSelector"/> class.
		/// </summary>
		public NuGenSmoothAlignSelector()
			: this(NuGenSmoothServiceManager.AlignSelectorServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothAlignSelector"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenRadioButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenRadioButtonRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothAlignSelector(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
