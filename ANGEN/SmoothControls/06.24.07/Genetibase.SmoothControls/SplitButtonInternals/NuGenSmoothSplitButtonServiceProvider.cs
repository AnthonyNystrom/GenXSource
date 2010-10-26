/* -----------------------------------------------
 * NuGenSmoothSplitButtonServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.SplitButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.SplitButtonInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenSplitButtonLayoutManager"/></para>
	/// <para><see cref="INuGenSplitButtonRenderer"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothSplitButtonServiceProvider : NuGenControlServiceProvider
	{
		private INuGenSplitButtonLayoutManager _splitButtonLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenSplitButtonLayoutManager SplitButtonLayoutManager
		{
			get
			{
				if (_splitButtonLayoutManager == null)
				{
					_splitButtonLayoutManager = new NuGenSmoothSplitButtonLayoutManager();
				}

				return _splitButtonLayoutManager;
			}
		}

		private INuGenSplitButtonRenderer _splitButtonRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenSplitButtonRenderer SplitButtonRenderer
		{
			get
			{
				if (_splitButtonRenderer == null)
				{
					_splitButtonRenderer = new NuGenSmoothSplitButtonRenderer(this);
				}

				return _splitButtonRenderer;
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
			else if (serviceType == typeof(INuGenSplitButtonLayoutManager))
			{
				Debug.Assert(this.SplitButtonLayoutManager != null, "this.SplitButtonLayoutManager != null");
				return this.SplitButtonLayoutManager;
			}
			else if (serviceType == typeof(INuGenSplitButtonRenderer))
			{
				Debug.Assert(this.SplitButtonRenderer != null, "this.SplitButtonRenderer != null");
				return this.SplitButtonRenderer;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothSplitButtonServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothSplitButtonServiceProvider()
		{
		}
	}
}
