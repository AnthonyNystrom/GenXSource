/* -----------------------------------------------
 * NuGenSmoothFontSizeBox.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Drawing;
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
	/// <seealso cref="NuGenFontSizeBox"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothFontSizeBox), "Resources.NuGenIcon.png")]
	public class NuGenSmoothFontSizeBox : NuGenFontSizeBox
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothFontSizeBox"/> class.
		/// </summary>
		public NuGenSmoothFontSizeBox()
			: this(NuGenSmoothServiceManager.ComboBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothFontSizeBox"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothFontSizeBox(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
