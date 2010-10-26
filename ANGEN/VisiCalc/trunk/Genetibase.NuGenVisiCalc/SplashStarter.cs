/* -----------------------------------------------
 * SplashStarter.cs
 * Copyright © 2007 Alex Nesterov
 * mailto:a.nesterov@genetibase.com
 * --------------------------------------------- */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Genetibase.NuGenVisiCalc
{
	internal sealed class SplashStarter
	{
		public void HideSplashScreen()
		{
			if (IsShowing)
			{
				_thread.Abort();
				_thread = null;
			}
		}

		public void ShowSplashScreen()
		{
			_thread = new Thread(ThreadEntryPoint);
			_thread.IsBackground = true;
			_thread.Start();
		}

		private void ThreadEntryPoint()
		{
			try
			{
				_splashForm = new SplashForm();
				_splashForm.ShowDialog();
			}
			finally
			{
				_splashForm.Close();
			}
		}

		private Boolean IsShowing
		{
			get
			{
				return _splashForm != null;
			}
		}

		private SplashForm _splashForm;
		private Thread _thread;

		public SplashStarter()
		{
		}
	}
}
