#region License
// Copyright (c) 2004 Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.IO;
using System.Reflection;

#endregion

namespace DotNetMock.TestFramework
{
	/// <summary>
	/// Summary description for SystemDynamicLinker.
	/// </summary>
	public class SystemDynamicLinker :
		IDynamicLinker
	{
	
		#region IDynamicLinker Members

		/// <summary>
		/// Try to load an <see cref="Assembly"/> by name.
		/// </summary>
		/// <param name="name">simple/partial name of desired
		/// <see cref="Assembly"/></param>
		/// <returns>desired <see cref="Assembly"/> or null if
		/// cannot be found</returns>
		public Assembly LoadAssembly(string name)
		{
			try
			{
				return Assembly.Load(name);
			}
			catch (FileNotFoundException) 
			{
				return null;
			}
		}
		/// <summary>
		/// Try to load an <see cref="Assembly"/> by partial name.
		/// </summary>
		/// <param name="name">simple/partial name of desired
		/// <see cref="Assembly"/></param>
		/// <returns>desired <see cref="Assembly"/> or null if
		/// cannot be found</returns>
		public Assembly LoadAssemblyWithPartialName(string name)
		{
			try
			{
#pragma warning disable 0618
				return Assembly.LoadWithPartialName(name);
#pragma warning restore 0618
			}
			catch (FileNotFoundException) 
			{
				return null;
			}
		}
		/// <summary>
		/// Get type by name from an assembly.
		/// </summary>
		/// <param name="typeName">full name of type</param>
		/// <param name="assembly">assembly to get it from</param>
		/// <returns></returns>
		public Type GetType(string typeName, Assembly assembly) 
		{
			return assembly.GetType(typeName);
		}
		/// <summary>
		/// Create instance of type.
		/// </summary>
		/// <param name="type">desired <see cref="Type"/></param>
		/// <returns>new instance</returns>
		public object CreateInstance(Type type)
		{
			return Activator.CreateInstance(type);
		}

		#endregion
	}
}
