/* -----------------------------------------------
 * INuGenEventInitiatorService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Indicates that this class provides methods for safe event firing.
	/// </summary>
	public interface INuGenEventInitiatorService
	{
		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler"/>.
		/// </exception>
		void InvokeDependencyPropertyChanged(Object key, DependencyPropertyChangedEventArgs e);

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler`1"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.EventArgs"/> inheritor to contain event data.</typeparam>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler`1"/>.
		/// </exception>
		void InvokeEventHandlerT<T>(Object key, T e) where T : EventArgs;

		/// <summary>
		/// Invokes the <see cref="T:EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler"/>.
		/// </exception>
		void InvokeEventHandler(Object key, EventArgs e);
		
		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Delegate"/> specified
		/// by the <paramref name="key"/> is not compatible with <see cref="T:System.EventHandler"/>.</exception>
		void InvokePropertyChanged(Object key, EventArgs e);
	}
}
