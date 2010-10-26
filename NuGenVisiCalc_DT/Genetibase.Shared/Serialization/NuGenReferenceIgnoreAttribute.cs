/* -----------------------------------------------
 * NuGenReferenceIgnoreAttribute.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Indicates that the class should not be serialized as a reference. Affects only if the type of the
	/// associated property is serialized as a class.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class NuGenReferenceIgnoreAttribute : Attribute
	{
		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenReferenceIgnoreAttribute"/> class.
		/// </summary>
		public NuGenReferenceIgnoreAttribute()
		{
		}
		
		#endregion
	}
}
