/* -----------------------------------------------
 * NuGenConstantE.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.MathX.FormulaInterpreter
{
	/// <summary>
	/// Represents the constant E = ~2.71.
	/// </summary>
	public class NuGenConstantE : NuGenValue
	{
		/// <summary>
		/// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		/// </returns>
		public override string ToString()
		{
			return "e";
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenConstantE"/> class.
		/// </summary>
		public NuGenConstantE()
			: base(Math.E)
		{
		}
	}
}
