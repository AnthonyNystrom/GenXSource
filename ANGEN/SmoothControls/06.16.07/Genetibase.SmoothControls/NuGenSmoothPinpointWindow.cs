/* -----------------------------------------------
 * NuGenSmoothPinpointWindow.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PinpointInternals;
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
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothPinpointWindow), "Resources.NuGenIcon.png")]
	public class NuGenSmoothPinpointWindow : NuGenPinpointWindow
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPinpointWindow"/> class.
		/// </summary>
		public NuGenSmoothPinpointWindow()
			: this(NuGenSmoothServiceManager.PinpointServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPinpointWindow"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenPinpointRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothPinpointWindow(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
