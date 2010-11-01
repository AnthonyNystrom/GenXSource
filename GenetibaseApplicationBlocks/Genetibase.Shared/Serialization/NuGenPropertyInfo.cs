/* -----------------------------------------------
 * NuGenPropertyInfo.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;

namespace Genetibase.Shared.Serialization
{
	/// <summary>
	/// Contains the associated property info.
	/// </summary>
	public class NuGenPropertyInfo
	{
		#region Properties.Public

		/*
		 * Count
		 */

		private int _count = 0;
		
		/// <summary>
		/// Gets or sets the number of elements if the property is a collection.
		/// </summary>
		public int Count
		{
			[DebuggerStepThrough]
			get 
			{
				return _count;
			}
			[DebuggerStepThrough]
			set
			{
				_count = value;
			}
		}

		/*
		 * DefaultValue
		 */

		private object _defaultValue = null;

		/// <summary>
		/// Gets or sets the default value for the property.
		/// </summary>
		public object DefaultValue
		{
			[DebuggerStepThrough]
			get
			{
				return _defaultValue;
			}
			[DebuggerStepThrough]
			set
			{
				_defaultValue = value;
			}
		}

		/*
		 * IsKey
		 */

		private bool _isKey = false;

		/// <summary>
		/// Gets or sets the value indicating whether type of the property is complex and may contain properties
		/// to serialize.
		/// </summary>
		public bool IsKey
		{
			[DebuggerStepThrough]
			get
			{
				return _isKey;
			}
			[DebuggerStepThrough]
			set
			{
				_isKey = value;
			}
		}

		/*
		 * IsList
		 */

		private bool _isList = false;

		/// <summary>
		/// Gets or sets the value indicating whether the property should be serialized as a list.
		/// </summary>
		public bool IsList
		{
			[DebuggerStepThrough]
			get
			{
				return _isList;
			}
			[DebuggerStepThrough]
			set
			{
				_isList = value;
			}
		}

		/*
		 * IsReference
		 */

		private bool _isReference = false;

		/// <summary>
		/// Gets or sets the value indicating whether the property should be serialized as a reference.
		/// </summary>
		public bool IsReference
		{
			[DebuggerStepThrough]
			get
			{
				return _isReference;
			}
			[DebuggerStepThrough]
			set
			{
				_isReference = value;
			}
		}

		/*
		 * IsSerializable
		 */

		private bool _isSerializable = false;

		/// <summary>
		/// Gets or sets the value indicating whether the property is serializable.
		/// </summary>
		public bool IsSerializable
		{
			[DebuggerStepThrough]
			get
			{
				return _isSerializable;
			}
			[DebuggerStepThrough]
			set
			{
				_isSerializable = value;
			}
		}

		/*
		 * Name
		 */

		private string _name = null;

		/// <summary>
		/// Gets or sets the property name.
		/// </summary>
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return _name;
			}
			[DebuggerStepThrough]
			set
			{
				_name = value;
			}
		}

		/*
		 * Parent
		 */

		private NuGenPropertyInfo _parent = null;

		/// <summary>
		/// Gets or sets the parent property.
		/// </summary>
		public NuGenPropertyInfo Parent
		{
			[DebuggerStepThrough]
			get
			{
				return _parent;
			}
			[DebuggerStepThrough]
			set
			{
				_parent = value;
			}
		}

		/*
		 * Properties
		 */

		private NuGenPropertyInfoCollection _properties = null;

		/// <summary>
		/// Gets or sets a collection of properties the type of the property associated with this
		/// <see cref="T:NuGenPropertyInfo"/> contains.
		/// </summary>
		public NuGenPropertyInfoCollection Properties
		{
			[DebuggerStepThrough]
			get
			{
				return _properties;
			}
			[DebuggerStepThrough]
			set
			{
				_properties = value;
			}
		}

		/*
		 * ReferenceCode
		 */

		private int _referenceCode = -1;
		
		/// <summary>
		/// Gets or sets the code for the reference if the property is serialized as a reference.
		/// </summary>
		public int ReferenceCode
		{
			[DebuggerStepThrough]
			get
			{
				return _referenceCode;
			}
			[DebuggerStepThrough]
			set
			{
				_referenceCode = value;
			}
		}

		/*
		 * Type
		 */

		/// <summary>
		/// Gets the type of the value the property contains.
		/// </summary>
		public Type Type
		{
			get 
			{
				if (this.Value != null) 
				{
					return this.Value.GetType();
				}

				return typeof(object);
			}
		}

		/*
		 * Value
		 */

		private object _value = null;

		/// <summary>
		/// Gets or sets the value of the property.
		/// </summary>
		public object Value
		{
			[DebuggerStepThrough]
			get
			{
				return _value;
			}
			[DebuggerStepThrough]
			set
			{
				_value = value;
			}
		}

		#endregion

		#region Constructors
	
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyInfo"/> class.
		/// </summary>
		/// <param name="name">Specifies the property name.</param>
		/// <param name="value">Specifies the property value.</param>
		/// <param name="isKey">Specifies the value indicating whether the type of the property is complex
		/// and may contain properties to serialize.</param>
		/// <param name="isReference">Specifies the value indicating whether the property should be serialized
		/// as a reference.</param>
		/// <param name="isList">Specifies the value indicating whether the property implements
		/// the <see cref="T:System.Collections.IList"/> or <see cref="T:System.Collections.IEnumerable"/> interface.</param>
		public NuGenPropertyInfo(
			string name,
			object value,
			bool isKey,
			bool isReference,
			bool isList
			)
			: this(name, value, null, isKey, isReference, isList)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyInfo"/> class.
		/// </summary>
		/// <param name="name">Specifies the property name.</param>
		/// <param name="value">Specifies the property value.</param>
		/// <param name="defaultValue">Specifies the property default value.</param>
		/// <param name="isKey">Specifies the value indicating whether the type of the property is complex
		/// and may contain properties to serialize.</param>
		/// <param name="isReference">Specifies the value indicating whether the property should be serialized
		/// as a reference.</param>
		/// <param name="isList">Specifies the value indicating whether the property implements
		/// the <see cref="T:System.Collections.IList"/> interface.</param>
		public NuGenPropertyInfo(
			string name,
			object value,
			object defaultValue,
			bool isKey,
			bool isReference,
			bool isList
			)
		{
			this.Properties = new NuGenPropertyInfoCollection(this);
			this.Name = name;
			this.Value = value;
			this.DefaultValue = defaultValue;
			this.IsKey = isKey;
			this.IsReference = isReference;
			this.IsList = isList;
		}
		
		#endregion
	}
}
