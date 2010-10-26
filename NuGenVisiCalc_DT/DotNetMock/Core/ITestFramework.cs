using System;

namespace DotNetMock.Core
{
	/// <summary>
	/// Interface to encapsulate specific testing framework from DotNetMock.
	/// </summary>
	public interface ITestFramework
	{
		#region AssertEquals methods
		/// <summary>
		/// Asserts that two objects are equal.  If the objects are not equal the assertions fails with the
		/// given message.
		/// </summary>
		void AssertEquals( string message, object expectedObject, object actualObject );
		/// <summary>
		/// Asserts that two objects are equal.  If the objects are not equal the assertions fails.
		/// </summary>
		void AssertEquals( object expectedObject, object actualObject );
		#endregion

		#region Fail methods
		/// <summary>
		/// Fails a test with no message
		/// </summary>
		void Fail();
		/// <summary>
		/// Fails a test with the specified message
		/// </summary>
		void Fail( string message );
		#endregion

		#region Assert methods 
		/// <summary>
		/// Asserts that a given condition is true.  If the condition is not true, the assertion fails
		/// with the given message.
		/// </summary>
		void Assert( string message, bool assertion );
		/// <summary>
		/// Asserts that a given condition is true.  If the condition is not true, the assertion fails.
		/// </summary>
		void Assert( bool assertion );
		#endregion

		#region AssertNull methods
		/// <summary>
		/// Asserts that an object is null.  If the object is not null, the assertion fails with the given 
		/// message.
		/// </summary>
		void AssertNull( string message, object assertion );
		/// <summary>
		/// Asserts that an object is null.  If the object is not null, the assertion fails.
		/// </summary>
		void AssertNull( object assertion );
		#endregion

		#region AssertNotNull methods
		/// <summary>
		/// Asserts that an object is not null.  If the object is null, the assertion fails with the given
		/// message.
		/// </summary>
		void AssertNotNull( string message, object assertion );
		/// <summary>
		/// Asserts that an object is not null.  If the object is null, the assertion fails.
		/// </summary>
		void AssertNotNull( object assertion );
		#endregion
	}
}
