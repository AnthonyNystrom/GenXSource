/* -----------------------------------------------
 * INuGenEventHandlerListProvider.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.ComponentModel;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Indicates that this class provides a list of handlers for classes that fire events.
	/// </summary>
	public interface INuGenEventHandlerListProvider : IDisposable
	{
		/// <summary>
		/// </summary>
		EventHandlerList Events
		{
			get;
		}
	}
}
