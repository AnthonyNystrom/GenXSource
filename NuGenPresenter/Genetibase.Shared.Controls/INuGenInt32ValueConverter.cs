/* -----------------------------------------------
 * INuGenInt32ValueConverter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public interface INuGenInt32ValueConverter : IDisposable
	{
		/*
		 * Maximum
		 */

		/// <summary>
		/// Gets or sets the maximum valid value.
		/// </summary>
		Int32 Maximum
		{
			get;
			set;
		}

		/// <summary>
		/// Occurs when the value of the <see cref="Maximum"/> property changes.
		/// </summary>
		event EventHandler MaximumChanged;

		/*
		 * Minimum
		 */

		/// <summary>
		/// Gets or sets the minimum valid value.
		/// </summary>
		Int32 Minimum
		{
			get;
			set;
		}

		/// <summary>
		/// Occurs when the value of the <see cref="Minimum"/> property changes.
		/// </summary>
		event EventHandler MinimumChanged;

		/*
		 * Text
		 */

		/// <summary>
		/// Gets or sets the string value representation.
		/// </summary>
		String Text
		{
			get;
			set;
		}
 
		/// <summary>
		/// Occurs when the value of the <see cref="Text"/> property changes.
		/// </summary>
		event EventHandler TextChanged;

		/*
		 * Value
		 */

		/// <summary>
		/// </summary>
		Int32 Value
		{
			get;
			set;
		}

		/// <summary>
		/// Occurs when the value of the <see cref="Value"/> property changes.
		/// </summary>
		event EventHandler ValueChanged;
	}
}
