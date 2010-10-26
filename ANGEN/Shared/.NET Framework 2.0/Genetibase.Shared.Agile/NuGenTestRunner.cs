/* -----------------------------------------------
 * NuGenTestRunner.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.Agile
{
	/// <summary>
	/// </summary>
	public static class NuGenTestRunner
	{
		#region Methods.Public.Static

		/*
		 * RunTests
		 */

		/// <summary>
		/// </summary>
		/// <param name="testsProvider"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="testFixture"/> is <see langword="null"/>.</para>
		/// </exception>
		public static void RunTests(Type testFixture)
		{
			if (testFixture == null)
			{
				throw new ArgumentNullException("testFixture");
			}

			List<NuGenTestInvoker> testMethods = NuGenTestMethodsProcessor.GetTestMethods(testFixture);

			if (testMethods != null)
			{
				foreach (NuGenTestInvoker testInvoker in testMethods)
				{
					testInvoker.Invoke();
				}
			}
		}

		public static void RunTests(object testFixture)
		{
			if (testFixture == null)
			{
				throw new ArgumentNullException("testFixture");
			}

		}

		#endregion
	}
}
