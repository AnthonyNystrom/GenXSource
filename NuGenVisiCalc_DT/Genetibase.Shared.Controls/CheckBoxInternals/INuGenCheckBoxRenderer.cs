/* -----------------------------------------------
 * INuGenCheckBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		void DrawCheckBox(NuGenCheckBoxPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawText(NuGenTextPaintParams paintParams);
	}
}
