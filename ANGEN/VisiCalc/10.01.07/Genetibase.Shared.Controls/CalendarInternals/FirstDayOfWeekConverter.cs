/* -----------------------------------------------
 * FirstDayOfWeekConverter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared.Controls.CalendarInternals
{
	internal class FirstDayOfWeekConverter : StringConverter
	{
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string))
			{
				string[] validNames = new string[8];
				string[] dayNames = culture.DateTimeFormat.DayNames;
				validNames.Initialize();

				validNames[0] = "Default";

				for (int i = 1; i <= 7; i++)
					validNames[i] = dayNames[i - 1];

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

					if ((m >= 0) && (m <= 7))
					{
						return validNames[m];
					}
				}
			}
			return new DateTime();

		}


		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			int ret;
			if (value.GetType() == typeof(string))
			{
				NuGenCalendar m = (NuGenCalendar)context.Instance;
				ret = m.DayNumber(value.ToString());
				if ((ret >= 0) && (ret <= 7))
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

			NuGenCalendar m = (NuGenCalendar)context.Instance;

			return new StandardValuesCollection(m.DayNames());
		}
	}
}
