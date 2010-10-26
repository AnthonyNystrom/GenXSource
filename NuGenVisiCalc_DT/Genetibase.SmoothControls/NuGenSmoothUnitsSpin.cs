/* -----------------------------------------------
 * NuGenSmoothUnitsSpin.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="NuGenSmoothUnitsSpin"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothUnitsSpin), "Resources.NuGenIcon.png")]
	public class NuGenSmoothUnitsSpin : NuGenUnitsSpin
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothUnitsSpin"/> class.
		/// </summary>
		public NuGenSmoothUnitsSpin()
			: this(NuGenSmoothServiceManager.UnitsSpinServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothUnitsSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para>
		/// 	<para><see cref="INuGenInt32ValueConverter"/></para>
		/// 	<para><see cref="INuGenMeasureUnitsValueConverter"/></para>
		/// 	<para><see cref="INuGenSpinRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothUnitsSpin(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
