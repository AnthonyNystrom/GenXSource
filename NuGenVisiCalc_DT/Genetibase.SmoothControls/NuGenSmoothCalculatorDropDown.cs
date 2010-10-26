/* -----------------------------------------------
 * NuGenSmoothCalculatorDropDown.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
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
	[ToolboxBitmap(typeof(NuGenSmoothCalculatorDropDown), "Resources.NuGenIcon.png")]
	public class NuGenSmoothCalculatorDropDown : NuGenCalculatorDropDown
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalculatorDropDown"/> class.
		/// </summary>
		public NuGenSmoothCalculatorDropDown()
			: this(NuGenSmoothServiceManager.CalculatorDropDownServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCalculatorDropDown"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		///		<para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		///		<para><see cref="INuGenTextBoxRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothCalculatorDropDown(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
