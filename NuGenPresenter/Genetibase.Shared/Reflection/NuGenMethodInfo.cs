/* -----------------------------------------------
 * NuGenMethodInfo.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// <see cref="MethodInfo"/> wrapper.
	/// </summary>
	public sealed class NuGenMethodInfo
	{
		private MethodInfo _methodInfo;
		private object _instance;

		/// <summary>
		/// Invokes a method with no return value and no formal parameters.
		/// </summary>
		public void Invoke()
		{
			_methodInfo.Invoke(_instance, null);
		}

		/// <summary>
		/// Invokes a method with formal parameters and with no return value.
		/// </summary>
		/// <param name="parameters"></param>
		public void Invoke(params object[] parameters)
		{
			_methodInfo.Invoke(_instance, parameters);
		}

		/// <summary>
		/// Invokes a method with formal parameters and a return value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameters"></param>
		/// <returns></returns>
		public T Invoke<T>(params object[] parameters)
		{
			return (T)_methodInfo.Invoke(_instance, parameters);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMethodInfo"/> class.<para/>
		/// Use this constructor to invoke static methods.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="methodInfo"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenMethodInfo(MethodInfo methodInfo)
			: this(methodInfo, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenMethodInfo"/> class.
		/// </summary>
		/// <param name="methodInfo"></param>
		/// <param name="instance"><see langword="null"/> if the method is static.</param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="methodInfo"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenMethodInfo(MethodInfo methodInfo, object instance)
		{
			if (methodInfo == null)
			{
				throw new ArgumentNullException("methodInfo");
			}

			_instance = instance;
			_methodInfo = methodInfo;
		}
	}
}
