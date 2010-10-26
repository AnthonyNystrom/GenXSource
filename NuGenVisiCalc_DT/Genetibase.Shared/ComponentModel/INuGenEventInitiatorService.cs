/* -----------------------------------------------
 * INuGenEventInitiatorService.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		/// Invokes the <see cref="T:System.Windows.Forms.DragEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.Windows.Forms.DragEventHandler"/>.
		/// </exception>
		void InvokeDragEventHandler(object key, DragEventArgs e);

		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler`1"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <typeparam name="T"><see cref="T:System.EventArgs"/> inheritor to contain event data.</typeparam>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler`1"/>.
		/// </exception>
		void InvokeEventHandlerT<T>(object key, T e) where T : EventArgs;

		/// <summary>
		/// Invokes the <see cref="T:EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.EventHandler"/>.
		/// </exception>
		void InvokeEventHandler(object key, EventArgs e);

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.KeyEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.Windows.Forms.KeyEventHandler"/>.
		/// </exception>
		void InvokeKeyEventHandler(object key, KeyEventArgs e);

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.KeyPressEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="T:System.Windows.Forms.KeyPressEventHandler"/>.
		/// </exception>
		void InvokeKeyPressEventHandler(object key, KeyPressEventArgs e);

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.MouseEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible with
		/// <see cref="T:System.Windows.Forms.MouseEventHandler"/>.
		/// </exception>
		void InvokeMouseEventHandler(object key, MouseEventArgs e);

		/// <summary>
		/// Invokes the <see cref="T:System.Windows.Forms.PaintEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">
		/// <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible with
		/// <see cref="T:System.Windows.Forms.PaintEventHandler"/>.
		/// </exception>
		void InvokePaintEventHandler(object key, PaintEventArgs e);
		
		/// <summary>
		/// Invokes the <see cref="T:System.EventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.Delegate"/> specified
		/// by the <paramref name="key"/> is not compatible with <see cref="T:System.EventHandler"/>.</exception>
		void InvokePropertyChanged(object key, EventArgs e);
	}
}
