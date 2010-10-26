/* -----------------------------------------------
 * INuGenToolStripRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.ToolStripInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenToolStripRenderer
	{
		/// <summary>
		/// </summary>
		ToolStripRenderer GetToolStripRenderer();
	}
}
