/* -----------------------------------------------
 * INuGenSerializable.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Xml;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Indicates that this class implements custom serialization algorithm.
	/// </summary>
	public interface INuGenSerializable
	{
		/// <summary>
		/// Deserializes the object.
		/// </summary>
		/// <param name="converter">Specifies the <see cref="NuGenObjectStringConverter"/> used to convert
		/// string values to the appropriate instances.</param>
		/// <param name="xmlTextReader">Specifies the <see cref="XmlTextReader"/> used to read deserialization data.</param>
		void Deserialize(INuGenObjectStringConverter converter, XmlTextReader xmlTextReader);

		/// <summary>
		/// Serializes the object.
		/// </summary>
		/// <param name="converter">Specifies the <see cref="NuGenObjectStringConverter"/> used to convert
		/// instances to the appropriate string values.</param>
		/// <param name="xmlTextWriter">Specifies the <see cref="XmlTextWriter"/> used to write serialization data.</param>
		void Serialize(INuGenObjectStringConverter converter, XmlTextWriter xmlTextWriter);
	}
}
