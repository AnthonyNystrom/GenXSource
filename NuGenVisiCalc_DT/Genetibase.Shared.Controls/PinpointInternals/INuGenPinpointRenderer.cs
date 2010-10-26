/* -----------------------------------------------
 * INuGenPinpointRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.PinpointInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenPinpointRenderer
	{
		/// <summary>
		/// </summary>
		void DrawFisheyeExpander(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawText(NuGenTextPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawSelectionFrame(NuGenPaintParams paintParams);
	}
}
