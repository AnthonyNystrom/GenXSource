#region License
// Copyright (c) 2004 Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace DotNetMock.TestFramework
{
	/// <summary>
	/// <see cref="IStubMaker"/> for NUnit.
	/// </summary>
	public class NUnitStubMaker : AbstractStubMaker
	{
		/// <summary>
		/// Create NUnit stub maker.
		/// </summary>
		/// <param name="providerAssembly">nunit.framework assembly</param>
		/// <param name="linker">reflection linkage provider</param>
		public NUnitStubMaker(
			Assembly providerAssembly,
			IDynamicLinker linker
			)
			: base("NUnit.Framework.Assertion", providerAssembly, linker)
		{
		}
		/// <summary>
		/// Implement stub methods that forward to the
		/// NUnit.Framework.Assertion class.
		/// </summary>
		/// <param name="ilg"><see cref="ILGenerator"/> for the method
		/// we are stubbing</param>
		/// <param name="mi"><see cref="MethodInfo"/> for the method
		/// we are stubbing</param>
		public override void ImplementStubMethod(ILGenerator ilg, MethodInfo mi)
		{
			IList parameterTypes = GetParameterTypes(mi);
			for (int i = 0; i<parameterTypes.Count; ++i) 
			{
				EmitLdarg(ilg, i+1);
			}
			EmitProviderCall(ilg, mi.Name, parameterTypes);
		}
	}
}
