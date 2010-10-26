/* -----------------------------------------------
 * NuGenSpinButtonPaintParams.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.SpinInternals
{
	/// <summary>
	/// </summary>
	public class NuGenSpinButtonPaintParams : NuGenPaintParams
	{
		private NuGenSpinButtonStyle _style;

		/// <summary>
		/// </summary>
		public NuGenSpinButtonStyle Style
		{
			get
			{
				return _style;
			}
			set
			{
				_style = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSpinButtonPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenSpinButtonPaintParams(Graphics g)
			: base(g)
		{
		}
	}
}
