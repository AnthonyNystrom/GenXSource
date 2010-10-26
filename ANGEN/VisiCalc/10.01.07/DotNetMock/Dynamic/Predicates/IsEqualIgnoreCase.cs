#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Globalization;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> comparing strings while ignoring case
	/// </summary>
	public class IsEqualIgnoreCase : AbstractPredicate 
	{
		private IsEqual _isEqual;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="compare">Value to compare against</param>
		public IsEqualIgnoreCase(object compare)
		{
			_isEqual = new IsEqual(compare.ToString().ToLower(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Evaluates input value against original value, ignoring case.
		/// </summary>
		/// <param name="inputValue">Input value</param>
		/// <returns>True if input value equals original value</returns>
		public override bool Eval(object inputValue)
		{
			return _isEqual.Eval(inputValue.ToString().ToLower(CultureInfo.CurrentCulture));
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} (ignore case)",
				_isEqual.ExpressionAsText(name)
				);
		}
	}
}
