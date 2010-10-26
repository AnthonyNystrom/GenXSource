#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
using System;

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> representing any value
	/// </summary>
	public class IsAnything : AbstractPredicate
	{
		/// <summary>
		/// Always evaluates to tue
		/// </summary>
		/// <param name="currentValue">INput value to evaluate</param>
		/// <returns>True</returns>
		public override bool Eval(object currentValue) 
		{
			return true;
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format("{0} is anything", name);
		}
	}
}
