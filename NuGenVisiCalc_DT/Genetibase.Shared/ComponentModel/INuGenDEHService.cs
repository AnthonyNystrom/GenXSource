/* -----------------------------------------------
 * INuGenDEHService.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Indicates that this class provides service for Delayed Event Handling infrastructure.
	/// </summary>
	public interface INuGenDEHService
	{
		/// <summary>
		/// </summary>
		void AddClient(INuGenDEHClient clientToAdd);

		/// <summary>
		/// </summary>
		void RemoveClient(INuGenDEHClient clientToRemove);
	}
}
