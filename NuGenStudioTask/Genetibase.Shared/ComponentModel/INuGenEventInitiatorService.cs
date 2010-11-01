/* -----------------------------------------------
 * INuGenEventInitiatorService.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Indicates that this class provides methods for safe event firing.
	/// </summary>
	public interface INuGenEventInitiatorService
	{
		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler`1"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.EventArgs"/> inheritor to contain event data.</typeparam>
		/// <param name="key">Specifies the <see cref="T:System.EventHandler`1"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.EventArgs"/> inheritor instance containing the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Delegate"/> specified
		/// by the <paramref name="key"/> is not compatible with <see cref="T:System.EventHandler`1"/>.</exception>
		void InvokeActionT<T>(object key, T e) where T : EventArgs;

		/// <summary>
		/// Invokes the <see cref="T:EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="T:System.EventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.EventArgs"/> instance containg the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Delegate"/> specified
		/// by the <paramref name="key"/> is not compatible with <see cref="T:System.EventHandler"/>.</exception>
		void InvokeAction(object key, EventArgs e);

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.MouseEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="T:System.Windows.Forms.MouseEventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Delegate"/> specified
		/// by the <paramref name="key"/> is not compatible with <see cref="T:System.Windows.Forms.MouseEventHandler"/>.</exception>
		void InvokeMouseAction(object key, MouseEventArgs e);

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="T:System.EventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="T:System.EventArgs"/> instance containg the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Delegate"/> specified
		/// by the <paramref name="key"/> is not compatible with <see cref="T:System.EventHandler"/>.</exception>
		void InvokePropertyChanged(object key, EventArgs e);
	}
}
