/* -----------------------------------------------
 * NuGenDEHEventInitiator.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using Genetibase.Shared;
using Genetibase.Shared.Properties;

using System;
using System.ComponentModel;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Provides methods for safe delayed events firing.
	/// </summary>
	public sealed class NuGenDEHEventInitiator : NuGenEventInitiatorService
	{
		#region Methods.Public

		/// <summary>
		/// Invokes <see cref="NuGenDEHEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="NuGenDEHEventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="INuGenDEHEventArgs"/> inheritor instance containing the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="NuGenDEHEventHandler"/>.
		/// </exception>
		public void InvokeEventToBeDelayed(object key, INuGenDEHEventArgs e)
		{
			Delegate handler = this.Events[key];

			if (handler is NuGenDEHEventHandler)
			{
				((NuGenDEHEventHandler)handler)(this.Sender, e);
			}
			else if (handler != null)
			{
				throw new InvalidOperationException(Resources.InvalidOperation_DEHEventHandlerType);
			}
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDEHEventInitiator"/> class.
		/// </summary>
		public NuGenDEHEventInitiator(object eventSender, EventHandlerList eventHandlers)
			: base(eventSender, eventHandlers)
		{
		}

		#endregion
	}
}
