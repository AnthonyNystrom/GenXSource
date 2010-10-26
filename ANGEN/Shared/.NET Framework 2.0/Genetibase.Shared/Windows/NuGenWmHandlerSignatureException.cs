/* -----------------------------------------------
 * NuGenWmHandlerSignatureException.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Properties;

using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Thrown if the method does not match <see cref="NuGenWmHandler"/> delegate.
	/// </summary>
	[Serializable]
	public class NuGenWmHandlerSignatureException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWmHandlerSignatureException"/> class.
		/// </summary>
		public NuGenWmHandlerSignatureException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWmHandlerSignatureException"/> class.
		/// </summary>
		public NuGenWmHandlerSignatureException(string invalidMethodName)
			: base(string.Format(CultureInfo.InvariantCulture, Resources.Message_WmHandlerSignatureException, invalidMethodName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWmHandlerSignatureException"/> class.
		/// </summary>
		public NuGenWmHandlerSignatureException(string invalidMethodName, Exception inner)
			: base(string.Format(CultureInfo.InvariantCulture, Resources.Message_WmHandlerSignatureException, invalidMethodName), inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenWmHandlerSignatureException"/> class.
		/// </summary>
		protected NuGenWmHandlerSignatureException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
