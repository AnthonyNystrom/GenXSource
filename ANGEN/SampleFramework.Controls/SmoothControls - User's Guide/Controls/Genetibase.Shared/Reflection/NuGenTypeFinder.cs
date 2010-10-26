/* -----------------------------------------------
 * NuGenTypeFinder.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// Provides helper methods to distinguish types.
	/// </summary>
	public static class NuGenTypeFinder
	{
		/// <summary>
		/// Returns <see cref="Type"/> from its string representation if the <see cref="Type"/> exists in the current
		/// application domain; otherwise, <see langword="null"/>.
		/// </summary>
		/// <param name="typeName">Specifies the <see cref="Type"/> string representation.</param>
		/// <exception cref="T:System.ArgumentNullException">
		/// <paramref name="typeName"/> is <see langword="null"/>.
		/// -or-
		/// <paramref name="typeName"/> is an empty string.
		/// </exception>
		public static Type GetType(String typeName)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			Type type = Type.GetType(typeName);

			if (type != null) 
			{
				return type;
			}

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

			foreach (Assembly assembly in assemblies) 
			{
				type = assembly.GetType(typeName);

				if (type != null) 
				{
					return type;
				}
			}

			return null;
		}
	}
}
