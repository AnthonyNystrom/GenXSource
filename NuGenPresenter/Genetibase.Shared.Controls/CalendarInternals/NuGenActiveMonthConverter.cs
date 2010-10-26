/* -----------------------------------------------
 * NuGenActiveMonthConverter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	internal class NuGenActiveMonthConverter : ExpandableObjectConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
				return true;
			if (sourceType == typeof(DateTime))
				return true;

			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(string))
				return true;
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{

			if (value.GetType() == typeof(string))
			{
				// Parse property string
				string[] ss = value.ToString().Split(new char[] { ';' }, 2);
				if (ss.Length == 2)
				{
					// Create new ActiveMonth
					NuGenActiveMonth item;
					NuGenCalendar m = (NuGenCalendar)context.Instance;
					item = m.ActiveMonth;
					// Set properties
					item.Month = item.Calendar.MonthNumber(ss[0]);
					if (item.Calendar.IsYearValid(ss[1].Trim()))
					{
						item.Year = Convert.ToInt32(ss[1].Trim());
						return item;
					}

				}
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{

			if (destinationType == typeof(string))
			{
				// cast value to ActiveMonth
				NuGenActiveMonth dest = (NuGenActiveMonth)value;
				// create property string
				return dest.Calendar.MonthName(dest.Month) + "; " + dest.Year;
			}
			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
