#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Text;

using DotNetMock.Util;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> comparing string values that ignore whitespace.
	/// </summary>
	public class IsEqualIgnoreWhiteSpace : AbstractPredicate
	{
		private IPredicate _originalPredicate;
		private string _compare;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="compare">original value to compare against</param>
		public IsEqualIgnoreWhiteSpace(object compare)
		{
			_compare = compare.ToString();
			_originalPredicate = new IsEqual(StripSpace(compare.ToString()));
		}
		/// <summary>
		/// Evaluates the inputValue against the original, ignoring any white space encountered.
		/// </summary>
		/// <param name="inputValue">input value to compare</param>
		/// <returns>True if the input value equals the original value, false otherwise</returns>
		public override bool Eval(object inputValue)
		{
			return _originalPredicate.Eval(StripSpace(inputValue.ToString()));
		}
		/// <summary>
		/// Strips any whitespace from the input string
		/// </summary>
		/// <param name="inputString">input string</param>
		/// <returns>input string, with whitespace removed</returns>
		public static string StripSpace(string inputString)
		{
			StringBuilder result = new StringBuilder();
			bool lastWasSpace = true;
			foreach(char c in inputString)
			{
				if (Char.IsWhiteSpace(c))
				{
					if (!lastWasSpace)
					{
						result.Append(' ');
					}
					lastWasSpace = true;					
				}
				else
				{
					result.Append(c);
					lastWasSpace = false;					
				}
			}
			return result.ToString().Trim();
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} equals {1} (ignore whitespace)",
				name,
				StringUtils.FormatScalar(_compare)
				);
		}
	}
}
