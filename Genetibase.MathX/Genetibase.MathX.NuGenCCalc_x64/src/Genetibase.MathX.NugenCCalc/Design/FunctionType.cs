using System;
using System.ComponentModel;

namespace Genetibase.MathX.NugenCCalc.Design
{
	/// <summary>
	/// Specifies the function type
	/// </summary>
	public enum FunctionType
	{
		/// <summary>
		/// Specify explicit type of function 
		/// </summary>
		[Description("Explicit")] Explicit,
		/// <summary>
		/// Specify implicit type of function 
		/// </summary>
		[Description("Implicit")] Implicit,
		/// <summary>
		/// Specify parametric type of function 
		/// </summary>
		[Description("Parametric")] Parametric
	}
}
