/* -----------------------------------------------
 * NuGenEventNotFoundException.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using Genetibase.Shared.Properties;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// </summary>
	[Serializable]
	public class NuGenEventNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventNotFoundException"/> class.
		/// </summary>
		public NuGenEventNotFoundException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventNotFoundException"/> class.
		/// </summary>
		public NuGenEventNotFoundException(String eventName, Type target)
			: base(String.Format(CultureInfo.InvariantCulture, Resources.Message_EventNotFoundException, eventName, target))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventNotFoundException"/> class.
		/// </summary>
		public NuGenEventNotFoundException(String eventName, Type target, Exception inner)
			: base(String.Format(CultureInfo.InvariantCulture, Resources.Message_EventNotFoundException, eventName, target), inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventNotFoundException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected NuGenEventNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
