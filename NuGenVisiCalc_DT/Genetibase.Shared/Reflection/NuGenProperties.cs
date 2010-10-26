/* -----------------------------------------------
 * NuGenProperties.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// </summary>
	public sealed class NuGenProperties
	{
		private object _instance;

		/// <summary>
		/// Retrieves the property with the specified name.
		/// </summary>
		/// <param name="propertyName"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="propertyName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="propertyName"/> is an empty string.</para>
		/// </exception>
		/// <exception cref="NuGenPropertyNotFoundException"/>
		public NuGenPropertyInfo this[string propertyName]
		{
			get
			{
				if (string.IsNullOrEmpty(propertyName))
				{
					throw new ArgumentNullException("propertyName");
				}

				PropertyInfo propertyInfo = this.GetPropertyInfo(propertyName);

				if (propertyInfo == null)
				{
					throw new NuGenPropertyNotFoundException(propertyName, _instance.GetType());
				}

				return new NuGenPropertyInfo(propertyInfo, _instance);
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public NuGenPropertyInfo FindProperty(string propertyName)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				return null;
			}

			PropertyInfo propertyInfo = this.GetPropertyInfo(propertyName);

			if (propertyInfo != null)
			{
				return new NuGenPropertyInfo(propertyInfo, _instance);
			}

			return null;
		}

		private PropertyInfo GetPropertyInfo(string propertyName)
		{
			Debug.Assert(!string.IsNullOrEmpty(propertyName), "!string.IsNullOrEmpty(propertyName)");
			return _instance.GetType().GetProperty(propertyName, NuGenBinding.Instance);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Properties"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenProperties(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
		}
	}
}
