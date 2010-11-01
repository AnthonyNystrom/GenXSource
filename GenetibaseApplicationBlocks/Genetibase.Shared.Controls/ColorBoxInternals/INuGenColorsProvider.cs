/* -----------------------------------------------
 * INuGenColorsProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.ColorBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenColorsProvider
	{
		/// <summary>
		/// </summary>
		void FillWithCustomColors(out List<Color> customColors);
		
		/// <summary>
		/// </summary>
		void FillWithStandardColors(out List<Color> standardColors);

		/// <summary>
		/// </summary>
		void FillWithWebColors(out List<Color> webColors);

		/// <summary>
		/// </summary>
		void FillWithColorSamples(List<Color> colors, out ImageList imageListToFill);
	}
}
