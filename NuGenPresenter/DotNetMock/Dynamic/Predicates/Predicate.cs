namespace DotNetMock.Dynamic.Predicates
{
	/// <summary>
	/// <see cref="IPredicate"/> that accepts a reference to the a delegate instance that performs the evaluation.
	/// </summary>
	public class Predicate : AbstractPredicate
	{
		/// <summary>
		/// Delegate that will perform the evaluation of the input value.
		/// </summary>
		public delegate bool EvaluationMethod(object currentValue);
		
		private EvaluationMethod _evaluationMethod;
		/// <summary>
		/// Default Constructor
		/// </summary>
		/// <param name="m">Delegate Reference</param>
		public Predicate(EvaluationMethod m)
		{
			_evaluationMethod = m;	
		}
		/// <summary>
		/// Evaluates the input value using the delegate method supplied.
		/// </summary>
		/// <param name="currentValue">input value</param>
		/// <returns>The results of the delegation methods evaluation</returns>
		public override bool Eval(object currentValue)
		{
			return _evaluationMethod(currentValue);
		}
	}
}
