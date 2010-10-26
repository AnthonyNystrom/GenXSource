/* -----------------------------------------------
 * NuGenArcDescriptor.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.UI.NuGenInterface.Drawing
{
	/// <summary>
	/// Encapsulates data necessary to build an arc.
	/// </summary>
	struct NuGenArcDescriptor
	{
		#region Properties.Public

		/*
		 * Height
		 */

		private float _Height;

		/// <summary>
		/// Gets the arc height.
		/// </summary>
		public float Height
		{
			get
			{
				return _Height;
			}
		}

		/*
		 * StartAngle
		 */

		private float _StartAngle;

		/// <summary>
		/// Gets the start angle for the arc.
		/// </summary>
		public float StartAngle
		{
			get
			{
				return _StartAngle;
			}
		}

		/*
		 * SweepAngle
		 */

		private float _SweepAngle;

		/// <summary>
		/// Gets the sweep angle for the arc.
		/// </summary>
		public float SweepAngle
		{
			get
			{
				return _SweepAngle;
			}
		}

		/*
		 * Width
		 */

		private float _Width;

		/// <summary>
		/// Gets the arc width.
		/// </summary>
		public float Width
		{
			get
			{
				return _Width;
			}
		}

		/*
		 * X
		 */

		private float _X;

		/// <summary>
		/// Gets the arc x-coordinate.
		/// </summary>
		public float X
		{
			get
			{
				return _X;
			}
		}

		/*
		 * Y
		 */

		private float _Y;

		/// <summary>
		/// Gets the arc y-coordinate.
		/// </summary>
		public float Y
		{
			get
			{
				return _Y;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenArcDescriptor"/> struct.
		/// </summary>
		public NuGenArcDescriptor(
			float x,
			float y,
			float width,
			float height,
			float startAngle,
			float sweepAngle
			)
		{
			_X = x;
			_Y = y;
			_Width = width;
			_Height = height;
			_StartAngle = startAngle;
			_SweepAngle = sweepAngle;
		}

		#endregion
	}
}
