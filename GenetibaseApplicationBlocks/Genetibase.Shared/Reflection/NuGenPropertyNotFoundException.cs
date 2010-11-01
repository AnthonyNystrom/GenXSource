/* -----------------------------------------------
 * NuGenPropertyNotFoundException.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Properties;

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// </summary>
	[Serializable]
	public class NuGenPropertyNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyNotFoundException"/> class.
		/// </summary>
		public NuGenPropertyNotFoundException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyNotFoundException"/> class.
		/// </summary>
		public NuGenPropertyNotFoundException(string propertyName, Type target)
			: base(string.Format(Resources.Message_PropertyNotFoundException, propertyName, target))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyNotFoundException"/> class.
		/// </summary>
		public NuGenPropertyNotFoundException(string propertyName, Type target, Exception inner)
			: base(string.Format(Resources.Message_PropertyNotFoundException, propertyName, target), inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenPropertyNotFoundException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected NuGenPropertyNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
