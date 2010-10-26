#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Collections;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// Abstract base class for all predicates.
	/// </summary>
	public abstract class AbstractPredicate : IPredicate
	{
		/// <summary>
		/// Evaluates whether input value satisfies this predicate.
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>true if the predicate was satisfied</returns>
		public abstract bool Eval(object inputValue);
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		/// <param name="name">
		///  name of value/variable to use in the expression text
		/// </param>
		/// <returns>text representation of this predicate</returns>
		public virtual string ExpressionAsText(string name) 
		{
			return "(N/A)";
		}
		/// <summary>
		/// Returns a <see cref="String"/> that represents the
		/// expression evaluated by this <see cref="IPredicate"/>.
		/// </summary>
		/// <returns>
		///  Expression evaluated by this predicate.
		/// </returns>
		public override string ToString()
		{
			return ExpressionAsText("value");
		}
	}
}
