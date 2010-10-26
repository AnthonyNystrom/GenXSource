#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> that verifies that the input value is null. Opposite of <see cref="IsNull"/>. 
	/// </summary>
	public class NotNull : AbstractPredicate 
	{
		/// <summary>
		/// Evaluates the input value against the original value.
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>True if the input value is not null, false otherwise.</returns>
		public override bool Eval(object inputValue)
		{
			return inputValue != null;
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} is not null",
				name
				);
		}
	}
}
