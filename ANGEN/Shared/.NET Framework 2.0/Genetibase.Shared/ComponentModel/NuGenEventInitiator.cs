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
	public abstract class NuGenEventInitiator : IDisposable
	{
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

		private INuGenEventHandlerListProvider _handlerListProvider;

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
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			this.Dispose(true);
		}

		/// <summary>
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_handlerListProvider != null)
				{
					_handlerListProvider.Dispose();
					_handlerListProvider = null;
				}
			}
		}
	}
}
