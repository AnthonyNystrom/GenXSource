/* -----------------------------------------------
 * NuGenSmoothNavigationBar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.NavigationBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Microsoft Outlook like navigation bar with a smooth renderer.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothNavigationBar), "Resources.NuGenIcon.png")]
	[Designer("Genetibase.SmoothControls.Design.NuGenSmoothNavigationBarDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothNavigationBar : NuGenNavigationBar
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothNavigationBar"/> class.
		/// </summary>
		public NuGenSmoothNavigationBar()
			: this(NuGenSmoothServiceManager.NavigationBarServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothNavigationBar"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenNavigationBarRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothNavigationBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
