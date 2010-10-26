#region License
// Copyright (c) 2004 Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Collections;
using System.Reflection;
using DotNetMock.Core;

#endregion

namespace DotNetMock.TestFramework
{
	/// <summary>
	/// Creates an implementation of <see cref="ITestFramework"/>
	/// based on settings in the environment.
	/// </summary>
	public class ImplementationFactory
	{
		const string STATIC_IMPLEMENTATION_ASSEMBLY_KEY =
			"DotNetMock_TestingAssembly";
		const string STATIC_IMPLEMENTATION_TYPE_KEY =
			"DotNetMock_TestingComponent";

		const string NUNIT_ASSEMBLY_NAME = "nunit.framework";
		const string MBUNIT_ASSEMBLY_NAME = "MbUnit.Core";
		const string CSUNIT_ASSEMBLY_NAME = "csUnit";

		private IDictionary _env;
		private IDynamicLinker _linker;

		/// <summary>
		/// Create an implementation factory.
		/// </summary>
		/// <param name="env"><see cref="IDictionary"/> of environment
		/// variable name value entries</param>
		/// <param name="linker">provider of reflection services</param>
		public ImplementationFactory(IDictionary env, IDynamicLinker linker)
		{
			_env    = env;
			_linker = linker;
		}
		/// <summary>
		/// Create an appropriate implementation for the given
		/// environment.
		/// </summary>
		/// <returns>a new <see cref="ITestFramework"/></returns>
		public ITestFramework NewImplementation() 
		{
			Type implementationType = getStaticImplementationType();
			if ( implementationType==null ) 
			{
				implementationType = getDynamicImplementationType();
			}
			if ( implementationType==null ) 
			{
				throw new SystemException(
					"Cannot find an appropriate test framework implementation."
					);
			}
			ITestFramework implementation = (ITestFramework)
				_linker.CreateInstance(implementationType);
			return implementation;
		}
		private Type getStaticImplementationType() 
		{
			string assemblyName = (string)
				_env[STATIC_IMPLEMENTATION_ASSEMBLY_KEY];
			if ( (assemblyName==null) || (assemblyName.Equals(String.Empty)) ) 
			{
				return null;
			}
			string typeName = (string)
				_env[STATIC_IMPLEMENTATION_TYPE_KEY];
			if ( (typeName==null) || (typeName.Equals(String.Empty)) ) 
			{
				return null;
			}
			Assembly assembly =
				_linker.LoadAssembly(assemblyName);
			Type type =
				_linker.GetType(typeName, assembly);
			return type;
		}
		private Type getDynamicImplementationType() 
		{
			StubClassMaker classMaker = new StubClassMaker();
			IStubMaker stubMaker = null;
			Assembly assembly = null;
			if ( (assembly=_linker.LoadAssembly(NUNIT_ASSEMBLY_NAME))!=null ) 
			{
				stubMaker = new NUnitStubMaker(assembly, _linker);
			}
			else if ( (assembly=_linker.LoadAssembly(MBUNIT_ASSEMBLY_NAME))!=null ) 
			{
				stubMaker = new MbUnitStubMaker(assembly, _linker);
			}
			else if ( (assembly=_linker.LoadAssembly(CSUNIT_ASSEMBLY_NAME))!=null ) 
			{
				stubMaker = new csUnitStubMaker(assembly, _linker);
			} 
			else if ( (assembly=_linker.LoadAssemblyWithPartialName(NUNIT_ASSEMBLY_NAME))!=null ) 
			{
				stubMaker = new NUnitStubMaker(assembly, _linker);
			}
			else if ( (assembly=_linker.LoadAssemblyWithPartialName(MBUNIT_ASSEMBLY_NAME))!=null ) 
			{
				stubMaker = new MbUnitStubMaker(assembly, _linker);
			}
			else if ( (assembly=_linker.LoadAssemblyWithPartialName(CSUNIT_ASSEMBLY_NAME))!=null ) 
			{
				stubMaker = new csUnitStubMaker(assembly, _linker);
			} 
			else 
			{
				return null;
			}
			Type stubClass = classMaker.MakeStubClass(
				typeof(ITestFramework),
				stubMaker
				);
			return stubClass;
		}
	}
}
