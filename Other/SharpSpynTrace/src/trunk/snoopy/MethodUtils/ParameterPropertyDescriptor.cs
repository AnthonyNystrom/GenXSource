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
	/// Summary description for ParameterPropertyDescriptor.
	/// </summary>
	internal class ParameterPropertyDescriptor : PropertyDescriptor
	{
		private readonly ParameterInfo param;
		private readonly MethodPropertyDescriptor methodDesc;
		private object localValue;

		public ParameterPropertyDescriptor(MethodPropertyDescriptor methodDesc, ParameterInfo param)
			: base ( param.Name + " (" + param.ParameterType.Name + ")", null )
		{
			this.methodDesc = methodDesc;
			this.param = param;
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
			localValue = value;
			methodDesc.Invoke ();
		}
		public override bool ShouldSerializeValue(object component)
		{
			return false;
		}
		public override object GetValue(object component)
		{
			return localValue;
		}

		public override bool IsReadOnly
		{
			get
			{
				return false;
			}
		}



		public override Type PropertyType
		{
			get
			{
				return param.ParameterType;
			}
		}
		public override Type ComponentType
		{
			get
			{
				return param.ParameterType;
			}
		}

		public override TypeConverter Converter
		{
			get
			{
				return base.Converter;
			}
		}

		protected override void FillAttributes(System.Collections.IList attributeList)
		{
			base.FillAttributes (attributeList);
			attributeList.Add( new RefreshPropertiesAttribute(RefreshProperties.Repaint) );
		}

	}
}
