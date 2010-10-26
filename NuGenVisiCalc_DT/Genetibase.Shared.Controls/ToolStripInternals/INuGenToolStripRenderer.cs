/* -----------------------------------------------
 * INuGenToolStripRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
