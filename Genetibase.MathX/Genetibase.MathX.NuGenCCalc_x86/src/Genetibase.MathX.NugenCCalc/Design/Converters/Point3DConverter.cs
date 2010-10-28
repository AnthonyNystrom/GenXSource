using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using Genetibase.MathX.Core;
using System.Reflection;

namespace Genetibase.MathX.NugenCCalc.Design.Converters
{
	/// <summary>
	/// Provides a type converter to convert Point3D objects to and from various other representations.
	/// </summary>
	public class Point3DConverter : TypeConverter
	{
		public Point3DConverter()
		{

		}

		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof(string))
			{
				return true;
			}
			return base.CanConvertFrom(context, sourceType);
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(InstanceDescriptor))
			{
				return true;
			}
			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
		{
			if (!(value is string))
			{
				return base.ConvertFrom(context, culture, value);
			}
			string text1 = ((string) value).Trim();
			if (text1.Length == 0)
			{
				return null;
			}
			if (culture == null)
			{
				culture = CultureInfo.CurrentCulture;
			}
			char ch1 = culture.TextInfo.ListSeparator[0];
			char[] chArray1 = new char[] { ch1 } ;
			string[] textArray1 = text1.Split(chArray1);
			double[] numArray1 = new double[textArray1.Length];
			TypeConverter converter1 = TypeDescriptor.GetConverter(typeof(double));
			for (int num1 = 0; num1 < numArray1.Length; num1++)
			{
				numArray1[num1] = (double) converter1.ConvertFromString(context, culture, textArray1[num1]);
			}
			if (numArray1.Length == 3)
			{
				return new Point3D(numArray1[0], numArray1[1], numArray1[2]);
			}
			object[] objArray1 = new object[] { text1, "x, y, z" } ;
			throw new ArgumentException("TextParseFailedFormat");

		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
			{
				throw new ArgumentNullException("destinationType");
			}
			if ((destinationType == typeof(string)) && (value is Point3D))
			{
				Point3D point1 = (Point3D) value;
				if (culture == null)
				{
					culture = CultureInfo.CurrentCulture;
				}
				string text1 = culture.TextInfo.ListSeparator + " ";
				TypeConverter converter1 = TypeDescriptor.GetConverter(typeof(double));
				string[] textArray1 = new string[3];
				int num1 = 0;
				textArray1[num1++] = converter1.ConvertToString(context, culture, point1.X);
				textArray1[num1++] = converter1.ConvertToString(context, culture, point1.Y);
				textArray1[num1++] = converter1.ConvertToString(context, culture, point1.Z);
				return string.Join(text1, textArray1);
			}
			if ((destinationType == typeof(InstanceDescriptor)) && (value is Point3D))
			{
				Point3D point2 = (Point3D) value;
				Type[] typeArray1 = new Type[] { typeof(double), typeof(double) } ;
				ConstructorInfo info1 = typeof(Point3D).GetConstructor(typeArray1);
				if (info1 != null)
				{
					object[] objArray1 = new object[] { point2.X, point2.Y, point2.Z} ;
					return new InstanceDescriptor(info1, objArray1);
				}
			}
			return base.ConvertTo(context, culture, value, destinationType);

		}

		public override object CreateInstance(ITypeDescriptorContext context, System.Collections.IDictionary propertyValues)
		{
			return new Point3D((double) propertyValues["X"], (double) propertyValues["Y"], (double) propertyValues["Z"]);
		}

		public override bool GetCreateInstanceSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
		{
			PropertyDescriptorCollection collection1 = TypeDescriptor.GetProperties(typeof(Point3D), attributes);
			string[] textArray1 = new string[] { "X", "Y", "Z" } ;
			return collection1.Sort(textArray1);
		}

		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return true;
		}


		

	}

}
