/* -----------------------------------------------
 * NuGenEventInitiatorService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using Genetibase.Shared.Properties;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Provides methods for safe event firing.
	/// </summary>
	/// <example><see cref="NuGenEventHandlerListProvider"/></example>
	public class NuGenEventInitiatorService : INuGenEventInitiatorService
	{
		private EventHandlerList _events;
		private Object _sender;

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler"/>.
		/// </exception>
		public void InvokeDependencyPropertyChanged(Object key, DependencyPropertyChangedEventArgs e)
		{
			this.InvokeHandler<DependencyPropertyChangedEventHandler, DependencyPropertyChangedEventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler`1"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.EventArgs"/> inheritor to contain event data.</typeparam>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler`1"/>.
		/// </exception>
		public void InvokeEventHandlerT<T>(Object key, T e) where T : EventArgs
		{
			this.InvokeHandler<EventHandler<T>, T>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler"/>.
		/// </exception>
		public void InvokeEventHandler(Object key, EventArgs e)
		{
			this.InvokeHandler<EventHandler, EventArgs>(key, e);
		}

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler"/>.
		/// </exception>
		public void InvokePropertyChanged(Object key, EventArgs e)
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
		protected internal void InvokeHandler<TEventHandler, TEventArgs>(Object key, TEventArgs e)
			where TEventHandler : class
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
							String.Format(CultureInfo.InvariantCulture, Resources.InvalidOperation_HandlerType, handlerType.Name)
						);
					}
				}

			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenEventInitiatorService"/> class.
		/// </summary>
		public NuGenEventInitiatorService(Object eventSender, EventHandlerList eventHandlers)
		{
			_sender = eventSender;
			_events = eventHandlers;
		}
	}
}
