/* -----------------------------------------------
 * NuGenRenderElementArgs.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace Genetibase.MathX.FormulaInterpreter
{
	/// <summary>
	/// Encapsulates data for rendering elements.
	/// </summary>
	public struct NuGenRenderElementArgs
	{
		/// <summary>
		/// </summary>
		public Brush Brush
		{
			get
			{
				return _pen.Brush;
			}
		}

		private Font _font;

		/// <summary>
		/// </summary>
		public Font Font
		{
			get
			{
				return _font;
			}
		}

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

		private Pen _pen;

		/// <summary>
		/// </summary>
		public Pen Pen
		{
			get
			{
				return _pen;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenRenderElementArgs"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="font"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="pen"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenRenderElementArgs(Graphics g, Font font, Pen pen)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (font == null)
			{
				throw new ArgumentNullException("font");
			}

			if (pen == null)
			{
				throw new ArgumentNullException("pen");
			}

			_graphics = g;
			_font = font;
			_pen = pen;
		}
	}
}
