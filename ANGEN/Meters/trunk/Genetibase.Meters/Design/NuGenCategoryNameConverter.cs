/* -----------------------------------------------
 * NuGenCategoryNameConverter.cs
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
	/// Provides a list of standard values for <see cref="System.Diagnostics.PerformanceCounter.CategoryName"/> like property.
	/// </summary>
	public class NuGenCategoryNameConverter : TypeConverter
	{
		/// <summary>
		/// Returns whether this converter can convert an Object of the given type to the type of this converter, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
		/// <returns>
		/// 	<see langword="true"/>if this converter can perform the conversion; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		public override Boolean CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(String))
				return true;
			else
				return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Converts the given Object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <returns>
		/// An <see cref="T:System.Object"/> that represents
		/// the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion could not be performed.</exception>
		public override Object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, Object value)
		{
			if (value is String)
				return ((String)value).Trim();
			else
				return base.ConvertFrom(context, culture, value);
		}

		private String _previousMachineName;
		private TypeConverter.StandardValuesCollection _previousStandardValues;

		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			INuGenCounter counter = (context == null) ? null : (context.Instance as INuGenCounter);

			String machineName = ".";

			if (context != null)
				machineName = counter.MachineName;

			if (machineName != _previousMachineName)
			{
				_previousMachineName = machineName;

				try
				{
					PerformanceCounter.CloseSharedResources();
					PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories(machineName);
					String[] categoryNames = new String[categories.Length];

					for (Int32 categoryIndex = 0; categoryIndex < categories.Length; categoryIndex++)
					{
						categoryNames[categoryIndex] = categories[categoryIndex].CategoryName;
					}

					Array.Sort(categoryNames, Comparer.Default);
					_previousStandardValues = new TypeConverter.StandardValuesCollection(categoryNames);
				}
				catch
				{
					_previousStandardValues = null;
				}
			}

			return _previousStandardValues;
		}

		/// <summary>
		/// Returns whether this Object
		/// supports a standard set of values that can be picked
		/// from a list, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>
		/// 	<see langword="true"/> if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> should be
		/// called to find a common set of values the Object supports; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
