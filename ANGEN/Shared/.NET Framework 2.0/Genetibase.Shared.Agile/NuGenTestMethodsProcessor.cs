/* -----------------------------------------------
 * NuGenTestMethodsProcessor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Genetibase.Shared.Agile
{
	/// <summary>
	/// </summary>
	public static class NuGenTestMethodsProcessor
	{
		#region Methods.Public

		/// <summary>
		/// Retrieves a list of instance and static methods contained within the specified object. Test
		/// method is assumed to be marked with <see cref="TestAttribute"/>.
		/// </summary>
		/// <param name="testFixture"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="testFixture"/> is <see langword="null"/>.</para>
		/// </exception>
		public static List<NuGenTestInvoker> GetTestMethods(object testFixture)
		{
			if (testFixture == null)
			{
				throw new ArgumentNullException("testFixture");
			}

			List<NuGenTestInvoker> testMethods = new List<NuGenTestInvoker>();
			MethodInfo[] methods = testFixture.GetType().GetMethods(
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static
			);

			if (methods != null)
			{
				for (int i = 0; i < methods.Length; i++)
				{
					MethodInfo method = methods[i];
					object[] attributes = method.GetCustomAttributes(typeof(TestAttribute), false);

					if (attributes != null && attributes.Length > 0)
					{
						string testMethodName = method.Name;
						NuGenTestInvoker testMethod = null;

						if (method.IsStatic)
						{
							testMethod = (NuGenTestInvoker)NuGenTestInvoker.CreateDelegate(
								typeof(NuGenTestInvoker),
								method,
								true
							);
						}
						else
						{
							testMethod = (NuGenTestInvoker)NuGenTestInvoker.CreateDelegate(
								typeof(NuGenTestInvoker),
								testFixture,
								testMethodName,
								false,
								true
							);
						}

						Debug.Assert(testMethod != null, "testMethod != null");
						testMethods.Add(testMethod);
					}
				}
			}

			return testMethods;
		}

		/// <summary>
		/// Retrieves a list of static test methods contained within the specified type. Test method
		/// is assumed to be marked with <see cref="TestAttribute"/>.
		/// </summary>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException"><paramref name="testFixture"/> is <see langword="null"/>.</exception>
		public static List<NuGenTestInvoker> GetTestMethods(Type testFixture)
		{
			if (testFixture == null)
			{
				throw new ArgumentNullException("testFixture");
			}
			
			List<NuGenTestInvoker> testMethods = new List<NuGenTestInvoker>();
			MethodInfo[] methods = testFixture.GetMethods(
				BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static
			);

			if (methods != null)
			{
				for (int i = 0; i < methods.Length; i++)
				{
					MethodInfo method = methods[i];
					Debug.Assert(method.IsStatic, "method.IsStatic");

					object[] attributes = method.GetCustomAttributes(typeof(TestAttribute), false);

					if (attributes != null && attributes.Length > 0)
					{
						string testMethodName = method.Name;
						NuGenTestInvoker testMethod = (NuGenTestInvoker)NuGenTestInvoker.CreateDelegate(
							typeof(NuGenTestInvoker),
							method
						);
						
						Debug.Assert(testMethod != null, "testMethod != null");
						testMethods.Add(testMethod);
					}
				}
			}

			return testMethods;
		}

		#endregion
	}
}
