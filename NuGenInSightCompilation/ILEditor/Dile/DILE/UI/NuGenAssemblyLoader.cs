using System;
using System.Collections.Generic;
using System.Text;

using Dile.Debug;
using Dile.Disassemble;
using Dile.Metadata;
using System.IO;
using System.Threading;

namespace Dile.UI
{	
	public class NuGenAssemblyLoader
	{
		public event AssembliesLoadedDelegate AssembliesLoaded;

		private static NuGenAssemblyLoader instance = new NuGenAssemblyLoader();
		public static NuGenAssemblyLoader Instance
		{
			get
			{
				return instance;
			}
		}

		private NuGenAssemblyLoader()
		{
		}

		public void LoadInMemoryModule(ModuleWrapper inMemoryModule)
		{
			List<NuGenAssembly> loadedAssemblies = new List<NuGenAssembly>(1);

			try
			{
				string moduleName = inMemoryModule.GetName();

				try
				{
					moduleName = Path.GetFileNameWithoutExtension(moduleName);
				}
				catch
				{
				}

				NuGenAssembly inMemoryAssembly = new NuGenAssembly(true);
				inMemoryAssembly.FileName = moduleName;
				inMemoryAssembly.Name = moduleName;
				NuGenMetaDataDispenserEx dispenser = new NuGenMetaDataDispenserEx();

				inMemoryAssembly.LoadAssemblyFromMetadataInterfaces(dispenser, (NuGenIMetaDataAssemblyImport)inMemoryModule.GetMetaDataAssemblyImport(), (NuGenIMetaDataImport2)inMemoryModule.GetMetaDataImport2(), inMemoryModule);
				loadedAssemblies.Add(inMemoryAssembly);
			}
			catch (Exception exception)
			{
				NuGenUIHandler.Instance.DisplayUserWarning("An error occurred while loading the assembly.");
				NuGenUIHandler.Instance.ShowException(exception);
			}

			NuGenUIHandler.Instance.ResetProgressBar();

			if (loadedAssemblies.Count > 0)
			{
				NuGenUIHandler.Instance.AssembliesLoaded(loadedAssemblies, false);

				if (AssembliesLoaded != null)
				{
					AssembliesLoaded(loadedAssemblies, false);
				}
			}
		}

		public void Start(string[] fileNames)
		{
			Start((object)fileNames);
		}

		private void Start(object parameter)
		{
			string[] fileNames = (string[])parameter;
			List<NuGenAssembly> loadedAssemblies = new List<NuGenAssembly>(fileNames.Length);
			bool isProjectChanged = false;

			foreach (string fileName in fileNames)
			{
				try
				{
					if (fileName == null || fileName.Length == 0)
					{
						NuGenUIHandler.Instance.SetProgressText(fileName, "File not found.", false);
					}
					else
					{
						NuGenAssembly assembly = new NuGenAssembly(false);
						assembly.FullPath = fileName;
						assembly.LoadAssembly();

						if (assembly != null)
						{
							loadedAssemblies.Add(assembly);
						}
					}
				}
				catch (Exception exception)
				{
					isProjectChanged = true;
					NuGenUIHandler.Instance.DisplayUserWarning("An error occurred while loading the assembly.");
					NuGenUIHandler.Instance.ShowException(exception);
				}

				NuGenUIHandler.Instance.SetProgressText("\n\n", true);
			}

			NuGenUIHandler.Instance.ResetProgressBar();
			NuGenUIHandler.Instance.AssembliesLoaded(loadedAssemblies, isProjectChanged);

			if (AssembliesLoaded != null)
			{
				AssembliesLoaded(loadedAssemblies, isProjectChanged);
			}
		}
	}
}