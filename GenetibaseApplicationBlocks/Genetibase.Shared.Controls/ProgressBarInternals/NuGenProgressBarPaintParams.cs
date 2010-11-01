/* -----------------------------------------------
 * NuGenProgressBarPaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.Shared.Controls.ProgressBarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenProgressBarPaintParams : NuGenPaintParams
	{
		private NuGenProgressBarStyle _style;

		/// <summary>
		/// </summary>
		public NuGenProgressBarStyle Style
		{
			get
			{
				return _style;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenProgressBarPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="sender"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenProgressBarPaintParams(
			object sender,
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			NuGenProgressBarStyle style
			)
			: base(sender, g, bounds, state)
		{
			_style = style;
		}
	}
}
