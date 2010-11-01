/* -----------------------------------------------
 * INuGenLabelRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.LabelInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenLabelRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawBackground(NuGenPaintParams paintParams);
	}
}
