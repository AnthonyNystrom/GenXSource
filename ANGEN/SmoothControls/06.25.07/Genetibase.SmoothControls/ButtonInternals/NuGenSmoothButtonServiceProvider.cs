/* -----------------------------------------------
 * NuGenSmoothButtonServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.ButtonInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenButtonRenderer"/></para>
	/// <para><see cref="INuGenButtonLayoutManager"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothButtonServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ButtonRenderer
		 */

		private INuGenButtonRenderer _buttonRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenButtonRenderer ButtonRenderer
		{
			get
			{
				if (_buttonRenderer == null)
				{
					_buttonRenderer = new NuGenSmoothButtonRenderer(this);
				}

				return _buttonRenderer;
			}
		}

		/*
		 * LayoutManager
		 */

		private INuGenButtonLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenButtonLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					_layoutManager = new NuGenSmoothButtonLayoutManager();
				}

				return _layoutManager;
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
			else if (serviceType == typeof(INuGenButtonRenderer))
			{
				Debug.Assert(this.ButtonRenderer != null, "this.ButtonRenderer != null");
				return this.ButtonRenderer;
			}
			else if (serviceType == typeof(INuGenButtonLayoutManager))
			{
				Debug.Assert(this.LayoutManager != null, "this.LayoutManager != null");
				return this.LayoutManager;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothButtonServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothButtonServiceProvider()
		{
		}

		#endregion
	}
}
