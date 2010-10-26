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
	/// <see cref="IStubMaker"/> for MbUnit.
	/// </summary>
	public class MbUnitStubMaker : AbstractStubMaker
	{
		/// <summary>
		/// Create MbUnit stub maker.
		/// </summary>
		/// <param name="providerAssembly">MbUnit.Core assembly</param>
		/// <param name="linker">reflection linkage provider</param>
		public MbUnitStubMaker(
			Assembly providerAssembly,
			IDynamicLinker linker
			)
			: base("MbUnit.Core.Framework.Assert", providerAssembly, linker)
		{
		}
		/// <summary>
		/// Implement stub methods that forward to the
		/// MbUnit.Core.Framework.Assert class.
		/// </summary>
		/// <param name="ilg"><see cref="ILGenerator"/> for the method
		/// we are stubbing</param>
		/// <param name="mi"><see cref="MethodInfo"/> for the method
		/// we are stubbing</param>
		public override void ImplementStubMethod(ILGenerator ilg, MethodInfo mi)
		{
			IList parameterTypes = GetParameterTypes(mi);
			// take message parameter and put it at end of parameter list
			bool hasMessageParameter =
				parameterTypes.Count>0 &&
				typeof(string).Equals(parameterTypes[0])
				;
			if ( hasMessageParameter ) 
			{
				// original parameter count
				int n = parameterTypes.Count;
				// edit call signature
				parameterTypes.RemoveAt(0);
				parameterTypes.Add(typeof(string));
				parameterTypes.Add(typeof(object[]));
				// push arguments starting after message arg 1
				for (int i = 1; i<n; ++i) 
				{
					EmitLdarg(ilg, i+1);
				}
				// load format/message from arg 1
				EmitLdarg(ilg, 1);
				// add empty object[] array
				ilg.Emit(OpCodes.Ldc_I4_0);
				ilg.Emit(OpCodes.Newarr, typeof(object));
			} 
			else 
			{
				for (int i = 0; i<parameterTypes.Count; ++i) 
				{
					EmitLdarg(ilg, i+1);
				}
			}
			string methodName = MapMethod(mi);
			EmitProviderCall(ilg, methodName, parameterTypes);
		}

		private string MapMethod(MethodInfo mi) 
		{
			switch ( mi.Name ) 
			{
				case "Assert":
					return "IsTrue";
				case "AssertNotNull":
					return "IsNotNull";
				case "AssertEquals":
					return "AreEqual";
				case "Fail":
					return "Fail";
				case "AssertNull":
					return "IsNull";
				default:
					throw new ArgumentException(String.Format(
						"Cannot map method name {0}",
						mi.Name
						));
			}
		}
	}
}
