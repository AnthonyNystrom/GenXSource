/* -----------------------------------------------
 * INuGenRadioButtonLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.RadioButtonInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenRadioButtonLayoutManager
	{
		/// <summary>
		/// </summary>
		Size GetRadioSize();

		/// <summary>
		/// </summary>
		Rectangle GetRadioButtonBounds(NuGenBoundsParams radioBoundsParams);

		/// <summary>
		/// </summary>
		Rectangle GetTextBounds(NuGenBoundsParams textBoundsParams);
	}
}
