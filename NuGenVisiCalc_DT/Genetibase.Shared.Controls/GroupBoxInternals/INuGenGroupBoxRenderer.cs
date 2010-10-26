/* -----------------------------------------------
 * INuGenGroupBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.GroupBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenGroupBoxRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawFrame(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawLabel(NuGenTextPaintParams paintParams);
	}
}
