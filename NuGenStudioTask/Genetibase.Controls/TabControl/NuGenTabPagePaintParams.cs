/* -----------------------------------------------
 * NuGenTabPagePaintParams.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Genetibase.Controls
{
	/// <summary>
	/// </summary>
	public class NuGenTabPagePaintParams
	{
		#region Properties.Public

		/*
		 * Bounds
		 */

		private Rectangle _bounds = Rectangle.Empty;

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

		private Graphics _graphics = null;

		/// <summary>
		/// Read-only.
		/// </summary>
		public Graphics Graphics
		{
			get
			{
				return _graphics;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabPagePaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabPagePaintParams(Graphics g, Rectangle bounds)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			_graphics = g;
			_bounds = bounds;
		}

		#endregion
	}
}
