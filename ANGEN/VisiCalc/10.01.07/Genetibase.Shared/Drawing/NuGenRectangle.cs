/* -----------------------------------------------
 * NuGenRectangle.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Drawing;
using System.Xml.Serialization;

namespace Genetibase.Shared.Drawing
{
	/// <summary>
	/// Use <see cref="NuGenRectangle"/> instead of <see cref="Rectangle"/> to optimize serialization.
	/// </summary>
	public struct NuGenRectangle
	{
		#region Properties.Public

		/*
		 * Height
		 */

		private int _height;

		/// <summary>
		/// Gets or sets the rectangle height.
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

		/*
		 * Width
		 */

		private int _width;

		/// <summary>
		/// Gets or sets the rectangle width.
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

		/*
		 * X
		 */

		/// <summary>
		/// Determines the rectangle x-coordinate.
		/// </summary>
		private int _x;

		/// <summary>
		/// Gets or sets the rectangle x-coordinate.
		/// </summary>
		public int X
		{
			get
			{
				return _x;
			}
			set
			{
				_x = value;
			}
		}

		/*
		 * Y
		 */

		/// <summary>
		/// Determines the rectangle y-coordinate.
		/// </summary>
		private int _y;

		/// <summary>
		/// Gets or sets the rectangle y-coordinate.
		/// </summary>
		public int Y
		{
			get
			{
				return _y;
			}
			set
			{
				_y = value;
			}
		}

		#endregion

		#region Properties.Public.XmlIgnore

		/*
		 * Bounds
		 */

		/// <summary>
		/// Gets or sets the rectangle bounds.
		/// </summary>
		[XmlIgnore]
		public Rectangle Bounds
		{
			get
			{
				return new Rectangle(this.X, this.Y, this.Width, this.Height);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
				this.Width = value.Width;
				this.Height = value.Height;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes the <see cref="NuGenRectangle"/> struct.
		/// </summary>
		public NuGenRectangle(int x, int y, int width, int height)
		{
			_x = x;
			_y = y;
			_width = width;
			_height = height;
		}

		#endregion
	}
}
