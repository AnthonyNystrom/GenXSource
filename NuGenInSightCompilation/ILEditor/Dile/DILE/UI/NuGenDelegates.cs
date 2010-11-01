using System;
using System.Collections.Generic;
using System.Text;

using Dile.Disassemble;

namespace Dile.UI
{
	public delegate void FoundItem(object sender, NuGenFoundItemEventArgs args);
	public delegate void NoArgumentsDelegate();
	public delegate void StringArrayDelegate(string[] array);
	public delegate void AssembliesLoadedDelegate(List<NuGenAssembly> assemblies, bool isProjectChanged);
}