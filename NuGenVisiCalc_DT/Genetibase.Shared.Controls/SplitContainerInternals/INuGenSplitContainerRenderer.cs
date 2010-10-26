/* -----------------------------------------------
 * INuGenSplitContainerRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.SplitContainerInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenSplitContainerRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawPanel(NuGenBorderPaintParams paintParams);
	}
}
