/* -----------------------------------------------
 * NuGenFields.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// </summary>
	public sealed class NuGenFields
	{
		private object _instance;

		/// <summary>
		/// Retrieves the field with the specified name.
		/// </summary>
		/// <param name="fieldName"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="fieldName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="fieldName"/> is an empty string.</para>
		/// </exception>
		/// <exception cref="NuGenFieldNotFoundException"/>
		public NuGenFieldInfo this[string fieldName]
		{
			get
			{
				if (string.IsNullOrEmpty(fieldName))
				{
					throw new ArgumentNullException("fieldName");
				}

				Type instanceType = _instance.GetType();

				FieldInfo fieldInfo = instanceType.GetField(fieldName, NuGenBinding.Instance);
				Type baseType = instanceType.BaseType;

				while (fieldInfo == null && baseType != typeof(object))
				{
					fieldInfo = baseType.GetField(fieldName, NuGenBinding.Instance);
					baseType = baseType.BaseType;
				}

				if (fieldInfo == null)
				{
					throw new NuGenFieldNotFoundException(fieldName, _instance.GetType());
				}

				return new NuGenFieldInfo(
					fieldInfo,
					_instance
				);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFields"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenFields(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
		}
	}
}
