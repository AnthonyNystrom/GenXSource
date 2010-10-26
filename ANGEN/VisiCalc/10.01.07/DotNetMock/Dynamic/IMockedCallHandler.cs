#region License
// Copyright (c) 2004 Choy Rim. All rights reserved.
#endregion
#region Imports
using System.Reflection;
#endregion

namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Interface for handling mocked calls.
	/// </summary>
	/// <author>Choy Rim</author>
	public interface IMockedCallHandler
	{
		/// <summary>
		/// Mocked object calls this on any method call so that we can
		/// verify that the calls meet the expectations.
		/// </summary>
		object Call(MethodInfo mi, params object[] args);			
	}
}
