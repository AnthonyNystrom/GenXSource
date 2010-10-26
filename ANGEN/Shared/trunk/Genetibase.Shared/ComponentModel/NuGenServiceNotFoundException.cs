/* -----------------------------------------------
 * NuGenServiceNotFoundException.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Globalization;
using System.Runtime.Serialization;
using Genetibase.Shared.Properties;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Thrown if the required service was not found.
	/// </summary>
	[Serializable]
	public class NuGenServiceNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenServiceNotFoundException"/> class.
		/// </summary>
		public NuGenServiceNotFoundException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenServiceNotFoundException"/> class.
		/// </summary>
		public NuGenServiceNotFoundException(Type serviceType)
			: base(string.Format(CultureInfo.InvariantCulture, Resources.Message_ServiceNotFoundException, serviceType))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenServiceNotFoundException"/> class.
		/// </summary>
		public NuGenServiceNotFoundException(Type serviceType, Exception inner)
			: base(String.Format(CultureInfo.InvariantCulture, Resources.Message_ServiceNotFoundException, serviceType), inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenServiceNotFoundException"/> class.
		/// </summary>
		protected NuGenServiceNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}

	/// <summary>
	/// Thrown if the required service was not found.
	/// </summary>
	[Serializable]
	public class NuGenServiceNotFoundException<TService> : NuGenServiceNotFoundException
	{
		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:Genetibase.Shared.ComponentModel.NuGenServiceNotFoundException`1"/> class.
		/// </summary>
		public NuGenServiceNotFoundException()
			: base(typeof(TService))
		{
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:Genetibase.Shared.ComponentModel.NuGenServiceNotFoundException`1"/> class.
		/// </summary>
		public NuGenServiceNotFoundException(Exception inner)
			: base(typeof(TService), inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="T:Genetibase.Shared.ComponentModel.NuGenServiceNotFoundException`1"/> class.
		/// </summary>
		protected NuGenServiceNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
