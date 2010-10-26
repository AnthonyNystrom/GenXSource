namespace DotNetMock 
{
	using System;
	/// <summary>
	/// Expectation Counter implementation.  Extends <c>AbstractExpectation</c>
	/// </summary>
	/// <remarks/>
	public class ExpectationCounter : AbstractStaticExpectation {
		private int _expectedCalls = 0;
    	private int _actualCalls = 0;

		/// <summary>
		/// Default Constructor. Sets the name of this Expectation and Strict to true
		/// </summary>
		/// <param name="name">Name of this Expectation</param>
		public ExpectationCounter(String name) : base(name) {
			Strict = true;
		}
		/// <summary>
		/// Sets Expected counter.
		/// </summary>
		public int Expected
		{
			set
			{
				_expectedCalls = value;
				this.HasExpectations = true;
			}
		}
		/// <summary>
		/// Resests Actual counter to 0.
		/// </summary>
		public override void ClearActual() {
			_actualCalls = 0;
		}
		/// <summary>
		/// Resets Expected counter to 0.
		/// </summary>
		public override void ClearExpected() {
			_expectedCalls = 0;
			HasExpectations = false;
		}
		/// <summary>
		/// Verifies object.
		/// </summary>
		public override void Verify() 
		{
			if ( Strict ) 
			{
				if (this.HasExpectations)
				{
					Assertion.AssertEquals("Did not receive the expected Count for object " + this.name, _expectedCalls, _actualCalls);
				}
			} 
			else 
			{
				if (this.HasExpectations)
				{
					Assertion.Assert("Did not receive the expected Count for object " + this.name, _actualCalls >= _expectedCalls );
				}
			}
		}
		/// <summary>
		/// Increments Actual counter.
		/// </summary>
		public void Inc()
		{
			_actualCalls++;
			if (ShouldCheckImmediate) {
				Assertion.Assert(
               this.name + " should not be called more than " + _expectedCalls + " times",
                _actualCalls <= _expectedCalls);
			}
		}
	}
}
	
