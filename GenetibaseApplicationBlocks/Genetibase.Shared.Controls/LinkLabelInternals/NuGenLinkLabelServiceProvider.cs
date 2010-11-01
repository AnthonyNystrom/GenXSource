/* -----------------------------------------------
 * NuGenLinkLabelServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.Shared.Controls.LinkLabelInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenLinkLabelRenderer"/></para>
	/// <para><see cref="INuGenLinkLabelLayoutManager"/></para>
	/// <para><see cref="INuGenControlImageManager"/></para>
	/// </summary>
	public class NuGenLinkLabelServiceProvider : NuGenImageControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * LayoutManager
		 */

		private INuGenLinkLabelLayoutManager _layoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenLinkLabelLayoutManager LayoutManager
		{
			get
			{
				if (_layoutManager == null)
				{
					_layoutManager = new NuGenLinkLabelLayoutManager();
				}

				return _layoutManager;
			}
		}

		/*
		 * LinkLabelRenderer
		 */

		private INuGenLinkLabelRenderer _linkLabelRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenLinkLabelRenderer LinkLabelRenderer
		{
			get
			{
				if (_linkLabelRenderer == null)
				{
					_linkLabelRenderer = new NuGenLinkLabelRenderer();
				}

				return _linkLabelRenderer;
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

			if (serviceType == typeof(INuGenLinkLabelRenderer))
			{
				Debug.Assert(this.LinkLabelRenderer != null, "this.LinkLabelRenderer != null");
				return this.LinkLabelRenderer;
			}
			else if (serviceType == typeof(INuGenLinkLabelLayoutManager))
			{
				Debug.Assert(this.LayoutManager != null, "this.LayoutManager != null");
				return this.LayoutManager;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenLinkLabelServiceProvider"/> class.
		/// </summary>
		public NuGenLinkLabelServiceProvider()
		{

		}

		#endregion
	}
}
