/* -----------------------------------------------
 * NuGenTrackButtonPaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.TrackBarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenTrackButtonPaintParams : NuGenPaintParams
	{
		private TickStyle _style;

		/// <summary>
		/// </summary>
		public TickStyle Style
		{
			get
			{
				return _style;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTrackButtonPaintParams"/> class.
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
		public NuGenTrackButtonPaintParams(
			object sender,
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			TickStyle style
			)
			: base(sender,  g, bounds, state)
		{
			_style = style;
		}
	}
}
