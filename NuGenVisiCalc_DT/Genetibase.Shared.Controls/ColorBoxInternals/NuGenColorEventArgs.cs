/* -----------------------------------------------
 * NuGenColorBoxPopup.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.ColorBoxInternals
{
	/// <summary>
	/// </summary>
	public sealed class NuGenColorEventArgs : EventArgs
	{
		private Color _selectedColor;

		/// <summary>
		/// </summary>
		public Color SelectedColor
		{
			get
			{
				return _selectedColor;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenColorEventArgs"/> class.
		/// </summary>
		public NuGenColorEventArgs(Color selectedColor)
		{
			_selectedColor = selectedColor;
		}
	}
}
