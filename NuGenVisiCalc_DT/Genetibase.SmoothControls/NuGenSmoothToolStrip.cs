/* -----------------------------------------------
 * NuGenSmoothToolStrip.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Windows;

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <see cref="ToolStrip"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothToolStrip), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothToolStrip : NuGenToolStrip
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolStrip"/> class.
		/// </summary>
		public NuGenSmoothToolStrip()
			: this(NuGenSmoothServiceManager.ToolStripServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolStrip"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenToolStripRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		/// <exception cref="NuGenServiceNotFoundException"/>
		public NuGenSmoothToolStrip(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
