/* -----------------------------------------------
 * INuGenTextBoxRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.TextBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenTextBoxRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawBorder(NuGenPaintParams paintParams);
	}
}
