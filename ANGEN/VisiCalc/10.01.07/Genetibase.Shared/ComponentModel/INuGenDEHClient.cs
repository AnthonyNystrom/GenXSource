/* -----------------------------------------------
 * INuGenDEHClient.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Indicates that this class can use <see cref="INuGenDEHService"/>.
	/// </summary>
	public interface INuGenDEHClient
	{
		/// <summary>
		/// <see cref="INuGenDEHService"/> listens to this event and calls <see cref="M:HandleDelayedEvent"/>
		/// method if there were no more <see cref="E:EventToBeDelayed"/> events for the specified interval.
		/// </summary>
		event NuGenDEHEventHandler EventToBeDelayed;
		
		/// <summary>
		/// </summary>
		void HandleDelayedEvent(object sender, NuGenDEHEventArgs e);
	}
}
