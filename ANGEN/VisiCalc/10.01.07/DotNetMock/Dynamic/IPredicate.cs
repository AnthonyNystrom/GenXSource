namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Interface for all predicates to implement
	/// </summary>
	public interface IPredicate
	{
		/// <summary>
		/// Evaluates the input value against the current value of the predicate.  
		/// A boolean result
		/// </summary>
		/// <param name="inputValue">Value to evaluate against</param>
		/// <returns>True/False if the current value equals the input value </returns>
		bool Eval(object inputValue);
		/// <summary>
		/// Text representation of what is evaluated by the
		/// <see cref="Eval"/> method.
		/// </summary>
		/// <param name="name">
		///  name of value/variable to use in the expression text
		/// </param>
		/// <returns>text representation of this predicate</returns>
		string ExpressionAsText(string name);
	}
}
