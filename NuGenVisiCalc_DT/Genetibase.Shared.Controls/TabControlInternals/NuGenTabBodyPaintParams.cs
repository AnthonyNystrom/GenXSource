/* -----------------------------------------------
 * NuGenTabBodyPaintParams.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// </summary>
	public class NuGenTabBodyPaintParams
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
		 * FlatStyle
		 */

		private FlatStyle _flatStyle = FlatStyle.System;

		/// <summary>
		/// </summary>
		public FlatStyle FlatStyle
		{
			get
			{
				return _flatStyle;
			}
			set
			{
				_flatStyle = value;
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
		/// Initializes a new instance of the <see cref="NuGenTabBodyPaintParams"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="g"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenTabBodyPaintParams(Graphics g, Rectangle bounds)
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
