using System;
using System.Runtime.Serialization;

namespace DotNetMock
{
	/// <summary>
	/// Base Exception for the DotNetMock assemblies.  All custom exceptions used within the DotNetMock library
	/// will derive from this exception
	/// </summary>
	public class DotNetMockException : ApplicationException
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public DotNetMockException() : base() {}
		/// <summary>
		/// Constructs a DotNetMockException with the provided message
		/// </summary>
		/// <param name="message">Message for this exception</param>
		public DotNetMockException(string message) : base(message) {}
		/// <summary>
		/// Constructs a DotNetMockException with the provided message and a reference to the inner 
		/// exception that caused this exception
		/// </summary>
		/// <param name="message">Message for this exception</param>
		/// <param name="innerException">Inner exception for this exception</param>
		public DotNetMockException(string message, Exception innerException) : base(message, innerException) {}
		/// <summary>
		/// Constructs a DotNetMockException with serialized data
		/// </summary>
		/// <param name="serialInfo">Object that holds the serialized object data</param>
		/// <param name="streamingContext">The contextual information about the source or destination</param>
		protected DotNetMockException(SerializationInfo serialInfo, StreamingContext streamingContext) : base(serialInfo, streamingContext) {}

	}
}
