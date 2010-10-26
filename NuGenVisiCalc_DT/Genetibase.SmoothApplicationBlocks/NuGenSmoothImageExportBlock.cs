/* -----------------------------------------------
 * NuGenSmoothImageExportBlock.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.ApplicationBlocks;
using Genetibase.ApplicationBlocks.ImageExportInternals;
using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Controls.ComboBoxInternals;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Controls.ScrollBarInternals;
using Genetibase.Shared.Controls.SwitcherInternals;
using Genetibase.Shared.Controls.TextBoxInternals;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothApplicationBlocks
{
	/// <summary>
	/// <seealso cref="NuGenImageExportBlock"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothImageExportBlock), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothImageExportBlock : NuGenImageExportBlock
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothImageExportBlock"/> class.
		/// </summary>
		public NuGenSmoothImageExportBlock()
			: this(NuGenSmoothServiceManager.ImageExportServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothImageExportBlock"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenButtonStateService"/></para>
		/// 	<para><see cref="INuGenControlStateService"/></para>
		/// 	<para><see cref="INuGenButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenButtonRenderer"/></para>
		/// 	<para><see cref="INuGenComboBoxRenderer"/></para>
		/// 	<para><see cref="INuGenImageListService"/></para>
		/// 	<para><see cref="INuGenPanelRenderer"/></para>
		/// 	<para><see cref="INuGenScrollBarRenderer"/></para>
		/// 	<para><see cref="INuGenSwitchButtonLayoutManager"/></para>
		/// 	<para><see cref="INuGenSwitchButtonRenderer"/></para>
		/// 	<para><see cref="INuGenTextBoxRenderer"/></para>
		/// 	<para><see cref="INuGenTrackBarRenderer"/></para>
		/// 	<para><see cref="INuGenThumbnailLayoutManager"/></para>
		/// 	<para><see cref="INuGenThumbnailRenderer"/></para>
		/// 	<para><see cref="INuGenToolStripRenderer"/></para>
		/// 	<para><see cref="INuGenValueTrackerService"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothImageExportBlock(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
