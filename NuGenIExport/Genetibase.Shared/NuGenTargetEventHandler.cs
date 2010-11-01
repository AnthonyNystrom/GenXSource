/* -----------------------------------------------
 * NuGenTargetEventHandler.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared
{
	/// <summary>
	/// Represents the method that will handle events with target specific data.
	/// </summary>
	public delegate void NuGenTargetEventHandler(object sender, NuGenTargetEventArgs e);
}
