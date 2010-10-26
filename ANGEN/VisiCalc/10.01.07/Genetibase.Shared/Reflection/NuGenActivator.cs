/* -----------------------------------------------
 * NuGenActivator.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// <see cref="T:System.Activator"/> wrapper.
	/// </summary>
	public static class NuGenActivator
	{
		#region Methods.Public.Static

		/// <summary>
		/// Creates an <see cref="Object"/> from the type specified.
		/// </summary>
		/// <param name="typeName">Specifies the type of the Object to create.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="typeName"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="typeName"/> is an emtpy String.
		/// </exception>
		public static Object CreateObject(String typeName)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			return NuGenActivator.CreateObject(typeName, new Object[0]);
		}

		/// <summary>
		/// Creates an <see cref="Object"/> from the type specified.
		/// </summary>
		/// <param name="type">Specifies the type of the Object ot create.</param>
		/// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null"/>.</exception>
		public static Object CreateObject(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			return Activator.CreateInstance(type);
		}

		/// <summary>
		/// Creates an <see cref="Object"/> from the type specified.
		/// </summary>
		/// <param name="typeName">Specifies the type of the Object to create.</param>
		/// <param name="arguments">Specifies the arguments to pass to the constructor.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="typeName"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="typeName"/> is an empty String.
		/// </exception>
		public static Object CreateObject(String typeName, Object[] arguments)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			Type type = NuGenTypeFinder.GetType(typeName);
			
			if (type != null)
			{
				return Activator.CreateInstance(type, arguments);
			}

			return null;
		}

		/// <summary>
		/// Creates an <see cref="Object"/> from the type specified.
		/// </summary>
		/// <param name="type">Specifies the type of the Object to create.</param>
		/// <param name="arguments">Specifies the arguments to pass to the constructor.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="type"/> is <see langword="null"/>.
		/// </exception>
		public static Object CreateObject(Type type, Object[] arguments)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			
			return NuGenActivator.CreateObject(type.ToString(), arguments);
		}

		#endregion
	}
}
