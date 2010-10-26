/* -----------------------------------------------
 * NuGenSmoothDialogBlock.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks;
using Genetibase.ApplicationBlocks.DialogInternals;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothApplicationBlocks
{
	/// <summary>
	/// <seealso cref="NuGenDialogBlock"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothDialogBlock), "Resources.NuGenIcon.png")]
	[Designer("Genetibase.SmoothApplicationBlocks.Design.NuGenSmoothDialogBlockDesigner")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothDialogBlock : NuGenDialogBlock
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDialogBlock"/> class.
		/// </summary>
		public NuGenSmoothDialogBlock()
			: this(NuGenSmoothServiceManager.DialogBlockServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothDialogBlock"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenDialogBlockLayoutManager"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothDialogBlock(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
