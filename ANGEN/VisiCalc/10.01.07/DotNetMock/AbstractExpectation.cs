namespace DotNetMock
{
	/// <summary>
	/// Abstract class that implements the <c>IExpectation</c> interface
	/// </summary>
	/// <remarks/>
	public abstract class AbstractExpectation : IExpectation
	{
		/// <summary>
		/// Flag indicating if the expectation is strict.  Default is false
		/// </summary>
		private bool _isStrict = false;
		/// <summary>
		/// Flag indicating if the expectation has expectations.
		/// </summary>
		private bool _hasExpectations = false;
		/// <summary>
		/// Flag to indicate if the Mock should be verified immediately
		/// </summary>
		private bool _verifyImmediate = false;
		/// <summary>
		/// Flag to indicate if this object has been verified
		/// </summary>
		private bool _verified = false;
		/// <summary>
		/// Verifys current object and all MockObject fields within.
		/// </summary>
		public abstract void Verify();
		/// <summary>
		/// Gets/Sets if the object has expectations.
		/// </summary>
		public bool HasExpectations
		{
			get 
			{ 
				return _hasExpectations;
			}
			set 
			{
				_hasExpectations = value;
			}
		}

		/// <summary>
		/// Gets/Sets if the object should verify immediately.
		/// </summary>
		public bool VerifyImmediate
		{
			get
			{
				return _verifyImmediate;
			}
			set 
			{
				_verifyImmediate = value;
			}
		}
		/// <summary>
		/// Gets if the object should verify itself immediately
		/// </summary>
		public bool ShouldCheckImmediate
		{
			get 
			{
				return _verifyImmediate && _hasExpectations;
			}
		}
		/// <summary>
		/// Gets / Sets if the expectation should be strict or not.  This means that as long as expectations are met, any other
		/// object state will be ignored.  If this is true, only the set expectations will be allowed.
		/// </summary>
		public bool Strict 
		{
			get { return _isStrict; } 
			set { _isStrict = value; }
		}
		/// <summary>
		/// Returns true / false if this object has been verified or not.
		/// </summary>
		public bool IsVerified 
		{
			get { return _verified; }
		}
	}
}
