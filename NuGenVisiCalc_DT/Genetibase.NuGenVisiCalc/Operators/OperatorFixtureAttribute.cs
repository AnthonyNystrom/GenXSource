/* -----------------------------------------------
 * OperatorFixtureAttribute.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Operators
{
	/// <summary>
	/// Indicates a class which may contain evaluate methods for operators implementation. The class should not be neither static nor abstract.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=false)]
	internal sealed class OperatorFixtureAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OperatorFixtureAttribute"/> class.
		/// </summary>
		public OperatorFixtureAttribute()
		{
		}
	}
}
