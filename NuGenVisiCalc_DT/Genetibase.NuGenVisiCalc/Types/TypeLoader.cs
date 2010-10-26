/* -----------------------------------------------
 * TypeLoader.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.NuGenVisiCalc.Types
{
	internal static class TypeLoader
	{
		/// <summary>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="assembly"/> is <see langword="null"/>.</para>
		/// </exception>
		public static IList<TypeDescriptor> LoadTypesFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			IList<TypeDescriptor> typeDescriptors = new List<TypeDescriptor>();
			System.Type[] types = assembly.GetTypes();

			foreach (System.Type type in types)
			{
				if (type.IsAbstract || !type.IsSubclassOf(typeof(NodeBase)))
				{
					continue;
				}

				Object[] typeAttributes = type.GetCustomAttributes(typeof(TypeAttribute), false);

				if (typeAttributes.Length > 0)
				{
					TypeAttribute typeAttribute = (TypeAttribute)typeAttributes[0];
					typeDescriptors.Add(new TypeDescriptor(type, typeAttribute.TypeName));
				}
			}

			return typeDescriptors;
		}
	}
}
