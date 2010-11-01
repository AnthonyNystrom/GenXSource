/* -----------------------------------------------
 * NuGenTransparencyConverter.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Design.Properties;

using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.Design
{
	/// <summary>
	/// Provides a converter for transparency values.
	/// </summary>
	public sealed class NuGenTransparencyConverter : TypeConverter
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
			if (sourceType == typeof(string)) 
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
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
		/// <exception cref="ArgumentException">
		/// <paramref name="value"/> is not in the corrent format. The transparency level should be between 0 and 100.
		/// </exception>
		/// <exception cref="T:System.NotSupportedException">
		/// The conversion could not be performed.
		/// </exception>
		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (value is string) 
			{
				string bufferValue = ((string)value).Trim();
				int bufferTransparency = 0;

				if (bufferValue.IndexOf("%") > -1) 
				{
					bufferValue = bufferValue.Replace("%", "").Trim();
				}
				
				try 
				{
					bufferTransparency = int.Parse(bufferValue);
				}
				catch
				{
					Trace.WriteLine("The specified value was not in the correct format.\nvalue = " + value.ToString(), "Error");
				}

				if (bufferTransparency < 0 || bufferTransparency > 100) 
				{
					throw new ArgumentException(Resources.Argument_InvalidTransparencyLevel);
				}
				
				return bufferTransparency;
			}

			return base.ConvertFrom(context, culture, value);
		}

		/// <summary>
		/// Converts the given value object
		/// to the specified type, using the specified context and culture information.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"/> that provides a format context.</param>
		/// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"/> object. If <see langword="null"/> is passed, the current culture is assumed.</param>
		/// <param name="value">The <see cref="T:System.Object"/> to convert.</param>
		/// <param name="destinationType">The <see cref="T:System.Type"/> to convert the <paramref name="value"/> parameter to.</param>
		/// <returns>
		/// An <see cref="T:System.Object"/> that represents
		/// the converted value.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="destinationType"/> parameter is <see langword="null"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The conversion could not be performed.</exception>
		public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == typeof(string)) 
			{
				return (int)value + "%";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}
