/* -----------------------------------------------
 * Program.cs
 * Copyright © 2006 Alex Nesterov
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.ApplicationBlocks.Demo
{
	/// <summary>
	/// Runs the application.
	/// </summary>
	class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.Run(new MainForm());
		}
	}
}
