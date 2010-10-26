/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	partial class NuGenColorBoxPopup
	{
		private sealed class ColorListItem
		{
			private Color _displayColor;

			public Color DisplayColor
			{
				get
				{
					return _displayColor;
				}
			}

			public override string ToString()
			{
				return this.DisplayColor.Name;
			}

			public ColorListItem(Color displayColor)
			{
				_displayColor = displayColor;
			}
		}
	}
}
