/* -----------------------------------------------
 * NuGenOperator.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.MathX.FormulaInterpreter
{
	/// <summary>
	/// </summary>
	public abstract class NuGenOperator : NuGenFormulaElement
	{
		private NuGenFormulaElement _left;

		/// <summary>
		/// </summary>
		protected NuGenFormulaElement Left
		{
			get
			{
				return _left;
			}
		}

		/// <summary>
		/// Returns the importancy in a complex calculation, e.g. ^ is more important than +.
		/// </summary>
		public abstract int Preference
		{
			get;
		}
		
		private NuGenFormulaElement _right;

		/// <summary>
		/// </summary>
		protected NuGenFormulaElement Right
		{
			get
			{
				return _right;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenOperator"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="left"/> is <see langword="null"/>.</para>
		/// -or-
		/// <para><paramref name="right"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenOperator(NuGenFormulaElement left, NuGenFormulaElement right)
		{
			if (left == null)
			{
				throw new ArgumentNullException("left");
			}

			if (right == null)
			{
				throw new ArgumentNullException("right");
			}

			_left = left;
			_right = right;
		}
	}
}
