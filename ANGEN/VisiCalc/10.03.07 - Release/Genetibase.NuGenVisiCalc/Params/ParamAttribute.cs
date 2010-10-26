/* -----------------------------------------------
 * ParamAttribute.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Params
{
	/// <summary>
	/// Apply this attribute to the class that is inteded to act like a parameter.
	/// This class should be derived from from the <see cref="NodeBase"/> class (directly or indirectly).
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	internal sealed class ParamAttribute : Attribute
	{
		private String _paramName;

		public String ParamName
		{
			get
			{
				return _paramName;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ParamAttribute"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="paramName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="paramName"/> is an empty string.</para>
		/// </exception>
		public ParamAttribute(String paramName)
		{
			if (String.IsNullOrEmpty(paramName))
			{
				throw new ArgumentNullException("paramName");
			}

			_paramName = paramName;
		}
	}
}
