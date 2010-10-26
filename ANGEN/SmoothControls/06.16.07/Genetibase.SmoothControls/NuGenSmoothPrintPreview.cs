/* -----------------------------------------------
 * NuGenSmoothPrintPreview.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PrintPreviewInternals;
using Genetibase.Shared.Controls.ToolStripInternals;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Represents a <see cref="NuGenPrintPreview"/> with a smooth renderer.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothPrintPreview), "Resources.NuGenIcon.png")]
	public class NuGenSmoothPrintPreview : NuGenPrintPreview
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPrintPreview"/> class.
		/// </summary>
		public NuGenSmoothPrintPreview()
			: this(NuGenSmoothServiceManager.PrintPreviewServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPrintPreview"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		///		<para>Requires:</para>
		/// 	<para><see cref="INuGenPrintPreviewToolStripManager"/></para>
		///		<para><see cref="INuGenToolStripRenderer"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		public NuGenSmoothPrintPreview(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
