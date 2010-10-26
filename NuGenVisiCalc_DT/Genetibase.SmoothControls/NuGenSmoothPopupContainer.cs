/* -----------------------------------------------
 * NuGenSmoothPopupContainer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Represents a <see cref="NuGenPopupContainer"/> with a smooth renderer.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothPopupContainer), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothPopupContainer : NuGenPopupContainer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPopupContainer"/> class.
		/// </summary>
		public NuGenSmoothPopupContainer()
			: this(NuGenSmoothServiceManager.PopupContainerServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPopupContainer"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothPopupContainer(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
