#region License
// need license
#endregion
#region Imports
using DotNetMock.Dynamic.Predicates;
#endregion

namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Abstract base class for all argument mutators.
	/// </summary>
	/// <author>Griffin Caprio</author>
	/// <author>Choy Rim</author>
	public abstract class AbstractArgumentMutator : IArgumentMutator, IPredicate
	{
		private static IPredicate DEFAULT_PREDICATE = new IsAnything();
		private IPredicate _predicate = DEFAULT_PREDICATE;
		/// <summary>
		/// Abstract method representing the exchanging of parameters
		/// </summary>
		/// <param name="argument">argument to change</param>
		public abstract void Mutate(ref object argument);
		/// <summary>
		/// Chains the current instance to another predicate to evaluate when
		/// exchanging parameters
		/// </summary>
		/// <param name="requirement">object requirement that can be converted
		/// to a <see cref="IPredicate"/></param>
		/// <returns>reference to this instance</returns>
		public object AndRequire(object requirement) 
		{
			_predicate = PredicateUtils.ConvertFrom(requirement);
			return this;
		}
		/// <summary>
		/// <seealso cref="IPredicate.Eval"/>
		/// </summary>
		public virtual bool Eval(object inputValue) 
		{
			return _predicate.Eval(inputValue);
		}
		/// <summary>
		/// <seealso cref="IPredicate.ExpressionAsText"/>
		/// </summary>
		public virtual string ExpressionAsText(string name) 
		{
			return _predicate.ExpressionAsText(name);
		}
	}
}
