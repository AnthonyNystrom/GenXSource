/* -----------------------------------------------
 * NuGenSerializationVisibilityAttribute.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Indicates that the property or class marked with this <see cref="NuGenSerializationVisibilityAttribute"/> will be
	/// serialized.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple=false, Inherited=true)]
	public sealed class NuGenSerializationVisibilityAttribute : Attribute
	{
		#region Properties.Public

		/* 
		 * Visibility
		 */

		/// <summary>
		/// Determines the visibility the object has to the serializer.
		/// </summary>
		private NuGenSerializationVisibility _visibility = NuGenSerializationVisibility.Hidden;

		/// <summary>
		/// Gets the visibility the object has to the serializer.
		/// </summary>
		public NuGenSerializationVisibility Visibility
		{
			[DebuggerStepThrough]
			get 
			{
				return _visibility;
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSerializationVisibilityAttribute"/> class.
		/// </summary>
		public NuGenSerializationVisibilityAttribute()
			: this(NuGenSerializationVisibility.Content)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSerializationVisibilityAttribute"/> class.
		/// </summary>
		/// <param name="visibility">Specifies the visibility the object has to the serializer.</param>
		public NuGenSerializationVisibilityAttribute(NuGenSerializationVisibility visibility)
		{
			_visibility = visibility;
		}
		
		#endregion
	}
}
