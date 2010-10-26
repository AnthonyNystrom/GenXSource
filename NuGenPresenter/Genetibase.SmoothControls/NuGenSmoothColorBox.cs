/* -----------------------------------------------
 * NuGenSmoothColorBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.DropDownInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <see cref="NuGenColorBox"/> with a smooth renderer.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothColorBox), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothColorBox : NuGenColorBox
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorBox"/> class.
		/// </summary>
		public NuGenSmoothColorBox()
			: this(NuGenSmoothServiceManager.ColorBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorBox"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		///		<para><see cref="INuGenDropDownRenderer"/></para>
		/// 	<para><see cref="INuGenTabRenderer"/></para>
		/// 	<para><see cref="INuGenTabStateTracker"/></para>
		/// 	<para><see cref="INuGenListBoxRenderer"/></para>
		/// 	<para><see cref="INuGenButtonRenderer"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		/// 	<para><see cref="INuGenColorsProvider"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothColorBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
