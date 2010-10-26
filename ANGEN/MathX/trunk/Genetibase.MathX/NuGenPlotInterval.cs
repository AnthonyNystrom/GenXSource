/* -----------------------------------------------
 * NuGenPlotInterval.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.MathX
{
	/// <summary>
	/// </summary>
	public struct NuGenPlotInterval
	{
		private double _end;

		public double End
		{
			get
			{
				return _end;
			}
			private set
			{
				_end = value;
			}
		}

		private double _start;

		public double Start
		{
			get
			{
				return _start;
			}
			private set
			{
				_start = value;
			}
		}

		/// <summary>
		/// </summary>
		public static NuGenPlotInterval Infinite
		{
			get
			{
				return new NuGenPlotInterval(double.NegativeInfinity, double.PositiveInfinity);
			}
		}

		/// <summary>
		/// </summary>
		public static bool Parse(string start, string end, out NuGenPlotInterval result)
		{
			result = NuGenPlotInterval.Infinite;
			double bufferStart = 0.0;
			double bufferEnd = 0.0;
			bool parseSucceded = NuGenPlotInterval.Parse(start, out bufferStart) && NuGenPlotInterval.Parse(end, out bufferEnd);
			result.Start = bufferStart;
			result.End = bufferEnd;
			return parseSucceded;
		}

		/// <summary>
		/// </summary>
		public bool IsInfinite()
		{
			return double.IsNegativeInfinity(_start) &&
				double.IsPositiveInfinity(_end);
		}

		private static bool Parse(string text, out double result)
		{
			return double.TryParse(text, System.Globalization.NumberStyles.Float, null, out result);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPlotInterval"/> structure.
		/// </summary>
		public NuGenPlotInterval(double start, double end)
		{
			_start = start;
			_end = end;
		}
	}
}
