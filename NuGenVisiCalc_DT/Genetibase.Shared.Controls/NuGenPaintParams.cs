/* -----------------------------------------------
 * NuGenPaintParams.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenPaintParams
	{
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
			set
			{
				_bounds = value;
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
			set
			{
				_state = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPaintParams(Graphics g)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			_graphics = g;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="initializeFrom"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPaintParams(NuGenPaintParams initializeFrom)
		{
			if (initializeFrom == null)
			{
				throw new ArgumentNullException("initializeFrom");
			}

			_bounds = initializeFrom.Bounds;
			_graphics = initializeFrom.Graphics;
			_state = initializeFrom.State;
		}
	}
}
