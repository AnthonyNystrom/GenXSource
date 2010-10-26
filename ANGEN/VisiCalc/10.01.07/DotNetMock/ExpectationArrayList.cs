namespace DotNetMock
{
	using System;
	using System.Collections;
	/// <summary>
	/// Expectation ArrayList implementation.  Extends <c>AbstractIListExpectation</c>
	/// </summary>
	/// <remarks/>
	public class ExpectationArrayList : AbstractIListExpectation
	{
		private ArrayList _actualArrayList = new ArrayList();
		private ArrayList _expectedArrayList = new ArrayList();
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name"></param>
		public ExpectationArrayList(string name) : base(name) {
			_actualArrayList = new ArrayList();
			_expectedArrayList = new ArrayList();
		}
		/// <summary>
		/// Returns actual collection
		/// </summary>
		/// <returns>Actual Collection</returns>
		public override IList ActualCollection
		{
			get { return _actualArrayList; }
		}
		/// <summary>
		/// Returns expected collection
		/// </summary>
		/// <returns>Expected Collection</returns>
		public override IList ExpectedCollection
		{
			get { return _expectedArrayList; }
		}
		/// <summary>
		/// Checks an Immediate value
		/// </summary>
		/// <param name="actual">Value to check</param>
		protected override void CheckImmediateValues(object actual)
		{
			int size = _actualArrayList.Count;
			Assertion.Assert(_expectedArrayList.Count >= size);
			Assertion.AssertEquals(_expectedArrayList[size - 1], actual);
		}
	}
}
