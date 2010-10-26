/* -----------------------------------------------
 * INuGenTextBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		void DrawBorder(NuGenBorderPaintParams paintParams);
	}
}
