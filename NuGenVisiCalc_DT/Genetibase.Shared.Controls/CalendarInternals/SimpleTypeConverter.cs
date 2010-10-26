/* -----------------------------------------------
 * SimpleTypeConverter.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	/// <summary>
	/// </summary>
	public class SimpleTypeConverter : ExpandableObjectConverter
	{
		/// <summary>
		/// Converts the given value object to the specified type, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.</param>
		/// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type"></see> to convert the value parameter to.</param>
		/// <returns>
		/// An <see cref="T:System.Object"></see> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		/// <exception cref="T:System.ArgumentNullException">The destinationType parameter is null. </exception>
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			return "";
		}
	}
}
