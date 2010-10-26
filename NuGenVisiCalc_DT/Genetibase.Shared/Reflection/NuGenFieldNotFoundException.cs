/* -----------------------------------------------
 * NuGenFieldNotFoundException.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Properties;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace Genetibase.Shared.Reflection
{
	/// <summary>
	/// </summary>
	[Serializable]
	public class NuGenFieldNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFieldNotFoundException"/> class.
		/// </summary>
		public NuGenFieldNotFoundException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFieldNotFoundException"/> class.
		/// </summary>
		public NuGenFieldNotFoundException(string fieldName, Type target)
			: base(string.Format(CultureInfo.InvariantCulture, Resources.Message_FieldNotFoundException, fieldName, target))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFieldNotFoundException"/> class.
		/// </summary>
		public NuGenFieldNotFoundException(string fieldName, Type target, Exception inner)
			: base(string.Format(CultureInfo.InvariantCulture, Resources.Message_FieldNotFoundException, fieldName, target), inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenFieldNotFoundException"/> class.
		/// </summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"></see> that holds the serialized object data about the exception being thrown.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"></see> that contains contextual information about the source or destination.</param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"></see> is zero (0). </exception>
		/// <exception cref="T:System.ArgumentNullException">The info parameter is null. </exception>
		protected NuGenFieldNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
