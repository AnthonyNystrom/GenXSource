/* -----------------------------------------------
 * NuGenSpinButtonPaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSpinButtonPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="sender"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenSpinButtonPaintParams(
			object sender,
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			NuGenSpinButtonStyle style
			)
			: base(sender, g, bounds, state)
		{
			_style = style;
		}
	}
}
