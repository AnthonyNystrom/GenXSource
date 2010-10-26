/* -----------------------------------------------
 * ParamLoader.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.NuGenVisiCalc.Params
{
	internal static class ParamLoader
	{
		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="assembly"/> is <see langword="null"/>.</para>
		/// </exception>
		public static IList<ParamDescriptor> LoadParamsFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			IList<ParamDescriptor> paramDescriptors = new List<ParamDescriptor>();
			System.Type[] types = assembly.GetTypes();

			foreach (System.Type type in types)
			{
				if (type.IsAbstract || !type.IsSubclassOf(typeof(NodeBase)))
				{
					continue;
				}

				Object[] paramAttributes = type.GetCustomAttributes(typeof(ParamAttribute), false);

				if (paramAttributes.Length > 0)
				{
					ParamAttribute paramAttribute = (ParamAttribute)paramAttributes[0];
					paramDescriptors.Add(new ParamDescriptor(type, paramAttribute.ParamName));
				}
			}

			return paramDescriptors;
		}
	}
}
