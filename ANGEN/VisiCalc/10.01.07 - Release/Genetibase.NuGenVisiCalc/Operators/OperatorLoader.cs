/* -----------------------------------------------
 * OperatorLoader.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.NuGenVisiCalc.Operators
{
	/// <summary>
	/// Retrieves the list of available operators for an assembly of type.
	/// </summary>
	internal static class OperatorLoader
	{
		public static Dictionary<String, OperatorDescriptor> LoadOperatorsFromAssembly(Assembly assembly)
		{
			if (assembly == null)
			{
				throw new ArgumentNullException("assembly");
			}

			Dictionary<String, OperatorDescriptor> operatorDescriptors = new Dictionary<String, OperatorDescriptor>();
			Type[] types = assembly.GetTypes();

			foreach (Type type in types)
			{
				if (type.IsAbstract)
				{
					continue;
				}

				PopulateOperatorDescriptorsFromType(operatorDescriptors, type);
			}

			return operatorDescriptors;
		}

		public static Dictionary<String, OperatorDescriptor> LoadOperatorsFromType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}

			Dictionary<String, OperatorDescriptor> operatorDescriptors = new Dictionary<String, OperatorDescriptor>();

			if (!type.IsAbstract)
			{
				PopulateOperatorDescriptorsFromType(operatorDescriptors, type);
			}

			return operatorDescriptors;
		}

		private static void PopulateOperatorDescriptorsFromType(Dictionary<String, OperatorDescriptor> operatorDescriptorsToPopulate, Type type)
		{
			Object[] operatorFixtures = type.GetCustomAttributes(typeof(OperatorFixtureAttribute), false);

			if (operatorFixtures.Length > 0)
			{
				MethodInfo[] allMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);

				foreach (MethodInfo currentMethod in allMethods)
				{
					Object[] operators = currentMethod.GetCustomAttributes(typeof(OperatorAttribute), false);

					if (operators.Length > 0)
					{
						OperatorAttribute op = (OperatorAttribute)operators[0];
						operatorDescriptorsToPopulate.Add(
							op.StringRepresentation
							, new OperatorDescriptor(currentMethod, op.Name, op.StringRepresentation, op.Precedence, op.PrimitiveOperator)
						);
					}
				}
			}
		}
	}
}
