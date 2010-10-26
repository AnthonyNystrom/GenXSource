/* -----------------------------------------------
 * NuGenObjectDescriptor.cs
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
	/// Provides functionality to operate the associated instance's members through <see cref="TypeDescriptor"/>.
	/// </summary>
	public sealed class NuGenObjectDescriptor
	{
		private object _instance;

		/// <summary>
		/// </summary>
		public NuGenPropertyDescriptorCollection Properties
		{
			get
			{
				return new NuGenPropertyDescriptorCollection(TypeDescriptor.GetProperties(_instance), _instance);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenObjectDescriptor"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="instance"/> is <see langword="null"/>.</para>
		/// </exception>
		public NuGenObjectDescriptor(object instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}

			_instance = instance;
		}
	}
}
