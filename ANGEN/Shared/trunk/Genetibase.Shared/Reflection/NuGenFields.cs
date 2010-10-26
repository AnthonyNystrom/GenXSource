/* -----------------------------------------------
 * NuGenFields.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
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
		private Object _instance;

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
		public NuGenFieldInfo this[String fieldName]
		{
			get
			{
				if (String.IsNullOrEmpty(fieldName))
				{
					throw new ArgumentNullException("fieldName");
				}

				FieldInfo fieldInfo = MemberFinder.FindField(_instance.GetType(), fieldName);

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
		public NuGenFields(Object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
		}
	}
}
