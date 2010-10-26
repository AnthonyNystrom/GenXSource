/* -----------------------------------------------
 * NuGenMethods.cs
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
	public sealed class NuGenMethods
	{
		private object _instance;
		private Type _staticType;

		/// <summary>
		/// Retrieves the method with the specified name.
		/// </summary>
		/// <param name="methodName"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="methodName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="methodName"/> is an empty string.</para>
		/// </exception>
		/// <exception cref="NuGenMethodNotFoundException"/>
		public NuGenMethodInfo this[string methodName]
		{
			get
			{
				if (string.IsNullOrEmpty(methodName))
				{
					throw new ArgumentNullException("methodName");
				}

				if (_instance != null)
				{
					MethodInfo methodInfo = _instance.GetType().GetMethod(methodName, NuGenBinding.Instance);

					if (methodInfo == null)
					{
						throw new NuGenMethodNotFoundException(methodName, _instance.GetType());
					}
					
					return new NuGenMethodInfo(
						methodInfo,
						_instance
					);
				}

				return new NuGenMethodInfo(
					_staticType.GetMethod(methodName, NuGenBinding.Static)
				);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMethods"/> class.<para/>
		/// Use this contructor to invoke static members.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="staticType"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenMethods(Type staticType)
		{
			if (staticType == null)
			{
				throw new ArgumentNullException("staticType");
			}

			_staticType = staticType;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMethods"/> class.<para/>
		/// Use this constructor to invoke instance members.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenMethods(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
		}
	}
}
