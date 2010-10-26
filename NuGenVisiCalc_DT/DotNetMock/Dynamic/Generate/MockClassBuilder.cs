#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using DotNetMock.Dynamic;
#endregion

namespace DotNetMock.Dynamic.Generate
{
	/// <summary>
	/// Builds mock classes dynamically.
	/// </summary>
	/// <author>Griffin Caprio</author>
	/// <author>Choy Rim</author>
	public class MockClassBuilder 
	{
		private const string HANDLER_FIELD_NAME = "_mockedCallHandler";
		private static readonly Type HANDLER_TYPE = typeof(IMockedCallHandler);
		private static readonly MethodInfo CALL_HANDLING_METHOD =
			HANDLER_TYPE.GetMethod(
			"Call",
			new Type[] { typeof(MethodInfo), typeof(object[]) }
			);

		TypeBuilder _typeBuilder;
		FieldBuilder _handlerFieldBuilder;
		Type _mockClass = null;

		/// <summary>
		/// Create instance of mock class builder.
		/// </summary>
		/// <param name="moduleBuilder"><see cref="ModuleBuilder"/> that
		/// we'll define new type in</param>
		/// <param name="mockClassName">name of mock class that we are
		/// making</param>
		/// <param name="superClass">base class that we are extending
		/// to implement our mock class</param>
		/// <param name="interfaces">interfaces that our mock class
		/// will (declare that we) implement</param>
		public MockClassBuilder(
			ModuleBuilder moduleBuilder,
			string mockClassName, Type superClass, Type[] interfaces
			) 
		{
			_typeBuilder = moduleBuilder.DefineType(
				mockClassName,
				TypeAttributes.Public,
				superClass, interfaces
				);
			_handlerFieldBuilder = _typeBuilder.DefineField(
				HANDLER_FIELD_NAME,
				HANDLER_TYPE,
				FieldAttributes.Public
				);
		}
		/// <summary>
		/// The mock class we've built.
		/// </summary>
		public Type MockClass 
		{
			get {
				if ( ! IsCompiled ) 
				{
					throw new InvalidOperationException(
						"Must compile before accessing the MockClass"
						);
				}
				return _mockClass;
			}
		}
		/// <summary>
		/// The field that holds our <see cref="IMockedCallHandler"/>
		/// which will receive all calls to this type.
		/// </summary>
		public FieldInfo HandlerField 
		{
			get 
			{
				if ( ! IsCompiled ) 
				{
					throw new InvalidOperationException(
						"Must compile before accessing the HandlerField"
						);
				}
				return _mockClass.GetField(HANDLER_FIELD_NAME);
			}
		}
		/// <summary>
		/// Has this class been compiled into a usable type?
		/// </summary>
		public bool IsCompiled 
		{
			get { return _mockClass!=null; }
		}
		/// <summary>
		/// Compile the implemented methods so far into a real
		/// type.
		/// </summary>
		public void Compile() 
		{
			if ( IsCompiled ) 
			{
				throw new InvalidOperationException(
					"Cannot compile class more than once."
					);
			}	
			_mockClass = _typeBuilder.CreateType();
		}
		/// <summary>
		/// Define a mock implementation method on our mock class given
		/// a <see cref="MethodInfo"/>.
		/// </summary>
		/// <param name="mi"><see cref="MethodInfo"/> describing
		/// the method we must mock</param>
		public void ImplementMockedMethod(MethodInfo mi) 
		{
			ImplementMockedMethod(mi.Name, mi.ReturnType, getParameterTypes(mi), mi);
		}
		/// <summary>
		/// Define a mock implementation of a method.
		/// </summary>
		/// <param name="methodName">name of method to mock</param>
		/// <param name="returnType">return <see cref="Type"/> of method
		/// to mock</param>
		/// <param name="parameterTypes">array of <see cref="Type"/>s in
		/// the method signature</param>
		public void ImplementMockedMethod(string methodName, Type returnType, Type[] parameterTypes )
		{
			ImplementMockedMethod(methodName, returnType, parameterTypes, null);
		}
		/// <summary>
		/// Define a mock implementation of a method.
		/// </summary>
		/// <param name="methodName">name of method to mock</param>
		/// <param name="returnType">return <see cref="Type"/> of method
		/// to mock</param>
		/// <param name="parameterTypes">array of <see cref="Type"/>s in
		/// the method signature</param>
		/// <param name="mi">if not null, used to get extra parameter information</param>
		public void ImplementMockedMethod(string methodName, Type returnType, Type[] parameterTypes, MethodInfo mi)
		{
			if ( IsCompiled ) 
			{
				throw new InvalidOperationException(
					"Cannot add methods after class has been compiled."
					);
			}
			MethodAttributes attributes = MethodAttributes.Public |
				MethodAttributes.Virtual;
			if ( mi!=null ) 
			{
				// only overlay SpecialName attribute
				attributes |= mi.Attributes & (MethodAttributes.SpecialName);
			}
			MethodBuilder methodBuilder = _typeBuilder.DefineMethod(
				methodName,
				attributes,
				returnType,
				parameterTypes
				);
			if ( mi!=null ) 
			{
				ParameterInfo[] pis = mi.GetParameters();
				for (int i = 0; i<pis.Length; ++i)
				{
					ParameterInfo pi = pis[i];
					methodBuilder.DefineParameter(
						i+1,
						pi.Attributes,
						pi.Name
						);
				}
			}
			ILGenerator il = methodBuilder.GetILGenerator();

			il.DeclareLocal(typeof(object[]));

			il.Emit(OpCodes.Ldarg_0);
			// stack: this
			il.Emit(OpCodes.Ldfld, _handlerFieldBuilder);
			// stack: this handler
			// emit call to get current method
			il.EmitCall(
				OpCodes.Call,
				typeof(MethodBase).GetMethod("GetCurrentMethod", new Type[0]),
				null
				);
			// stack: this handler methodinfo
			// assert parameterTypes.Length<128
			il.Emit(OpCodes.Ldc_I4_S, (sbyte) parameterTypes.Length);
			il.Emit(OpCodes.Newarr, typeof(object));
			il.Emit(OpCodes.Dup);
			// store args array in local var
			il.Emit(OpCodes.Stloc_0);
			// stack: this handler methodinfo args[]

			// fill args array with arguments of call
			if (parameterTypes.Length > 0) 
			{
				for(int i = 0; i < parameterTypes.Length; i++)
				{
					Type parameterType = parameterTypes[i];
					Type elementType = parameterType;
					// push array ref onto stack
					il.Emit(OpCodes.Ldloc_0);
					// push array index onto stack
					il.Emit(OpCodes.Ldc_I4_S, (sbyte) i);
					// load corresponding argument
					il.Emit(OpCodes.Ldarg_S, (sbyte) (i + 1));
					// dereference if necessary
					if ( parameterType.IsByRef ) 
					{
						elementType = parameterType.GetElementType();
						ILUtils.EmitTypedLdind(il, elementType);
					}
					if ( elementType.IsValueType )
					{
						il.Emit(OpCodes.Box, elementType);
					}
					il.Emit(OpCodes.Stelem_Ref);
				}
			}

			il.EmitCall(OpCodes.Callvirt, CALL_HANDLING_METHOD, null);

			if (returnType == typeof(void)) 
			{
				il.Emit(OpCodes.Pop);
			}
			else 
			{
				if (returnType.IsValueType) 
				{
					il.Emit(OpCodes.Unbox, returnType);
					// load boxed value from heap
					ILUtils.EmitTypedLdind(il, returnType);
				}
			}
			// load out/ref parameter values
			if (parameterTypes.Length > 0) 
			{
				for(int i = 0; i < parameterTypes.Length; i++)
				{
					Type parameterType = parameterTypes[i];
					if ( parameterType.IsByRef ) 
					{
						Type elementType = parameterType.GetElementType();
						// address of byref arg
						il.Emit(OpCodes.Ldarg_S, (sbyte) (i+1));
						// fetch value from args array
						il.Emit(OpCodes.Ldloc_0);
						il.Emit(OpCodes.Ldc_I4_S, (sbyte) i);
						il.Emit(OpCodes.Ldelem_Ref);
						// unbox if necessary
						if ( elementType.IsValueType )
						{
							il.Emit(OpCodes.Unbox, elementType);
							ILUtils.EmitTypedLdind(il, elementType);
						}
						// store indirectly into ref arg
						ILUtils.EmitTypedStind(il, elementType);
					}
				}
			}

			il.Emit(OpCodes.Ret);
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
	}	
}
