/* -----------------------------------------------
 * NuGenNativeWindow.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.ComponentModel;

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Represents a <see cref="NativeWindow"/> with <see cref="EventHandlerList"/> defined.
	/// </summary>
	public class NuGenNativeWindow : NativeWindow
	{
		#region Properties.Protected

		private NuGenEventHandlerListProvider _eventHandlerList = null;

		/// <summary>
		/// Gets the list of handlers for the events defined.
		/// </summary>
		/// <value></value>
		protected EventHandlerList Events
		{
			get
			{
				if (_eventHandlerList == null)
				{
					_eventHandlerList = new NuGenEventHandlerListProvider();
				}
		
				return _eventHandlerList.Events;
			}
		}

		#endregion

		#region Methods.Protected

		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetControl"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		protected virtual void InitializeTargetControl(Control targetControl)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			if (targetControl.IsHandleCreated)
			{
				this.AssignHandle(targetControl.Handle);
			}
			else
			{
				targetControl.HandleCreated += this.TargetControl_HandleCreated;
			}

			targetControl.HandleDestroyed += this.TargetControl_HandleDestroyed;
		}

		/// <summary>
		/// </summary>
		/// <param name="targetControl"></param>
		/// <exception cref="ArgumentNullException">
		/// <para>
		///		<paramref name="targetControl"/> is <see langword="null"/>.
		/// </para>
		/// </exception>
		protected virtual void ResetTargetControl(Control targetControl)
		{
			if (targetControl == null)
			{
				throw new ArgumentNullException("targetControl");
			}

			targetControl.HandleCreated -= this.TargetControl_HandleCreated;
			targetControl.HandleDestroyed -= this.TargetControl_HandleDestroyed;

			this.ReleaseHandle();
		}

		#endregion

		#region EventHandlers

		private void TargetControl_HandleCreated(object sender, EventArgs e)
		{
			Debug.Assert(sender is Control, "sender is Control");
			this.AssignHandle(((Control)sender).Handle);
		}

		private void TargetControl_HandleDestroyed(object sender, EventArgs e)
		{
			this.ReleaseHandle();
		}

		#endregion

		#region Constructors

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

			this.InitializeTargetControl(targetControl);
		}
		
		#endregion
	}
}
