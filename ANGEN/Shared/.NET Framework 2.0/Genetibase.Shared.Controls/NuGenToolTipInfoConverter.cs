/* -----------------------------------------------
 * NuGenToolTipInfoConverter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	public sealed class NuGenToolTipInfoConverter : TypeConverter
	{
		#region Methods.Public.Overridden

		/*
		 * CanConvertTo
		 */

		/// <summary>
		/// Returns whether this converter can convert the object to the specified type, using the specified context.
		/// </summary>
		/// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
		/// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
		/// <returns>
		/// true if this converter can perform the conversion; otherwise, false.
		/// </returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}

			return base.CanConvertTo(context, destinationType);
		}

		/*
		 * ConvertTo
		 */

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
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}

			NuGenToolTipInfo tooltipInfo = value as NuGenToolTipInfo;

			if (destinationType == typeof(InstanceDescriptor) && tooltipInfo != null)
			{
				object[] constructorValues;
				MemberInfo constructorInfo = NuGenToolTipInfoConverter.GetConstructorInfo(
					tooltipInfo,
					out constructorValues
				);

				return new InstanceDescriptor(constructorInfo, constructorValues);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}

		#endregion

		#region Methods.Internal

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="tooltipInfo"/> is <see langword="null"/>.</para>
		/// </exception>
		internal static MemberInfo GetConstructorInfo(
			NuGenToolTipInfo tooltipInfo,
			out object[] constructorValues
			)
		{
			if (tooltipInfo == null)
			{
				throw new ArgumentNullException("tooltipInfo");
			}

			Type tooltipInfoType = tooltipInfo.GetType();
			Type[] constructorTypes;

			if (
				tooltipInfo.IsRemarksHeaderVisible
				|| tooltipInfo.IsRemarksImageVisible
				|| tooltipInfo.IsRemarksVisible
				|| tooltipInfo.IsCustomSize
				)
			{
				constructorValues = new object[] { 
					tooltipInfo.Header,
					tooltipInfo.Image,
					tooltipInfo.Text,
					tooltipInfo.RemarksHeader,
					tooltipInfo.RemarksImage,
					tooltipInfo.Remarks,
					tooltipInfo.CustomSize
				};

				constructorTypes = new Type[] { 
					typeof(string),
					typeof(Image),
					typeof(string),
					typeof(string),
					typeof(Image),
					typeof(string),
					typeof(Size)
				};
			}
			else if (
				tooltipInfo.IsTextVisible
				|| tooltipInfo.IsHeaderVisible
				|| tooltipInfo.IsImageVisible
				)
			{
				constructorValues = new object[] {
					tooltipInfo.Header,
					tooltipInfo.Image,
					tooltipInfo.Text
				};

				constructorTypes = new Type[] {
					typeof(string),
					typeof(Image),
					typeof(string) 
				};
			}
			else
			{
				constructorValues = new object[] { };
				constructorTypes = new Type[] { };
			}

			return tooltipInfoType.GetConstructor(constructorTypes);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenToolTipInfoConverter"/> class.
		/// </summary>
		public NuGenToolTipInfoConverter()
		{
		}
	}
}
