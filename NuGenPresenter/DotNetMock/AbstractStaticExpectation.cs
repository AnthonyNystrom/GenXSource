namespace DotNetMock
{
	/// <summary>
	/// Abstract class that implements the represents a static Expectation within the Framework.
	/// </summary>
	/// <remarks/>
	public abstract class AbstractStaticExpectation : AbstractExpectation
	{
		/// <summary>
		/// Name of this Expectation
		/// </summary>
		protected string name = null;
		/// <summary>
		/// Default Constructor for AbstractExpectation
		/// </summary>
		protected AbstractStaticExpectation() : this( "AbstractStaticExpectation" )
		{
		}
		/// <summary>
		/// Constructor for AbstractExpectation that sets the name of the Expectation
		/// </summary>
		/// <param name="name">Name of this Expectation</param>
		protected AbstractStaticExpectation(string name) 
		{
			this.name = name;
		}
		/// <summary>
		/// Clears Actual field.
		/// </summary>
		public abstract void ClearActual();
		/// <summary>
		/// Clears Expected field.
		/// </summary>
		public abstract void ClearExpected();		
		/// <summary>
		/// Clears Expectations and sets HasExpectations to true.
		/// </summary>
		public virtual void ExpectNothing()
		{
			ClearExpected();
			this.HasExpectations = true;
		}
	}
}
