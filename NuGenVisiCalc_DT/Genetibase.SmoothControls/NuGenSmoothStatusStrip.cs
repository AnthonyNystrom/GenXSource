/* -----------------------------------------------
 * NuGenSmoothStatusStrip.cs
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
	/// <seealso cref="StatusStrip"/>
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothStatusStrip), "Resources.NuGenIcon.png")]
	[System.ComponentModel.DesignerCategory("Code")]
	public class NuGenSmoothStatusStrip : NuGenStatusStrip
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothStatusStrip"/> class.
		/// </summary>
		public NuGenSmoothStatusStrip()
			: this(NuGenSmoothServiceManager.ToolStripServiceProvider)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothStatusStrip"/> class.
		/// </summary>
		/// <param name="serviceProvider"></param>
		/// <exception cref="ArgumentNullException"><paramref name="serviceProvider"/> is <see langword="null"/>.</exception>
		/// <exception cref="NuGenServiceNotFoundException"/>
		public NuGenSmoothStatusStrip(INuGenServiceProvider serviceProvider)
			: base(serviceProvider)
		{

		}
	}
}
