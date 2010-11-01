/* -----------------------------------------------
 * Enums.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Defines the visibility an object has to the serializer.
	/// </summary>
	public enum NuGenSerializationVisibility
	{
		/// <summary>
		/// The object is not serialized.
		/// </summary>
		Hidden,

		/// <summary>
		/// The value of the object is serialized rather than the object itself.
		/// </summary>
		Content,

		/// <summary>
		/// The object is serialized as a class.
		/// </summary>
		Class,
		
		/// <summary>
		/// The object is serialized as a reference.
		/// </summary>
		Reference
	}
}
