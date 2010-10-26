#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> that compares the input value against two <see cref="IPredicate"/> instances.
	/// </summary>
	public class OrPredicate : AbstractPredicate
	{
		private IPredicate _originalPredicate1; 
		private IPredicate _originalPredicate2;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="originalPredicate1"><see cref="IPredicate"/> instance 1</param>
		/// <param name="originalPredicate2"><see cref="IPredicate"/> instance 2</param>
		public OrPredicate(IPredicate originalPredicate1, IPredicate originalPredicate2)
		{
			_originalPredicate1 = originalPredicate1;
			_originalPredicate2 = originalPredicate2;
		}
		/// <summary>
		/// Evaluates the input value against the either of the original <see cref="IPredicate"/> values.
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>True if the input value satisfies either of the original <see cref="IPredicate"/> objects, false otherwise.</returns>		
		public override bool Eval(object inputValue)
		{
			return _originalPredicate1.Eval(inputValue) || _originalPredicate2.Eval(inputValue);
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"({0}) or ({1})",
				_originalPredicate1.ExpressionAsText(name),
				_originalPredicate2.ExpressionAsText(name)
				);
		}
	}
}
