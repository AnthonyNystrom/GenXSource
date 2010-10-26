/* -----------------------------------------------
 * NuGenWmHandler.cs
 * Copyright © 2006 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.Shared.Windows
{
	/// <summary>
	/// Defines Windows message handler.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="baseWndProc"></param>
	public delegate void NuGenWmHandler(ref Message m, NuGenWndProcDelegate baseWndProc);
}
