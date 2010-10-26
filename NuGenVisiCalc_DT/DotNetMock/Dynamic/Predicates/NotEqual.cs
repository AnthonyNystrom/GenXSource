#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;

using DotNetMock.Util;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> that verifies the input value is not equal to the original value.
	/// </summary>
	public class NotEqual : AbstractPredicate
	{
		private IPredicate p;
		object _rhs;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="compare">original value to compare against</param>
		public NotEqual(object compare)
		{
			_rhs = compare;
			p = new NotPredicate(new IsEqual(compare));
		}
		/// <summary>
		/// Evaluates the input value against the original value.
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>True if the input value does not equal the original value, false otherwise.</returns>
		public override bool Eval(object inputValue)
		{
			return p.Eval(inputValue);
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} is not equal to {1}",
				name,
				StringUtils.FormatScalar(_rhs)
				);
		}
	}
}
