/* -----------------------------------------------
 * NuGenTabControlServiceProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
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
	/// <see cref="INuGenTabRenderer"/><para/>
	/// <see cref="INuGenTabStateTracker"/><para/>
	/// </summary>
	public class NuGenTabControlServiceProvider : INuGenServiceProvider
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

		/*
		 * TabRenderer
		 */

		private INuGenTabRenderer _tabRenderer = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenTabRenderer TabRenderer
		{
			get
			{
				if (_tabRenderer == null)
				{
					_tabRenderer = new NuGenTabRenderer();
				}

				return _tabRenderer;
			}
		}

		/*
		 * TabStateTracker
		 */

		private INuGenTabStateTracker _tabStateTracker = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenTabStateTracker TabStateTracker
		{
			get
			{
				if (_tabStateTracker == null)
				{
					_tabStateTracker = new NuGenTabStateTracker();
				}

				return _tabStateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * GetService
		 */

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

			if (serviceType == typeof(INuGenTabRenderer))
			{
				Debug.Assert(this.TabRenderer != null, "this.TabRenderer != null");
				return this.TabRenderer;
			}

			if (serviceType == typeof(INuGenTabStateTracker))
			{
				Debug.Assert(this.TabStateTracker != null, "this.TabStateTracker != null");
				return this.TabStateTracker;
			}

			return null;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenTabControlServiceProvider"/> class.
		/// </summary>
		public NuGenTabControlServiceProvider()
		{

		}

		#endregion
	}
}
