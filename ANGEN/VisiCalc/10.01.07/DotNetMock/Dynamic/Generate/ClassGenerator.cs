#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion

#region Imports
using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
#endregion

namespace DotNetMock.Dynamic.Generate
{
	/// <summary>
	/// Dynamic Mock object generation engine.  Generates the dynamic proxies for dynamic mock objects.
	/// </summary>
	/// <author>Griffin Caprio</author>
	/// <author>Choy Rim</author>
	/// <author>Roman V. Gavrilov</author>
	public class ClassGenerator
	{
		private AssemblyBuilder _assemblyBuilder;
		private ModuleBuilder _moduleBuilder;
		private string _assemblyFilename;

		#region Constructors
		/// <summary>
		/// Create default instance of mock object generator.
		/// </summary>
		public ClassGenerator( )
		{
			_assemblyBuilder = newAssemblyBuilder( AssemblyBuilderAccess.Run );
			_moduleBuilder = _assemblyBuilder.DefineDynamicModule( "MockModule" );
		}
		/// <summary>
		/// Create mock object generator that can save.
		/// </summary>
		/// <param name="filename">filename to save to</param>
		public ClassGenerator( string filename )
		{
			_assemblyBuilder = newAssemblyBuilder( AssemblyBuilderAccess.RunAndSave );
			_moduleBuilder = _assemblyBuilder.DefineDynamicModule( "MockModule", filename );
			_assemblyFilename = filename;
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Generates a mock object for the specified type.
		/// </summary>
		/// <param name="type">type to generate proxy for.</param>
		/// <param name="handler"><see cref="IMockedCallHandler"/> which
		/// will handle all calls to the generated mock object.</param>
		/// <returns>proxy mock object for input type.</returns>
		public object Generate( Type type, IMockedCallHandler handler )
		{
			string mockClassName = "Mock" + type.Name;
			Type superClass;
			Type[] interfaces;
			determineSuperClassAndInterfaces(
				type,
				out superClass, out interfaces
				);
			MockClassBuilder classBuilder = new MockClassBuilder(
				_moduleBuilder,
				mockClassName,
				superClass,
				interfaces
				);
			IList methods = getMethods( type );
			foreach ( MethodInfo mi in methods )
			{
				classBuilder.ImplementMockedMethod( mi );
			}
			return compileAndGenerateMock( classBuilder, handler );
		}
		/// <summary>
		/// Generates a mock object with an interface defined by an array of method signatures.
		/// </summary>
		/// <param name="typeName">Name of the type to ganerate.</param>
		/// <param name="methodSignatures">Array of method signatures of methods to be implemented.</param>
		/// <param name="handler">Mocked calls handler.</param>
		/// <returns>Instance of generated class.</returns>
		public object Generate( string typeName, MethodSignature[] methodSignatures, IMockedCallHandler handler )
		{
			MockClassBuilder classBuilder = new MockClassBuilder(
				_moduleBuilder,
				typeName,
				typeof ( object ),
				Type.EmptyTypes
				);
			foreach ( MethodSignature ms in methodSignatures )
			{
				classBuilder.ImplementMockedMethod( ms.MethodName, ms.ReturnType, ms.ParamTypes );
			}
			return compileAndGenerateMock( classBuilder, handler );
		}
		private object compileAndGenerateMock( MockClassBuilder classBuilder, IMockedCallHandler handler )
		{
			// create type
			classBuilder.Compile( );
			object newMockObject =
				Activator.CreateInstance( classBuilder.MockClass );
			// set handler field
			classBuilder.HandlerField.SetValue( newMockObject, handler );
			// save if necessary
			if ( _assemblyFilename != null )
			{
				_assemblyBuilder.Save( _assemblyFilename );
			}
			return newMockObject;
		}
		#endregion

		#region Private Methods
		private IList getMethods( Type type )
		{
			IList methods;
			if ( type.IsInterface )
			{
				ArrayList list = new ArrayList( );
				getMethodsForInterface( type, list );
				methods = list;
			}
			else
			{
				methods = type.GetMethods( );
			}
			return methods;
		}
		private void getMethodsForInterface( Type type, ArrayList list )
		{
			list.AddRange( type.GetMethods( ) );
			foreach ( Type interfaceType in type.GetInterfaces( ) )
			{
				getMethodsForInterface( interfaceType, list );
			}
		}
		private static void determineSuperClassAndInterfaces( Type targetType, out Type superClass, out Type[] interfaces )
		{
			if ( targetType.IsInterface )
			{
				superClass = null;
				interfaces = new Type[] {targetType};
			}
			else
			{
				superClass = targetType;
				interfaces = new Type[0];
			}
		}
		private static AssemblyBuilder newAssemblyBuilder( AssemblyBuilderAccess access )
		{
			AppDomain appDomain = AppDomain.CurrentDomain;
			AssemblyName assemblyName = new AssemblyName( );
			assemblyName.Name = "DynamicMockAssembly";
			return appDomain.DefineDynamicAssembly( assemblyName, access );
		}
		#endregion

	}
}