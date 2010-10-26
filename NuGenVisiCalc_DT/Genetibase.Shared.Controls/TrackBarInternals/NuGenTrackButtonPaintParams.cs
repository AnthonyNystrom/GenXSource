/* -----------------------------------------------
 * NuGenTrackButtonPaintParams.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
			set
			{
				_style = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTrackButtonPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTrackButtonPaintParams(Graphics g)
			: base(g)
		{
		}
	}
}
