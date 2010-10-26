/* -----------------------------------------------
 * NuGenProperties.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
		private Object _instance;

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
		public NuGenPropertyInfo this[String propertyName]
		{
			get
			{
				if (String.IsNullOrEmpty(propertyName))
				{
					throw new ArgumentNullException("propertyName");
				}

				PropertyInfo propertyInfo = MemberFinder.FindProperty(_instance.GetType(), propertyName);

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
		public NuGenPropertyInfo FindProperty(String propertyName)
		{
			if (String.IsNullOrEmpty(propertyName))
			{
				return null;
			}

			PropertyInfo propertyInfo = MemberFinder.FindProperty(_instance.GetType(), propertyName);

			if (propertyInfo != null)
			{
				return new NuGenPropertyInfo(propertyInfo, _instance);
			}

			return null;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Properties"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenProperties(Object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
		}
	}
}
