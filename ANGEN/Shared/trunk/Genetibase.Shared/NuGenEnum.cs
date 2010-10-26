/* -----------------------------------------------
 * NuGenEnum.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Genetibase.Shared
{
	/// <summary>
	/// Provides functionality to process <see cref="T:System.Enum"/> inheritors.
	/// </summary>
	public static class NuGenEnum
	{
		/*
		 * FlagsSetOn
		 */

		/// <summary>
		/// Calculates the number of set flags (i.e. are set to 1) for the specified <see cref="T:System.Enum"/>.
		/// <param name="flags">Specifies the enumration to calculate set flags on.</param>
		/// </summary>
		/// <example>
		/// The following example illustrates how this method processes enumerations with negative elements.
		/// In this case only the last flag is recognized.	
		///	<code>
		/// 	enum NegativeEnum : long
		/// 	{
		///			NegativeValueOne = -1,
		///			NegativeValueTwo = -2
		/// 	}
		/// 	
		/// 	...
		/// 	
		/// 	NegativeEnum negativeEnum = NegativeEnum.NegativeValueOne;
		/// 	Console.WriteLine(NuGenEnum.FlagsSetOn(negativeEnum)); // Output: 1
		/// 	
		/// 	negativeEnum |= NegativeEnum.NegativeValueTwo;
		/// 	Console.WriteLine(NuGenEnum.FlagsSetOn(negativeEnum)); // Output: 1
		///
		/// 	...
		/// </code>
		/// </example>
		/// <exception cref="ArgumentNullException"><paramref name="flags"/> is <see langword="null"/>.</exception>
		public static Int32 FlagsSetOn(Enum flags)
		{
			if (flags == null)
			{
				throw new ArgumentNullException("flags");
			}

			var flagsSetCounter = 0;

			var enumLong = NuGenEnum.ToUInt64(flags);
			var enumValues = NuGenEnum.GetEnumValues(flags.GetType());

			for (var i = 0; i < enumValues.Length; i++)
			{
				if (enumValues[i] == 0)
				{
					continue;
				}
				else if ((enumLong & enumValues[i]) == enumValues[i])
				{
					enumLong -= enumValues[i];
					flagsSetCounter++;
				}
			}

			return flagsSetCounter;
		}

		/*
		 * ToArray
		 */

		/// <summary>
		/// </summary>
		/// <typeparam name="TEnum">Enum type.</typeparam>
		/// <exception cref="ArgumentException">
		/// Specified value for <typeparamref name="TEnum"/> is not <see cref="Enum"/>.
		/// </exception>
		public static TEnum[] ToArray<TEnum>()
		{
			return (TEnum[])Enum.GetValues(typeof(TEnum));
		}

		/// <summary>
		/// Converts the specified <paramref name="value"/> to <see cref="T:System.UInt64"/> representation.
		/// Types <see cref="T:System.SByte"/>, <see cref="T:System.Int16"/>, <see cref="T:System.Int32"/>,
		/// <see cref="T:System.Int64"/>, <see cref="T:System.Byte"/>, <see cref="T:System.UInt16"/>,
		/// <see cref="T:System.UInt32"/>, or <see cref="T:System.UInt64"/> expected.
		/// </summary>
		/// <param name="value">Specifies the value to convert to <see cref="T:System.UInt64"/> representation.</param>
		/// <exception cref="T:System.ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
		/// <exception cref="T:System.ArgumentException">The type of the specified <paramref name="value"/>
		/// is invalid. See method description for valid values.</exception>
		internal static UInt64 ToUInt64(Object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}

			switch (Convert.GetTypeCode(value))
			{
				case TypeCode.SByte:
				case TypeCode.Int16:
				case TypeCode.Int32:
				case TypeCode.Int64:
				{
					return (UInt64)Convert.ToInt64(value, CultureInfo.InvariantCulture);
				}
				case TypeCode.Byte:
				case TypeCode.UInt16:
				case TypeCode.UInt32:
				case TypeCode.UInt64:
				{
					return Convert.ToUInt64(value, CultureInfo.InvariantCulture);
				}
				default:
				{
					throw new ArgumentException(
						Properties.Resources.Argument_InvalidEnumType,
						"value"
						);
				}
			}
		}

		private static UInt64[] GetEnumValues(Type enumType)
		{
			Debug.Assert(enumType != null, "enumType != null");

			var bufferEnumValues = Enum.GetValues(enumType);
			var enumValues = new UInt64[bufferEnumValues.Length];

			var i = 0;

			foreach (var value in bufferEnumValues)
			{
				enumValues[i++] = ToUInt64(value);
			}

			return enumValues;
		}
	}
}
