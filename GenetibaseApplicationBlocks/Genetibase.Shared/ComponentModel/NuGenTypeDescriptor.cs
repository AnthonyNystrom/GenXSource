/* -----------------------------------------------
 * NuGenTypeDescriptor.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Provides methods for <see cref="TypeDescriptor"/> functionality easy access.
	/// </summary>
	public static class NuGenTypeDescriptor
	{
		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public static NuGenObjectDescriptor Instance(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			return new NuGenObjectDescriptor(instance);
		}
	}
}
