/* -----------------------------------------------
 * INuGenLinkLabelRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.LinkLabelInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenLinkLabelRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawImage(NuGenImagePaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawText(NuGenTextPaintParams paintParams);
	}
}
