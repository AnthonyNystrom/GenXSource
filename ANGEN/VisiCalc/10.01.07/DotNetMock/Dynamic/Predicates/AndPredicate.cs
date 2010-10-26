#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// A <see cref="IPredicate"/> that expects a value to satisfy two predicates.
	/// </summary>
	public class AndPredicate : AbstractPredicate
	{
		private IPredicate _lhs, _rhs;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="lhs">first <see cref="IPredicate"/></param>
		/// <param name="rhs">first <see cref="IPredicate"/></param>
		public AndPredicate(IPredicate lhs, IPredicate rhs) 
		{
			_lhs = lhs;
			_rhs = rhs;
		}
		/// <summary>
		/// Evaluates the input value against both <see cref="IPredicate"/>s
		/// </summary>
		/// <param name="inputValue">Value to evaluate</param>
		/// <returns>True if the input value satisfies both <see cref="IPredicate"/> instances</returns>
		public override bool Eval(object inputValue) 
		{
			return _lhs.Eval(inputValue) && _rhs.Eval(inputValue);
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"({0}) and ({1})",
				_lhs.ExpressionAsText(name),
				_rhs.ExpressionAsText(name)
				);
		}

	}
}
