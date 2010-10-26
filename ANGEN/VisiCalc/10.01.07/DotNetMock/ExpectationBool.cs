using System;

namespace DotNetMock
{
	/// <summary>
	/// Expectation Bool implementation.  Extends <c>AbstractExpectation</c>
	/// </summary>
	/// <remarks/>
	public class ExpectationBool : AbstractStaticExpectation
	{
		private bool _actualBool = false;
		private bool _expectedBool = false;

		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="name">Name of this Expectation</param>
		public ExpectationBool(string name) : base(name) {}
		/// <summary>
		/// Clears actual value
		/// </summary>
		public override void ClearActual()
		{
			_actualBool = false;
		}
		/// <summary>
		/// Clear expected value
		/// </summary>
		public override void ClearExpected()
		{
			_expectedBool = false;
			HasExpectations = false;
		}
		/// <summary>
		/// Verifies object
		/// </summary>
		public override void Verify()
		{
			if (this.HasExpectations) 
			{
				Assertion.AssertEquals("Bool values not equal for " + this.name, _expectedBool, _actualBool);
			}
		}
		/// <summary>
		/// Gets/Sets actual boolean value
		/// </summary>
		public bool Actual
		{
			get 
			{ 
				return _actualBool; 
			}
			set 
			{ 
				_actualBool = value;
				if (ShouldCheckImmediate)
				{
					Verify();
				}	
			}
		}
		/// <summary>
		/// Gets/Sets expected boolean value
		/// </summary>
		public bool Expected
		{
			get { return _expectedBool; }
			set { 
				_expectedBool = value;
				this.HasExpectations = true;
			}
		}
	}
}
