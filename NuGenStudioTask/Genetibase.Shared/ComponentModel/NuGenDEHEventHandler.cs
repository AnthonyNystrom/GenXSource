/* -----------------------------------------------
 * NuGenDEHEventHandler.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared.ComponentModel
{
	/// <summary>
	/// Defines a delayed event handler.
	/// </summary>
	public delegate void NuGenDEHEventHandler(object sender, INuGenDEHEventArgs e);
}
