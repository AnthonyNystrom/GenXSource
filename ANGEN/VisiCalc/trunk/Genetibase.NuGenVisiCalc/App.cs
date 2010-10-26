/* -----------------------------------------------
 * App.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Windows.Forms;

namespace Genetibase.NuGenVisiCalc
{
	internal static class App
	{
		[STAThread]
		static void Main()
		{
			Application.SetCompatibleTextRenderingDefault(false);

			VisiCalcServiceProvider serviceProvider = new VisiCalcServiceProvider();
			MainForm mainForm = new MainForm(serviceProvider);

			Application.EnableVisualStyles();
			Application.Run(mainForm);
		}
	}
}
