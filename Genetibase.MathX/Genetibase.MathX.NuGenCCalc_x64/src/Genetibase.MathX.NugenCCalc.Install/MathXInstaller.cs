using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.IO;
using System.Reflection;
using System.Windows.Forms; 

namespace Genetibase.MathX.NugenCCalc.Install
{
	/// <summary>Installer for install component to toolbox.</summary>
	/// <remarks>
	/// As parameters for this installer must be send install path and Visual Studio
	/// version.
	/// </remarks>
	[RunInstaller(true)]
	[ToolboxItem(false)]
	public class MathXInstaller : System.Configuration.Install.Installer
	{
		private const int vsToolBoxItemFormatDotNETComponent = 8;
		private const string vsWindowKindToolbox = "{B1E99781-AB81-11D0-B683-00AA00A3EE26}";
 
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public MathXInstaller()
		{
			// This call is required by the Designer.
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion


		public override void Install(IDictionary stateSaver)
		{
			base.Install (stateSaver);

			try
			{
				string assembliesPath = (string)Context.Parameters["InstallPath"]+"bin";
				object devEnv = null;
				do 
				{
					try
					{
						switch((string)Context.Parameters["VSversion"])
						{
							case "2003":
								devEnv = System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.7.1");
								break;
							case "2005":
								devEnv = System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.8.0");
								break;
						}
					}
					catch
					{
						devEnv = null;
					}
					if (devEnv != null)
					{
						if (MessageBox.Show("Setup has found that MS Visual Studio .NET is running. Please close it and click Retry button to continue the installation progress. To skip this operation click Cancel.", 
							"Setup Wizard", 
							MessageBoxButtons.RetryCancel, 
							MessageBoxIcon.Warning) == DialogResult.Cancel)
						{
							return;
						}
					}
				}
				while (devEnv != null);

				if (devEnv == null)
				{
					try
					{
						switch((string)Context.Parameters["VSversion"])
						{
							case "2003":
								System.Type vs70Type = System.Type.GetTypeFromProgID("VisualStudio.DTE.7.1");
								devEnv = System.Activator.CreateInstance(vs70Type, true);
								break;
							case "2005":
								System.Type vs80Type = System.Type.GetTypeFromProgID("VisualStudio.DTE.8.0");
								devEnv = System.Activator.CreateInstance(vs80Type, true);
								break;
						}
					}
					catch
					{
						return;
					}
				}

				// Make sure the properties window is visible. This gets around a VS.Net bug when installing 
				// toolbox icons 
				devEnv.GetType().InvokeMember("ExecuteCommand", BindingFlags.InvokeMethod,
					null, devEnv, new object[]{"View.PropertiesWindow", string.Empty});

				object windows = devEnv.GetType().InvokeMember("Windows", BindingFlags.GetProperty,
					null, devEnv, null);
				object window = windows.GetType().InvokeMember("Item", BindingFlags.InvokeMethod,
					null, windows, new object[]{vsWindowKindToolbox});

				object toolbox = window.GetType().InvokeMember("Object", BindingFlags.GetProperty,
					null, window, null);
				object tabs = toolbox.GetType().InvokeMember("ToolBoxTabs", BindingFlags.GetProperty,
					null, toolbox, null);

				foreach(object t in (IEnumerable)tabs)
				{
					string tabName = (string)t.GetType().InvokeMember("Name", BindingFlags.GetProperty,
						null, t, null);

					if (tabName == "Genetibase") 
					{
						t.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod,
							null, t, null);
					}
					ReleaseCOMObject(t);
				}

				// Add the "Genetibase" tab 
				object newTab = tabs.GetType().InvokeMember("Add", BindingFlags.InvokeMethod,
					null, tabs, new object[]{"Genetibase"});
				// Activate the "Genetibase" tab 
				newTab.GetType().InvokeMember("Activate", BindingFlags.InvokeMethod,
					null, newTab, null);
				assembliesPath = Path.Combine(assembliesPath, "Genetibase.MathX.NugenCCalc.dll"); 
				// Add controls to the "Genetibase" tab 
				object newTabItems = newTab.GetType().InvokeMember("ToolBoxItems", BindingFlags.GetProperty,
					null, newTab, null);
				newTabItems.GetType().InvokeMember("Add", BindingFlags.InvokeMethod,
					null, newTabItems, new object[]{"MathX", assembliesPath, 
													   vsToolBoxItemFormatDotNETComponent});


				devEnv.GetType().InvokeMember("Quit", BindingFlags.InvokeMethod,
					null, devEnv, null);

				ReleaseCOMObject(newTabItems);
				ReleaseCOMObject(newTab);
				ReleaseCOMObject(tabs);
				ReleaseCOMObject(toolbox);
				ReleaseCOMObject(window);
				ReleaseCOMObject(windows);
				ReleaseCOMObject(devEnv);
			}
			catch//(Exception ex)
			{
				//MessageBox.Show(ex.Message);
			}
		}

		public override void Uninstall(IDictionary savedState)
		{
			if (savedState != null)
			{
				base.Uninstall (savedState);
				try
				{
					string assembliesPath = (string)Context.Parameters["InstallPath"]+"bin";
					object devEnv = null;

					if (devEnv == null)
					{
						try
						{
							System.Type vs70Type = System.Type.GetTypeFromProgID("VisualStudio.DTE.7.1");
							devEnv = System.Activator.CreateInstance(vs70Type, true);
						}
						catch
						{
							return;
						}
					}

					// Make sure the properties window is visible. This gets around a VS.Net bug when installing 
					// toolbox icons 
					devEnv.GetType().InvokeMember("ExecuteCommand", BindingFlags.InvokeMethod,
						null, devEnv, new object[]{"View.PropertiesWindow", string.Empty});

					object windows = devEnv.GetType().InvokeMember("Windows", BindingFlags.GetProperty,
						null, devEnv, null);
					object window = windows.GetType().InvokeMember("Item", BindingFlags.InvokeMethod,
						null, windows, new object[]{vsWindowKindToolbox});

					object toolbox = window.GetType().InvokeMember("Object", BindingFlags.GetProperty,
						null, window, null);
					object tabs = toolbox.GetType().InvokeMember("ToolBoxTabs", BindingFlags.GetProperty,
						null, toolbox, null);

					foreach(object t in (IEnumerable)tabs)
					{
						string tabName = (string)t.GetType().InvokeMember("Name", BindingFlags.GetProperty,
							null, t, null);

						if (tabName == "Genetibase") 
						{
							t.GetType().InvokeMember("Delete", BindingFlags.InvokeMethod,
								null, t, null);
						}
						ReleaseCOMObject(t);
					}

					ReleaseCOMObject(tabs);
					ReleaseCOMObject(toolbox);
					ReleaseCOMObject(window);
					ReleaseCOMObject(windows);
					ReleaseCOMObject(devEnv);
				}
				catch//(Exception ex)
				{
					//MessageBox.Show(ex.Message);
				}
			}
		}

		private void ReleaseCOMObject(object obj)
		{
			try
			{
				Marshal.ReleaseComObject(obj);
			}
			catch{}
			finally
			{
				obj = null;
			}
		}


	}
}
