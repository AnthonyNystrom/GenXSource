/* -----------------------------------------------
 * NuGenTargetEventHandler.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
 * --------------------------------------------- */

using System;

namespace Genetibase.Shared
{
	/// <summary>
	/// Represents the method that will handle events with target specific data.
	/// </summary>
	public delegate void NuGenTargetEventHandler(object sender, NuGenTargetEventArgs e);
}
