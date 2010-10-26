#region License
// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.
#endregion
#region Imports
using System;
using System.Collections;
using System.Reflection;

using DotNetMock.Dynamic.Generate;
#endregion

namespace DotNetMock.Dynamic 
{
	/// <summary>
	/// Control class for creating mock objects during runtime.
	/// </summary>
	/// <author>Griffin Caprio</author>
	/// <author>Choy Rim</author>
	public class DynamicMock : IDynamicMock
	{
		private string _name;
		private object obj;
		private bool strict;
		private IDictionary expectations;
		private bool verified;
		private Type type;
		/// <summary>
		/// Holds the expectation values for an specified methods.
		/// </summary>
		protected IDictionary values;
		/// <summary>
		/// Default Constructor
		/// </summary>
		public DynamicMock()
		{
			verified = false;
			expectations = new Hashtable();
			values = new Hashtable();
		}
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="name">Name to assign to the generated mock</param>
		public DynamicMock(string name) : this()
		{
			_name = name;
	    }
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="type">Type to generate the mock for.</param>
		public DynamicMock( Type type ) : this()
		{
			string name = type.Name;
			if (name.StartsWith("I"))
			{
				name = name.Substring(1);
			}
			_name = "Mock" + name;

			this.type = type;
		}
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="type">Type to generate the mock for.</param>
		/// <param name="name">Name for the generated mock</param>
		public DynamicMock( Type type, string name ) : this( name )
		{
			this.type = type;
		}
		/// <summary>
		/// Returns the generated mock object for the given type.
		/// </summary>
		public virtual object Object
		{
			get 
			{
				if (obj == null)
				{
					generate();
				}
				return obj;
			}
		}
		/// <summary>
		/// Sets / Gets the Strict flag for the mock object.  If this flag is set, only specific operations 
		/// will be allowed on the generated mock object.
		/// </summary>
		public virtual bool Strict
		{
			get { return strict; }
			set { strict = value; }
		}
		/// <summary>
		/// Gets / Sets the name for the generated mock object
		/// </summary>
		public string MockName
		{
			get {return _name;}
			set { _name = value;}
		}
		/// <summary>
		/// Returns true / false if this object has been verified or not.
		/// </summary>
		public bool IsVerified 
		{
			get { return verified; }
		}
		/// <summary>
		/// Throws NotImplementedException.
		/// </summary>
		/// <param name="methodName">Method that isn't implemented.</param>
		public void NotImplemented(string methodName) 
		{
			throw new NotImplementedException(methodName + " not currently implemented");
		}
		/// <summary>
		/// Verifies this mock object.
		/// </summary>
		public virtual void Verify() 
		{
			foreach ( string key in expectations.Keys )
			{
				IList expectationsList = (IList)expectations[key];
				if ( expectationsList.Count > 0 )
				{
                    foreach (ExpectationMethod em in expectationsList)
                    {
                        if (em.ExpectationType != ExpectationMethodType.NoCall)
                        {
                            throw new AssertionException(expectationsList.Count + " more call(s) were expected for " + key);
                        }
                    }
				}
			}
		}
		/// <summary>
		/// Adds expected call to the method with the supplied arguments.
		/// </summary>
		/// <param name="methodName">Name of expected method to be called</param>
		/// <param name="args">Expected arguments</param>
		public virtual void Expect(string methodName, params object[] args) 
		{
			if ( args.Length>0 ) 
			{
				ExpectAndReturn(methodName, null, args);
			} 
			else 
			{
				// no argument checking
				addExpectation(new ExpectationMethod(methodName));
			}
		}
		/// <summary>
		/// Adds the expectation that the supplied method should not be called.
		/// </summary>
		/// <param name="methodName">Name of the method</param>
		public virtual void ExpectNoCall(string methodName)
		{
            addExpectation(new ExpectationMethod(methodName, null, null, new VerifyException(methodName + "() should never be called."), ExpectationMethodType.NoCall));
		}
		/// <summary>
		/// Addes a expected call with the supplied parameters, that returns an expected result.
		/// </summary>
		/// <param name="methodName">Name of expected method to be called</param>
		/// <param name="result">Results to return</param>
		/// <param name="args">Expected arguments</param>
		public virtual void ExpectAndReturn( string methodName, object result, params object[] args ) 
		{
			addExpectation( new ExpectationMethod( methodName, result, args ) );
		}
		/// <summary>
		/// Addes an expected call that results in the given exception being thrown.
		/// </summary>
		/// <param name="methodName">Method to call</param>
		/// <param name="e">Exception to throw</param>
		/// <param name="args">Expected arguments</param>
		public virtual void ExpectAndThrow( string methodName, Exception e, params object[] args) 
		{
			addExpectation( new ExpectationMethod( methodName, null, args, e ) );
		}
		/// <summary>
		/// Sets the return value for the supplied method
		/// </summary>
		/// <param name="methodName">Method to call</param>
		/// <param name="returnVal">Value to return</param>
		public virtual void SetValue(string methodName, object returnVal)
		{
			values[methodName] = returnVal;
		}
		/// <summary>
		/// Uses the given method to verify expectations for the method.
		/// </summary>
		/// <param name="mi">mathod called</param>
		/// <param name="args">arguments to the method</param>
		/// <returns>Return value, if any, from method call.</returns>
		public virtual object Call(MethodInfo mi, params object[] args) 
		{
			MethodCall methodCall = new MethodCall(mi, args);
			string methodName = methodCall.MethodName;
			if (values.Contains(methodName))
			{
				return values[methodName];
			}
			IMethodCallExpectation e = nextExpectation(methodCall);
			return e.CheckCallAndSendResponse(methodCall);
		}
		/// <summary>
		/// Adds a <see cref="IMethodCallExpectation"/> to the list of expectations of the mock object.
		/// </summary>
		/// <param name="e">Expectation to add</param>
		protected virtual void addExpectation(IMethodCallExpectation e)
		{
			IList list = (IList) expectations[e.ExpectedMethodName];
			if (list == null)
			{
				list = new ArrayList();
				expectations[e.ExpectedMethodName] = list;
			}
			list.Add(e);
		}
		/// <summary>
		/// Retrieve next expectation and remove from FIFO.
		/// </summary>
		/// <param name="methodCall">
		///  <see cref="MethodCall"/> to get expectation for
		/// </param>
		/// <returns>next <see cref="IMethodCallExpectation"/></returns>
		/// <remarks>
		/// This is a state mutating method. It removes the expectation from
		/// a list. Not a big deal since we don't ever recover from any
		/// exceptions.
		/// </remarks>
		protected virtual IMethodCallExpectation nextExpectation(MethodCall methodCall) 
		{
			string methodName = methodCall.MethodName;
			IList list = (IList) expectations[methodName];
			if (list == null) 
			{
				if ( strict ) 
				{
					throw new VerifyException(methodName + "() called too many times");
				}
				return null;
			}
			if (list.Count == 0) 
			{
				Assertion.Fail(methodName + "() called too many times");
			}
			IMethodCallExpectation e = (IMethodCallExpectation) list[0];
			list.RemoveAt(0);
			return e;
		}

		private void generate() 
		{
			ClassGenerator cg = new ClassGenerator();
			obj = cg.Generate(type, this);
		}
	}
}
