using System;

namespace DotNetMock
{
	/// <summary>
	/// Interface that all expectation implement. Also implements the Verifiable interface
	/// </summary>
	/// <remarks/>
	public interface IExpectation : IVerifiable
	{
		/// <summary>
		/// Gets/Sets Has Expectations
		/// </summary>
		bool HasExpectations {get;set;}
		/// <summary>
		/// Sets the verify immediate flag
		/// </summary>
		bool VerifyImmediate {get;set;}
		/// <summary>
		/// Gets should check immediate
		/// </summary>
		bool ShouldCheckImmediate {get;}
		/// <summary>
		/// Gets / Sets if the expectation should be strict or not.  This means that as long as expectations are met, any other
		/// object state will be ignored.  If this is false, only the set expectations will be allowed.
		/// </summary>
		bool Strict { get; set; }
	}
}
