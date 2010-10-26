/* -----------------------------------------------
 * NuGenTabControlServiceProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.CloseButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.TabControlInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenTabRenderer"/><para/>
	/// <see cref="INuGenTabStateService"/><para/>
	/// <see cref="INuGenTabLayoutManager"/><para/>
	/// <see cref="INuGenCloseButtonRenderer"/><para/>
	/// </summary>
	public class NuGenTabControlServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * CloseButtonRenderer
		 */

		private INuGenCloseButtonRenderer _closeButtonRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenCloseButtonRenderer CloseButtonRenderer
		{
			get
			{
				if (_closeButtonRenderer == null)
				{
					_closeButtonRenderer = new NuGenCloseButtonRenderer();
				}

				return _closeButtonRenderer;
			}
		}

		/*
		 * TabLayoutManager
		 */

		private INuGenTabLayoutManager _tabLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenTabLayoutManager TabLayoutManager
		{
			get
			{
				if (_tabLayoutManager == null)
				{
					_tabLayoutManager = new NuGenTabLayoutManager();
				}

				return _tabLayoutManager;
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
		 * TabStateService
		 */

		private INuGenTabStateService _tabStateService;

		/// <summary>
		/// </summary>
		protected virtual INuGenTabStateService TabStateService
		{
			get
			{
				if (_tabStateService == null)
				{
					_tabStateService = new NuGenTabStateService();
				}

				return _tabStateService;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

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
		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			else if (serviceType == typeof(INuGenTabRenderer))
			{
				Debug.Assert(this.TabRenderer != null, "this.TabRenderer != null");
				return this.TabRenderer;
			}
			else if (serviceType == typeof(INuGenTabStateService))
			{
				Debug.Assert(this.TabStateService != null, "this.TabStateService != null");
				return this.TabStateService;
			}
			else if (serviceType == typeof(INuGenCloseButtonRenderer))
			{
				Debug.Assert(this.CloseButtonRenderer != null, "this.CloseButtonRenderer != null");
				return this.CloseButtonRenderer;
			}
			else if (serviceType == typeof(INuGenTabLayoutManager))
			{
				Debug.Assert(this.TabLayoutManager != null, "this.TabLayoutManager != null");
				return this.TabLayoutManager;
			}

			return base.GetService(serviceType);
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
