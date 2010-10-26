/* -----------------------------------------------
 * Program.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using appBlocks = Genetibase.ApplicationBlocks;

using Genetibase.WinApi;

using Microsoft.Win32.SafeHandles;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.ComponentModel;
using Genetibase.Shared.Windows;

namespace Genetibase.NuGenPresenter
{
	internal static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			NotifyIconHolder iconHolder = new NotifyIconHolder(new PresenterServiceProvider());
			Application.Run();
		}
	}
}