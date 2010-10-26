/* -----------------------------------------------
 * INuGenDropDownRenderer.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.DropDownInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenDropDownRenderer
	{
		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawBorder(NuGenPaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawDropDownBody(NuGenItemPaintParams paintParams);

		/// <summary>
		/// </summary>
		/// <param name="paintParams"></param>
		void DrawDropDownButton(NuGenPaintParams paintParams);
	}
}
