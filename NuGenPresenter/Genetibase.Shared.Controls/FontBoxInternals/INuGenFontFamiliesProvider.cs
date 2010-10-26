/* -----------------------------------------------
 * INuGenFontFamiliesProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.FontBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenFontFamiliesProvider
	{
		/// <summary>
		/// </summary>
		/// <param name="collectionToFill"></param>
		void FillWithFontNames(out IList<string> collectionToFill);

		/// <summary>
		/// </summary>
		/// <param name="fontNames"></param>
		/// <param name="imageListToFill"></param>
		void FillWithFontSamples(IList<string> fontNames, out ImageList imageListToFill);
	}
}
