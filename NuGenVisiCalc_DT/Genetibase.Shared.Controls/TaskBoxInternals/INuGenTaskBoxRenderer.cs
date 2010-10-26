/* -----------------------------------------------
 * INuGenTaskBoxRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.TaskBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenTaskBoxRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawCollapseButton(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawHeader(NuGenItemPaintParams paintParams);
	}
}
