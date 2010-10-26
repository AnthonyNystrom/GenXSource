/* -----------------------------------------------
 * NuGenPropertyDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Represents a <see cref="PropertyDescriptor"/> wrapper that stores an instance to operate properties of.
	/// </summary>
	public sealed class NuGenPropertyDescriptor
	{
		private PropertyDescriptor _property;
		private object _instance;

		/// <summary>
		/// Gets the value of the associated property.
		/// </summary>
		public T GetValue<T>()
		{
			return (T)this.GetValueInternal();
		}

		/// <summary>
		/// Gets the value of the associated property.
		/// </summary>
		public object GetValue()
		{
			return this.GetValueInternal();
		}

		private object GetValueInternal()
		{
			return _property.GetValue(_instance);
		}

		/// <summary>
		/// Sets the specified <paramref name="value"/> for the associated property.
		/// </summary>
		/// <param name="value"></param>
		public void SetValue(object value)
		{
			Debug.Assert(_property != null, "_property != null");
			Debug.Assert(_instance != null, "_instance != null");

			_property.SetValue(_instance, value);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyDescriptor"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="property"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPropertyDescriptor(PropertyDescriptor property, object instance)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}

			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_property = property;
			_instance = instance;
		}
	}
}
