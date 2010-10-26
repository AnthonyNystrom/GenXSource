/* -----------------------------------------------
 * NuGenNativeWindow.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Represents a <see cref="NativeWindow"/> with <see cref="EventHandlerList"/> defined.
	/// </summary>
	[SecurityPermission(SecurityAction.LinkDemand)]
	[SecurityPermission(SecurityAction.InheritanceDemand)]
	public class NuGenNativeWindow : NativeWindow, IDisposable
	{
		/// <summary>
		/// Gets the list of handlers for the events defined.
		/// </summary>
		/// <value></value>
		protected EventHandlerList Events
		{
			get
			{
				return this.EventHandlerListProvider.Events;
			}
		}

		private INuGenEventHandlerListProvider _eventHandlerListProvider;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventHandlerListProvider EventHandlerListProvider
		{
			get
			{
				if (_eventHandlerListProvider == null)
				{
					_eventHandlerListProvider = new NuGenEventHandlerListProvider();
				}

				return _eventHandlerListProvider;
			}
		}

		private INuGenEventInitiatorService _initiator;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventInitiatorService Initiator
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

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="targetControl"/> is <see langword="null"/>.</para></exception>
		protected void InitializeTargetControl(Control targetControl)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			if (targetControl.IsHandleCreated)
			{
				this.AssignHandle(targetControl.Handle);
			}
			
			targetControl.HandleCreated += _targetControl_HandleCreated;
			targetControl.HandleDestroyed += _targetControl_HandleDestroyed;
		}

		/// <summary>
		/// </summary>
		/// <exception cref="ArgumentNullException"><para><paramref name="targetControl"/> is <see langword="null"/>.</para></exception>
		protected void ResetTargetControl(Control targetControl)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			targetControl.HandleCreated -= _targetControl_HandleCreated;
			targetControl.HandleDestroyed -= _targetControl_HandleDestroyed;

			this.ReleaseHandle();
		}

		private void _targetControl_HandleCreated(object sender, EventArgs e)
		{
			Debug.Assert(sender is Control, "sender is Control");
			this.AssignHandle(((Control)sender).Handle);
		}

		private void _targetControl_HandleDestroyed(object sender, EventArgs e)
		{
			this.ReleaseHandle();
		}

		private Control _targetControl;

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNativeWindow"/> class.
		/// </summary>
		public NuGenNativeWindow()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenNativeWindow"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="targetControl"/> is <see langword="null"/>.
		/// </exception>
		public NuGenNativeWindow(Control targetControl)
			: this()
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			_targetControl = targetControl;
			this.InitializeTargetControl(_targetControl);
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to dispose both managed and unmanaged resources; <see langword="false"/> to dispose only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_eventHandlerListProvider != null)
				{
					_eventHandlerListProvider.Dispose();
					_eventHandlerListProvider = null;
				}

				if (_targetControl != null)
				{
					this.ResetTargetControl(_targetControl);
				}
			}
		}
	}
}
