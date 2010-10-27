using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Collections;
using System.Threading;

using System.Diagnostics;

namespace Genetibase.Debug
{
	internal class GenerateEventAssembly
	{
		Type parent = typeof(ControlEvent);
	
		Hashtable typeCollection = new Hashtable();

		AssemblyName assemblyName;
		AssemblyBuilder newAssembly;
		ModuleBuilder newModule;

		private GenerateEventAssembly() 
		{
			StartAssembly();
		}

//		string filename = "james.dll";

		private void StartAssembly() 
		{
			// Create an assembly name
			assemblyName = new AssemblyName( );
			assemblyName.Name = "James";
 
			// Create a new assembly with one module.
			// Change to AssemblyBuilderAccess.RunAndSave if you want to save the
			// assembly to disk for inspection
			newAssembly =
				Thread.GetDomain( ).DefineDynamicAssembly(
				assemblyName, AssemblyBuilderAccess.Run);

			// This is the replacement to allow persisting the assembly to disk
			//newModule =
			//		newAssembly.DefineDynamicModule("James2", filename);

			newModule =
				newAssembly.DefineDynamicModule("James2");
		}

		// the private method which emits the assembly
		// using op codes
		private Type GenerateEventConsumerType(Type eventHandlerType)
		{
			string typeName = "DerivedControlEvent" + eventHandlerType.Name;

			// Define a public class in the assembly with the calculated
			// name, above
			TypeBuilder myType =
				newModule.DefineType(
				typeName, TypeAttributes.Public, parent);
 
			// Define a method on the type to call. Pass an
			// array that defines the types of the parameters,
			// the type of the return type, the name of the 
			// method, and the method attributes.
			Type[] paramTypes = new Type[2];

			paramTypes[0] = typeof(object);
			paramTypes[1] = eventHandlerType;

			Type returnType = typeof(void);

			MethodBuilder simpleMethod =
				myType.DefineMethod(
				"HandleEvent",
				MethodAttributes.Public | 
				MethodAttributes.HideBySig,
				returnType,
				paramTypes);
 
			// Get an ILGenerator. This is used
			// to emit the IL that you want.
			ILGenerator generator = 
				simpleMethod.GetILGenerator( );
 
			// Emit the IL that you'd get if you 
			// compiled the code example 
			// and then ran ILDasm on the output.
 
			generator.Emit(OpCodes.Ldarg_0);
			generator.Emit(OpCodes.Ldarg_1);
			generator.Emit(OpCodes.Ldarg_2);

			MethodInfo miGenericEventHandler;
			miGenericEventHandler = parent.GetMethod("GenericHandleEvent");
			
			generator.Emit(OpCodes.Call, miGenericEventHandler);

			// return the value
			generator.Emit(OpCodes.Ret);
 
 
			// Create the type.
			Type result = myType.CreateType( );

			//  for debugging purposes, save out the assembly. Remember to change
			//  the call that creates newAssembly above
//			newAssembly.Save(filename);

			return result;
		}
 
		static GenerateEventAssembly instance;
		public static GenerateEventAssembly Instance 
		{
			get 
			{
				if (instance == null) 
				{
					instance = new GenerateEventAssembly();
				}

				return instance;
			}
		}

		// emit the assembly, create an instance 
		// and get the interface
		public ControlEvent GetEventConsumerType(Type eventHandlerType)
		{
			Type eventConsumerType;

			if (typeCollection.Contains(eventHandlerType)) 
			{
				
				eventConsumerType = (Type) typeCollection[eventHandlerType];
			} 
			else 
			{
				eventConsumerType = GenerateEventConsumerType(eventHandlerType);

				typeCollection[eventHandlerType] = eventConsumerType;
			}

			ControlEvent ev = (ControlEvent) Activator.CreateInstance(eventConsumerType);

			return ev;
		}
 
	}

}
