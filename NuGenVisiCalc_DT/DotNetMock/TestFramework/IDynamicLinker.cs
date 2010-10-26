#region License
// Copyright (c) 2004 Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Reflection;

#endregion

namespace DotNetMock.TestFramework
{
	/// <summary>
	/// Interface for dynamically loading assemblies, accessing their
	/// types and creating instances of those types.
	/// </summary>
	public interface IDynamicLinker
	{
		/// <summary>
		/// Try to load an <see cref="Assembly"/> by simple name.
		/// </summary>
		/// <param name="name">simple name of desired
		/// <see cref="Assembly"/></param>
		/// <returns>desired <see cref="Assembly"/> or null if
		/// cannot be found</returns>
		Assembly LoadAssembly(string name);
		/// <summary>
		/// Try to load an <see cref="Assembly"/> by partial name.
		/// </summary>
		/// <param name="name">partial name of desired
		/// <see cref="Assembly"/></param>
		/// <returns>desired <see cref="Assembly"/> or null if
		/// cannot be found</returns>
		Assembly LoadAssemblyWithPartialName(string name);
		/// <summary>
		/// Get type by name from an assembly.
		/// </summary>
		/// <param name="typeName">full name of type</param>
		/// <param name="sourceAssembly">assembly to get it from</param>
		/// <returns></returns>
		Type GetType(string typeName, Assembly sourceAssembly);
		/// <summary>
		/// Create instance of type.
		/// </summary>
		/// <param name="type">desired <see cref="Type"/></param>
		/// <returns>new instance</returns>
		object CreateInstance(Type type);
	}
}
