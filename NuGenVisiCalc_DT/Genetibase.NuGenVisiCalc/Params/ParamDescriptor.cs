/* -----------------------------------------------
 * ParamDescriptor.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Genetibase.Shared.Reflection;

namespace Genetibase.NuGenVisiCalc.Params
{
	internal sealed class ParamDescriptor : IDescriptor
	{
		private String _paramName;

		public String ParamName
		{
			get
			{
				return _paramName;
			}
		}

		private Type _paramType;

		public Type ParamType
		{
			get
			{
				return _paramType;
			}
		}

		public NodeBase CreateNode(Canvas canvas)
		{
			return (NodeBase)NuGenActivator.CreateObject(ParamType);
		}

		public override String ToString()
		{
			return ParamName;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ParamDescriptor"/> class.
		/// </summary>
		/// <param name="paramType"></param>
		/// <param name="paramName"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paramType"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="paramName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="paramName"/> is an empty string.</para>
		/// </exception>
		public ParamDescriptor(Type paramType, String paramName)
		{
			if (paramType == null)
			{
				throw new ArgumentNullException("paramType");
			}

			if (String.IsNullOrEmpty(paramName))
			{
				throw new ArgumentNullException("paramName");
			}

			_paramType = paramType;
			_paramName = paramName;
		}
	}
}
