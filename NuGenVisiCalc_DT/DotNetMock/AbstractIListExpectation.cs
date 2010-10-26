using System;
using System.Collections;

namespace DotNetMock
{
	/// <summary>
	/// Abstract class that implements the <c>IExpectationCollection</c> interface and extends <c>AbstractExpectation</c>
	/// </summary>
	/// <remarks/>
	public abstract class AbstractIListExpectation : AbstractStaticExpectation, IListExpectation
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		protected AbstractIListExpectation() : base() {}
		/// <summary>
		/// Constructor that takes in the name of the Expectation Collection
		/// </summary>
		/// <param name="name">Name for AbstractExpectationCollection</param>
		protected AbstractIListExpectation(string name) : base(name) {}
		/// <summary>
		/// Checks the given values immediatly
		/// </summary>
		/// <param name="actual">Values to check</param>
		protected abstract void CheckImmediateValues(object actual);
		/// <summary>
		/// Returns Actual Collection
		/// </summary>
		/// <returns>Actual Collection</returns>
		public abstract IList ActualCollection {get;}
		/// <summary>
		/// Returns Expected Collection
		/// </summary>
		/// <returns>Expected Collection</returns>
		public abstract IList ExpectedCollection {get;}
		/// <summary>
		/// Adds value to actual collection
		/// </summary>
		/// <param name="actual">Value to add</param>
		public void AddActual(object actual) 
		{
			this.ActualCollection.Add(actual);
			if (ShouldCheckImmediate)
			{
				CheckImmediateValues(actual);
			}
		}
		/// <summary>
		/// Adds array of values to the actual collection
		/// </summary>
		/// <param name="actualMany">Values to add</param>
		public void AddActualMany(object[] actualMany)
		{
			for (int i = 0; i < actualMany.Length; i++)
			{
				AddActual(actualMany[i]);
			}
		}
		/// <summary>
		/// Adds enumeration of values to the actual collection
		/// </summary>
		/// <param name="actualMany">Values to add</param>
		public void AddActualMany(IEnumerable actualMany)
		{
			IEnumerator enumerator = actualMany.GetEnumerator();
			while (enumerator.MoveNext())
			{
				AddActual(enumerator.Current);
			}
		}
		/// <summary>
		/// Adds a list values to the actual collection
		/// </summary>
		/// <param name="actualMany">Values to add</param>
		public void AddActualMany(IList actualMany)
		{
			for (int i = 0; i < actualMany.Count; i++)
			{
				AddActual(actualMany[i]);
			}
		}
		/// <summary>
		/// Adds value to the expected collection
		/// </summary>
		/// <param name="expected">Value to add</param>
		public void AddExpected(object expected) 
		{
			this.ExpectedCollection.Add(expected);
			this.HasExpectations = true;
		}
		/// <summary>
		/// Adds several values to the expected collection
		/// </summary>
		/// <param name="expectedMany">Values to add</param>
		public void AddExpectedMany(object[] expectedMany)
		{
			for (int i = 0; i < expectedMany.Length; i++)
			{
				AddExpected(expectedMany[i]);
			}
		}
		/// <summary>
		/// Adds several values to the expected collection
		/// </summary>
		/// <param name="expectedMany">Values to add</param>
		public void AddExpectedMany(IEnumerable expectedMany)
		{
			IEnumerator enumerator = expectedMany.GetEnumerator();
			while (enumerator.MoveNext())
			{
				AddExpected(enumerator.Current);
			}
		}
		/// <summary>
		/// Adds several values to the expected collection
		/// </summary>
		/// <param name="expectedMany">Values to add</param>
		public void AddExpectedMany(IList expectedMany)
		{
			for (int i = 0; i < expectedMany.Count; i++)
			{
				AddExpected(expectedMany[i]);
			}
		}
		/// <summary>
		/// Clears actual collection
		/// </summary>
		public override void ClearActual()
		{
			this.ActualCollection.Clear();
		}
		/// <summary>
		/// Clears expected collection
		/// </summary>
		public override void ClearExpected()
		{
			this.ExpectedCollection.Clear();
		}
		/// <summary>
		/// Verifies expectation collection
		/// </summary>
		public override void Verify()
		{
			if (this.HasExpectations) 
			{
				Assertion.AssertEquals("Expectation Count's do not equal.", this.ExpectedCollection.Count, this.ActualCollection.Count);
				for (int i = 0; i < this.ActualCollection.Count; i++)
				{
					Assertion.AssertEquals("Collection items not equal at index " + i, this.ExpectedCollection[i], this.ActualCollection[i]);
				}
			}
		}
	}
}
