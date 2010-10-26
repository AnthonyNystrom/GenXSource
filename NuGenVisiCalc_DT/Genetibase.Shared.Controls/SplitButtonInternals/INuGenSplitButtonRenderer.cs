/* -----------------------------------------------
 * INuGenSplitButtonRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ButtonInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.SplitButtonInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenSplitButtonRenderer : INuGenButtonRenderer
	{
		/// <summary>
		/// </summary>
		void DrawArrow(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawSplitLine(NuGenPaintParams paintParams);
	}
}
