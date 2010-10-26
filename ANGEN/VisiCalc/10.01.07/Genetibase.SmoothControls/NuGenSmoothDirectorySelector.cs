/* -----------------------------------------------
 * NuGenSmoothDirectorySelector.cs
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
	/// <seealso cref="NuGenDirectorySelector"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothDirectorySelector), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothDirectorySelector : NuGenDirectorySelector
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDirectorySelector"/> class.
		/// </summary>
		public NuGenSmoothDirectorySelector()
			: this(NuGenSmoothServiceManager.DirectorySelectorServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDirectorySelector"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothDirectorySelector(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
