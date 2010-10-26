/* -----------------------------------------------
 * NuGenSmoothTaskBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
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
	[ToolboxBitmap(typeof(NuGenSmoothTaskBox), "Resources.NuGenIcon.png")]
	public class NuGenSmoothTaskBox : NuGenTaskBox
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTaskBox"/> class.
		/// </summary>
		public NuGenSmoothTaskBox()
			: this(NuGenSmoothServiceManager.TaskBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTaskBox"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothTaskBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
