/* -----------------------------------------------
 * NuGenLimit.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared
{
	/// <summary>
	/// Stores a pair of integers representing the minimum and maximum values. 
	/// </summary>
	public struct NuGenLimit
	{
		/*
		 * Maximum
		 */

		private Int32 _maximum;

		/// <summary>
		/// </summary>
		public Int32 Maximum
		{
			get
			{
				return _maximum;
			}
			set
			{
				_maximum = value;
			}
		}

		/*
		 * Minimum
		 */

		private Int32 _minimum;

		/// <summary>
		/// </summary>
		public Int32 Minimum
		{
			get
			{
				return _minimum;
			}
			set
			{
				_minimum = value;
			}
		}

		/*
		 * GetLimitedValue
		 */

		/// <summary>
		/// </summary>
		/// <param name="valueToLimit"></param>
		/// <returns></returns>
		public Int32 GetLimitedValue(Int32 valueToLimit)
		{
			return Math.Min(this.Maximum, Math.Max(this.Minimum, valueToLimit));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLimit"/> structure.
		/// </summary>
		public NuGenLimit(Int32 minimum, Int32 maximum)
		{
			_minimum = minimum;
			_maximum = maximum;
		}
	}
}
