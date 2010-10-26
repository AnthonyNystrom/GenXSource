/* -----------------------------------------------
 * INuGenCheckBoxLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.CheckBoxInternals
{
	/// <summary>
	/// </summary>
	public interface INuGenCheckBoxLayoutManager
	{
		/// <summary>
		/// </summary>
		Size GetCheckSize();

		/// <summary>
		/// </summary>
		Rectangle GetCheckBoxBounds(NuGenBoundsParams checkBoundsParams);

		/// <summary>
		/// </summary>
		Rectangle GetTextBounds(NuGenBoundsParams textBoundsParams);
	}
}
