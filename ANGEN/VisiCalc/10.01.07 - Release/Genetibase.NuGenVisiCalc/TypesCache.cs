/* -----------------------------------------------
 * TypesCache.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using VisiTypes = Genetibase.NuGenVisiCalc.Types;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed class TypesCache
	{
		private IList<VisiTypes.TypeDescriptor> _types;

		public IList<VisiTypes.TypeDescriptor> Types
		{
			get
			{
				Debug.Assert(_types != null, "_types != null");
				return _types;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="assembly"/> is <see langword="null"/>.</para>
		/// </exception>
		public static TypesCache FromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			TypesCache cache = new TypesCache();
			cache._types = VisiTypes.TypeLoader.LoadTypesFromAssembly(assembly);
			return cache;
		}

		private TypesCache()
		{
		}
	}
}
