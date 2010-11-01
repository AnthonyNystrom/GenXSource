/* -----------------------------------------------
 * NuGenPaintParams.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Encapsulates parameters necessary for basic drawing.
	/// </summary>
	public class NuGenPaintParams
	{
		#region Properties.Public

		/*
		 * Bounds
		 */

		private Rectangle _bounds;

		/// <summary>
		/// </summary>
		public Rectangle Bounds
		{
			get
			{
				return _bounds;
			}
		}

		/*
		 * Graphics
		 */

		private Graphics _graphics;

		/// <summary>
		/// </summary>
		public Graphics Graphics
		{
			get
			{
				return _graphics;
			}
		}

		/*
		 * Sender
		 */

		private object _sender;

		/// <summary>
		/// </summary>
		public object Sender
		{
			get
			{
				return _sender;
			}
		}

		/*
		 * State
		 */

		private NuGenControlState _state;

		/// <summary>
		/// </summary>
		public NuGenControlState State
		{
			get
			{
				return _state;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="sender"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPaintParams(object sender, Graphics g, Rectangle bounds, NuGenControlState state)
		{
			if (sender == null)
			{
				throw new ArgumentNullException("sender");
			}

			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			_sender = sender;
			_graphics = g;
			_bounds = bounds;
			_state = state;
		}

		#endregion
	}
}
