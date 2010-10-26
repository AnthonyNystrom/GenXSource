/* -----------------------------------------------
 * MemberFinder.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Genetibase.Shared.Reflection
{
	internal static class MemberFinder
	{
		public static EventInfo FindEvent(Type instanceType, String eventName)
		{
			EventInfo info;

			do
			{
				info = instanceType.GetEvent(eventName, NuGenBinding.Instance);
				instanceType = instanceType.BaseType;
			} while (info == null && instanceType != _objectType);

			return info;
		}

		public static FieldInfo FindField(Type instanceType, String fieldName)
		{
			FieldInfo info;

			do
			{
				info = instanceType.GetField(fieldName, NuGenBinding.Instance);
				instanceType = instanceType.BaseType;
			} while (info == null && instanceType != _objectType);

			return info;
		}

		public static MethodInfo FindMethod(Type instanceType, String methodName)
		{
			MethodInfo info;

			do
			{
				info = instanceType.GetMethod(methodName, NuGenBinding.Instance);
				instanceType = instanceType.BaseType;
			} while (info == null && instanceType != _objectType);

			return info;
		}

		public static PropertyInfo FindProperty(Type instanceType, String propertyName)
		{
			PropertyInfo info;

			do
			{
				info = instanceType.GetProperty(propertyName, NuGenBinding.Instance);
				instanceType = instanceType.BaseType;
			} while (info == null && instanceType != _objectType);

			return info;
		}

		private static readonly Type _objectType = typeof(Object);
	}
}
