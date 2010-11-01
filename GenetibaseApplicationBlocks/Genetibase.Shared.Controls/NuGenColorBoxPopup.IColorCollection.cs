/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls.ColorBoxInternals;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	partial class NuGenColorBoxPopup
	{
		private interface IColorCollection
		{
			/// <summary>
			/// Occurs when a color was selected in a collection.
			/// </summary>
			event EventHandler<NuGenColorEventArgs> ColorSelected;

			/// <summary>
			/// Returns <see langword="true"/> if the specified <paramref name="color"/> is contained
			/// within the collection; otherwise, <see langword="false"/>.
			/// </summary>
			bool SetSelectedColor(Color color);
		}
	}
}
