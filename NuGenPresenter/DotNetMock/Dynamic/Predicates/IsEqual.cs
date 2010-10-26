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
	/// <see cref="IPredicate"/> comparing two values
	/// </summary>
	public class IsEqual : AbstractPredicate 
	{
		private object _compare;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="compare">value to compare against</param>
		public IsEqual(object compare)
		{
			_compare = compare;
		}
		/// <summary>
		/// Evaluates input value against original value for equality
		/// </summary>
		/// <param name="inputValue">input value</param>
		/// <returns>True if input value equals original value, false otherwise.</returns>
		public override bool Eval(object inputValue)
		{
			if ( ( inputValue == null ) & ( _compare != null ) ) 
			{
				return false;
			}
			if ( ( inputValue != null ) & ( _compare == null ) ) 
			{
				return false;
			}
			if ( ( inputValue == null ) & ( _compare == null ) ) 
			{
				return true;
			}
			if ( _compare.GetType().IsArray ) 
			{
				if ( ! inputValue.GetType().IsArray ) 
				{
					return false;
				}
				object[] currentArray = (object[]) inputValue;
				object[] compareArray = (object[]) _compare;
				if ( compareArray.Length != currentArray.Length ) 
				{
					return false;
				}
				for ( int counter = 0; counter < compareArray.Length; counter++ ) 
				{
					if ( !compareArray[ counter ].Equals( currentArray[ counter] ) )
					{
						return false;
					}
				}
				return true;
			} 
			else 
			{
				return _compare.Equals(inputValue);
			}
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} equals {1}",
				name,
				StringUtils.FormatScalar(_compare)
				);
		}

	}
}
