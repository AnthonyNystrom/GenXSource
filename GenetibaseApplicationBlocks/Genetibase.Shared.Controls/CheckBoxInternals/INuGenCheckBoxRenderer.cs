/* -----------------------------------------------
 * INuGenCheckBoxRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.CheckBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenCheckBoxRenderer
	{
		/// <summary>
		/// </summary>
		void DrawCheckBox(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawText(NuGenTextPaintParams paintParams);
	}
}
