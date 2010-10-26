/* -----------------------------------------------
 * NuGenCommandHandler.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Text;

namespace Genetibase.Commander
{
	/// <summary>
	/// Defines method that can handle <see cref="T:NuGenCommand"/>.
	/// </summary>
	public delegate void NuGenCommandHandler(NuGenCommand cmd);
}
