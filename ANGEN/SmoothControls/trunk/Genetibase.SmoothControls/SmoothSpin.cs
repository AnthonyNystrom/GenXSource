/* -----------------------------------------------
 * SmoothSpin.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Represents a <see cref="NuGenSmoothSpin"/> that <see cref="ToolStrip"/> and the inheritors can host.
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public class SmoothSpin : NuGenToolStripSpin
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SmoothSpin"/> class.
		/// </summary>
		public SmoothSpin()
			: this(NuGenSmoothServiceManager.SpinServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SmoothSpin"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenSpinRenderer"/></para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenControlStateTracker"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public SmoothSpin(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
