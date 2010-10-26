#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
// until we figure out what to put here
#endregion
#region Imports
using System;
using System.Collections;
using System.Reflection.Emit;
#endregion

namespace DotNetMock.Dynamic.Generate
{
	/// <summary>
	/// Utility functions for emitting IL.
	/// </summary>
	public class ILUtils
	{
		/// <summary>
		/// Return appropriate ldind <see cref="OpCode"/> for type
		/// referenced by address on top of stack (of emitted code).
		/// </summary>
		/// <param name="referencedType">type referenced by address
		/// on the top of the stack (of emitted code)</param>
		/// <returns>appropriate ldind <see cref="OpCode"/> that will
		/// load the stack with value in the address that is expected
		/// to be on the stack</returns>
		/// <remarks>
		/// If <see cref="OpCodes.Ldobj"/> is returned, then you must
		/// emit the <see cref="ValueType"/> with it.
		/// </remarks>
		public static OpCode GetLdindOpCodeForType(Type referencedType) 
		{
			// short-circuit reference types
			if ( ! referencedType.IsValueType ) 
			{
				return OpCodes.Ldind_Ref;
			}
			// must be a value type
			// check among primitive types
			// ensure table is initialized
			if (ldindOpCodes == null) 
			{
				ldindOpCodes = new Hashtable();
				ldindOpCodes[typeof(sbyte)]  = OpCodes.Ldind_I1;
				ldindOpCodes[typeof(short)]  = OpCodes.Ldind_I2;
				ldindOpCodes[typeof(int)]    = OpCodes.Ldind_I4;
				ldindOpCodes[typeof(long)]   = OpCodes.Ldind_I8;
				ldindOpCodes[typeof(byte)]   = OpCodes.Ldind_U1;
				ldindOpCodes[typeof(ushort)] = OpCodes.Ldind_U2;
				ldindOpCodes[typeof(uint)]   = OpCodes.Ldind_U4;
				ldindOpCodes[typeof(ulong)]  = OpCodes.Ldind_I8;
				ldindOpCodes[typeof(float)]  = OpCodes.Ldind_R4;
				ldindOpCodes[typeof(double)] = OpCodes.Ldind_R8;
				ldindOpCodes[typeof(char)]   = OpCodes.Ldind_U2;
				ldindOpCodes[typeof(bool)]   = OpCodes.Ldind_I1;
			}
			object opCodeObject = ldindOpCodes[referencedType];
			if (opCodeObject != null)
			{
				return (OpCode) opCodeObject;
			}
			// must be non-primitive ValueType
			return OpCodes.Ldobj;
		}

		/// <summary>
		/// Return appropriate stind <see cref="OpCode"/> for type
		/// on top of stack (of emitted code).
		/// </summary>
		/// <param name="referencedType">type 
		/// on the top of the stack (of emitted code)</param>
		/// <returns>appropriate stind <see cref="OpCode"/> that will
		/// store the value on the stack in the address below it</returns>
		/// <remarks>
		/// If <see cref="OpCodes.Stobj"/> is returned, then you must
		/// emit the <see cref="ValueType"/> with it.
		/// </remarks>
		public static OpCode GetStindOpCodeForType(Type referencedType) 
		{
			// short-circuit reference types
			if ( ! referencedType.IsValueType ) 
			{
				return OpCodes.Stind_Ref;
			}
			// must be a value type, first check among primitives
			// ensure primitive opcodes table is initialized
			if (stindOpCodes == null) 
			{
				stindOpCodes = new Hashtable();
				stindOpCodes[typeof(sbyte)]  = OpCodes.Stind_I1;
				stindOpCodes[typeof(short)]  = OpCodes.Stind_I2;
				stindOpCodes[typeof(int)]    = OpCodes.Stind_I4;
				stindOpCodes[typeof(long)]   = OpCodes.Stind_I8;
				stindOpCodes[typeof(byte)]   = OpCodes.Stind_I1;
				stindOpCodes[typeof(ushort)] = OpCodes.Stind_I2;
				stindOpCodes[typeof(uint)]   = OpCodes.Stind_I4;
				stindOpCodes[typeof(ulong)]  = OpCodes.Stind_I8;
				stindOpCodes[typeof(float)]  = OpCodes.Stind_R4;
				stindOpCodes[typeof(double)] = OpCodes.Stind_R8;
				stindOpCodes[typeof(char)]   = OpCodes.Stind_I2;
				stindOpCodes[typeof(bool)]   = OpCodes.Stind_I1;
			}
			object opCodeObject = stindOpCodes[referencedType];
			if (opCodeObject != null)
			{
				return (OpCode) opCodeObject;
			}
			// must be non-primitive ValueType
			return OpCodes.Stobj;
		}

		/// <summary>
		/// Emit the appropriate ldind instruction(s) to load the
		/// specified type from the address on the stack.
		/// </summary>
		/// <param name="il">generator to emit code to</param>
		/// <param name="referencedType">the type in the address on
		/// the top of the stack</param>
		public static void EmitTypedLdind(ILGenerator il, Type referencedType) 
		{
			OpCode opCode = GetLdindOpCodeForType(referencedType);
			if ( opCode.Equals(OpCodes.Ldobj) ) 
			{
				il.Emit(opCode, referencedType);
			}
			else 
			{
				il.Emit(opCode);
			}
		}

		/// <summary>
		/// Emit the appropriate stind instruction(s) to store the
		/// specified type from the top of the stack to the address
		/// bwlow it.
		/// </summary>
		/// <param name="il">generator to emit code to</param>
		/// <param name="referencedType">type of the value on the top of
		/// the stack</param>
		public static void EmitTypedStind(ILGenerator il, Type referencedType) 
		{
			OpCode opCode = GetStindOpCodeForType(referencedType);
			if ( opCode.Equals(OpCodes.Stobj) ) 
			{
				il.Emit(opCode, referencedType);
			}
			else 
			{
				il.Emit(opCode);
			}
		}

		private static IDictionary ldindOpCodes = null;
		private static IDictionary stindOpCodes = null;
	}
}
