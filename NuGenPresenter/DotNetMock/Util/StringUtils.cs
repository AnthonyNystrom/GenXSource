#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Collections;
#endregion

namespace DotNetMock.Util
{
	/// <summary>
	/// Perl-like utility functions.
	/// </summary>
	public class StringUtils
	{
		/// <summary>
		/// Format an object.
		/// </summary>
		/// <param name="scalar">object to format</param>
		/// <returns>string representation of an object value</returns>
		public static string FormatScalar(object scalar) 
		{
			return mapElement(scalar).ToString();
		}
		/// <summary>
		/// Format an array.
		/// </summary>
		/// <param name="args">array to format</param>
		/// <returns>string representation of array</returns>
		public static string FormatArray(params object[] args) 
		{
			return FormatCollection(args, DefaultElementMapper);
		}
		/// <summary>
		/// Format a collection..
		/// </summary>
		/// <param name="collection">collection to format</param>
		/// <returns>string representation of collection</returns>
		public static string FormatCollection(ICollection collection) 
		{
			return FormatCollection(collection, DefaultElementMapper);
		}
		#region Private Static
		/// <summary>
		/// Delegate for mapping/transforming objects.
		/// </summary>
		private delegate object Mapper(object element);
		/// <summary>
		/// Default <see cref="Mapper"/> for displaying objects.
		/// </summary>
		private static readonly Mapper DefaultElementMapper = new Mapper(mapElement);

		private static string FormatCollection(ICollection collection, Mapper mapper) 
		{
			return String.Join(", ", mapToString(mapCollection(
				mapper,
				collection
				)));
		}
		private static object mapElement(object arg) 
		{
			if ( arg is string ) 
			{
				string stringArg = arg as string;
				return "'" + stringArg + "'";
			}
			else if ( arg is DictionaryEntry ) 
			{
				DictionaryEntry entry = (DictionaryEntry) arg;
				return String.Format(
					"{0}={1}",
					entry.Key,
					mapElement(entry.Value)
					);
			}
			else if ( arg is ICollection ) 
			{
				return "["+FormatCollection(
					(ICollection) arg,
					new Mapper(mapElement)
					)+"]";
			}
			else 
			{
				return arg;
			}
		}
		private static object[] mapCollection(Mapper mapper, ICollection collection) 
		{
			object[] result = new object[collection.Count];
			int i = 0;
			foreach (object element in collection) 
			{
				result[i++] = mapper(element);
			}
			return result;
		}
		private static string[] mapToString(params object[] array) 
		{
			string[] result = new string[array.Length];
			for (int i = 0; i<array.Length; ++i) 
			{
				result[i] = array[i].ToString();
			}
			return result;
		}
		#endregion
	}
}
