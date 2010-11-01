using System;
using System.Collections.Generic;
using System.Text;

using Dile.Configuration;
using Dile.Debug;
using Dile.UI;
using Dile.UI.Debug;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Dile.Disassemble
{
	[Serializable()]
	public class NuGenProject : IDisposable, ISerializable
	{
		public static event EventHandler ProjectChanged;
		public static event EventHandler ProjectIsSavedChanged;

		#region Singleton pattern
		private static NuGenProject instance;
		public static NuGenProject Instance
		{
			get
			{
				return instance;
			}
			set
			{
				if (instance != null)
				{
					instance.Dispose();
				}

				instance = value;

				if (instance != null)
				{
					instance.AssociateBreakpointsWithMethods();
				}

				if (ProjectChanged != null)
				{
					ProjectChanged(value, new EventArgs());
				}
			}
		}
		#endregion

		public const int DefaultArrayCount = 16;
		private const int DefaultCharArrayCount = 1024;

		private static char[] defaultCharArray = new char[DefaultCharArrayCount];
		public static char[] DefaultCharArray
		{
			get
			{
				return defaultCharArray;
			}
			set
			{
				if (value.Length < defaultCharArray.Length)
				{
					throw new InvalidOperationException("Shorter array cannot be set.");
				}

				defaultCharArray = value;
			}
		}

		private List<NuGenAssembly> assemblies = new List<NuGenAssembly>();
		public List<NuGenAssembly> Assemblies
		{
			get
			{
				return assemblies;
			}
			set
			{
				SetSearchOptions(assemblies, value);
				assemblies = value;
				startupAssembly = null;

				SetStartupAssembly();
			}
		}

		private string name = "New project";
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				name = value;
			}
		}

		private bool isSaved = false;
		[XmlIgnore()]
		public bool IsSaved
		{
			get
			{
				return (isSaved && (FullPath.Length > 0 || Assemblies.Count == 0));
			}
			set
			{
				isSaved = value;

				if (ProjectIsSavedChanged != null)
				{
					ProjectIsSavedChanged(null, new EventArgs());
				}
			}
		}

		private string fullPath = string.Empty;
		[XmlIgnore()]
		public string FullPath
		{
			get
			{
				return fullPath;
			}
			set
			{
				fullPath = value;
			}
		}

		private NuGenAssembly startupAssembly = null;
		[XmlIgnore()]
		public NuGenAssembly StartupAssembly
		{
			get
			{
				return startupAssembly;
			}
			set
			{
				startupAssembly = value;

				if (value == null)
				{
					startupAssemblyPath = string.Empty;
				}
				else
				{
					startupAssemblyPath = startupAssembly.FullPath;
				}
			}
		}

		private string startupAssemblyPath = string.Empty;
		public string StartupAssemblyPath
		{
			get
			{
				return startupAssemblyPath;
			}
			set
			{
				startupAssemblyPath = value;

				SetStartupAssembly();
			}
		}

		private List<NuGenExceptionInformation> exceptions = new List<NuGenExceptionInformation>();
		public List<NuGenExceptionInformation> Exceptions
		{
			get
			{
				return exceptions;
			}
			set
			{
				exceptions = value;
			}
		}

		private List<NuGenFunctionBreakpointInformation> functionBreakpoints = new List<NuGenFunctionBreakpointInformation>();
		public List<NuGenFunctionBreakpointInformation> FunctionBreakpoints
		{
			get
			{
				return functionBreakpoints;
			}
			set
			{
				functionBreakpoints = value;
			}
		}

		private List<SuspendableDebugEvent> suspendingDebugEvents = new List<SuspendableDebugEvent>();
		public List<SuspendableDebugEvent> SuspendingDebugEvents
		{
			get
			{
				return suspendingDebugEvents;
			}
			set
			{
				suspendingDebugEvents = value;
			}
		}

		private ProjectStartMode startMode;
		public ProjectStartMode StartMode
		{
			get
			{
				return startMode;
			}
			set
			{
				startMode = value;
			}
		}

		private string assemblyArguments;
		public string AssemblyArguments
		{
			get
			{
				return assemblyArguments;
			}
			set
			{
				assemblyArguments = value;
			}
		}

		private string assemblyWorkingDirectory;
		public string AssemblyWorkingDirectory
		{
			get
			{
				return assemblyWorkingDirectory;
			}
			set
			{
				assemblyWorkingDirectory = value;
			}
		}

		private string programExecutable;
		public string ProgramExecutable
		{
			get
			{
				return programExecutable;
			}
			set
			{
				programExecutable = value;
			}
		}

		private string programArguments;
		public string ProgramArguments
		{
			get
			{
				return programArguments;
			}
			set
			{
				programArguments = value;
			}
		}

		private string programWorkingDirectory;
		public string ProgramWorkingDirectory
		{
			get
			{
				return programWorkingDirectory;
			}
			set
			{
				programWorkingDirectory = value;
			}
		}

		#region ASP.NET Debugging
		//private string browserUrl;
		//public string BrowserUrl
		//{
		//  get
		//  {
		//    return browserUrl;
		//  }
		//  set
		//  {
		//    browserUrl = value;
		//  }
		//}

		//private bool autoAttachToAspNet;
		//public bool AutoAttachToAspNet
		//{
		//  get
		//  {
		//    return autoAttachToAspNet;
		//  }
		//  set
		//  {
		//    autoAttachToAspNet = value;
		//  }
		//}
		#endregion

		private NuGenFunctionBreakpointInformation runToCursorBreakpoint;
		[XmlIgnore()]
		public NuGenFunctionBreakpointInformation RunToCursorBreakpoint
		{
			get
			{
				return runToCursorBreakpoint;
			}
			set
			{
				runToCursorBreakpoint = value;
			}
		}

		public NuGenProject()
		{
		}

        private List<String> assembliesStrings = new List<string>();

        public List<String> AssembliesStrings
        {
            get
            {
                return assembliesStrings;
            }           
       }

        public NuGenProject(SerializationInfo info, StreamingContext ctxt)
        {            
            name = (String)info.GetValue("Name", typeof(string));

            int nAssemblies = (int)info.GetValue("Assemblies", typeof(int));

            for (int i = 0; i < nAssemblies; i++)
            {
                assembliesStrings.Add((String)info.GetValue("Assembly" + i, typeof(string)));
            }
        }

        private void AssociateBreakpointsWithMethods()
		{
			if (FunctionBreakpoints != null && FunctionBreakpoints.Count > 0 && Assemblies != null)
			{
				if (Assemblies.Count > 0)
				{
					int index = 0;

					while (index < FunctionBreakpoints.Count)
					{
						NuGenFunctionBreakpointInformation functionBreakpoint = FunctionBreakpoints[index];

						if (functionBreakpoint.AssociateWithMethod())
						{
							index++;
						}
						else
						{
							FunctionBreakpoints.Remove(functionBreakpoint);
						}
					}
				}
				else
				{
					FunctionBreakpoints.Clear();
				}
			}

			if (RunToCursorBreakpoint != null)
			{
				if (!RunToCursorBreakpoint.AssociateWithMethod())
				{
					RunToCursorBreakpoint.Remove();
					RunToCursorBreakpoint = null;
				}
			}
		}

		private void SetStartupAssembly()
		{
			if (StartupAssemblyPath == string.Empty)
			{
				StartupAssembly = null;
			}
			else if (Assemblies != null && Assemblies.Count > 0 && StartupAssembly == null)
			{
				NuGenAssembly foundAssembly = FindAssemblyByFullPath(Assemblies, StartupAssemblyPath);

				if (foundAssembly != null)
				{
					StartupAssembly = foundAssembly;
				}
			}
		}

		private NuGenAssembly FindAssemblyByFullPath(List<NuGenAssembly> assemblies, string assemblyFullPath)
		{
			NuGenAssembly result = null;
			assemblyFullPath = assemblyFullPath.ToUpperInvariant();
			int index = 0;

			while (result == null && index < assemblies.Count)
			{
				NuGenAssembly assembly = assemblies[index++];

				if (assembly.FullPath.ToUpperInvariant() == assemblyFullPath)
				{
					result = assembly;
				}
			}

			return result;
		}

		private void SetSearchOptions(List<NuGenAssembly> oldAssemblies, List<NuGenAssembly> newAssemblies)
		{
			if (oldAssemblies != null && oldAssemblies.Count > 0 && newAssemblies != null && newAssemblies.Count > 0)
			{
				foreach (NuGenAssembly oldAssembly in oldAssemblies)
				{
					NuGenAssembly newAssembly = FindAssemblyByFullPath(newAssemblies, oldAssembly.FullPath);

					if (newAssembly != null)
					{
						newAssembly.SearchOptions = oldAssembly.SearchOptions;
					}
				}
			}
		}

		public bool IsAssemblyLoaded(string assemblyPath)
		{
			bool result = false;

			if (Assemblies != null)
			{
				int index = 0;

				while (!result && index < Assemblies.Count)
				{
					NuGenAssembly assembly = Assemblies[index++];

					if ((assembly.IsInMemory && assembly.Name.Equals(assemblyPath, StringComparison.InvariantCulture)) || (!assembly.IsInMemory && assembly.FullPath.Equals(assemblyPath, StringComparison.OrdinalIgnoreCase)))
					{
						result = true;
					}
				}
			}

			return result;
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (Assemblies != null && Assemblies.Count > 0)
			{
				foreach (NuGenAssembly assembly in Assemblies)
				{
					assembly.Dispose();
				}

				Assemblies = null;
			}
		}

		#endregion

		public void Save()
		{
			if (FullPath != null && FullPath.Length > 0)
			{
				Save(FullPath);
			}
		}

		public void Save(string path)
		{
			List<NuGenAssembly> inMemoryAssemblies = new List<NuGenAssembly>();

			if (Assemblies != null && Assemblies.Count > 0)
			{
				int index = 0;

				while (index < Assemblies.Count)
				{
					NuGenAssembly assembly = Assemblies[index];

					if (assembly.IsInMemory)
					{
						inMemoryAssemblies.Add(assembly);
						Assemblies.Remove(assembly);
					}
					else
					{
						index++;
					}
				}
			}

			using (FileStream projectFile = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				XmlSerializer serializer = new XmlSerializer(typeof(NuGenProject));
				serializer.Serialize(projectFile, this);
				FullPath = path;
				IsSaved = true;
			}

			if (inMemoryAssemblies.Count > 0)
			{
				foreach (NuGenAssembly inMemoryAssembly in inMemoryAssemblies)
				{
					Assemblies.Add(inMemoryAssembly);
				}
			}
		}

		public bool SkipException(string assemblyPath, uint exceptionClassToken, uint throwingMethodToken, uint? currentIP)
		{
			bool result = false;
			int index = 0;

			while (!result && index < Exceptions.Count)
			{
				result = Exceptions[index++].Equals(assemblyPath, exceptionClassToken, throwingMethodToken, currentIP);
			}

			return result;
		}

		public NuGenFunctionBreakpointInformation FindFunctionBreakpoint(NuGenMethodDefinition methodDefinition, uint offset)
		{
			NuGenFunctionBreakpointInformation result = null;
			int index = 0;

			while (result == null && index < NuGenProject.Instance.FunctionBreakpoints.Count)
			{
				NuGenFunctionBreakpointInformation functionBreakpoint = NuGenProject.Instance.FunctionBreakpoints[index++];

				if (functionBreakpoint.MethodDefinition == methodDefinition && functionBreakpoint.Offset == offset)
				{
					result = functionBreakpoint;
				}
			}

			return result;
		}

		public bool HasBreakpointsInMethod(NuGenMethodDefinition methodDefinition)
		{
			bool result = false;
			int index = 0;

			while (!result && index < FunctionBreakpoints.Count)
			{
				NuGenFunctionBreakpointInformation functionBreakpoint = FunctionBreakpoints[index++];

				if (functionBreakpoint.MethodDefinition == methodDefinition)
				{
					result = true;
				}
			}

			return result;
		}

		public int FindExceptionInformationByIP(NuGenExceptionInformation exceptionInformation)
		{
			int result = 0;
			bool found = false;

			while (!found && result < Exceptions.Count)
			{
				NuGenExceptionInformation existingInformation = Exceptions[result];

				if (existingInformation.CompareTo(exceptionInformation) == 0 && existingInformation.ThrowingMethodToken == exceptionInformation.ThrowingMethodToken && existingInformation.IP == exceptionInformation.IP)
				{
					found = true;
				}
				else
				{
					result++;
				}
			}

			return (found ? result : -1);
		}

		public bool SuspendOnDebugEvent(SuspendableDebugEvent debugEvent)
		{
			return SuspendingDebugEvents.Contains(debugEvent);
		}

		public void RemoveInMemoryAssemblies()
		{
			if (Assemblies != null && Assemblies.Count > 0)
			{
				int index = 0;

				while (index < Assemblies.Count)
				{
					NuGenAssembly assembly = Assemblies[index];

					if (assembly.IsInMemory)
					{
						RemoveAssemblyRelatedBreakpoints(assembly);
						Assemblies.Remove(assembly);
					}
					else
					{
						index++;
					}
				}
			}
		}

		public void RemoveAssemblyRelatedBreakpoints(NuGenAssembly assembly)
		{
			int index = 0;

			while (index < FunctionBreakpoints.Count)
			{
				NuGenFunctionBreakpointInformation breakpoint = FunctionBreakpoints[index];

				if (breakpoint.MethodDefinition.BaseTypeDefinition.ModuleScope.Assembly == assembly)
				{
					breakpoint.Remove();
					FunctionBreakpoints.Remove(breakpoint);
					NuGenUIHandler.Instance.RemoveBreakpoint(breakpoint);
				}
				else
				{
					index++;
				}
			}
		}

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Name", name);
            info.AddValue("Assemblies", assemblies.Count);

            for(int i = 0; i<assemblies.Count; i++)
            {
                info.AddValue("Assembly" + i, assemblies[i].FullPath);
            }
        }
	}
}