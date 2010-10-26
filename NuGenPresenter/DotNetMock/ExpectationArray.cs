using System;

namespace DotNetMock
{
	/// <summary>
	/// Expectation class for ordered object arrays.
	/// </summary>
	// TODO: This is a perfect candidate for generics.  To avoid boxing, there should be other array types.
	public class ExpectationArray : AbstractStaticExpectation
	{
		/// <summary>
		/// Expected Array
		/// </summary>
		private object[] _expectedArray = new object[0];
		/// <summary>
		/// Actual Array
		/// </summary>
		private object[] _actualArray = new object[0];
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ExpectationArray( string name ) : base( name ) {}
		/// <summary>
		/// Verifies that the expected array and the actual array are equal
		/// </summary>
		public override void Verify()
		{
			if ( ( _expectedArray == null ) && ( _actualArray != null ) ) 
			{
				throw new AssertionException( "Expected Array was null, but Actual Array had length of " + _actualArray.Length ); 
			}
			if ( ( _actualArray == null ) && ( _expectedArray != null ) ) 
			{
				throw new AssertionException( "Actual Array was null, but Expected Array had length of " + _expectedArray.Length ); 
			}
			if ( ( _expectedArray != null ) && ( _actualArray != null ) ) 
			{
				if ( Strict ) 
				{
					if ( _actualArray.Length != _expectedArray.Length ) 
					{
						throw new AssertionException( "Array lengths do not equal.  Expected: " + _expectedArray.Length + "; Actual: " + _actualArray.Length );
					}
				} 
				else 
				{
					if ( _actualArray.Length < _expectedArray.Length ) 
					{
						throw new AssertionException( "Array lengths are not acceptable.  Expected: " + _expectedArray.Length + "; Actual: " + _actualArray.Length );
					}
				}
				for ( int i = 0; i < _expectedArray.Length; i++ ) 
				{
					if ( _expectedArray[i] != _actualArray[i] ) 
					{
						Assertion.AssertEquals( "Array values do not equal at index " + i + ". Expected: " + _expectedArray[i] + "; Actual: " + _actualArray[i], _expectedArray[i], _actualArray[i] );
					}
				}
			}
		}
		/// <summary>
		/// Clears the actual array
		/// </summary>
		public override void ClearActual()
		{
			_actualArray = new object[0];
		}
		/// <summary>
		/// Clears the expected array
		/// </summary>
		public override void ClearExpected()
		{
			_expectedArray = new object[0];
			HasExpectations = false;
		}
		/// <summary>
		/// Gets / Sets the expected array
		/// </summary>
		public object[] Expected
		{
			get { return _expectedArray; }
			set { 
				_expectedArray = value; 
				HasExpectations = true;
			}
		}
		/// <summary>
		/// Gets / Sets the actual array.
		/// </summary>
		public object[] Actual
		{
			get { return _actualArray; }
			set { 
				_actualArray = value; 
				if (ShouldCheckImmediate)
				{
					Verify();
				}	
			}
		}
	}
}
