/* -----------------------------------------------
 * INuGenRadioButtonRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.RadioButtonInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenRadioButtonRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawRadioButton(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawText(NuGenTextPaintParams paintParams);
	}
}
