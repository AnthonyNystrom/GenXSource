/* -----------------------------------------------
 * NuGenEventInitiator.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Represents base functionality for a class that can initiate events.
	/// </summary>
	/// <example><see cref="NuGenEventHandlerListProvider"/></example>
	public class NuGenEventInitiator
	{
		#region Properties.Protected

		/*
		 * Events
		 */

		/// <summary>
		/// Gets the list of handlers for the events defined.
		/// </summary>
		/// <value></value>
		protected EventHandlerList Events
		{
			[DebuggerStepThrough]
			get
			{
				return this.HandlerListProvider.Events;
			}
		}

		#endregion

		#region Properties.Protected.Virtual

		/*
		 * HandlerListProvider
		 */

		private INuGenEventHandlerListProvider _handlerListProvider = null;

		/// <summary>
		/// </summary>
		protected virtual INuGenEventHandlerListProvider HandlerListProvider
		{
			get
			{
				if (_handlerListProvider == null)
				{
					_handlerListProvider = new NuGenEventHandlerListProvider();
				}

				return _handlerListProvider;
			}
		}

		/*
		 * Initiator
		 */

		private INuGenEventInitiatorService _initiator = null;

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

		#endregion

		#region Methods.Protected

		/*
		 * InvokeAction
		 */

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler`1"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.EventArgs"/> inheritor to contain event data.</typeparam>
		/// <param name="key">Specifies the <see cref="T:System.EventHandler`1"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.EventArgs"/> inheritor instance containing the event data.</param>
		/// <exception cref="InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler`1"/>.
		/// </exception>
		protected virtual void InvokeActionT<T>(object key, T e) where T : EventArgs
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeActionT<T>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="EventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="EventArgs"/> instance containing the event data.</param>
		/// <exception cref="InvalidOperationException">
		/// The <see cref="Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="EventHandler"/>.
		/// </exception>
		protected virtual void InvokeAction(object key, EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeAction(key, e);
		}

		/*
		 * InvokeMouseAction
		 */

		/// <summary>
		/// Invokes the <see cref="MouseEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="MouseEventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="MouseEventArgs"/> instance containing the event data.</param>
		/// 
		/// <exception cref="InvalidOperationException">
		/// The <see cref="Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="MouseEventHandler"/>.
		/// </exception>
		protected virtual void InvokeMouseAction(object key, MouseEventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokeMouseAction(key, e);
		}

		/*
		 * InvokePropertyChanged
		 */

		/// <summary>
		/// Invokes the <see cref="EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="EventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.EventArgs"/> inheritor instance containing the event data.</param>
		/// <exception cref="InvalidOperationException">
		/// The <see cref="Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="MouseEventHandler"/>.
		/// </exception>
		protected virtual void InvokePropertyChanged(object key, EventArgs e)
		{
			Debug.Assert(this.Initiator != null, "this.Initiator != null");
			this.Initiator.InvokePropertyChanged(key, e);
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventInitiator"/> class.
		/// </summary>
		protected NuGenEventInitiator()
		{
		}

		#endregion
	}
}
