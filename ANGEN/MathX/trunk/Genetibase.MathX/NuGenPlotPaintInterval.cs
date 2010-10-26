/* -----------------------------------------------
 * NuGenPlotPaintInterval.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.MathX
{
	public struct NuGenPlotPaintInterval
	{
		private int _end;

		public int End
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

		private int _start;

		public int Start
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

		public int Width
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

		/// <summary>
		/// </summary>
		public static NuGenPlotPaintInterval Round(NuGenPlotPaintIntervalF iv)
		{
			return new NuGenPlotPaintInterval((int)iv.Start, (int)iv.End);
		}

		/// <summary>
		/// </summary>
		public static readonly NuGenPlotPaintInterval Empty = new NuGenPlotPaintInterval(0, 0);

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPlotPaintInterval"/> class.
		/// </summary>
		public NuGenPlotPaintInterval(int start, int end)
		{
			_start = start;
			_end = end;
		}
	}
}
