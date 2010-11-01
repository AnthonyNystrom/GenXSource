/* -----------------------------------------------
 * NuGenEventInitiatorService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Properties;

using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Provides methods for safe event firing.
	/// </summary>
	/// <example><see cref="NuGenEventHandlerListProvider"/></example>
	public class NuGenEventInitiatorService : INuGenEventInitiatorService
	{
		#region Properties.Protected

		/*
		 * Events
		 */

		private EventHandlerList _events;

		/// <summary>
		/// Gets the list of event handlers.
		/// </summary>
		protected EventHandlerList Events
		{
			get
			{
				return _events;
			}
		}

		/*
		 * Sender
		 */

		private object _sender;

		/// <summary>
		/// Gets the event sender.
		/// </summary>
		protected object Sender
		{
			get
			{
				return _sender;
			}
		}

		#endregion

		#region Methods.Public

		/*
		 * InvokeAction
		 */

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler`1"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.EventArgs"/> inheritor to contain event data.</typeparam>
		/// <param name="key">Specifies the <see cref="T:System.EventHandler`1"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.EventArgs"/> inheritor instance containing the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler`1"/>.
		/// </exception>
		public void InvokeActionT<T>(object key, T e) where T : EventArgs
		{
			if (this.Events != null)
			{
				Delegate handler = this.Events[key];

				if (handler is EventHandler<T>)
				{
					((EventHandler<T>)handler)(this.Sender, e);
				}
				else if (handler != null)
				{
					throw new InvalidOperationException(Resources.InvalidOperation_EventHandlerGenericType);
				}
			}
		}

		/// <summary>
		/// Invokes the <see cref="T:EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="T:System.EventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.EventArgs"/> instance containg the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler"/>.
		/// </exception>
		public void InvokeAction(object key, EventArgs e)
		{
			if (this.Events != null)
			{
				Delegate handler = this.Events[key];

				if (handler is EventHandler)
				{
					((EventHandler)handler)(this.Sender, e);
				}
				else if (handler != null)
				{
					throw new InvalidOperationException(Resources.InvalidOperation_EventHandlerType);
				}
			}
		}

		/*
		 * InvokeMouseAction
		 */

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.MouseEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="T:System.Windows.Forms.MouseEventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible with
		/// <see cref="T:System.Windows.Forms.MouseEventHandler"/>.
		/// </exception>
		public void InvokeMouseAction(object key, MouseEventArgs e)
		{
			if (this.Events != null)
			{
				Delegate handler = this.Events[key];

				if (handler is MouseEventHandler)
				{
					((MouseEventHandler)handler)(this.Sender, e);
				}
				else if (handler != null)
				{
					throw new InvalidOperationException(Resources.InvalidOperation_MouseEventHandlerType);
				}
			}
		}

		/*
		 * InvokePropertyChanged
		 */

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="T:System.EventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.EventArgs"/> instance containg the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Delegate"/> specified
		/// by the <paramref name="key"/> is not compatible with <see cref="T:System.EventHandler"/>.</exception>
		public void InvokePropertyChanged(object key, EventArgs e)
		{
			if (this.Events != null)
			{
				Delegate handler = this.Events[key];

				if (handler is EventHandler)
				{
					((EventHandler)handler)(this.Sender, e);
				}
				else if (handler != null)
				{
					throw new InvalidOperationException(Resources.InvalidOperation_EventHandlerType);
				}
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventInitiatorService"/> class.
		/// </summary>
		public NuGenEventInitiatorService(object eventSender, EventHandlerList eventHandlers)
		{
			_sender = eventSender;
			_events = eventHandlers;
		}

		#endregion
	}
}
