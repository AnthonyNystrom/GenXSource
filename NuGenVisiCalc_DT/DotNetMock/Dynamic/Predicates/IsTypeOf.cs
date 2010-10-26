#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
#endregion

namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> that evaluates the input value against the expected <see cref="Type"/>
	/// </summary>
	public class IsTypeOf : AbstractPredicate 
	{
		private Type _type;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="type">type to evaluate against</param>
		public IsTypeOf(Type type)
		{
			_type = type;
		}
		/// <summary>
		/// Evaluates the input value to verify that it is correctly typed.
		/// </summary>
		/// <param name="currentValue">input value</param>
		/// <returns>True if the original type is assignable from the input value, false otherwise.</returns>
		public override bool Eval(object currentValue)
		{
			return currentValue == null ? false : _type.IsAssignableFrom(currentValue.GetType()); 
		}
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		public override string ExpressionAsText(string name)
		{
			return String.Format(
				"{0} is a {1}",
				name,
				_type.FullName
				);
		}
	}
}
