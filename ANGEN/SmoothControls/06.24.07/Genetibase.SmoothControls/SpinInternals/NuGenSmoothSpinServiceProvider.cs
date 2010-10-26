/* -----------------------------------------------
 * NuGenSmoothSpinServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SpinInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.SpinInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenSmoothColorManager"/><para/>
	/// <see cref="INuGenSpinRenderer"/><para/>
	/// </summary>
	public class NuGenSmoothSpinServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Overridden

		/*
		 * SpinRenderer
		 */

		private INuGenSpinRenderer _spinRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenSpinRenderer SpinRenderer
		{
			get
			{
				if (_spinRenderer == null)
				{
					_spinRenderer = new NuGenSmoothSpinRenderer(this);
				}

				return _spinRenderer;
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

			if (serviceType == typeof(INuGenSpinRenderer))
			{
				Debug.Assert(this.SpinRenderer != null, "this.SpinRenderer != null");
				return this.SpinRenderer;
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
		/// Initializes a new instance of the <see cref="NuGenSmoothSpinServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothSpinServiceProvider()
		{
			
		}

		#endregion
	}
}
