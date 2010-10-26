/* -----------------------------------------------
 * NuGenSmoothScrollButton.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ScrollBarInternals;
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
	[ToolboxBitmap(typeof(NuGenSmoothScrollButton), "Resources.NuGenIcon.png")]
	public class NuGenSmoothScrollButton : NuGenScrollButton
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothScrollButton"/> class.
		/// </summary>
		public NuGenSmoothScrollButton()
			: this(NuGenSmoothServiceManager.ScrollBarServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothScrollButton"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenButtonStateService"/><para/>
		/// 	<see cref="INuGenScrollBarRenderer"/><para/>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothScrollButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
