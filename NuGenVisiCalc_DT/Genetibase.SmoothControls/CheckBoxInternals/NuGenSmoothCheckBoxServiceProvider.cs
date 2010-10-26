/* -----------------------------------------------
 * NuGenSmoothCheckBoxServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.CheckBoxInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.CheckBoxInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenCheckBoxRenderer"/></para>
	/// <para><see cref="INuGenCheckBoxLayoutManager"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// </summary>
	public class NuGenSmoothCheckBoxServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * CheckBoxLayoutManager
		 */

		private INuGenCheckBoxLayoutManager _checkBoxLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenCheckBoxLayoutManager CheckBoxLayoutManager
		{
			get
			{
				if (_checkBoxLayoutManager == null)
				{
					_checkBoxLayoutManager = new NuGenCheckBoxLayoutManager();
				}

				return _checkBoxLayoutManager;
			}
		}

		/*
		 * CheckBoxRenderer
		 */

		private INuGenCheckBoxRenderer _checkBoxRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenCheckBoxRenderer CheckBoxRenderer
		{
			get
			{
				if (_checkBoxRenderer == null)
				{
					_checkBoxRenderer = new NuGenSmoothCheckBoxRenderer(this);
				}

				return _checkBoxRenderer;
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
			if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenCheckBoxRenderer))
			{
				Debug.Assert(this.CheckBoxRenderer != null, "this.CheckBoxRenderer != null");
				return this.CheckBoxRenderer;
			}
			else if (serviceType == typeof(INuGenCheckBoxLayoutManager))
			{
				Debug.Assert(this.CheckBoxLayoutManager != null, "this.CheckBoxLayoutManager != null");
				return this.CheckBoxLayoutManager;
			}

			return base.GetService(serviceType);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothCheckBoxServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothCheckBoxServiceProvider()
		{
		}

		#endregion
	}
}
