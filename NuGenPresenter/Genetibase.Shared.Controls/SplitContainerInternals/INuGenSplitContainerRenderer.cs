/* -----------------------------------------------
 * INuGenSplitContainerRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
