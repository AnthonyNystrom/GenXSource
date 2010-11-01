/* -----------------------------------------------
 * NuGenTrackBarPaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.TrackBarInternals
{
	/// <summary>
	/// </summary>
	public class NuGenTrackBarPaintParams : NuGenPaintParams
	{
		#region Properties.Public

		/*
		 * TickStyle
		 */

		private TickStyle _tickStyle;

		/// <summary>
		/// </summary>
		public TickStyle TickStyle
		{
			get
			{
				return _tickStyle;
			}
		}

		/*
		 * ValueTracker
		 */

		private INuGenValueTracker _valueTracker;

		/// <summary>
		/// </summary>
		public INuGenValueTracker ValueTracker
		{
			get
			{
				return _valueTracker;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTrackBarPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="sender"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="valueTracker"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenTrackBarPaintParams(
			object sender,
			Graphics g,
			Rectangle bounds,
			NuGenControlState state,
			INuGenValueTracker valueTracker,
			TickStyle tickStyle
			)
			: base(sender, g, bounds, state)
		{
			if (valueTracker == null)
			{
				throw new ArgumentNullException("valueTracker");
			}

			_valueTracker = valueTracker;
			_tickStyle = tickStyle;
		}

		#endregion
	}
}
