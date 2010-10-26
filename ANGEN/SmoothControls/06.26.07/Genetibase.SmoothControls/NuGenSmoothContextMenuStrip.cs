/* -----------------------------------------------
 * NuGenSmoothContextMenuStrip.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// <seealso cref="ContextMenuStrip"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothContextMenuStrip), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothContextMenuStrip : NuGenContextMenuStrip
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothContextMenuStrip"/> class.
		/// </summary>
		public NuGenSmoothContextMenuStrip()
			: this(NuGenSmoothServiceManager.ToolStripServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothContextMenuStrip"/> class.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		/// <exception cref="NuGenServiceNotFoundException"/>
		public NuGenSmoothContextMenuStrip(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}
	}
}
