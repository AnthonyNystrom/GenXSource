/* -----------------------------------------------
 * NuGenSmoothTitlePanel.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TitleInternals;
using Genetibase.Shared.Drawing;
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
	[ToolboxBitmap(typeof(NuGenSmoothTitlePanel), "Resources.NuGenIcon.png")]
	public class NuGenSmoothTitlePanel : NuGenTitlePanel
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTitlePanel"/> class.
		/// </summary>
		public NuGenSmoothTitlePanel()
			: base(NuGenSmoothServiceManager.TitlePanelServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTitlePanel"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		///		<para>Requires:</para>
		/// 	<para><see cref="INuGenControlImageManager"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenTitleRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothTitlePanel(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
