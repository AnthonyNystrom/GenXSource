/* -----------------------------------------------
 * INuGenSpinRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.SpinInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenSpinRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawSpinButton(NuGenSpinButtonPaintParams paintParams);
	}
}
