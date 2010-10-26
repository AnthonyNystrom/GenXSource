/* -----------------------------------------------
 * NuGenDEHEventInitiator.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
		/// <summary>
		/// Invokes <see cref="NuGenDEHEventHandler"/> specified by the <paramref name="key"/>.
		/// </summary>
		/// <param name="key">Specifies the <see cref="NuGenDEHEventHandler"/> to invoke.</param>
		/// <param name="e">Specifies the <see cref="NuGenDEHEventArgs"/> instance containing the event data.</param>
		/// <exception cref="T:System.InvalidOperationException">
		/// The <see cref="T:System.Delegate"/> specified by the <paramref name="key"/> is not compatible
		/// with <see cref="NuGenDEHEventHandler"/>.
		/// </exception>
		public void InvokeEventToBeDelayed(object key, NuGenDEHEventArgs e)
		{
			this.InvokeHandler<NuGenDEHEventHandler, NuGenDEHEventArgs>(key, e);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NuGenDEHEventInitiator"/> class.
		/// </summary>
		public NuGenDEHEventInitiator(object eventSender, EventHandlerList eventHandlers)
			: base(eventSender, eventHandlers)
		{
		}
	}
}
