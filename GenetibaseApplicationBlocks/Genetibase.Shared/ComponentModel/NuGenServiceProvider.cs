/* -----------------------------------------------
 * NuGenServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Basic <see cref="INuGenServiceProvider"/> implementation.
	/// </summary>
	public abstract class NuGenServiceProvider : INuGenServiceProvider
	{
		#region INuGenServiceProvider Members

		/// <summary>
		/// </summary>
		/// <returns></returns>
		/// <typeparam name="T"></typeparam>
		public T GetService<T>() where T : class
		{
			return (T)this.GetService(typeof(T));
		}

		#endregion

		#region Methods.Protected.Virtual

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceType"/> is <see langword="null"/>.</para>
		/// </exception>
		protected virtual object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			return null;
		}

		#endregion
	}
}
