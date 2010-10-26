/* -----------------------------------------------
 * INuGenPropertyGridRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.PropertyGridInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenPropertyGridRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawDocComment(NuGenPaintParams paintParams);
	}
}
