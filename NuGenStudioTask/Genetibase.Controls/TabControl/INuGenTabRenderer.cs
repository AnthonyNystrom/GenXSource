/* -----------------------------------------------
 * INuGenTabRenderer.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Genetibase.Controls
{
	/// <summary>
	/// </summary>
	public interface INuGenTabRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="flatStyle"></param>
		/// <returns></returns>
		Padding GetPadding(FlatStyle flatStyle);

		/// <summary>
		/// </summary>
		void DrawTabBody(NuGenTabBodyPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawTabButton(NuGenTabButtonPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawTabPage(NuGenTabPagePaintParams paintParams);
	}
}
