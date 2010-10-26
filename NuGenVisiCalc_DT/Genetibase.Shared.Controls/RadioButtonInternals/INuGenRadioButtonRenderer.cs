/* -----------------------------------------------
 * INuGenRadioButtonRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		void DrawRadioButton(NuGenRadioButtonPaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawText(NuGenTextPaintParams paintParams);
	}
}
