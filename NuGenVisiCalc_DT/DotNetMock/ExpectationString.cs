using System;

namespace DotNetMock
{
	/// <summary>
	/// Expectation String implementation. Extends <c>AbstractExpectaion</c>
	/// </summary>
	/// <remarks/>
	public class ExpectationString : AbstractStaticExpectation
	{
		private string _actualString = null;
		private string _expectedString = null;

		/// <summary>
		/// Default Constructor for ExpectationString.  Set the name for this Expectation
		/// </summary>
		/// <param name="name">Name of this Expectation</param>
		public ExpectationString(string name) : base(name)
		{
			ClearActual();
		}

		/// <summary>
		/// Gets/Sets Actual string.
		/// </summary>
		public string Actual 
		{
			get 
			{
				return _actualString;
			}
			set 
			{
				_actualString = value;
				if (ShouldCheckImmediate)
				{
					Verify();
				}	
			}
		}
		/// <summary>
		/// Gets/Sets Expected string.
		/// </summary>
		public string Expected
		{
			get 
			{
				return _expectedString;
			}
			set 
			{
				_expectedString = value;
				this.HasExpectations = true;
			}
		}
		/// <summary>
		/// Clears Actual string.
		/// </summary>
		public override void ClearActual()
		{
			_actualString = null;
		}
		/// <summary>
		/// Clears Expected string.
		/// </summary>
		public override void ClearExpected()
		{
			_expectedString = null;
			HasExpectations = false;
		}
		/// <summary>
		/// Verifies object
		/// </summary>
		public override void Verify()
		{
			if (this.HasExpectations) 
			{
				Assertion.AssertEquals("String values not equal for object " + this.name, _expectedString, _actualString);
			}
		}
		
	}
}
