/* -----------------------------------------------
 * ParamsCache.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Genetibase.NuGenVisiCalc.Params;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed class ParamsCache
	{
		private IList<ParamDescriptor> _params;

		public IList<ParamDescriptor> Params
		{
			get
			{
				Debug.Assert(_params != null, "_params != null");
				return _params;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="assembly"/> is <see langword="null"/>.</para>
		/// </exception>
		public static ParamsCache FromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			ParamsCache cache = new ParamsCache();
			cache._params = ParamLoader.LoadParamsFromAssembly(assembly);
			return cache;
		}

		private ParamsCache()
		{
		}
	}
}
