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
			set
			{
				_tickStyle = value;
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
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="valueTracker"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenTrackBarPaintParams(Graphics g, INuGenValueTracker valueTracker)
			: base(g)
		{
			if (valueTracker == null)
			{
				throw new ArgumentNullException("valueTracker");
			}

			_valueTracker = valueTracker;
		}

		#endregion
	}
}
