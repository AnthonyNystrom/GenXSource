/* -----------------------------------------------
 * NuGenCounterNameConverter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Meters.Design
{
	/// <summary>
	/// Provides a list of standard values for <see cref="System.Diagnostics.PerformanceCounter.CounterName"/> like property.
	/// </summary>
	public class NuGenCounterNameConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string)) return true;
			else return base.CanConvertFrom (context, sourceType);
		}

		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <returns>
		/// An <see cref="T:System.Object"/> that represents the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string) return ((string)value).Trim();
			else return base.ConvertFrom (context, culture, value);
		}

		/// <summary>
		/// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
		/// <returns>
		/// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"/> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
		/// </returns>
		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			INuGenCounter counter = (context == null) ? null : (context.Instance as INuGenCounter);

			string machineName = ".";
			string categoryName = string.Empty;

			if (counter != null) 
			{
				machineName = counter.MachineName;
				categoryName = counter.CategoryName;
			}

			try 
			{
				PerformanceCounterCategory category = new PerformanceCounterCategory(categoryName, machineName);
				
				string[] instances = category.GetInstanceNames();
				PerformanceCounter[] counters = null;

				if (instances.Length == 0) counters = category.GetCounters();
				else counters = category.GetCounters(instances[0]);

				string[] names = new string[counters.Length];

				for (int nameIndex = 0; nameIndex < counters.Length; nameIndex++) 
				{
					names[nameIndex] = counters[nameIndex].CounterName;
				}

				Array.Sort(names, Comparer.Default);

				return new TypeConverter.StandardValuesCollection(names);
			}
			catch
			{
			}

			return null;
		}

		/// <summary>
		/// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>
		/// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> should be called to find a common set of values the object supports; otherwise, false.
		/// </returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}	
	}
}
