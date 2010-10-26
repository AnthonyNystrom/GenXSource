#region License
// Copyright (c) 2004 Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace DotNetMock.TestFramework
{
	/// <summary>
	/// Interface for making method stubs for test framework providers.
	/// </summary>
	public interface IStubMaker
	{
		/// <summary>
		/// Implement one of the methods of the interface we are required
		/// to stub out.
		/// </summary>
		/// <param name="ilg"><see cref="ILGenerator"/> of the method
		/// on the dynamically generated type</param>
		/// <param name="mi"><see cref="MethodInfo"/> of the method
		/// on the interface we want to implement</param>
		void ImplementStubMethod(ILGenerator ilg, MethodInfo mi);
	}
}
