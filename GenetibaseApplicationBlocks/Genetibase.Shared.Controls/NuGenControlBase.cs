/* -----------------------------------------------
 * NuGenControlBase.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.ComponentModel;
using Genetibase.Shared.Controls.ComponentModel;
using Genetibase.Shared.Controls.Properties;
using Genetibase.Shared.Drawing;
using Genetibase.Shared.Windows;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Controls
{
	/// <summary>
	/// </summary>
	[System.ComponentModel.DesignerCategory("Code")]
	public abstract class NuGenControlBase : UserControl
	{
		#region Properties.Services

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator = null;

		/// <summary>
		/// </summary>
		protected INuGenEventInitiatorService Initiator
		{
			get
			{
				if (_initiator == null)
				{
					_initiator = new NuGenEventInitiatorService(this, this.Events);
				}

				return _initiator;
			}
		}

		/*
		 * ServiceProvider
		 */

		private INuGenServiceProvider _serviceProvider;

		/// <summary>
		/// </summary>
		protected INuGenServiceProvider ServiceProvider
		{
			get
			{
				return _serviceProvider;
			}
		}

		/*
		 * StateTracker
		 */

		private INuGenControlStateTracker _stateTracker;

		/// <summary>
		/// </summary>
		/// <exception cref="NuGenServiceNotFoundException"/>
		protected INuGenControlStateTracker StateTracker
		{
			get
			{
				if (_stateTracker == null)
				{
					Debug.Assert(this.ServiceProvider != null, "this.ServiceProvider != null");
					INuGenControlStateService stateService = this.ServiceProvider.GetService<INuGenControlStateService>();

					if (stateService == null)
					{
						throw new NuGenServiceNotFoundException<INuGenControlStateService>();
					}

					_stateTracker = stateService.CreateStateTracker();
					Debug.Assert(_stateTracker != null, "_stateTracker != null");
				}

				return _stateTracker;
			}
		}

		#endregion

		#region Methods.Protected.Overridden

		/*
		 * OnEnabledChanged
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnEnabledChanged(EventArgs e)
		{
			this.StateTracker.Enabled(this.Enabled);
			base.OnEnabledChanged(e);
		}

		/*
		 * OnGotFocus
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.GotFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnGotFocus(EventArgs e)
		{
			this.StateTracker.GotFocus();
			base.OnGotFocus(e);
			this.Invalidate();
		}

		/*
		 * OnLostFocus
		 */

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.LostFocus"></see> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
		protected override void OnLostFocus(EventArgs e)
		{
			this.StateTracker.LostFocus();
			base.OnLostFocus(e);
			this.Invalidate();
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenControlBase"/> class.
		/// </summary>
		/// <param name="serviceProvider">
		/// <para>Requires:</para>
		/// <para><see cref="INuGenControlStateService"/></para>
		/// </param>
		/// <exception cref="ArgumentNullException">
		///	<para>
		///		<paramref name="serviceProvider"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		public NuGenControlBase(INuGenServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
			{
				throw new ArgumentNullException("serviceProvider");
			}

			_serviceProvider = serviceProvider;
		}

		#endregion
	}
}
