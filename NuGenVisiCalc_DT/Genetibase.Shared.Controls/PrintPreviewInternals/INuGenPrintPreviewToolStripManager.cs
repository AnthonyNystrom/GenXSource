/* -----------------------------------------------
 * INuGenPrintPreviewToolStripManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.PrintPreviewInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenPrintPreviewToolStripManager
	{
		/// <summary>
		/// </summary>
		ToolStripButton GetCloseToolStripButton();
		
		/// <summary>
		/// </summary>
		ToolStripButton GetFourPagesToolStripButton();

		/// <summary>
		/// </summary>
		ToolStripLabel GetPageToolStripLabel();

		/// <summary>
		/// </summary>
		ToolStripButton GetPrintToolStripButton();

		/// <summary>
		/// </summary>
		ToolStripButton GetSinglePageToolStripButton();
		
		/// <summary>
		/// </summary>
		ToolStripButton GetTwoPagesToolStripButton();

		/// <summary>
		/// </summary>
		ToolStripDropDownButton GetZoomToolStripDropDownButton();
	}
}
