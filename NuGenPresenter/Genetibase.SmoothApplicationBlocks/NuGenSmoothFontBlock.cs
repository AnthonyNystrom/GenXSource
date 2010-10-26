/* -----------------------------------------------
 * NuGenSmoothFontBlock.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.FontBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothApplicationBlocks
{
	/// <summary>
	/// <seealso cref="NuGenFontBlock"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothFontBlock), "Resources.NuGenIcon.png")]
	public class NuGenSmoothFontBlock : NuGenFontBlock
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothFontBlock"/> class.
		/// </summary>
		public NuGenSmoothFontBlock()
			: this(NuGenSmoothServiceManager.FontBlockServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothFontBlock"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		///		<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		/// 	<para><see cref="INuGenFontFamiliesProvider"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothFontBlock(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
