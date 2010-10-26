/* -----------------------------------------------
 * NuGenSmoothAlignDropDown.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Drawing;
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
	/// <seealso cref="NuGenAlignDropDown"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothAlignDropDown), "Resources.NuGenIcon.png")]
	public class NuGenSmoothAlignDropDown : NuGenAlignDropDown
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothAlignDropDown"/> class.
		/// </summary>
		public NuGenSmoothAlignDropDown()
			: this(NuGenSmoothServiceManager.AlignDropDownServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothAlignDropDown"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		/// 	<para><see cref="INuGenControlImageManager"/></para>
		/// 	<para><see cref="INuGenDropDownRenderer"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para>
		/// 	<para><see cref="INuGenRadioButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenRadioButtonRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothAlignDropDown(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
