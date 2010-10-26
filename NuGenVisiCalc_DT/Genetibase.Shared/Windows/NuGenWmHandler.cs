/* -----------------------------------------------
 * NuGenWmHandler.cs
 * Copyright © 2006 Anthony Nystrom
 * mailto:a.nystrom@genetibase.com
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
