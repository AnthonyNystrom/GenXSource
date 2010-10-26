using System;
using System.Runtime.Serialization;

namespace DotNetMock
{
	/// <summary>
	/// Exception thrown for errors in Verify()
	/// </summary>
	[Serializable()]
	public class VerifyException : ApplicationException
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public VerifyException() : base() {}
		/// <summary>
		/// Constructs a VerifyException with the provided message
		/// </summary>
		/// <param name="message">Message for this exception</param>
		public VerifyException(string message) : base(message) {}
		/// <summary>
		/// Constructs a VerifyException with the provided message and a reference to the inner 
		/// exception that caused this exception
		/// </summary>
		/// <param name="message">Message for this exception</param>
		/// <param name="innerException">Inner exception for this exception</param>
		public VerifyException(string message, Exception innerException) : base(message, innerException) {}
		/// <summary>
		/// Constructs a VerifyException with serialized data
		/// </summary>
		/// <param name="serialInfo">Object that holds the serialized object data</param>
		/// <param name="streamingContext">The contextual information about the source or destination</param>
		protected VerifyException(SerializationInfo serialInfo, StreamingContext streamingContext) : base(serialInfo, streamingContext) {}
	}
}
