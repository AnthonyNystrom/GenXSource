/* -----------------------------------------------
 * TypeAttribute.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Types
{
	/// <summary>
	/// Apply this attribute to the class that is intended to act like a type.
	/// This class should be derived from the <see cref="NodeBase"/> class (directly or indirectly).
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
	internal sealed class TypeAttribute : Attribute
	{
		private String _typeName;

		public String TypeName
		{
			get
			{
				return _typeName;
			}
			set
			{
				_typeName = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeAttribute"/> class.
		/// </summary>
		/// <param name="typeName"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="typeName"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="typeName"/> is an empty string.</para>
		/// </exception>
		public TypeAttribute(String typeName)
		{
			if (String.IsNullOrEmpty(typeName))
			{
				throw new ArgumentNullException("typeName");
			}

			_typeName = typeName;
		}
	}
}
