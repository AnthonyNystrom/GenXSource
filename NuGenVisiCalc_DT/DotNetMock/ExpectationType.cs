using System;

namespace DotNetMock
{
	/// <summary>
	/// Expectation Type implementation.  Extends <c>AbstractExpectation</c>
	/// </summary>
	/// <remarks/>
	public class ExpectationType : AbstractStaticExpectation
	{
		private Type _actualType = null;
		private Type _expectedType = null;
        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public ExpectationType(string name) : base(name) 
		{
			ClearActual();
		}

		/// <summary>
		/// Clears Actual value.
		/// </summary>
		public override void ClearActual() 
		{
			_actualType = null;
		}
		/// <summary>
		/// Clears Expected value.
		/// </summary>
		public override void ClearExpected()
		{
			_expectedType = null;
			HasExpectations = false;
		}
		/// <summary>
		/// Gets/Sets Actual values.
		/// </summary>
		public Type Actual 
		{
			get 
			{
				return _actualType;
			}
			set 
			{
				_actualType = (Type)value;
				if (ShouldCheckImmediate)
				{
					Verify();
				}		
			}
		}
		/// <summary>
		/// Gets/Sets Expected value.
		/// </summary>
		public Type Expected 
		{
			get 
			{	
				return _expectedType;
			}
			set 
			{
				_expectedType = (Type)value;
				this.HasExpectations = true;
			}
		}
		/// <summary>
		/// Verifies object.
		/// </summary>
		public override void Verify() 
		{
			if (this.HasExpectations) 
			{
				Assertion.AssertEquals("Types do not equal for object " + this.name, _expectedType, _actualType);
			}
		}
	}
}
