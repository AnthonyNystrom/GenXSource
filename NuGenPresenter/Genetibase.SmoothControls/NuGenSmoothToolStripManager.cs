/* -----------------------------------------------
 * NuGenSmoothToolStripManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// Sets the <see cref="NuGenSmoothToolStripManager"/> for <see cref="ToolStripManager"/>.
	/// </summary>
	[ToolboxItem(true)]
	[ToolboxBitmap(typeof(NuGenSmoothToolStripManager), "Resources.NuGenIcon.png")]
	public class NuGenSmoothToolStripManager : NuGenToolStripManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolStripManager"/> class.
		/// </summary>
		public NuGenSmoothToolStripManager()
			: base(new NuGenSmoothToolStripRenderer())
		{
		}
	}
}
