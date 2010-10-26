/* -----------------------------------------------
 * NuGenCategoryNameConverter.cs
 * Author: Alex Nesterov
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.UI.NuGenMeters.Design
{
	/// <summary>
	/// Provides a list of standard values for <c>System.Diagnostics.PerformanceCounter.CategoryName</c> like property.
	/// </summary>
	public class NuGenCategoryNameConverter : TypeConverter
	{
		/// <summary>
		/// Returns
		/// whether this converter can convert an object of the given type to the type of this converter, using
		/// the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="sourceType">A <see cref="T:System.Type"/> that represents the type you want to convert from.</param>
		/// <returns>
		/// 	<see langword="true "/>if this converter can perform the conversion; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string)) return true;
			else return base.CanConvertFrom(context, sourceType);
		}

		/// <summary>
		/// Converts the given object to the type of this converter, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"/> to use as the current culture.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <returns>
		/// An <see cref="T:System.Object"/> that represents
		/// the converted value.
		/// </returns>
		/// <exception cref="T:System.NotSupportedException">The conversion could not be performed.</exception>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string) return ((string)value).Trim();
			else return base.ConvertFrom (context, culture, value);
		}

		/// <summary>
		/// Compare the current machine name with the previous one.
		/// </summary>
		private string previousMachineName;

		/// <summary>
		/// The previous list of standard values.
		/// </summary>
		private TypeConverter.StandardValuesCollection values;

		/// <summary>
		/// Gets the standard values.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			INuGenCounter counter = (context == null) ? null : (context.Instance as INuGenCounter);

			string machineName = ".";

			if (context != null) machineName = counter.MachineName;

			if (machineName != this.previousMachineName)
			{
				this.previousMachineName = machineName;

				try 
				{
					PerformanceCounter.CloseSharedResources();
					PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories(machineName);
					string[] categoryNames = new string[categories.Length];

					for (int categoryIndex = 0; categoryIndex < categories.Length; categoryIndex++) 
					{
						categoryNames[categoryIndex] = categories[categoryIndex].CategoryName;
					}

					Array.Sort(categoryNames, Comparer.Default);
					this.values = new TypeConverter.StandardValuesCollection(categoryNames);
				}
				catch
				{
					this.values = null;
				}
			}

			return this.values;
		}
		
		/// <summary>
		/// Returns whether this object
		/// supports a standard set of values that can be picked
		/// from a list, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <returns>
		/// 	<see langword="true"/> if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"/> should be
		/// called to find a common set of values the object supports; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}
	}
}
