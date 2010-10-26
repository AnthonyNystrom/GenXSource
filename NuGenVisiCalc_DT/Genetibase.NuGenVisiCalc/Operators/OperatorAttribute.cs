/* -----------------------------------------------
 * OperatorNameAttribute.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.NuGenVisiCalc.Operators
{
	/// <summary>
	/// Apply this attribute to the method that implements an operator evaluation logic. It is expected
	/// to take some input parameters and return the result of evaluation (i.e. should not be void). The
	/// method should be public and not static.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, Inherited=false, AllowMultiple=false)]
	internal sealed class OperatorAttribute : Attribute
	{
		private String _name;

		/// <summary>
		/// Read-only.
		/// </summary>
		public String Name
		{
			get
			{
				return _name;
			}
		}

		private Int32 _precedence;

		/// <summary>
		/// Read-only.
		/// </summary>
		public Int32 Precedence
		{
			get
			{
				return _precedence;
			}
		}

		private PrimitiveOperator _primitiveOperator;

		/// <summary>
		/// Read-only.
		/// </summary>
		public PrimitiveOperator PrimitiveOperator
		{
			get
			{
				return _primitiveOperator;
			}
		}

		private String _stringRepresentation;

		/// <summary>
		/// Read-only.
		/// </summary>
		public String StringRepresentation
		{
			get
			{
				return _stringRepresentation;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OperatorAttribute"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="name"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="name"/> is an empty String.</para>
		/// -or-
		/// <para><paramref name="stringRepresentation"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="stringRepresentation"/> is an empty String.</para>
		/// </exception>
		public OperatorAttribute(String name, String stringRepresentation)
			: this(name, stringRepresentation, 0, PrimitiveOperator.No)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OperatorAttribute"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="name"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="name"/> is an empty String.</para>
		/// -or-
		/// <para><paramref name="stringRepresentation"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="stringRepresentation"/> is an empty String.</para>
		/// </exception>
		public OperatorAttribute(String name, String stringRepresentation, Int32 precedence, PrimitiveOperator primitiveOperator)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}

			if (String.IsNullOrEmpty(stringRepresentation))
			{
				throw new ArgumentNullException("stringRepresentation");
			}

			_name = name;
			_stringRepresentation = stringRepresentation;
			_precedence = precedence;
			_primitiveOperator = primitiveOperator;
		}
	}
}
