using System;
using System.Collections ;
using System.IO;
using System.Reflection;
using Genetibase.MathX.NugenCCalc.Adapters.IChartAdapter;

namespace Genetibase.MathX.NugenCCalc
{
	/// <summary>
	/// Provide methods for loading chart adapters
	/// </summary>
	public class AdaptersLoader
	{
		/// <summary>
		/// Load adapters assemblies located in the given directory.
		/// </summary>
		public static Assembly[] LoadAdapters(string path)
		{
			ArrayList assemblies = new ArrayList();
			if (Directory.Exists(path))
			{
				foreach (string file in Directory.GetFiles(path, "*.dll"))
				{
					try
					{
						Assembly assembly = Assembly.LoadFrom(file);
						ChartAdapterAttribute adapterAttibute = (ChartAdapterAttribute)Attribute.GetCustomAttribute(assembly, typeof(ChartAdapterAttribute));
						if (adapterAttibute != null)
						{
							assemblies.Add(assembly);
						}
					}
					catch(Exception)// ex)
					{
						//System.Windows.Forms.MessageBox.Show("LoadAdapters"+ex.Message);
					}
				}
			}

			return (Assembly[])assemblies.ToArray(typeof(Assembly));
		}


		/// <summary>
		/// Load adapters assemblies by names
		/// </summary>
		public static Assembly[] LoadAdapters(string[] assemblyNames)
		{

			ArrayList assemblies = new ArrayList();
			foreach (string file in assemblyNames)
			{
				try
				{
					Assembly assembly = Assembly.LoadWithPartialName(file);
					ChartAdapterAttribute adapterAttibute = (ChartAdapterAttribute)Attribute.GetCustomAttribute(assembly, typeof(ChartAdapterAttribute));
					if (adapterAttibute != null)
					{
						assemblies.Add(assembly);
					}
				}
				catch(Exception)// ex)
				{
					//System.Windows.Forms.MessageBox.Show("LoadAdapters"+ex.Message);
				}

			}
			return (Assembly[])assemblies.ToArray(typeof(Assembly));
		}


		/// <summary>
		/// Load adapters from given assemblies
		/// </summary>
		public static IChartAdapter[] GetCustomAdapters(Assembly[] assemblies, Type adapterInterface)
		{
			ArrayList adapters = new ArrayList();
			foreach (Assembly assembly in assemblies)
			{
				try
				{
					Type[] types = assembly.GetTypes();
					foreach (Type t in types)
					{
						if(t.IsClass)
						{
							if (Array.IndexOf(t.GetInterfaces(), adapterInterface) > -1)
							{
								adapters.Add(Activator.CreateInstance(t));
							}
						}
					}
				}
				catch(Exception)// ex)
				{
					//System.Windows.Forms.MessageBox.Show("GetCustomAdapters"+ex.Message);
				}
			}

			return (IChartAdapter[])adapters.ToArray(typeof(IChartAdapter));
		}

	}
}
