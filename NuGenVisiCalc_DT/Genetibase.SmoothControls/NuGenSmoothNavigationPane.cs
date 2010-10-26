/* -----------------------------------------------
 * NuGenSmoothNavigationPane.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="NuGenNavigationPane"/>
	/// </summary>
	[Designer("Genetibase.SmoothControls.Design.NuGenSmoothNavigationPaneDesigner")]
	public class NuGenSmoothNavigationPane : NuGenNavigationPane
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothNavigationPane"/> class.
		/// </summary>
		public NuGenSmoothNavigationPane()
			: base(NuGenSmoothServiceManager.NavigationBarServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothNavigationPane"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenControlStateTracker"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothNavigationPane(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
