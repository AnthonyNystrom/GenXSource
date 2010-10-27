/* ****************************************************************************
 *  RuntimeObjectEditor
 * 
 * Copyright (c) 2005 Corneliu I. Tusnea
 * 
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the author be held liable for any damages arising from 
 * the use of this software.
 * Permission to use, copy, modify, distribute and sell this software for any 
 * purpose is hereby granted without fee, provided that the above copyright 
 * notice appear in all copies and that both that copyright notice and this 
 * permission notice appear in supporting documentation.
 * 
 * Corneliu I. Tusnea (corneliutusnea@yahoo.com.au)
 * ****************************************************************************/

using System;
using System.ComponentModel;
using System.Reflection;

namespace Genetibase.Debug
{
	/// <summary>
	/// Summary description for ReturnParameterDescriptor.
	/// </summary>
	internal class ReturnParameterDescriptor: PropertyDescriptor
	{
		private readonly MethodPropertyDescriptor method;
		private readonly System.Type returnType;
		private object returnValue;

		public ReturnParameterDescriptor( MethodPropertyDescriptor method )
			: base ( "Return (" + method.MethodInfo.ReturnType.Name +")" , null )
		{
			this.method = method;
			this.returnType = method.MethodInfo.ReturnType;
		}

		public override bool CanResetValue(object component)
		{
			return false;
		}
		public override void ResetValue(object component)
		{

		}
		public override void SetValue(object component, object value)
		{
			returnValue = value;
		}
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
		public override object GetValue(object component)
		{
			return returnValue;
		}

		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}
		public override Type PropertyType
		{
			get
			{
				return returnType;
			}
		}
		public override Type ComponentType
		{
			get
			{
				return returnType;
			}
		}

		public override TypeConverter Converter
		{
			get
			{
				//return base.Converter;
				return method.Converter;
			}
		}

		public object ReturnValue
		{
			get { return returnValue; }
			set { returnValue = value; }
		}
	}
}
