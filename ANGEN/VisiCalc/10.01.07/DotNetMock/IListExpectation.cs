using System;
using System.Collections;

namespace DotNetMock
{
	/// <summary>
	/// IExpectationCollection interface. Interface for all collection based Expectations.  Implements <c>IExpectation</c> interface
	/// </summary>
	/// <remarks/>
	public interface IListExpectation : IExpectation
	{
		/// <summary>
		/// Adds object to actual collection
		/// </summary>
		/// <param name="actual">object to add</param>
		void AddActual(object actual);
		/// <summary>
		/// Adds an array of objects to actual collection
		/// </summary>
		/// <param name="actualMany">array of objects to add</param>
		void AddActualMany(object[] actualMany);
		/// <summary>
		/// Adds a Collection that implements the IEnumerable interface to actual collection
		/// </summary>
		/// <param name="actualMany">Enumerator full of objects to add to the actual collection</param>
		void AddActualMany(IEnumerable actualMany);
		/// <summary>
		/// Adds the elements of an object that implements IList to the actual collection
		/// </summary>
		/// <param name="actualMany">List of objects to add to the actual collection</param>
		void AddActualMany(IList actualMany);
		/// <summary>
		/// Adds object to expected collection
		/// </summary>
		/// <param name="expected">Object to add to the expected collection</param>
		void AddExpected(object expected);
		/// <summary>
		/// Adds an array of objects to expected collection
		/// </summary>
		/// <param name="expectedMany">Objects to add to the expected collection</param>
		void AddExpectedMany(object[] expectedMany);
		/// <summary>
		/// Adds a Collection that implements the IEnumerable interface to expected collection
		/// </summary>
		/// <param name="expectedMany">Enumerator full of objects to add to the expected collection</param>
		void AddExpectedMany(IEnumerable expectedMany);
		/// <summary>
		/// Adds the elements of an object that implements IList to the expected collection
		/// </summary>
		/// <param name="expectedMany">IList to add to the expected collection</param>
		void AddExpectedMany(IList expectedMany);
	}
}
