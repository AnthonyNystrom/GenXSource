/* -----------------------------------------------
 * NuGenSmoothProgressBarServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.ProgressBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.ProgressBarInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenProgressBarRenderer"/></para>
	/// <para><see cref="INuGenProgressBarLayoutManager"/></para>
	/// </summary>
	public class NuGenSmoothProgressBarServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * ProgressBarLayoutManager
		 */

		private INuGenProgressBarLayoutManager _progressBarLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenProgressBarLayoutManager ProgressBarLayoutManager
		{
			get
			{
				if (_progressBarLayoutManager == null)
				{
					_progressBarLayoutManager = new NuGenProgressBarLayoutManager();
				}

				return _progressBarLayoutManager;
			}
		}

		/*
		 * ProgressBarRenderer
		 */

		private INuGenProgressBarRenderer _progressBarRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenProgressBarRenderer ProgressBarRenderer
		{
			get
			{
				if (_progressBarRenderer == null)
				{
					_progressBarRenderer = new NuGenSmoothProgressBarRenderer(this);
				}

				return _progressBarRenderer;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

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

			if (serviceType == typeof(INuGenProgressBarRenderer))
			{
				Debug.Assert(this.ProgressBarRenderer != null, "this.ProgressBarRenderer != null");
				return this.ProgressBarRenderer;
			}
			else if (serviceType == typeof(INuGenProgressBarLayoutManager))
			{
				Debug.Assert(this.ProgressBarLayoutManager != null, "this.ProgressBarLayoutManager != null");
				return this.ProgressBarLayoutManager;
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
		/// Initializes a new instance of the <see cref="NuGenSmoothProgressBarServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothProgressBarServiceProvider()
		{

		}

		#endregion
	}
}
