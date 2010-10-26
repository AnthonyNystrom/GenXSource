/* -----------------------------------------------
 * NuGenSmoothPanelServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.PanelInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.SmoothControls.PanelInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenSmoothColorManager"/><para/>
	/// <see cref="INuGenPanelRenderer"/><para/>
	/// </summary>
	public class NuGenSmoothPanelServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * PanelRenderer
		 */

		private INuGenPanelRenderer _panelRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenPanelRenderer PanelRenderer
		{
			get
			{
				if (_panelRenderer == null)
				{
					_panelRenderer = new NuGenSmoothPanelRenderer(this);
				}

				return _panelRenderer;
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

			if (serviceType == typeof(INuGenPanelRenderer))
			{
				return this.PanelRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothPanelServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothPanelServiceProvider()
		{

		}

		#endregion
	}
}
