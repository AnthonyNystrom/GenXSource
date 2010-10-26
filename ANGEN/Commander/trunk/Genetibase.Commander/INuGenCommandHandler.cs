/* -----------------------------------------------
 * INuGenCommandHandler.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Commander
{
	/// <summary>
	/// Indicates that this class can handle commands.
	/// </summary>
	public interface INuGenCommandHandler
	{
		/// <summary>
		/// Handles the specified command.
		/// </summary>
		/// <param name="cmd">Specifies the command to process.</param>
		void CommandProc(NuGenCommand cmd);
	}
}
