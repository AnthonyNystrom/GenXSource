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
	/// <see cref="IPredicate"/> to verify that the input value is not in the original list of values.  Opposite of <see cref="IsIn"/>.
	/// </summary>
	public class NotIn : AbstractPredicate 
	{
		private IPredicate p;
		private object[] _inList;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="inList">original list of values</param>
		public NotIn(params object[] inList)
		{
			_inList = inList;
			p = new NotPredicate(new IsIn(inList));
		}
		/// <summary>
		/// Evaluates the input value against the original value.
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>True if the input value is not in the original array of values, false otherwise.</returns>
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
				"{0} is not in [{1}]",
				name,
				StringUtils.FormatArray(_inList)
				);
		}
	}
}
