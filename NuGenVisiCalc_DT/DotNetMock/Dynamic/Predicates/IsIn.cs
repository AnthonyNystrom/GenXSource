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
	/// <see cref="IPredicate"/> that looks for an input value in an array of values.
	/// </summary>
	public class IsIn : AbstractPredicate 
	{
		private object[] _inList;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="inList">array of values to use for searching</param>
		public IsIn(params object[] inList)
		{
			if (inList.Length == 1 && inList[0].GetType().IsArray)
			{
				Array arr = (Array)inList[0];
				_inList = new object[arr.Length];
				arr.CopyTo(_inList, 0);
			}
			else
			{
				_inList = inList;
			}
		}
		/// <summary>
		/// Evaluates the input value by looking within the original array list of values for the input value.
		/// </summary>
		/// <param name="inputValue">input value to look for</param>
		/// <returns>True if the input value is found, false otherwise.</returns>
		public override bool Eval(object inputValue)
		{
			foreach (object o in _inList)
			{
				if (o.Equals(inputValue))
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} is in [{1}]",
				name,
				StringUtils.FormatArray(_inList)
				);
		}
	}
}
