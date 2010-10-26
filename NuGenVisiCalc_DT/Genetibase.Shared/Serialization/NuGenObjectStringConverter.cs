/* -----------------------------------------------
 * NuGenObjectStringConverter.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Reflection;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Provides methods that allow to convert a class instance to its string representation and vice versa.
	/// </summary>
	public class NuGenObjectStringConverter : INuGenObjectStringConverter
	{
		#region Methods.Public

		/*
		 * ObjectToString
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="objectToConvert"/> is <see langword="null"/>.
		/// </exception>
		public virtual string ObjectToString(object objectToConvert)
		{
			if (objectToConvert == null)
			{
				throw new ArgumentNullException("objectToConvert");
			}

			if (objectToConvert is string) 
			{
				return (string)objectToConvert;
			}

			if (objectToConvert is Type) 
			{
				return objectToConvert.ToString();
			}

			return TypeDescriptor.GetConverter(objectToConvert.GetType()).ConvertToString(objectToConvert);
		}

		/*
		 * StringToObject
		 */

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="str"/> is <see langword="null"/>.
		/// </para>
		/// -or-
		/// <para>
		///		<paramref name="type"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		/// <exception cref="TypeLoadException">
		/// <paramref name="objectType"/> is defined in the assembly that could not be found in the current
		/// application domain.
		/// </exception>
		public virtual object StringToObject(string stringToConstructObjectFrom, Type objectType)
		{
			if (stringToConstructObjectFrom == null)
			{
				throw new ArgumentNullException("stringToConstructObjectFrom");
			}

			if (objectType == null)
			{
				throw new ArgumentNullException("objectType");
			}

			if (objectType == typeof(string)) 
			{
				return stringToConstructObjectFrom;
			}

			if (objectType == typeof(decimal)) 
			{
				return decimal.Parse(stringToConstructObjectFrom, CultureInfo.CurrentCulture);
			}

			if (objectType == typeof(Type)) 
			{
				Type typeFromStr = NuGenTypeFinder.GetType(stringToConstructObjectFrom);

				if (typeFromStr != null) 
				{
					return typeFromStr;
				}
				else
				{
					throw new TypeLoadException(
						string.Format(CultureInfo.InvariantCulture, Properties.Resources.TypeLoad_NotFound, stringToConstructObjectFrom)
						);
				}
			}

			return TypeDescriptor.GetConverter(objectType).ConvertFromString(stringToConstructObjectFrom);
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenObjectStringConverter"/> class.
		/// </summary>
		public NuGenObjectStringConverter()
		{
		}
		
		#endregion
	}
}
