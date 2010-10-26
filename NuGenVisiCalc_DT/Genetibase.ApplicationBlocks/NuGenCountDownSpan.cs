/* -----------------------------------------------
 * NuGenCountDownSpan.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using res = Genetibase.ApplicationBlocks.Properties.Resources;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.ApplicationBlocks
{
	/// <summary>
	/// Encapsulates span data for <see cref="NuGenCountDownBlock"/>.
	/// </summary>
	public struct NuGenCountDownSpan
	{
		private int _minutes;

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// The specified <paramref name="value"/> should be non-negative.
		/// </exception>
		public int Minutes
		{
			get
			{
				return _minutes;
			}
			set
			{
				if (this.IsMinutesValid(value))
				{
					_minutes = value;
				}
				else
				{
					throw new ArgumentException(res.Argument_InvalidMinutes);
				}
			}
		}

		private int _seconds;

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentException">
		/// The specified <paramref name="value"/> should be non-negative and not greater than 60.
		/// </exception>
		public int Seconds
		{
			get
			{
				return _seconds;
			}
			set
			{
				if (this.IsSecondsValid(value))
				{
					_seconds = value;
				}
				else
				{
					throw new ArgumentException(res.Argument_InvalidSeconds);
				}
			}
		}

		/// <summary>
		/// Returns the fully qualified type name of this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> containing a fully qualified type name.
		/// </returns>
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "{0:00}:{1:00}", this.Minutes, this.Seconds);
		}

		private bool IsMinutesValid(int minutes)
		{
			return minutes >= 0;
		}

		private bool IsSecondsValid(int seconds)
		{
			return seconds >= 0 && seconds <= 59;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenCountDownSpan"/> struct.
		/// </summary>
		/// <exception cref="ArgumentException">
		///	<para>
		///		<paramref name="minutes"/> is negative.
		/// </para>
		/// -or-
		///	<para>
		///		<paramref name="seconds"/> is negative or greater than 59.
		/// </para>
		/// </exception>
		/// <example>
		/// NuGenCountDownSpan span = new NuGenCountDownSpan(99, 10);
		/// System.Console.WriteLine(span.ToString()); // 99:10
		/// </example>
		public NuGenCountDownSpan(int minutes, int seconds)
		{
			_minutes = 0;
			_seconds = 0;

			if (this.IsMinutesValid(minutes))
			{
				_minutes = minutes;
			}
			else
			{
				throw new ArgumentException(res.Argument_InvalidMinutes);
			}

			if (this.IsSecondsValid(seconds))
			{
				_seconds = seconds;
			}
			else
			{
				throw new ArgumentException(res.Argument_InvalidSeconds);
			}
		}
	}
}
