namespace DotNetMock
{
	using System;
	/// <summary>
	/// A Verifiable is an object that can confirm at the end of a unit test that
	/// the correct behvaiour has occurred.
	/// </summary>
	/// <remarks/>
	public interface IVerifiable
	{
		/// <summary>
		/// Verifies object
		/// </summary>
		void Verify();
		/// <summary>
		/// Flag indicating if this object has been verified.
		/// </summary>
		bool IsVerified { get; }
	}
}
