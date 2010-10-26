#region License
// Copyright (c) 2004 Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Collections;
using DotNetMock.Core;

#endregion

namespace DotNetMock.TestFramework
{
	/// <summary>
	/// Responsible for obtaining an implementation
	/// of <see cref="ITestFramework"/> for DotNetMock to use.
	/// </summary>
	public class Implementation
	{
		private static ITestFramework _instance = null;
		private static object _lock = new object();
		/// <summary>
		/// Global instance of <see cref="ITestFramework"/>
		/// implementation used by the rest of DotNetMock.
		/// </summary>
		public static ITestFramework Instance 
		{
			get 
			{
				// the synchronization here may be overkill
				// but just in case ...
				lock ( _lock ) 
				{
					if ( _instance==null ) 
					{
						IDictionary env =
							Environment.GetEnvironmentVariables();
						IDynamicLinker linker = new SystemDynamicLinker();
						ImplementationFactory factory =
							new ImplementationFactory(env, linker);
						_instance = factory.NewImplementation();
					}
					return _instance;
				}
			}
		}
	}
}
