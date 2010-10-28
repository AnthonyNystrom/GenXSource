using System;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// The exception that is thrown when compiler for code expression return error.
	/// </summary>
	public class CompileException : NugenCCalcException
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public CompileException() : base()
		{

		}

		/// <summary>
		/// Constructor with exception message
		/// </summary>
		/// <param name="message"></param>
		public CompileException(string message) : base(message)
		{

		}


		/// <summary>
		/// Constructor with message and inner exception
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public CompileException(string message, Exception inner) : base(message,inner)
		{

		}
	}
}
