/* -----------------------------------------------
 * NuGenSmoothMenuButton.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SplitButtonInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
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
	[ToolboxBitmap(typeof(NuGenSmoothMenuButton), "Resources.NuGenIcon.png")]
	public class NuGenSmoothMenuButton : NuGenMenuButton
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothMenuButton"/> class.
		/// </summary>
		public NuGenSmoothMenuButton()
			: this(NuGenSmoothServiceManager.MenuButtonServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothMenuButton"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateTracker"/></para>
		/// 	<para><see cref="INuGenSplitButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenSplitButtonRenderer"/></para>
		///		<para><see cref="INuGenToolStripRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothMenuButton(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
