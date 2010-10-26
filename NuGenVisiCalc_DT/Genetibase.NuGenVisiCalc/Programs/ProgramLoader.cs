/* -----------------------------------------------
 * ProgramLoader.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.NuGenVisiCalc.Programs
{
	internal static class ProgramLoader
	{
		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="assembly"/> is <see langword="null"/>.</para>
		/// </exception>
		public static IList<ProgramDescriptor> LoadProgramsFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			IList<ProgramDescriptor> programs = new List<ProgramDescriptor>();
			Type[] types = assembly.GetTypes();

			foreach (Type type in types)
			{
				if (type.IsAbstract || !type.IsSubclassOf(typeof(NodeBase)))
				{
					continue;
				}
				
				Object[] programAttributes = type.GetCustomAttributes(typeof(ProgramAttribute), false);

				if (programAttributes.Length > 0)
				{
					ProgramAttribute programAttribute = (ProgramAttribute)programAttributes[0];
					programs.Add(new ProgramDescriptor(type, programAttribute.ProgramName));
				}
			}

			return programs;
		}
	}
}
