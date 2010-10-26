/* -----------------------------------------------
 * NuGenSmoothTrackBarServiceProvider.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls;
using Genetibase.Shared.Controls.TrackBarInternals;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Genetibase.SmoothControls.TrackBarInternals
{
	/// <summary>
	/// Provides:<para/>
	/// <see cref="INuGenSmoothColorManager"/><para/>
	/// <see cref="INuGenButtonStateService"/><para/>
	/// <see cref="INuGenControlStateService"/><para/>
	/// <see cref="INuGenTrackBarRenderer"/><para/>
	/// <see cref="INuGenValueTrackerService"/><para/>
	/// </summary>
	public class NuGenSmoothTrackBarServiceProvider : NuGenControlServiceProvider
	{
		#region Properties.Protected.Virtual
		
		/*
		 * TrackBarRenderer
		 */

		private INuGenTrackBarRenderer _trackBarRenderer;

		/// <summary>
		/// </summary>
		protected virtual INuGenTrackBarRenderer TrackBarRenderer
		{
			get
			{
				if (_trackBarRenderer == null)
				{
					_trackBarRenderer = new NuGenSmoothTrackBarRenderer(this);
				}

				return _trackBarRenderer;
			}
		}

		/*
		 * ValueTrackerService
		 */

		private INuGenValueTrackerService _valueTrackerService;

		/// <summary>
		/// </summary>
		protected virtual INuGenValueTrackerService ValueTrackerService
		{
			get
			{
				if (_valueTrackerService == null)
				{
					_valueTrackerService = new NuGenValueTrackerService();
				}

				return _valueTrackerService;
			}
		}

		#endregion

		#region Methods.Protected.Virtual

		/*
		 * GetService
		 */

		/// <summary>
		/// </summary>
		/// <param name="serviceType"></param>
		/// <exception cref="ArgumentNullException">
		/// <para><paramref name="serviceType"/> is <see langword="null"/>.</para>
		/// </exception>
		protected override object GetService(Type serviceType)
		{
			if (serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}

			if (serviceType == typeof(INuGenTrackBarRenderer))
			{
				Debug.Assert(this.TrackBarRenderer != null, "this.TrackBarRenderer != null");
				return this.TrackBarRenderer;
			}
			else if (serviceType == typeof(INuGenValueTrackerService))
			{
				Debug.Assert(this.ValueTrackerService != null, "this.ValueTrackerService != null");
				return this.ValueTrackerService;
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
		/// Initializes a new instance of the <see cref="NuGenSmoothTrackBarServiceProvider"/> class.
		/// </summary>
		public NuGenSmoothTrackBarServiceProvider()
		{

		}

		#endregion
	}
}
