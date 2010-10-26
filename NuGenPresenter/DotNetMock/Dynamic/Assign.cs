#region License
// Copyright (c) 2004 Choy Rim. All rights reserved.
#endregion
#region Imports
#endregion

namespace DotNetMock.Dynamic
{
	/// <summary>
	/// An argument expectation that assigns a new value to ref/out
	/// parameters.
	/// </summary>
	public class Assign : AbstractArgumentMutator
	{
		private object _newValue;
		/// <summary>
		/// Default constructor.
		/// </summary>
		/// <param name="newValue">Value to assgin when Mutate calls are performed.</param>
		public Assign(object newValue)
		{
			_newValue = newValue;
		}
		/// <summary>
		/// Exchanges the input parameter with the value originally associate with this instance.
		/// </summary>
		/// <param name="parameterValue">argument to change</param>
		public override void Mutate(ref object parameterValue) 
		{
			parameterValue = _newValue;
		}
	}
}
