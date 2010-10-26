#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using DotNetMock.Dynamic.Predicates;
#endregion

namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Utilities that for working with predicates.
	/// </summary>
	public class PredicateUtils
	{
		/// <summary>
		/// Create an appropriate <see cref="IPredicate"/> given an
		/// abitrary object.
		/// </summary>
		/// <param name="expectation">argument expectation</param>
		/// <returns><see cref="IPredicate"/> that is appropriate for
		/// the specified object</returns>
		public static IPredicate ConvertFrom(object expectation) 
		{
			IPredicate predicate = expectation as IPredicate;
			if ( predicate==null ) 
			{
				if ( expectation==null ) 
				{
					predicate = new IsAnything();
				}
				else 
				{
					predicate = new IsEqual(expectation);
				}
			}
			return predicate;
		}
	}
}
