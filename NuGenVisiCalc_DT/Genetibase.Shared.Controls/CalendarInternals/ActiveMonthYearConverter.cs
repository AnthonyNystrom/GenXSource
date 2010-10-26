/* -----------------------------------------------
 * ActiveMonthYearConverter.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	internal class ActiveMonthYearConverter : StringConverter
	{

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{

			if (destinationType == typeof(string))
			{
				return Convert.ToString(value);
			}
			else throw new ArgumentException("Invalid");

		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value.GetType() == typeof(string))
			{
				NuGenActiveMonth m = (NuGenActiveMonth)context.Instance;
				if (m.Calendar.IsYearValid(value.ToString()))
					return Convert.ToInt32(value);
			}
			return base.ConvertFrom(context, culture, value);

		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			// Return true to allow standard values.
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			// Allow user to type the value
			return false;
		}

		public override System.ComponentModel.TypeConverter.StandardValuesCollection
			GetStandardValues(ITypeDescriptorContext context)
		{

			NuGenActiveMonth m = (NuGenActiveMonth)context.Instance;

			return new StandardValuesCollection(m.Calendar.AllowedYears());
		}
	}
}
