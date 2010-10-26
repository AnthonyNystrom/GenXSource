#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> that indicates that the input value should be null.
	/// </summary>
	public class IsNull : AbstractPredicate
	{
		/// <summary>
		/// Evaluates the input value against null
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>True if the input value is null, false otherwise.</returns>
		public override bool Eval(object inputValue)
		{
			return inputValue == null;
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} is null",
				name
				);
		}
	}
}
