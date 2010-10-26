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
	/// Abstract base class for implementations of <see cref="IStubMaker"/>.
	/// </summary>
	public abstract class AbstractStubMaker :
		IStubMaker
	{
		private Type _providerClass;

		/// <summary>
		/// Initialize stub maker.
		/// </summary>
		/// <param name="providerClass">class that we will
		/// forward all methods to</param>
		public AbstractStubMaker( Type providerClass )
		{
			if ( providerClass==null ) 
			{
				throw new ArgumentNullException(
					"providerClass",
					"Cannot specify null provider class"
					);
			}
			_providerClass = providerClass;
		}
		/// <summary>
		/// Initialize a stub maker.
		/// </summary>
		/// <param name="providerClassName">name of assertion
		/// class</param>
		/// <param name="providerAssembly">Assembly that provides
		/// assertion class</param>
		/// <param name="linker">reflection linkage provider</param>
		public AbstractStubMaker(
			string providerClassName,
			Assembly providerAssembly,
			IDynamicLinker linker
			) :
			this(linker.GetType(providerClassName, providerAssembly))
		{
		}

		#region IStubMaker Members

		/// <summary>
		/// Implement one of the methods of the interface we are required
		/// to stub out.
		/// </summary>
		/// <param name="ilg"><see cref="ILGenerator"/> of the method
		/// on the dynamically generated type</param>
		/// <param name="mi"><see cref="MethodInfo"/> of the method
		/// on the interface we want to implement</param>
		public abstract void ImplementStubMethod(ILGenerator ilg, MethodInfo mi);

		#endregion

		/// <summary>
		/// Get list of parameter types on a <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="targetMethodInfo"><see cref="MethodInfo"/> of method we're
		/// interested in</param>
		/// <returns><see cref="IList"/> of parameter types.</returns>
		protected static IList GetParameterTypes( MethodInfo targetMethodInfo ) 
		{
			ArrayList types = new ArrayList();
			foreach (ParameterInfo pi in targetMethodInfo.GetParameters()) 
			{
				types.Add(pi.ParameterType);
			}
			return types;
		}
		/// <summary>
		/// Emit IL Call to method.
		/// </summary>
		/// <param name="ilg"><see cref="ILGenerator"/> of the method
		/// on the dynamically generated type</param>
		/// <param name="methodName">name of method to call</param>
		/// <param name="parameterTypeList"><see cref="IList"/>
		/// of parameters in method we want to call</param>
		protected void EmitProviderCall(
			ILGenerator ilg,
			string methodName, IList parameterTypeList
			) 
		{
			Type[] parameterTypes = new Type[parameterTypeList.Count];
			for (int i = 0; i<parameterTypeList.Count; ++i) 
			{
				parameterTypes[i] = (Type) parameterTypeList[i];
			}
			MethodInfo mi =
				_providerClass.GetMethod(methodName, parameterTypes);
			if ( mi==null ) 
			{
				throw new ArgumentException(String.Format(
					"Cannot find method named {0}",
					methodName
					));
			}
			ilg.EmitCall(OpCodes.Call, mi, null);
		}
		/// <summary>
		/// Emit IL ldarg
		/// </summary>
		/// <param name="ilg"><see cref="ILGenerator"/> of the method
		/// on the dynamically generated type</param>
		/// <param name="argIndex">index of argument to put on
		/// the stack</param>
		protected static void EmitLdarg( ILGenerator ilg, int argIndex ) 
		{
			if ( argIndex>0xff ) 
			{
				ilg.Emit(OpCodes.Ldarg, (short) argIndex);
				return;
			}
			switch (argIndex) 
			{
				case 0:
					ilg.Emit(OpCodes.Ldarg_0);
					break;
				case 1:
					ilg.Emit(OpCodes.Ldarg_1);
					break;
				case 2:
					ilg.Emit(OpCodes.Ldarg_2);
					break;
				case 3:
					ilg.Emit(OpCodes.Ldarg_3);
					break;
				default:
					ilg.Emit(OpCodes.Ldarg_S, (sbyte) argIndex);
					break;
			}
		}
	}
}
