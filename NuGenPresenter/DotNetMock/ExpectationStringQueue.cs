using System.Collections;
namespace DotNetMock
{
	/// <summary>
	/// Expectation String Queue implementation. Extends <c>AbstractStaticExpectaion</c>
	/// </summary>
	/// <author>Evhen Khasenevich</author>
	public class ExpectationStringQueue : AbstractStaticExpectation
	{
		private Queue _actualStringQueue = new Queue( );
		private Queue _expectedStringQueue = new Queue( );
		/// <summary>
		/// Default Constructor for ExpectationQueueString.  Set the name for this Expectation
		/// </summary>
		/// <param name="name">Name of this Expectation</param>
		public ExpectationStringQueue( string name ) : base( name )
		{
			ClearActual( );
		}
		/// <summary>
		/// Enqueue/Dequeue Actual strings.
		/// </summary>
		public string Actual
		{
			get
			{
				if ( _actualStringQueue.Count == 0 )
				{
					return null;
				}
				return ( string )_actualStringQueue.Dequeue( );
			}
			set
			{
				_actualStringQueue.Enqueue( value );
				if ( ShouldCheckImmediate )
				{
					Verify( );
				}
			}
		}
		/// <summary>
		/// Enqueue/Dequeue Expected strings.
		/// </summary>
		public string Expected
		{
			get
			{
				if ( _expectedStringQueue.Count == 0 )
				{
					return null;
				}
				if ( _expectedStringQueue.Count == 1 )
				{
					this.HasExpectations = false;
				}
				return ( string )_expectedStringQueue.Dequeue( );
			}
			set
			{
				_expectedStringQueue.Enqueue( value );
				this.HasExpectations = true;
			}
		}
		/// <summary>
		/// Clears Actual strings.
		/// </summary>
		public override void ClearActual( )
		{
			_actualStringQueue.Clear( );
		}
		/// <summary>
		/// Clears Expected strings.
		/// </summary>
		public override void ClearExpected( )
		{
			_expectedStringQueue.Clear( );
			HasExpectations = false;
		}
		/// <summary>
		/// Verifies object
		/// </summary>
		public override void Verify( )
		{
			if ( this.HasExpectations )
			{
				if ( this.ShouldCheckImmediate )
				{
					Assertion.AssertEquals( "String values in the queues are not equal for object " + this.name, this.Expected, this.Actual );
				}
				else
				{
					string expected;
					while ( ( expected = this.Expected ) != null )
					{
						if ( _actualStringQueue.Count > 0 )
						{
							Assertion.AssertEquals( "String values in the queues are not equal for object " + this.name, expected, this.Actual );
						}
						else
						{
							Assertion.Fail( string.Format( "{0} values left in the expected queue for object {1}: '{2}', ...", _expectedStringQueue.Count + 1, this.name, expected ) );
						}
					}
					if ( _actualStringQueue.Count > 0 )
					{
						Assertion.Fail( string.Format( "{0} values left in the actual queue for object {1}: '{2}', ...", _actualStringQueue.Count, this.name, this.Actual ) );
					}
				}
			}
		}
	}
}