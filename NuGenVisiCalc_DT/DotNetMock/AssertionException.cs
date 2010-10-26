using System;
using System.Runtime.Serialization;

namespace DotNetMock
{
	/// <summary>
	/// Assertion exception to encapsulate framework specific exceptions into a general AssertionException
	/// that is used throughout the DotNetMock library.
	/// </summary>
	public class AssertionException: ApplicationException
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public AssertionException() : base() {}
		/// <summary>
		/// Constructs a AssertionException with the provided message
		/// </summary>
		/// <param name="message">Message for this exception</param>
		public AssertionException(string message) : base(message) {}
		/// <summary>
		/// Constructs a AssertionException with the provided message and a reference to the inner 
		/// exception that caused this exception
		/// </summary>
		/// <param name="message">Message for this exception</param>
		/// <param name="innerException">Inner exception for this exception</param>
		public AssertionException(string message, Exception innerException) : base(message, innerException) {}
		/// <summary>
		/// Constructs a AssertionException with serialized data
		/// </summary>
		/// <param name="serialInfo">Object that holds the serialized object data</param>
		/// <param name="streamingContext">The contextual information about the source or destination</param>
		protected AssertionException(SerializationInfo serialInfo, StreamingContext streamingContext) : base(serialInfo, streamingContext) {}
	}
}
