/* -----------------------------------------------
 * NuGenCheckBoxLayoutManager.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
	public class NuGenCheckBoxLayoutManager : NuGenControlLayoutManager, INuGenCheckBoxLayoutManager
	{
		/// <summary>
		/// </summary>
		public Size GetCheckSize()
		{
			return new Size(_checkBoxSize, _checkBoxSize);
		}

		/// <summary>
		/// </summary>
		public Rectangle GetCheckBoxBounds(NuGenBoundsParams checkBoundsParams)
		{
			return this.GetImageBounds(checkBoundsParams);
		}

		private const int _checkBoxSize = 12;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCheckBoxLayoutManager"/> class.
		/// </summary>
		public NuGenCheckBoxLayoutManager()
		{
		}
	}
}
