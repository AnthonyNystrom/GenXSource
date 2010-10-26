#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;

using System.Collections;
#endregion

namespace DotNetMock.Dynamic 
{
	/// <summary>
	/// Represents a dynamic mock object that enables expectations to be set to be called in a certain order.
	/// </summary>
	public class DynamicOrderedMock : DynamicMock	
	{
		private IList expectations = new ArrayList();

		/// <summary>
		/// Default constructor
		/// </summary>
		/// <param name="name">Name for the mock object</param>
		public DynamicOrderedMock( string name ) : base( name ) {}
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="type">Type to generate the mock for</param>
		/// <param name="name">Name for the mock object</param>
		public DynamicOrderedMock( Type type, string name ) : base( type, name ){}
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="type">Type to generate the mock for</param>
		public DynamicOrderedMock( Type type ) : base( type ) {}
		/// <summary>
		/// Verifies the mock object.
		/// </summary>
		public override void Verify() 
		{
            if (expectations.Count > 0)
            {
                foreach(ExpectationMethod em in expectations)
                {
                    if (em.ExpectationType != ExpectationMethodType.NoCall)
                    {
                        Assertion.Fail("Unfinished scenario: method " + em.ExpectedMethodName + "() wasn't called");
                    }
 
                }
            }
		}
		/// <summary>
		/// Adds a <see cref="IMethodCallExpectation"/> to the list of expectations of the mock object.
		/// </summary>
		/// <param name="e">Expectation to add</param>
		protected override void addExpectation(IMethodCallExpectation e)
		{
		    // prevent adding a Call expectation after a NoCall expectation
            if (e.ExpectationType == ExpectationMethodType.Call)
            {
                foreach (ExpectationMethod em in expectations)
                {
                    if (em.ExpectationType == ExpectationMethodType.NoCall)
                    {
                        Assertion.Fail("ExpectNoCall must be last on a DynamicOrderedMock");
                    }
                }
            }
			expectations.Add(e);
		}
		/// <summary>
		/// Retrieve next expectation and remove from FIFO.
		/// </summary>
		/// <param name="methodCall">
		///  <see cref="MethodCall"/> to get expectation for
		/// </param>
		/// <returns>next <see cref="IMethodCallExpectation"/></returns>
		/// <remarks>
		/// This is a state mutating method. It removes the expectation from
		/// a list. Not a big deal since we don't ever recover from any
		/// exceptions.
		/// </remarks>
		protected override IMethodCallExpectation nextExpectation(MethodCall methodCall) 
		{
			if (expectations.Count == 0) 
			{
				Assertion.Fail(String.Format(
					"Unexpected call {0}",
					methodCall
					));
			}
			IMethodCallExpectation e = (IMethodCallExpectation)expectations[0];
			expectations.RemoveAt(0);
			return e;
		}
	}
}
