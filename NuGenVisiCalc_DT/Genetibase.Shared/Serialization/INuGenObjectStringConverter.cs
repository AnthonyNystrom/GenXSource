/* -----------------------------------------------
 * INuGenObjectStringConverter.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Indicates that this class provides methods that allow to convert a class instance
	/// to its string representation and vice versa.
	/// </summary>
	public interface INuGenObjectStringConverter
	{
		/// <summary>
		/// </summary>
		string ObjectToString(object objectToConvert);

		/// <summary>
		/// </summary>
		object StringToObject(string stringToConstructObjectFrom, Type objectType);
	}
}
