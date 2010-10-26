/* -----------------------------------------------
 * INuGenComboBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.ComboBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenComboBoxRenderer : INuGenItemRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawComboBoxButton(NuGenPaintParams paintParams);
	}
}
