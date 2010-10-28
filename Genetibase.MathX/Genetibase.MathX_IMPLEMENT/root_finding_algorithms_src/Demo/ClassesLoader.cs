using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace RootFinding
{
	internal class ClassesLoader
	{
		static internal void Load(string assemblyname,Type baseclass,out List<Type> classes) {
			try {
				// Load the assembly and get the defined types.
				Assembly assembly=Assembly.LoadFrom(assemblyname);
				Type[] types=assembly.GetTypes();
				classes=new List<Type>(types.Length);
				foreach(Type type in types) if(type.IsSubclassOf(baseclass)) classes.Add(type);
				classes.TrimExcess();
			} catch {
				classes=null;
			}
		}
	}
}
