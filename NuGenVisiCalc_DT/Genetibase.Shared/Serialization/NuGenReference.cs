/* -----------------------------------------------
 * NuGenReference.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;

using System;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Represents an object that is serialized as a reference.
	/// </summary>
	public class NuGenReference
	{
		#region Properties.Public

		/*
		 * Object
		 */

		private object _object;

		/// <summary>
		/// Gets or sets the object being serialized as a reference.
		/// </summary>
		public object Object
		{
			[DebuggerStepThrough]
			get
			{
				return _object;
			}
			[DebuggerStepThrough]
			set
			{
				_object = value;
			}
		}

		/*
		 * PropertyInfo
		 */

		private NuGenPropertyInfo _propertyInfo;

		/// <summary>
		/// Gets or sets the <see cref="T:NuGenPropertyInfo"/> for the <see cref="P:NuGenReference.Object"/>.
		/// </summary>
		public NuGenPropertyInfo PropertyInfo
		{
			[DebuggerStepThrough]
			get
			{
				return _propertyInfo;
			}
			[DebuggerStepThrough]
			set
			{
				_propertyInfo = value;
			}
		}

		/*
		 * StandardPropertyInfo
		 */

		private PropertyInfo _standardPropertyInfo;

		/// <summary>
		/// Gets or sets the <see cref="T:PropertyInfo"/> for the <see cref="P:NuGenReference.Object"/>.
		/// </summary>
		public PropertyInfo StandardPropertyInfo
		{
			[DebuggerStepThrough]
			get
			{
				return _standardPropertyInfo;
			}
			[DebuggerStepThrough]
			set
			{
				_standardPropertyInfo = value;
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenReference"/> class.
		/// </summary>
		/// <param name="propertyInfo">Specifies the <see cref="T:NuGenPropertyInfo"/> of the object being
		/// serialized as a reference.</param>
		public NuGenReference(
			NuGenPropertyInfo propertyInfo
			)
			: this(propertyInfo, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenReference"/> class.
		/// </summary>
		/// <param name="propertyInfo">Specifies the <see cref="T:NuGenPropertyInfo"/> of the object being
		/// serialized as a reference.</param>
		/// <param name="standardPropertyInfo">Specifies the <see cref="T:PropertyInfo"/> of the object being
		/// serialized as a reference.</param>
		/// <param name="obj">Specifies the object being serialized as a reference.</param>
		public NuGenReference(
			NuGenPropertyInfo propertyInfo,
			PropertyInfo standardPropertyInfo,
			object obj
			)
		{
			this.PropertyInfo = propertyInfo;
			this.StandardPropertyInfo = standardPropertyInfo;
			this.Object = obj;
		}
		
		#endregion
	}
}
