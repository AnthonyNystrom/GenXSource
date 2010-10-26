using System;

namespace DotNetMock
{
	/// <summary>
	/// Default IMockObject interface.  Also inherits from the IVerifiable interface
	/// </summary>
	public interface IMockObject : IVerifiable
	{
		/// <summary>
		/// Throws a NotImplementedException with given method name in the exception message
		/// </summary>
		/// <param name="methodName">Method that is not supported</param>
		void NotImplemented( string methodName );
		/// <summary>
		/// Name property for this mock object.
		/// </summary>
		string MockName {get; set;}
	}
}
