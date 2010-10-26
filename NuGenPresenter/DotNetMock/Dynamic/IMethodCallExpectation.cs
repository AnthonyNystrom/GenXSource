#region License
// Copyright (c) 2005 Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
#endregion

namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Interface for expectations on method calls..
	/// </summary>
	public interface IMethodCallExpectation
	{
		/// <summary>
		/// Expected method name.
		/// </summary>
		string ExpectedMethodName { get; }

        /// <summary>
        /// The type of expectation, call or no call
        /// </summary>
        ExpectationMethodType ExpectationType { get; }
	    
		/// <summary>
		/// Check actual incoming method call and return expected outgoing response.
		/// </summary>
		/// <param name="call">incoming call</param>
		/// <returns>expected return value</returns>
		/// <remarks>
		/// The outgoing response may be an exception or the modification
		/// of ref/out parameters.
		/// </remarks>
		object CheckCallAndSendResponse(MethodCall call);
	}
}
