using config = SampleFramework.Properties.Settings;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace SampleFramework
{
	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		private static void Main(String[] args)
		{
			if (args.Length == 0)
			{
				args = new String[] { config.Default.DefaultSamplesLocation };
			}

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			WndServiceProvider serviceProvider = new WndServiceProvider();

			if (args.Length > 0)
			{
				ISampleFolderDescriptor sampleFolderDescriptor = serviceProvider.GetService<ISampleFolderDescriptor>();
				Debug.Assert(sampleFolderDescriptor != null, "sampleFolderDescriptor != null");

				sampleFolderDescriptor.CSProjectExtension = config.Default.CSProjectExtension;
				sampleFolderDescriptor.CSProjectFolderName = config.Default.CSProjectFolderName;
				sampleFolderDescriptor.DescriptionFileName = config.Default.DescriptionFileName;
				sampleFolderDescriptor.Path = args[0];
				sampleFolderDescriptor.ScreenshotFileName = config.Default.ScreenshotFileName;
				sampleFolderDescriptor.VBProjectExtension = config.Default.VBProjectExtension;
				sampleFolderDescriptor.VBProjectFolderName = config.Default.VBProjectFolderName;
			}

			// Application.Run(new Browser(serviceProvider));
			Application.Run(new Dialog());
		}
	}
}
