/* -----------------------------------------------
 * NuGenPropertyDescriptorCollection.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using Genetibase.Shared.Reflection;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Represents a <see cref="PropertyDescriptorCollection"/> wrapper that stores the instance to retrieve properties for.
	/// </summary>
	public sealed class NuGenPropertyDescriptorCollection
	{
		private PropertyDescriptorCollection _properties;
		private Object _instance;

		/// <summary>
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="propertyName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="propertyName"/> is an empty string.</para>
		/// </exception>
		/// <exception cref="NuGenPropertyNotFoundException"/>
		public NuGenPropertyDescriptor this[String propertyName]
		{
			get
			{
				if (String.IsNullOrEmpty(propertyName))
				{
					throw new ArgumentNullException("propertyName");
				}

				Debug.Assert(_properties != null, "_properties != null");
				Debug.Assert(_instance != null, "_instance != null");

				PropertyDescriptor property = _properties[propertyName];

				if (property == null)
				{
					throw new NuGenPropertyNotFoundException(propertyName, _instance.GetType());
				}

				return new NuGenPropertyDescriptor(property, _instance);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyDescriptorCollection"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="properties"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenPropertyDescriptorCollection(PropertyDescriptorCollection properties, Object instance)
		{
			if (properties == null)
			{
				throw new ArgumentNullException("properties");
			}

			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			
			_properties = properties;
			_instance = instance;
		}
	}
}
