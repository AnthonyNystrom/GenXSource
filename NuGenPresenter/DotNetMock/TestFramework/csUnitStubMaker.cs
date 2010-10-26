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
	/// <see cref="IStubMaker"/> for csUnit.
	/// </summary>
	public class csUnitStubMaker : AbstractStubMaker
	{
		/// <summary>
		/// Create csUnit stub maker.
		/// </summary>
		/// <param name="providerAssembly">csUnit assembly</param>
		/// <param name="linker">reflection linkage provider</param>
		public csUnitStubMaker(
			Assembly providerAssembly,
			IDynamicLinker linker
			)
			: base("csUnit.Assert", providerAssembly, linker)
		{
		}
		/// <summary>
		/// Implement stub methods that forward to the
		/// csUnit.Assert class.
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
				parameterTypes.RemoveAt(0);
				parameterTypes.Add(typeof(string));
				for (int i = 1; i<parameterTypes.Count; ++i) 
				{
					EmitLdarg(ilg, i+1);
				}
				EmitLdarg(ilg, 1);
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
					return "True";
				case "AssertNotNull":
					return "NotNull";
				case "AssertEquals":
					return "Equals";
				case "Fail":
					return "Fail";
				case "AssertNull":
					return "Null";
				default:
					throw new ArgumentException(String.Format(
						"Cannot map method name {0}",
						mi.Name
						));
			}
		}
	}
}
