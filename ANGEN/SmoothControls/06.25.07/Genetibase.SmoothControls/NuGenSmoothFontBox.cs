/* -----------------------------------------------
 * NuGenSmoothFontBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Represents a combo box which displays available fonts to the user.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothFontBox), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothFontBox : NuGenFontBox
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothFontBox"/> class.
		/// </summary>
		public NuGenSmoothFontBox()
			: this(NuGenSmoothServiceManager.FontBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothFontBox"/> class.
		/// </summary>
		/// <param name="serviceProvider">Requires:<para/>
		/// 	<see cref="INuGenComboBoxRenderer"/><para/>
		/// 	<see cref="INuGenButtonStateTracker"/><para/>
		/// 	<see cref="INuGenImageListService"/><para/>
		/// 	<see cref="INuGenFontFamiliesProvider"/><para/></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothFontBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
