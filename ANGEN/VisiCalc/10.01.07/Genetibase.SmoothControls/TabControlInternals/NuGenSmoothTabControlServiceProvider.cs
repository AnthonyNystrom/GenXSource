/* -----------------------------------------------
 * NuGenSmoothTabControlServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.CloseButtonInternals;
using Genetibase.Shared.Controls.TabControlInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.TabControlInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/>
	/// <see cref="INuGenCloseButtonRenderer"/><para/>
	/// <see cref="INuGenTabRenderer"/><para/>
	/// <see cref="INuGenTabStateService"/><para/>
	/// <see cref="INuGenTabLayoutManager"/><para/>
	/// <see cref="INuGenSmoothColorManager"/><para/>
	/// </summary>
	public class NuGenSmoothTabControlServiceProvider : NuGenTabControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * TabLayoutManager
		 */

		private INuGenTabLayoutManager _tabLayoutManager;

		/// <summary>
		/// </summary>
		protected override INuGenTabLayoutManager TabLayoutManager
		{
			get
			{
				if (_tabLayoutManager == null)
				{
					_tabLayoutManager = new NuGenSmoothTabLayoutManager();
				}

				return _tabLayoutManager;
			}
		}

		/*
		 * TabRenderer
		 */

		private INuGenTabRenderer _tabRenderer;

		/// <summary>
		/// </summary>
		/// <value></value>
		protected override INuGenTabRenderer TabRenderer
		{
			get
			{
				if (_tabRenderer == null)
				{
					_tabRenderer = new NuGenSmoothTabRenderer(this);
				}

				return _tabRenderer;
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
		/// <exception cref="ArgumentNullException"><paramref name="serviceType"/> is <see langword="null"/>.</exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTabControlServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothTabControlServiceProvider()
		{

		}

		#endregion
	}
}
