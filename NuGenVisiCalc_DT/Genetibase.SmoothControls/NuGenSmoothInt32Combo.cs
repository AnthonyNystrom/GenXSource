/* -----------------------------------------------
 * NuGenSmoothInt32Combo.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
	/// <seealso cref="NuGenInt32Combo"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothInt32Combo), "Resources.NuGenIcon.png")]
	public class NuGenSmoothInt32Combo : NuGenInt32Combo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothInt32Combo"/> class.
		/// </summary>
		public NuGenSmoothInt32Combo()
			: this(NuGenSmoothServiceManager.ComboBoxServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothInt32Combo"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		/// 	<para><see cref="INuGenInt32ValueConverter"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothInt32Combo(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
