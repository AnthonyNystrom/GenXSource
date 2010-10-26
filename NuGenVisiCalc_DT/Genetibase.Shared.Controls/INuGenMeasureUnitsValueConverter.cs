/* -----------------------------------------------
 * INuGenMeasureUnitsValueConverter.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// Provides functionality to convert a value to its string representation according to current measure units.
	/// </summary>
	public interface INuGenMeasureUnitsValueConverter : INuGenInt32ValueConverter
	{
		/// <summary>
		/// Gets or sets the step for measure units. E.g. 1000 g = 1 kg. Then the factor will be 1000.
		/// </summary>
		Int32 Factor
		{
			get;
			set;
		}

		/// <summary>
		/// Occurs when the value of the <see cref="Factor"/> property changes.
		/// </summary>
		event EventHandler FactorChanged;

		/// <summary>
		/// Gets the amount to increment or decrement.
		/// </summary>
		Int32 Increment
		{
			get;
		}

		/// <summary>
		/// Occurs when the value of the <see cref="Increment"/> property changes.
		/// </summary>
		event EventHandler IncrementChanged;

		/// <summary>
		/// Gets or sets the list of available measure units. For weight it can be g, kg, T.
		/// </summary>
		String[] MeasureUnits
		{
			get;
			set;
		}

		/// <summary>
		/// Occurs when the value of the <see cref="MeasureUnits"/> property changes.
		/// </summary>
		event EventHandler MeasureUnitsChanged;
	}
}
