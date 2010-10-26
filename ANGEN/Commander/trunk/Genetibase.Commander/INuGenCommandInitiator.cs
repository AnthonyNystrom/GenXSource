/* -----------------------------------------------
 * INuGenCommandInitiator.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Commander
{
	/// <summary>
	/// Indicates that this class can initiate commands.
	/// </summary>
	public interface INuGenCommandInitiator
	{
		/// <summary>
		/// Occurs when a new command has been initiated.
		/// </summary>
		event NuGenCommandHandler CommandInitiated;
	}
}
