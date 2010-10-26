/* -----------------------------------------------
 * NuGenButtonBaseServiceProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Controls
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateTracker"/><para/>
	/// </summary>
	public class NuGenButtonBaseServiceProvider : INuGenServiceProvider
	{
		#region INuGenServiceProvider Members

		/// <summary>
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T GetService<T>() where T : class
		{
			return (T)this.GetService(typeof(T));
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * ButtonStateTracker
		 */

		private INuGenButtonStateTracker _buttonStateTracker = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenButtonStateTracker ButtonStateTracker
		{
			get
			{
				if (_buttonStateTracker == null)
				{
					_buttonStateTracker = new NuGenButtonStateTracker();
				}

				return _buttonStateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="serviceType"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		protected virtual object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenButtonStateTracker))
			{
				Debug.Assert(this.ButtonStateTracker != null, "this.ButtonStateTracker != null");
				return this.ButtonStateTracker;
			}

			return null;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenButtonBaseServiceProvider"/> class.
		/// </summary>
		public NuGenButtonBaseServiceProvider()
		{

		}

		#endregion
	}
}
