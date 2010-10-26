#region License
// need license
#endregion
#region Imports

#endregion

namespace DotNetMock.Dynamic
{
	/// <summary>
	/// Interface for specifying an expected modification to an argument.
	/// </summary>
	/// <remarks>
	/// This interface is delegate-like but since we can't extend
	/// delegates, we need to go with this.
	/// </remarks>
	/// <author>Choy Rim</author>
	public interface IArgumentMutator
	{
		/// <summary>
		/// Mutate the argument value.
		/// </summary>
		/// <param name="argument">reference to argument that must be modified</param>
		void Mutate(ref object argument);
	}
}
