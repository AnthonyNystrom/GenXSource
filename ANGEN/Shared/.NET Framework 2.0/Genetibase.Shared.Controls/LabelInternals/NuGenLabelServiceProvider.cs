/* -----------------------------------------------
 * NuGenLabelServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.Shared.Controls.LabelInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenLabelRenderer"/></para>
	/// <para><see cref="INuGenLabelLayoutManager"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// </summary>
	public class NuGenLabelServiceProvider : NuGenImageControlServiceProvider
	{
		/*
		 * LayoutManager
		 */

		private INuGenLabelLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenLabelLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					_layoutManager = new NuGenLabelLayoutManager();
				}

				return _layoutManager;
			}
		}

		/*
		 * LabelRenderer
		 */

		private INuGenLabelRenderer _labelRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenLabelRenderer LabelRenderer
		{
			get
			{
				if (_labelRenderer == null)
				{
					_labelRenderer = new NuGenLabelRenderer();
				}

				return _labelRenderer;
			}
		}

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

			if (serviceType == typeof(INuGenLabelRenderer))
			{
				Debug.Assert(this.LabelRenderer != null, "this.LabelRenderer != null");
				return this.LabelRenderer;
			}
			else if (serviceType == typeof(INuGenLabelLayoutManager))
			{
				Debug.Assert(this.LayoutManager != null, "this.LayoutManager != null");
				return this.LayoutManager;
			}

			return base.GetService(serviceType);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLabelServiceProvider"/> class.
		/// </summary>
		public NuGenLabelServiceProvider()
		{
		}
	}
}
