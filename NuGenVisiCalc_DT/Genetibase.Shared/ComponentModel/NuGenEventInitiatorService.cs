/* -----------------------------------------------
 * NuGenEventInitiatorService.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared.Properties;

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Forms;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Provides methods for safe event firing.
	/// </summary>
	/// <example><see cref="NuGenEventHandlerListProvider"/></example>
	public class NuGenEventInitiatorService : INuGenEventInitiatorService
	{
		private EventHandlerList _events;
		private object _sender;

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.DragEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="e"></param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.Windows.Forms.DragEventHandler"/>.
		/// </exception>
		public void InvokeDragEventHandler(object key, DragEventArgs e)
		{
			this.InvokeHandler<DragEventHandler, DragEventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler`1"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.EventArgs"/> inheritor to contain event data.</typeparam>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler`1"/>.
		/// </exception>
		public void InvokeEventHandlerT<T>(object key, T e) where T : EventArgs
		{
			this.InvokeHandler<EventHandler<T>, T>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler"/>.
		/// </exception>
		public void InvokeEventHandler(object key, EventArgs e)
		{
			this.InvokeHandler<EventHandler, EventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.KeyEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.Windows.Forms.KeyEventHandler"/>.
		/// </exception>
		public void InvokeKeyEventHandler(object key, KeyEventArgs e)
		{
			this.InvokeHandler<KeyEventHandler, KeyEventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.KeyPressEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.Windows.Forms.KeyPressEventHandler"/>.
		/// </exception>
		public void InvokeKeyPressEventHandler(object key, KeyPressEventArgs e)
		{
			this.InvokeHandler<KeyPressEventHandler, KeyPressEventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.MouseEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible with
		/// <see cref="T:System.Windows.Forms.MouseEventHandler"/>.
		/// </exception>
		public void InvokeMouseEventHandler(object key, MouseEventArgs e)
		{
			this.InvokeHandler<MouseEventHandler, MouseEventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.PaintEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible with
		/// <see cref="T:System.Windows.Forms.PaintEventHandler"/>.
		/// </exception>
		public void InvokePaintEventHandler(object key, PaintEventArgs e)
		{
			this.InvokeHandler<PaintEventHandler, PaintEventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Delegate"/> specified
		/// by the <paramref name="key"/> is not compatible with <see cref="T:System.EventHandler"/>.</exception>
		public void InvokePropertyChanged(object key, EventArgs e)
		{
			this.InvokeHandler<EventHandler, EventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.Delegate"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="TEventHandler">
		///		<see cref="T:System.Delegate"/> is expected to contain two formal parameters. The first one
		///		should be of type <see cref="T:System.Object"/> while the second should be of type
		///		<see cref="T:System.EventArgs"/> or any derived.
		/// </typeparam>
		/// <typeparam name="TEventArgs">
		///		<see cref="T:System.EventArgs"/> inheritor to contain event data.
		/// </typeparam>
		/// <exception cref="T:System.InvalidOperationException">
		/// <para>
		///		The <see cref="T:System.Delegate"/> of the specified type is expected to contain two
		///		formal parameters. The first one should be of type <see cref="T:System.Object"/> while the
		///		second should be of type <see cref="T:System.EventArgs"/> or any derived.
		/// </para>
		/// -or-
		/// <para>
		///		The type of the event specified by the <paramref name="key"/> should be compatible with the
		///		type specified by <paramref name="TEventHandler"/>.
		/// </para>
		/// </exception>
		protected internal void InvokeHandler<TEventHandler, TEventArgs>(object key, TEventArgs e)
			where TEventHandler : class
			where TEventArgs : EventArgs
		{
			if (_events != null)
			{
				Delegate handler = _events[key];

				if (handler != null)
				{
					Type handlerType = handler.GetType();

					if (handlerType.Equals(typeof(TEventHandler)))
					{
						try
						{
							handler.DynamicInvoke(_sender, e);
						}
						catch (TargetParameterCountException targetParameterCountException)
						{
							throw new InvalidOperationException(
								Resources.InvalidOperation_HandlerParams,
								targetParameterCountException
							);
						}
						catch (ArgumentException argumentException)
						{
							throw new InvalidOperationException(
								Resources.InvalidOperation_HandlerParams,
								argumentException
							);
						}
					}
					else
					{
						throw new InvalidOperationException(
							string.Format(CultureInfo.InvariantCulture, Resources.InvalidOperation_HandlerType, handlerType.Name)
						);
					}
				}

			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventInitiatorService"/> class.
		/// </summary>
		public NuGenEventInitiatorService(object eventSender, EventHandlerList eventHandlers)
		{
			_sender = eventSender;
			_events = eventHandlers;
		}
	}
}
