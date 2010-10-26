#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> that is used to wrap other <see cref="IPredicate"/> objects, and invert their results.
	/// </summary>
	public class NotPredicate : AbstractPredicate 
	{
		private IPredicate _wrappedPredicate;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="wrappedPredicate">original <see cref="IPredicate"/> to wrap.</param>
		public NotPredicate(IPredicate wrappedPredicate)
		{
			_wrappedPredicate = wrappedPredicate;
		}
		/// <summary>
		/// Evaluates the input value against the original value.
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>The opposite ( ! ) of the original <see cref="IPredicate"/>s evaluation.</returns>	
		public override bool Eval(object inputValue) 
		{
			return !_wrappedPredicate.Eval(inputValue);
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"not ({0})",
				_wrappedPredicate.ExpressionAsText(name)
				);
		}
	}
}
