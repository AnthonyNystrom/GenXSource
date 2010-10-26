/* -----------------------------------------------
 * NuGenSmoothToolStripRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ToolStripInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.SmoothControls
{
	/// <summary>
	/// </summary>
	public sealed class NuGenSmoothToolStripRenderer : ToolStripProfessionalRenderer, INuGenToolStripRenderer
	{
		/// <summary>
		/// </summary>
		/// <returns></returns>
		public ToolStripRenderer GetToolStripRenderer()
		{
			return this;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothToolStripRenderer"/> class.
		/// </summary>
		public NuGenSmoothToolStripRenderer()
			: base(new NuGenSmoothColorTable())
		{
		}
	}
}
