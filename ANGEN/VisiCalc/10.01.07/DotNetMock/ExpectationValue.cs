using System;

namespace DotNetMock
{
	/// <summary>
	/// Expectation Value implementation.  Extends <c>AbstractStaticExpectation</c>
	/// </summary>
	/// <remarks/>
	public class ExpectationValue : AbstractStaticExpectation
	{
		private Object _actualValue = null;
		private Object _expectedValue = null;

		/// <summary>
		/// Default Constructor for ExpectationValue.  Set the name for this Expectation
		/// </summary>
		/// <param name="name">Name of this Expectation</param>
		public ExpectationValue(string name) : base(name) 
		{
			ClearActual();
		}
		/// <summary>
		/// Clears Actual value.
		/// </summary>
		public override void ClearActual() 
		{
			_actualValue = null;
		}
		/// <summary>
		/// Clears Expected value.
		/// </summary>
		public override void ClearExpected()
		{
			_expectedValue = null;
			HasExpectations = false;
		}
		/// <summary>
		/// Gets/Sets Actual values.
		/// </summary>
		public Object Actual 
		{
			get 
			{
				return _actualValue;
			}
			set 
			{
				_actualValue = value;
				if (ShouldCheckImmediate)
				{
					Verify();
				}
			}
		}
		/// <summary>
		/// Gets/Sets Expected value.
		/// </summary>
		public Object Expected 
		{
			get 
			{
				return _expectedValue;
			}
			set 
			{
				_expectedValue = value;
				this.HasExpectations = true;
			}
		}
		/// <summary>
		/// Verifies object.
		/// </summary>
		public override void Verify() {
			if (this.HasExpectations) 
			{
				Assertion.AssertEquals("Object values do not equal for object " + this.name ,_expectedValue, _actualValue);
			}
		}
	}
}
