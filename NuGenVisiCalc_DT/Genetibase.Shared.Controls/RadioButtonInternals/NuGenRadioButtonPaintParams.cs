/* -----------------------------------------------
 * NuGenRadioButtonPaintParams.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.RadioButtonInternals
{
	/// <summary>
	/// </summary>
	public class NuGenRadioButtonPaintParams : NuGenPaintParams
	{
		private bool _checked;

		/// <summary>
		/// </summary>
		public bool Checked
		{
			get
			{
				return _checked;
			}
			set
			{
				_checked = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRadioButtonPaintParams"/> class.
		/// </summary>
		/// <param name="g"></param>
		/// <exception cref="ArgumentNullException"><paramref name="g"/> is <see langword="null"/>.</exception>
		public NuGenRadioButtonPaintParams(Graphics g)
			: base(g)
		{
		}
	}
}
