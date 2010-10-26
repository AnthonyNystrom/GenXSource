/* -----------------------------------------------
 * NuGenSmoothMenuStrip.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ToolStripInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="MenuStrip"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothMenuStrip), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothMenuStrip : NuGenMenuStrip
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothMenuStrip"/> class.
		/// </summary>
		public NuGenSmoothMenuStrip()
			: this(NuGenSmoothServiceManager.ToolStripServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothMenuStrip"/> class.
		/// </summary>
		/// <param name="serviceProvider"><para>Requires:</para>
		/// 	<para><see cref="INuGenToolStripRenderer"/></para></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		/// <exception cref="NuGenServiceNotFoundException"/>
		public NuGenSmoothMenuStrip(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
