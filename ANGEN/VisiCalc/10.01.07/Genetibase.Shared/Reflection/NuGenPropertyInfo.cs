/* -----------------------------------------------
 * NuGenPropertyInfo.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// <see cref="PropertyInfo"/> wrapper.
	/// </summary>
	public sealed class NuGenPropertyInfo
	{
		private PropertyInfo _propertyInfo;
		private object _instance;

		/// <summary>
		/// </summary>
		/// <typeparam name="F"></typeparam>
		/// <returns></returns>
		public F GetValue<F>()
		{
			return (F)this.GetValueInternal();
		}

		/// <summary>
		/// </summary>
		public object GetValue()
		{
			return this.GetValueInternal();
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		public void SetValue(object value)
		{
			this.SetValueInternal(value);
		}

		/// <summary>
		/// </summary>
		public PropertyInfo GetUnderlyingPropertyInfo()
		{
			return _propertyInfo;
		}

		/// <summary>
		/// </summary>
		/// <returns></returns>
		private object GetValueInternal()
		{
			return _propertyInfo.GetValue(_instance, null);
		}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private void SetValueInternal(object value)
		{
			_propertyInfo.SetValue(_instance, value, null);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyInfo"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="propertyInfo"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPropertyInfo(PropertyInfo propertyInfo, object instance)
		{
			if (propertyInfo == null)
			{
				throw new ArgumentNullException("propertyInfo");
			}

			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_propertyInfo = propertyInfo;
			_instance = instance;
		}
	}
}
