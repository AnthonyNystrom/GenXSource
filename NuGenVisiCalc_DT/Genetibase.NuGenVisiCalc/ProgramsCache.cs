/* -----------------------------------------------
 * ProgramsCache.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.NuGenVisiCalc.Programs;
using System.Reflection;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed class ProgramsCache
	{
		private IList<ProgramDescriptor> _programs;

		public IList<ProgramDescriptor> Programs
		{
			get
			{
				Debug.Assert(_programs != null, "_programs != null");
				return _programs;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="assembly"/> is <see langword="null"/>.</para>
		/// </exception>
		public static ProgramsCache FromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			ProgramsCache cache = new ProgramsCache();
			cache._programs = ProgramLoader.LoadProgramsFromAssembly(assembly);
			return cache;
		}

		private ProgramsCache()
		{
		}
	}
}
