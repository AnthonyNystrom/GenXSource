/* -----------------------------------------------
 * NuGenRadioButtonLayoutManager.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
	public class NuGenRadioButtonLayoutManager : NuGenControlLayoutManager, INuGenRadioButtonLayoutManager
	{
		/// <summary>
		/// </summary>
		public Size GetRadioSize()
		{
			return new Size(_radioButtonSize, _radioButtonSize);
		}

		/// <summary>
		/// </summary>
		public Rectangle GetRadioButtonBounds(NuGenBoundsParams radioBoundsParams)
		{
			return base.GetImageBounds(radioBoundsParams);
		}

		private const int _radioButtonSize = 12;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRadioButtonLayoutManager"/> class.
		/// </summary>
		public NuGenRadioButtonLayoutManager()
		{
		}
	}
}
