/* -----------------------------------------------
 * INuGenLabelRenderer.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

namespace Genetibase.Shared.Controls.LabelInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenLabelRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawImage(NuGenImagePaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawText(NuGenTextPaintParams paintParams);
	}
}
