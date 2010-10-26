/* -----------------------------------------------
 * ActiveMonthMonthConverter.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	internal class ActiveMonthMonthConverter : StringConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{

				string[] validNames;
				validNames = culture.DateTimeFormat.MonthNames;
				if (value.GetType() == typeof(string))
				{
					for (int i = 0; i < validNames.Length; i++)
					{
						if (value.ToString().CompareTo(validNames[i]) == 0)
							return validNames[i];
					}
				}
				else if (value.GetType() == typeof(int))
				{
					int m = Convert.ToInt32(value);

					if ((m >= 1) && (m <= 12))
					{
						return validNames[m - 1];
					}
				}
			}
			return new DateTime();

		}


		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			int ret = 0;
			if (value.GetType() == typeof(string))
			{
				NuGenActiveMonth m = (NuGenActiveMonth)context.Instance;
				ret = m.Calendar.MonthNumber(value.ToString());
				if ((ret >= 1) && (ret <= 12))
					return ret;
			}
			return base.ConvertFrom(context, culture, value);
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
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

			return new StandardValuesCollection(m.Calendar.AllowedMonths());
		}

	}
}
