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
	/// <see cref="IPredicate"/> representing a expected value, but also an acceptable delta for the input value
	/// </summary>
	public class IsCloseTo : AbstractPredicate 
	{
		private double _expected;
		private double _error;
		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="expected">Expected value</param>
		/// <param name="error">Acceptable delta for input value.</param>
		public IsCloseTo(double expected, double error)
		{
			_expected = expected;
			_error = error;
		}
		/// <summary>
		/// Evalutates input value against any available delta
		/// </summary>
		/// <param name="currentValue">Input value to evaluate</param>
		/// <returns>True if the input value is within acceptable range.</returns>
		public override bool Eval(object currentValue)
		{
			try
			{
				double actual = Convert.ToDouble(currentValue, CultureInfo.CurrentCulture);
				return Math.Abs(actual - _expected) <= _error;
			}
			catch (FormatException)
			{
				return false;
			}
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} is within {1} of {2}",
				name,
				_error,
				_expected
				);
		}
	}
}
