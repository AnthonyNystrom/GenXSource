using System;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Base exception class for NugenCCalc component
	/// </summary>
	public abstract class NugenCCalcException : ApplicationException
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public NugenCCalcException() : base()
		{
		}

		/// <summary>
		/// Constructor with exception message
		/// </summary>
		/// <param name="message"></param>
		public NugenCCalcException(string message) : base(message)
		{

		}


		/// <summary>
		/// Constructor with message and inner exception
		/// </summary>
		/// <param name="message"></param>
		/// <param name="inner"></param>
		public NugenCCalcException(string message, Exception inner) : base(message,inner)
		{

		}
	}
}
