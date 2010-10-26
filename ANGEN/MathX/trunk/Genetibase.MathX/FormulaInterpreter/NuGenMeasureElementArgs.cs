/* -----------------------------------------------
 * NuGenMeasureElementArgs.cs
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
	/// Encapsulates data for measuring elements.
	/// </summary>
	public struct NuGenMeasureElementArgs
	{
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

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMeasureElementArgs"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="g"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="font"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenMeasureElementArgs(Graphics g, Font font)
		{
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}

			if (font == null)
			{
				throw new ArgumentNullException("font");
			}

			_graphics = g;
			_font = font;
		}
	}
}
