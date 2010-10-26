/* -----------------------------------------------
 * INuGenScrollBarRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Controls.ScrollBarInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenScrollBarRenderer
	{
		/// <summary>
		/// </summary>
		void DrawBackground(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawDoubleScrollButton(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawScrollButton(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawScrollButtonBody(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawScrollTrack(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		void DrawSizeBox(NuGenPaintParams paintParams);
	}
}
