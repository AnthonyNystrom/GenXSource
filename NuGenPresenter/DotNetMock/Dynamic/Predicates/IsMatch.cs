#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Text.RegularExpressions;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> that compares the input value against an regular expression pattern.
	/// </summary>
	public class IsMatch : AbstractPredicate 
	{
		private Regex _regex;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="regex">Regular expression object to use.</param>
		public IsMatch(Regex regex)
		{
			_regex = regex;
		}
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="regex">Regular expression pattern to use.</param>
		public IsMatch(String regex) : this(new Regex(regex)) {}
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="regex">Regular expression pattern to use.</param>
		/// <param name="ignoreCase">Flag indicating if the regular expression object should ignore case or not.</param>
		public IsMatch(String regex, bool ignoreCase) : this(new Regex(regex, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None)) {}
		/// <summary>
		/// Evaluates the input value against the provided regular expression 
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>True if the regular expression matches the input value, flase otherwise.</returns>
		public override bool Eval(object inputValue)
		{
			return inputValue == null ? false : _regex.IsMatch(inputValue.ToString());
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} matches /{1}/",
				name,
				_regex
				);
		}
	}
}
