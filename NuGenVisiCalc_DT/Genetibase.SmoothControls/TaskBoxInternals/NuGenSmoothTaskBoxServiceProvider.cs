/* -----------------------------------------------
 * NuGenSmoothTaskBoxServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TaskBoxInternals;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.TaskBoxInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenTaskBoxLayoutManager"/></para>
	/// <para><see cref="INuGenTaskBoxRenderer"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// </summary>
	public class NuGenSmoothTaskBoxServiceProvider : NuGenImageControlServiceProvider
	{
		/*
		 * TaskBoxLayoutManager
		 */

		private INuGenTaskBoxLayoutManager _taskBoxLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenTaskBoxLayoutManager TaskBoxLayoutManager
		{
			get
			{
				if (_taskBoxLayoutManager == null)
				{
					_taskBoxLayoutManager = new NuGenSmoothTaskBoxLayoutManager();
				}

				return _taskBoxLayoutManager;
			}
		}

		/*
		 * TaskBoxRenderer
		 */

		private INuGenTaskBoxRenderer _taskBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenTaskBoxRenderer TaskBoxRenderer
		{
			get
			{
				if (_taskBoxRenderer == null)
				{
					_taskBoxRenderer = new NuGenSmoothTaskBoxRenderer(this);
				}

				return _taskBoxRenderer;
			}
		}

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
			else if (serviceType == typeof(INuGenTaskBoxRenderer))
			{
				Debug.Assert(this.TaskBoxRenderer != null, "this.TaskBoxRenderer != null");
				return this.TaskBoxRenderer;
			}
			else if (serviceType == typeof(INuGenTaskBoxLayoutManager))
			{
				Debug.Assert(this.TaskBoxLayoutManager != null, "this.TaskBoxLayoutManager != null");
				return this.TaskBoxLayoutManager;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothTaskBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothTaskBoxServiceProvider()
		{
		}
	}
}
