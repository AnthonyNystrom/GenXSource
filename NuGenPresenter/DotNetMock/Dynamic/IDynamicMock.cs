using System;

namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Interface for setting up and invoking a IDynamicMock object. The default implementation of 
	/// this is <c>DynamicMock</c> but users may choose to implement their own with custom needs.
	/// </summary>
	/// <see cref="DynamicMock"/>
	public interface IDynamicMock : IMockObject, IMockedCallHandler
	{
		/// <summary>
		/// Get mocked version of object.
		/// </summary>
		object Object { get; }
		
		/// <summary>
		/// If strict, any method called that doesn't have an expectation set
		/// will fail. (Defaults false)
		/// </summary>
		bool Strict { get; set; }

		/// <summary>
		/// Expect a method to be called with the supplied parameters.
		/// </summary>		
		void Expect(string methodName, params object[] args);

		/// <summary>
		/// Expect no call to this method.
		/// </summary>		
		void ExpectNoCall(string methodName);

		/// <summary>
		/// Expect a method to be called with the supplied parameters and setup a 
		/// value to be returned.
		/// </summary>		
		void ExpectAndReturn(string methodName, object returnVal, params object[] args);

		/// <summary>
		/// Expect a method to be called with the supplied parameters and setup an 
		/// exception to be thrown.
		/// </summary>		
		void ExpectAndThrow(string methodName, Exception exceptionVal, params object[] args);

		/// <summary>
		/// Set a fixed return value for a method/property. This allows the method to be 
		/// called multiple times in no particular sequence and have the same value returned
		/// each time. Useful for getter style methods.
		/// </summary>
		void SetValue(string methodName, object returnVal);
	}
}
