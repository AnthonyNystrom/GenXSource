/* -----------------------------------------------
 * NuGenSmoothScrollBar.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Scroll bar with smooth renderer.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothScrollBar), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothScrollBar : NuGenScrollBar
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothScrollBar"/> class.
		/// </summary>
		public NuGenSmoothScrollBar()
			: this(NuGenSmoothServiceManager.ScrollBarServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothScrollBar"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenButtonStateTracker"/><para/>
		/// 	<see cref="INuGenControlStateTracker"/><para/>
		/// 	<see cref="INuGenValueTracker"/><para/>
		/// 	<see cref="INuGenScrollBarRenderer"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothScrollBar(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
