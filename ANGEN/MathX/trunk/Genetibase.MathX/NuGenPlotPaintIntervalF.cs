/* -----------------------------------------------
 * NuGenPlotPaintIntervalF.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.MathX
{
	public struct NuGenPlotPaintIntervalF
	{
		private float _end;

		/// <summary>
		/// </summary>
		public float End
		{
			get
			{
				return _end;
			}
			set
			{
				_end = value;
			}
		}

		private float _start;

		/// <summary>
		/// </summary>
		public float Start
		{
			get
			{
				return _start;
			}
			set
			{
				_start = value;
			}
		}

		/// <summary>
		/// </summary>
		public float Width
		{
			get
			{
				return _end - _start;
			}
			set
			{
				_end = _start + value;
			}
		}

		public static NuGenPlotPaintIntervalF Union(NuGenPlotPaintIntervalF left, NuGenPlotPaintIntervalF right)
		{
			return new NuGenPlotPaintIntervalF(
				Math.Min(left.Start, right.Start),
				Math.Max(left.End, right.End)
			);
		}

		/// <summary>
		/// </summary>
		public static readonly NuGenPlotPaintIntervalF Empty = new NuGenPlotPaintIntervalF(0f, 0f);

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPlotPaintIntervalF"/> class.
		/// </summary>
		public NuGenPlotPaintIntervalF(float start, float end)
		{
			_start = start;
			_end = end;
		}
	}
}
