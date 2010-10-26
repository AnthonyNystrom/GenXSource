/* -----------------------------------------------
 * OperatorsCache.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Genetibase.NuGenVisiCalc.Operators;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed class OperatorsCache
	{
		private Dictionary<String, OperatorDescriptor> _operators;

		public Dictionary<String, OperatorDescriptor> Operators
		{
			get
			{
				return _operators;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="assembly"/> is <see langword="null"/>.</para>
		/// </exception>
		public static OperatorsCache FromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			OperatorsCache cache = new OperatorsCache();
			cache._operators = OperatorLoader.LoadOperatorsFromAssembly(assembly);
			return cache;
		}

		public OperatorsCache()
		{
		}
	}
}
