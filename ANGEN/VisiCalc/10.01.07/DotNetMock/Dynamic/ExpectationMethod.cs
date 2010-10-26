#region License

// Copyright (c) 2004 Griffin Caprio & Choy Rim. All rights reserved.

#endregion

#region Imports

using System;
using System.Reflection;

#endregion

namespace DotNetMock.Dynamic
{
    /// <summary>
    /// The kind of expectation on the method
    /// </summary>
    public enum ExpectationMethodType
    {
        /// <summary>
        /// Expect the method to be called
        /// </summary>
        Call,
        /// <summary>
        /// Expect the method to not be called
        /// </summary>
        NoCall
    };
    
    
	/// <summary>
	/// Expected method call used for building dynamic mocks
	/// </summary>
	public class ExpectationMethod : IMethodCallExpectation
	{
		/// <summary>
		/// Expected Method Name
		/// </summary>
		private string _expectedMethodName;
		/// <summary>
		/// Expectations on the arguments of the expected method call.
		/// </summary>
		private object[] _argumentExpectations;
		/// <summary>
		/// Expected return value for this method call if any.
		/// </summary>
		private object _expectedReturnValue;
		/// <summary>
		/// Exception to throw when the method is called
		/// </summary>
		private Exception _expectedException;
        /// <summary>
        /// The type of expectation, call or no call
        /// </summary>
        private ExpectationMethodType _expectationType;
	    
		/// <summary>
		/// Actual <see cref="MethodCall"/>.
		/// </summary>
		private MethodCall _actualMethodCall = null;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="methodName">Method name to expect</param>
		public ExpectationMethod( string methodName )
			: this( methodName, null, null, null )
		{
		}
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="methodName">Method name to expect</param>
		/// <param name="returnValue">return value when expectation is called</param>
		public ExpectationMethod( string methodName, object returnValue )
			: this( methodName, returnValue, null, null )
		{
		}
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="methodName">Method name to expect</param>
		/// <param name="returnValue">return value when expectation is called</param>
		/// <param name="argumentExpectations">Expectations on the arguments</param>
		public ExpectationMethod(
			string methodName,
			object returnValue,
			object[] argumentExpectations
			)
			: this( methodName, returnValue, argumentExpectations, null )
		{
		}
        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="methodName">Method name to expect</param>
        /// <param name="returnValue">return value when expectation is called</param>
        /// <param name="argumentExpectations">Expectations on the arguments</param>
        /// <param name="expectedException">Exception to throw when called.</param>
        public ExpectationMethod(
            string methodName,
            object returnValue,
            object[] argumentExpectations,
            Exception expectedException
            )
            : this(methodName, returnValue, argumentExpectations, expectedException, ExpectationMethodType.Call)
        {
            
        }		
	    /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="methodName">Method name to expect</param>
        /// <param name="returnValue">return value when expectation is called</param>
        /// <param name="argumentExpectations">Expectations on the arguments</param>
        /// <param name="expectedException">Exception to throw when called.</param>
        /// <param name="expectationType">Type of expectation</param>
        public ExpectationMethod(
            string methodName,
            object returnValue,
            object[] argumentExpectations,
            Exception expectedException,
            ExpectationMethodType expectationType
            )
        {
            _expectedMethodName = methodName;
            _argumentExpectations = argumentExpectations;
            _expectedReturnValue = returnValue;
            _expectedException = expectedException;
            _expectationType = expectationType;
        }
        /// <summary>
        /// Expected Method Name
        /// </summary>
        public string ExpectedMethodName
        {
            get
            {
                return _expectedMethodName;
            }
        }		
	    /// <summary>
        /// Expected Method Type
        /// </summary>
        public ExpectationMethodType ExpectationType
        {
            get
            {
                return _expectationType;
            }
        }
		/// <summary>
		/// Expected Return Value
		/// </summary>
		public object ReturnValue
		{
			get
			{
				return _expectedReturnValue;
			}
		}
		/// <summary>
		/// True if actual method call was set.
		/// <seealso cref="ActualMethodCall"/>
		/// </summary>
		public bool ActualMethodCallWasSet
		{
			get
			{
				return _actualMethodCall != null;
			}
		}
		/// <summary>
		/// <see cref="MethodInfo"/> of actual method called.
		/// </summary>
		public MethodInfo ActualMethod
		{
			get
			{
				return ActualMethodCall.Method;
			}
		}
		/// <summary>
		/// Name of actual method called.
		/// </summary>
		public string ActualMethodName
		{
			get
			{
				return ActualMethodCall.MethodName;
			}
		}
		/// <summary>
		/// Actual method call.
		/// </summary>
		public MethodCall ActualMethodCall
		{
			get
			{
				if ( ! ActualMethodCallWasSet )
				{
					throw new InvalidOperationException(
						"Cannot get property ActualMethodCall " +
							"before setting it."
						);
				}
				return _actualMethodCall;
			}
			set
			{
				if ( value == null )
				{
					throw new ArgumentNullException(
						"Cannot set ActualMethodCall property to null"
						);
				}
				if ( ActualMethodCallWasSet )
				{
					throw new InvalidOperationException(
						"Cannot set property ActualMethodCall " +
							"more than once."
						);
				}
				_actualMethodCall = value;
			}
		}
		//TODO: Refactor methods
		private string argsToString( object[] args )
		{
			if ( args != null && args.Length > 0 )
			{
				string[] argText = new string[args.Length];
				for ( int i = 0; i < args.Length; ++i )
				{
					object arg = args[ i ];
					if ( object.ReferenceEquals( arg, null ) )
					{
						argText[ i ] = "null";
					}
					else if ( arg is string )
					{
						argText[ i ] = "\"" + arg.ToString( ) + "\"";
					}
					else
					{
						argText[ i ] = arg.ToString( );
					}
				}
				return String.Join( ", ", argText );
			}
			return String.Empty;
		}
		/// <summary>
		/// Verifies this expectation, when the method is called
		/// </summary>
		public void Verify( )
		{
			if ( ! ActualMethodCallWasSet )
			{
				Assertion.Fail( String.Format(
					"{0}({1}) expected but never called.",
					ExpectedMethodName,
					argsToString( _argumentExpectations )
					) );
			}
            if (ExpectedMethodName != ActualMethodName && _expectationType == ExpectationMethodType.Call)
            {
                Assertion.Fail(String.Format(
                    "{0}({1}) expected, but {2} called.",
                    ExpectedMethodName, argsToString(_argumentExpectations),
                    ActualMethodCall
                    ));
            }
            if (ExpectedMethodName == ActualMethodName && _expectationType == ExpectationMethodType.NoCall)
            {
                Assertion.Fail(String.Format(
                    "{0}({1}) was not expected.",
                    ExpectedMethodName, argsToString(_argumentExpectations)
                    ));
            }

            if (_argumentExpectations == null)
			{
				return;
			}
			object[] actualArguments = ActualMethodCall.Arguments;
			// actual arguments must be equal to expectations
			if ( actualArguments.Length != _argumentExpectations.Length )
			{
				Assertion.Fail( String.Format(
					"Expected {0} arguments but received {1} " +
						"in method call {2}.",
					_argumentExpectations.Length,
					actualArguments.Length,
					ActualMethodCall
					) );
			}
			// assert that each passed in arg is validated by the appropriate predicate.					
			for ( int i = 0; i < _argumentExpectations.Length; i++ )
			{
				object argumentExpectation = _argumentExpectations[ i ];
				object actualArgument = actualArguments[ i ];
				// evaluate whether input expectations have been met
				IPredicate predicate =
					PredicateUtils.ConvertFrom( argumentExpectation );
				bool isPredicateSatisfied =
					predicate.Eval( actualArgument );
				if ( ! isPredicateSatisfied )
				{
					Assertion.Fail( String.Format(
						"Failed to satisfy '{0}' on argument[{1}] " +
							"of method call {2}",
						predicate,
						i,
						ActualMethodCall
						) );
				}
				// return output expectations if specified
				IArgumentMutator mutator =
					argumentExpectation as IArgumentMutator;
				if ( mutator != null )
				{
					mutator.Mutate( ref actualArguments[ i ] );
				}
			}
			if ( _expectedReturnValue!=null ) 
			{
				// handle return values that are ValueTypes and ensure they can be casted
				// in the IL that unboxes the return value.
				Type expectedReturnType = _expectedReturnValue.GetType();
				Type returnType = ActualMethod.ReturnType;
				if ( returnType!=typeof(void) && returnType.IsValueType ) 
				{
					if ( returnType!=expectedReturnType ) 
					{
						_expectedReturnValue = Convert.ChangeType(_expectedReturnValue, returnType);
					}
				}
			}
			// if exception setup to be thrown, throw it
			if ( _expectedException != null )
			{
				throw _expectedException;
			}
		}
		/// <summary>
		/// Check actual incoming method call and return expected outgoing response.
		/// <see cref="IMethodCallExpectation.CheckCallAndSendResponse"/>
		/// </summary>
		public object CheckCallAndSendResponse( MethodCall call )
		{
			_actualMethodCall = call;
			this.Verify( );
			return _expectedReturnValue;
		}
	}
}