/* -----------------------------------------------
 * NuGenSmoothRadioButtonServiceProvider.cs
 * Copyright © 2007 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.RadioButtonInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Genetibase.SmoothControls.RadioButtonInternals
{
	/// <summary>
	/// <para>Provides:</para>
	/// <para><see cref="INuGenButtonStateService"/></para>
	/// <para><see cref="INuGenControlStateService"/></para>
	/// <para><see cref="INuGenSmoothColorManager"/></para>
	/// <para><see cref="INuGenRadioButtonLayoutManager"/></para>
	/// <para><see cref="INuGenRadioButtonRenderer"/></para>
	/// </summary>
	public class NuGenSmoothRadioButtonServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual

		/*
		 * RadioButtonLayoutManager
		 */

		private INuGenRadioButtonLayoutManager _radioButtonLayoutManager;

		/// <summary>
		/// </summary>
		protected virtual INuGenRadioButtonLayoutManager RadioButtonLayoutManager
		{
			get
			{
				if (_radioButtonLayoutManager == null)
				{
					_radioButtonLayoutManager = new NuGenRadioButtonLayoutManager();
				}

				return _radioButtonLayoutManager;
			}
		}

		/*
		 * RadioButtonRenderer
		 */

		private INuGenRadioButtonRenderer _radioButtonRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenRadioButtonRenderer RadioButtonRenderer
		{
			get
			{
				if (_radioButtonRenderer == null)
				{
					_radioButtonRenderer = new NuGenSmoothRadioButtonRenderer(this);
				}

				return _radioButtonRenderer;
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
			if (serviceType == typeof(INuGenRadioButtonRenderer))
			{
				Debug.Assert(this.RadioButtonRenderer != null, "this.RadioButtonRenderer != null");
				return this.RadioButtonRenderer;
			}
			else if (serviceType == typeof(INuGenSmoothColorManager))
			{
				return NuGenSmoothServiceManager.SmoothServiceProvider.GetService<INuGenSmoothColorManager>();
			}
			else if (serviceType == typeof(INuGenRadioButtonLayoutManager))
			{
				Debug.Assert(this.RadioButtonLayoutManager != null, "this.RadioButtonLayoutManager != null");
				return this.RadioButtonLayoutManager;
			}
			
			return base.GetService(serviceType);
		}

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenSmoothRadioButtonServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothRadioButtonServiceProvider()
		{
		}
	}
}
