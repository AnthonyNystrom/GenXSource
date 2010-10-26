/* -----------------------------------------------
 * NuGenSmoothCalculatorPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
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
	[ToolboxBitmap(typeof(NuGenSmoothCalculatorPopup), "Resources.NuGenIcon.png")]
	public class NuGenSmoothCalculatorPopup : NuGenCalculatorPopup
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalculatorPopup"/> class.
		/// </summary>
		public NuGenSmoothCalculatorPopup()
			: this(NuGenSmoothServiceManager.CalculatorPopupServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalculatorPopup"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenButtonRenderer"/></para>
		/// 	<para><see cref="INuGenButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para>
		/// 	<para><see cref="INuGenTextBoxRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothCalculatorPopup(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
