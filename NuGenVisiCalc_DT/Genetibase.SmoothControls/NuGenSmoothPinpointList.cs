/* -----------------------------------------------
 * NuGenSmoothPinpointList.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
using Genetibase.SmoothControls.ComponentModel;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothPinpointList), "Resources.NuGenIcon.png")]
	public class NuGenSmoothPinpointList : NuGenPinpointList
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPinpointList"/> class.
		/// </summary>
		public NuGenSmoothPinpointList()
			: this(NuGenSmoothServiceManager.PinpointServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPinpointList"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenPinpointLayoutManager"/></para>
		/// 	<para><see cref="INuGenPinpointRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothPinpointList(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
