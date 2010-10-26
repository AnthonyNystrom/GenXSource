#region License
// Copyright (c) 2004, 2005 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using DotNetMock.Core;

#endregion

namespace DotNetMock
{
	/// <summary>
	/// Access point to underlying testing framework.
	/// </summary>
	/// <author>Griffin Caprio</author>
	/// <author>Choy Rim</author>
	/// <remarks>
	/// Assertion class used throughout the DotNetMock framework to
	/// encapsulate specific testing frameworks.
	/// NOTE: All exceptions will be caught, and rethrown as DotNetMock.AssertionException. 
	/// </remarks>
	public class Assertion
	{
		/// <summary>
		/// Private variable to hold concrete instance of ITestFramework interface.
		/// </summary>
		private static ITestFramework _testFramework =
			DotNetMock.TestFramework.Implementation.Instance;

		/// <summary>
		/// Asserts that two objects are equal.  If the objects are not equal the assertions fails with the
		/// given message.
		/// </summary>
		public static void AssertEquals( string message, object expectedObject, object actualObject )
		{
			try 
			{
				_testFramework.AssertEquals( message, expectedObject, actualObject );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Asserts that two objects are equal.  If the objects are not equal the assertions fails.
		/// </summary>
		public static void AssertEquals( object expectedObject, object actualObject )
		{
			try 
			{
			_testFramework.AssertEquals( expectedObject, actualObject );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Asserts that a given condition is true.  If the condition is not true, the assertion fails
		/// with the given message.
		/// </summary>
		public static void Assert( string message, bool assertion )
		{
			try 
			{
			_testFramework.Assert( message, assertion );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Asserts that a given condition is true.  If the condition is not true, the assertion fails.
		/// </summary>
		public static void Assert( bool assertion )
		{
			try 
			{
				_testFramework.Assert( assertion );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Asserts that an object is null.  If the object is not null, the assertion fails.
		/// </summary>
		public static void AssertNull( object assertion )
		{
			try 
			{
				_testFramework.AssertNull( assertion );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Asserts that an object is null.  If the object is not null, the assertion fails with the given 
		/// message.
		/// </summary>
		public static void AssertNull( string message, object assertion )
		{
			try 
			{
				_testFramework.AssertNull( message, assertion );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Asserts that an object is not null.  If the object is null, the assertion fails.
		/// </summary>
		public static void AssertNotNull( object assertion )
		{
			try 
			{
				_testFramework.AssertNotNull( assertion );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Asserts that an object is not null.  If the object is null, the assertion fails with the given
		/// message.
		/// </summary>
		public static void AssertNotNull( string message, object assertion )
		{
			try {
				_testFramework.AssertNotNull( message, assertion );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Fails a test with the specified message
		/// </summary>
		public static void Fail( string message )
		{
			try {
				_testFramework.Fail( message );
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
		/// <summary>
		/// Fails a test with no message
		/// </summary>
		public static void Fail()
		{
			try 
			{
				_testFramework.Fail();
			} 
			catch ( Exception ex ) 
			{
				throw new AssertionException( ex.Message );
			}
		}
	}
}
