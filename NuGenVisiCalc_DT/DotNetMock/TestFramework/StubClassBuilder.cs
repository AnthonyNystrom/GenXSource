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
	/// Creates stub classes that implement an interface.
	/// </summary>
	/// <author>Choy Rim</author>
	public class StubClassMaker
	{
		AssemblyBuilder _assemblyBuilder = null;
		ModuleBuilder _moduleBuilder = null;

		/// <summary>
		/// Create stub class maker.
		/// </summary>
		public StubClassMaker()
		{
			_assemblyBuilder = newAssemblyBuilder(AssemblyBuilderAccess.Run);
			_moduleBuilder = _assemblyBuilder.DefineDynamicModule("ProviderStub");
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="supportedInterface">interface which the stub
		/// class will support</param>
		/// <param name="stubMaker">object which will generate
		/// method bodies</param>
		/// <returns>new stub class</returns>
		public Type MakeStubClass(Type supportedInterface, IStubMaker stubMaker) 
		{
			if ( ! supportedInterface.IsInterface ) 
			{
				throw new ArgumentException(
					"Can only stub interfaces.",
					"supportedInterface"
					);
			}
			TypeBuilder typeBuilder = _moduleBuilder.DefineType(
				"ProviderStub",
				TypeAttributes.Public,
				null,
				new Type[] { supportedInterface }
				);
			defineAndImplementStubMethods(typeBuilder, supportedInterface, stubMaker);
			Type stubClass = typeBuilder.CreateType();
			return stubClass;
		}
		/// <summary>
		/// Define stub methods through <see cref="IStubMaker"/>
		/// </summary>
		/// <param name="typeBuilder">where to add methods</param>
		/// <param name="supportedInterface">interface which the stub
		/// class will support</param>
		/// <param name="stubMaker">object which will generate
		/// method bodies</param>
		private static void defineAndImplementStubMethods(
			TypeBuilder typeBuilder,
			Type supportedInterface, IStubMaker stubMaker
			) 
		{
			ArrayList methods = new ArrayList();
			getMethodsForInterface(supportedInterface, methods);
			foreach (MethodInfo mi in methods) 
			{
				MethodBuilder methodBuilder = typeBuilder.DefineMethod(
					mi.Name,
					MethodAttributes.Public | MethodAttributes.Virtual,
					mi.ReturnType,
					getParameterTypes(mi)
					);
				ILGenerator ilg = methodBuilder.GetILGenerator();
				stubMaker.ImplementStubMethod(ilg, mi);
				ilg.Emit(OpCodes.Ret);
			}
		}
		/// <summary>
		/// Returns the array of parameters types given a
		/// <see cref="MethodInfo"/>
		/// </summary>
		/// <param name="mi">method we want the parameter types of</param>
		/// <returns>array of parameter types in method signature</returns>
		private static Type[] getParameterTypes(MethodInfo mi) 
		{
			ArrayList types = new ArrayList();
			foreach (ParameterInfo pi in mi.GetParameters()) 
			{
				types.Add(pi.ParameterType);
			}
			return (Type[]) types.ToArray(typeof(Type));
		}
		private static void getMethodsForInterface(Type type, ArrayList list)
		{
			list.AddRange(type.GetMethods());
			foreach (Type interfaceType in type.GetInterfaces())
			{
				getMethodsForInterface(interfaceType, list);
			}
		}
		private static AssemblyBuilder newAssemblyBuilder(AssemblyBuilderAccess access) 
		{
			AppDomain appDomain = AppDomain.CurrentDomain;
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = "ProviderStubAssembly";
			return appDomain.DefineDynamicAssembly( assemblyName, access );
		}
	}
}
