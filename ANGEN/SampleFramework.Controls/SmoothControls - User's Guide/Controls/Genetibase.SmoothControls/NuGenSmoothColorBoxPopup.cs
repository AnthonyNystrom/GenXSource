/* -----------------------------------------------
 * NuGenSmoothColorBoxPopup.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ColorBoxInternals;
using Genetibase.Shared.Controls.ListBoxInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Represents <see cref="NuGenColorBoxPopup"/> with a smooth renderer.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothColorBoxPopup), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothColorBoxPopup : NuGenColorBoxPopup
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorBoxPopup"/> class.
		/// </summary>
		public NuGenSmoothColorBoxPopup()
			: this(NuGenSmoothServiceManager.ColorBoxPopupServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothColorBoxPopup"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		/// 	<para><see cref="INuGenTabRenderer"/></para>
		/// 	<para><see cref="INuGenTabStateTracker"/></para>
		///		<para><see cref="INuGenTabLayoutManager"/></para>
		/// 	<para><see cref="INuGenListBoxRenderer"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		/// 	<para><see cref="INuGenColorsProvider"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothColorBoxPopup(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		#endregion
	}
}
