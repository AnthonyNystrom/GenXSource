using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Genetibase.Debug
{
	public class Exception : ApplicationException
	{
		public Exception(string message) : base(message)
		{
		}
	}

	public sealed class DebugMessageArgs : EventArgs
	{
		private string _message;
	
		public DebugMessageArgs(string message)
		{
			_message = message;
		}

		public string Message
		{
			get
			{
				return _message;
			}
		}
	}

	public delegate void DebugMessageAvailable(object sender, DebugMessageArgs e);
}
