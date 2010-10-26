/* -----------------------------------------------
 * NuGenFunction.cs
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
	public abstract class NuGenFunction : NuGenFormulaElement
	{
		private NuGenFormulaElement _element;

		/// <summary>
		/// </summary>
		protected NuGenFormulaElement Element
		{
			get
			{
				return _element;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFunction"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="element"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenFunction(NuGenFormulaElement element)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			_element = element;
		}
	}
}
