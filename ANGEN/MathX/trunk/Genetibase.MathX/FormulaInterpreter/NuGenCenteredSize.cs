/* -----------------------------------------------
 * NuGenCenteredSize.cs
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
	/// </summary>
	public struct NuGenCenteredSize
	{
		/// <summary>
		/// </summary>
		public int BottomPart
		{
			get
			{
				return _height - _centerY;
			}
		}

		private int _height;

		/// <summary>
		/// </summary>
		public int Height
		{
			get
			{
				return _height;
			}
			set
			{
				_height = value;
			}
		}

		/// <summary>
		/// </summary>
		public int LeftPart
		{
			get
			{
				return _centerX;
			}
			set
			{
				_centerX = value;
			}
		}

		/// <summary>
		/// </summary>
		public int RightPart
		{
			get
			{
				return _width - _centerX;
			}
		}

		/// <summary>
		/// </summary>
		public int TopPart
		{
			get
			{
				return _centerY;
			}
			set
			{
				_centerY = value;
			}
		}

		private int _width;

		/// <summary>
		/// </summary>
		public int Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
			}
		}

		private int _centerX;
		private int _centerY;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCenteredSize"/> class.
		/// </summary>
		public NuGenCenteredSize(int width, int height, int centerX, int centerY)
		{
			_width = width;
			_height = height;
			_centerX = centerX;
			_centerY = centerY;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCenteredSize"/> class.
		/// </summary>
		public NuGenCenteredSize(int width, int height)
			: this(width, height, width / 2, height / 2)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCenteredSize"/> class.
		/// </summary>
		public NuGenCenteredSize(Size size)
			: this(size.Width, size.Height, size.Width / 2, size.Height / 2)
		{
		}
	}
}
